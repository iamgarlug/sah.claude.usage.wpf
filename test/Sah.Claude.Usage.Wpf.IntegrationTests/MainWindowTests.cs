using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Tools;
using FlaUI.UIA3;

namespace Sah.Claude.Usage.Wpf.IntegrationTests;

public class MainWindowTests
{
    private const string AppPath = @"..\..\..\..\..\src\Sah.Claude.Usage.Wpf\bin\Debug\net10.0-windows\Sah.Claude.Usage.Wpf.exe";

    private Application _app = null!;
    private UIA3Automation _automation = null!;
    private Window _mainWindow = null!;

    [Before(Test)]
    public void Setup()
    {
        _automation = new UIA3Automation();
        _app = Application.Launch(AppPath);
        _mainWindow = _app.GetMainWindow(_automation)
            ?? throw new InvalidOperationException("Main window did not appear.");
        Thread.Sleep(500);
    }

    [After(Test)]
    public void Teardown()
    {
        _app?.Close();
        _automation?.Dispose();
    }

    [Test]
    public async Task MainWindow_HasCorrectTitle()
    {
        await Assert.That(_mainWindow.Title).IsEqualTo("Sah.Claude.Usage.Wpf");
    }

    [Test]
    public async Task MainWindow_HasRefreshButton()
    {
        var button = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("RefreshButton"));

        await Assert.That(button).IsNotNull();
    }

    [Test]
    public async Task MainWindow_HasSettingsButton()
    {
        var button = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("SettingsButton"));

        await Assert.That(button).IsNotNull();
    }

    [Test]
    public async Task SettingsButton_Click_OpensSettingsWindow()
    {
        _mainWindow
            .FindFirstDescendant(cf => cf.ByAutomationId("SettingsButton"))!
            .AsButton()
            .Invoke();

        var settingsWindow = Retry.WhileNull(
            () => _mainWindow
                .FindFirstChild(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Window))
                ?.AsWindow(),
            TimeSpan.FromSeconds(5)).Result;

        await Assert.That(settingsWindow).IsNotNull();

        settingsWindow?.Close();
    }

    [Test]
    public async Task SettingsWindow_HasThemeComboBox()
    {
        var settingsWindow = OpenSettingsWindow();

        var comboBox = settingsWindow.FindFirstDescendant(cf => cf.ByAutomationId("ThemeComboBox"));

        await Assert.That(comboBox).IsNotNull();

        settingsWindow.Close();
    }

    [Test]
    public async Task SettingsWindow_CancelButton_ClosesWindow()
    {
        var settingsWindow = OpenSettingsWindow();

        settingsWindow
            .FindFirstDescendant(cf => cf.ByAutomationId("CancelButton"))!
            .AsButton()
            .Invoke();

        var stillOpen = _mainWindow
            .FindFirstChild(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Window)) != null;

        await Assert.That(stillOpen).IsFalse();
    }

    private Window OpenSettingsWindow()
    {
        _mainWindow
            .FindFirstDescendant(cf => cf.ByAutomationId("SettingsButton"))!
            .AsButton()
            .Invoke();

        // Owned WPF windows appear as children of their owner in the UIA tree,
        // not as desktop-level windows, so search within _mainWindow.
        return Retry.WhileNull(
            () => _mainWindow
                .FindFirstChild(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Window))
                ?.AsWindow(),
            TimeSpan.FromSeconds(5)).Result
            ?? throw new InvalidOperationException("Settings window did not appear.");
    }
}

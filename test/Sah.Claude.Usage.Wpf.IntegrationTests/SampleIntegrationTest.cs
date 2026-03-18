using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;

namespace Sah.Claude.Usage.Wpf.IntegrationTests;

public class SampleIntegrationTest
{
    // Example: launch the WPF app and verify the main window appears.
    // Update AppPath to the built executable path before running.
    private const string AppPath = @"..\..\..\..\src\Sah.Claude.Usage.Wpf\bin\Debug\net10.0-windows\Sah.Claude.Usage.Wpf.exe";

    [Test]
    public async Task MainWindow_LaunchesSuccessfully()
    {
        using var automation = new UIA3Automation();
        using Application app = Application.Launch(AppPath);

        Window? mainWindow = app.GetMainWindow(automation);

        await Assert.That(mainWindow).IsNotNull();

        app.Close();
    }
}

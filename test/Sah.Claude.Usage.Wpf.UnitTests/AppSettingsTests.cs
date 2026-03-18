namespace Sah.Claude.Usage.Wpf.UnitTests;

public class AppSettingsTests
{
    private static readonly SemaphoreSlim _fileLock = new(1, 1);

    [Test]
    public async Task Theme_DefaultsTo_System()
    {
        var settings = new AppSettings();

        await Assert.That(settings.Theme).IsEqualTo(Theme.System);
    }

    [Test]
    [Arguments(Theme.System)]
    [Arguments(Theme.Light)]
    [Arguments(Theme.Dark)]
    public async Task SaveAndLoad_RoundTrips_Theme(Theme theme)
    {
        await _fileLock.WaitAsync();
        try
        {
            new AppSettings { Theme = theme }.Save();

            var loaded = AppSettings.Load();

            await Assert.That(loaded.Theme).IsEqualTo(theme);
        }
        finally
        {
            _fileLock.Release();
        }
    }
}

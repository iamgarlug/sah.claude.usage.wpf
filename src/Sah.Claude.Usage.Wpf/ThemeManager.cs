using Microsoft.Win32;
using System.Windows;

namespace Sah.Claude.Usage.Wpf;

public static class ThemeManager
{
    public static void Apply(Theme theme)
    {
        var resolved = theme == Theme.System ? GetSystemTheme() : theme;
        var uri = new Uri(
            $"pack://application:,,,/Themes/{resolved}.xaml",
            UriKind.Absolute);

        var resources = Application.Current.Resources.MergedDictionaries;
        var existing = resources.FirstOrDefault(
            d => d.Source?.OriginalString.Contains("/Themes/") == true);

        var dict = new ResourceDictionary { Source = uri };

        if (existing is not null)
            resources.Remove(existing);

        resources.Add(dict);
    }

    private static Theme GetSystemTheme()
    {
        using var key = Registry.CurrentUser.OpenSubKey(
            @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
        return key?.GetValue("AppsUseLightTheme") is int v && v == 0
            ? Theme.Dark
            : Theme.Light;
    }
}

using System.Windows;
using System.Windows.Controls;

namespace Sah.Claude.Usage.Wpf;

public partial class SettingsWindow : Window
{
    private readonly AppSettings _settings;
    private readonly Theme _originalTheme;

    public SettingsWindow(AppSettings settings)
    {
        InitializeComponent();
        _settings = settings;
        _originalTheme = settings.Theme;
        ThemeComboBox.ItemsSource = Enum.GetValues<Theme>();
        ThemeComboBox.SelectedItem = settings.Theme;
    }

    private void OnThemeSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ThemeComboBox.SelectedItem is Theme selected)
            ThemeManager.Apply(selected);
    }

    private void OnCancelClick(object sender, RoutedEventArgs e)
    {
        ThemeManager.Apply(_originalTheme);
        Close();
    }

    private void OnSaveClick(object sender, RoutedEventArgs e)
    {
        _settings.Theme = (Theme)ThemeComboBox.SelectedItem;
        _settings.Save();
        Close();
    }
}

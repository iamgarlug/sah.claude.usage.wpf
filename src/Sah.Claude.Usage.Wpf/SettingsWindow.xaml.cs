using System.Windows;

namespace Sah.Claude.Usage.Wpf;

public partial class SettingsWindow : Window
{
    public SettingsWindow()
    {
        InitializeComponent();
    }

    private void OnCancelClick(object sender, RoutedEventArgs e) => Close();

    private void OnSaveClick(object sender, RoutedEventArgs e) => Close();
}

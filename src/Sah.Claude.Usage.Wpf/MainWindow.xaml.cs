using System.Windows;

namespace Sah.Claude.Usage.Wpf;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void OnRefreshClick(object sender, RoutedEventArgs e)
    {
    }

    private void OnSettingsClick(object sender, RoutedEventArgs e)
    {
        var settings = new SettingsWindow(AppSettings.Load()) { Owner = this };
        settings.ShowDialog();
    }
}

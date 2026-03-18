using System.Windows;

namespace Sah.Claude.Usage.Wpf;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        ThemeManager.Apply(AppSettings.Load().Theme);
        new MainWindow().Show();
    }
}

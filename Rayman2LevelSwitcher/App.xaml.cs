using System.Windows;

namespace Rayman2LevelSwitcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            MainWindow = new MainWindow(new MainViewModel(new Rayman2GameManager()));
            MainWindow.Show();
        }
    }
}
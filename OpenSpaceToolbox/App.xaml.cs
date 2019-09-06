using System;
using System.Windows;

namespace OpenSpaceToolbox
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            GameChooserViewModel gameChooserViewModel = new GameChooserViewModel();
            Window gameChooser = new GameChooser(gameChooserViewModel)
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            gameChooser.ShowDialog();

            if (gameChooserViewModel.SelectedGame == null) return;

            var game = Activator.CreateInstance(gameChooserViewModel.SelectedGame.Class);
            if (!(game is GenericGameManager gameManager)) return;

            MainWindow = new MainWindow(new MainViewModel(gameManager));
            MainWindow.Show();
        }
    }
}
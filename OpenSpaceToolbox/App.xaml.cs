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
            var game = ChooseGame();

            if (game != null)
            {
                MainWindow = new MainWindow(new MainViewModel(game));
                MainWindow.Show();
            }
            else Shutdown();
        }

        private GenericGameManager ChooseGame()
        {
            GameChooserViewModel gameChooserViewModel = new GameChooserViewModel();
            GameChooser gameChooser = new GameChooser(gameChooserViewModel)
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            gameChooser.ShowDialog();

            if (gameChooserViewModel.SelectedGame == null)
                return null;

            var game = Activator.CreateInstance(gameChooserViewModel.SelectedGame.Class);

            if (game is GenericGameManager gameManager)
                return gameManager;

            return null;
        }
    }
}
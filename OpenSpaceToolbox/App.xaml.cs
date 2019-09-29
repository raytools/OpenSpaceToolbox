using System;
using System.IO;
using System.Windows;
using OpenSpaceCore.DataModels;
using OpenSpaceCore.GameManager;

namespace OpenSpaceToolbox
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            Directory.CreateDirectory(ProgramPaths.Games);
            Directory.CreateDirectory(ProgramPaths.Bookmarks);

            GameChooserViewModel gameChooserViewModel = new GameChooserViewModel();
            Window gameChooser = new GameChooser(gameChooserViewModel)
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            gameChooser.ShowDialog();

            if (gameChooserViewModel.SelectedGame == null) return;

            MainWindow = new MainWindow(new MainViewModel(gameChooserViewModel.SelectedGame.Class));
            MainWindow.Show();
        }
    }
}
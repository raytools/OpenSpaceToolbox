using System.ComponentModel;
using System.Windows;

namespace OpenSpaceToolbox
{
    /// <summary>
    /// Interaction logic for GameChooser.xaml
    /// </summary>
    public partial class GameChooser : Window
    {
        public GameChooser(GameChooserViewModel viewModel)
        {
            InitializeComponent();
            DataContext = ViewModel = viewModel;
        }

        private GameChooserViewModel ViewModel { get; }

        private void ProcessItem(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectGame();
            Close();
        }
    }
}

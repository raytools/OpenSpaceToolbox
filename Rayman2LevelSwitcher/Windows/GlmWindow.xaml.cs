using System.Windows;

namespace Rayman2LevelSwitcher
{
    /// <summary>
    /// Interaction logic for GlmWindow.xaml
    /// </summary>
    public partial class GlmWindow : Window
    {
        public GlmWindow(GlmWindowViewModel viewModel)
        {
            InitializeComponent();

            DataContext = ViewModel = viewModel;
        }

        private GlmWindowViewModel ViewModel { get; }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ViewModel.LivePreviewEnabled = false;
        }
    }
}

using System.Windows;

namespace Rayman2LevelSwitcher
{
    /// <summary>
    /// Interaction logic for GlmWindow.xaml
    /// </summary>
    public partial class GlmWindow : Window
    {
        public GlmWindow()
        {
            InitializeComponent();

            DataContext = GlmWindowVM = new GlmWindowViewModel();
        }

        private GlmWindowViewModel GlmWindowVM { get; }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GlmWindowVM.LivePreviewEnabled = false;
        }
    }
}

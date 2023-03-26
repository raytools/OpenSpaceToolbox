using System.Threading.Tasks;
using System.Windows;

namespace OpenSpaceToolbox
{
    /// <summary>
    /// Interaction logic for GlmWindow.xaml
    /// </summary>
    public partial class SpeedMonitorWindow : Window
    {
        public SpeedMonitorWindow(SpeedMonitorWindowViewModel viewModel)
        {
            InitializeComponent();

            DataContext = ViewModel = viewModel;
            //Task.Run(viewModel.UpdateSpeed);
        }

        private SpeedMonitorWindowViewModel ViewModel { get; }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ViewModel.Active = false;
        }
    }
}

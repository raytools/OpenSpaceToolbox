using System.Windows;

namespace OpenSpaceToolbox
{
    /// <summary>
    /// Interaction logic for ConsoleWindow.xaml
    /// </summary>
    public partial class ConsoleWindow : Window
    {
        #region Constructor

        public ConsoleWindow(ConsoleViewModel consoleVm)
        {
            InitializeComponent();

            DataContext = ViewModel = consoleVm;
        }

        #endregion

        #region Private Properties

        private  ConsoleViewModel ViewModel { get; }

        #endregion
    }
}

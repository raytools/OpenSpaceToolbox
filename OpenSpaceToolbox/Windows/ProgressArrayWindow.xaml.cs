using System;
using System.Windows;

namespace OpenSpaceToolbox
{
    /// <summary>
    /// Interaction logic for GlmWindow.xaml
    /// </summary>
    public partial class ProgressArrayWindow : Window
    {
        public ProgressArrayWindow(ProgressArrayWindowViewModel viewModel)
        {
            InitializeComponent();

            DataContext = ViewModel = viewModel;
        }

        private ProgressArrayWindowViewModel ViewModel { get; }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

      private void button_Click(object sender, RoutedEventArgs e)
      {

      }

      private void RefreshFlagList(object sender, EventArgs e)
      {
         ViewModel.RefreshFlagList();
      }
    }
}

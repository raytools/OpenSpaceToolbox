using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Rayman2LevelSwitcher
{
    /*
     
     --- TODO List ---
     
     - Save the Rayman 2 Process & ID to avoid searching for it every time. An event can be hooked into when it exits. Close the app then?
     - The hotkey system can be made more modular
     - Make sure main window dispose actually runs
     - Add try/catch block when memory is being read/written to as it might fail
     - Have bookmarks change the level too
    
    */

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindow(MainViewModel mainVm)
        {
            InitializeComponent();
            DataContext = ViewModel = mainVm;
        }

        #endregion

        #region Event Handlers

        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            ViewModel.Dispose();
            ViewModel.BookmarksVm.SaveBookmarks();
        }

        #endregion

        #region Private Properties

        private MainViewModel ViewModel { get; }

        #endregion
    }
}
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

// ReSharper disable StringLiteralTypo

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
    public partial class MainWindow : Window, IDisposable
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindow()
        {
            // Set up UI
            InitializeComponent();

            // Create view models
            AppVM = new AppViewModel();
            DataContext = GameManagerVM = new GameManagerViewModel(AppVM);
            BookmarkPanel.DataContext = BookmarksVM = new BookmarksViewModel(AppVM);

            // Setup keyboard hook
            GlobalKeyboardHook = new GlobalKeyboardHook();
            GlobalKeyboardHook.KeyboardPressed += OnKeyPressed;
        }

        #endregion

        #region Event Handlers

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Dispose();
            BookmarksVM.SaveBookmarks();
        }

        private void Listbox_bookmarklist_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BookmarksVM.SelectedBookmark?.LoadBookmark();
        }

        public void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            var manager = new Rayman2Manager();

            if (!AppVM.HotkeysEnabled || Application.Current.Windows.Count > 1 || manager.IsRayman2Paused() || !manager.IsRayman2Focused() && !Application.Current.MainWindow.IsActive)
            {
                e.Handled = false;
                return;
            }

            // Only handle key down
            if (e.KeyboardState != GlobalKeyboardHook.KeyboardState.KeyDown)
                return;

            // O to load position
            if (e.KeyboardData.VirtualCode == 0x4F)
                GameManagerVM.LoadSavedPosition();

            // P to save position
            else if (e.KeyboardData.VirtualCode == 0x50)
                GameManagerVM.SavePosition();

            // K for previous level
            else if(e.KeyboardData.VirtualCode == 0x4B)
                GameManagerVM.LoadOffsetLevel(-1);

            // L for next level
            else if(e.KeyboardData.VirtualCode == 0x4C)
                GameManagerVM.LoadOffsetLevel(1);

            // R for random level
            else if(e.KeyboardData.VirtualCode == 0x52)
                GameManagerVM.LoadRandomLevel();

            // B to add bookmark
            else if(e.KeyboardData.VirtualCode == 0x42)
                BookmarksVM.AddBookmark();
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// The global keyboard hook
        /// </summary>
        private GlobalKeyboardHook GlobalKeyboardHook { get; }

        /// <summary>
        /// The main window view model
        /// </summary>
        private GameManagerViewModel GameManagerVM { get; }

        /// <summary>
        /// The bookmarks view model
        /// </summary>
        private BookmarksViewModel BookmarksVM { get; }

        /// <summary>
        /// The app view model
        /// </summary>
        private AppViewModel AppVM { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            GlobalKeyboardHook?.Dispose();
        }

        #endregion
    }
}
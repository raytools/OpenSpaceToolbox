using System;
using System.Windows;

namespace Rayman2LevelSwitcher
{
    public class MainViewModel : BaseViewModel, IDisposable
    {
        #region Constructor

        public MainViewModel()
        {
            // Instantiate view models
            AppVm = new AppViewModel();
            GameManagerVm = new GameManagerViewModel(AppVm);
            BookmarksVm = new BookmarksViewModel(AppVm);

            // Setup keyboard hook
            GlobalKeyboardHook = new GlobalKeyboardHook();
            GlobalKeyboardHook.KeyboardPressed += OnKeyPressed;
        }

        #endregion

        #region Event Handlers

        public void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            var manager = new Rayman2Manager();

            if (!AppVm.HotkeysEnabled || manager.IsRayman2Paused() || !manager.IsRayman2Focused() && !Application.Current.MainWindow.IsActive)
            {
                e.Handled = false;
                return;
            }

            // Only handle key down
            if (e.KeyboardState != GlobalKeyboardHook.KeyboardState.KeyDown)
            {
                e.Handled = false;
                return;
            }

            switch (e.KeyboardData.VirtualCode)
            {
                // O to load position
                case 0x4F:
                    GameManagerVm.LoadSavedPosition();
                    break;
                // P to save position
                case 0x50:
                    GameManagerVm.SavePosition();
                    break;
                // K for previous level
                case 0x4B:
                    GameManagerVm.LoadOffsetLevel(-1);
                    break;
                // L for next level
                case 0x4C:
                    GameManagerVm.LoadOffsetLevel(1);
                    break;
                // R for reload level
                case 0x52:
                    manager.ReloadLevel();
                    break;
                // B to add bookmark
                case 0x42:
                    BookmarksVm.AddBookmark();
                    break;
                default:
                    e.Handled = false;
                    return;
            }

            e.Handled = true;
        }

        #endregion

        #region Private Properties

        private GlobalKeyboardHook GlobalKeyboardHook { get; }

        #endregion

        #region Public Properties

        public AppViewModel AppVm { get; }

        public GameManagerViewModel GameManagerVm { get; }

        public BookmarksViewModel BookmarksVm { get; }

        #endregion

        #region Public Methods

        public void Dispose()
        {
            GlobalKeyboardHook?.Dispose();
        }

        #endregion
    }
}

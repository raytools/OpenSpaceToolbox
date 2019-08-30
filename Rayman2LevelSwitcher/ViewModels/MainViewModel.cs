using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            
            // Create commands
            ToggleMinimizedUiCommand = new RelayCommand(ToggleMinimizedUi);

            // Set full UI as default
            CurrentView = new GameManagerFullView();

            // Setup keyboard hook
            GlobalKeyboardHook = new GlobalKeyboardHook();
            GlobalKeyboardHook.KeyboardPressed += OnKeyPressed;

            WindowProperties = new WindowProperties {Width = 640, Height = 480, ResizeMode = ResizeMode.CanResize};
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

        #region Commands

        public ICommand ToggleMinimizedUiCommand { get; }

        #endregion

        #region Private Properties

        private GlobalKeyboardHook GlobalKeyboardHook { get; }

        #endregion

        #region Public Properties

        public AppViewModel AppVm { get; }

        public GameManagerViewModel GameManagerVm { get; }

        public BookmarksViewModel BookmarksVm { get; }

        public UserControl CurrentView { get; set; }

        public WindowProperties WindowProperties { get; }

        #endregion

        #region Public Methods

        public void ToggleMinimizedUi()
        {
            if (CurrentView is GameManagerFullView)
            {
                CurrentView = new GameManagerMinimizedView();
                WindowProperties.SetSize(280, 520);
                WindowProperties.ResizeMode = ResizeMode.NoResize;
            }
            else
            {
                CurrentView = new GameManagerFullView();
                WindowProperties.SetSize(640, 480);
                WindowProperties.ResizeMode = ResizeMode.CanResize;
            }

        }

        public void Dispose()
        {
            GlobalKeyboardHook?.Dispose();
        }

        #endregion
    }
}

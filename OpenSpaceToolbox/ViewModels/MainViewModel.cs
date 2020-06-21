using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OpenSpaceToolbox
{
    public class MainViewModel : BaseViewModel, IDisposable
    {
        #region Constructor

        public MainViewModel(GenericGameManager gameManager)
        {
            GameManager = gameManager;
            // Instantiate view models
            GameManagerVm = new GameManagerViewModel(GameManager);
            BookmarksVm = new BookmarksViewModel(GameManager);
            
            // Create commands
            ToggleMinimizedUiCommand = new RelayCommand(ToggleMinimizedUi);

            // Set full UI as default
            WindowProperties = new WindowProperties {ResizeMode = ResizeMode.CanResize};
            SetFullView();

            // Setup keyboard hook
            GlobalKeyboardHook = new GlobalKeyboardHook();
            GlobalKeyboardHook.KeyboardPressed += OnKeyPressed;

            HotkeysToggleTooltip =
                "Enables hotkeys:\nR - reload level\nK - previous level\nL - next level\nP - save position\nO - load position\nB - add bookmark";
        }

        #endregion

        #region Event Handlers

        public void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            if (!HotkeysEnabled || GameManager.IsGamePaused() || !GameManager.IsGameFocused() && !Application.Current.MainWindow.IsActive)
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
                    GameManager.ReloadLevel();
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

        public GenericGameManager GameManager { get; }

        public GameManagerViewModel GameManagerVm { get; }

        public BookmarksViewModel BookmarksVm { get; }

        public UserControl CurrentView { get; set; }

        public WindowProperties WindowProperties { get; }
        
        public bool HotkeysEnabled { get; set; }

        public string HotkeysToggleTooltip { get; }

        #endregion

        #region Private Methods

        private void SetMinimizedView()
        {
            CurrentView = new GameManagerMinimizedView();
            WindowProperties.SetMinSize(250, 400);
            WindowProperties.SetSize(280, 520);
        }

        private void SetFullView()
        {
            CurrentView = new GameManagerFullView();
            WindowProperties.SetMinSize(400, 300);
            WindowProperties.SetSize(640, 480);
        }

        #endregion

        #region Public Methods

        public void ToggleMinimizedUi()
        {
            if (CurrentView is GameManagerFullView)
            {
                SetMinimizedView();
            }
            else
            {
                SetFullView();
            }

        }

        public void Dispose()
        {
            GlobalKeyboardHook?.Dispose();
        }

        #endregion
    }
}

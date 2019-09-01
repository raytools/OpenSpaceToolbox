using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Rayman2LevelSwitcher
{
    /// <summary>
    /// View model for the game manager
    /// </summary>
    public class GameManagerViewModel : BaseViewModel
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="app">The app view model</param>
        public GameManagerViewModel(AppViewModel app)
        {
            // Create the commands
            LoadSavedPositionCommand = new RelayCommand(LoadSavedPosition);
            SavePositionCommand = new RelayCommand(SavePosition);
            ActivateVoidCommand = new RelayCommand(() => new Rayman2Manager().ActivateVoid());
            ActivateNoHealthCommand = new RelayCommand(() => new Rayman2Manager().ActivateNoHealth());
            ReloadLevelCommand = new RelayCommand(() => new Rayman2Manager().ReloadLevel());
            LoadRandomLevelCommand = new RelayCommand(LoadRandomLevel);
            ShowGlmWindowCommand = new RelayCommand(ShowGlmWindow);

            // Create the properties
            Random = new Random();
            App = app;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The saved X position
        /// </summary>
        public float SavedXPosition { get; set; }

        /// <summary>
        /// The saved Y position
        /// </summary>
        public float SavedYPosition { get; set; }

        /// <summary>
        /// The saved Z position
        /// </summary>
        public float SavedZPosition { get; set; }

        /// <summary>
        /// The random generator for this view model
        /// </summary>
        public Random Random { get; }

        /// <summary>
        /// The app view model
        /// </summary>
        public AppViewModel App { get; }

        #endregion

        #region Commands

        public ICommand LoadRandomLevelCommand { get; }

        public ICommand ReloadLevelCommand { get; }

        public ICommand ActivateNoHealthCommand { get; }

        public ICommand ActivateVoidCommand { get; }

        public ICommand LoadSavedPositionCommand { get; }

        public ICommand SavePositionCommand { get; }

        public ICommand ShowGlmWindowCommand { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads a random playable level
        /// </summary>
        public void LoadRandomLevel()
        {
            var lvls = App.Levels.Where(x => x.Type == Rayman2LevelType.Level).ToList();
            new Rayman2Manager().ChangeLevel(lvls[Random.Next(lvls.Count - 1)].FileName);
        }

        /// <summary>
        /// Loads the level from the offset of the current level
        /// </summary>
        /// <param name="offset">The offset</param>
        public void LoadOffsetLevel(int offset)
        {
            var manager = new Rayman2Manager();

            int processHandle = manager.GetProcessHandle();

            if (processHandle < 0)
                return;

            string levelName = manager.GetCurrentLevelName(processHandle);

            var lvls = App.Levels.ToList();

            int currentIndex = lvls.FindIndex(x => string.Equals(x.FileName, levelName, StringComparison.CurrentCultureIgnoreCase));

            if (currentIndex < 0)
                return;

            int newIndex = currentIndex + offset;

            if (newIndex < 0)
                newIndex = lvls.Count - 1;

            if (newIndex >= lvls.Count)
                newIndex = 0;

            string levelToLoad = lvls[newIndex].FileName;

            manager.ChangeLevel(levelToLoad);
        }

        /// <summary>
        /// Load the saved position
        /// </summary>
        public void LoadSavedPosition()
        {
            new Rayman2Manager().PlayerCoordinates = (SavedXPosition, SavedYPosition, SavedZPosition);
        }

        /// <summary>
        /// Save the position
        /// </summary>
        public void SavePosition()
        {
            var coords = new Rayman2Manager().PlayerCoordinates;
            SavedXPosition = coords.Item1;
            SavedYPosition = coords.Item2;
            SavedZPosition = coords.Item3;
        }

        /// <summary>
        /// Show the GLM Monitor 2000 window
        /// </summary>
        public void ShowGlmWindow()
        {
            GlmWindow glmWindow = new GlmWindow
            {
                Owner = Application.Current.MainWindow
            };
            glmWindow.Show();
        }

        #endregion
    }
}
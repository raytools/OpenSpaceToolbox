using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace OpenSpaceToolbox
{
    /// <summary>
    /// View model for the game manager
    /// </summary>
    public class GameManagerViewModel : BaseViewModel
    {
        #region Constructor

        public GameManagerViewModel(GenericGameManager gameManager)
        {
            // Create the properties
            Random = new Random();
            GameManager = gameManager;

            // Create the commands
            LoadSavedPositionCommand = new RelayCommand(LoadSavedPosition);
            SavePositionCommand = new RelayCommand(SavePosition);
            ReloadLevelCommand = new RelayCommand(() => GameManager.ReloadLevel());
            LoadRandomLevelCommand = new RelayCommand(LoadRandomLevel);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The random generator for this view model
        /// </summary>
        public Random Random { get; }

        /// <summary>
        /// The game manager
        /// </summary>
        public GenericGameManager GameManager { get; }

        #endregion

        #region Commands

        public ICommand LoadRandomLevelCommand { get; }

        public ICommand ReloadLevelCommand { get; }

        public ICommand LoadSavedPositionCommand { get; }

        public ICommand SavePositionCommand { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads a random playable level
        /// </summary>
        public void LoadRandomLevel()
        {
            var lvls = GameManager.Levels.Where(x => x.Type == LevelType.Level).ToList();
            GameManager.CurrentLevel = lvls[Random.Next(lvls.Count - 1)].FileName;
        }

        /// <summary>
        /// Loads the level from the offset of the current level
        /// </summary>
        /// <param name="offset">The offset</param>
        public void LoadOffsetLevel(int offset)
        {
            GameManager.LoadOffsetLevel(offset);
        }

        /// <summary>
        /// Load the saved position
        /// </summary>
        public void LoadSavedPosition()
        {
            GameManager.PlayerCoordinates = GameManager.SavedPosition;
        }

        /// <summary>
        /// Save the position
        /// </summary>
        public void SavePosition()
        {
            GameManager.SavedPosition = GameManager.PlayerCoordinates;
        }

        #endregion
    }
}
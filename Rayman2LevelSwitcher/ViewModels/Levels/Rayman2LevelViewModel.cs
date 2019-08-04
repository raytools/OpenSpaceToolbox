using System.Windows.Input;

namespace Rayman2LevelSwitcher
{
    /// <summary>
    /// View model for a Rayman 2 level
    /// </summary>
    public class Rayman2LevelViewModel : BaseViewModel
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="name">The level name</param>
        /// <param name="fileName">The level file name</param>
        /// <param name="type">The level type</param>
        public Rayman2LevelViewModel(string name, string fileName, Rayman2LevelType type)
        {
            Name = name;
            FileName = fileName;
            Type = type;

            LoadLevelCommand = new RelayCommand(LoadLevel);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The level name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The level file name
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// The level type
        /// </summary>
        public Rayman2LevelType Type { get; }

        #endregion

        #region Commands

        public ICommand LoadLevelCommand { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads the level
        /// </summary>
        public void LoadLevel()
        {
            new Rayman2Manager().ChangeLevel(FileName);
        }

        #endregion
    }
}
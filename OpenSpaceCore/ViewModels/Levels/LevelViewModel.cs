using OpenSpaceCore.DataModels.Enums;
using OpenSpaceCore.Helpers.WPF;

namespace OpenSpaceCore.ViewModels.Levels
{
    /// <summary>
    /// View model for a Rayman 2 level
    /// </summary>
    public class LevelViewModel : BaseViewModel
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="name">The level name</param>
        /// <param name="fileName">The level file name</param>
        /// <param name="type">The level type</param>
        public LevelViewModel(string name, string fileName, LevelType type)
        {
            Name = name;
            FileName = fileName;
            Type = type;
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
        public LevelType Type { get; }

        #endregion

    }
}
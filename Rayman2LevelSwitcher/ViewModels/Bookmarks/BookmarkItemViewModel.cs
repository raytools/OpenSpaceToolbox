namespace Rayman2LevelSwitcher
{
    /// <summary>
    /// View model for a bookmark item
    /// </summary>
    public class BookmarkItemViewModel : BaseViewModel
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="level">The bookmark level</param>
        /// <param name="name">The bookmark name</param>
        /// <param name="x">The X position</param>
        /// <param name="y">The Y position</param>
        /// <param name="z">The Z position</param>
        public BookmarkItemViewModel(string level, string name, float x, float y, float z)
        {
            Level = level;
            Name = name;
            X = x;
            Y = y;
            Z = z;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The bookmark name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The bookmark level
        /// </summary>
        public string Level { get; }

        /// <summary>
        /// The X position
        /// </summary>
        public float X { get; }

        /// <summary>
        /// The Y position
        /// </summary>
        public float Y { get; }

        /// <summary>
        /// The Z position
        /// </summary>
        public float Z { get; }

        #endregion

    }
}
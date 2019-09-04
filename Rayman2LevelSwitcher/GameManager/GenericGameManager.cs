namespace Rayman2LevelSwitcher
{
    /// <summary>
    /// Generic game manager.
    /// </summary>
    public abstract class GenericGameManager
    {
        #region Public Properties

        /// <summary>
        /// The full name of the game.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// The name of the executable, without extension.
        /// </summary>
        public string ExecName { get; protected set; }

        /// <summary>
        /// The name of the game window, used for determining if the game is focused.
        /// </summary>
        public string WindowName { get; protected set; }

        /// <summary>
        /// Player coordinates tuple.
        /// Retrieving and writing values to the memory should be handled in functions: ReadCoordinates and WriteCoordinates.
        /// </summary>
        public (float, float, float) PlayerCoordinates
        {
            get => ReadCoordinates();
            set => WriteCoordinates(value.Item1, value.Item2, value.Item3);
        }

        /// <summary>
        /// Current level name.
        /// Retrieving and writing values to the memory should be handled in functions: GetCurrentLevelName and ChangeLevel.
        /// </summary>
        public string CurrentLevel
        {
            get => GetCurrentLevelName();
            set => ChangeLevel(value);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Reads player coordinates from the game memory.
        /// </summary>
        /// <returns></returns>
        protected abstract (float, float, float) ReadCoordinates();

        /// <summary>
        /// Writes player coordinates from the game memory.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        protected abstract void WriteCoordinates(float x, float y, float z);

        /// <summary>
        /// Get current level name from the game memory.
        /// </summary>
        /// <returns></returns>
        protected abstract string GetCurrentLevelName();

        /// <summary>
        /// Loads a new level.
        /// </summary>
        /// <param name="levelName"></param>
        protected abstract void ChangeLevel(string levelName);

        #endregion

        #region Public Methods

        /// <summary>
        /// Get the process handle of the game.
        /// </summary>
        /// <param name="showMessage">Indicates if a message should be shown if the process is not found</param>
        /// <returns></returns>
        public abstract int GetProcessHandle(bool showMessage = true);

        /// <summary>
        /// Checks if the game is currently paused.
        /// </summary>
        /// <returns></returns>
        public abstract bool IsGamePaused();

        /// <summary>
        /// Checks if the game window is currently focused.
        /// </summary>
        /// <returns></returns>
        public abstract bool IsGameFocused();

        /// <summary>
        /// Reloads the current level.
        /// </summary>
        public abstract void ReloadLevel();

        #endregion
    }
}

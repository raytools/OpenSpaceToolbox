namespace Rayman2LevelSwitcher
{
    /// <summary>
    /// Generic game manager.
    /// </summary>
    public abstract class GenericGameManager
    {
        #region Protected Properties


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

        #endregion

        #region Public Properties

        /// <summary>
        /// The full name of the game.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// The name of the game executable, without extension.
        /// </summary>
        public abstract string ExecName { get; }

        /// <summary>
        /// The name of the game window, used for determining if the game is focused.
        /// </summary>
        public abstract string WindowName { get; }

        /// <summary>
        /// Player coordinates tuple.
        /// Retrieving and writing values to the memory should be handled in the ReadCoordinates and WriteCoordinates functions.
        /// </summary>
        public (float, float, float) PlayerCoordinates
        {
            get => ReadCoordinates();
            set => WriteCoordinates(value.Item1, value.Item2, value.Item3);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get the process handle of the game.
        /// </summary>
        /// <param name="showMessage">Indicates if a message should be shown if the process is not found</param>
        /// <returns></returns>
        public abstract int GetProcessHandle(bool showMessage = true);

        /// <summary>
        /// Get current level name from the game memory.
        /// </summary>
        /// <returns></returns>
        public abstract string GetCurrentLevelName();

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

        /// <summary>
        /// Loads a new level.
        /// </summary>
        /// <param name="levelName"></param>
        public abstract void ChangeLevel(string levelName);

        #endregion
    }
}

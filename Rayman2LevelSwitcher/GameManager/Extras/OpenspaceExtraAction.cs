namespace Rayman2LevelSwitcher
{
    public abstract class OpenspaceExtraAction : ExtraAction
    {
        #region Constructor

        protected OpenspaceExtraAction(OpenspaceGameManager gameManager)
        {
            GameManager = gameManager;
        }

        #endregion

        #region Public Properties

        public OpenspaceGameManager GameManager { get; }

        #endregion
    }
}

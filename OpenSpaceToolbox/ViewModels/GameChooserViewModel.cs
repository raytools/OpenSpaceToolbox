using OpenSpaceCore.DataModels;
using OpenSpaceCore.Helpers.WPF;

namespace OpenSpaceToolbox
{
    public class GameChooserViewModel : BaseViewModel
    {
        #region Constructor

        public GameChooserViewModel()
        {
            GameList = new GameList();
        }

        #endregion

        #region Public Properties

        public GameList GameList { get; }

        public int SelectedListIndex { get; set; }

        public GameItem SelectedGame { get; set; }

        #endregion

        #region Public Methods

        public void SelectGame()
        {
            SelectedGame = GameList.Games[SelectedListIndex];
        }

        #endregion
    }
}

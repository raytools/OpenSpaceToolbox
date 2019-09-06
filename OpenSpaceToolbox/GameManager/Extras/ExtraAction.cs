using System.Windows.Input;

namespace OpenSpaceToolbox
{
    public abstract class ExtraAction : BaseViewModel
    {
        #region Constructor

        protected ExtraAction()
        {
            ExecuteActionCommand = new RelayCommand(Action);
        }

        #endregion

        #region Commands

        public ICommand ExecuteActionCommand { get; }

        #endregion

        #region Public Properties

        public string Name { get; protected set; }

        public string ShortName { get; protected set; }

        #endregion

        #region Public Methods

        public abstract void Action();

        #endregion

    }
}
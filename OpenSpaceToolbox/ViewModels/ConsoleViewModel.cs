using System.Linq;
using System.Windows.Input;
using OpenSpaceCore.GameManager;
using OpenSpaceCore.Helpers.WPF;
using OpenSpaceCore.Helpers.WPF.Command;

namespace OpenSpaceToolbox
{
    public class ConsoleViewModel : BaseViewModel
    {
        #region Constructor

        public ConsoleViewModel(GenericGameManager gameManager)
        {
            ConsoleManager = new ConsoleCommandManager(gameManager);

            ExecutePromptCommand = new RelayCommand(ExecutePrompt);
            ClearCommand = new RelayCommand(Clear);

            Log += ConsoleManager.Execute("version") + '\n';
        }

        #endregion

        #region Commands

        public ICommand ExecutePromptCommand { get; }

        public ICommand ClearCommand { get; }

        #endregion

        #region Public Properties

        public ConsoleCommandManager ConsoleManager { get; }

        public string Log { get; set; }

        public string Prompt { get; set; }

        #endregion

        #region Public Methods

        public void ExecutePrompt()
        {
            Log += $"> {Prompt}\n";

            string[] args = Prompt.Split(' ');
            Log += ConsoleManager.Execute(args[0], args.Skip(1).ToArray()) + '\n';

            Prompt = string.Empty;
        }

        public void Clear()
        {
            Log = string.Empty;
            Prompt = string.Empty;
        }

        #endregion
    }
}
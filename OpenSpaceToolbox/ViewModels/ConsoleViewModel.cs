using System;
using System.Windows;
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
            GameManager = gameManager;

            ExecutePromptCommand = new RelayCommand(ExecutePrompt);
            ClearCommand = new RelayCommand(Clear);

            Log = string.Empty;
        }

        #endregion

        #region Commands

        public ICommand ExecutePromptCommand { get; }

        public ICommand ClearCommand { get; }

        #endregion

        #region Public Properties

        public GenericGameManager GameManager { get; }

        public string Log { get; set; }

        public string Prompt { get; set; }

        #endregion

        #region Public Methods

        public void ExecutePrompt()
        {
            Log += Prompt + '\n';
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
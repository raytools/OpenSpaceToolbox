using System.Windows;
using System.Windows.Input;
using OpenSpaceCore.Helpers.WPF.Command;

namespace OpenSpaceCore.Helpers.WPF
{
    /// <summary>
    /// Common WPF commands
    /// </summary>
    public static class CommonCommands
    {
        /// <summary>
        /// Closes the current application
        /// </summary>
        public static ICommand CloseAppCommand => new RelayCommand(() => Application.Current.Shutdown());
    }
}
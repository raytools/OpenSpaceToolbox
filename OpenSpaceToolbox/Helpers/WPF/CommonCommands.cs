using System.Windows;
using System.Windows.Input;

namespace OpenSpaceToolbox
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
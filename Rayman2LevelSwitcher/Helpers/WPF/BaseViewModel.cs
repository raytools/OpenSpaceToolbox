using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Rayman2LevelSwitcher
{
    /// <summary>
    /// A base view model
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
using System.Collections.ObjectModel;

namespace Rayman2LevelSwitcher
{
    /// <summary>
    /// View model for a Rayman 2 level container
    /// </summary>
    public class Rayman2LevelContainerViewModel : BaseViewModel
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="name">The container name</param>
        /// <param name="levels">The contained levels</param>
        public Rayman2LevelContainerViewModel(string name, ObservableCollection<Rayman2LevelViewModel> levels)
        {
            Name = name;
            Levels = levels;
        }

        /// <summary>
        /// The container name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The contained levels
        /// </summary>
        public ObservableCollection<Rayman2LevelViewModel> Levels { get; }

    }
}
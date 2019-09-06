using System.Collections.ObjectModel;

namespace OpenSpaceToolbox
{
    /// <summary>
    /// View model for a Rayman 2 level container
    /// </summary>
    public class LevelContainerViewModel : BaseViewModel
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="name">The container name</param>
        /// <param name="levels">The contained levels</param>
        public LevelContainerViewModel(string name, ObservableCollection<LevelViewModel> levels)
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
        public ObservableCollection<LevelViewModel> Levels { get; }

    }
}
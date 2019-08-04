using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;

namespace Rayman2LevelSwitcher
{
    /// <summary>
    /// The app view model
    /// </summary>
    public class AppViewModel : BaseViewModel
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public AppViewModel()
        {
            LevelContainers = new ObservableCollection<Rayman2LevelContainerViewModel>()
            {
                new Rayman2LevelContainerViewModel("Menu", new ObservableCollection<Rayman2LevelViewModel>()
                {
                    new Rayman2LevelViewModel("The Hall of Doors", "mapmonde", Rayman2LevelType.Menu),
                    new Rayman2LevelViewModel("Score Screen", "raycap", Rayman2LevelType.Menu),
                }),
                new Rayman2LevelContainerViewModel("Bonus", new ObservableCollection<Rayman2LevelViewModel>()
                {
                    new Rayman2LevelViewModel("Walk of Life", "ly_10", Rayman2LevelType.Level),
                    new Rayman2LevelViewModel("Walk of Power", "ly_20", Rayman2LevelType.Level),
                }),
                new Rayman2LevelContainerViewModel("Cutscenes", new ObservableCollection<Rayman2LevelViewModel>()
                {
                    new Rayman2LevelViewModel("Council Chamber of the Teensies", "nego_10", Rayman2LevelType.Cutscene),
                    new Rayman2LevelViewModel("Meanwhile in the Prison Ship 1", "batam_10", Rayman2LevelType.Cutscene),
                    new Rayman2LevelViewModel("Meanwhile in the Prison Ship 2", "batam_20", Rayman2LevelType.Cutscene),
                    new Rayman2LevelViewModel("Echoing Caves Intro", "bast_09", Rayman2LevelType.Cutscene),
                    new Rayman2LevelViewModel("Iron Mountains Balloon Cutscene", "ball", Rayman2LevelType.Cutscene),
                    new Rayman2LevelViewModel("Ending", "end_10", Rayman2LevelType.Cutscene),
                    new Rayman2LevelViewModel("Credits", "staff_10", Rayman2LevelType.Cutscene),
                    new Rayman2LevelViewModel("Polokus Mask 1", "poloc_10", Rayman2LevelType.Cutscene),
                    new Rayman2LevelViewModel("Polokus Mask 2", "poloc_20", Rayman2LevelType.Cutscene),
                    new Rayman2LevelViewModel("Polokus Mask 3", "poloc_30", Rayman2LevelType.Cutscene),
                    new Rayman2LevelViewModel("Polokus Mask 4", "poloc_40", Rayman2LevelType.Cutscene),
                }),
                new Rayman2LevelContainerViewModel("The Woods of Light", new ObservableCollection<Rayman2LevelViewModel>()
                {
                    new Rayman2LevelViewModel("Intro", "jail_10", Rayman2LevelType.Level),
                    new Rayman2LevelViewModel("Jail", "jail_20", Rayman2LevelType.Level),
                    new Rayman2LevelViewModel("The Woods of Light", "learn_10", Rayman2LevelType.Level),
                }),
                new Rayman2LevelContainerViewModel("The Fairy Glade", new ObservableCollection<Rayman2LevelViewModel>()
                {
                    new Rayman2LevelViewModel("Part 1", "learn_30", Rayman2LevelType.Level),
                    new Rayman2LevelViewModel("Part 2", "learn_31", Rayman2LevelType.Level),
                    new Rayman2LevelViewModel("Part 3", "bast_20", Rayman2LevelType.Level),
                    new Rayman2LevelViewModel("Part 4", "bast_22", Rayman2LevelType.Level),
                    new Rayman2LevelViewModel("Part 5", "learn_60", Rayman2LevelType.Level),
                }),
                new Rayman2LevelContainerViewModel("The Marshes of Awakening", new ObservableCollection<Rayman2LevelViewModel>()
                {
                    new Rayman2LevelViewModel("Part 1", "ski_10", Rayman2LevelType.Level),
                    new Rayman2LevelViewModel("Part 2", "ski_60", Rayman2LevelType.Level),
                }),
                new Rayman2LevelContainerViewModel("The Bayou", new ObservableCollection<Rayman2LevelViewModel>()
                {
                    new Rayman2LevelViewModel("Part 1", "chase_10", Rayman2LevelType.Level),
                    new Rayman2LevelViewModel("Part 2", "chase_22", Rayman2LevelType.Level),
                }),
                new Rayman2LevelContainerViewModel("The Sanctuary of Water and Ice", new ObservableCollection<Rayman2LevelViewModel>()
                {
                    new Rayman2LevelViewModel("Part 1", "water_10", Rayman2LevelType.Level),
                    new Rayman2LevelViewModel("Part 2", "water_20", Rayman2LevelType.Level),
                }),
                new Rayman2LevelContainerViewModel("The Menhir Hills", new ObservableCollection<Rayman2LevelViewModel>()
                {
                    new Rayman2LevelViewModel("Part 1", "rodeo_10", Rayman2LevelType.Level),
                    new Rayman2LevelViewModel("Part 2", "rodeo_40", Rayman2LevelType.Level),
                    new Rayman2LevelViewModel("Part 3", "rodeo_60", Rayman2LevelType.Level),
                }),
                new Rayman2LevelContainerViewModel("The Cave of Bad Dreams", new ObservableCollection<Rayman2LevelViewModel>()
                {
                    new Rayman2LevelViewModel("Part 1", "vulca_10", Rayman2LevelType.Level),
                    new Rayman2LevelViewModel("Part 2", "vulca_20", Rayman2LevelType.Level),
                }),
                new Rayman2LevelContainerViewModel("The Canopy", new ObservableCollection<Rayman2LevelViewModel>()
                {
                    new Rayman2LevelViewModel("Part 1", "glob_30", Rayman2LevelType.Level),
                    new Rayman2LevelViewModel("Part 2", "glob_10", Rayman2LevelType.Level),
                    new Rayman2LevelViewModel("Part 3", "glob_20", Rayman2LevelType.Level),
                }),
                new Rayman2LevelContainerViewModel("Whale Bay", new ObservableCollection<Rayman2LevelViewModel>()
                {
                    new Rayman2LevelViewModel("Part 1", "whale_00", Rayman2LevelType.Level),
                    new Rayman2LevelViewModel("Part 2", "whale_05", Rayman2LevelType.Level),
                    new Rayman2LevelViewModel("Part 3", "whale_10", Rayman2LevelType.Level),
                }),
                new Rayman2LevelContainerViewModel("The Sanctuary of Stone and Fire", new ObservableCollection<Rayman2LevelViewModel>()
                {
                    new Rayman2LevelViewModel("Part 1", "plum_00", Rayman2LevelType.Level),
                    new Rayman2LevelViewModel("Part 2", "plum_20", Rayman2LevelType.Level),
                    new Rayman2LevelViewModel("Part 3", "plum_10", Rayman2LevelType.Level),
                }),
                new Rayman2LevelContainerViewModel("The Echoing Caves", new ObservableCollection<Rayman2LevelViewModel>()
                {
                    new Rayman2LevelViewModel("Part 1", "bast_10", Rayman2LevelType.Level),
                    new Rayman2LevelViewModel("Part 2", "cask_10", Rayman2LevelType.Level),
                    new Rayman2LevelViewModel("Part 3", "cask_30", Rayman2LevelType.Level),
                }),
                new Rayman2LevelContainerViewModel("The Precipice", new ObservableCollection<Rayman2LevelViewModel>()
                {
                    new Rayman2LevelViewModel("Part 1", "nave_10", Rayman2LevelType.Level),
                    new Rayman2LevelViewModel("Part 2", "nave_15", Rayman2LevelType.Level),
                    new Rayman2LevelViewModel("Part 3", "nave_20", Rayman2LevelType.Level),
                }),
                new Rayman2LevelContainerViewModel("The Top of the World", new ObservableCollection<Rayman2LevelViewModel>()
                {
                    new Rayman2LevelViewModel("Part 1", "seat_10", Rayman2LevelType.Level),
                    new Rayman2LevelViewModel("Part 2", "seat_11", Rayman2LevelType.Level),
                }),
                new Rayman2LevelContainerViewModel("The Sanctuary of Rock and Lava", new ObservableCollection<Rayman2LevelViewModel>()
                {
                    new Rayman2LevelViewModel("Part 1", "earth_10", Rayman2LevelType.Level),
                    new Rayman2LevelViewModel("Part 2", "earth_20", Rayman2LevelType.Level),
                    new Rayman2LevelViewModel("Part 3", "earth_30", Rayman2LevelType.Level),
                }),
                new Rayman2LevelContainerViewModel("Beneath the Sanctuary of Rock and Lava", new ObservableCollection<Rayman2LevelViewModel>()
                {
                    new Rayman2LevelViewModel("Part 1", "helic_10", Rayman2LevelType.Level),
                    new Rayman2LevelViewModel("Part 2", "helic_20", Rayman2LevelType.Level),
                    new Rayman2LevelViewModel("Part 3", "helic_30", Rayman2LevelType.Level),
                }),
                new Rayman2LevelContainerViewModel("Tomb of the Ancients", new ObservableCollection<Rayman2LevelViewModel>()
                {
                    new Rayman2LevelViewModel("Part 1", "morb_00", Rayman2LevelType.Level),
                    new Rayman2LevelViewModel("Part 2", "morb_10", Rayman2LevelType.Level),
                    new Rayman2LevelViewModel("Part 3", "morb_20", Rayman2LevelType.Level),
                }),
                new Rayman2LevelContainerViewModel("The Iron Mountains", new ObservableCollection<Rayman2LevelViewModel>()
                {
                    new Rayman2LevelViewModel("Part 1", "learn_40", Rayman2LevelType.Level),
                    new Rayman2LevelViewModel("Part 2", "ile_10", Rayman2LevelType.Level),
                    new Rayman2LevelViewModel("Part 3", "mine_10", Rayman2LevelType.Level),
                }),
                new Rayman2LevelContainerViewModel("The Prison Ship", new ObservableCollection<Rayman2LevelViewModel>()
                {
                    new Rayman2LevelViewModel("Part 1", "boat01", Rayman2LevelType.Level),
                    new Rayman2LevelViewModel("Part 2", "boat02", Rayman2LevelType.Level),
                    new Rayman2LevelViewModel("Part 3", "astro_00", Rayman2LevelType.Level),
                    new Rayman2LevelViewModel("Part 4", "astro_10", Rayman2LevelType.Level),
                }),
                new Rayman2LevelContainerViewModel("The Crow's Nest", new ObservableCollection<Rayman2LevelViewModel>()
                {
                    new Rayman2LevelViewModel("Part 1", "rhop_10", Rayman2LevelType.Level),
                }),
            };

            Levels = LevelContainers.SelectMany(x => x.Levels);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The level containers
        /// </summary>
        public ObservableCollection<Rayman2LevelContainerViewModel> LevelContainers { get; }

        /// <summary>
        /// The levels
        /// </summary>
        public IEnumerable<Rayman2LevelViewModel> Levels { get; }

        /// <summary>
        /// Indicates if hotkeys are enabled
        /// </summary>
        public bool HotkeysEnabled { get; set; }

        #endregion
    }
}
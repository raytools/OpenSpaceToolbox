using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OpenSpaceToolbox
{
    public class Rayman2GameManager : OpenspaceGameManager
    {
        #region Constructor

        public Rayman2GameManager()
        {
            //Generic properties
            Name = "Rayman 2: The Great Escape";
            ExecName = "Rayman2";
            WindowName = "Rayman II";
            BookmarkFileName = "Rayman2LevelBookmarks";

            //OpenSpace properties
            EngineStructurePointer = 0x500380;
            EngineModePointer = EngineStructurePointer + 0x0;
            LevelNamePointer = EngineStructurePointer + 0x1F;
            PausedStatePointer = 0x500FAA;
            PlayerCoordinatesBasePointer = 0x500560;
            PlayerCoordinatesOffsets = new[] { 0x224, 0x310, 0x34, 0x0, 0x1ac };

            GhostModePointer = 0x500370;

            PossibleProcessNames = new[] { "Rayman2", "Rayman2.exe", "Rayman2.exe.noshim" };

            //Extras
            ExtraActions = new ObservableCollection<ExtraAction>()
            {
                new GenericSpeedMonitorExtra(this),
                new Rayman2VoidExtra(this),
                new Rayman2NoHpExtra(this),
                new Rayman2MaxHpExtra(this),
                new Rayman2GlmMonitorExtra(this),
                new Rayman2ProgressArrayExtra(this),
                new Rayman2CheckpointExtra(this, Rayman2CheckpointExtra.CheckpointMode.CurrentPosition),
                new Rayman2CheckpointExtra(this, Rayman2CheckpointExtra.CheckpointMode.SavedPosition),
            };

            //Levels
            LevelContainers = new ObservableCollection<LevelContainerViewModel>()
            {
                new LevelContainerViewModel("Menu", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("Main Menu", "menu", LevelType.Menu),
                    new LevelViewModel("The Hall of Doors", "mapmonde", LevelType.Menu),
                    new LevelViewModel("Score Screen", "raycap", LevelType.Menu),
                }),
                new LevelContainerViewModel("Bonus", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("Walk of Life", "ly_10", LevelType.Level),
                    new LevelViewModel("Walk of Power", "ly_20", LevelType.Level),
                    new LevelViewModel("Bonus Stage", "bonux", LevelType.Level),
                }),
                new LevelContainerViewModel("Cutscenes", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("Council Chamber of the Teensies", "nego_10", LevelType.Cutscene),
                    new LevelViewModel("Meanwhile in the Prison Ship 1", "batam_10", LevelType.Cutscene),
                    new LevelViewModel("Meanwhile in the Prison Ship 2", "batam_20", LevelType.Cutscene),
                    new LevelViewModel("Echoing Caves Intro", "bast_09", LevelType.Cutscene),
                    new LevelViewModel("Iron Mountains Balloon Cutscene", "ball", LevelType.Cutscene),
                    new LevelViewModel("Ending", "end_10", LevelType.Cutscene),
                    new LevelViewModel("Credits", "staff_10", LevelType.Cutscene),
                    new LevelViewModel("Polokus Mask 1", "poloc_10", LevelType.Cutscene),
                    new LevelViewModel("Polokus Mask 2", "poloc_20", LevelType.Cutscene),
                    new LevelViewModel("Polokus Mask 3", "poloc_30", LevelType.Cutscene),
                    new LevelViewModel("Polokus Mask 4", "poloc_40", LevelType.Cutscene),
                }),
                new LevelContainerViewModel("The Woods of Light", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("Intro", "jail_10", LevelType.Level),
                    new LevelViewModel("Jail", "jail_20", LevelType.Level),
                    new LevelViewModel("The Woods of Light", "learn_10", LevelType.Level),
                }),
                new LevelContainerViewModel("The Fairy Glade", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("Part 1", "learn_30", LevelType.Level),
                    new LevelViewModel("Part 2", "learn_31", LevelType.Level),
                    new LevelViewModel("Part 3", "bast_20", LevelType.Level),
                    new LevelViewModel("Part 4", "bast_22", LevelType.Level),
                    new LevelViewModel("Part 5", "learn_60", LevelType.Level),
                }),
                new LevelContainerViewModel("The Marshes of Awakening", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("Part 1", "ski_10", LevelType.Level),
                    new LevelViewModel("Part 2", "ski_60", LevelType.Level),
                }),
                new LevelContainerViewModel("The Bayou", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("Part 1", "chase_10", LevelType.Level),
                    new LevelViewModel("Part 2", "chase_22", LevelType.Level),
                }),
                new LevelContainerViewModel("The Sanctuary of Water and Ice", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("Part 1", "water_10", LevelType.Level),
                    new LevelViewModel("Part 2", "water_20", LevelType.Level),
                }),
                new LevelContainerViewModel("The Menhir Hills", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("Part 1", "rodeo_10", LevelType.Level),
                    new LevelViewModel("Part 2", "rodeo_40", LevelType.Level),
                    new LevelViewModel("Part 3", "rodeo_60", LevelType.Level),
                }),
                new LevelContainerViewModel("The Cave of Bad Dreams", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("Part 1", "vulca_10", LevelType.Level),
                    new LevelViewModel("Part 2", "vulca_20", LevelType.Level),
                }),
                new LevelContainerViewModel("The Canopy", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("Part 1", "glob_30", LevelType.Level),
                    new LevelViewModel("Part 2", "glob_10", LevelType.Level),
                    new LevelViewModel("Part 3", "glob_20", LevelType.Level),
                }),
                new LevelContainerViewModel("Whale Bay", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("Part 1", "whale_00", LevelType.Level),
                    new LevelViewModel("Part 2", "whale_05", LevelType.Level),
                    new LevelViewModel("Part 3", "whale_10", LevelType.Level),
                }),
                new LevelContainerViewModel("The Sanctuary of Stone and Fire", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("Part 1", "plum_00", LevelType.Level),
                    new LevelViewModel("Part 2", "plum_20", LevelType.Level),
                    new LevelViewModel("Part 3", "plum_10", LevelType.Level),
                }),
                new LevelContainerViewModel("The Echoing Caves", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("Part 1", "bast_10", LevelType.Level),
                    new LevelViewModel("Part 2", "cask_10", LevelType.Level),
                    new LevelViewModel("Part 3", "cask_30", LevelType.Level),
                }),
                new LevelContainerViewModel("The Precipice", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("Part 1", "nave_10", LevelType.Level),
                    new LevelViewModel("Part 2", "nave_15", LevelType.Level),
                    new LevelViewModel("Part 3", "nave_20", LevelType.Level),
                }),
                new LevelContainerViewModel("The Top of the World", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("Part 1", "seat_10", LevelType.Level),
                    new LevelViewModel("Part 2", "seat_11", LevelType.Level),
                }),
                new LevelContainerViewModel("The Sanctuary of Rock and Lava", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("Part 1", "earth_10", LevelType.Level),
                    new LevelViewModel("Part 2", "earth_20", LevelType.Level),
                    new LevelViewModel("Part 3", "earth_30", LevelType.Level),
                }),
                new LevelContainerViewModel("Beneath the Sanctuary of Rock and Lava", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("Part 1", "helic_10", LevelType.Level),
                    new LevelViewModel("Part 2", "helic_20", LevelType.Level),
                    new LevelViewModel("Part 3", "helic_30", LevelType.Level),
                }),
                new LevelContainerViewModel("Tomb of the Ancients", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("Part 1", "morb_00", LevelType.Level),
                    new LevelViewModel("Part 2", "morb_10", LevelType.Level),
                    new LevelViewModel("Part 3", "morb_20", LevelType.Level),
                }),
                new LevelContainerViewModel("The Iron Mountains", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("Part 1", "learn_40", LevelType.Level),
                    new LevelViewModel("Part 2", "ile_10", LevelType.Level),
                    new LevelViewModel("Part 3", "mine_10", LevelType.Level),
                }),
                new LevelContainerViewModel("The Prison Ship", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("Part 1", "boat01", LevelType.Level),
                    new LevelViewModel("Part 2", "boat02", LevelType.Level),
                    new LevelViewModel("Part 3", "astro_00", LevelType.Level),
                    new LevelViewModel("Part 4", "astro_10", LevelType.Level),
                }),
                new LevelContainerViewModel("The Crow's Nest", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("Part 1", "rhop_10", LevelType.Level),
                }),
            };

            Levels = LevelContainers.SelectMany(x => x.Levels);

            Task.Run(ChangeHudIcon);
        }

        private int _hudChangedForProcess = -1;

        private async void ChangeHudIcon()
        {
            while (true) {
                int processHandle = GetProcessHandle(false);

                if (processHandle > 0 && _hudChangedForProcess != processHandle) {
                    _hudChangedForProcess = processHandle;
                    WriteBytes(new byte[] { (byte)'x' }, 0x4B7314, 0x48, 0x54, 0x0, 0x10, 0x0, 0x4, 0x6EF);
                }

                await Task.Delay(1000);
            }
        }

        #endregion
    }
}

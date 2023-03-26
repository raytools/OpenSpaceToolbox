using System.Collections.ObjectModel;
using System.Linq;

namespace OpenSpaceToolbox
{
    public class Rayman3GameManager : OpenspaceGameManager
    {
        #region Constructor

        public Rayman3GameManager()
        {
            //Generic properties
            Name = "Rayman 3: Hoodlum Havoc";
            ExecName = "Rayman3";
            WindowName = "Rayman III";
            BookmarkFileName = "Rayman3LevelBookmarks";

            //OpenSpace properties
            EngineStructurePointer = 0x7D7DC0;
            EngineModePointer = EngineStructurePointer + 0x0;
            LevelNamePointer = EngineStructurePointer + 0x1F;
            PausedStatePointer = 0x7D848C;
            PlayerCoordinatesBasePointer = 0x5BFAD4;
            PlayerCoordinatesOffsets = new[] { 0x140, 0x258, 0x108, 0x324};
            PossibleProcessNames = new[] { "Rayman3", "Rayman3.exe", "Rayman3.exe.noshim" };

            //Extras
            ExtraActions = new ObservableCollection<ExtraAction>()
            {
                new GenericSpeedMonitorExtra(this),
            };

            //Levels
            LevelContainers = new ObservableCollection<LevelContainerViewModel>()
            {
                new LevelContainerViewModel("Menu / Miscellaneous", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("Main Menu", "menumap", LevelType.Menu),
                    new LevelViewModel("Staff Roll", "staff", LevelType.Menu),
                    new LevelViewModel("Bonus (Empty)", "BonusTXT", LevelType.Menu),
                    new LevelViewModel("Endgame (Empty)", "endgame", LevelType.Menu),
                }),
                new LevelContainerViewModel("Minigames", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("Arcade - 2D Madness",       "toudi_00", LevelType.Bonus),
                    new LevelViewModel("Arcade - Racket Jump",      "ten_map", LevelType.Bonus),
                    new LevelViewModel("Arcade - Crush",            "crush",  LevelType.Bonus),
                    new LevelViewModel("Arcade - Razoff Circus",    "raz_map", LevelType.Bonus),
                    new LevelViewModel("Arcade - Sentinel",         "sentinel", LevelType.Bonus),
                    new LevelViewModel("Arcade - Missile Command",  "snipe_00", LevelType.Bonus),
                    new LevelViewModel("Arcade - Balloons",         "ballmap", LevelType.Bonus),
                    new LevelViewModel("Arcade - Special Invaders", "ship_map", LevelType.Bonus),
                    new LevelViewModel("Arcade - Commando",         "commando", LevelType.Bonus),
                }),
                new LevelContainerViewModel("The Fairy Council", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("The Fairy Council 1 (Murfy)", "intro_10", LevelType.Level),
                    new LevelViewModel("The Fairy Council 2 (Finding Globox)", "intro_15", LevelType.Level),
                    new LevelViewModel("The Fairy Council 3 (Inside)", "intro_17", LevelType.Level),
                    new LevelViewModel("The Fairy Council 4", "intro_20", LevelType.Level),
                    new LevelViewModel("The Fairy Council 5 (Heart of the World)", "menu_00", LevelType.Level),
                    new LevelViewModel("The Fairy Council 6 (Teensie Highway)", "sk8_00", LevelType.Level),
                }),
                new LevelContainerViewModel("Clearleaf Forest", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("Clearleaf Forest 1", "wood_11", LevelType.Level),
                    new LevelViewModel("Clearleaf Forest 2", "wood_10", LevelType.Level),
                    new LevelViewModel("Clearleaf Forest 3", "wood_19", LevelType.Level),
                    new LevelViewModel("Clearleaf Forest 4 (Master Kaag)", "wood_50", LevelType.Level),
                    new LevelViewModel("Clearleaf Forest 5 (Doctor's Office)", "menu_10", LevelType.Level),
                    new LevelViewModel("Clearleaf Forest 6 (Teensie Highway)", "sk8_10", LevelType.Level),
                }),
                new LevelContainerViewModel("The Bog of Murk", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("The Bog of Murk 1 (Bégoniax)", "swamp_60", LevelType.Level),
                    new LevelViewModel("The Bog of Murk 2", "swamp_82", LevelType.Level),
                    new LevelViewModel("The Bog of Murk 3", "swamp_81", LevelType.Level),
                    new LevelViewModel("The Bog of Murk 4", "swamp_83", LevelType.Level),
                    new LevelViewModel("The Bog of Murk 5 (Razoff's Mansion)", "swamp_50", LevelType.Level),
                    new LevelViewModel("The Bog of Murk 6 (Razoff's Basement)", "swamp_51", LevelType.Level),
                }),
                new LevelContainerViewModel("The Land of the Livid Dead", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("The Land of the Livid Dead 1", "moor_00", LevelType.Level),
                    new LevelViewModel("The Land of the Livid Dead 2", "moor_30", LevelType.Level),
                    new LevelViewModel("The Land of the Livid Dead 3 (Tower)", "moor_60", LevelType.Level),
                    new LevelViewModel("The Land of the Livid Dead 4 (Céloche)", "moor_19", LevelType.Level),
                    new LevelViewModel("The Land of the Livid Dead 5 (Doctor's Office)", "menu_20", LevelType.Level),
                    new LevelViewModel("The Land of the Livid Dead 6 (Teensie Highway)", "sk8_20", LevelType.Level),
                }),
                new LevelContainerViewModel("The Desert of the Knaaren", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("The Desert of the Knaaren 1", "knaar_10", LevelType.Level),
                    new LevelViewModel("The Desert of the Knaaren 2 (The Great Hall)", "knaar_20", LevelType.Level),
                    new LevelViewModel("The Desert of the Knaaren 3 (Tower)", "knaar_30", LevelType.Level),
                    new LevelViewModel("The Desert of the Knaaren 4", "knaar_45", LevelType.Level),
                    new LevelViewModel("The Desert of the Knaaren 5 (Arena)", "knaar_60", LevelType.Level),
                    new LevelViewModel("The Desert of the Knaaren 6 (Grimace Room)", "knaar_69", LevelType.Level),
                    new LevelViewModel("The Desert of the Knaaren 7", "knaar_70", LevelType.Level),
                    new LevelViewModel("The Desert of the Knaaren 8 (Doctor's Office)", "menu_30", LevelType.Level),
                }),
                new LevelContainerViewModel("The Longest Shortcut", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("The Longest Shortcut 1", "flash_20", LevelType.Level),
                    new LevelViewModel("The Longest Shortcut 2", "flash_30", LevelType.Level),
                    new LevelViewModel("The Longest Shortcut 3", "flash_10", LevelType.Level),
                }),
                new LevelContainerViewModel("The Summit Beyond the Clouds", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("The Summit Beyond the Clouds 1 (The Looming Sea)", "sea_10", LevelType.Level),
                    new LevelViewModel("The Summit Beyond the Clouds 2", "mount_50", LevelType.Level),
                    new LevelViewModel("The Summit Beyond the Clouds 3 (Snowboard)", "mount_4x", LevelType.Level),
                }),
                new LevelContainerViewModel("Hoodlum Headquarters", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("Hoodlum Headquarters 1", "fact_40", LevelType.Level),
                    new LevelViewModel("Hoodlum Headquarters 2 (Firing Range)", "fact_50", LevelType.Level),
                    new LevelViewModel("Hoodlum Headquarters 3", "fact_55", LevelType.Level),
                    new LevelViewModel("Hoodlum Headquarters 4 (Horrible Machine)", "fact_34", LevelType.Level),
                    new LevelViewModel("Hoodlum Headquarters 5 (Rising Lava)", "fact_22", LevelType.Level),
                }),
                new LevelContainerViewModel("The Tower of the Leptys", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("The Tower of the Leptys 1", "tower_10", LevelType.Level),
                    new LevelViewModel("The Tower of the Leptys 2", "tower_20", LevelType.Level),
                    new LevelViewModel("The Tower of the Leptys 3", "tower_30", LevelType.Level),
                    new LevelViewModel("The Tower of the Leptys 4", "tower_40", LevelType.Level),
                    new LevelViewModel("The Tower of the Leptys 5 (Final Battle)", "lept_15", LevelType.Level),
                }),
               
            };

            Levels = LevelContainers.SelectMany(x => x.Levels);
        }

        #endregion
    }
}

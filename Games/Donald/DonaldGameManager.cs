using System.Collections.ObjectModel;
using System.Linq;
using OpenSpaceCore.DataModels.Enums;
using OpenSpaceCore.GameManager;
using OpenSpaceCore.GameManager.Extras;
using OpenSpaceCore.ViewModels.Levels;

namespace Donald
{
    public class DonaldGameManager : OpenspaceGameManager
    {
        #region Constructor

        public DonaldGameManager()
        {
            //Generic properties
            Name = "Donald Duck: Goin' Quackers";
            ExecName = "Donald";
            WindowName = "Donald Duck";
            BookmarkFileName = "DonaldLevelBookmarks";

            //OpenSpace properties
            EngineStructurePointer = 0x400000 + 0x1AE2A0;
            EngineModePointer = EngineStructurePointer + 0x0;
            LevelNamePointer = EngineStructurePointer + 0x1F;
            PausedStatePointer = 0x400000 + 0x167298;
            PlayerCoordinatesBasePointer = 0x400000 + 0x1622C4;
            PlayerCoordinatesOffsets = new[] { 0x8, 0x40 + 0x6C};
            PossibleProcessNames = new[] { "Donald", "Donald.exe" };

            //Extras
            ExtraActions = new ObservableCollection<ExtraAction>()
            {
                new DonaldLivesExtra(this),
            };

            //Levels
            LevelContainers = new ObservableCollection<LevelContainerViewModel>()
            {
                new LevelContainerViewModel("General", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("Menu", "menu", LevelType.Menu),
                    new LevelViewModel("Gyro's Lab/Hubs", "mapmonde", LevelType.Menu),
                    new LevelViewModel("Credits", "credits", LevelType.Menu),
                }),
                new LevelContainerViewModel("Forest", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("The Gorge", "TheHike", LevelType.Level),
                    new LevelViewModel("Forest Edge", "TRAIL", LevelType.Level),
                    new LevelViewModel("Dangerous Cliff", "BRUSH", LevelType.Level),
                    new LevelViewModel("The Track", "Forest_1_B", LevelType.Level),
                    new LevelViewModel("Boss (Bernadette Bird)", "forest_Boss", LevelType.Level),
                    new LevelViewModel("Bear's Path", "forest_chase", LevelType.Level),
                }),
                new LevelContainerViewModel("City", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("Urban High-Rises", "EDGE", LevelType.Level),
                    new LevelViewModel("Roof Tops", "CITY_2_B", LevelType.Level),
                    new LevelViewModel("First Avenue", "SIDEWALK", LevelType.Level),
                    new LevelViewModel("The Roofs", "ROOF", LevelType.Level),
                    new LevelViewModel("Boss (Beagle Boys)", "city_boss", LevelType.Level),
                    new LevelViewModel("Main Street", "city_chase", LevelType.Level),
                }),
                new LevelContainerViewModel("Haunted", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("Haunted Hall", "Haunted_3_1", LevelType.Level),
                    new LevelViewModel("Earie Alley", "Haunted_3_2", LevelType.Level),
                    new LevelViewModel("Ghostly Path", "Haunted_3_3", LevelType.Level),
                    new LevelViewModel("Creepy Corridor", "Haunted_3_B", LevelType.Level),
                    new LevelViewModel("Boss (Magica De Spell)", "Haunted_boss", LevelType.Level),
                    new LevelViewModel("Under'hand'ed", "Haunted_3_4", LevelType.Level),
                }),
                new LevelContainerViewModel("Inca", new ObservableCollection<LevelViewModel>()
                {
                    new LevelViewModel("Temple's Entrance", "Inca_4_1", LevelType.Level),
                    new LevelViewModel("Artifact Way", "INCA_4_2", LevelType.Level),
                    new LevelViewModel("Ancient Fate", "INCA_4_3", LevelType.Level),
                    new LevelViewModel("Murky Way", "INCA_4_B", LevelType.Level),
                    new LevelViewModel("Boss (Merlock)", "inca_boss", LevelType.Level),
                    new LevelViewModel("Head Alley", "INCA_4_4", LevelType.Level),
                }),

            };

            Levels = LevelContainers.SelectMany(x => x.Levels);
        }

        #endregion
    }
}

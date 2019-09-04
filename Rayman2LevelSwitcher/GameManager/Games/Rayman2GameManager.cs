namespace Rayman2LevelSwitcher
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

            //OpenSpace properties
            EngineStructurePointer = 0x500380;
            EngineModePointer = EngineStructurePointer + 0x0;
            LevelNamePointer = EngineStructurePointer + 0x1F;
            PausedStatePointer = 0x500FAA;
            PlayerCoordinatesBasePointer = 0x500560;
            PlayerCoordinatesOffsets = new[] { 0x224, 0x310, 0x34, 0x0, 0x1ac };
            PossibleProcessNames = new[] { "Rayman2", "Rayman2.exe", "Rayman2.exe.noshim" };
        }

        #endregion
    }
}

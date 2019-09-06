namespace OpenSpaceToolbox
{
    public class Rayman2VoidExtra : OpenspaceExtraAction
    {
        public Rayman2VoidExtra(Rayman2GameManager gameManager) : base(gameManager)
        {
            Name = ShortName = "Void";
            VoidPointer = 0x500FAA;
        }

        private int VoidPointer { get; }

        public override void Action()
        {
            int processHandle = GameManager.GetProcessHandle();
            if (processHandle < 0)
                return;

            int bytesReadOrWritten = 0;
            byte[] buffer = { 0 };

            Memory.WriteProcessMemory((int)processHandle, VoidPointer, buffer, buffer.Length, ref bytesReadOrWritten);
        }
    }
}
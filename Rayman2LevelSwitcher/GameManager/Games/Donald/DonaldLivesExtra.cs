namespace Rayman2LevelSwitcher
{
    public class DonaldLivesExtra : OpenspaceExtraAction
    {
        private int LivesPointerBase;
        private int[] LivesPointerOffsets;

        public DonaldLivesExtra(DonaldGameManager gameManager) : base(gameManager)
        {
            Name = ShortName = "99 Lives";
            LivesPointerBase = 0x400000 + 0x1B8C34;
            LivesPointerOffsets = new int[] { 0, 0xC, 0x10, 0x14C, 0x7CC };
        }

        public override void Action()
        {
            int processHandle = GameManager.GetProcessHandle();
            if (processHandle < 0)
                return;

            int bytesReadOrWritten = 0;
            byte[] buffer = { 99 };

            int livesOffset = Memory.GetPointerPath((int)processHandle, LivesPointerBase, LivesPointerOffsets);
            Memory.WriteProcessMemory((int)processHandle, livesOffset, buffer, buffer.Length, ref bytesReadOrWritten);
        }
    }
}
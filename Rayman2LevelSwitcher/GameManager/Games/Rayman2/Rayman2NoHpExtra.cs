using System;

namespace Rayman2LevelSwitcher
{
    public class Rayman2NoHpExtra : OpenspaceExtraAction
    {
        public Rayman2NoHpExtra(Rayman2GameManager gameManager) : base(gameManager)
        {
            Name = ShortName = "0 HP";
            HealthPointer = 0x500584;
        }

        private int HealthPointer { get; }

        public override void Action()
        {
            int processHandle = GameManager.GetProcessHandle();
            if (processHandle < 0)
                return;

            int bytesReadOrWritten = 0;
            byte[] buffer = { 0 };
            byte[] healthPointerBuffer = new byte[4];

            Memory.ReadProcessMemory(processHandle, HealthPointer, healthPointerBuffer, healthPointerBuffer.Length, ref bytesReadOrWritten);

            int offHealthPointer = BitConverter.ToInt32(healthPointerBuffer, 0) + 0x245;

            Memory.WriteProcessMemory(processHandle, offHealthPointer, buffer, buffer.Length, ref bytesReadOrWritten);
        }
    }
}
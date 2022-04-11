using System;

namespace OpenSpaceToolbox
{
    public class Rayman2MaxHpExtra : OpenspaceExtraAction
    {
        public Rayman2MaxHpExtra(Rayman2GameManager gameManager) : base(gameManager)
        {
            Name = ShortName = "Max HP";
            HealthPointer = 0x500584;
        }

        private int HealthPointer { get; }

        public override void Action()
        {
            int processHandle = GameManager.GetProcessHandle();
            if (processHandle < 0)
                return;

            int bytesReadOrWritten = 0;
            byte[] buffer = { 255 };
            byte[] healthPointerBuffer = new byte[4];

            Memory.ReadProcessMemory(processHandle, HealthPointer, healthPointerBuffer, healthPointerBuffer.Length, ref bytesReadOrWritten);

            int offHealthPointer = BitConverter.ToInt32(healthPointerBuffer, 0) + 0x245;

            Memory.WriteProcessMemory(processHandle, offHealthPointer, buffer, buffer.Length, ref bytesReadOrWritten);
        }
    }
}
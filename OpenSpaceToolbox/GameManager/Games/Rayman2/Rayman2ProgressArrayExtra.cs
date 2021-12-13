using System;
using System.Windows;

namespace OpenSpaceToolbox
{
    public class Rayman2ProgressArrayExtra : OpenspaceExtraAction
    {
        public Rayman2ProgressArrayExtra(Rayman2GameManager gameManager) : base(gameManager)
        {
            Name = "Progress Array";
            ShortName = "Progress Array";
            ProgressArrayBasePointer = 0x500560;
            ProgressArrayOffsets = new[] {0x0, 0x0, 0x6C, 0x4, 0x0, 0x1C, 0x4E4};
        }

        public const int ProgressArrayLengthBytes = 46 * 4; // 46 (not 45) ints in global.DsgVar42

        private int ProgressArrayBasePointer { get; }
        private int[] ProgressArrayOffsets { get; }

        public byte[] ProgressArray
        {
            get => ReadByteArray(ProgressArrayBasePointer, ProgressArrayLengthBytes, ProgressArrayOffsets);
            set
            {
                if (value != null) {
                    WriteByteArray(ProgressArrayBasePointer, value, ProgressArrayOffsets);
                }
            } 
        }

        public byte[] ReadByteArray(int baseAddress, int arrayLength, params int[] offsets)
        {
            int processHandle = GameManager.GetProcessHandle();
            if (processHandle < 0)
                return null;

            int bytesReadOrWritten = 0;
            int offXcoord = Memory.GetPointerPath(processHandle, baseAddress, offsets);

            byte[] buffer = new byte[arrayLength];

            Memory.ReadProcessMemory(processHandle, offXcoord, buffer, arrayLength, ref bytesReadOrWritten);

            return buffer;
        }

        public void WriteByteArray(int baseAddress, byte[] bytes, params int[] offsets)
        {
            int processHandle = GameManager.GetProcessHandle();
            if (processHandle < 0)
                return;

            int bytesReadOrWritten = 0;
            int offset = Memory.GetPointerPath(processHandle, baseAddress, offsets);

            Memory.WriteProcessMemory(processHandle, offset, bytes, bytes.Length, ref bytesReadOrWritten);
        }

        public override void Action()
        {
            ProgressArrayWindow window = new ProgressArrayWindow(new ProgressArrayWindowViewModel(this))
            {
                Owner = Application.Current.MainWindow
            };
            window.Show();
        }
    }
}

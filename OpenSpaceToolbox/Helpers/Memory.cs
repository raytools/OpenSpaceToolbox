using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenSpaceToolbox
{
    public static class Memory
    {
        public const int PROCESS_WM_READ = 0x0010;
        public const int PROCESS_VM_WRITE = 0x0020;
        public const int PROCESS_VM_OPERATION = 0x0008;

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        public static int GetPointerPath(int processHandle, int baseAddress, params int[] offsets)
        {
            int currentAddress = baseAddress;
            int bytesReadOrWritten = 0;

            byte[] buffer = new byte[4];
            ReadProcessMemory(processHandle, currentAddress, buffer, buffer.Length, ref bytesReadOrWritten);
            currentAddress = BitConverter.ToInt32(buffer, 0);

            foreach (int offset in offsets)
            {
                if (offset == offsets.Last())
                {
                    currentAddress += offset;
                }
                else
                {
                    ReadProcessMemory(processHandle, currentAddress + offset, buffer, buffer.Length, ref bytesReadOrWritten);
                    currentAddress = BitConverter.ToInt32(buffer, 0);
                }
            }

            return currentAddress;
        }
    }
}

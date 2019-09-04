using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;

namespace Rayman2LevelSwitcher
{
    /// <summary>
    /// The manager for Rayman 2 operations
    /// </summary>
    /// TODO: extend OpenspaceGameManager
    public class Rayman2Manager
    {
        #region Constant Values

        private const int OffEngineStructure = 0x500380;
        private const int OffEngineMode = OffEngineStructure + 0x0;
        private const int OffLevelName = OffEngineStructure + 0x1F;
        private const int OffHealthPointer = 0x500584;
        private const int OffVoidPointer = 0x500FAA;

        #endregion

        public (float, float, float) PlayerCoordinates
        {
            get => ReadCoordinates(0x500560, 0x224, 0x310, 0x34, 0x0, 0x1ac);
            set => WriteCoordinates(value.Item1, value.Item2, value.Item3, 0x500560, 0x224, 0x310, 0x34, 0x0, 0x1ac);
        }

        //TODO: to generic - extra
        public (float, float, float) GlmCoordinates
        {
            get => ReadCoordinates(0x500298, 0x234, 0x10, 0xC, 0xB0);
            set => WriteCoordinates(value.Item1, value.Item2, value.Item3, 0x500298, 0x234, 0x10, 0xC, 0xB0);
        }

        /// <summary>
        /// Write coordinates to the game memory
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="baseAddress"></param>
        /// <param name="offsets"></param>
        private void WriteCoordinates(float x, float y, float z, int baseAddress, params int[] offsets)
        {
            int processHandle = GetProcessHandle();
            if (processHandle < 0)
                return;

            int bytesReadOrWritten = 0;
            int offXcoord = Memory.GetPointerPath(processHandle, baseAddress, offsets);
            int offYcoord = offXcoord + 4;
            int offZcoord = offXcoord + 8;

            byte[] xCoordBuffer = BitConverter.GetBytes(x);
            byte[] yCoordBuffer = BitConverter.GetBytes(y);
            byte[] zCoordBuffer = BitConverter.GetBytes(z);

            Memory.WriteProcessMemory(processHandle, offXcoord, xCoordBuffer, 4, ref bytesReadOrWritten);
            Memory.WriteProcessMemory(processHandle, offYcoord, yCoordBuffer, 4, ref bytesReadOrWritten);
            Memory.WriteProcessMemory(processHandle, offZcoord, zCoordBuffer, 4, ref bytesReadOrWritten);
        }

        /// <summary>
        /// Reads coordinates from game memory
        /// </summary>
        /// <param name="baseAddress"></param>
        /// <param name="offsets"></param>
        /// <returns></returns>
        private (float, float, float) ReadCoordinates(int baseAddress, params int[] offsets)
        {
            int processHandle = GetProcessHandle();
            if (processHandle < 0)
                return (0, 0, 0);

            int bytesReadOrWritten = 0;
            int offXcoord = Memory.GetPointerPath(processHandle, baseAddress, offsets);
            int offYcoord = offXcoord + 4;
            int offZcoord = offXcoord + 8;

            byte[] xCoordBuffer = new byte[4];
            byte[] yCoordBuffer = new byte[4];
            byte[] zCoordBuffer = new byte[4];

            Memory.ReadProcessMemory(processHandle, offXcoord, xCoordBuffer, 4, ref bytesReadOrWritten);
            Memory.ReadProcessMemory(processHandle, offYcoord, yCoordBuffer, 4, ref bytesReadOrWritten);
            Memory.ReadProcessMemory(processHandle, offZcoord, zCoordBuffer, 4, ref bytesReadOrWritten);

            return (
                BitConverter.ToSingle(xCoordBuffer, 0),
                BitConverter.ToSingle(yCoordBuffer, 0),
                BitConverter.ToSingle(zCoordBuffer, 0)
            );
        }

        /// <summary>
        /// Gets the process handle for Rayman 2
        /// </summary>
        /// <param name="showMessage">Indicates if a message should be shown if the process is not found</param>
        /// <returns></returns>
        public int GetProcessHandle(bool showMessage = true)
        {
            var process = GetRayman2Process();

            if (process == null)
            {
                if (showMessage)
                    MessageBox.Show("Couldn't find process 'Rayman2'. Please make sure Rayman is running or try launching this program with Administrator rights.");

                return -1;
            }

            IntPtr processHandle = Memory.OpenProcess(Memory.PROCESS_WM_READ | Memory.PROCESS_VM_WRITE | Memory.PROCESS_VM_OPERATION, false, process.Id);

            process.Dispose();

            return (int)processHandle;
        }

        /// <summary>
        /// Gets the Rayman 2 process, or null if none was found
        /// </summary>
        /// <returns>The process or null if none was found</returns>
        public Process GetRayman2Process()
        {
            string[] possibleNames =
            {
                "Rayman2",
                "Rayman2.exe",
                "Rayman2.exe.noshim"
            };

            return (from name in possibleNames select Process.GetProcessesByName(name) into p where p.Any() select p.First()).FirstOrDefault();
        }

        /// <summary>
        /// Gets the name of the currently loaded level
        /// </summary>
        /// <param name="processHandle">The process handle</param>
        /// <returns>The level name</returns>
        public string GetCurrentLevelName(int processHandle)
        {
            int bytesReadOrWritten = 0;

            byte[] buffer = new byte[16];

            Memory.ReadProcessMemory(processHandle, OffLevelName, buffer, buffer.Length, ref bytesReadOrWritten);

            string levelName = Encoding.ASCII.GetString(buffer);

            levelName = levelName.Substring(0, levelName.IndexOf((char)0));

            return levelName;
        }

        /// <summary>
        /// Indicates if Rayman 2 if focused
        /// </summary>
        /// <returns>True if it is focused</returns>
        public bool IsGameFocused()
        {
            const int nChars = 256;

            StringBuilder buff = new StringBuilder(nChars);

            IntPtr handle = Memory.GetForegroundWindow();

            if (Memory.GetWindowText(handle, buff, nChars) <= 0)
                return false;

            return buff.ToString() == "Rayman II";
        }

        /// <summary>
        /// Indicates if Rayman 2 is paused
        /// </summary>
        /// <returns>True if it is paused</returns>
        public bool IsGamePaused()
        {
            int processHandle = GetProcessHandle(false);

            if (processHandle < 0)
                return false;

            int bytesReadOrWritten = 0;

            byte[] pausePointerBuffer = new byte[4];

            Memory.ReadProcessMemory(processHandle, OffVoidPointer, pausePointerBuffer, pausePointerBuffer.Length, ref bytesReadOrWritten);

            return BitConverter.ToInt32(pausePointerBuffer, 0) == 1;
        }

        /// <summary>
        /// Activate no health in Rayman 2
        /// </summary>
        /// TODO: to generic - extra
        public void ActivateNoHealth()
        {
            int processHandle = GetProcessHandle();

            if (processHandle < 0)
                return;

            int bytesReadOrWritten = 0;

            byte[] buffer = { 0 };
            byte[] healthPointerBuffer = new byte[4];

            Memory.ReadProcessMemory(processHandle, OffHealthPointer, healthPointerBuffer, healthPointerBuffer.Length, ref bytesReadOrWritten);

            int offHealthPointer = BitConverter.ToInt32(healthPointerBuffer, 0) + 0x245;

            Memory.WriteProcessMemory(processHandle, offHealthPointer, buffer, buffer.Length, ref bytesReadOrWritten);
        }

        /// <summary>
        /// Activate void in Rayman 2
        /// </summary>
        /// TODO: to generic - extra
        public void ActivateVoid()
        {
            Process process = GetRayman2Process();

            if (process == null)
            {
                MessageBox.Show("Couldn't find process 'Rayman2'. Please make sure Rayman is running or try launching this program with Administrator rights.");
                return;
            }

            IntPtr processHandle = Memory.OpenProcess(Memory.PROCESS_WM_READ | Memory.PROCESS_VM_WRITE | Memory.PROCESS_VM_OPERATION, false, process.Id);

            int bytesReadOrWritten = 0; // Required somehow

            byte[] buffer = { 0 };

            Memory.WriteProcessMemory((int)processHandle, OffVoidPointer, buffer, buffer.Length, ref bytesReadOrWritten);
        }

        /// <summary>
        /// Reload the current level in Rayman 2
        /// </summary>
        public void ReloadLevel()
        {
            int processHandle = GetProcessHandle();

            int bytesReadOrWritten = 0;

            byte[] currentBufferLevelName = new byte[1];
            Memory.ReadProcessMemory(processHandle, OffLevelName, currentBufferLevelName, currentBufferLevelName.Length, ref bytesReadOrWritten);

            if (currentBufferLevelName[0] == 0)
            {
                byte[] bufferLevelNameMenu = Encoding.ASCII.GetBytes("menu" + Char.MinValue); // null-terminated
                Memory.WriteProcessMemory(processHandle, OffLevelName, bufferLevelNameMenu, bufferLevelNameMenu.Length, ref bytesReadOrWritten);
                return;
            }

            var buffer = new byte[]
            {
                6
            };

            Memory.WriteProcessMemory(processHandle, OffEngineMode, buffer, buffer.Length, ref bytesReadOrWritten);
        }

        /// <summary>
        /// Change the level in Rayman 2
        /// </summary>
        /// <param name="levelName"></param>
        public void ChangeLevel(string levelName)
        {
            int processHandle = GetProcessHandle();

            int bytesReadOrWritten = 0;

            var buffer = Encoding.ASCII.GetBytes(levelName + Char.MinValue);
            Memory.WriteProcessMemory(processHandle, OffLevelName, buffer, buffer.Length, ref bytesReadOrWritten);

            buffer = new byte[]
            {
                6
            };

            Memory.WriteProcessMemory(processHandle, OffEngineMode, buffer, buffer.Length, ref bytesReadOrWritten);
        }

    }
}
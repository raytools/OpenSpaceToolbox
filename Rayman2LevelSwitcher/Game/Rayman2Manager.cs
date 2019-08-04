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
    public class Rayman2Manager
    {
        #region Constant Values

        private const int Off_engineStructure = 0x500380;
        private const int Off_engineMode = Off_engineStructure + 0x0;
        private const int Off_levelName = Off_engineStructure + 0x1F;
        private const int Off_healthpointer_1 = 0x500584;
        private const int Off_voidpointer = 0x500FAA;

        #endregion

        /// <summary>
        /// Loads the specified position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void LoadPosition(float x, float y, float z)
        {
            int processHandle = GetRayman2ProcessHandle();

            if (processHandle < 0)
                return;

            int bytesReadOrWritten = 0;
            int off_xcoord = Memory.GetPointerPath(processHandle, 0x500560, 0x224, 0x310, 0x34, 0x0) + 0x1ac;
            int off_ycoord = off_xcoord + 4;
            int off_zcoord = off_xcoord + 8;

            byte[] xCoordBuffer = BitConverter.GetBytes(x);
            byte[] yCoordBuffer = BitConverter.GetBytes(y);
            byte[] zCoordBuffer = BitConverter.GetBytes(z);

            Memory.WriteProcessMemory(processHandle, off_xcoord, xCoordBuffer, 4, ref bytesReadOrWritten);
            Memory.WriteProcessMemory(processHandle, off_ycoord, yCoordBuffer, 4, ref bytesReadOrWritten);
            Memory.WriteProcessMemory(processHandle, off_zcoord, zCoordBuffer, 4, ref bytesReadOrWritten);
        }

        /// <summary>
        /// Gets the process handle for Rayman 2
        /// </summary>
        /// <param name="showMessage">Indicates if a message should be shown if the process is not found</param>
        /// <returns></returns>
        public int GetRayman2ProcessHandle(bool showMessage = true)
        {
            var process = GetRayman2Process();

            if (process == null)
            {
                if (showMessage)
                    MessageBox.Show("Couldn't find process 'Rayman2'. Please make sure Rayman is running or try launching this program with Administrator rights.");

                return -1;
            }

            IntPtr processHandle = Memory.OpenProcess(Memory.PROCESS_WM_READ | Memory.PROCESS_VM_WRITE | Memory.PROCESS_VM_OPERATION, false, process.Id);

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

            Memory.ReadProcessMemory(processHandle, Off_levelName, buffer, buffer.Length, ref bytesReadOrWritten);

            string levelName = Encoding.ASCII.GetString(buffer);

            levelName = levelName.Substring(0, levelName.IndexOf((char)0));

            return levelName;
        }

        /// <summary>
        /// Indicates if Rayman 2 if focused
        /// </summary>
        /// <returns>True if it is focused</returns>
        public bool IsRayman2Focused()
        {
            const int nChars = 256;

            StringBuilder Buff = new StringBuilder(nChars);

            IntPtr handle = Memory.GetForegroundWindow();

            if (Memory.GetWindowText(handle, Buff, nChars) <= 0)
                return false;

            return Buff.ToString() == "Rayman II";
        }

        /// <summary>
        /// Indicates if Rayman 2 is paused
        /// </summary>
        /// <returns>True if it is paused</returns>
        public bool IsRayman2Paused()
        {
            int processHandle = GetRayman2ProcessHandle(false);

            if (processHandle < 0)
                return false;

            int bytesReadOrWritten = 0;

            byte[] pausePointerBuffer = new byte[4];

            Memory.ReadProcessMemory(processHandle, Off_voidpointer, pausePointerBuffer, pausePointerBuffer.Length, ref bytesReadOrWritten);

            return BitConverter.ToInt32(pausePointerBuffer, 0) == 1;
        }

        /// <summary>
        /// Activate no health in Rayman 2
        /// </summary>
        public void ActivateNoHealth()
        {
            int processHandle = GetRayman2ProcessHandle();

            if (processHandle < 0)
                return;

            int bytesReadOrWritten = 0;

            byte[] buffer = { 0 };
            byte[] healthPointerBuffer = new byte[4];

            Memory.ReadProcessMemory(processHandle, Off_healthpointer_1, healthPointerBuffer, healthPointerBuffer.Length, ref bytesReadOrWritten);

            int off_healthPointer = BitConverter.ToInt32(healthPointerBuffer, 0) + 0x245;

            Memory.WriteProcessMemory(processHandle, off_healthPointer, buffer, buffer.Length, ref bytesReadOrWritten);
        }

        /// <summary>
        /// Activate void in Rayman 2
        /// </summary>
        public void ActivateVoid()
        {
            Process process = new Rayman2Manager().GetRayman2Process();

            if (process == null)
            {
                MessageBox.Show("Couldn't find process 'Rayman2'. Please make sure Rayman is running or try launching this program with Administrator rights.");
                return;
            }

            IntPtr processHandle = Memory.OpenProcess(Memory.PROCESS_WM_READ | Memory.PROCESS_VM_WRITE | Memory.PROCESS_VM_OPERATION, false, process.Id);

            int bytesReadOrWritten = 0; // Required somehow

            byte[] buffer = { 0 };

            Memory.WriteProcessMemory((int)processHandle, Rayman2Manager.Off_voidpointer, buffer, buffer.Length, ref bytesReadOrWritten);
        }

        /// <summary>
        /// Reload the current level in Rayman 2
        /// </summary>
        public void ReloadLevel()
        {
            var manager = new Rayman2Manager();

            int processHandle = manager.GetRayman2ProcessHandle();

            int bytesReadOrWritten = 0;

            byte[] currentBufferLevelName = new byte[1];
            Memory.ReadProcessMemory(processHandle, Off_levelName, currentBufferLevelName, currentBufferLevelName.Length, ref bytesReadOrWritten);

            if (currentBufferLevelName[0] == 0)
            {
                byte[] bufferLevelNameMenu = Encoding.ASCII.GetBytes("menu" + Char.MinValue); // null-terminated
                Memory.WriteProcessMemory(processHandle, Off_levelName, bufferLevelNameMenu, bufferLevelNameMenu.Length, ref bytesReadOrWritten);
                return;
            }

            var buffer = new byte[]
            {
                6
            };

            Memory.WriteProcessMemory(processHandle, Off_engineMode, buffer, buffer.Length, ref bytesReadOrWritten);
        }

        /// <summary>
        /// Change the level in Rayman 2
        /// </summary>
        /// <param name="levelName"></param>
        public void ChangeLevel(string levelName)
        {
            var manager = new Rayman2Manager();

            int processHandle = manager.GetRayman2ProcessHandle();

            int bytesReadOrWritten = 0;

            var buffer = Encoding.ASCII.GetBytes(levelName + Char.MinValue);
            Memory.WriteProcessMemory(processHandle, Off_levelName, buffer, buffer.Length, ref bytesReadOrWritten);

            buffer = new byte[]
            {
                6
            };

            Memory.WriteProcessMemory(processHandle, Off_engineMode, buffer, buffer.Length, ref bytesReadOrWritten);
        }
    }
}
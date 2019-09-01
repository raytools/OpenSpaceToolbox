using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;

namespace Rayman2LevelSwitcher
{
    /// <summary>
    /// Game manager for Ubisoft OpenSpace engine games.
    /// </summary>
    public abstract class OpenspaceGameManager : GenericGameManager
    {

        #region Protected Properties

        protected abstract int EngineStructurePointer { get; }
        protected abstract int EngineModePointer { get; }
        protected abstract int LevelNamePointer { get; }
        protected abstract int PausedStatePointer { get; }
        protected abstract string[] PossibleProcessNames { get; }

        #endregion

        #region Protected Methods

        protected override (float, float, float) ReadCoordinates()
        {
            throw new NotImplementedException();
        }

        protected override void WriteCoordinates(float x, float y, float z)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Public Methods

        public override int GetProcessHandle(bool showMessage = true)
        {
            Process process = (from name in PossibleProcessNames select Process.GetProcessesByName(name) into p where p.Any() select p.First()).FirstOrDefault();

            if (process == null)
            {
                if (showMessage)
                    MessageBox.Show($"Couldn't find process '{ExecName}'. Please make sure the game is running or try launching this program with Administrator rights.");

                return -1;
            }

            IntPtr processHandle = Memory.OpenProcess(Memory.PROCESS_WM_READ | Memory.PROCESS_VM_WRITE | Memory.PROCESS_VM_OPERATION, false, process.Id);
            process.Dispose();

            return (int)processHandle;
        }

        public override string GetCurrentLevelName()
        {
            int processHandle = GetProcessHandle(false);

            int bytesReadOrWritten = 0;
            byte[] buffer = new byte[16];

            Memory.ReadProcessMemory(processHandle, LevelNamePointer, buffer, buffer.Length, ref bytesReadOrWritten);

            string levelName = Encoding.ASCII.GetString(buffer);
            levelName = levelName.Substring(0, levelName.IndexOf((char)0));

            return levelName;
        }

        public override bool IsGamePaused()
        {
            int processHandle = GetProcessHandle(false);
            if (processHandle < 0)
                return false;

            int bytesReadOrWritten = 0;
            byte[] pausePointerBuffer = new byte[4];

            Memory.ReadProcessMemory(processHandle, PausedStatePointer, pausePointerBuffer, pausePointerBuffer.Length, ref bytesReadOrWritten);

            return BitConverter.ToInt32(pausePointerBuffer, 0) == 1;
        }

        public override bool IsGameFocused()
        {
            const int nChars = 256;

            StringBuilder buff = new StringBuilder(nChars);

            IntPtr handle = Memory.GetForegroundWindow();

            if (Memory.GetWindowText(handle, buff, nChars) <= 0)
                return false;

            return buff.ToString() == WindowName;
        }

        public override void ReloadLevel()
        {
            int processHandle = GetProcessHandle();
            int bytesReadOrWritten = 0;

            byte[] currentBufferLevelName = new byte[1];
            Memory.ReadProcessMemory(processHandle, LevelNamePointer, currentBufferLevelName, currentBufferLevelName.Length, ref bytesReadOrWritten);

            if (currentBufferLevelName[0] == 0) return;

            var buffer = new byte[] {6};

            Memory.WriteProcessMemory(processHandle, LevelNamePointer, buffer, buffer.Length, ref bytesReadOrWritten);
        }

        public override void ChangeLevel(string levelName)
        {
            int processHandle = GetProcessHandle();
            int bytesReadOrWritten = 0;

            var buffer = Encoding.ASCII.GetBytes(levelName + char.MinValue);
            Memory.WriteProcessMemory(processHandle, LevelNamePointer, buffer, buffer.Length, ref bytesReadOrWritten);

            buffer = new byte[] {6};

            Memory.WriteProcessMemory(processHandle, LevelNamePointer, buffer, buffer.Length, ref bytesReadOrWritten);
        }

        #endregion
    }
}

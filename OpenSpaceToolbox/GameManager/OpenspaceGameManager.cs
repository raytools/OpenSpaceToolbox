using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;

namespace OpenSpaceToolbox
{
    /// <summary>
    /// Game manager for Ubisoft OpenSpace engine games.
    /// </summary>
    public abstract class OpenspaceGameManager : GenericGameManager
    {

       #region Protected Properties

      protected int EngineStructurePointer { get; set; }
        protected int EngineModePointer { get; set; }
        protected int LevelNamePointer { get; set; }
        protected int PausedStatePointer { get; set; }
        protected int GhostModePointer { get; set; }
        protected int PlayerCoordinatesBasePointer { get; set; }
        protected int[] PlayerCoordinatesOffsets { get; set; }
        protected string[] PossibleProcessNames { get; set; }

        #endregion

        #region Protected Methods

        protected override (float, float, float) ReadPlayerCoordinates() =>
            ReadCoordinates(PlayerCoordinatesBasePointer, PlayerCoordinatesOffsets);

        protected override void WritePlayerCoordinates(float x, float y, float z) =>
            WriteCoordinates(x, y, z, PlayerCoordinatesBasePointer, PlayerCoordinatesOffsets);

        protected override string GetCurrentLevelName()
        {
            int processHandle = GetProcessHandle(false);

            int bytesReadOrWritten = 0;
            byte[] buffer = new byte[16];

            Memory.ReadProcessMemory(processHandle, LevelNamePointer, buffer, buffer.Length, ref bytesReadOrWritten);

            string levelName = Encoding.ASCII.GetString(buffer);
            levelName = levelName.Substring(0, levelName.IndexOf((char)0));

            return levelName;
        }

        protected override void ChangeLevel(string levelName)
        {
           base.ChangeLevel(levelName);
           int processHandle = GetProcessHandle();
            int bytesReadOrWritten = 0;

            var buffer = Encoding.ASCII.GetBytes(levelName + char.MinValue);
            Memory.WriteProcessMemory(processHandle, LevelNamePointer, buffer, buffer.Length, ref bytesReadOrWritten);

            buffer = new byte[] {6};

            Memory.WriteProcessMemory(processHandle, EngineModePointer, buffer, buffer.Length, ref bytesReadOrWritten);

      }

        #endregion

        #region Public Methods

        public (float, float, float) ReadCoordinates(int baseAddress, params int[] offsets)
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

        public void WriteCoordinates(float x, float y, float z, int baseAddress, params int[] offsets)
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

            if ((int)processHandle == 0) {
                if (showMessage) {
                    MessageBox.Show($"Error opening process '{ExecName}'. Try launching this program with Administrator rights.");
                }
            }

            return (int)processHandle;
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
           base.ReloadLevel();

         int processHandle = GetProcessHandle();
            int bytesReadOrWritten = 0;

            byte[] currentBufferLevelName = new byte[1];
            Memory.ReadProcessMemory(processHandle, LevelNamePointer, currentBufferLevelName, currentBufferLevelName.Length, ref bytesReadOrWritten);

            if (currentBufferLevelName[0] == 0) return;

            var buffer = new byte[] {6};

            Memory.WriteProcessMemory(processHandle, EngineModePointer, buffer, buffer.Length, ref bytesReadOrWritten);

        }

        public override void LoadOffsetLevel(int offset)
        {
            int processHandle = GetProcessHandle();
            if (processHandle < 0)
                return;

            string levelName = CurrentLevel;
            var lvls = Levels.ToList();
            int currentIndex = lvls.FindIndex(x => string.Equals(x.FileName, levelName, StringComparison.CurrentCultureIgnoreCase));

            if (currentIndex < 0)
                return;

            int newIndex = currentIndex + offset;

            if (newIndex < 0)
                newIndex = lvls.Count - 1;

            if (newIndex >= lvls.Count)
                newIndex = 0;

            string levelToLoad = lvls[newIndex].FileName;

            CurrentLevel = levelToLoad;
        }

        public void WriteGhostMode(bool enable)
        {
           var processHandle = GetProcessHandle();
           if (processHandle < 0)
              return;

           var buffer = BitConverter.GetBytes(enable ? 1 : 0);

           var bytesReadOrWritten = 0;
           Memory.WriteProcessMemory(processHandle, GhostModePointer, buffer, 1, ref bytesReadOrWritten);
        }

      #endregion
   }
}

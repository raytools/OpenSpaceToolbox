using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rayman2LevelSwitcher {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window {

        const int off_engineStructure = 0x500380;
        const int off_engineMode = off_engineStructure + 0x0;
        const int off_levelName = off_engineStructure + 0x1F;
        const int off_healthpointer_1 = 0x500584;
        const int off_voidpointer = 0x500FAA;

        float savedXCoord = 0;
        float savedYCoord = 0;
        float savedZCoord = 0;

        String[] randomLevels = new String[] { "Learn_10", "Learn_30", "Learn_31", "Bast_20", "Bast_22", "Learn_60", "Ski_10", "Ski_60", "Chase_10", "Ly_10", "Chase_22", "Water_10", "Water_20", "Rodeo_10", "Rodeo_40", "Vulca_10", "Vulca_20", "Rodeo_60", "GLob_30", "GLob_10", "GLob_20", "Whale_00", "Whale_05", "Whale_10", "Plum_00", "Plum_10", "Bast_10", "Cask_10", "Cask_30", "Nave_10", "Nave_15", "Nave_20", "Seat_10", "Seat_11", "Earth_10", "Earth_20", "Ly_20", "Earth_30", "Helic_10", "Helic_20", "Helic_30", "Plum_20", "Morb_00", "Morb_10", "Morb_20", "Learn_40", "ile_10", "Mine_10", "Boat01", "Boat02", "Astro_00", "Astro_10", "Rhop_10" };

        public MainWindow()
        {
            InitializeComponent();
            SetupKeyboardHooks();
            chk_hotkeys.ToolTip = "Enables hotkeys:#R for random level#K for previous level#L for next level#P to save position#O to load position".Replace("#", Environment.NewLine);
        }

        private void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            //Debug.WriteLine(e.KeyboardData.VirtualCode);
                        
            if (!chk_hotkeys.IsChecked.Value) {
                e.Handled = false;
                return;
            }

            // Only handle 
            if (e.KeyboardState != GlobalKeyboardHook.KeyboardState.KeyDown) {
                return;
            }

            if (e.KeyboardData.VirtualCode == 0x4F) { // O to load position
                btn_loadpos_Click(null, null);
                return;
            }

            if (e.KeyboardData.VirtualCode == 0x50) { // P to save position
                btn_savepos_Click(null, null);
                return;
            }

            if (e.KeyboardData.VirtualCode == 0x4B) { // K for previous level
                LoadPreviousLevel();
                return;
            }

            if (e.KeyboardData.VirtualCode == 0x4C) { // L for next level
                LoadNextLevel();
                return;
            }

            if (e.KeyboardData.VirtualCode == 0x52) { // R for random level
                btn_random_Click(null, null);
                return;
            }
        }

        private void LoadNextLevel()
        {
            LoadOffsetLevel(1);
        }

        private void LoadPreviousLevel()
        {
            LoadOffsetLevel(-1);
        }

        private string GetCurrentLevelName(int processHandle)
        {
            int bytesReadOrWritten = 0;
            byte[] buffer = new byte[16];
            Memory.ReadProcessMemory((int)processHandle, off_levelName, buffer, buffer.Length, ref bytesReadOrWritten);
            string levelName = Encoding.ASCII.GetString(buffer);
            levelName = levelName.Substring(0, levelName.IndexOf((char)0)); // remove after null terminator
            return levelName;
        }

        private void LoadOffsetLevel(int offset)
        {
            int processHandle = GetRayman2ProcessHandle();
            if (processHandle<0) {
                return;
            }

            int bytesReadOrWritten = 0;
            string levelName = GetCurrentLevelName(processHandle);

            int currentIndex = Array.FindIndex(randomLevels, (name) => name.ToLower() == levelName.ToLower());
            if (currentIndex < 0) {
                currentIndex = 0;
            }
            if (currentIndex>=0) {
                int newIndex = currentIndex + offset;
                if (newIndex < 0) { newIndex = randomLevels.Length - 1; }
                if (newIndex >= randomLevels.Length) { newIndex = 0; }

                string levelToLoad = randomLevels[newIndex];
                ChangeLevel(levelToLoad);
            }
        }

        private GlobalKeyboardHook _globalKeyboardHook;

        public void SetupKeyboardHooks()
        {
            _globalKeyboardHook = new GlobalKeyboardHook();
            _globalKeyboardHook.KeyboardPressed += OnKeyPressed;
        }

        private int GetRayman2ProcessHandle()
        {
            Process process;
            if (Process.GetProcessesByName("Rayman2").Length > 0) {
                process = Process.GetProcessesByName("Rayman2")[0];
            } else if (Process.GetProcessesByName("Rayman2.exe").Length > 0) {
                process = Process.GetProcessesByName("Rayman2.exe")[0];
            } else if (Process.GetProcessesByName("Rayman2.exe.noshim").Length > 0) {
                process = Process.GetProcessesByName("Rayman2.exe.noshim")[0];
            } else {
                MessageBox.Show("Couldn't find process 'Rayman2'. Please make sure Rayman is running or try launching this program with Administrator rights.");
                return -1;
            }
            IntPtr processHandle = Memory.OpenProcess(Memory.PROCESS_WM_READ | Memory.PROCESS_VM_WRITE | Memory.PROCESS_VM_OPERATION, false, process.Id);
            return (int)processHandle;
        }

        private void btn_Switch_Click(object sender, RoutedEventArgs e)
        {
            if (treeView_levels.SelectedItem!=null && treeView_levels.SelectedItem is TreeViewItem) {
                TreeViewItem selectedItem = treeView_levels.SelectedItem as TreeViewItem;
                if (selectedItem.Name != "") {
                    ChangeLevel(selectedItem.Name);
                }
            }
        }

        private void btn_Reload_Click(object sender, RoutedEventArgs e)
        {
            RefreshLevel();
        }

        private void RefreshLevel()
        {
            ChangeLevel("");
        }

        private void ChangeLevel(string levelName)
        {
            int processHandle = GetRayman2ProcessHandle();

            int bytesReadOrWritten = 0; // Required somehow
            byte[] buffer; // Generic buffer

            if (levelName!="") {
                buffer = Encoding.ASCII.GetBytes(levelName + char.MinValue); // null-terminated
                Memory.WriteProcessMemory((int)processHandle, off_levelName, buffer, buffer.Length, ref bytesReadOrWritten);
            } else {
                byte[] currentBufferLevelName = new byte[1];
                Memory.ReadProcessMemory((int)processHandle, off_levelName, currentBufferLevelName, currentBufferLevelName.Length, ref bytesReadOrWritten);
                if (currentBufferLevelName[0]==0) {
                    byte[] bufferLevelNameMenu = Encoding.ASCII.GetBytes("menu" + char.MinValue); // null-terminated
                    Memory.WriteProcessMemory((int)processHandle, off_levelName, bufferLevelNameMenu, bufferLevelNameMenu.Length, ref bytesReadOrWritten);
                    return;
                }
            }

            buffer = new byte[] { 6 };
            Memory.WriteProcessMemory((int)processHandle, off_engineMode, buffer, buffer.Length, ref bytesReadOrWritten);

            Console.ReadLine();
        }

        private void treeView_levels_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btn_Switch_Click(sender, e);
        }

        private void btn_random_Click(object sender, RoutedEventArgs e)
        {
            string level = randomLevels[new Random().Next(randomLevels.Length)];
            ChangeLevel(level);
        }

        private void btn_void_Click(object sender, RoutedEventArgs e)
        {
            Process process;
            if (Process.GetProcessesByName("Rayman2").Length > 0) {
                process = Process.GetProcessesByName("Rayman2")[0];
            } else if (Process.GetProcessesByName("Rayman2.exe").Length > 0) {
                process = Process.GetProcessesByName("Rayman2.exe")[0];
            } else if (Process.GetProcessesByName("Rayman2.exe.noshim").Length > 0) {
                process = Process.GetProcessesByName("Rayman2.exe.noshim")[0];
            } else {
                MessageBox.Show("Couldn't find process 'Rayman2'. Please make sure Rayman is running or try launching this program with Administrator rights.");
                return;
            }
            IntPtr processHandle = Memory.OpenProcess(Memory.PROCESS_WM_READ | Memory.PROCESS_VM_WRITE | Memory.PROCESS_VM_OPERATION, false, process.Id);

            int bytesReadOrWritten = 0; // Required somehow

            byte[] buffer = new byte[] { 0 };

            Memory.WriteProcessMemory((int)processHandle, off_voidpointer, buffer, buffer.Length, ref bytesReadOrWritten);
        }

        private void btn_zerohp_Click(object sender, RoutedEventArgs e)
        {
            int processHandle = GetRayman2ProcessHandle();
            if (processHandle<0) { return; }

            int bytesReadOrWritten = 0; // Required somehow

            byte[] buffer = new byte[] { 0 };
            byte[] healthPointerBuffer = new byte[4];
            Memory.ReadProcessMemory((int)processHandle, off_healthpointer_1, healthPointerBuffer, healthPointerBuffer.Length, ref bytesReadOrWritten);
            int off_healthPointer = BitConverter.ToInt32(healthPointerBuffer, 0) + 0x245;

            Memory.WriteProcessMemory((int)processHandle, off_healthPointer, buffer, buffer.Length, ref bytesReadOrWritten);
        }

        private void btn_loadpos_Click(object sender, RoutedEventArgs e)
        {
            int processHandle = GetRayman2ProcessHandle();
            if (processHandle < 0) { return; }

            int bytesReadOrWritten = 0;
            int off_xcoord = Memory.GetPointerPath((int)processHandle, 0x500560, 0x224, 0x310, 0x34, 0x0) + 0x1ac;
            int off_ycoord = off_xcoord + 4;
            int off_zcoord = off_xcoord + 8;

            byte[] xCoordBuffer = BitConverter.GetBytes(savedXCoord);
            byte[] yCoordBuffer = BitConverter.GetBytes(savedYCoord);
            byte[] zCoordBuffer = BitConverter.GetBytes(savedZCoord);
            Memory.WriteProcessMemory(processHandle, off_xcoord, xCoordBuffer, 4, ref bytesReadOrWritten);
            Memory.WriteProcessMemory(processHandle, off_ycoord, yCoordBuffer, 4, ref bytesReadOrWritten);
            Memory.WriteProcessMemory(processHandle, off_zcoord, zCoordBuffer, 4, ref bytesReadOrWritten);
        }

        private void btn_savepos_Click(object sender, RoutedEventArgs e)
        {
            int processHandle = GetRayman2ProcessHandle();
            if (processHandle < 0) { return; }

            int bytesReadOrWritten = 0;
            int off_xcoord = Memory.GetPointerPath((int)processHandle, 0x500560, 0x224, 0x310, 0x34, 0x0) + 0x1ac;
            int off_ycoord = off_xcoord + 4;
            int off_zcoord = off_xcoord + 8;

            byte[] xCoordBuffer = new byte[4];
            byte[] yCoordBuffer = new byte[4];
            byte[] zCoordBuffer = new byte[4];
            Memory.ReadProcessMemory(processHandle, off_xcoord, xCoordBuffer, 4, ref bytesReadOrWritten);
            Memory.ReadProcessMemory(processHandle, off_ycoord, yCoordBuffer, 4, ref bytesReadOrWritten);
            Memory.ReadProcessMemory(processHandle, off_zcoord, zCoordBuffer, 4, ref bytesReadOrWritten);

            savedXCoord = BitConverter.ToSingle(xCoordBuffer, 0);
            savedYCoord = BitConverter.ToSingle(yCoordBuffer, 0);
            savedZCoord = BitConverter.ToSingle(zCoordBuffer, 0);
        }


        public void Dispose()
        {
            _globalKeyboardHook?.Dispose();
        }
    }
}

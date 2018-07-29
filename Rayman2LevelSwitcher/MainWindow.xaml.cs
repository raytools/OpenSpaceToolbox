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

        String[] randomLevels = new String[] { "Learn_10", "Learn_30", "Learn_31", "Bast_20", "Bast_22", "Learn_60", "Ski_10", "Vulca_10", "Vulca_20", "Ski_60", "Chase_10", "Ly_10", "Chase_22", "Rodeo_10", "Rodeo_40", "Rodeo_60", "Water_10", "Water_20", "GLob_30", "GLob_10", "GLob_20", "Whale_00", "Whale_05", "Whale_10", "Plum_00", "Plum_10", "Bast_10", "Cask_10", "Cask_30", "Nave_10", "Nave_15", "Nave_20", "Seat_10", "Seat_11", "Earth_10", "Earth_20", "Ly_20", "Earth_30", "Helic_10", "Helic_20", "Helic_30", "Plum_20", "Morb_00", "Morb_10", "Morb_20", "Learn_40", "ile_10", "Mine_10", "Boat01", "Boat02", "Astro_00", "Astro_10", "Rhop_10" };

        public MainWindow()
        {
            InitializeComponent();
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

            if (levelName!="") {
                byte[] bufferLevelName = Encoding.ASCII.GetBytes(levelName + char.MinValue); // null-terminated
                Memory.WriteProcessMemory((int)processHandle, off_levelName, bufferLevelName, bufferLevelName.Length, ref bytesReadOrWritten);
            } else {
                byte[] currentBufferLevelName = new byte[1];
                Memory.ReadProcessMemory((int)processHandle, off_levelName, currentBufferLevelName, currentBufferLevelName.Length, ref bytesReadOrWritten);
                if (currentBufferLevelName[0]==0) {
                    byte[] bufferLevelNameMenu = Encoding.ASCII.GetBytes("menu" + char.MinValue); // null-terminated
                    Memory.WriteProcessMemory((int)processHandle, off_levelName, bufferLevelNameMenu, bufferLevelNameMenu.Length, ref bytesReadOrWritten);
                    return;
                }
            }

            byte[] buffer = new byte[] { 6 };
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
    }
}

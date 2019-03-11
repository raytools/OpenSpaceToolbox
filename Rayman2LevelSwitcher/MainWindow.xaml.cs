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
using System.IO;
using System.Xml.Linq;
using System.Globalization;
using System.Threading;
using System.Windows.Threading;

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
        // Don't change order in realLevelNames and allLevels!
        String[] allLevels = new String[] { "Jail_20", "Learn_10", "Learn_30", "Learn_31", "Bast_20", "Bast_22", "Learn_60", "Ski_10", "Ski_60", "Chase_10", "Ly_10", "Chase_22", "Water_10", "Water_20", "Rodeo_10", "Rodeo_40", "Vulca_10", "Vulca_20", "Rodeo_60", "GLob_30", "GLob_10", "GLob_20", "Whale_00", "Whale_05", "Whale_10", "Plum_00", "Plum_20", "Plum_10", "Bast_10", "Cask_10", "Cask_30", "Nave_10", "Nave_15", "Nave_20", "Seat_10", "Seat_11", "Earth_10", "Earth_20", "Ly_20", "Earth_30", "Helic_10", "Helic_20", "Helic_30", "Morb_00", "Morb_10", "Morb_20", "Learn_40", "ile_10", "Mine_10", "Boat01", "Boat02", "Astro_00", "Astro_10", "Rhop_10" };
        String[] realLevelNames = new String[] { "Jail", "Woods of Light", "Fairy Glade 1", "Fairy Glade 2", "Fairy Glade 3", "Fairy Glade 4", "Fairy Glade 5", "The Marshes of Awakening 1", "The Marshes of Awakening 2", "The Bayou 1", "The Walk of Life", "The Bayou 2", "The Sanctuary of Water and Ice 1", "The Sanctuary of Water and Ice 2", "The Menhir Hills 1", "The Menhir Hills 2", "The Cave of Bad Dreams 1", "The Cave of Bad Dreams 2", "The Menhir Hills 3", "The Canopy 1", "The Canopy 2", "The Canopy 3", "Whale Bay 1", "Whale Bay 2", "Whale Bay 3", "The Sanctuary of Stone and Fire 1", "The Sanctuary of Stone and Fire 1-B", "The Sanctuary of Stone and Fire 2", "The Echoing Caves 1", "The Echoing Caves 2", "The Echoing Caves 3", "The Precipice 1", "The Precipice 2", "The Precipice 3", "The Top of the World 1", "The Top of the World 2", "The Sanctuary of Rock and Lava 1", "The Sanctuary of Rock and Lava 2", "The Walk of Power", "The Sanctuary of Rock and Lava 3", "Beneath the Sanctuary of Rock and Lava 1", "Beneath the Sanctuary of Rock and Lava 2", "Beneath the Sanctuary of Rock and Lava 3", "Tomb of the Ancients 1", "Tomb of the Ancients 2", "Tomb of the Ancients 3", "The Iron Mountains 1", "The Iron Mountains 2", "The Iron Mountains 3 ( ͡° ͜ʖ ͡°)", "The Prison Ship 1", "The Prison Ship 2", "The Prison Ship 3", "The Prison Ship 4", "The Crow's Nest" };

        String[] cutscenesAndExtras = new String[] { "menu", "mapmonde", "bonux", "raycap", "nego_10", "batam_10", "bast_09", "ball", "batam_20", "end_10", "staff_10", "poloc_10", "poloc_20", "poloc_30", "poloc_40", "jail_10" };
        string bookmarkFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Rayman2LevelBookmarks.xml";
        public static string renameBookmarkName;
        string bookmarkCurrentLevel;

        Thread bgThread;

        public MainWindow()
        {
            InitializeComponent();
            SetupKeyboardHooks();
            chk_hotkeys.ToolTip = "Enables hotkeys:#R for random level#K for previous level#L for next level#P to save position#O to load position#B to add bookmark".Replace("#", Environment.NewLine);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bgThread = new Thread(BookmarkUpdater);
            bgThread.IsBackground = true;
            bgThread.Start();
        }

        public void BookmarkUpdater()
        {
            while (true)
            {
                //Update bookmarks every second
                Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(() => UpdateBookmarks() ));
                Thread.Sleep(1000);
            }
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

            if (e.KeyboardData.VirtualCode == 0x42) { // B to add bookmark
                btn_addbookmark_Click(null, null);
                return;
            }
        }

        private void LoadNextLevel()
        {
            LoadOffsetLevel(1);
            UpdateBookmarks(true);
        }

        private void LoadPreviousLevel()
        {
            LoadOffsetLevel(-1);
            UpdateBookmarks(true);
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

        private int GetRayman2ProcessHandle(bool alert = true)
        {
            Process process;
            if (Process.GetProcessesByName("Rayman2").Length > 0) {
                process = Process.GetProcessesByName("Rayman2")[0];
            } else if (Process.GetProcessesByName("Rayman2.exe").Length > 0) {
                process = Process.GetProcessesByName("Rayman2.exe")[0];
            } else if (Process.GetProcessesByName("Rayman2.exe.noshim").Length > 0) {
                process = Process.GetProcessesByName("Rayman2.exe.noshim")[0];
            } else {
                if (alert)
                {
                    MessageBox.Show("Couldn't find process 'Rayman2'. Please make sure Rayman is running or try launching this program with Administrator rights.");
                }
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
                    UpdateBookmarks(true);
                }
            }
        }

        private void btn_Reload_Click(object sender, RoutedEventArgs e)
        {
            RefreshLevel();
            UpdateBookmarks(true);
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
            UpdateBookmarks(true);
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

        private void exp_bookmarkmenu_Collapsed(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.MinWidth -= exp_bookmarkmenu.Width - 50;
            Application.Current.MainWindow.Width -= exp_bookmarkmenu.Width - 50;
            exp_bookmarkmenu.Margin = new Thickness(exp_bookmarkmenu.Margin.Left, exp_bookmarkmenu.Margin.Top, exp_bookmarkmenu.Margin.Right - exp_bookmarkmenu.Width + 50, exp_bookmarkmenu.Margin.Bottom);

            treeView_levels.Margin = new Thickness(treeView_levels.Margin.Left, treeView_levels.Margin.Top, treeView_levels.Margin.Right - exp_bookmarkmenu.Width + 50, treeView_levels.Margin.Bottom);
            btn_random.Margin = new Thickness(btn_random.Margin.Left, btn_random.Margin.Top, btn_random.Margin.Right - exp_bookmarkmenu.Width + 50, btn_random.Margin.Bottom);
            btn_zerohp.Margin = new Thickness(btn_zerohp.Margin.Left, btn_zerohp.Margin.Top, btn_zerohp.Margin.Right - exp_bookmarkmenu.Width + 50, btn_zerohp.Margin.Bottom);
            btn_void.Margin = new Thickness(btn_void.Margin.Left, btn_void.Margin.Top, btn_void.Margin.Right - exp_bookmarkmenu.Width + 50, btn_void.Margin.Bottom);
            chk_hotkeys.Margin = new Thickness(chk_hotkeys.Margin.Left, chk_hotkeys.Margin.Top, chk_hotkeys.Margin.Right - exp_bookmarkmenu.Width + 50, chk_hotkeys.Margin.Bottom);
        }

        private void exp_bookmarkmenu_Expanded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Width += exp_bookmarkmenu.Width - 50;
            Application.Current.MainWindow.MinWidth += exp_bookmarkmenu.Width - 50;
            exp_bookmarkmenu.Margin = new Thickness(exp_bookmarkmenu.Margin.Left, exp_bookmarkmenu.Margin.Top, exp_bookmarkmenu.Margin.Right + exp_bookmarkmenu.Width - 50, exp_bookmarkmenu.Margin.Bottom);

            treeView_levels.Margin = new Thickness(treeView_levels.Margin.Left, treeView_levels.Margin.Top, treeView_levels.Margin.Right + exp_bookmarkmenu.Width - 50, treeView_levels.Margin.Bottom);
            btn_random.Margin = new Thickness(btn_random.Margin.Left, btn_random.Margin.Top, btn_random.Margin.Right + exp_bookmarkmenu.Width - 50, btn_random.Margin.Bottom);
            btn_zerohp.Margin = new Thickness(btn_zerohp.Margin.Left, btn_zerohp.Margin.Top, btn_zerohp.Margin.Right + exp_bookmarkmenu.Width - 50, btn_zerohp.Margin.Bottom);
            btn_void.Margin = new Thickness(btn_void.Margin.Left, btn_void.Margin.Top, btn_void.Margin.Right + exp_bookmarkmenu.Width - 50, btn_void.Margin.Bottom);
            chk_hotkeys.Margin = new Thickness(chk_hotkeys.Margin.Left, chk_hotkeys.Margin.Top, chk_hotkeys.Margin.Right + exp_bookmarkmenu.Width - 50, chk_hotkeys.Margin.Bottom);
        }

        private void CreateBookmarkFile()
        {
            new XDocument(new XElement("Rayman2LevelBookmarks")).Save(bookmarkFile);
        }

        private void AddBookmark()
        {
            int processHandle = GetRayman2ProcessHandle();
            if (processHandle < 0) { return; }

            string levelname = GetCurrentLevelName(processHandle).ToLower();

            if (cutscenesAndExtras.Contains(levelname) || String.IsNullOrEmpty(levelname))
            {
                return;
            }

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

            if (!File.Exists(bookmarkFile)) { CreateBookmarkFile(); }

            var xml = XDocument.Load(bookmarkFile);

            if (xml.Element("Rayman2LevelBookmarks").Elements(levelname).Any() == false)
            {
                xml.Element("Rayman2LevelBookmarks").Add(new XElement(levelname));
            }

            string newBookmarkName = "Bookmark";
            int newBookmarkSuffix = 1;

            isUniqueCheck:
            foreach (XElement element in xml.Element("Rayman2LevelBookmarks").Element(levelname).Elements("Bookmark"))
            {
                if (element.Element("Name").Value == newBookmarkName + " " + newBookmarkSuffix)
                {
                    newBookmarkSuffix++;
                    goto isUniqueCheck;
                }
            }



            xml.Element("Rayman2LevelBookmarks").Element(levelname).Add(
                new XElement("Bookmark",
                new XElement("Name", newBookmarkName + " " + newBookmarkSuffix),
                new XElement("X", BitConverter.ToSingle(xCoordBuffer, 0)),
                new XElement("Y", BitConverter.ToSingle(yCoordBuffer, 0)),
                new XElement("Z", BitConverter.ToSingle(zCoordBuffer, 0))));

            xml.Save(bookmarkFile);
            UpdateBookmarks(true);
        }

        private void UpdateBookmarks(bool forceUpdate = false)
        {
            int processHandle = GetRayman2ProcessHandle(false);
            if (processHandle < 0)
            {
                txtblock_currentbookmarklevel.Text = "No level is currently loaded";
                listbox_bookmarklist.Items.Clear();
                return;
            }

            if (!File.Exists(bookmarkFile))
            {
                listbox_bookmarklist.Items.Clear();
            }

            string levelname = GetCurrentLevelName(processHandle).ToLower();

            if (levelname == bookmarkCurrentLevel && !forceUpdate || String.IsNullOrEmpty(levelname))
            {
                return;
            }

            bookmarkCurrentLevel = levelname;

            if (cutscenesAndExtras.Contains(levelname))
            {
                txtblock_currentbookmarklevel.Text = "No level is currently loaded";
                listbox_bookmarklist.Items.Clear();
                return;
            }
            try
            {
                int indexOfSubLevel = levelname.IndexOf('$');
                if (indexOfSubLevel > 0)
                {
                    txtblock_currentbookmarklevel.Text = realLevelNames.ElementAt(Array.FindIndex(allLevels, row => row.ToLower().Contains(levelname.ToLower().Substring(0, indexOfSubLevel))));
                }

                else
                {
                    txtblock_currentbookmarklevel.Text = realLevelNames.ElementAt(Array.FindIndex(allLevels, row => row.ToLower().Contains(levelname.ToLower())));
                }
            }

            catch (System.ArgumentOutOfRangeException)
            {
                txtblock_currentbookmarklevel.Text = levelname;
            }
            listbox_bookmarklist.Items.Clear();

            if (!File.Exists(bookmarkFile))
            {
                return;
            }

            var xml = XDocument.Load(bookmarkFile);


            if (xml.Element("Rayman2LevelBookmarks").Elements(levelname).Count() == 0)
            {
                return;
            }

            foreach (XElement bookmark in xml.Element("Rayman2LevelBookmarks").Element(levelname).Elements("Bookmark"))
            {
                listbox_bookmarklist.Items.Add(bookmark.Element("Name").Value);
            }
        }

        private void LoadBookmark()
        {
            int processHandle = GetRayman2ProcessHandle();
            if (processHandle < 0 || !File.Exists(bookmarkFile) || listbox_bookmarklist.SelectedItem == null || listbox_bookmarklist.SelectedItems.Count > 1)
            {
                return;
            }

            string levelname = GetCurrentLevelName(processHandle).ToLower();

            var xml = XDocument.Load(bookmarkFile);

            foreach (XElement element in xml.Element("Rayman2LevelBookmarks").Element(levelname).Elements("Bookmark"))
            {
                if (element.Element("Name").Value == listbox_bookmarklist.SelectedItem.ToString())
                {
                    savedXCoord = float.Parse(element.Element("X").Value, CultureInfo.InvariantCulture.NumberFormat);
                    savedYCoord = float.Parse(element.Element("Y").Value, CultureInfo.InvariantCulture.NumberFormat);
                    savedZCoord = float.Parse(element.Element("Z").Value, CultureInfo.InvariantCulture.NumberFormat);
                    btn_loadpos_Click(null, null);
                    return;
                }
            }
        }

        private void RenameBookmark()
        {
            renameBookmarkName = "";
            bool hotkeysIsChecked = chk_hotkeys.IsChecked ?? true;
            int processHandle = GetRayman2ProcessHandle(false);
            if (processHandle < 0 || !File.Exists(bookmarkFile) || listbox_bookmarklist.SelectedItem == null || listbox_bookmarklist.SelectedItems.Count > 1)
            {
                return;
            }

            string levelname = GetCurrentLevelName(processHandle).ToLower();

            int selectedIndex = listbox_bookmarklist.SelectedIndex;

            RenameDialog rename = new RenameDialog();
            rename.Left = Application.Current.MainWindow.Left + (Application.Current.MainWindow.Width / 2) - (rename.Width / 2);
            rename.Top = Application.Current.MainWindow.Top + (Application.Current.MainWindow.Height / 2) - (rename.Height / 2);
            rename.txtbox_name.Text = listbox_bookmarklist.SelectedItem.ToString();
            chk_hotkeys.IsChecked = false;
            rename.ShowDialog();
            chk_hotkeys.IsChecked = hotkeysIsChecked;
            if (!String.IsNullOrEmpty(renameBookmarkName) && listbox_bookmarklist.SelectedItem.ToString() != renameBookmarkName)
            {
                var xml = XDocument.Load(bookmarkFile);
                foreach (XElement element in xml.Element("Rayman2LevelBookmarks").Element(levelname).Elements("Bookmark"))
                {
                    if (element.Element("Name").Value == renameBookmarkName)
                    {
                        MessageBox.Show("A bookmark with that name already exists!" + Environment.NewLine + "Choose another name.");
                        return;
                    }
                }

                foreach (XElement element in xml.Element("Rayman2LevelBookmarks").Element(levelname).Elements("Bookmark"))
                {
                    if (element.Element("Name").Value == listbox_bookmarklist.SelectedItem.ToString())
                    {
                        element.Element("Name").Value = renameBookmarkName;
                    }
                }
                xml.Save(bookmarkFile);
                UpdateBookmarks(true);
                listbox_bookmarklist.SelectedIndex = selectedIndex;
                listbox_bookmarklist.Focus();
            }
        }

        private void DeleteBookmark()
        {
            int processHandle = GetRayman2ProcessHandle(false);
            if (processHandle < 0 || !File.Exists(bookmarkFile) || listbox_bookmarklist.SelectedItem == null)
            {
                return;
            }

            string levelname = GetCurrentLevelName(processHandle).ToLower();
            var xml = XDocument.Load(bookmarkFile);

            nextfile:
            foreach (XElement element in xml.Element("Rayman2LevelBookmarks").Element(levelname).Elements("Bookmark"))
            {
                if (listbox_bookmarklist.SelectedItems.Contains(element.Element("Name").Value))
                {
                    element.Remove();
                    goto nextfile;
                }
            }
            
            int selectedIndex = listbox_bookmarklist.SelectedIndex - 1;
            if (selectedIndex < 0)
            {
                selectedIndex = 0;
            }
            xml.Save(bookmarkFile);
            UpdateBookmarks(true);
            listbox_bookmarklist.SelectedIndex = selectedIndex;
            listbox_bookmarklist.Focus();
        }

        private void MoveBookmarkUp()
        {
            int processHandle = GetRayman2ProcessHandle(false);
            if (processHandle < 0 || !File.Exists(bookmarkFile) || listbox_bookmarklist.SelectedItem == null)
            {
                return;
            }

            string levelname = GetCurrentLevelName(processHandle).ToLower();

            var xml = XDocument.Load(bookmarkFile);

            XElement currentBookmark = xml.Element("Rayman2LevelBookmarks");

            foreach (XElement bookmark in xml.Element("Rayman2LevelBookmarks").Element(levelname).Elements("Bookmark"))
            {
                if (bookmark.Element("Name").Value == listbox_bookmarklist.SelectedItem.ToString())
                {
                    currentBookmark = bookmark;
                }
            }
            if (currentBookmark == xml.Element("Rayman2LevelBookmarks"))
            {
                return;
            }

            XNode previousBookmark = currentBookmark.PreviousNode;
            while (previousBookmark != null && !(previousBookmark is XElement))
            {
                previousBookmark = previousBookmark.PreviousNode;
            }
            if (previousBookmark == null)
            {
                return;
            }
            currentBookmark.Remove();
            previousBookmark.AddBeforeSelf(currentBookmark);
            xml.Save(bookmarkFile);
            int selectedIndex = listbox_bookmarklist.SelectedIndex - 1;
            UpdateBookmarks(true);
            listbox_bookmarklist.SelectedIndex = selectedIndex;
        }

        private void MoveBookmarkDown()
        {
            int processHandle = GetRayman2ProcessHandle(false);
            if (processHandle < 0 || !File.Exists(bookmarkFile) || listbox_bookmarklist.SelectedItem == null)
            {
                return;
            }

            string levelname = GetCurrentLevelName(processHandle).ToLower();

            var xml = XDocument.Load(bookmarkFile);

            XElement currentBookmark = xml.Element("Rayman2LevelBookmarks");

            foreach (XElement bookmark in xml.Element("Rayman2LevelBookmarks").Element(levelname).Elements("Bookmark"))
            {
                if (bookmark.Element("Name").Value == listbox_bookmarklist.SelectedItem.ToString())
                {
                    currentBookmark = bookmark;
                }
            }
            if (currentBookmark == xml.Element("Rayman2LevelBookmarks"))
            {
                return;
            }

            XNode nextBookmark = currentBookmark.NextNode;
            while (nextBookmark != null && !(nextBookmark is XElement))
            {
                nextBookmark = nextBookmark.NextNode;
            }
            if (nextBookmark == null)
            {
                return;
            }
            currentBookmark.Remove();
            nextBookmark.AddAfterSelf(currentBookmark);
            xml.Save(bookmarkFile);
            int selectedIndex = listbox_bookmarklist.SelectedIndex + 1;
            UpdateBookmarks(true);
            listbox_bookmarklist.SelectedIndex = selectedIndex;
        }

        private void btn_loadbookmark_Click(object sender, RoutedEventArgs e)
        {
            LoadBookmark();
        }

        private void btn_addbookmark_Click(object sender, RoutedEventArgs e)
        {
            AddBookmark();
        }

        private void btn_renamebookmark_Click(object sender, RoutedEventArgs e)
        {
            RenameBookmark();
        }

        private void btn_deletebookmark_Click(object sender, RoutedEventArgs e)
        {
            DeleteBookmark();
        }

        private void btn_moveup_Click(object sender, RoutedEventArgs e)
        {
            MoveBookmarkUp();
        }

        private void btn_movedown_Click(object sender, RoutedEventArgs e)
        {
            MoveBookmarkDown();
        }

        private void listbox_bookmarklist_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listbox_bookmarklist.SelectedItem != null)
            {
                LoadBookmark();
            }
        }

        private void contxtmenu_moveup_Click(object sender, RoutedEventArgs e)
        {
            MoveBookmarkUp();
        }

        private void contxtmenu_movedown_Click(object sender, RoutedEventArgs e)
        {
            MoveBookmarkDown();
        }

        private void contxtmenu_rename_Click(object sender, RoutedEventArgs e)
        {
            RenameBookmark();
        }

        private void contxtmenu_delete_Click(object sender, RoutedEventArgs e)
        {
            DeleteBookmark();
        }

        private void listbox_bookmarklist_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                DeleteBookmark();
            }

            if (e.Key == Key.F2)
            {
                RenameBookmark();
            }
        }

        private void listbox_bookmarklist_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (listbox_bookmarklist.SelectedItem == null || listbox_bookmarklist.Items.Count == 0)
            {
                e.Handled = true;
            }
        }

        private void listbox_bookmarklist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listbox_bookmarklist.SelectedIndex != 0 && listbox_bookmarklist.SelectedIndex != listbox_bookmarklist.Items.Count - 1 && listbox_bookmarklist.SelectedItem != null)
            {
                btn_moveup.IsEnabled = true;
                btn_movedown.IsEnabled = true;
            }

            else if (listbox_bookmarklist.SelectedIndex == 0 && listbox_bookmarklist.Items.Count > 1)
            {
                btn_moveup.IsEnabled = false;
                btn_movedown.IsEnabled = true;
            }

            else if (listbox_bookmarklist.SelectedIndex == listbox_bookmarklist.Items.Count - 1 && listbox_bookmarklist.Items.Count > 1)
            {
                btn_moveup.IsEnabled = true;
                btn_movedown.IsEnabled = false;
            }

            else
            {
                btn_moveup.IsEnabled = false;
                btn_movedown.IsEnabled = false;
            }

            if (listbox_bookmarklist.SelectedItem == null)
            {
                btn_renamebookmark.IsEnabled = false;
                btn_deletebookmark.IsEnabled = false;
                btn_moveup.IsEnabled = false;
                btn_movedown.IsEnabled = false;
            }

            else if (listbox_bookmarklist.SelectedItems.Count > 1)
            {
                btn_renamebookmark.IsEnabled = false;
                btn_moveup.IsEnabled = false;
                btn_movedown.IsEnabled = false;
            }

            else
            {
                btn_renamebookmark.IsEnabled = true;
                btn_deletebookmark.IsEnabled = true;
            }

        }
    }
}
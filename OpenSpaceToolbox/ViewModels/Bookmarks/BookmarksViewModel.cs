﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Linq;

namespace OpenSpaceToolbox
{
    /// <summary>
    /// View model for bookmarks
    /// </summary>
    public class BookmarksViewModel : BaseViewModel
    {
        #region Constructor

        public BookmarksViewModel(GenericGameManager gameManager)
        {
            // Set properties
            GameManager = gameManager;

            BookmarkFile = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), GameManager.BookmarkFileName+".xml");
            BookmarkItems = new ObservableCollection<BookmarkItemViewModel>();
            AllBookmarkItems = new List<BookmarkItemViewModel>();
            SelectedBookmarkIndex = -1;

            // Enable collection synchronization so the collection can be updated from another thread
            BindingOperations.EnableCollectionSynchronization(BookmarkItems, this);

            // Create commands
            AddBookmarkCommand = new RelayCommand(AddBookmark);
            RenameBookmarkCommand = new RelayCommand(RenameBookmark);
            DeleteBookmarkCommand = new RelayCommand(DeleteBookmark);
            LoadBookmarkCommand = new RelayCommand(LoadBookmark);

            // Load existing bookmarks
            LoadBookmarks();

            Task.Run(RefreshAsync);
        }

        #endregion

        #region Commands

        public ICommand LoadBookmarkCommand { get; }

        public ICommand DeleteBookmarkCommand { get; }

        public ICommand RenameBookmarkCommand { get; }

        public ICommand AddBookmarkCommand { get; }

        #endregion

        #region Private Fields

        private string _currentLevel;

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads the bookmarks from a file
        /// </summary>
        private void LoadBookmarks()
        {
            if (!File.Exists(BookmarkFile))
                return;

            var xml = XDocument.Load(BookmarkFile);

            foreach (XElement element in xml.Element(GameManager.BookmarkFileName).Elements())
            {
                // TODO: Try/catch in case of corruption

                var bookmark = element.Element("Bookmark");

                AllBookmarkItems.Add(new BookmarkItemViewModel(element.Name.LocalName, bookmark.Element("Name").Value,
                    float.Parse(bookmark.Element("X").Value, CultureInfo.InvariantCulture.NumberFormat),
                    float.Parse(bookmark.Element("Y").Value, CultureInfo.InvariantCulture.NumberFormat),
                    float.Parse(bookmark.Element("Z").Value, CultureInfo.InvariantCulture.NumberFormat)));
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The app view model
        /// </summary>
        public GenericGameManager GameManager { get; }

        /// <summary>
        /// The currently loaded level
        /// </summary>
        public string CurrentLevel
        {
            get => _currentLevel;
            set
            {
                lock (this)
                {
                    if (_currentLevel == value)
                        return;

                    // Reorder to match UI order
                    foreach (var item in BookmarkItems)
                    {
                        AllBookmarkItems.Remove(item);
                        AllBookmarkItems.Add(item);
                    }

                    BookmarkItems.Clear();

                    foreach (var item in AllBookmarkItems.Where(x => String.Equals(x.Level, value, StringComparison.InvariantCultureIgnoreCase)))
                        BookmarkItems.Add(item);

                    _currentLevel = value;

                    // Get the level and container
                    var lvl = GameManager.Levels.ToList().Find(x => String.Equals(x.FileName, value, StringComparison.InvariantCultureIgnoreCase));

                    if (lvl == null)
                    {
                        CurrentLevelName = "N/A";
                        CurrentSectionName = "";
                        return;
                    }

                    var container = GameManager.LevelContainers.ToList().Find(x => x.Levels.Contains(lvl));

                    if (container == null)
                    {
                        CurrentLevelName = "N/A";
                        CurrentSectionName = "";
                        return;
                    }

                    CurrentLevelName = container.Name;
                    CurrentSectionName = lvl.Name;
                }
            }
        }

        /// <summary>
        /// The name of the currently loaded level
        /// </summary>
        public string CurrentLevelName { get; set; }

        public string CurrentSectionName { get; set; }

        /// <summary>
        /// The index for the currently selected bookmark item
        /// </summary>
        public int SelectedBookmarkIndex { get; set; }

        /// <summary>
        /// Gets the selected bookmark or null if none was found
        /// </summary>
        public BookmarkItemViewModel SelectedBookmark => BookmarkItems.ElementAtOrDefault(SelectedBookmarkIndex);

        /// <summary>
        /// The file path for the bookmark file
        /// </summary>
        public string BookmarkFile { get; }

        /// <summary>
        /// All bookmark items
        /// </summary>
        public List<BookmarkItemViewModel> AllBookmarkItems { get; }

        /// <summary>
        /// The bookmark items to show
        /// </summary>
        public ObservableCollection<BookmarkItemViewModel> BookmarkItems { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Refreshes bookmarks and current level
        /// </summary>
        /// <returns>The task</returns>
        public async Task RefreshAsync()
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        int processHandle = GameManager.GetProcessHandle(false);

                        if (processHandle >= 0)
                        {
                            string levelname = GameManager.CurrentLevel.ToLower();

                            int indexOfSubLevel = levelname.IndexOf('$');

                            if (indexOfSubLevel > 0)
                                levelname = levelname.ToLower().Substring(0, indexOfSubLevel);

                            CurrentLevel = levelname;
                        }
                        else
                        {
                            CurrentLevel = string.Empty;
                        }

                        await Task.Delay(1500);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            });
        }

        /// <summary>
        /// Loads the currently selected bookmark
        /// </summary>
        public void LoadBookmark()
        {
            if (SelectedBookmark == null)
                return;

            GameManager.PlayerCoordinates = GameManager.SavedPosition = (SelectedBookmark.X, SelectedBookmark.Y, SelectedBookmark.Z);
        }

        /// <summary>
        /// Deletes the currently selected bookmark
        /// </summary>
        public void DeleteBookmark()
        {
            if (SelectedBookmark == null)
                return;

            AllBookmarkItems.Remove(SelectedBookmark);
            BookmarkItems.Remove(SelectedBookmark);
        }

        /// <summary>
        /// Renames the currently selected bookmark
        /// </summary>
        public void RenameBookmark()
        {
            if (SelectedBookmark == null)
                return;

            RenameDialog rename =
                new RenameDialog(SelectedBookmark.Name, SelectedBookmark.X, SelectedBookmark.Y, SelectedBookmark.Z)
                {
                    Owner = Application.Current.MainWindow,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };

            rename.ShowDialog();

            if (!rename.Result) return;

            string newName;

            if (rename.BookmarkName != SelectedBookmark.Name && AllBookmarkItems.Any(x => x.Name == rename.BookmarkName))
            {
                int i = 1;
                do
                {
                    newName = rename.BookmarkName + $" ({i})";
                    i++;
                } while (AllBookmarkItems.Any(x => x.Name == newName));
            }
            else newName = rename.BookmarkName;

            SelectedBookmark.Name = newName;
            SelectedBookmark.X = rename.X;
            SelectedBookmark.Y = rename.Y;
            SelectedBookmark.Z = rename.Z;
        }

        /// <summary>
        /// Adds a new bookmark
        /// </summary>
        public void AddBookmark()
        {
            int processHandle = GameManager.GetProcessHandle();
            if (processHandle < 0) { return; }

            string levelname = GameManager.CurrentLevel.ToLower();

            int indexOfSubLevel = levelname.IndexOf('$');

            if (indexOfSubLevel > 0)
                levelname = levelname.ToLower().Substring(0, indexOfSubLevel);

            if (String.IsNullOrEmpty(levelname))
                return;

            var coords = GameManager.PlayerCoordinates;

            const string newBookmarkName = "Bookmark";
            int newBookmarkSuffix = 0;

            while (AllBookmarkItems.Any(x => x.Name == $"{newBookmarkName} {newBookmarkSuffix}"))
                newBookmarkSuffix++;

            var bookmark = new BookmarkItemViewModel(levelname, $"{newBookmarkName} {newBookmarkSuffix}", coords.Item1, coords.Item2, coords.Item3);

            AllBookmarkItems.Add(bookmark);
            BookmarkItems.Add(bookmark);

            GameManager.SavedPosition = coords;
        }

        /// <summary>
        /// Saves the bookmarks to a file
        /// </summary>
        public void SaveBookmarks()
        {
            // Reorder to match UI order
            foreach (var item in BookmarkItems)
            {
                AllBookmarkItems.Remove(item);
                AllBookmarkItems.Add(item);
            }

            var headerElement = new XElement(GameManager.BookmarkFileName);

            foreach (BookmarkItemViewModel bookmarkItem in AllBookmarkItems)
                headerElement.Add(new XElement(bookmarkItem.Level, new XElement("Bookmark", new XElement("Name", bookmarkItem.Name), new XElement("X", bookmarkItem.X), new XElement("Y", bookmarkItem.Y), new XElement("Z", bookmarkItem.Z))));

            var document = new XDocument(headerElement);

            // Save
            document.Save(BookmarkFile);
        }

        #endregion
    }
}
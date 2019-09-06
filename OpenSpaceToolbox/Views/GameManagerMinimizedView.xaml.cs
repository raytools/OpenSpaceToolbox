using System.Windows.Controls;
using System.Windows.Input;

namespace OpenSpaceToolbox
{
    /// <summary>
    /// Interaction logic for GameManagerMinimizedView.xaml
    /// </summary>
    public partial class GameManagerMinimizedView : UserControl
    {
        public GameManagerMinimizedView()
        {
            InitializeComponent();
        }

        private void BookmarkListItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is MainViewModel viewModel)
                viewModel.GameManager.PlayerCoordinates = (viewModel.BookmarksVm.SelectedBookmark.X,
                    viewModel.BookmarksVm.SelectedBookmark.Y, viewModel.BookmarksVm.SelectedBookmark.Z);
        }

        private void LevelTreeItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is TreeViewItem item && item.DataContext is LevelViewModel lvl &&
                DataContext is MainViewModel viewModel)
                viewModel.GameManager.CurrentLevel = lvl.FileName;
        }
    }
}

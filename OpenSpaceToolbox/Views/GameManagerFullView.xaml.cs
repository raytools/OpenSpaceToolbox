using System.Windows.Controls;
using System.Windows.Input;

namespace OpenSpaceToolbox
{
    /// <summary>
    /// Interaction logic for GameManagerFullView.xaml
    /// </summary>
    public partial class GameManagerFullView : UserControl
    {
        public GameManagerFullView()
        {
            InitializeComponent();
        }

        private void BookmarkListItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is MainViewModel viewModel)
                viewModel.GameManager.PlayerCoordinates = viewModel.GameManager.SavedPosition =
                    (viewModel.BookmarksVm.SelectedBookmark.X,
                    viewModel.BookmarksVm.SelectedBookmark.Y,
                    viewModel.BookmarksVm.SelectedBookmark.Z);
        }

        private void LevelTreeItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is TreeViewItem item && item.DataContext is LevelViewModel lvl &&
                DataContext is MainViewModel viewModel)
                viewModel.GameManager.CurrentLevel = lvl.FileName;
        }

      private void MenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
      {

        }
    }
}

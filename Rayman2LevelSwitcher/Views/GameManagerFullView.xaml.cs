using System.Windows.Controls;
using System.Windows.Input;

namespace Rayman2LevelSwitcher
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
                viewModel.BookmarksVm.SelectedBookmark?.LoadBookmark();
        }

        private void LevelTreeItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is TreeViewItem item && item.DataContext is Rayman2LevelViewModel lvl)
                lvl.LoadLevel();
        }
    }
}

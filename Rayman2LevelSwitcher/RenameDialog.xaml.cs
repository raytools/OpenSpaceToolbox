using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Rayman2LevelSwitcher
{
    /// <summary>
    /// Interaction logic for RenameDialog.xaml
    /// </summary>
    public partial class RenameDialog : Window
    {
        public RenameDialog()
        {
            InitializeComponent();
        }

        private void btn_rename_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.renameBookmarkName = txtbox_name.Text;
            this.Close();
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtbox_name.Focus();
        }

        private void txtbox_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                btn_close_Click(sender, e);
            }

            if (e.Key == Key.Return)
            {
                btn_rename_Click(sender, e);
            }
        }
    }
}

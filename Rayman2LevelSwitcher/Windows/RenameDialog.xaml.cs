using System.Windows;

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

        public string Result { get; set; }

        private void Btn_rename_Click(object sender, RoutedEventArgs e)
        {
            Result = txtbox_name.Text;
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtbox_name.Focus();
            txtbox_name.SelectAll();
        }
    }
}
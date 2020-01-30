using System.Globalization;
using System.Windows;

namespace OpenSpaceToolbox
{
    /// <summary>
    /// Interaction logic for RenameDialog.xaml
    /// </summary>
    public partial class RenameDialog : Window
    {
        public RenameDialog(string name, float x, float y, float z)
        {
            InitializeComponent();

            NameBox.Text = name;
            XBox.Text = x.ToString(CultureInfo.InvariantCulture);
            YBox.Text = y.ToString(CultureInfo.InvariantCulture);
            ZBox.Text = z.ToString(CultureInfo.InvariantCulture);
            Result = false;
        }

        public string BookmarkName { get; set; }

        public float X;
        public float Y;
        public float Z;

        public bool Result { get; set; }

        private void Btn_rename_Click(object sender, RoutedEventArgs e)
        {
            if (float.TryParse(XBox.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out X) &&
                float.TryParse(YBox.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out Y) &&
                float.TryParse(ZBox.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out Z))
            {
                if (!string.IsNullOrWhiteSpace(NameBox.Text))
                {
                    BookmarkName = NameBox.Text;
                    Result = true;
                    Close();
                }
                else MessageBox.Show("Bookmark name cannot be empty.");
            }
            else MessageBox.Show("Invalid number format. Make sure the coordinates are correct.");
        }
    }
}
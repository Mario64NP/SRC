using System.Windows;
using System.Windows.Controls;

namespace WpfApp
{
    public partial class PlayerDetails : Window
    {
        public string Nick { get { return txtNick.Text; } set { txtNick.Text = value; } }
        public int Age { get { return int.Parse(txtAge.Text) ; } set { txtAge.Text = value.ToString(); } }

        public PlayerDetails()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
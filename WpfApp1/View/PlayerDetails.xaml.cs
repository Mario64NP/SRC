using System.Windows;
using System.Windows.Controls;

namespace WpfApp
{
    public partial class PlayerDetails : Window
    {
        public string Nick { get { return txtNick.Text; } set { txtNick.Text = value; } }
        public int Age { 
            get 
            {
                _ = int.TryParse(txtAge.Text, out int age);
                return age; 
            } 
            set { txtAge.Text = value.ToString(); } }

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
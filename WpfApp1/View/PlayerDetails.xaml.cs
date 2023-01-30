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
            if (string.IsNullOrEmpty(txtNick.Text))
            {
                txtNick.Background = System.Windows.Media.Brushes.Red;
                MessageBox.Show("Nick can't be empty!", "Invalid nick", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtAge.Text) || Age < 13)
            {
                txtAge.Background = System.Windows.Media.Brushes.Red;
                MessageBox.Show("Age can't be empty or less than 13!", "Invalid age", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DialogResult = true;
        }
    }
}
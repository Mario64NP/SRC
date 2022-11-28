using Microsoft.EntityFrameworkCore;
using System.Windows;
using WpfApp.Controller;
using WpfApp.Domain;

namespace WpfApp.View
{
    public partial class GameDetails : Window
    {
        private SRCContext _context = new SRCContext();
        public new string Name { get { return txtName.Text; } set { txtName.Text = value; } }
        public string Developer { get { return txtDeveloper.Text; } set { txtDeveloper.Text = value; } }
        public int ReleaseYear { get { return int.Parse(txtReleaseYear.Text); } set { txtReleaseYear.Text = value.ToString(); } }
        public Platform Platform { get { return (Platform)cmbPlatform.SelectedItem; } set { cmbPlatform.SelectedItem = value; } }
        public GameDetails()
        {
            InitializeComponent();
            _context.Platforms.Load();
            cmbPlatform.ItemsSource = _context.Platforms.Local.ToObservableCollection();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}

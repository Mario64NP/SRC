using System.Collections;
using System.Collections.Generic;
using System.Windows;
using WpfApp.Controller;
using WpfApp.DataAccessLayer.Implementations;
using WpfApp.DataAccessLayer.Interfaces;
using WpfApp.Domain;

namespace WpfApp.View
{
    public partial class GameDetails : Window
    {
        private readonly IUnitOfWork _unitOfWork;
        public new string Name { get { return txtName.Text; } set { txtName.Text = value; } }
        public string Developer { get { return txtDeveloper.Text; } set { txtDeveloper.Text = value; } }
        public int ReleaseYear { 
            get 
            {
                _ = int.TryParse(txtReleaseYear.Text, out int year);
                return year;
            } 
            set { txtReleaseYear.Text = value.ToString(); } }
        public Platform Platform { get { return (Platform)cmbPlatform.SelectedItem; } set { cmbPlatform.SelectedItem = value; } }
        public IList SelectedCategories { get => lvCategories.SelectedItems; }
        public GameDetails()
        {
            InitializeComponent();

            _unitOfWork = new UnitOfWork(new SRCContext());
            cmbPlatform.ItemsSource   = _unitOfWork.Platforms.GetAll();
            cmbPlatform.SelectedIndex = 0;

            foreach (var item in _unitOfWork.Categories.GetAll())
                lvCategories.Items.Add(item);
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                txtName.Background = System.Windows.Media.Brushes.Red;
                MessageBox.Show("Name can't be empty!", "Invalid name", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtDeveloper.Text))
            {
                txtDeveloper.Background = System.Windows.Media.Brushes.Red;
                MessageBox.Show("Developer can't be empty!", "Invalid developer", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtReleaseYear.Text) || ReleaseYear < 1958)
            {
                txtReleaseYear.Background = System.Windows.Media.Brushes.Red;
                MessageBox.Show("Release year can't be empty!", "Invalid release year", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (lvCategories.SelectedItems.Count == 0)
            {
                lvCategories.Background = System.Windows.Media.Brushes.Red;
                MessageBox.Show("Please select at least one category!", "Invalid categories", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DialogResult = true;
        }
    }
}
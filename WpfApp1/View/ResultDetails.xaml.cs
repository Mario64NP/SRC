using Microsoft.EntityFrameworkCore;
using System;
using System.Windows;
using WpfApp.Controller;
using WpfApp.Domain;

namespace WpfApp.View
{
    public partial class ResultDetails : Window
    {
        private SRCContext _context = new SRCContext();
        public Player Player { get { return (Player)cmbPlayer.SelectedItem; } set { cmbPlayer.SelectedItem = value; } }
        public Game Game { get { return (Game)cmbGame.SelectedItem; } set { cmbGame.SelectedItem = value; } }
        public Category Category { get { return (Category)cmbCategory.SelectedItem; } set { cmbCategory.SelectedItem = value; } }
        public int Time { get { return int.Parse(txtTime.Text); } set { txtTime.Text = value.ToString(); } }
        public DateTime Date { get { return DateTime.Parse(dtpDate.Text); } set { dtpDate.Text = value.ToString(); } }
        public ResultDetails()
        {
            InitializeComponent();
            _context.Players.Load();
            _context.Games.Load();
            _context.Categories.Load();
            cmbPlayer.ItemsSource = _context.Players.Local.ToObservableCollection();
            cmbGame.ItemsSource = _context.Games.Local.ToObservableCollection();
            cmbCategory.ItemsSource = _context.Categories.Local.ToObservableCollection();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}

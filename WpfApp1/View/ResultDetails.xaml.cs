using Microsoft.EntityFrameworkCore;
using System;
using System.Windows;
using WpfApp.Controller;
using WpfApp.DataAccessLayer.Implementations;
using WpfApp.DataAccessLayer.Interfaces;
using WpfApp.Domain;

namespace WpfApp.View
{
    public partial class ResultDetails : Window
    {
        private readonly IUnitOfWork _unitOfWork;
        public Player Player { get { return (Player)cmbPlayer.SelectedItem; } set { cmbPlayer.SelectedItem = value; } }
        public Game Game { get { return (Game)cmbGame.SelectedItem; } set { cmbGame.SelectedItem = value; } }
        public Category Category { get { return (Category)cmbCategory.SelectedItem; } set { cmbCategory.SelectedItem = value; } }
        public int Time { get { return int.Parse(txtTime.Text); } set { txtTime.Text = value.ToString(); } }
        public DateTime Date { get { return DateTime.Parse(dtpDate.Text); } set { dtpDate.Text = value.ToString(); } }
        public ResultDetails()
        {
            InitializeComponent();

            _unitOfWork = new UnitOfWork(new SRCContext());
            cmbPlayer.ItemsSource     = _unitOfWork.Players.GetAll();
            cmbGame.ItemsSource       = _unitOfWork.Games.GetAll();
            cmbCategory.ItemsSource   = _unitOfWork.Categories.GetAll();
            cmbPlayer.SelectedIndex   = 0;
            cmbGame.SelectedIndex     = 0;
            cmbCategory.SelectedIndex = 0;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
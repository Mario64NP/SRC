﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        public int Time { 
            get 
            {
                _ = int.TryParse(txtTime.Text, out int t);
                return t;
            } 
            set { txtTime.Text = value.ToString(); } }
        public DateTime Date { 
            get 
            {
                _ = DateTime.TryParse(dtpDate.Text, out DateTime dt);
                return dt;
            } 
            set { dtpDate.Text = value.ToString(); } }
        public ResultDetails()
        {
            InitializeComponent();
            dtpDate.Text = DateTime.Now.ToString();

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
            if (string.IsNullOrEmpty(txtTime.Text) || Time <= 0)
            {
                txtTime.Background = System.Windows.Media.Brushes.Red;
                MessageBox.Show("Time can't be empty or less than 1!", "Invalid nick", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DialogResult = true;
        }

        private void cmbGame_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            List<Category> list = new();

            foreach (var item in _unitOfWork.GameCategories.Find(gc => gc.Game.Equals((Game)cmbGame.SelectedItem)).ToList())
                list.Add(_unitOfWork.Categories.GetById(item.Category.ID));

            cmbCategory.ItemsSource = list;
            cmbCategory.SelectedIndex = 0;
        }

        public void DisableEditingKeyFields()
        {
            cmbPlayer.IsEnabled   = false;
            cmbGame.IsEnabled     = false;
            cmbCategory.IsEnabled = false;
        }
    }
}
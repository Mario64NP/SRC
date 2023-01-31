using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;
using WpfApp.Controller;
using WpfApp.DataAccessLayer.Implementations;
using WpfApp.DataAccessLayer.Interfaces;
using WpfApp.Domain;
using WpfApp.View;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        private readonly IUnitOfWork _unitOfWork;
        public MainWindow()
        {
            InitializeComponent();

            _unitOfWork = new UnitOfWork(new SRCContext());

            dgPlayers.ItemsSource = _unitOfWork.Players.GetAll();
            dgGames.ItemsSource   = _unitOfWork.Games.GetAll();
            dgResults.ItemsSource = _unitOfWork.Results.GetAll();
        }

        private void btnAddPlayer_Click(object sender, RoutedEventArgs e)
        {
            PlayerDetails playerDetails = new()
            {
                Title = "Add a new player",
                Owner = this
            };

            if ((bool)playerDetails.ShowDialog())
            {
                Player p = new()
                {
                    Nick = playerDetails.Nick,
                    Age  = playerDetails.Age,
                };

                if (p.IsValid())
                {
                    _unitOfWork.Players.Add(p);
                    _unitOfWork.Complete();
                }
                else
                    MessageBox.Show("The details you've entered aren't valid.", "Invalid details", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSearchPlayers_Click(object sender, RoutedEventArgs e)
        {
            int.TryParse(txtPlayersSearch.Text, out int a);
            dgPlayers.ItemsSource = _unitOfWork.Players.Find(x => x.Nick.Contains(txtPlayersSearch.Text) || x.Age == a).ToList();
        }

        private void btnEditPlayer_Click(object sender, RoutedEventArgs e)
        {
            if (dgPlayers.SelectedItem == null)
                return;

            Player selectedPlayer = (Player)dgPlayers.SelectedItem;
            PlayerDetails playerDetails = new()
            {
                Title = "Edit a player",
                Owner = this,
                Nick  = selectedPlayer.Nick,
                Age   = selectedPlayer.Age
            };

            if ((bool)playerDetails.ShowDialog())
            {
                if (new Player() { Nick = playerDetails.Nick, Age = playerDetails.Age}.IsValid())
                {
                    selectedPlayer.Nick = playerDetails.Nick;
                    selectedPlayer.Age  = playerDetails.Age;
                    _unitOfWork.Complete();

                    dgPlayers.ItemsSource = null;
                    dgPlayers.ItemsSource = _unitOfWork.Players.GetAll();
                    dgResults.ItemsSource = null;
                    dgResults.ItemsSource = _unitOfWork.Results.GetAll();
                }
                else
                    MessageBox.Show("The details you've entered aren't valid.", "Invalid details", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDeletePlayer_Click(object sender, RoutedEventArgs e)
        {
            if (dgPlayers.SelectedItem != null)
            {
                _unitOfWork.Players.Remove((Player)dgPlayers.SelectedItem);
                _unitOfWork.Complete();
            }
        }

        private void btnAddGame_Click(object sender, RoutedEventArgs e)
        {
            GameDetails gameDetails = new()
            {
                Title = "Add a new game",
                Owner = this
            };

            if ((bool)gameDetails.ShowDialog())
            {
                Game g = new()
                {
                    Name        = gameDetails.Name,
                    Developer   = gameDetails.Developer,
                    ReleaseYear = gameDetails.ReleaseYear,
                    Platform    = _unitOfWork.Platforms.GetAll().Single(x => x.Equals(gameDetails.Platform))
                };

                if (g.IsValid() && gameDetails.SelectedCategories.Count > 0)
                {
                    _unitOfWork.Games.Add(g);

                    foreach (var item in gameDetails.SelectedCategories)
                    {
                        GameCategory gc = new()
                        {
                            Game = g,
                            Category = _unitOfWork.Categories.GetById(((Category)item).ID)
                        };
                        _unitOfWork.GameCategories.Add(gc);
                    }

                    _unitOfWork.Complete();
                }
                else
                    MessageBox.Show("The details you've entered aren't valid.", "Invalid details", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSearchGames_Click(object sender, RoutedEventArgs e)
        {
            int.TryParse(txtGamesSearch.Text, out int a);
            dgGames.ItemsSource = _unitOfWork.Games.Find(x => x.Name.Contains(txtGamesSearch.Text) || x.Developer.Contains(txtGamesSearch.Text) || x.ReleaseYear == a || x.Platform.Name.Contains(txtGamesSearch.Text)).ToList();
        }

        private void btnEditGame_Click(object sender, RoutedEventArgs e)
        {
            if (dgGames.SelectedItem == null)
                return;

            Game selectedGame = (Game)dgGames.SelectedItem;
            GameDetails gameDetails = new()
            {
                Title       = "Edit a game",
                Owner       = this,
                Name        = selectedGame.Name,
                Developer   = selectedGame.Developer,
                ReleaseYear = selectedGame.ReleaseYear,
                Platform    = selectedGame.Platform
            };

            if ((bool)gameDetails.ShowDialog())
            {
                if (new Game() { Name = gameDetails.Name, Developer = gameDetails.Developer, ReleaseYear = gameDetails.ReleaseYear, Platform = gameDetails.Platform}.IsValid() && gameDetails.SelectedCategories.Count > 0)
                {
                    selectedGame.Name        = gameDetails.Name;
                    selectedGame.Developer   = gameDetails.Developer;
                    selectedGame.ReleaseYear = gameDetails.ReleaseYear;
                    selectedGame.Platform    = _unitOfWork.Platforms.GetAll().Single(x => x.Equals(gameDetails.Platform));

                    foreach (var item in _unitOfWork.GameCategories.GetAll().Where(gc => gc.Game.Equals(selectedGame)).ToList())
                        _unitOfWork.GameCategories.Remove(item);

                    foreach (var item in gameDetails.SelectedCategories)
                    {
                        GameCategory gc = new()
                        {
                            Game = selectedGame,
                            Category = _unitOfWork.Categories.GetById(((Category)item).ID)
                        };
                        _unitOfWork.GameCategories.Add(gc);
                    }

                    _unitOfWork.Complete();

                    dgGames.ItemsSource = null;
                    dgGames.ItemsSource = _unitOfWork.Games.GetAll();
                    dgResults.ItemsSource = null;
                    dgResults.ItemsSource = _unitOfWork.Results.GetAll();
                }
                else
                    MessageBox.Show("The details you've entered aren't valid.", "Invalid details", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDeleteGame_Click(object sender, RoutedEventArgs e)
        {
            if (dgGames.SelectedItem != null)
            {
                _unitOfWork.Games.Remove((Game)dgGames.SelectedItem);
                _unitOfWork.Complete();
            }
        }

        private void btnAddResult_Click(object sender, RoutedEventArgs e)
        {
            ResultDetails resultDetails = new()
            {
                Title = "Add a new result",
                Owner = this
            };

            if ((bool)resultDetails.ShowDialog())
            {
                Result r = new()
                {
                    Player       = _unitOfWork.Players.GetAll().Single(x => x.Equals(resultDetails.Player)),
                    GameCategory = _unitOfWork.GameCategories.GetAll().Single(x => x.Game.Equals(resultDetails.Game) && x.Category.Equals(resultDetails.Category)),
                    Time         = resultDetails.Time,
                    Date         = resultDetails.Date
                };

                if (r.IsValid())
                {
                    _unitOfWork.Results.Add(r);
                    _unitOfWork.Complete();
                }
                else
                    MessageBox.Show("The details you've entered aren't valid.", "Invalid details", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSearchResults_Click(object sender, RoutedEventArgs e)
        {
            int.TryParse(txtResultsSearch.Text, out int a);
            dgResults.ItemsSource = _unitOfWork.Results.Find(x => 
            x.Player.Nick.Contains(txtResultsSearch.Text) || x.GameCategory.Game.Name.Contains(txtResultsSearch.Text) || 
            x.GameCategory.Category.Name.Contains(txtResultsSearch.Text) || x.Time == a).ToList();
        }

        private void btnEditResult_Click(object sender, RoutedEventArgs e)
        {
            if (dgResults.SelectedItem == null)
                return;

            Result selectedResult = (Result)dgResults.SelectedItem;
            ResultDetails resultDetails = new()
            {
                Title    = "Edit a result",
                Owner    = this,
                Player   = selectedResult.Player,
                Game     = selectedResult.Game,
                Category = selectedResult.Category,
                Time     = selectedResult.Time,
                Date     = selectedResult.Date
            };
            resultDetails.DisableEditingKeyFields();

            if ((bool)resultDetails.ShowDialog())
            {
                if (new Result() { 
                    Player       = resultDetails.Player, 
                    GameCategory = _unitOfWork.GameCategories.GetAll().Single(gc => gc.Game.Equals(resultDetails.Game) && gc.Category.Equals(resultDetails.Category)), 
                    Time         = resultDetails.Time, 
                    Date         = resultDetails.Date}.IsValid())
                {
                    selectedResult.Time = resultDetails.Time;
                    selectedResult.Date = resultDetails.Date;
                    _unitOfWork.Complete();

                    dgResults.ItemsSource = null;
                    dgResults.ItemsSource = _unitOfWork.Results.GetAll();
                }
                else
                    MessageBox.Show("The details you've entered aren't valid.", "Invalid details", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDeleteResult_Click(object sender, RoutedEventArgs e)
        {
            if (dgResults.SelectedItem != null)
            {
                _unitOfWork.Results.Remove((Result)dgResults.SelectedItem);
                _unitOfWork.Complete();
            }
        }

        private void dgResults_AutoGeneratedColumns(object sender, System.EventArgs e)
        {
            dgResults.Columns[0].Visibility = Visibility.Collapsed;
            dgResults.Columns[2].Visibility = Visibility.Collapsed;
            dgResults.Columns[3].Visibility = Visibility.Collapsed;
            dgResults.Columns[5].Visibility = Visibility.Collapsed;
        }

        private void btnSearchPlayers_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DoubleAnimation sizeAnimation = new(100, new Duration(TimeSpan.FromSeconds(0.3)));
            btnSearchPlayers.BeginAnimation(WidthProperty, sizeAnimation);
        }

        private void btnSearchPlayers_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DoubleAnimation sizeAnimation = new(75, new Duration(TimeSpan.FromSeconds(0.3)));
            btnSearchPlayers.BeginAnimation(WidthProperty, sizeAnimation);
        }

        private void btnSearchGames_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DoubleAnimation sizeAnimation = new(100, new Duration(TimeSpan.FromSeconds(0.3)));
            btnSearchGames.BeginAnimation(WidthProperty, sizeAnimation);
        }

        private void btnSearchGames_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DoubleAnimation sizeAnimation = new(75, new Duration(TimeSpan.FromSeconds(0.3)));
            btnSearchGames.BeginAnimation(WidthProperty, sizeAnimation);
        }

        private void btnSearchResults_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DoubleAnimation sizeAnimation = new(100, new Duration(TimeSpan.FromSeconds(0.3)));
            btnSearchResults.BeginAnimation(WidthProperty, sizeAnimation);
        }

        private void btnSearchResults_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DoubleAnimation sizeAnimation = new(75, new Duration(TimeSpan.FromSeconds(0.3)));
            btnSearchResults.BeginAnimation(WidthProperty, sizeAnimation);
        }
    }
}
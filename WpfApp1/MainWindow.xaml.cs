using System.Linq;
using System.Windows;
using WpfApp.Controller;
using WpfApp.DataAccessLayer.Implementations;
using WpfApp.DataAccessLayer.Interfaces;
using WpfApp.Domain;
using WpfApp.View;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        private IUnitOfWork _unitOfWork;
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
                Title = "Add new player",
                Owner = this
            };

            if ((bool)playerDetails.ShowDialog())
            {
                Player p = new()
                {
                    Nick = playerDetails.Nick,
                    Age = playerDetails.Age,
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
            PlayerDetails playerDetails = new()
            {
                Title = "Search players",
                Owner = this
            };

            if ((bool)playerDetails.ShowDialog())
            {
                dgPlayers.ItemsSource = _unitOfWork.Players.Find(x => x.Nick == playerDetails.Nick && x.Age == playerDetails.Age).ToList();
            }
        }

        private void btnEditPlayer_Click(object sender, RoutedEventArgs e)
        {
            PlayerDetails playerDetails = new()
            {
                Title = "Edit a player",
                Owner = this
            };
            Player selectedPlayer = (Player)dgPlayers.SelectedItem;
            playerDetails.Nick = selectedPlayer.Nick;
            playerDetails.Age = selectedPlayer.Age;

            if ((bool)playerDetails.ShowDialog())
            {
                if (new Player() { Nick = playerDetails.Nick, Age = playerDetails.Age}.IsValid())
                {
                    selectedPlayer.Nick = playerDetails.Nick;
                    selectedPlayer.Age = playerDetails.Age;
                    _unitOfWork.Complete();
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
                Title = "Add new game",
                Owner = this
            };

            if ((bool)gameDetails.ShowDialog())
            {
                Game g = new()
                {
                    Name = gameDetails.Name,
                    Developer = gameDetails.Developer,
                    ReleaseYear = gameDetails.ReleaseYear,
                    Platform = _unitOfWork.Platforms.GetAll().Single(x => x.ID == gameDetails.Platform.ID)
                };

                if (g.IsValid())
                {
                    _unitOfWork.Games.Add(g);
                    _unitOfWork.Complete();
                }
                else
                    MessageBox.Show("The details you've entered aren't valid.", "Invalid details", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSearchGames_Click(object sender, RoutedEventArgs e)
        {
            GameDetails gameDetails = new()
            {
                Title = "Search games",
                Owner = this
            };

            if ((bool)gameDetails.ShowDialog())
            {
                dgGames.ItemsSource = _unitOfWork.Games.Find(x => x.Name == gameDetails.Name && x.Developer == gameDetails.Developer && x.ReleaseYear == gameDetails.ReleaseYear && x.Platform == gameDetails.Platform).ToList();
            }
        }

        private void btnEditGame_Click(object sender, RoutedEventArgs e)
        {
            GameDetails gameDetails = new()
            {
                Title = "Edit a player",
                Owner = this
            };
            Game selectedGame = (Game)dgGames.SelectedItem;
            gameDetails.Name = selectedGame.Name;
            gameDetails.Developer = selectedGame.Developer;
            gameDetails.ReleaseYear = selectedGame.ReleaseYear;
            gameDetails.Platform = selectedGame.Platform;

            if ((bool)gameDetails.ShowDialog())
            {
                if (new Game() { Name = gameDetails.Name, Developer = gameDetails.Developer, ReleaseYear = gameDetails.ReleaseYear, Platform = gameDetails.Platform}.IsValid())
                {
                    selectedGame.Name = gameDetails.Name;
                    selectedGame.Developer = gameDetails.Developer;
                    selectedGame.ReleaseYear = gameDetails.ReleaseYear;
                    selectedGame.Platform = _unitOfWork.Platforms.GetAll().Single(x => x.ID == gameDetails.Platform.ID);
                    _unitOfWork.Complete();
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
                Title = "Add new result",
                Owner = this
            };

            if ((bool)resultDetails.ShowDialog())
            {
                Result r = new()
                {
                    Player = _unitOfWork.Players.GetAll().Single(x => x.ID == resultDetails.Player.ID),
                    GameCategory = _unitOfWork.GameCategories.GetAll().Single(x => x.Game.ID == resultDetails.Game.ID && x.Category.ID == resultDetails.Category.ID),
                    Time = resultDetails.Time,
                    Date = resultDetails.Date
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
            ResultDetails resultDetails = new()
            {
                Title = "Search results",
                Owner = this
            };

            if ((bool)resultDetails.ShowDialog())
            {
                dgResults.ItemsSource = _unitOfWork.Results.Find(x => 
                x.Player.Equals(resultDetails.Player) && x.GameCategory.Game.Equals(resultDetails.Game) && 
                x.GameCategory.Category.Equals(resultDetails.Category) && x.Time == resultDetails.Time && x.Date == resultDetails.Date).ToList();
            }
        }

        private void btnEditResult_Click(object sender, RoutedEventArgs e)
        {
            ResultDetails resultDetails = new()
            {
                Title = "Edit a player",
                Owner = this
            };
            Result selectedResult = (Result)dgResults.SelectedItem;
            resultDetails.Player = selectedResult.Player;
            resultDetails.Game = selectedResult.GameCategory.Game;
            resultDetails.Category = selectedResult.GameCategory.Category;
            resultDetails.Time = selectedResult.Time;
            resultDetails.Date = selectedResult.Date;

            if ((bool)resultDetails.ShowDialog())
            {
                if (new Result() { 
                    Player = resultDetails.Player, 
                    GameCategory = _unitOfWork.GameCategories.GetAll().Single(gc => gc.Game.Equals(resultDetails.Game) && gc.Category.Equals(resultDetails.Category)), 
                    Time = resultDetails.Time, 
                    Date = resultDetails.Date}.IsValid())
                {
                    selectedResult.Player = _unitOfWork.Players.GetAll().Single(x => x.ID == resultDetails.Player.ID);
                    selectedResult.GameCategory = _unitOfWork.GameCategories.GetAll().Single(x => x.Game.ID == resultDetails.Game.ID && x.Category.ID == resultDetails.Category.ID);
                    selectedResult.Time = resultDetails.Time;
                    selectedResult.Date = resultDetails.Date;
                    _unitOfWork.Complete();
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
            dgResults.Columns[4].Visibility = Visibility.Collapsed;
        }
    }
}
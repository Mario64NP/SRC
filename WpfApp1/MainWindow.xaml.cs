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
                dgGames.ItemsSource = _unitOfWork.Games.Find(x => x.Name == gameDetails.Name && x.Developer == gameDetails.Developer && x.ReleaseYear == gameDetails.ReleaseYear && x.Platform.Equals(gameDetails.Platform)).ToList();
            }
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
                if (new Game() { Name = gameDetails.Name, Developer = gameDetails.Developer, ReleaseYear = gameDetails.ReleaseYear, Platform = gameDetails.Platform}.IsValid())
                {
                    selectedGame.Name        = gameDetails.Name;
                    selectedGame.Developer   = gameDetails.Developer;
                    selectedGame.ReleaseYear = gameDetails.ReleaseYear;
                    selectedGame.Platform    = _unitOfWork.Platforms.GetAll().Single(x => x.Equals(gameDetails.Platform));
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
    }
}
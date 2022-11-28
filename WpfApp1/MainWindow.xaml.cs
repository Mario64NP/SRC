using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;
using WpfApp.Controller;
using WpfApp.Domain;
using WpfApp.View;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        private SRCContext _context = new SRCContext();
        public MainWindow()
        {
            InitializeComponent();

            _context.Players.Load();
            _context.Platforms.Load();
            _context.Games.Load();
            _context.Categories.Load();
            _context.GameCategories.Load();
            _context.Results.Load();
            dgPlayers.ItemsSource = _context.Players.Local.ToObservableCollection();
            dgGames.ItemsSource = _context.Games.Local.ToObservableCollection();
            dgResults.ItemsSource = _context.Results.Local.ToObservableCollection();
        }

        private void btnAddPlayer_Click(object sender, RoutedEventArgs e)
        {
            PlayerDetails playerDetails = new PlayerDetails();
            playerDetails.Title = "Add new player";

            if ((bool)playerDetails.ShowDialog())
            {
                _context.Add(new Player()
                {
                    Nick = playerDetails.Nick,
                    Age = playerDetails.Age,
                });
                _context.SaveChanges();
            }
        }

        private void btnSearchPlayers_Click(object sender, RoutedEventArgs e)
        {
            PlayerDetails playerDetails = new PlayerDetails();
            playerDetails.Title = "Search players";

            if ((bool)playerDetails.ShowDialog())
            {
                dgPlayers.ItemsSource = _context.Players.Where(x => x.Nick == playerDetails.Nick && x.Age == playerDetails.Age).ToList();
            }
        }

        private void btnEditPlayer_Click(object sender, RoutedEventArgs e)
        {
            PlayerDetails playerDetails = new PlayerDetails();
            playerDetails.Title = "Edit a player";
            Player selectedPlayer = (Player)dgPlayers.SelectedItem;
            playerDetails.Nick = selectedPlayer.Nick;
            playerDetails.Age = selectedPlayer.Age;

            if ((bool)playerDetails.ShowDialog())
            {
                selectedPlayer.Nick = playerDetails.Nick;
                selectedPlayer.Age = playerDetails.Age;
                _context.SaveChanges();
            }
        }

        private void btnDeletePlayer_Click(object sender, RoutedEventArgs e)
        {
            _context.Remove((Player)dgPlayers.SelectedItem);
            _context.SaveChanges();
        }

        private void btnAddGame_Click(object sender, RoutedEventArgs e)
        {
            GameDetails gameDetails = new GameDetails();
            gameDetails.Title = "Add new game";

            if ((bool)gameDetails.ShowDialog())
            {
                _context.Add(new Game()
                {
                    Name = gameDetails.Name,
                    Developer = gameDetails.Developer,
                    ReleaseYear = gameDetails.ReleaseYear,
                    Platform = _context.Platforms.Single(x => x.ID == gameDetails.Platform.ID)
                });
                _context.SaveChanges();
            }
        }

        private void btnSearchGames_Click(object sender, RoutedEventArgs e)
        {
            GameDetails gameDetails = new GameDetails();
            gameDetails.Title = "Search games";

            if ((bool)gameDetails.ShowDialog())
            {
                dgGames.ItemsSource = _context.Games.Where(x => x.Name == gameDetails.Name && x.Developer == gameDetails.Developer && x.ReleaseYear == gameDetails.ReleaseYear && x.Platform == gameDetails.Platform).ToList();
            }
        }

        private void btnEditGame_Click(object sender, RoutedEventArgs e)
        {
            GameDetails gameDetails = new GameDetails();
            gameDetails.Title = "Edit a player";
            Game selectedGame = (Game)dgGames.SelectedItem;
            gameDetails.Name = selectedGame.Name;
            gameDetails.Developer = selectedGame.Developer;
            gameDetails.ReleaseYear = selectedGame.ReleaseYear;
            gameDetails.Platform = selectedGame.Platform;

            if ((bool)gameDetails.ShowDialog())
            {
                selectedGame.Name = gameDetails.Name;
                selectedGame.Developer = gameDetails.Developer;
                selectedGame.ReleaseYear = gameDetails.ReleaseYear;
                selectedGame.Platform = _context.Platforms.Single(x => x.ID == gameDetails.Platform.ID);
                _context.SaveChanges();
            }
        }

        private void btnDeleteGame_Click(object sender, RoutedEventArgs e)
        {
            _context.Remove((Game)dgGames.SelectedItem);
            _context.SaveChanges();
        }

        private void btnAddResult_Click(object sender, RoutedEventArgs e)
        {
            ResultDetails resultDetails = new ResultDetails();
            resultDetails.Title = "Add new result";

            if ((bool)resultDetails.ShowDialog())
            {
                _context.Add(new Result()
                {
                    Player = _context.Players.Single(x => x.ID == resultDetails.Player.ID), //resultDetails.Player,
                    Game = _context.Games.Single(x => x.ID == resultDetails.Game.ID), //resultDetails.Game,
                    Category = _context.Categories.Single(x => x.ID == resultDetails.Category.ID), //resultDetails.Category,
                    Time = resultDetails.Time,
                    Date = resultDetails.Date
                });
                _context.SaveChanges();
            }
        }

        private void btnSearchResults_Click(object sender, RoutedEventArgs e)
        {
            ResultDetails resultDetails = new ResultDetails();
            resultDetails.Title = "Search results";

            if ((bool)resultDetails.ShowDialog())
            {
                dgResults.ItemsSource = _context.Results.Where(x => x.Player == resultDetails.Player && x.Game == resultDetails.Game && x.Category == resultDetails.Category && x.Time == resultDetails.Time && x.Date == resultDetails.Date).ToList();
            }
        }

        private void btnEditResult_Click(object sender, RoutedEventArgs e)
        {
            ResultDetails resultDetails = new ResultDetails();
            resultDetails.Title = "Edit a player";
            Result selectedResult = (Result)dgResults.SelectedItem;
            resultDetails.Player = selectedResult.Player;
            resultDetails.Game = selectedResult.Game;
            resultDetails.Category = selectedResult.Category;
            resultDetails.Time = selectedResult.Time;
            resultDetails.Date = selectedResult.Date;

            if ((bool)resultDetails.ShowDialog())
            {
                selectedResult.Player = _context.Players.Single(x => x.ID == resultDetails.Player.ID);
                selectedResult.Game = _context.Games.Single(x => x.ID == resultDetails.Game.ID);
                selectedResult.Category = _context.Categories.Single(x => x.ID == resultDetails.Category.ID);
                selectedResult.Time = resultDetails.Time;
                selectedResult.Date = resultDetails.Date;
                _context.SaveChanges();
            }
        }

        private void btnDeleteResult_Click(object sender, RoutedEventArgs e)
        {
            _context.Remove((Result)dgResults.SelectedItem);
            _context.SaveChanges();
        }

        private void dgResults_AutoGeneratedColumns(object sender, System.EventArgs e)
        {
            dgResults.Columns[0].Visibility = Visibility.Collapsed;
            dgResults.Columns[2].Visibility = Visibility.Collapsed;
            dgResults.Columns[4].Visibility = Visibility.Collapsed;
        }
    }
}
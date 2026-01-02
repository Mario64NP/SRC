using System.Collections.ObjectModel;
using System.Linq;
using SpeedrunCommunity.Core;
using SpeedrunCommunity.Core.Services;
using SpeedrunCommunity.Domain;
using SpeedrunCommunity.Persistence;
using System.Windows.Input;
using SpeedrunCommunity.Repositories.Implementations;
using SpeedrunCommunity.Repositories.Interfaces;

namespace SpeedrunCommunity.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDialogService _dialogService;

        // Collections
        private ObservableCollection<Player> _players = null!;
        public ObservableCollection<Player> Players { get => _players; set { _players = value; OnPropertyChanged(); } }

        private ObservableCollection<Game> _games = null!;
        public ObservableCollection<Game> Games { get => _games; set { _games = value; OnPropertyChanged(); } }

        private ObservableCollection<Result> _results = null!;
        public ObservableCollection<Result> Results { get => _results; set { _results = value; OnPropertyChanged(); } }

        // Selected Items
        private Player? _selectedPlayer;
        public Player? SelectedPlayer { get => _selectedPlayer; set { _selectedPlayer = value; OnPropertyChanged(); } }

        private Game? _selectedGame;
        public Game? SelectedGame { get => _selectedGame; set { _selectedGame = value; OnPropertyChanged(); } }

        private Result? _selectedResult;
        public Result? SelectedResult { get => _selectedResult; set { _selectedResult = value; OnPropertyChanged(); } }

        // Search Text
        public string PlayerSearchText { get; set; } = string.Empty;
        public string GameSearchText { get; set; } = string.Empty;
        public string ResultSearchText { get; set; } = string.Empty;

        // Commands
        public ICommand AddPlayerCommand { get; }
        public ICommand EditPlayerCommand { get; }
        public ICommand DeletePlayerCommand { get; }
        public ICommand SearchPlayerCommand { get; }

        public ICommand AddGameCommand { get; }
        public ICommand EditGameCommand { get; }
        public ICommand DeleteGameCommand { get; }
        public ICommand SearchGameCommand { get; }

        public ICommand AddResultCommand { get; }
        public ICommand EditResultCommand { get; }
        public ICommand DeleteResultCommand { get; }
        public ICommand SearchResultCommand { get; }

        public MainViewModel()
        {
            _unitOfWork = new UnitOfWork(new SRCContext());
            _dialogService = new DialogService();

            AddPlayerCommand = new RelayCommand(AddPlayer);
            EditPlayerCommand = new RelayCommand(EditPlayer, _ => SelectedPlayer != null);
            DeletePlayerCommand = new RelayCommand(DeletePlayer, _ => SelectedPlayer != null);
            SearchPlayerCommand = new RelayCommand(SearchPlayer);

            AddGameCommand = new RelayCommand(AddGame);
            EditGameCommand = new RelayCommand(EditGame, _ => SelectedGame != null);
            DeleteGameCommand = new RelayCommand(DeleteGame, _ => SelectedGame != null);
            SearchGameCommand = new RelayCommand(SearchGame);

            AddResultCommand = new RelayCommand(AddResult);
            EditResultCommand = new RelayCommand(EditResult, _ => SelectedResult != null);
            DeleteResultCommand = new RelayCommand(DeleteResult, _ => SelectedResult != null);
            SearchResultCommand = new RelayCommand(SearchResult);

            LoadData();
        }

        private void LoadData()
        {
            Players = new ObservableCollection<Player>(_unitOfWork.Players.GetAll());
            Games = new ObservableCollection<Game>(_unitOfWork.Games.GetAll());
            Results = new ObservableCollection<Result>(_unitOfWork.Results.GetAll());
        }

        // PLAYER Methods
        private void AddPlayer(object? obj)
        {
            if (_dialogService.ShowAddPlayerDialog(out string nick, out int age))
            {
                var player = new Player { Nick = nick, Age = age };
                if (player.IsValid())
                {
                    _unitOfWork.Players.Add(player);
                    _unitOfWork.Complete();
                    LoadData();
                }
                else
                {
                    _dialogService.ShowError("The details you've entered aren't valid.", "Invalid details");
                }
            }
        }

        private void EditPlayer(object? obj)
        {
            if (SelectedPlayer == null) return;

            if (_dialogService.ShowEditPlayerDialog(SelectedPlayer.Nick, SelectedPlayer.Age, out string newNick, out int newAge))
            {
                var tempPlayer = new Player { Nick = newNick, Age = newAge };
                if (tempPlayer.IsValid())
                {
                    SelectedPlayer.Nick = newNick;
                    SelectedPlayer.Age = newAge;
                    _unitOfWork.Complete();
                    LoadData();
                }
                else
                {
                    _dialogService.ShowError("The details you've entered aren't valid.", "Invalid details");
                }
            }
        }

        private void DeletePlayer(object? obj)
        {
            if (SelectedPlayer == null) return;
            _unitOfWork.Players.Remove(SelectedPlayer);
            _unitOfWork.Complete();
            LoadData();
        }

        private void SearchPlayer(object? obj)
        {
            int.TryParse(PlayerSearchText, out int age);
            var results = _unitOfWork.Players.Find(x => x.Nick.Contains(PlayerSearchText) || x.Age == age).ToList();
            Players = new ObservableCollection<Player>(results);
        }

        // GAME Methods
        private void AddGame(object? obj)
        {
            if (_dialogService.ShowAddGameDialog(out string name, out string developer, out int releaseYear, out Platform platform, out System.Collections.IList categories))
            {
                var game = new Game
                {
                    Name = name,
                    Developer = developer,
                    ReleaseYear = releaseYear,
                    Platform = _unitOfWork.Platforms.GetAll().Single(x => x.Equals(platform))
                };

                if (game.IsValid() && categories.Count > 0)
                {
                    _unitOfWork.Games.Add(game);
                    foreach (var item in categories)
                    {
                        var gc = new GameCategory
                        {
                            Game = game,
                            Category = _unitOfWork.Categories.GetById(((Category)item).ID)!
                        };
                        _unitOfWork.GameCategories.Add(gc);
                    }
                    _unitOfWork.Complete();
                    LoadData();
                }
                else
                {
                    _dialogService.ShowError("The details you've entered aren't valid.", "Invalid details");
                }
            }
        }

        private void EditGame(object? obj)
        {
            if (SelectedGame == null) return;

            if (_dialogService.ShowEditGameDialog(SelectedGame.Name, SelectedGame.Developer, SelectedGame.ReleaseYear, SelectedGame.Platform, out string newName, out string newDeb, out int newYear, out Platform newPlat, out System.Collections.IList newCats))
            {
                var tempGame = new Game { Name = newName, Developer = newDeb, ReleaseYear = newYear, Platform = newPlat };
                if (tempGame.IsValid() && newCats.Count > 0)
                {
                    SelectedGame.Name = newName;
                    SelectedGame.Developer = newDeb;
                    SelectedGame.ReleaseYear = newYear;
                    SelectedGame.Platform = _unitOfWork.Platforms.GetAll().Single(x => x.Equals(newPlat));

                    var existingCats = _unitOfWork.GameCategories.GetAll().Where(gc => gc.Game.Equals(SelectedGame)).ToList();
                    foreach (var item in existingCats)
                        _unitOfWork.GameCategories.Remove(item);

                    foreach (var item in newCats)
                    {
                        var gc = new GameCategory
                        {
                            Game = SelectedGame,
                            Category = _unitOfWork.Categories.GetById(((Category)item).ID)!
                        };
                        _unitOfWork.GameCategories.Add(gc);
                    }

                    _unitOfWork.Complete();
                    LoadData();
                }
                else
                {
                    _dialogService.ShowError("The details you've entered aren't valid.", "Invalid details");
                }
            }
        }

        private void DeleteGame(object? obj)
        {
            if (SelectedGame == null) return;
            _unitOfWork.Games.Remove(SelectedGame);
            _unitOfWork.Complete();
            LoadData();
        }

        private void SearchGame(object? obj)
        {
            int.TryParse(GameSearchText, out int year);
            var results = _unitOfWork.Games.Find(x => x.Name.Contains(GameSearchText) || x.Developer.Contains(GameSearchText) || x.ReleaseYear == year || x.Platform.Name.Contains(GameSearchText)).ToList();
            Games = new ObservableCollection<Game>(results);
        }

        // RESULT Methods
        private void AddResult(object? obj)
        {
            if (_dialogService.ShowAddResultDialog(out Player player, out Game game, out Category category, out int time, out System.DateTime date))
            {
                var gameCategory = _unitOfWork.GameCategories.GetAll().SingleOrDefault(x => x.Game.Equals(game) && x.Category.Equals(category));
                
                if (gameCategory == null)
                {
                     _dialogService.ShowError("Invalid Game/Category combination.", "Error");
                     return;
                }

                var r = new Result
                {
                    Player = _unitOfWork.Players.GetAll().Single(x => x.Equals(player)),
                    GameCategory = gameCategory,
                    Game = gameCategory.Game,
                    Category = gameCategory.Category,
                    Time = time,
                    Date = date
                };

                if (r.IsValid())
                {
                    _unitOfWork.Results.Add(r);
                    _unitOfWork.Complete();
                    LoadData();
                }
                else
                {
                    _dialogService.ShowError("The details you've entered aren't valid.", "Invalid details");
                }
            }
        }

        private void EditResult(object? obj)
        {
            if (SelectedResult == null) return;

            if (_dialogService.ShowEditResultDialog(SelectedResult.Player, SelectedResult.Game, SelectedResult.Category, SelectedResult.Time, SelectedResult.Date, out int newTime, out System.DateTime newDate))
            {
                var tempResult = new Result 
                { 
                    Player = SelectedResult.Player,
                    GameCategory = SelectedResult.GameCategory,
                    Game = SelectedResult.Game,
                    Category = SelectedResult.Category,
                    Time = newTime, 
                    Date = newDate 
                };

                if (tempResult.IsValid())
                {
                    SelectedResult.Time = newTime;
                    SelectedResult.Date = newDate;
                    _unitOfWork.Complete();
                    LoadData();
                }
                else
                {
                    _dialogService.ShowError("The details you've entered aren't valid.", "Invalid details");
                }
            }
        }

        private void DeleteResult(object? obj)
        {
            if (SelectedResult == null) return;
            _unitOfWork.Results.Remove(SelectedResult);
            _unitOfWork.Complete();
            LoadData();
        }

        private void SearchResult(object? obj)
        {
            int.TryParse(ResultSearchText, out int time);
            var results = _unitOfWork.Results.Find(x => 
                x.Player.Nick.Contains(ResultSearchText) || 
                x.GameCategory.Game.Name.Contains(ResultSearchText) || 
                x.GameCategory.Category.Name.Contains(ResultSearchText) || 
                x.Time == time).ToList();
            Results = new ObservableCollection<Result>(results);
        }
    }
}

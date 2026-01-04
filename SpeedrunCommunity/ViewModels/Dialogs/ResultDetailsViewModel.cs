using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using SpeedrunCommunity.Core;
using SpeedrunCommunity.Core.Services;
using SpeedrunCommunity.Models;

namespace SpeedrunCommunity.ViewModels.Dialogs;

public class ResultDetailsViewModel : ViewModelBase
{
    private readonly List<GameCategory> _allGameCategories;
    public bool IsKeyFieldsEnabled { get; set { field = value; OnPropertyChanged(); } }

    public Player? SelectedPlayer { get; set { field = value; OnPropertyChanged(); } }
    public Game? SelectedGame
    {
        get;
        set
        {
            if (field != value)
            {
                field = value;
                OnPropertyChanged();
                UpdateAvailableCategories();
            }
        }
    }
    public Category? SelectedCategory { get; set { field = value; OnPropertyChanged(); } }
    public int? Time { get; set { field = value; OnPropertyChanged(); } }
    public DateTime Date { get; set { field = value; OnPropertyChanged(); } } = DateTime.Now;

    public ObservableCollection<Player> Players { get; }
    public ObservableCollection<Game> Games { get; }
    public ObservableCollection<Category> AvailableCategories { get; } = [];

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public event EventHandler<bool>? RequestClose;

    public ResultDetailsViewModel(IEnumerable<Player> players, IEnumerable<Game> games, IEnumerable<GameCategory> gameCategories)
    {
        Players = new ObservableCollection<Player>(players);
        Games = new ObservableCollection<Game>(games);
        _allGameCategories = gameCategories.ToList();

        if (Players.Count > 0) SelectedPlayer = Players[0];
        if (Games.Count > 0) SelectedGame = Games[0];

        SaveCommand = new RelayCommand(Save);
        CancelCommand = new RelayCommand(Cancel);
    }

    private void UpdateAvailableCategories()
    {
        AvailableCategories.Clear();
        if (SelectedGame != null)
        {
            var cats = _allGameCategories
                        .Where(gc => gc.GameID == SelectedGame.ID)
                        .Select(gc => gc.Category);

            foreach (var c in cats) 
                AvailableCategories.Add(c);

            if (AvailableCategories.Count > 0) 
                SelectedCategory = AvailableCategories[0];
            else 
                SelectedCategory = null;
        }
    }

    public GameCategory? GetResultGameCategory()
    {
        if (SelectedGame == null || SelectedCategory == null) 
            return null;

        return _allGameCategories.FirstOrDefault(gc => gc.GameID == SelectedGame.ID && gc.CategoryID == SelectedCategory.ID);
    }

    private void Save(object? obj)
    {
        var gc = GetResultGameCategory();
        if (SelectedPlayer == null || gc == null || Time is null or <= 0)
        {
            DialogService.ShowError("Please ensure all fields are valid.\n- Player, Game, and Category must be selected\n- Time must be positive", "Invalid Result");
            return;
        }

        RequestClose?.Invoke(this, true);
    }

    private void Cancel(object? obj) => RequestClose?.Invoke(this, false);
}

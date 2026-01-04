using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using SpeedrunCommunity.Core;
using SpeedrunCommunity.Core.Services;
using SpeedrunCommunity.Models;
using SpeedrunCommunity.Persistence;

namespace SpeedrunCommunity.ViewModels.Pages;

public class ResultViewModel : ViewModelBase
{
    private readonly SRCContext _context;

    public ObservableCollection<Result> Results { get; } = [];
    
    public Result? SelectedResult { get; set { field = value; OnPropertyChanged(); } }
    
    public string SearchText
    {
        get;
        set
        {
            if (field != value)
            {
                field = value;
                OnPropertyChanged();
                ApplyFilter();
            }
        }
    } = string.Empty;

    public ICommand AddCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand DeleteCommand { get; }

    public ResultViewModel(SRCContext context)
    {
        _context = context;

        AddCommand = new RelayCommand(Add);
        EditCommand = new RelayCommand(Edit, _ => SelectedResult != null);
        DeleteCommand = new RelayCommand(Delete, _ => SelectedResult != null);

        LoadData();
    }

    private async void LoadData()
    {
        Results.Clear();
        foreach (var r in await _context.Results
            .Include(r => r.Player)
            .Include(r => r.Game)
            .Include(r => r.Category)
            .ToListAsync())
        {
            Results.Add(r);
        }
    }

    private async void Add(object? obj)
    {
        var players = await _context.Players.ToListAsync();
        var games = await _context.Games.Include(g => g.Platform).ToListAsync();
        var gameCategories = await _context.GameCategories
            .Include(gc => gc.Game)
            .Include(gc => gc.Category)
            .ToListAsync();

        if (DialogService.ShowAddResultDialog(players, games, gameCategories, out Player player, out Game game, out Category category, out int time, out System.DateTime date))
        {
            var gameCategory = gameCategories.SingleOrDefault(x => x.Game.ID == game.ID && x.Category.ID == category.ID);
            
            if (gameCategory == null)
            {
                DialogService.ShowError("Invalid Game/Category combination.", "Error");
                return;
            }

            var r = new Result
            {
                Player = players.Single(x => x.ID == player.ID),
                GameCategory = gameCategory,
                Game = gameCategory.Game,
                Category = gameCategory.Category,
                Time = time,
                Date = date
            };

            if (r.IsValid())
            {
                await _context.Results.AddAsync(r);
                await _context.SaveChangesAsync();
                Results.Add(r);
            }
            else
            {
                DialogService.ShowError("The details you've entered aren't valid.", "Invalid details");
            }
        }
    }

    private async void Edit(object? obj)
    {
        if (SelectedResult == null) return;

        var players = await _context.Players.ToListAsync();
        var games = await _context.Games.Include(g => g.Platform).ToListAsync();
        var gameCategories = await _context.GameCategories
            .Include(gc => gc.Game)
            .Include(gc => gc.Category)
            .ToListAsync();

        if (DialogService.ShowEditResultDialog(SelectedResult.Player, SelectedResult.Game, SelectedResult.Category, SelectedResult.Time, SelectedResult.Date, players, games, gameCategories, out int newTime, out System.DateTime newDate))
        {
            SelectedResult.Time = newTime;
            SelectedResult.Date = newDate;
            
            var temp = new Result 
            { 
                Player = SelectedResult.Player,
                GameCategory = SelectedResult.GameCategory,
                Game = SelectedResult.Game,
                Category = SelectedResult.Category,
                Time = newTime, 
                Date = newDate 
            };

            if (temp.IsValid())
            {
                await _context.SaveChangesAsync();
            }
            else
            {
                DialogService.ShowError("The details you've entered aren't valid.", "Invalid details");
            }
        }
    }

    private async void Delete(object? obj)
    {
        if (SelectedResult == null) return;

        var r = SelectedResult;
        _context.Results.Remove(r);
        await _context.SaveChangesAsync();
        Results.Remove(r);
    }

    private void ApplyFilter()
    {
        var view = CollectionViewSource.GetDefaultView(Results);
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            view.Filter = null;
        }
        else
        {
            view.Filter = item =>
            {
                if (item is Result r)
                {
                    return r.Player.Nick.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase) || 
                           r.Game.Name.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase) || 
                           r.Category.Name.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase) ||
                           r.Time.ToString().Contains(SearchText) ||
                           r.Date.ToString().Contains(SearchText);
                }
                return false;
            };
        }
        view.Refresh();
    }
}

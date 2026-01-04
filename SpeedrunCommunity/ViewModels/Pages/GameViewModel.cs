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

public class GameViewModel : ViewModelBase
{
    private readonly SRCContext _context;

    public ObservableCollection<Game> Games { get; } = [];
    
    public Game? SelectedGame { get; set { field = value; OnPropertyChanged(); } }
    
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

    public GameViewModel(SRCContext context)
    {
        _context = context;

        AddCommand = new RelayCommand(Add);
        EditCommand = new RelayCommand(Edit, _ => SelectedGame != null);
        DeleteCommand = new RelayCommand(Delete, _ => SelectedGame != null);

        LoadData();
    }

    private async void LoadData()
    {
        Games.Clear();
        foreach (var g in await _context.Games.Include(g => g.Platform).ToListAsync())
            Games.Add(g);
    }

    private async void Add(object? obj)
    {
        var platforms = await _context.Platforms.ToListAsync();
        var categories = await _context.Categories.ToListAsync();

        if (DialogService.ShowAddGameDialog(platforms, categories, out string name, out string developer, out int releaseYear, out Platform platform, out System.Collections.Generic.IList<Category> selectedCategories))
        {
            var game = new Game
            {
                Name = name,
                Developer = developer,
                ReleaseYear = releaseYear,
                Platform = platforms.Single(x => x.Equals(platform))
            };
            
            if (game.IsValid() && selectedCategories.Count > 0)
            {
                await _context.Games.AddAsync(game);
                foreach (var cat in selectedCategories)
                {
                    var gc = new GameCategory { Game = game, Category = cat };
                    await _context.GameCategories.AddAsync(gc);
                }
                await _context.SaveChangesAsync();
                Games.Add(game);
            }
            else
            {
                DialogService.ShowError("The details you've entered aren't valid.", "Invalid details");
            }
        }
    }

    private async void Edit(object? obj)
    {
        if (SelectedGame == null) return;

        var allGameCategories = await _context.GameCategories
            .Include(gc => gc.Game)
            .Include(gc => gc.Category)
            .ToListAsync();
            
        var currentCategories = allGameCategories
            .Where(gc => gc.Game.Equals(SelectedGame))
            .Select(gc => gc.Category)
            .ToList();

        var platforms = await _context.Platforms.ToListAsync();
        var allCategories = await _context.Categories.ToListAsync();

        if (DialogService.ShowEditGameDialog(SelectedGame.Name, SelectedGame.Developer, SelectedGame.ReleaseYear, SelectedGame.Platform, currentCategories, platforms, allCategories, out string newName, out string newDev, out int newYear, out Platform newPlat, out System.Collections.Generic.IList<Category> newCats))
        {
            var temp = new Game { Name = newName, Developer = newDev, ReleaseYear = newYear, Platform = newPlat };
            if (temp.IsValid() && newCats.Count > 0)
            {
                SelectedGame.Name = newName;
                SelectedGame.Developer = newDev;
                SelectedGame.ReleaseYear = newYear;
                SelectedGame.Platform = platforms.Single(x => x.Equals(newPlat));

                var existingCats = allGameCategories.Where(gc => gc.Game.Equals(SelectedGame)).ToList();
                foreach (var item in existingCats)
                    _context.GameCategories.Remove(item);

                foreach (var cat in newCats)
                {
                    var gc = new GameCategory { Game = SelectedGame, Category = cat };
                    await _context.GameCategories.AddAsync(gc);
                }

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
        if (SelectedGame == null) return;
        
        var g = SelectedGame;
        _context.Games.Remove(g);
        await _context.SaveChangesAsync();
        Games.Remove(g);
    }

    private void ApplyFilter()
    {
        var view = CollectionViewSource.GetDefaultView(Games);
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            view.Filter = null;
        }
        else
        {
            view.Filter = item =>
            {
                if (item is Game g)
                    return g.Name.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase);
                return false;
            };
        }
        view.Refresh();
    }
}

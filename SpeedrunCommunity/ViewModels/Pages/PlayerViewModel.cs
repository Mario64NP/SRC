using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using SpeedrunCommunity.Core;
using SpeedrunCommunity.Core.Services;
using SpeedrunCommunity.Models;
using SpeedrunCommunity.Persistence;

namespace SpeedrunCommunity.ViewModels.Pages;

public class PlayerViewModel : ViewModelBase
{
    private readonly SRCContext _context;

    public ObservableCollection<Player> Players { get; } = [];
    
    public Player? SelectedPlayer { get; set { field = value; OnPropertyChanged(); } }
    
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

    public PlayerViewModel(SRCContext context)
    {
        _context = context;

        AddCommand = new RelayCommand(Add);
        EditCommand = new RelayCommand(Edit, _ => SelectedPlayer != null);
        DeleteCommand = new RelayCommand(Delete, _ => SelectedPlayer != null);

        LoadData();
    }

    private async void LoadData()
    {
        Players.Clear();
        foreach (var p in await _context.Players.ToListAsync())
            Players.Add(p);
    }

    private async void Add(object? obj)
    {
        if (DialogService.ShowAddPlayerDialog(out string nick, out int age))
        {
            var player = new Player { Nick = nick, Age = age };
            if (player.IsValid())
            {
                await _context.Players.AddAsync(player);
                await _context.SaveChangesAsync();
                Players.Add(player);
            }
            else
            {
                DialogService.ShowError("The details you've entered aren't valid.", "Invalid details");
            }
        }
    }

    private async void Edit(object? obj)
    {
        if (SelectedPlayer == null) return;

        if (DialogService.ShowEditPlayerDialog(SelectedPlayer.Nick, SelectedPlayer.Age, out string newNick, out int newAge))
        {
            var temp = new Player { Nick = newNick, Age = newAge };
            if (temp.IsValid())
            {
                SelectedPlayer.Nick = newNick;
                SelectedPlayer.Age = newAge;
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
        if (SelectedPlayer == null) return;

        var p = SelectedPlayer;
        _context.Players.Remove(p);
        await _context.SaveChangesAsync();
        Players.Remove(p);
    }

    private void ApplyFilter()
    {
        var view = CollectionViewSource.GetDefaultView(Players);
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            view.Filter = null;
        }
        else
        {
            _ = int.TryParse(SearchText, out int age);
            view.Filter = item =>
            {
                if (item is Player p)
                    return p.Nick.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase) || p.Age == age;
                return false;
            };
        }
        view.Refresh();
    }
}

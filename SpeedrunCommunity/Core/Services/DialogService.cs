using System.Collections.Generic;
using System.Windows;
using SpeedrunCommunity.Models;
using SpeedrunCommunity.Views.Dialogs;
using SpeedrunCommunity.ViewModels.Dialogs;

namespace SpeedrunCommunity.Core.Services;

public class DialogService
{
    private static Window GetOwner() => Application.Current.MainWindow;

    public static bool ShowAddPlayerDialog(out string nick, out int age)
    {
        var vm = new PlayerDetailsViewModel();
        var win = new PlayerDetails { DataContext = vm, Owner = GetOwner() };
        vm.RequestClose += (s, e) => { win.DialogResult = e; win.Close(); };

        if (win.ShowDialog() == true)
        {
            nick = vm.Nick;
            age = vm.Age!.Value;

            return true;
        }

        nick = string.Empty;
        age = 0;

        return false;
    }

    public static bool ShowEditPlayerDialog(string currentNick, int currentAge, out string newNick, out int newAge)
    {
        var vm = new PlayerDetailsViewModel() { Nick = currentNick, Age = currentAge };
        var win = new PlayerDetails { DataContext = vm, Owner = GetOwner() };
        vm.RequestClose += (s, e) => { win.DialogResult = e; win.Close(); };

        if (win.ShowDialog() == true)
        {
            newNick = vm.Nick;
            newAge = vm.Age!.Value;

            return true;
        }

        newNick = string.Empty;
        newAge = 0;

        return false;
    }

    public static bool ShowAddGameDialog(IEnumerable<Platform> platforms, IEnumerable<Category> categories, out string name, out string developer, out int releaseYear, out Platform platform, out IList<Category> selectedCategories)
    {
        var vm = new GameDetailsViewModel(platforms, categories, []);
        var win = new GameDetails { DataContext = vm, Owner = GetOwner() };
        vm.RequestClose += (s, e) => { win.DialogResult = e; win.Close(); };

        if (win.ShowDialog() == true)
        {
            name = vm.Name;
            developer = vm.Developer;
            releaseYear = vm.ReleaseYear!.Value;
            platform = vm.SelectedPlatform!;
            selectedCategories = vm.GetSelectedCategories();

            return true;
        }

        name = string.Empty;
        developer = string.Empty;
        releaseYear = 0;
        platform = null!;
        selectedCategories = null!;

        return false;
    }

    public static bool ShowEditGameDialog(string name, string developer, int releaseYear, Platform platform, IEnumerable<Category> currentCategories, IEnumerable<Platform> allPlatforms, IEnumerable<Category> allCategories, out string newName, out string newDeb, out int newYear, out Platform newPlat, out IList<Category> newCats)
    {
        var vm = new GameDetailsViewModel(allPlatforms, allCategories, currentCategories) 
        { 
            Name = name, 
            Developer = developer, 
            ReleaseYear = releaseYear,
            SelectedPlatform = platform 
        };
        var win = new GameDetails { DataContext = vm, Owner = GetOwner() };
        vm.RequestClose += (s, e) => { win.DialogResult = e; win.Close(); };

        if (win.ShowDialog() == true)
        {
            newName = vm.Name;
            newDeb = vm.Developer;
            newYear = vm.ReleaseYear!.Value;
            newPlat = vm.SelectedPlatform!;
            newCats = vm.GetSelectedCategories();

            return true;
        }

        newName = string.Empty;
        newDeb = string.Empty;
        newYear = 0;
        newPlat = null!;
        newCats = null!;

        return false;
    }

    public static bool ShowAddResultDialog(IEnumerable<Player> players, IEnumerable<Game> games, IEnumerable<GameCategory> gameCategories, out Player player, out Game game, out Category category, out int time, out System.DateTime date)
    {
        var vm = new ResultDetailsViewModel(players, games, gameCategories) { IsKeyFieldsEnabled = true };
        var win = new ResultDetails { DataContext = vm, Owner = GetOwner() };
        vm.RequestClose += (s, e) => { win.DialogResult = e; win.Close(); };

        if (win.ShowDialog() == true)
        {
            player = vm.SelectedPlayer!;
            game = vm.SelectedGame!;
            category = vm.SelectedCategory!;
            time = vm.Time!.Value;
            date = vm.Date;

            return true;
        }

        player = null!;
        game = null!;
        category = null!;
        time = 0;
        date = System.DateTime.MinValue;

        return false;
    }

    public static bool ShowEditResultDialog(Player player, Game game, Category category, int time, System.DateTime date, IEnumerable<Player> allPlayers, IEnumerable<Game> allGames, IEnumerable<GameCategory> allGameCategories, out int newTime, out System.DateTime newDate)
    {
        var vm = new ResultDetailsViewModel(allPlayers, allGames, allGameCategories)
        {
            SelectedPlayer = player,
            SelectedGame = game,
            SelectedCategory = category,
            Time = time,
            Date = date,
            IsKeyFieldsEnabled = false
        };  
        var win = new ResultDetails { DataContext = vm, Owner = GetOwner() };  
        vm.RequestClose += (s, e) => { win.DialogResult = e; win.Close(); };

        if (win.ShowDialog() == true)
        {
            newTime = vm.Time!.Value;
            newDate = vm.Date;

            return true;
        }

        newTime = 0;
        newDate = System.DateTime.MinValue;

        return false;
    }

    public static void ShowError(string message, string title) => MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
}

using System.Collections;
using System.Collections.Generic;
using System.Windows;
using SpeedrunCommunity.Domain;
using SpeedrunCommunity.View;

namespace SpeedrunCommunity.Core.Services
{
    public class DialogService : IDialogService
    {
        private Window GetOwner()
        {
            return Application.Current.MainWindow;
        }

        public bool ShowAddPlayerDialog(out string nick, out int age)
        {
            PlayerDetails dialog = new PlayerDetails
            {
                Title = "Add a new player",
                Owner = GetOwner()
            };

            bool result = dialog.ShowDialog() == true;
            nick = dialog.Nick;
            age = dialog.Age;
            return result;
        }

        public bool ShowEditPlayerDialog(string currentNick, int currentAge, out string newNick, out int newAge)
        {
            PlayerDetails dialog = new PlayerDetails
            {
                Title = "Edit a player",
                Owner = GetOwner(),
                Nick = currentNick,
                Age = currentAge
            };

            bool result = dialog.ShowDialog() == true;
            newNick = dialog.Nick;
            newAge = dialog.Age;
            return result;
        }

        public bool ShowAddGameDialog(out string name, out string developer, out int releaseYear, out Platform platform, out IList selectedCategories)
        {
            GameDetails dialog = new GameDetails
            {
                Title = "Add a new game",
                Owner = GetOwner()
            };

            bool result = dialog.ShowDialog() == true;
            name = dialog.Name;
            developer = dialog.Developer;
            releaseYear = dialog.ReleaseYear;
            platform = dialog.Platform;
            selectedCategories = dialog.SelectedCategories;
            return result;
        }

        public bool ShowEditGameDialog(string currentName, string currentDeveloper, int currentReleaseYear, Platform currentPlatform, IList existingCategories, out string newName, out string newDeveloper, out int newReleaseYear, out Platform newPlatform, out IList selectedCategories)
        {
            GameDetails dialog = new GameDetails
            {
                Title = "Edit a game",
                Owner = GetOwner(),
                Name = currentName,
                Developer = currentDeveloper,
                ReleaseYear = currentReleaseYear,
                Platform = currentPlatform,
            };
            
            if (existingCategories is IEnumerable<Category> categories)
                dialog.SetSelectedCategories(categories);

            bool result = dialog.ShowDialog() == true;
            newName = dialog.Name;
            newDeveloper = dialog.Developer;
            newReleaseYear = dialog.ReleaseYear;
            newPlatform = dialog.Platform;
            selectedCategories = dialog.SelectedCategories;
            return result;
        }

        public bool ShowAddResultDialog(out Player player, out Game game, out Category category, out int time, out System.DateTime date)
        {
            ResultDetails dialog = new ResultDetails
            {
                Title = "Add a new result",
                Owner = GetOwner()
            };

            bool result = dialog.ShowDialog() == true;
            player = dialog.Player;
            game = dialog.Game;
            category = dialog.Category;
            time = dialog.Time;
            date = dialog.Date;
            return result;
        }

        public bool ShowEditResultDialog(Player currentPlayer, Game currentGame, Category currentCategory, int currentTime, System.DateTime currentDate, out int newTime, out System.DateTime newDate)
        {
            ResultDetails dialog = new ResultDetails
            {
                Title = "Edit a result",
                Owner = GetOwner(),
                Player = currentPlayer,
                Game = currentGame,
                Category = currentCategory,
                Time = currentTime,
                Date = currentDate
            };
            dialog.DisableEditingKeyFields();

            bool result = dialog.ShowDialog() == true;
            newTime = dialog.Time;
            newDate = dialog.Date;
            return result;
        }

        public void ShowError(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}

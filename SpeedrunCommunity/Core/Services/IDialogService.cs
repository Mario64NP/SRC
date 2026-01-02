using System.Collections;
using SpeedrunCommunity.Domain;

namespace SpeedrunCommunity.Core.Services
{
    public interface IDialogService
    {
        bool ShowAddPlayerDialog(out string nick, out int age);
        bool ShowEditPlayerDialog(string currentNick, int currentAge, out string newNick, out int newAge);
        
        bool ShowAddGameDialog(out string name, out string developer, out int releaseYear, out Platform platform, out IList selectedCategories);
        bool ShowEditGameDialog(string currentName, string currentDeveloper, int currentReleaseYear, Platform currentPlatform, out string newName, out string newDeveloper, out int newReleaseYear, out Platform newPlatform, out IList selectedCategories);

        bool ShowAddResultDialog(out Player player, out Game game, out Category category, out int time, out System.DateTime date);
        bool ShowEditResultDialog(Player currentPlayer, Game currentGame, Category currentCategory, int currentTime, System.DateTime currentDate, out int newTime, out System.DateTime newDate);
        
        void ShowError(string message, string title);
    }
}

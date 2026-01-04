namespace SpeedrunCommunity.Models;

public class GameCategory
{
    public int GameID { get; set; }
    public required Game Game { get; set; }
    public int CategoryID { get; set; }
    public required Category Category { get; set; }
}

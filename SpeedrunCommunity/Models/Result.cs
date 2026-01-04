using System;

namespace SpeedrunCommunity.Models;

public class Result : BaseModel
{
    public int PlayerID { get; set; }
    public required Player Player { get; set; }
    public required GameCategory GameCategory { get; set; }
    public int GameID { get; set; }
    public required Game Game { get; set; }
    public int CategoryID { get; set; }
    public required Category Category { get; set; }
    public int Time { get; set => SetProperty(ref field, value); }
    public DateTime Date { get; set => SetProperty(ref field, value); }

    public bool IsValid() => Player != null && GameCategory != null && Time > 0 && Date > DateTime.Parse("1.1.1958.") && Date < DateTime.Now;
}

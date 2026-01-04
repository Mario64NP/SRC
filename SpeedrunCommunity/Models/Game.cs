namespace SpeedrunCommunity.Models;

public class Game : BaseModel
{
    public int ID { get; set; }
    public required string Name { get; set => SetProperty(ref field, value); }
    public required string Developer { get; set => SetProperty(ref field, value); }
    public int ReleaseYear { get; set => SetProperty(ref field, value); }
    public required Platform Platform { get; set => SetProperty(ref field, value); }
    public override string ToString() => Name;
    public override bool Equals(object? obj)
    {
        if (obj == null || obj is not Game g)
            return false;
        else
            return g.ID == ID;
    }

    public override int GetHashCode() => ID.GetHashCode();

    public bool IsValid() => !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Developer) && ReleaseYear > 1958 && Platform != null;
}

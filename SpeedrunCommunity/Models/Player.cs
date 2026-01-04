namespace SpeedrunCommunity.Models;

public class Player : BaseModel
{
    public int ID { get; set; }
    public required string Nick { get; set => SetProperty(ref field, value); }
    public int Age { get; set => SetProperty(ref field, value); }

    public override string ToString() => Nick;

    public override bool Equals(object? obj)
    {
        if (obj == null || obj is not Player p)
            return false;
        else
            return p.ID == ID;
    }

    public override int GetHashCode() => ID.GetHashCode();

    public bool IsValid() => !string.IsNullOrEmpty(Nick) && Age > 13;
}

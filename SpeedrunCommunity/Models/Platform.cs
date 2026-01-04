namespace SpeedrunCommunity.Models;

public class Platform
{
    public int ID { get; set; }
    public required string Name { get; set; }

    public override string ToString() => Name;

    public override bool Equals(object? obj)
    {
        if (obj == null || obj is not Platform p)
            return false;
        else
            return p.ID == ID;
    }

    public override int GetHashCode() => ID.GetHashCode();
}

namespace SpeedrunCommunity.Models;

public class Category
{
    public int ID { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }

    public override string ToString() => Name;

    public override bool Equals(object? obj)
    {
        if (obj == null || obj is not Category c)
            return false;
        else
            return c.ID == ID;
    }

    public override int GetHashCode() => ID.GetHashCode();
}

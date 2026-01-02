namespace SpeedrunCommunity.Domain
{
    public class Game
    {
        public int ID { get; set; }
        public required string Name { get; set; }
        public required string Developer { get; set; }
        public int ReleaseYear { get; set; }
        public required Platform Platform { get; set; }
        public override string ToString()
        {
            return Name;
        }
        public override bool Equals(object? obj)
        {
            if (obj == null || obj is not Game g)
                return false;
            else
                return g.ID == ID;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Developer) && ReleaseYear > 1958 && Platform != null;
        }
    }
}

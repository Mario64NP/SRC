namespace WpfApp.Domain
{
    public class Player
    {
        public int ID { get; set; }
        public string Nick { get; set; }
        public int Age { get; set; }

        public override string ToString()
        {
            return Nick;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj is not Player p)
                return false;
            else
                return p.ID == ID;
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Nick) && Age > 13;
        }
    }
}
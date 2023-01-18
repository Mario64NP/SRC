namespace WpfApp.Domain
{
    public class Platform
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj is not Platform p)
                return false;
            else
                return p.ID == ID;
        }
    }
}
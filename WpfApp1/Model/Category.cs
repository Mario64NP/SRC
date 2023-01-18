namespace WpfApp.Domain
{
    public class Category
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj is not Category c)
                return false;
            else
                return c.ID == ID;
        }
    }
}
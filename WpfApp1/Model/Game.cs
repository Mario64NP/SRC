namespace WpfApp.Domain
{
    public class Game
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Developer { get; set; }
        public int ReleaseYear { get; set; }
        public Platform Platform { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
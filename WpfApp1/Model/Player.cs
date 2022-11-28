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

    }
}
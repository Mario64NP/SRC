using System;

namespace WpfApp.Domain
{
    public class Result
    {
        public int PlayerID { get; set; }
        public Player Player { get; set; }
        public GameCategory GameCategory { get; set; }
        public int GameID { get; set; }
        public Game Game { get; set; }
        public int CategoryID { get; set; }
        public Category Category { get; set; }
        public int Time { get; set; }
        public DateTime Date { get; set; }

        public bool IsValid()
        {
            return Player != null && GameCategory != null && Time > 0 && Date > DateTime.Parse("1.1.1958.") && Date < DateTime.Now;
        }
    }
}
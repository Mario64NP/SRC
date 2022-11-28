using System;

namespace WpfApp.Domain
{
    public class Result
    {
        public int PlayerID { get; set; }
        public Player Player { get; set; }
        public int GameID { get; set; }
        public Game Game { get; set; }
        public int CategoryID { get; set; }
        public Category Category { get; set; }
        public int Time { get; set; }
        public DateTime Date { get; set; }
    }
}
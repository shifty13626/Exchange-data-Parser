using CsvHelper.Configuration.Attributes;
using System.Collections.Generic;

namespace DataParser.Entities {
    public class GameSales {
        public int Rank { get; set; }
        public string Name { get; set; }
        public string Platform { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
        public string Publisher { get; set; }
        public double NaSales { get; set; }
        public double EuSales { get; set; }
        public double JpSales { get; set; }
        public double OtherSales { get; set; }
        public double GlobalSales { get; set; }
        /// <summary>
        /// Rating
        /// Source : IGDB
        /// </summary>
        public double RatingAPI { get; set; }
        /// <summary>
        /// List of game mode for the game
        /// Source : IGDB
        /// </summary>
        public List<string> GameModes { get; set; }
        /// <summary>
        /// List of all company to develop the game
        /// Source : IGDB
        /// </summary>
        public List<string> Company { get; set; }
        /// <summary>
        /// List of player perspective of this game
        /// Source : IGDB
        /// </summary>
        public List<string> PlayerPerspective { get; set; }

        /// <summary>
        /// Category for game
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// winner of award
        /// </summary>
        public bool Winner { get; set; }
        /// <summary>
        /// People have voted
        /// </summary>
        public string Voted { get; set; }





        /// <summary>
        /// constructor
        /// </summary>
        public GameSales()
        {
            GameModes = new List<string>();
            Company = new List<string>();
            PlayerPerspective = new List<string>();
        }
    }
}
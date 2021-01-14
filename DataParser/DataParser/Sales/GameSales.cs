using CsvHelper.Configuration.Attributes;

namespace DataParser.Sales {
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
    }
}
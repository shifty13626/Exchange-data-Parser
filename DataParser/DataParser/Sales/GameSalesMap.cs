using CsvHelper.Configuration;
using DataParser.Entities;

namespace DataParser.Sales {
    public class GameSalesMap : ClassMap<GameSales>
    {
        public GameSalesMap()
        {
            Map(m => m.Rank);
            Map(m => m.Name);
            Map(m => m.Platform);
            Map(m => m.Year).ConvertUsing(row => row.GetField("Year") == "N/A" ? -1 : int.Parse(row.GetField("Year")));
            Map(m => m.Genre);
            Map(m => m.Publisher);
            Map(m => m.NaSales).ConvertUsing(row => row.GetField<double>("NA_Sales"));
            Map(m => m.EuSales).ConvertUsing(row => row.GetField<double>("EU_Sales"));
            Map(m => m.JpSales).ConvertUsing(row => row.GetField<double>("JP_Sales"));
            Map(m => m.OtherSales).ConvertUsing(row => row.GetField<double>("Other_Sales"));
            Map(m => m.GlobalSales).ConvertUsing(row => row.GetField<double>("Global_Sales"));
        }
    }
}
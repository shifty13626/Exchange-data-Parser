using CsvHelper.Configuration;

namespace DataParser.Awards
{
    public class GameAwardsMap : ClassMap<GameAwards>
    {
        public GameAwardsMap()
        {
            Map(m => m.Year).ConvertUsing(row => row.GetField<double>("year"));
            Map(m => m.Category).ConvertUsing(row => row.GetField<string>("category"));
            Map(m => m.Company).ConvertUsing(row => row.GetField<string>("company"));
            Map(m => m.Nominee).ConvertUsing(row => row.GetField<string>("nominee"));
            Map(m => m.Winner).ConvertUsing(row => row.GetField<bool>("winner"));
            Map(m => m.Voted).ConvertUsing(row => row.GetField<string>("voted"));
        }
    }
}
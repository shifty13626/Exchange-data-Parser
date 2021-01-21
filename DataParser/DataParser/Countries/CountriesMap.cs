using CsvHelper.Configuration;
using DataParser.Entities;

namespace DataParser.Sales {
    public class CountriesMap : ClassMap<Country>
    {
        public CountriesMap()
        {
            Map(m => m.Name).ConvertUsing(row => row.GetField<string>("Country"));
            Map(m => m.Indicator);
            Map(m => m.Unit);
            Map(m => m.Date);
            Map(m => m.Value);
        }
    }
}
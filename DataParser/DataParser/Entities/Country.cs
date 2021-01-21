using CsvHelper.Configuration.Attributes;
using System.Collections.Generic;

namespace DataParser.Entities {
    public class Country
    {
        public string Name { get; set; }
        public string Indicator { get; set; }
        public string Unit { get; set; }
        public string Date { get; set; }
        public double Value { get; set; }
    }
}
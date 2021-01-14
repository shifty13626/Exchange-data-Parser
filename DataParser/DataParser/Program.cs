using DataParser.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using DataParser.Sales;

namespace DataParser
{
    class Program
    {
        private static ConfigLoader configLoader;
        private static RequestManager requestManager;

        static void Main(string[] args) 
        {
            // Load config
            configLoader = new ConfigLoader();
            var config = configLoader.LoadConfig(Path.Combine(Environment.CurrentDirectory, "config.xml"));

            // create request manager
            requestManager = new RequestManager();

            // get info CyberPunk 2077
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "retour.json"), requestManager.GetGameInfo(config, "Cyberpunk 2077"));

            Console.WriteLine("Parsing done");

            IList<GameSales> gameList;
            
            using (var reader = new StreamReader(Path.Combine("dataset", "vgsales.csv")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) {
                csv.Configuration.RegisterClassMap<GameSalesMap>();
                gameList = csv.GetRecords<GameSales>().ToList();
            }
        }
    }
}

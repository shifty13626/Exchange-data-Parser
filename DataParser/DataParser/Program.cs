using DataParser.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using DataParser.Awards;
using DataParser.Sales;
using DataParser.Entities;

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

            IList<Country> countriesList;
            
            using (var reader = new StreamReader(Path.Combine("dataset", "topCountries.csv")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) {
                csv.Configuration.RegisterClassMap<CountriesMap>();
                countriesList = csv.GetRecords<Country>().ToList();
            }
            
            IList<GameSales> gameList;

            using (var reader = new StreamReader(Path.Combine("dataset", "vgsales.csv")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) {
                csv.Configuration.RegisterClassMap<GameSalesMap>();
                gameList = csv.GetRecords<GameSales>().ToList();
            }

            IList<GameAwards> awardList;
            
            using (var reader = new StreamReader(Path.Combine("dataset", "the_game_awards.csv")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) {
                csv.Configuration.RegisterClassMap<GameAwardsMap>();
                awardList = csv.GetRecords<GameAwards>().ToList();
            }

            // get all games from API
            for(int i= 0; i < 1; i++)
            {
                requestManager.GetGameInfo(config, gameList.ElementAt(i));
            }
            
            Console.WriteLine("Parsing done");

            
        }
    }
}

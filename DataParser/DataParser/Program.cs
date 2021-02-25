using DataParser.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using DataParser.Sales;
using DataParser.Entities;
using Newtonsoft.Json;
using System.Diagnostics;

namespace DataParser
{
    class Program
    {
        private static ConfigLoader configLoader;
        private static RequestManager requestManager;

        static void Main(string[] args) 
        {
            // Create chrono
            Stopwatch chronoTotal = new Stopwatch();
            Stopwatch chronoRequest = new Stopwatch();
            chronoTotal.Start();

            // Load config
            configLoader = new ConfigLoader();
            var config = configLoader.LoadConfig(Path.Combine(Environment.CurrentDirectory, "config.xml"));

            // create request manager
            requestManager = new RequestManager();

            config.Token = requestManager.RefreshToken(config);
            if(config.Token.Equals(null))
            {
                Console.WriteLine("Can't get token of this account. End execution");
                return;
            }

            IList<GameSales> gameList;
            
            using (var reader = new StreamReader(Path.Combine("dataset", "vgsales.csv")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) {
                csv.Configuration.RegisterClassMap<GameSalesMap>();
                gameList = csv.GetRecords<GameSales>().ToList();
            }


            IList<GameAwards> awardList;

            using (var reader = new StreamReader(Path.Combine("dataset", "the_game_awards.csv")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.RegisterClassMap<GameAwardsMap>();
                awardList = csv.GetRecords<GameAwards>().ToList();
            }



            // get all games from API
            var cpt = 1;
            foreach(var game in gameList)
            {
                chronoRequest.Start();
                Console.Write("Search data for game : \"" + game.Name + "\" (" +cpt +"/" +gameList.Count +") in IGDB API. ==> ");
                if (requestManager.GetGameInfo(config, game))
                    Console.Write("Game treat.");
                else
                    Console.Write("Game NOT treat.");
                chronoRequest.Stop();
                Console.WriteLine(" ({0:hh\\:mm\\:ss\\.fffffff})", chronoRequest.Elapsed);

                cpt++;
            }
                
            /*
            gameList = 
            from game in gameList
            join awardedGame in awardList
            on game.Name equals awardedGame.Nominee into toto
            from p in toto.DefaultIfEmpty()
            select game.Winner, game.
            */

            // Join games with awards
            foreach (var award in awardList)
            {

                

                
                var gameSelected = gameList.FirstOrDefault(x => x.Name.Equals(award.Nominee));
                if(gameSelected != null)
                {
                    gameSelected.Winner = award.Winner;
                    gameSelected.Voted = award.Voted;
                }
                
            }


            // Write all game in result files
            // create folder result
            if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, "results")))
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "results"));
            // write as Json
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "results", "result.json"), JsonConvert.SerializeObject(gameList));
            // write as csv
            using (var writer = new StreamWriter(Path.Combine(Environment.CurrentDirectory, "results", "result.csv")))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(gameList);
            }

            // Write statistics
            Console.WriteLine("Game found : " + gameList.Count);
            Console.WriteLine("Game created between 80' and 90' : " +gameList.Count(x => x.Year >= 1980 && x.Year <= 1989));
            Console.WriteLine("Game created between 90' and 2000 : " +gameList.Count(x => x.Year >= 1990 && x.Year <= 1999));
            Console.WriteLine("Game created between 2000 and 2010 : " +gameList.Count(x => x.Year >= 2000 && x.Year <= 2009));
            Console.WriteLine("Game created between 2010 and 2020 : " +gameList.Count(x => x.Year >= 2010 && x.Year <= 2019));

            // End chrono
            chronoTotal.Stop();
            Console.WriteLine("Time elapsed: {0:hh\\:mm\\:ss\\.fffffff}", chronoTotal.Elapsed);
            Console.WriteLine("Parsing done");
        }
    }
}

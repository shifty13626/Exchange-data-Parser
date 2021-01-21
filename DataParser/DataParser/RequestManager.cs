using DataParser.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;
using Newtonsoft.Json.Linq;
using DataParser.Entities;
using Newtonsoft.Json;

namespace DataParser
{
    public class RequestManager
    {
        /// <summary>
        /// Function to get information about a game
        /// Source : IGDB
        /// </summary>
        /// <param name="config"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        public bool GetGameInfo(Config config, GameSales game)
        {
            try
            {
                ////////////////////////////////////////////////
                /////////////// POSTMAN Request ////////////////
                var client = new RestClient(config.AddressAPI +"v4/games");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Client-ID", config.ClientID);
                request.AddHeader("Authorization", "Bearer " +config.Token);
                request.AddHeader("Content-Type", "text/plain");
                request.AddHeader("Cookie", "__cfduid=dc76395227eec70d487cb139cbaa634161610457073");
                request.AddParameter("text/plain", "fields *;\r\nlimit 100;\r\nwhere name = \"" +game.Name +"\";", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (JArray.Parse(response.Content).Count.Equals(0))
                    return false;

                // parse result and build dico
                game.RatingAPI = GetKeyRating(response.Content);

                // get list of game mode
                game.GameModes = GetAllGameMode(config, response.Content);

                // get list of company
                game.Company = GetAllCompany(config, response.Content);

                // get player perspective
                game.PlayerPerspective = GetPlayerPerspective(config, response.Content);

                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// Search rating on Json return from API
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public double GetKeyRating(string json)
        {
            try
            {
                JToken objectSearched;
                JArray JArray = JArray.Parse(json);
                var rss = JArray.ToObject<List<JObject>>().FirstOrDefault();
                if (rss.TryGetValue("aggregated_rating", out objectSearched))
                    return (double)objectSearched;
                else
                    return 0.00;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return 0;
            }
        }

        /// <summary>
        /// Get list string of all gamemode of this game
        /// </summary>
        /// <param name="config"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public List<string> GetAllGameMode(Config config, string json)
        {
            try
            {
                List<int> listIdGameMode = new List<int>();
                List<string> listGameMode = new List<string>();

                // get all id of game mode
                JToken objectSearched;
                JArray JArray = JArray.Parse(json);
                var rss = JArray.ToObject<List<JObject>>().FirstOrDefault();
                if (rss.TryGetValue("game_modes", out objectSearched))
                    foreach (var value in objectSearched)
                        listIdGameMode.Add((int)value);
                else
                    return null;

                // requesto to get all gameMode
                foreach(var GameModeKey in listIdGameMode)
                {
                    var client = new RestClient(config.AddressAPI + "v4/game_modes");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Client-ID", config.ClientID);
                    request.AddHeader("Authorization", "Bearer " +config.Token);
                    request.AddHeader("Content-Type", "text/plain");
                    request.AddHeader("Cookie", "__cfduid=df1e5c35c64b02b17a3efeb129ff7ecf41610732111");
                    request.AddParameter("text/plain", "fields *;\r\nwhere id = " +GameModeKey +";", ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);

                    JArray rss2 = JArray.Parse(response.Content);
                    var jsonObjects2 = rss2.OfType<JObject>().ToList();
                    listGameMode.Add((string)jsonObjects2[0]["name"]);
                }
                return listGameMode;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// Get list of campany worked on the game
        /// </summary>
        /// <param name="config"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public List<string> GetAllCompany(Config config, string json)
        {
            try
            {

                List<int> listIdInvolvedComapny = new List<int>();
                List<string> listcompany = new List<string>();

                // get all id of involved company
                JToken objectSearched;
                JArray JArray = JArray.Parse(json);
                var rss = JArray.ToObject<List<JObject>>().FirstOrDefault();
                if (rss.TryGetValue("involved_companies", out objectSearched))
                    foreach (var value in objectSearched)
                        listIdInvolvedComapny.Add((int)value);
                else
                    return null;

                // get id of all company
                foreach(var involvedCompany in listIdInvolvedComapny)
                {
                    // get involved company
                    var client = new RestClient(config.AddressAPI +"v4/involved_companies");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Client-ID", config.ClientID);
                    request.AddHeader("Authorization", "Bearer " +config.Token);
                    request.AddHeader("Content-Type", "text/plain");
                    request.AddHeader("Cookie", "__cfduid=df1e5c35c64b02b17a3efeb129ff7ecf41610732111");
                    request.AddParameter("text/plain", "fields *;\r\nwhere id = " +involvedCompany +";", ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);

                    // get id company
                    JArray resultRequestInvolvedCompany = JArray.Parse(response.Content);
                    var jsonObjectsInvolvedCompany = resultRequestInvolvedCompany.OfType<JObject>().ToList();
                    var idCompany = jsonObjectsInvolvedCompany[0]["company"];

                    // get company
                    client = new RestClient(config.AddressAPI +"v4/companies");
                    client.Timeout = -1;
                    request = new RestRequest(Method.POST);
                    request.AddHeader("Client-ID", config.ClientID);
                    request.AddHeader("Authorization", "Bearer " +config.Token);
                    request.AddHeader("Content-Type", "text/plain");
                    request.AddHeader("Cookie", "__cfduid=df1e5c35c64b02b17a3efeb129ff7ecf41610732111");
                    request.AddParameter("text/plain", "fields *;\r\nwhere id = " +idCompany +";", ParameterType.RequestBody);
                    response = client.Execute(request);

                    JArray resultRequestcompany = JArray.Parse(response.Content);
                    var jsonObjectsCompany = resultRequestcompany.OfType<JObject>().ToList();
                    listcompany.Add((string)jsonObjectsCompany[0]["name"]);
                }

                return listcompany;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// Get al player perspective for this game
        /// </summary>
        /// <param name="config"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public List<string> GetPlayerPerspective(Config config, string json)
        {
            try
            {
                List<int> listIdplayerPerspective = new List<int>();
                List<string> listPlayerPerspective = new List<string>();

                // get all id of perspective
                JToken objectSearched;
                JArray JArray = JArray.Parse(json);
                var rss = JArray.ToObject<List<JObject>>().FirstOrDefault();
                if (rss.TryGetValue("player_perspectives", out objectSearched))
                    foreach (var value in objectSearched)
                        listIdplayerPerspective.Add((int)value);
                else
                    return null;

                // requesto to get all gameMode
                foreach (var perspective in listIdplayerPerspective)
                {
                    var client = new RestClient(config.AddressAPI + "v4/player_perspectives");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Client-ID", config.ClientID);
                    request.AddHeader("Authorization", "Bearer " +config.Token);
                    request.AddHeader("Content-Type", "text/plain");
                    request.AddHeader("Cookie", "__cfduid=df1e5c35c64b02b17a3efeb129ff7ecf41610732111");
                    request.AddParameter("text/plain", "fields *;\r\nwhere id = " + perspective + ";", ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);

                    JArray rss2 = JArray.Parse(response.Content);
                    var jsonObjects2 = rss2.OfType<JObject>().ToList();
                    listPlayerPerspective.Add((string)jsonObjects2[0]["name"]);
                }

                return listPlayerPerspective;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return null;
            }
        }
    }
}

using DataParser.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

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
        public string GetGameInfo(Config config, string game)
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
                request.AddParameter("text/plain", "fields *;\r\nlimit 100;\r\nwhere name = \"" +game +"\";", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return response.Content;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return null;
            }
        }
    }
}

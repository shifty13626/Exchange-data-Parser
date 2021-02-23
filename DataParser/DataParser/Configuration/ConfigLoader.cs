using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataParser.Configuration
{
    /// <summary>
    /// Manager to load configuration
    /// </summary>
    public class ConfigLoader
    {
        #region - Nodes
        // All nodes XML file
        private static string Node_Config = "config";
        private static string Node_AddresseAPI = "addressAPI";
        private static string Node_TokenAPI = "token";
        private static string Node_ClientIdAPI = "clientID";
        private static string Node_ClientSecretAPI = "clientSecret";
        #endregion


        /// <summary>
        /// Load data from XML File
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Config LoadConfig(string url)
        {
            var doc = XDocument.Load(url);
            var root = doc.Element(Node_Config);
            if (root == null)
            {
                Console.WriteLine("Can't find root element of config");
                return null;
            }

            return new Config()
            {
                AddressAPI = root.Element(Node_AddresseAPI).Value,
                ClientID = root.Element(Node_ClientIdAPI).Value,
                ClientSecret = root.Element(Node_ClientSecretAPI).Value
            };
        }
    }
}

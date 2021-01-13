using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataParser.Configuration
{
    public class Config
    {
        /// <summary>
        /// Address of API
        /// </summary>
        public string AddressAPI { get; set; }
        /// <summary>
        /// Token to connect to API
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// ClientID identifier to connect to API
        /// </summary>
        public string ClientID { get; set; }
    }
}

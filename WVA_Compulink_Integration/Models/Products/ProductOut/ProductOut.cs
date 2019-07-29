using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CDI.Models.Products.ProductOut
{
    class ProductOut
    {
        [JsonProperty("api_key")]
        public string Api_key { get; set; }

        [JsonProperty("account_id")]
        public string AccountID { get; set; }
    }
}

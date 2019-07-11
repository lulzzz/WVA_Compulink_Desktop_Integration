using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Compulink_Desktop_Integration.Models.Products
{
    class Product
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("product_key")]
        public string ProductKey { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("vendor")]
        public string Vendor { get; set; }
    }
}

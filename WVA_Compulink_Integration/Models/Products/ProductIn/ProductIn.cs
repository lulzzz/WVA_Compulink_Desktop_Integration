using WVA_Connect_CDI.Models.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CDI.Models.Products.ProductIn
{
    class ProductIn : Response
    {
        [JsonProperty("data")]
        public List<Product> Products { get; set; }
    }
}

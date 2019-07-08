using WVA_Compulink_Desktop_Integration.Models.Products.ProductOut;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Compulink_Desktop_Integration.Models.Requests
{
    class OutRequest
    {
        [JsonProperty("request")]
        public ProductOut Request { get; set; }
    }
}

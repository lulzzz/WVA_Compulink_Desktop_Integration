using WVA_Connect_CDI.Models.Products.ProductOut;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CDI.Models.Requests
{
    class OutRequest
    {
        [JsonProperty("request")]
        public ProductOut Request { get; set; }
    }
}

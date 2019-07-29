using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CDI.Models.Responses
{
    public class OrderResponse : Response
    {
        [JsonProperty("data")]
        public ResponseData Data { get; set; }
    }
}

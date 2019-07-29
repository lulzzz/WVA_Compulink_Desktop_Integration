using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CDI.Models.OrderStatus.ToApi
{
    class RequestWrapper
    {
        [JsonProperty("request")]
        public StatusRequest Request { get; set; }
    }
}

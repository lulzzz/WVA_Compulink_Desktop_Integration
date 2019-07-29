using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CDI.Models.Orders.Out
{
    public class OutOrderWrapper
    {
        [JsonProperty("request")]
        public OutOrder OutOrder { get; set; }
    }
}

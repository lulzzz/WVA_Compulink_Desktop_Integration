using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Compulink_Desktop_Integration.Models.Orders.Out
{
    public class OutOrder
    {
        [JsonProperty("api_key")]
        public string ApiKey { get; set; }

        [JsonProperty("order")]
        public Order PatientOrder { get; set; }
    }
}

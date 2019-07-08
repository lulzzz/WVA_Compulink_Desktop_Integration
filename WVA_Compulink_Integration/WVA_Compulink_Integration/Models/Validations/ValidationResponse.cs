using WVA_Compulink_Desktop_Integration.Models.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Compulink_Desktop_Integration.Models.Validations
{
    class ValidationResponse : Response
    {
        [JsonProperty("data")]
        public ValidationProducts Data { get; set; }
    }
}

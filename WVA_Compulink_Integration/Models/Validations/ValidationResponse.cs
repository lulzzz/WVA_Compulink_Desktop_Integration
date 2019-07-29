using WVA_Connect_CDI.Models.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CDI.Models.Validations
{
    class ValidationResponse : Response
    {
        [JsonProperty("data")]
        public ValidationProducts Data { get; set; }
    }
}

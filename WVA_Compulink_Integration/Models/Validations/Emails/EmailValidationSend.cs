using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CDI.Models.Validations.Emails
{
    class EmailValidationSend
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("api_key")]
        public string ApiKey { get; set; }
    }
}

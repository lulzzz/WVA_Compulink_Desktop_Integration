using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CDI.Models.Validations.Emails
{
    class EmailValidationCode : EmailValidationSend
    {
        [JsonProperty("email_code")]
        public string EmailCode { get; set; }
    }
}

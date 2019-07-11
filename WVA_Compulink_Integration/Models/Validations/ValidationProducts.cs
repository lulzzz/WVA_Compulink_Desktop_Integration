using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Compulink_Desktop_Integration.Models.Validations
{
    class ValidationProducts
    {
        [JsonProperty("products")]
        public List<ValidationDetail> Products { get; set; }
    }
}

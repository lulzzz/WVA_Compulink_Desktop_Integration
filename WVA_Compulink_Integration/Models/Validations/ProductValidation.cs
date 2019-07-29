using WVA_Connect_CDI.Models.Orders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CDI.Models.Validations
{
    public class ProductValidation
    {
        [JsonProperty("api_key")]
        public string Key { get; set; }

        [JsonProperty("products")]
        public List<ItemDetail> ProductsToValidate { get; set; }
    }
}

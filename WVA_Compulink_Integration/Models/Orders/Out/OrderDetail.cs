using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CDI.Models.Orders.Out
{
    public class OrderDetail
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("product_reviewed")]
        public bool ProductReviewed { get; set; }

        [JsonProperty("wva_sku")]
        public string SKU { get; set; }

        [JsonProperty("product_key")]
        public string ProductKey { get; set; }

        [JsonProperty("upc")]
        public string UPC { get; set; }

        [JsonProperty("bc")]
        public string BaseCurve { get; set; }

        [JsonProperty("dia")]
        public string Diameter { get; set; }

        [JsonProperty("sph")]
        public string Sphere { get; set; }

        [JsonProperty("cyl")]
        public string Cylinder { get; set; }

        [JsonProperty("ax")]
        public string Axis { get; set; }

        [JsonProperty("add")]
        public string Add { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("multifocal")]
        public string Multifocal { get; set; }

        [JsonProperty("lens_rx")]
        public string LensRx { get; set; }
    }
}

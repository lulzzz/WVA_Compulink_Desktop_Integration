using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Compulink_Desktop_Integration.Models.Orders.Out
{
    public class Order
    {
        [JsonProperty("customer_id")]
        public string CustomerID { get; set; }

        [JsonProperty("order_name")]
        public string OrderName { get; set; }

        [JsonProperty("created_date")]
        public string CreatedDate { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("wva_store_id")]
        public string WvaStoreID { get; set; }

        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("dob")]
        public string DoB { get; set; }

        [JsonProperty("name_1")]
        public string Name_1 { get; set; }

        [JsonProperty("name_2")]
        public string Name_2 { get; set; }

        [JsonProperty("street_address_1")]
        public string StreetAddr_1 { get; set; }

        [JsonProperty("street_address_2")]
        public string StreetAddr_2 { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("zip")]
        public string Zip { get; set; }

        [JsonProperty("ordered_by")]
        public string OrderedBy { get; set; }

        [JsonProperty("po_number")]
        public string PoNumber { get; set; }

        [JsonProperty("shipping_method")]
        public string ShippingMethod { get; set; }

        [JsonProperty("ship_to_patient")]
        public string ShipToPatient { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("processed_flag")]
        public string ProcessedFlag { get; set; }

        [JsonProperty("deleted_flag")]
        public string DeletedFlag { get; set; }

        [JsonProperty("items")]
        public List<Item> Items { get; set; }
    }
}

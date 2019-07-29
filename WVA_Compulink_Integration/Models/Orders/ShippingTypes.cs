using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CDI.Models.Orders
{
    class ShippingTypes
    {
        public static List<string> ListShippingTypes = new List<string>()
        {
            "Free Standard",
            "Standard",
            "UPS Ground",
            "UPS 2nd Day Air",
            "UPS Next Day Air",
        };
    }
}

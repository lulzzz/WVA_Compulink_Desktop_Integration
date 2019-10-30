using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CDI.Utility.UI_Tools
{
    public class ShippingTools
    {
        public static string GetShippingString(string shipID)
        {
            switch (shipID)
            {
                case "1":
                    return "Standard";
                case "D":
                    return "UPS Ground";
                case "J":
                    return "UPS 2nd Day Air";
                case "P":
                    return "UPS Next Day Air";
                default:
                    return shipID;
            }
        }

        public static string GetShippingTypeID(string shipType)
        {
            switch (shipType)
            {
                case "Standard":
                    return "1";
                case "UPS Ground":
                    return "D";
                case "UPS 2nd Day Air":
                    return "J";
                case "UPS Next Day Air":
                    return "P";
                default:
                    return shipType;
            }
        }
    }
}

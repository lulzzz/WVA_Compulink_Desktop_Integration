using WVA_Connect_CDI.Models.Orders.Out;
using WVA_Connect_CDI.Models.Prescriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CDI.Memory
{
    class Orders
    {
        public static List<Prescription> CompulinkOrders = new List<Prescription>() { };
        public static List<Order> WVAOrders = new List<Order>() { };
    }
}

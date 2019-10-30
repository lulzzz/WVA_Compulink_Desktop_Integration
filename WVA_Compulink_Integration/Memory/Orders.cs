using WVA_Connect_CDI.Models.Orders.Out;
using WVA_Connect_CDI.Models.Prescriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CDI.Memory
{
    public class Orders
    {
        // Purpose of these objects is to hang onto data while switching views and view models
        public static List<Prescription> CompulinkOrders = new List<Prescription>() { };
        public static List<Order> WVAOrders = new List<Order>() { };

        // Ideally, I would like to get rid of these ugly 'public static' members and have them 
        // be passed between view models, but I encountered bugs that made the view transitions
        // unusable while doing so, and didn't have the time to refactor all of the views involved.

    }
}

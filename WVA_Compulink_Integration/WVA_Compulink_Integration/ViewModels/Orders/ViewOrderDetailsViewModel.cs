using WVA_Compulink_Desktop_Integration.Models.Orders.Out;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Compulink_Desktop_Integration.ViewModels.Orders
{
    class ViewOrderDetailsViewModel
    {
        public static Order SelectedOrder;

        public ViewOrderDetailsViewModel(Order order)
        {
            SelectedOrder = order;
        }
    }
}

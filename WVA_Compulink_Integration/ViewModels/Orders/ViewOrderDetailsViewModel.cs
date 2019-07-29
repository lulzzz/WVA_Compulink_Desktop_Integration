using WVA_Connect_CDI.Models.Orders.Out;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CDI.ViewModels.Orders
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

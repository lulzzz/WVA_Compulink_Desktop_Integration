using WVA_Connect_CDI.Models.Prescriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CDI.ViewModels
{
    public class OrdersViewModel
    {
        public static string SelectedView { get; set; }
        public static List<Prescription> ListPrescriptions { get; set; }

        public OrdersViewModel()
        {

        }

        public OrdersViewModel(string selectedView, List<Prescription> listPrescriptions)
        {
            SelectedView = selectedView;
            ListPrescriptions = listPrescriptions;
        }
    }
}

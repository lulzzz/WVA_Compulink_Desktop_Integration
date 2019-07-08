using WVA_Compulink_Desktop_Integration.Models.Prescriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Compulink_Desktop_Integration.ViewModels.Orders
{
    class WVAOrdersViewModel
    {
        public static List<Prescription> ListPrescriptions = new List<Prescription>();

        public WVAOrdersViewModel()
        {

        }

        public WVAOrdersViewModel(List<Prescription> prescriptions)
        {
            if (prescriptions == null)
                return;

            ListPrescriptions.AddRange(prescriptions);
        }
    }
}

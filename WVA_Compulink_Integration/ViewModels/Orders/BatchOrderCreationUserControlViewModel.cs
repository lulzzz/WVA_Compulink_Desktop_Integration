using WVA_Compulink_Desktop_Integration.MatchFinder;
using WVA_Compulink_Desktop_Integration.MatchFinder.ProductPredictions;
using WVA_Compulink_Desktop_Integration.Memory;
using WVA_Compulink_Desktop_Integration.Models.Prescriptions;
using WVA_Compulink_Desktop_Integration.Models.Products;
using WVA_Compulink_Desktop_Integration.Models.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Compulink_Desktop_Integration.ViewModels.Orders
{
    public class BatchOrderCreationUserControlViewModel
    {
        public string OrderName { get; set; }
        public List<Prescription> Prescriptions = new List<Prescription>();
        public List<List<MatchProduct>> Matches = new List<List<MatchProduct>>();
    }
}

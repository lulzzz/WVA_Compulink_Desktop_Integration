using WVA_Connect_CDI.MatchFinder;
using WVA_Connect_CDI.MatchFinder.ProductPredictions;
using WVA_Connect_CDI.Memory;
using WVA_Connect_CDI.Models.Prescriptions;
using WVA_Connect_CDI.Models.Products;
using WVA_Connect_CDI.Models.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CDI.ViewModels.Orders
{
    public class BatchOrderCreationUserControlViewModel
    {
        public string OrderName { get; set; }
        public List<Prescription> Prescriptions = new List<Prescription>();
        public List<List<MatchProduct>> Matches = new List<List<MatchProduct>>();
    }
}

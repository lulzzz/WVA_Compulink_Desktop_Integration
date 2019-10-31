using WVA_Connect_CDI.Models.Prescriptions;
using System;
using System.Collections.Generic;
using WVA_Connect_CDI.ProductMatcher.Models;

namespace WVA_Connect_CDI.ViewModels.Orders
{
    public class BatchOrderCreationUserControlViewModel
    {
        public string OrderName { get; set; }
        public List<Prescription> Prescriptions = new List<Prescription>();
        public List<List<MatchedProduct>> Matches = new List<List<MatchedProduct>>();
    }
}

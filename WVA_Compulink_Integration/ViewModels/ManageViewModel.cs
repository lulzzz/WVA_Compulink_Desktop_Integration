using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CDI.MatchFinder.ProductPredictions;
using WVA_Connect_CDI.Models.Products;
using WVA_Connect_CDI.ProductMatcher.ProductPredictions.Models;

namespace WVA_Connect_CDI.ViewModels
{
    public class ManageViewModel
    {
        public List<LearnedProduct> LearnedProducts = new List<LearnedProduct>();

        public ManageViewModel()
        {
            SetLearnedProducts();
        }

        private void SetLearnedProducts()
        {
            LearnedProducts = GetLearnedProducts();
        }

        public List<LearnedProduct> GetLearnedProducts()
        {
            return Database.GetLearnedProducts();
        }


    }
}

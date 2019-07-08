using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Compulink_Desktop_Integration.MatchFinder.ProductPredictions;
using WVA_Compulink_Desktop_Integration.ProductMatcher.ProductPredictions;
using WVA_Compulink_Desktop_Integration.Utility.Files;

namespace WVA_Compulink_Desktop_Integration.Models.Products
{
    class ProductCrossRefDict
    {
        public static void CheckForUpdates()
        {
            if (File.Exists(Paths.ProductCrossRefFile))
            {
                RunDataInjection(GetProductCrossRef());
                DeleteCrossRefFile();
            }
        }

        private static void RunDataInjection(Dictionary<string, string> products)
        {
            foreach (KeyValuePair<string, string> productPair in products)
            {
                Database.CreateCompulinkProduct(productPair.Key, productPair.Value);
            }
        }

        private static void DeleteCrossRefFile()
        {
            if (File.Exists(Paths.ProductCrossRefFile))
                File.Delete(Paths.ProductCrossRefFile);
        }

        private static Dictionary<string, string> GetProductCrossRef()
        {
            var strCrossRef = File.ReadAllText(Paths.ProductCrossRefFile);
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(strCrossRef);
        }
    }
}

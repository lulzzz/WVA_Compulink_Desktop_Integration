using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CDI.Errors;
using WVA_Connect_CDI.MatchFinder.ProductPredictions;
using WVA_Connect_CDI.ProductMatcher.ProductPredictions;
using WVA_Connect_CDI.Utility.Files;

namespace WVA_Connect_CDI.Models.Products
{
    class ProductCrossRefDict
    {
        public static void CheckForUpdates()
        {
            try
            {
                string[] files = Directory.GetDirectories(Paths.AppBasePath);

                foreach (string file in files)
                {
                    string crossRefFile = file + @"\ProductCrossRef.json";

                    if (file.Contains("app-") && File.Exists(crossRefFile))
                    {
                        bool successful = RunDataInjection(GetProductCrossRef(crossRefFile));

                        if (successful)
                            DeleteCrossRefFile(crossRefFile);
                    }
                }
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
            }
        }

        private static bool RunDataInjection(Dictionary<string, string> products)
        {
            bool successful = true;
            foreach (KeyValuePair<string, string> productPair in products)
            {
                successful = Database.CreateCompulinkProduct(productPair.Key, productPair.Value);
            }

            return successful;
        }

        private static void DeleteCrossRefFile(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }

        private static Dictionary<string, string> GetProductCrossRef(string path)
        {
            var strCrossRef = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(strCrossRef);
        }
    }
}

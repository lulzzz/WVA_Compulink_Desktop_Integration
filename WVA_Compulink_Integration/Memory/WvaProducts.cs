using WVA_Connect_CDI.WebTools;
using WVA_Connect_CDI.Models.Products;
using WVA_Connect_CDI.Models.Products.ProductIn;
using WVA_Connect_CDI.Models.Products.ProductOut;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CDI.Errors;
using System.Windows;
using System.IO;

namespace WVA_Connect_CDI.Memory
{
    class WvaProducts
    {
        public static List<Product> ListProducts;

        public static void LoadProductList(RequestOut request, string endpoint)
        {
            string data = API.Post(endpoint, request);

            ProductIn productIn = JsonConvert.DeserializeObject<ProductIn>(data);

            if (productIn == null || productIn.Products == null || productIn.Products.Count < 1)
                throw new Exception("List WVA products returned null or empty.");
            else if (productIn.Status == "SUCCESS")
                ListProducts = productIn.Products;
            else
            {
                Error.Log($"Respons: FAIL when getting products. Error Message:{productIn.Message}");
                throw new Exception($"Error getting WVA products. Status: {productIn.Status} -- Message: {productIn.Message}");
            }
        }
    }
}

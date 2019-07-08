using WVA_Compulink_Desktop_Integration.WebTools;
using WVA_Compulink_Desktop_Integration.Models.Products;
using WVA_Compulink_Desktop_Integration.Models.Products.ProductIn;
using WVA_Compulink_Desktop_Integration.Models.Products.ProductOut;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Compulink_Desktop_Integration.Memory
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
                throw new Exception($"Error getting WVA products. Status: {productIn.Status} -- Message: {productIn.Message}");
        }
    }
}

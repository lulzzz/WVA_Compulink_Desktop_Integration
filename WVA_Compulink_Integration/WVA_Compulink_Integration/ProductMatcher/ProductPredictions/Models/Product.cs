using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Compulink_Desktop_Integration.ProductMatcher.ProductPredictions.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string CompulinkProduct { get; set; }
        public string WvaProduct { get; set; }
        public int NumPicks { get; set; }
    }
}

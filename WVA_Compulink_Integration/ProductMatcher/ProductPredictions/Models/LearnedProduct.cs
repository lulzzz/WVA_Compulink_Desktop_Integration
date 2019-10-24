using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CDI.ProductMatcher.ProductPredictions.Models
{
    public class LearnedProduct
    {
        public int Id { get; set; }
        public string CompulinkProduct { get; set; }
        public string WvaProduct { get; set; }
        public int NumPicks { get; set; }
        public bool ChangeEnabled { get; set; }
    }
}

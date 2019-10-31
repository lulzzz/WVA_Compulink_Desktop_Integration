using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CDI.ProductMatcher.Models
{
    // Stores a WVA product's data and is used to determine how close of a match it 
    // is to a compulink product. This object contains quite a few variables so it 
    // is only hung onto in memory while the algorithm is trying to determine good matches
    public class PreMatchedProduct
    {
        public string CompulinkProduct { get; set; }
        public string WvaProduct { get; set; }
        public string SKU { get; set; }
        public string Quantity { get; set; }
        public string ProductCode { get; set; }

        public double CharacterSequenceMatchScore { get; set; } = 0;
        public double WordMatchScore { get; set; } = 0;
        public double SkuTypeMatchScore { get; set; } = 0;
        public double QuantityMatchScore { get; set; } = 0;

        public double MatchScore
        {
            get { return CharacterSequenceMatchScore + WordMatchScore + SkuTypeMatchScore + QuantityMatchScore; }
            set { MatchScore = value; }
        }
    }
}

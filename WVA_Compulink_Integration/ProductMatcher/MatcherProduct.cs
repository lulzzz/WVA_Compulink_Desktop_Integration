using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CDI.MatchFinder
{
    public class MatcherProduct
    {
        public string MatchProduct { get; set; }
        public string WvaProduct { get; set; }
        public string SKU { get; set; }
        public string Quantity { get; set; }
        public string WvaProdCode { get; set; }

        //public double CharacterMatchScore { get; set; } = 0;
        public double CharacterSequenceMatchScore { get; set; } = 0;
        public double WordMatchScore { get; set; } = 0;
        public double SkuTypeMatchScore { get; set; } = 0;
        public double QuantityMatchScore { get; set; } = 0;


        public double TotalScore
        {
            get { return CharacterSequenceMatchScore + WordMatchScore + SkuTypeMatchScore + QuantityMatchScore; }
            set { TotalScore = value; }
        }

    }
}

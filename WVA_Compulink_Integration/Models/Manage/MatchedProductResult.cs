using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CDI.ProductMatcher.Models;

namespace WVA_Connect_CDI.Models.Manage
{
    public class MatchedProductResult
    {
        public string CompulinkProduct { get; set; }
        public string WvaProduct { get; set; }

        private string rowColor;
        public string RowColor
        {
            get
            {
                if (MatchScore >= 80)
                    return "Green";
                else if (MatchScore >= 40)
                    return "Yellow";
                else
                    return "Red";
            }
            set
            { 
                rowColor = value; 
            }
        }


        public double MatchScore { get; set; } = 0;

        public bool IsSelected { get; set; }

        public List<string> SuggestedWvaProducts { get; set; }
        public List<MatchedProduct> MatchedProducts { get; set; }
    }
}

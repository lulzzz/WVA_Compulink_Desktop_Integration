using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CDI.ProductMatcher.Models
{
    // This class is used to determine how close of a match a WVA product is to a compulink product.
    // It is similar to the 'PreMatchProduct', but it only contains data that is necessary to present
    // to the UI. Since we are often storing a lot of possible matches for each product in an order,
    // it is more memory-efficient to store collections of this smaller class.
    public class MatchedProduct
    {
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public double MatchScore { get; set; }

        public MatchedProduct(string productName, double matchScore)
        {
            // Check for nulls 
            if (productName == null || productName?.Trim() == "")
                throw new Exception("'name' cannot be null or blank.'");

            if (matchScore < 0)
                throw new Exception("'matchScore' cannot be negative.'");

            ProductName = productName;
            MatchScore = matchScore;
        }
    }
}

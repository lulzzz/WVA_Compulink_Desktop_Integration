using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CDI.MatchFinder
{
    public class MatchProduct
    {
        public MatchProduct(string name, double matchScore)
        {
            // Check for nulls 
            if (name == null || name?.Trim() == "")
                throw new Exception("'name' cannot be null or blank.'");

            if (matchScore < 0)
                throw new Exception("'matchScore cannot be negative.'");

            Name = name;
            MatchScore = matchScore;
        }

        public string Name { get; set; }
        public double MatchScore { get; set; }
        public string ProductKey { get; set; }
    }
}

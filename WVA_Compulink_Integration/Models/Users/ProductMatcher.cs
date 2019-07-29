using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CDI.Models.Users
{
    public class ProductMatcher
    {
        public int CharSequenceMaxScore { get; set; } = 40;

        public int SameWordMaxScore { get; set; } = 50;

        public int SkuTypeMaxScore { get; set; } = 5;

        public int QuantityMaxScore { get; set; } = 5;
    }
}

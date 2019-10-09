using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CDI.Models.Users
{
    public class UserSettings
    {
        public bool DeleteBlankCompulinkOrders { get; set; } = false;
        public bool AutoFillLearnedProducts { get; set; } = true;
        public bool AutoUpdate { get; set; } = true;
        public ProductMatcher ProductMatcher { get; set; } = new ProductMatcher() { CharSequenceMaxScore = 40, SameWordMaxScore = 50, SkuTypeMaxScore = 5, QuantityMaxScore = 5 };
    }
}

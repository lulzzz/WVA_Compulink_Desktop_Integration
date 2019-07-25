using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Compulink_Desktop_Integration.Models.Users
{
    public class UserSettings
    {
        public bool DeleteBlankCompulinkOrders { get; set; } = false;

        public ProductMatcher ProductMatcher { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Compulink_Desktop_Integration.Models.Products
{
    class ProductItem
    {
        public int ID { get; set; }
        public string CustomerID { get; set; }
        public string ProductKey { get; set; }
        public string BaseCurve { get; set; }
        public string Diameter { get; set; }
        public string Sphere { get; set; }
        public string Cylinder { get; set; }
        public string Axis { get; set; }
        public string Add { get; set; }
        public string Multifocal { get; set; }
        public string Color { get; set; }
    }
}

using WVA_Connect_CDI.Memory;
using WVA_Connect_CDI.Models.ProductParameters.Derived;
using WVA_Connect_CDI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CDI.Models.Prescriptions
{
    public class Prescription
    {
        // These properties are not visible to the client in the data grid
        public CustomerID _CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProductCode { get; set; }
        public string SKU { get; set; }
        public string UPC { get; set; }
        public string Price { get; set; }
        public string ID { get; set; }
        public bool IsTrial { get; set; }
        public string LensRx { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }

        // Used for creating batch orders
        public string AccountNumber { get; set; }

        public bool CanBeValidated { get; set; } = true;

        // These properties are visible to the client in the data grid
        private string patient { get; set; }
        public string Patient
        {
            get { return $"{LastName}, {FirstName}"; }
            set { patient = value; }
        }
        public string Date { get; set; }
        public string Eye { get; set; }
        public string Product { get; set; }
        public string Quantity { get; set; }
        public string BaseCurve { get; set; }
        public string Diameter { get; set; }
        public string Sphere { get; set; }
        public string Cylinder { get; set; }
        public string Axis { get; set; }
        public string Add { get; set; }
        public string Color { get; set; }
        public string Multifocal { get; set; }

        // This is the property that controls the value of the row's check box 
        public bool IsChecked { get; set; } = false;

        // This is the property that determines if an order is a STO or STP
        public bool IsShipToPat { get; set; } = false;

        // Change this string to the path of the green check mark to show that the item has been validated
        public string ProductImagePath { get; set; }

        // Assign these "White", "Green", or "Red" to change the corresponding cell's background color
        public string BaseCurveCellColor { get; set; }
        public string DiameterCellColor { get; set; }
        public string SphereCellColor { get; set; }
        public string CylinderCellColor { get; set; }
        public string AxisCellColor { get; set; }
        public string AddCellColor { get; set; }
        public string ColorCellColor { get; set; }
        public string MultifocalCellColor { get; set; }

        // These are suggested products for a given parameter
        public List<string> BaseCurveValidItems { get; set; }
        public List<string> DiameterValidItems { get; set; }
        public List<string> SphereValidItems { get; set; }
        public List<string> CylinderValidItems { get; set; }
        public List<string> AxisValidItems { get; set; }
        public List<string> AddValidItems { get; set; }
        public List<string> ColorValidItems { get; set; }
        public List<string> MultifocalValidItems { get; set; }
    }
}

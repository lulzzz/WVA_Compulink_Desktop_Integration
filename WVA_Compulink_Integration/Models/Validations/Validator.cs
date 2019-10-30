using WVA_Connect_CDI.Models.ProductParameters.Derived;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CDI.Models.Validations
{
    public class Validator
    {
        public static bool CheckIfValid(SKU sku)
        {
            return sku == null || sku.IsValid ? true : false;
        }

        public static bool CheckIfValid(UPC upc)
        {
            return upc == null || upc.IsValid ? true : false;
        }

        public static bool CheckIfValid(ProductKey productKey)
        {
            return productKey == null || productKey.IsValid ? true : false;
        }

        public static bool CheckIfValid(ID id)
        {
            return id == null || id.IsValid ? true : false;
        }

        public static bool CheckIfValid(CustomerID customerID)
        {
            return customerID == null || customerID.IsValid ? true : false;
        }

        public static bool CheckIfValid(BaseCurve baseCurve)
        {
            return baseCurve == null || baseCurve.IsValid ? true : false;
        }

        public static bool CheckIfValid(Diameter diameter)
        {
            return diameter == null || diameter.IsValid ? true : false;
        }

        public static bool CheckIfValid(Sphere sphere)
        {
            return sphere == null || sphere.IsValid ? true : false;
        }

        public static bool CheckIfValid(Cylinder cylinder)
        {
            return cylinder == null || cylinder.IsValid ? true : false;
        }

        public static bool CheckIfValid(Axis axis)
        {
            return axis == null || axis.IsValid ? true : false;
        }

        public static bool CheckIfValid(Add add)
        {
            return add == null || add.IsValid ? true : false;
        }

        public static bool CheckIfValid(Color color)
        {
            return color == null || color.IsValid ? true : false;
        }

        public static bool CheckIfValid(Multifocal multifocal)
        {
            return multifocal == null || multifocal.IsValid ? true : false;
        }
    }
}

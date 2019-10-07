using WVA_Connect_CDI.Models.Orders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CDI.Errors;

namespace WVA_Connect_CDI.Models.Validations
{
    public class ValidationDetail : ItemDetail 
    {
        internal string _PatientName;

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("can_be_validated")]
        public bool CanBeValidated { get; set; } = true;

        // Standard instantiation. Creates a blank ValidationDetail object
        public ValidationDetail()
        {

        }

        // Instantiation with an ItemDetail object. Creates a ValidationDetail object from an ItemDetail object
        public ValidationDetail(ItemDetail checkDetail)
        {
            SetItemDetail(checkDetail);
        }

        // Instantiation with a ValidationDetail object
        public ValidationDetail(ValidationDetail checkDetail)
        {
            SetValidationDetail(checkDetail);
        }

        // Sets the ValidationDetail properties equal to the ItemDetail properties 
        private void SetItemDetail(ItemDetail checkDetail)
        {
            PatientName = checkDetail?.PatientName;
            Eye = checkDetail?.Eye;
            Quantity = checkDetail?.Quantity;
            Description = checkDetail?.Description;
            Vendor = checkDetail?.Vendor;
            Price = checkDetail?.Price;
            _ID = checkDetail?._ID;
            _ProductKey = checkDetail?._ProductKey;
            _CustomerID = checkDetail?._CustomerID;
            _BaseCurve = checkDetail?._BaseCurve;
            _Diameter = checkDetail?._Diameter;
            _Sphere = checkDetail?._Sphere;
            _Cylinder = checkDetail?._Cylinder;
            _Axis = checkDetail?._Axis;
            _Add = checkDetail?._Add;
            _Color = checkDetail?._Color;
            _Multifocal = checkDetail?._Multifocal;
            _UPC = checkDetail?._UPC;
            _SKU = checkDetail?._SKU;
        }

        // Sets the ValidationDetail properties equal to the ValidationDetail properties
        private void SetValidationDetail(ValidationDetail checkDetail)
        {
            PatientName = checkDetail?.PatientName;
            Eye = checkDetail?.Eye;
            Quantity = checkDetail?.Quantity;
            Description = checkDetail?.Description;
            Vendor = checkDetail?.Vendor;
            Price = checkDetail?.Price;
            _ID = checkDetail?._ID;
            _ProductKey = checkDetail?._ProductKey;
            _CustomerID = checkDetail?._CustomerID;
            _BaseCurve = checkDetail?._BaseCurve;
            _Diameter = checkDetail?._Diameter;
            _Sphere = checkDetail?._Sphere;
            _Cylinder = checkDetail?._Cylinder;
            _Axis = checkDetail?._Axis;
            _Add = checkDetail?._Add;
            _Color = checkDetail?._Color;
            _Multifocal = checkDetail?._Multifocal;
            _UPC = checkDetail?._UPC;
            _SKU = checkDetail?._SKU;
            Status = checkDetail?.Status;
        }
    }
}

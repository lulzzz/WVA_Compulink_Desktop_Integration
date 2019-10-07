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
            try
            {
                // _PatientName
                PatientName = checkDetail?.PatientName;

                // _Eye
                Eye = checkDetail?.Eye;

                // _Quantity
                Quantity = checkDetail?.Quantity;

                // _Description
                Description = checkDetail?.Description;

                // _Vendor
                Vendor = checkDetail?.Vendor;

                // _Price
                Price = checkDetail?.Price;

                // _ID
                _ID = checkDetail?._ID;

                // _ProductKey
                _ProductKey = checkDetail?._ProductKey;

                // _CustomerID
                _CustomerID = checkDetail?._CustomerID;

                // _BaseCurve
                _BaseCurve = checkDetail?._BaseCurve;

                // _Diameter
                _Diameter = checkDetail?._Diameter;

                // _Sphere
                _Sphere = checkDetail?._Sphere;

                // _Cylinder
                _Cylinder = checkDetail?._Cylinder;

                // _Axis
                _Axis = checkDetail?._Axis;

                // _Adddd != null)
                _Add = checkDetail?._Add;

                // _Color
                _Color = checkDetail?._Color;

                // _Multifocal
                _Multifocal = checkDetail?._Multifocal;

                // _UPC
                _UPC = checkDetail?._UPC;

                // _SKU
                _SKU = checkDetail?._SKU;
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
            };
        }

        // Sets the ValidationDetail properties equal to the ValidationDetail properties
        private void SetValidationDetail(ValidationDetail checkDetail)
        {
            try
            {
                // _PatientName
                PatientName = checkDetail?.PatientName;

                // _Eye
                Eye = checkDetail?.Eye;

                // _Quantity
                Quantity = checkDetail?.Quantity;

                // _Description
                Description = checkDetail?.Description;

                // _Vendor
                Vendor = checkDetail?.Vendor;

                // _Price
                Price = checkDetail?.Price;

                // _ID
                _ID = checkDetail?._ID;

                // _ProductKey
                _ProductKey = checkDetail?._ProductKey;

                // _CustomerID
                _CustomerID = checkDetail?._CustomerID;

                // _BaseCurve
                _BaseCurve = checkDetail?._BaseCurve;

                // _Diameter
                _Diameter = checkDetail?._Diameter;

                // _Sphere
                _Sphere = checkDetail?._Sphere;

                // _Cylinder
                _Cylinder = checkDetail?._Cylinder;

                // _Axis
                _Axis = checkDetail?._Axis;

                // _Add
                _Add = checkDetail?._Add;

                // _Color
                _Color = checkDetail?._Color;

                // _Multifocal
                _Multifocal = checkDetail?._Multifocal;

                // _UPC
                _UPC = checkDetail?._UPC;

                // _SKU
                _SKU = checkDetail?._SKU;

                // Status
                Status = checkDetail?.Status;
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
            };
        }
    }
}

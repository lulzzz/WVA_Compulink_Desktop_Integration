using WVA_Compulink_Desktop_Integration.Models.Orders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Compulink_Desktop_Integration.Errors;

namespace WVA_Compulink_Desktop_Integration.Models.Validations
{
    class ValidationDetail : ItemDetail 
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

        private void SetItemDetail(ItemDetail checkDetail)
        {
            try
            {
                // _PatientName
                if (checkDetail.PatientName != null)
                    PatientName = checkDetail.PatientName;

                // _Eye
                if (checkDetail.Eye != null)
                    Eye = checkDetail.Eye;

                // _Quantity
                if (checkDetail.Quantity != null)
                    Quantity = checkDetail.Quantity;

                // _Description
                if (checkDetail.Description != null)
                    Description = checkDetail.Description;

                // _Vendor
                if (checkDetail.Vendor != null)
                    Vendor = checkDetail.Vendor;

                // _Price
                if (checkDetail.Price != null)
                    Price = checkDetail.Price;

                // _ID
                if (checkDetail._ID != null)
                    _ID = checkDetail._ID;

                // _ProductKey
                if (checkDetail._ProductKey != null)
                    _ProductKey = checkDetail._ProductKey;

                // _CustomerID
                if (checkDetail._CustomerID != null)
                    _CustomerID = checkDetail._CustomerID;

                // _BaseCurve
                if (checkDetail._BaseCurve != null)
                    _BaseCurve = checkDetail._BaseCurve;

                // _Diameter
                if (checkDetail._Diameter != null)
                    _Diameter = checkDetail._Diameter;

                // _Sphere
                if (checkDetail._Sphere != null)
                    _Sphere = checkDetail._Sphere;

                // _Cylinder
                if (checkDetail._Cylinder != null)
                    _Cylinder = checkDetail._Cylinder;

                // _Axis
                if (checkDetail._Axis != null)
                    _Axis = checkDetail._Axis;

                // _Add
                if (checkDetail._Add != null)
                    _Add = checkDetail._Add;

                // _Color
                if (checkDetail._Color != null)
                    _Color = checkDetail._Color;

                // _Multifocal
                if (checkDetail._Multifocal != null)
                    _Multifocal = checkDetail._Multifocal;

                // _UPC
                if (checkDetail._UPC != null)
                    _UPC = checkDetail._UPC;

                // _SKU
                if (checkDetail._SKU != null)
                    _SKU = checkDetail._SKU;
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
            };
        }

        private void SetValidationDetail(ValidationDetail checkDetail)
        {
            try
            {
                // _PatientName
                if (checkDetail.PatientName != null)
                    PatientName = checkDetail.PatientName;

                // _Eye
                if (checkDetail.Eye != null)
                    Eye = checkDetail.Eye;

                // _Quantity
                if (checkDetail.Quantity != null)
                    Quantity = checkDetail.Quantity;

                // _Description
                if (checkDetail.Description != null)
                    Description = checkDetail.Description;

                // _Vendor
                if (checkDetail.Vendor != null)
                    Vendor = checkDetail.Vendor;

                // _Price
                if (checkDetail.Price != null)
                    Price = checkDetail.Price;

                // _ID
                if (checkDetail._ID != null)
                    _ID = checkDetail._ID;

                // _ProductKey
                if (checkDetail._ProductKey != null)
                    _ProductKey = checkDetail._ProductKey;

                // _CustomerID
                if (checkDetail._CustomerID != null)
                    _CustomerID = checkDetail._CustomerID;

                // _BaseCurve
                if (checkDetail._BaseCurve != null)
                    _BaseCurve = checkDetail._BaseCurve;

                // _Diameter
                if (checkDetail._Diameter != null)
                    _Diameter = checkDetail._Diameter;

                // _Sphere
                if (checkDetail._Sphere != null)
                    _Sphere = checkDetail._Sphere;

                // _Cylinder
                if (checkDetail._Cylinder != null)
                    _Cylinder = checkDetail._Cylinder;

                // _Axis
                if (checkDetail._Axis != null)
                    _Axis = checkDetail._Axis;

                // _Add
                if (checkDetail._Add != null)
                    _Add = checkDetail._Add;

                // _Color
                if (checkDetail._Color != null)
                    _Color = checkDetail._Color;

                // _Multifocal
                if (checkDetail._Multifocal != null)
                    _Multifocal = checkDetail._Multifocal;

                // _UPC
                if (checkDetail._UPC != null)
                    _UPC = checkDetail._UPC;

                // _SKU
                if (checkDetail._SKU != null)
                    _SKU = checkDetail._SKU;

                // Status
                if (checkDetail.Status != null)
                    Status = checkDetail.Status;
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
            };
        }
    }
}

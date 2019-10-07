using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CDI.Models.Orders;
using WVA_Connect_CDI.Models.ProductParameters.Derived;
using WVA_Connect_CDI.Models.Validations;

namespace WVA_Connect_CDI.UnitTests.Models
{
    [TestClass]
    public class ValidationDetailTests
    {
        [TestMethod]
        public void ValidationDetail_PatientName_MatchesItemDetailPatientName()
        {
            var itemDetail = new ItemDetail() { PatientName = "Doe, John" };
            var validationDetail = new ValidationDetail(itemDetail);

            string actual = validationDetail.PatientName;
            string expected = "Doe, John";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ValidationDetail_Eye_MatchesItemDetailEye()
        {
            var itemDetail = new ItemDetail() { Eye = "Right" };
            var validationDetail = new ValidationDetail(itemDetail);

            string actual = validationDetail.Eye;
            string expected = "Right";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ValidationDetail_Quantity_MatchesItemDetailQuantity()
        {
            var itemDetail = new ItemDetail() { Quantity = "1" };
            var validationDetail = new ValidationDetail(itemDetail);

            string actual = validationDetail.Quantity;
            string expected = "1";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ValidationDetail_Description_MatchesItemDetailDescription()
        {
            var itemDetail = new ItemDetail() { Description = "proclear sphere" };
            var validationDetail = new ValidationDetail(itemDetail);

            string actual = validationDetail.Description;
            string expected = "proclear sphere";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ValidationDetail_Vendor_MatchesItemDetailVendor()
        {
            var itemDetail = new ItemDetail() { Vendor = "alcon" };
            var validationDetail = new ValidationDetail(itemDetail);

            string actual = validationDetail.Vendor;
            string expected = "alcon";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ValidationDetail_Price_MatchesItemDetailPrice()
        {
            var itemDetail = new ItemDetail() { Price = "15.50" };
            var validationDetail = new ValidationDetail(itemDetail);

            string actual = validationDetail.Price;
            string expected = "15.50";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ValidationDetail_ID_MatchesItemDetailID()
        {
            var itemDetail = new ItemDetail() { _ID = new ID() { Value = "1" } };
            var validationDetail = new ValidationDetail(itemDetail);

            string actual = validationDetail._ID.Value;
            string expected = "1";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ValidationDetail_ProductKey_MatchesItemDetailProductKey()
        {
            var itemDetail = new ItemDetail() { _ProductKey = new ProductKey() { Value = "1234" } };
            var validationDetail = new ValidationDetail(itemDetail);

            string actual = validationDetail._ProductKey.Value;
            string expected = "1234";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ValidationDetail_CustomerID_MatchesItemDetailCustomerID()
        {
            var itemDetail = new ItemDetail() { _CustomerID = new CustomerID() { Value = "99999" } };
            var validationDetail = new ValidationDetail(itemDetail);

            string actual = validationDetail._CustomerID.Value;
            string expected = "99999";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ValidationDetail_BaseCurve_MatchesItemDetailBaseCurve()
        {
            var itemDetail = new ItemDetail() { _BaseCurve = new BaseCurve() { Value = "8.6" } };
            var validationDetail = new ValidationDetail(itemDetail);

            string actual = validationDetail._BaseCurve.Value;
            string expected = "8.6";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ValidationDetail_Diameter_MatchesItemDetailDiameter()
        {
            var itemDetail = new ItemDetail() { _Diameter = new Diameter() { Value = "14.6" } };
            var validationDetail = new ValidationDetail(itemDetail);

            string actual = validationDetail._Diameter.Value;
            string expected = "14.6";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ValidationDetail_Sphere_MatchesItemDetailSphere()
        {
            var itemDetail = new ItemDetail() { _Sphere = new Sphere() { Value = "-300" } };
            var validationDetail = new ValidationDetail(itemDetail);

            string actual = validationDetail._Sphere.Value;
            string expected = "-300";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ValidationDetail_Cylinder_MatchesItemDetailCylinder()
        {
            var itemDetail = new ItemDetail() { _Cylinder = new Cylinder() { Value = "-175" } };
            var validationDetail = new ValidationDetail(itemDetail);

            string actual = validationDetail._Cylinder.Value;
            string expected = "-175";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ValidationDetail_Axis_MatchesItemDetailAxis()
        {
            var itemDetail = new ItemDetail() { _Axis = new Axis() { Value = "180" } };
            var validationDetail = new ValidationDetail(itemDetail);

            string actual = validationDetail._Axis.Value;
            string expected = "180";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ValidationDetail_Add_MatchesItemDetailAdd()
        {
            var itemDetail = new ItemDetail() { _Add = new Add() { Value = "+1.00" } };
            var validationDetail = new ValidationDetail(itemDetail);

            string actual = validationDetail._Add.Value;
            string expected = "+1.00";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ValidationDetail_Color_MatchesItemDetailColor()
        {
            var itemDetail = new ItemDetail() { _Color = new Color() { Value = "Gemstone Green" } };
            var validationDetail = new ValidationDetail(itemDetail);

            string actual = validationDetail._Color.Value;
            string expected = "Gemstone Green";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ValidationDetail_Multifocal_MatchesItemDetailMultifocal()
        {
            var itemDetail = new ItemDetail() { _Multifocal = new Multifocal() { Value = "MED" } };
            var validationDetail = new ValidationDetail(itemDetail);

            string actual = validationDetail._Multifocal.Value;
            string expected = "MED";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ValidationDetail_UPC_MatchesItemDetailUPC()
        {
            var itemDetail = new ItemDetail() { _UPC = new UPC() { Value = "123465789" } };
            var validationDetail = new ValidationDetail(itemDetail);

            string actual = validationDetail._UPC.Value;
            string expected = "123465789";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ValidationDetail_SKU_MatchesItemDetailSKU()
        {
            var itemDetail = new ItemDetail() { _SKU = new SKU() { Value = "86-1460-0300" } };
            var validationDetail = new ValidationDetail(itemDetail);

            string actual = validationDetail._SKU.Value;
            string expected = "86-1460-0300";

            Assert.AreEqual(expected, actual);
        }

    }
}

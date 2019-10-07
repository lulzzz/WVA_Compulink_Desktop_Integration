using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CDI.Models.ProductParameters.Derived;
using WVA_Connect_CDI.Models.Validations;

namespace WVA_Connect_CDI.UnitTests.Models
{
    [TestClass]
    public class ValidatorTests
    {
        // 
        // SKU
        //

        [TestMethod]
        public void Validator_CheckIfValid_SKU_IsValidTrue()
        {
            var sku = new SKU();
            sku.IsValid = true;

            bool actual = Validator.CheckIfValid(sku);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Validator_CheckIfValid_SKU_IsNull()
        {
            var sku = new SKU();

            bool actual = Validator.CheckIfValid(sku);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Validator_CheckIfValid_SKU_IsValidFalse()
        {
            var sku = new SKU();
            sku.IsValid = false;

            bool actual = Validator.CheckIfValid(sku);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        // 
        // UPC
        //

        [TestMethod]
        public void Validator_CheckIfValid_UPC_IsValidTrue()
        {
            var upc = new UPC();
            upc.IsValid = true;

            bool actual = Validator.CheckIfValid(upc);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Validator_CheckIfValid_UPC_IsNull()
        {
            var upc = new UPC();

            bool actual = Validator.CheckIfValid(upc);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Validator_CheckIfValid_UPC_IsValidFalse()
        {
            var upc = new UPC();
            upc.IsValid = false;

            bool actual = Validator.CheckIfValid(upc);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        // 
        // ProductKey
        //

        [TestMethod]
        public void Validator_CheckIfValid_ProductKey_IsValidTrue()
        {
            var productKey = new ProductKey();
            productKey.IsValid = true;

            bool actual = Validator.CheckIfValid(productKey);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Validator_CheckIfValid_ProductKey_IsNull()
        {
            var productKey = new ProductKey();

            bool actual = Validator.CheckIfValid(productKey);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Validator_CheckIfValid_ProductKey_IsValidFalse()
        {
            var productKey = new ProductKey();
            productKey.IsValid = false;

            bool actual = Validator.CheckIfValid(productKey);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        // 
        // ID
        //

        [TestMethod]
        public void Validator_CheckIfValid_ID_IsValidTrue()
        {
            var id = new ID();
            id.IsValid = true;

            bool actual = Validator.CheckIfValid(id);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Validator_CheckIfValid_ID_IsNull()
        {
            var id = new ID();

            bool actual = Validator.CheckIfValid(id);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Validator_CheckIfValid_ID_IsValidFalse()
        {
            var id = new ID();
            id.IsValid = false;

            bool actual = Validator.CheckIfValid(id);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        // 
        // CustomerID
        //

        [TestMethod]
        public void Validator_CheckIfValid_CustomerID_IsValidTrue()
        {
            var customerID = new CustomerID();
            customerID.IsValid = true;

            bool actual = Validator.CheckIfValid(customerID);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        } 

        [TestMethod]
        public void Validator_CheckIfValid_CustomerID_IsNull()
        {
            var customerID = new CustomerID();

            bool actual = Validator.CheckIfValid(customerID);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Validator_CheckIfValid_CustomerID_IsValidFalse()
        {
            var customerID = new CustomerID();
            customerID.IsValid = false;

            bool actual = Validator.CheckIfValid(customerID);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        // 
        // BaseCurve
        //

        [TestMethod]
        public void Validator_CheckIfValid_BaseCurve_IsValidTrue()
        {
            var baseCurve = new BaseCurve();
            baseCurve.IsValid = true;

            bool actual = Validator.CheckIfValid(baseCurve);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Validator_CheckIfValid_BaseCurve_IsNull()
        {
            var baseCurve = new BaseCurve();

            bool actual = Validator.CheckIfValid(baseCurve);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Validator_CheckIfValid_BaseCurve_IsValidFalse()
        {
            var baseCurve = new BaseCurve();
            baseCurve.IsValid = false;

            bool actual = Validator.CheckIfValid(baseCurve);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        // 
        // Diameter
        //

        [TestMethod]
        public void Validator_CheckIfValid_Diameter_IsValidTrue()
        {
            var diameter = new Diameter();
            diameter.IsValid = true;

            bool actual = Validator.CheckIfValid(diameter);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Validator_CheckIfValid_Diameter_IsNull()
        {
            var diameter = new Diameter();

            bool actual = Validator.CheckIfValid(diameter);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Validator_CheckIfValid_Diameter_IsValidFalse()
        {
            var diameter = new Diameter();
            diameter.IsValid = false;

            bool actual = Validator.CheckIfValid(diameter);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        // 
        // Sphere
        //

        [TestMethod]
        public void Validator_CheckIfValid_Sphere_IsValidTrue()
        {
            var sphere = new Sphere();
            sphere.IsValid = true;

            bool actual = Validator.CheckIfValid(sphere);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Validator_CheckIfValid_Sphere_IsNull()
        {
            var sphere = new Sphere();

            bool actual = Validator.CheckIfValid(sphere);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Validator_CheckIfValid_Sphere_IsValidFalse()
        {
            var sphere = new Sphere();
            sphere.IsValid = false;

            bool actual = Validator.CheckIfValid(sphere);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        // 
        // Cylinder
        //

        [TestMethod]
        public void Validator_CheckIfValid_Cylinder_IsValidTrue()
        {
            var cylinder = new Cylinder();
            cylinder.IsValid = true;

            bool actual = Validator.CheckIfValid(cylinder);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Validator_CheckIfValid_Cylinder_IsNull()
        {
            var cylinder = new Cylinder();

            bool actual = Validator.CheckIfValid(cylinder);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Validator_CheckIfValid_Cylinder_IsValidFalse()
        {
            var cylinder = new Cylinder();
            cylinder.IsValid = false;

            bool actual = Validator.CheckIfValid(cylinder);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        // 
        // Axis
        //

        [TestMethod]
        public void Validator_CheckIfValid_Axis_IsValidTrue()
        {
            var axis = new Axis();
            axis.IsValid = true;

            bool actual = Validator.CheckIfValid(axis);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Validator_CheckIfValid_Axis_IsNull()
        {
            var axis = new Axis();

            bool actual = Validator.CheckIfValid(axis);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Validator_CheckIfValid_Axis_IsValidFalse()
        {
            var axis = new Axis();
            axis.IsValid = false;

            bool actual = Validator.CheckIfValid(axis);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        // 
        // Add
        //

        [TestMethod]
        public void Validator_CheckIfValid_Add_IsValidTrue()
        {
            var add = new Add();
            add.IsValid = true;

            bool actual = Validator.CheckIfValid(add);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Validator_CheckIfValid_Add_IsNull()
        {
            var add = new Add();

            bool actual = Validator.CheckIfValid(add);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Validator_CheckIfValid_Add_IsValidFalse()
        {
            var add = new Add();
            add.IsValid = false;

            bool actual = Validator.CheckIfValid(add);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        // 
        // Color
        //

        [TestMethod]
        public void Validator_CheckIfValid_Color_IsValidTrue()
        {
            var color = new Color();
            color.IsValid = true;

            bool actual = Validator.CheckIfValid(color);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Validator_CheckIfValid_Color_IsNull()
        {
            var color = new Color();

            bool actual = Validator.CheckIfValid(color);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Validator_CheckIfValid_Color_IsValidFalse()
        {
            var color = new Color();
            color.IsValid = false;

            bool actual = Validator.CheckIfValid(color);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        // 
        // Multifocal
        //

        [TestMethod]
        public void Validator_CheckIfValid_Multifocal_IsValidTrue()
        {
            var multifocal = new Multifocal();
            multifocal.IsValid = true;

            bool actual = Validator.CheckIfValid(multifocal);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Validator_CheckIfValid_Multifocal_IsNull()
        {
            var multifocal = new Multifocal();

            bool actual = Validator.CheckIfValid(multifocal);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Validator_CheckIfValid_Multifocal_IsValidFalse()
        {
            var multifocal = new Multifocal();
            multifocal.IsValid = false;

            bool actual = Validator.CheckIfValid(multifocal);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CDI.Models.Prescriptions;

namespace WVA_Connect_CDI.UnitTests.Models
{
    [TestClass]
    public class PrescriptionTests
    {
        [TestMethod]
        public void Prescription_Patient_EqualsLastNamePlusFirstName()
        {
            var prescription = new Prescription()
            {
               
                FirstName = "Bruce",
                LastName = "Banner"
            };

            string actual = prescription.Patient;
            string expected = "Banner, Bruce";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Prescription_Patient_EqualsFirstNameOnly()
        {
            var prescription = new Prescription()
            {
                FirstName = "Bruce",
            };

            string actual = prescription.Patient;
            string expected = ", Bruce";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Prescription_Patient_EqualsLastNameOnly()
        {
            var prescription = new Prescription()
            {

                LastName = "Banner"
            };

            string actual = prescription.Patient;
            string expected = "Banner, ";

            Assert.AreEqual(expected, actual);
        }

    }
}

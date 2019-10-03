using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CDI.Models.Patients;

namespace WVA_Connect_CDI.UnitTests.Models
{
    [TestClass]
    public class PatientTests
    {
        [TestMethod]
        public void Patient_FullName_EqualsLastNameFirstName()
        {
            var patient = new Patient()
            {
                FirstName = "John",
                LastName = "Doe"
            };

            string actual = patient.FullName;
            string expected = "Doe John";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Patient_FirstNameNull_LastNameOnlyVisible()
        {
            var patient = new Patient()
            {
                LastName = "Doe"
            };

            string actual = patient.FullName;
            string expected = "Doe ";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Patient_LastNameNull_FirstNameOnlyVisible()
        {
            var patient = new Patient()
            {
                FirstName = "John",
            };

            string actual = patient.FullName;
            string expected = " John";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Patient_FirstNameLastNameNull_BlankString()
        {
            var patient = new Patient();

            string actual = patient.FullName;
            string expected = " ";

            Assert.AreEqual(expected, actual);
        }

    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CDI.Models.ProductParameters.Base;

namespace WVA_Connect_CDI.UnitTests.Models
{
    [TestClass]
    public class ParameterTests
    {
        [TestMethod]
        public void Parameter_ErrorMessageNull_IsValid_EqualsTrue()
        {
            var parameter = new Parameter();
                parameter.ErrorMessage = null;

            bool actual = parameter.IsValid;
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Parameter_ErrorMessageBlankString_IsValid_EqualsTrue()
        {
            var parameter = new Parameter();
                parameter.ErrorMessage = "";

            bool actual = parameter.IsValid;
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Parameter_ErrorMessageHasValue_IsValid_EqualsFalse()
        {
            var parameter = new Parameter();
                parameter.ErrorMessage = "some error";

            bool actual = parameter.IsValid;
            bool expected = false;

            Assert.AreEqual(expected, actual);
        }

    }
}

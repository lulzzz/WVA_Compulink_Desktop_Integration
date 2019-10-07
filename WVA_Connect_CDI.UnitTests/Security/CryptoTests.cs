using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CDI.Security;

namespace WVA_Connect_CDI.UnitTests.Security
{
    [TestClass]
    class CryptoTests
    {
        [TestMethod]
        public void ConvertToHash_GivesNormalString_ReturnsValue()
        {
            string actual = Crypto.ConvertToHash("password123");
            string expected = "2ba33ac8f9f9aad8336b71b4e7f851fe1628709672802ce56c107157a569a5b9";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertToHash_GivesTextBoxMaxLength_ReturnsValue()
        {
            string actual = Crypto.ConvertToHash("aaaaaaaaaaaaaaaaaaaa");
            string expected = "0594ad161e0e5658d3ec5ea839c59e6e9dd0d3fe0621dc65be72ccac304bf1b1";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "'inputString' must be at least 6 characters")]
        public void ConvertToHash_GivesBlankString_ReturnsNull()
        {
            Crypto.ConvertToHash("");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "'inputString' must be at least 6 characters")]
        public void ConvertToHash_GivesNullValue_ReturnsNull()
        {
            Crypto.ConvertToHash(null);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "'inputString' must be at least 6 characters")]
        public void ConvertToHash_GivesShortString_ThrowsException()
        {
            Crypto.ConvertToHash("aaaaa");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "'inputString' must not exceed 63 characters")]
        public void ConvertToHash_GivesHugeString_ThrowsException()
        {
            Crypto.ConvertToHash("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        }
    }
}

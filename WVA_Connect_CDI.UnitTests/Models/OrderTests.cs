using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CDI.Models.Orders.Out;

namespace WVA_Connect_CDI.UnitTests.Models
{
    [TestClass]
    public class OrderTests
    {
        [TestMethod]
        public void Order_GetItemQuantity_EqualsZero()
        {
            var order = new Order();
            order.Items = new List<Item>();

            int actual = order.Quantity;
            int expected = 0;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Order_GetItemQuantity_EqualsThree()
        {
            var order = new Order();

            order.Items = new List<Item>()
            {
                new Item()
                {
                    Quantity = "1"
                },
                new Item()
                {
                    Quantity = "1"
                },
                new Item()
                {
                    Quantity = "1"
                }
            };

            int actual = order.Quantity;
            int expected = 3;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Order_GetShippingString_OneEqualsStandard()
        {
            var order = new Order();
            order.ShippingMethod = "1";

            string actual = order.ShippingMethod;
            string expected = "Standard";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Order_GetShippingString_DEqualsUPSGround()
        {
            var order = new Order();
            order.ShippingMethod = "D";

            string actual = order.ShippingMethod;
            string expected = "UPS Ground";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Order_GetShippingString_JEqualsUPSSecondDayAir()
        {
            var order = new Order();
            order.ShippingMethod = "J";

            string actual = order.ShippingMethod;
            string expected = "UPS 2nd Day Air";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Order_GetShippingString_PEqualsUPSNextDayAir()
        {
            var order = new Order();
            order.ShippingMethod = "P";

            string actual = order.ShippingMethod;
            string expected = "UPS Next Day Air";

            Assert.AreEqual(expected, actual);
        }

    }
}

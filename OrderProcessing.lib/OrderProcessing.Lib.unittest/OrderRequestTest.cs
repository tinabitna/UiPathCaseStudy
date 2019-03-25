using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using OrderProcessing.lib;

namespace OrderProcessing.Lib.UnitTest
{
    [TestClass]
    public class OrderRequestTest
    {
        [TestMethod]
        public void FromEmail()
        {
            string emailBody = File.ReadAllText("OrderRequests\\Order Request-Princesa Isabel Vinhos.txt");
            MailMessage email = new MailMessage
            {
                Subject = "Order Request",
                Body = emailBody
            };
            //Company, PlacedBy, OrderDate, RequiredDate, Shipper, Freight, Shipping
            OrderRequest notice = OrderRequest.FromEmail(email);
            Assert.AreEqual("Princesa Isabel Vinhos (PRINI)", notice.Company);
            Assert.AreEqual("Andrew Fuller (2)", notice.PlacedBy);
            Assert.AreEqual(12 - 04 - 2018, notice.OrderDate);
            Assert.AreEqual(12 - 11 - 2018, notice.RequiredDate);
            Assert.AreEqual("United Package (2)", notice.Shipper);
            Assert.AreEqual(179.72M, notice.Freight);
            Assert.AreEqual("default", notice.Shipping);

          
            //Prdouct, ProductID, Quantity, Discount
            OrderRequestDetails detail = notice.OrderDetails.Last();
            Assert.AreEqual("Teatime Chocolate Biscuits", detail.Product);
            Assert.AreEqual("(19)", detail.ProductID);
            Assert.AreEqual(5M, detail.Quantity);
            Assert.AreEqual(0M, detail.Discount);

        }

        [TestMethod]
        public void TestAllEmail()
        {
            foreach (string fpath in Directory.GetFiles("OrderRequests", "*.txt"))
            {
                MailMessage msg = new MailMessage
                {
                    Subject = "Order Request",
                    Body = File.ReadAllText(fpath)
                };
                Console.WriteLine(fpath);
                OrderRequest notice = OrderRequest.FromEmail(msg);
                Assert.IsFalse(string.IsNullOrEmpty(notice.Company));
                Assert.IsFalse(string.IsNullOrEmpty(notice.PlacedBy));
                Assert.IsFalse(string.IsNullOrEmpty(notice.Shipper));
                Assert.IsTrue(notice.Freight > 0);
                Assert.IsNotNull(notice.OrderDetails);
                Assert.IsTrue(notice.OrderDetails.Count >= 1);

                OrderRequestDetails details = notice.OrderDetails.Last();
                Assert.IsTrue(details.Quantity > 0);
                Assert.IsFalse(string.IsNullOrEmpty(details.Product));
                Assert.IsFalse(string.IsNullOrEmpty(details.ProductID));
                Assert.IsTrue(details.Discount >= 0);

                //Can't be null: Company, PlacedBy, Shipper, Product, ProductID
                //Is true: Shipping = Default
                //Greater than "0" = Freight, Quantity
                //Greater than or Equal to "0" = Discount
                //OrderDetails: Product, ProductID, Quantity, Discount 

            }


        }
    }

}

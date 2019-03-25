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
    public class RestockNoticeTest
    {
        [TestMethod]
        public void FromEmail()
        {
            string emailBody = File.ReadAllText("RestockNotices\\Shipment Received - Pavlova, Ltd..txt");
            MailMessage email = new MailMessage
            {
                Subject = "Shipment Received",
                Body = emailBody
            };
            //Supplier
            RestockNotice notice = RestockNotice.FromEmail(email);
            Assert.AreEqual("Pavlova, Ltd.", notice.Supplier);
            Assert.AreEqual("(7)", notice.SupplierID);

            //ProductID, ProductName, Quantity
            RestockDetails detail = notice.RestockDetails.Last();
            Assert.AreEqual(70, detail.ProductID);
            Assert.AreEqual("Outback Lager", detail.ProductName);
            Assert.AreEqual(10, detail.Quantity);
        }

        [TestMethod]
        public void TestAllEmail()
        {
            foreach (string fpath in Directory.GetFiles("RestockNotices", "*.txt"))
            {
                MailMessage msg = new MailMessage
                {
                    Subject = "Shipment Received",
                    Body = File.ReadAllText(fpath)
                };
                Console.WriteLine(fpath);
                RestockNotice notice = RestockNotice.FromEmail(msg);
                Assert.IsNotNull(notice.Supplier);
                Assert.IsNotNull(notice.SupplierID);

                RestockDetails details = notice.RestockDetails.Last();
                Assert.IsNotNull(details.ProductName);
                Assert.IsNotNull(details.ProductID);
                Assert.IsTrue(notice.Quantity > 0);

                //Can't be null: Supplier, SupplierID, ProductName
                //Greater than "0" = ProductID, Quantity

            }


        }
    }

}
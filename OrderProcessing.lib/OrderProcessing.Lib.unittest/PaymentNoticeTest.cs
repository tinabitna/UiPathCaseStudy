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
    class PaymentNoticeTest
    {
        [TestMethod]
        public void FromEmail()
        {
            string emailBody = File.ReadAllText("PaymentNoties\\Payment Received - Ernst Handel.txt");
            MailMessage email = new MailMessage
            {
                Subject = "Payment Received",
                Body = emailBody
            };
            //PaymentAmount, OrderDate, OrderNumber, CheckNumber
            PaymentNotice notice = PaymentNotice.FromEmail(email);
            Assert.AreEqual(5080.08M, notice.PaymentAmount);
            Assert.AreEqual(3-9-2019, notice.OrderDate);
            Assert.AreEqual(11008, notice.OrderNumber);
            Assert.AreEqual(2293, notice.CheckNumber);
        }

        [TestMethod]
        public void TestAllEmail()
        {
            foreach (string fpath in Directory.GetFiles("PaymentNotices", "*.txt"))
            {
                MailMessage msg = new MailMessage
                {
                    Subject = "Payment Notie",
                    Body = File.ReadAllText(fpath)
                };
                Console.WriteLine(fpath);
                PaymentNotice notice = PaymentNotice.FromEmail(msg);
                Assert.IsTrue(notice.PaymentAmount > 0);
                Assert.IsNotNull(notice.OrderNumber);
                Assert.IsNotNull(notice.CheckNumber);
                Assert.IsNotNull(notice.OrderDate);
            }


        }
    }

}

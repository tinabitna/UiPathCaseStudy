using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderProcessing.lib.Activities;

namespace OrderProcessing.Lib.unittest.Activities
{
    [TestClass]
    public class ExtractDateFromMailMessageActivityTest
    {
        [TestMethod]
        public void ParseDate()
        {
            //PrivateObject po = new PrivateObject(typeof(ExtractDateFromMailMessageActivityTest));
            //DateTime result = (DateTime)po.Invoke("ParseDate", "11 Mar 2019 10:04:59 -0400");
            DateTime result = ExtractDateFromMailMessageActivity.ParseDate("11 Mar 2019 10:04:59 -0400");
            Assert.AreEqual(new DateTime (2019 , 3 ,11 ,10 ,4 ,59), result);
        }
    }
}

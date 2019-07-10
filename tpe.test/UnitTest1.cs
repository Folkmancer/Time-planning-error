using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using tpe;

namespace tpe.test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string path = ".records.csv";
            Data data = new Data(path);
            DateTime startDate = DateTimeOffset.Now.LocalDateTime;
            DateTime endDate = DateTimeOffset.Now.LocalDateTime.AddHours(1);
            data.GetTimeFromSameDate();
            Assert.AreEqual
        }
    }
}

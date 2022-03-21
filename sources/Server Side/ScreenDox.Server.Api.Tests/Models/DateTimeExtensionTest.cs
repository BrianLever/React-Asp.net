using FrontDesk.Server.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ScreenDox.Server.Api.Tests.Models
{
    
    
    /// <summary>
    ///This is a test class for DateTimeExtensionTest and is intended
    ///to contain all DateTimeExtensionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DateTimeExtensionTest
    {

        [TestMethod()]
        public void GetDiffInMonthsTest_1()
        {
            DateTime endDate = new DateTime(2012, 10, 5);
            DateTime startDate = new DateTime(2012, 7, 1); 
            int expected = 3; 
            int actual;
            actual = DateTimeExtension.GetDiffInMonths(endDate, startDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetDiffInMonthsTest_2()
        {
            DateTime endDate = new DateTime(2012, 10, 5);
            DateTime startDate = new DateTime(2012, 7, 10);
            int expected = 2;
            int actual;
            actual = DateTimeExtension.GetDiffInMonths(endDate, startDate);
            Assert.AreEqual(expected, actual);
        }

    }
}

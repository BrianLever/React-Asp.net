using FrontDesk.Server.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Common;
using System.Collections.Generic;
using System;
using System.Data;
using FrontDesk.Common.Data;

namespace FrontDesk_UnitTest
{
    [TestClass()]
    public class ErrorLogTest
    {


        /// <summary>
        ///A test for Add
        ///</summary>
        [TestCategory("E2E")]
        [TestMethod()]
        public void AddTest()
        {
            ErrorLog logItem = new ErrorLog()
            {
                ErrorMessage = "Test error message",
                ErrorTraceLog = "Test error trace log"
            };
            const long expected = 0;
            var actual = ErrorLog.Add(logItem);
            Assert.AreNotEqual(expected, actual.ErrorLogID);
        }



        /// <summary>
        ///A test for Get
        ///</summary>
        [TestCategory("E2E")]
        [TestMethod()]
        public void GetTest()
        {
            ErrorLog expected = new ErrorLog()
            {
                ErrorMessage = "Test error message",
                ErrorTraceLog = "Test error trace log"
            };




            long errorLogID = ErrorLog.Add(expected).ErrorLogID;
            ErrorLog actual;
            actual = ErrorLog.Get(errorLogID);
            Assert.AreEqual(expected.ErrorLogID, actual.ErrorLogID);
            Assert.AreEqual(expected.ErrorMessage, actual.ErrorMessage);
            Assert.AreEqual(expected.ErrorTraceLog, actual.ErrorTraceLog);
            Assert.AreEqual(expected.CreatedDate, actual.CreatedDate);
        }

        /// <summary>
        ///A test for Delete
        ///</summary>

        [TestCategory("E2E")]
        [TestMethod()]
        public void DeleteTest1()
        {
            bool actual;
            var ds = ErrorLog.GetForExport(null, null, 0, 1000);
            if (DBDatabase.IsHasOneRow(ds))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    actual = ErrorLog.Delete(Convert.ToInt64(row["ErrorLogID"]));
                    Assert.AreEqual(true, actual);
                    break;
                }

            }
        }

        /// <summary>
        ///A test for Delete
        ///</summary>
        [TestCategory("E2E")]
        [TestMethod()]
        public void DeleteTest()
        {
            bool actual;
            bool expected = true;
            List<long> idList = new List<long>(2000);
            var result = ErrorLog.Get(null, null, 0, 2000);
            int max = 2000;
            int counter = 0;
            if (result.Count > 0)
            {
                foreach (var item in result)
                {
                    if (counter++ >= max) { break; }
                    idList.Add(item.ErrorLogID);
                }
                actual = ErrorLog.Delete(idList);
                Assert.AreEqual<bool>(expected, actual);
            }
        }



    }
}

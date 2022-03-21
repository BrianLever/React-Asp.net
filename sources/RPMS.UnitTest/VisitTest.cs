using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RPMS.Common;

namespace RPMS.UnitTest
{
    /// <summary>
    /// Summary description for VisitTest
    /// </summary>
    [TestClass]
    public class VisitTest
    {
        public VisitTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void GetPatientVisitsTest()
        {
            int patientID = 2;
            int? locationID = null;
            int pageIndex = 1, pageSize = 20;

            List<Visit> visits = Visit.GetPatientVisits(patientID, locationID, pageIndex, pageSize);
            Assert.AreNotEqual(null, visits);
            Assert.AreNotEqual(visits.Count, 0);
            Assert.IsTrue(visits.Count <= pageSize);

            pageIndex = 11;
            locationID = 2615;

            visits = Visit.GetPatientVisits(patientID, locationID, pageIndex, pageSize);
            Assert.AreNotEqual(null, visits);
            Assert.AreNotEqual(visits.Count, 0);
            Assert.IsTrue(visits.Count <= pageSize);
        }

        [TestMethod]
        public void GetPatientVisitsCountTest()
        {
            int patientID = 2;
            int? locationID = null;

            int count = Visit.GetPatientVisitsCount(patientID, locationID);
            Assert.AreNotEqual(0, count);

            locationID = 2615;
            count = Visit.GetPatientVisitsCount(patientID, locationID);
            Assert.AreNotEqual(0, count);

            patientID = -1;
            count = Visit.GetPatientVisitsCount(patientID, locationID);
            Assert.AreEqual(0, count);


            patientID = 2;
            locationID = -2;
            count = Visit.GetPatientVisitsCount(patientID, locationID);
            Assert.AreEqual(0, count);

        }
    }
}

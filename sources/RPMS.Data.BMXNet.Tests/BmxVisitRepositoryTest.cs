using RPMS.Data.BMXNet;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using RPMS.Common.Models;
using System.Collections.Generic;

namespace RPMS.Data.BMXNet.Tests
{


    /// <summary>
    ///This is a test class for BmxVisitRepositoryTest and is intended
    ///to contain all BmxVisitRepositoryTest Unit Tests
    ///</summary>
    [TestClass()]
    [Ignore]
    public class BmxVisitRepositoryTest
    {
      

        /// <summary>
        ///A test for GetVisitsByPatient
        ///</summary>
        [TestMethod()]
        public void GetVisitsByPatientTest()
        {
            BmxVisitRepository target = new BmxVisitRepository();
            int patientID = 16009; //DEMO,ANDREA
            int startRow = 5;
            int maxRows = 5;
            List<Visit> actual;
            actual = target.GetVisitsByPatient(patientID, startRow, maxRows);

            Assert.IsNotNull(actual);
            Assert.AreEqual(5, actual.Count, "Count failed.");

            DateTime maxArangeTime = actual[0].Date;
            //check order
            foreach(var visit in actual)
            {
                Assert.IsTrue(visit.Date <= maxArangeTime, "Sort by Date DESC failed. Current item: {0}. Prev: {1}", visit.Date, maxArangeTime);
                maxArangeTime = visit.Date;
            }


        }

        [TestMethod()]
        public void GetVisitsByPatientTest_can_return_last_page()
        {
            BmxVisitRepository target = new BmxVisitRepository();
            int patientID = 16009; //DEMO,ANDREA
            int startRow = 8;
            int maxRows = 5;
            List<Visit> actual;
            actual = target.GetVisitsByPatient(patientID, startRow, maxRows);

            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Count, "Count failed.");

            DateTime maxArangeTime = actual[0].Date;
            //check order
            foreach (var visit in actual)
            {
                Assert.IsTrue(visit.Date <= maxArangeTime, "Sort by Date DESC failed. Current item: {0}. Prev: {1}", visit.Date, maxArangeTime);
                maxArangeTime = visit.Date;
            }


        }

        [TestMethod()]
        public void GetVisitsByPatientTest_can_return_fist_page()
        {
            BmxVisitRepository target = new BmxVisitRepository();
            int patientID = 16009; //DEMO,ANDREA
            int startRow = 0;
            int maxRows = 5;
            List<Visit> actual;
            actual = target.GetVisitsByPatient(patientID, startRow, maxRows);

            Assert.IsNotNull(actual);
            Assert.AreEqual(5, actual.Count, "Count failed.");

            DateTime maxArangeTime = actual[0].Date;
            //check order
            foreach (var visit in actual)
            {
                Assert.IsTrue(visit.Date <= maxArangeTime, "Sort by Date DESC failed. Current item: {0}. Prev: {1}", visit.Date, maxArangeTime);
                maxArangeTime = visit.Date;
            }


        }

        /// <summary>
        ///A test for GetVisitsByPatientCount
        ///</summary>
        [TestMethod()]
        public void GetVisitsByPatientCountTest()
        {
            BmxVisitRepository target = new BmxVisitRepository();
            int patientID = 16009; //DEMO,ANDREA
            int expected = 10;
            int actual;
            actual = target.GetVisitsByPatientCount(patientID);
            Assert.IsTrue(actual >= expected);
        }

        /// <summary>
        ///A test for GetVisitRecord
        ///</summary>
        [TestMethod()]
        public void GetVisitRecordTest()
        {
            BmxVisitRepository target = new BmxVisitRepository();
            int visitId = 217367;
            Visit expected = new Visit
            {
                ID = visitId,
                Date = new DateTime(2010, 4, 16, 12, 0, 0),
                Location = new Location { Name = "JWARD AMERICAN HEALTH CENTER" },
                ServiceCategory = "AMBULATORY"
            };

            var patientSearch = new PatientSearch
            {
                ID = 16009 // //DEMO,ANDREA
            };

            Visit actual;
            actual = target.GetVisitRecord(visitId, patientSearch);

            Assert.IsNotNull(actual);

            Assert.AreEqual(expected.ID, actual.ID, "ID failed");
            Assert.AreEqual(expected.Date, actual.Date, "Date failed");
            Assert.AreEqual(expected.ServiceCategory, actual.ServiceCategory, "ServiceCategory failed");
            Assert.AreEqual(expected.Location.ID, actual.Location.ID, "Location.ID failed");
            Assert.AreEqual(expected.Location.Name, actual.Location.Name, "Location.Name failed");
        }
    }
}

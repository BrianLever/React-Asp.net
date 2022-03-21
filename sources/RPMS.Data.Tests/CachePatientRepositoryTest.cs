using RPMS.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using RPMS.Common.Models;
using System.Collections.Generic;

namespace RPMS.Data.Tests
{
    
    
    /// <summary>
    ///This is a test class for CachePatientRepositoryTest and is intended
    ///to contain all CachePatientRepositoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CachePatientRepositoryTest
    {


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
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for UpdatePatientRecordFields
        ///</summary>
        [TestMethod()]
        public void UpdatePatientRecordFieldsTest()
        {
            int patientId = 15859;
            CachePatientRepository target = new CachePatientRepository();
            IEnumerable<PatientRecordModification> modifications = new List<PatientRecordModification>{
                new PatientRecordModification{ Field = PatientRecordExportFields.AddressLine1, CurrentValue = "123 Main", UpdateWithValue = "267 HILL STREET"},
                //new PatientRecordModification{ Field = PatientRecordExportFields.AddressLine2, CurrentValue = null, UpdateWithValue = null},
                //new PatientRecordModification{ Field = PatientRecordExportFields.AddressLine3, CurrentValue = null, UpdateWithValue = null},
                //new PatientRecordModification{ Field = PatientRecordExportFields.City, CurrentValue = "Pauma Valley", UpdateWithValue = "Pauma Valley 1"},
                //new PatientRecordModification{ Field = PatientRecordExportFields.ZipCode, CurrentValue = "89323", UpdateWithValue = "89320"},
                //new PatientRecordModification{ Field = PatientRecordExportFields.StateID, CurrentValue = "CA", UpdateWithValue = "NY"},
                new PatientRecordModification{ Field = PatientRecordExportFields.Phone, CurrentValue = "(213) 202-3240", UpdateWithValue = "(213) 202-3000"},

            };
            int expected = 1;
            int actual;
            actual = target.UpdatePatientRecordFields(modifications, patientId, 0);
            Assert.AreEqual(expected, actual);
          
        }
    }
}

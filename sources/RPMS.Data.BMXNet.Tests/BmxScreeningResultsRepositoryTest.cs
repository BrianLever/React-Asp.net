using RPMS.Data.BMXNet;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using RPMS.Common.Models;
using FrontDesk;
using System.Collections.Generic;
using RPMS.Common.Configuration;

namespace RPMS.Data.BMXNet.Tests
{


    /// <summary>
    ///This is a test class for BmxScreeningResultsRepositoryTest and is intended
    ///to contain all BmxScreeningResultsRepositoryTest Unit Tests
    ///</summary>
    [TestClass]
    [Ignore]
    public class BmxScreeningResultsRepositoryTest
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
        ///A test for ExportExams
        ///</summary>
        [TestMethod()]
        public void ExportExamsTest()
        {
            BmxScreeningResultsRepository target = new BmxScreeningResultsRepository();
            int patientID = 16009;
            int visitID = 217367;
            List<Exam> list = new List<Exam>
            {
                new Exam
                {
                    ExamName = "DEPRESSION SCREENING",
                    ExamID = 34,
                    Code = "36",
                    Result = "PR",
                    Comment = "Score: {0}. Level of Risk: {1}. Exported from Screendox - Unit Test.".FormatWith(1, "Minimal Depression")
                },
                new Exam
                {
                    ExamName = "ALCOHOL SCREENING",
                    ExamID = 33,
                    Code = "33",
                    Result = "PO",
                    Comment = "Score: {0}. Level of Risk: {1}. Exported from Screendox - Unit Test.".FormatWith(2, "Current Problem")
                }
            };
            target.ExportExams(patientID, visitID, list);

        }

        [TestMethod()]
        public void ExportExamsTest_Case2()
        {
            BmxScreeningResultsRepository target = new BmxScreeningResultsRepository();
            int patientID = 16009;
            int visitID = 217213; //9/21/2009
            List<Exam> list = new List<Exam>
            {
                new Exam
                {
                    ExamName = "DEPRESSION SCREENING",
                    ExamID = 34,
                    Code = "36",
                    Result = "N",
                    Comment = "Score: {0}. Level of Risk: {1}. Exported from ScreenDox - Unit Test.".FormatWith(0, "No Depression")
                },
                new Exam
                {
                    ExamName = "ALCOHOL SCREENING",
                    ExamID = 33,
                    Code = "33",
                    Result = "PO",
                    Comment = "Score: {0}. Level of Risk: {1}. Exported from ScreenDox - Unit Test.".FormatWith(2, "Current Problem")
                }
            };
            target.ExportExams(patientID, visitID, list);

        }

        /// <summary>
        ///A test for ExportHealthFactors
        ///</summary>
        [DeploymentItem(@"Configuration\rpmsExportConfiguration.config", "Configuration")]
        [TestMethod()]
        public void ExportHealthFactorsTest()
        {
            BmxScreeningResultsRepository target = new BmxScreeningResultsRepository();
            int patientID = 16009;
            int visitID = 217177;
            string testID = Guid.NewGuid().ToString("N");


            List<HealthFactor> healthFactors = new List<HealthFactor>();

            var rpmsConfigSection = RPMS.Common.Configuration.RpmsExportConfiguration.GetConfiguration();
            foreach (HealthFactorElement factor in rpmsConfigSection.HealthFactors)
            {
                healthFactors.Add(
                    new HealthFactor
                {
                    Factor = factor.Factor,
                    FactorID = factor.Id,
                    Code = factor.Code,
                    Comment = "Exported from Screendox - Unit Test-" + testID
                });
            }

            target.ExportHealthFactors(patientID, visitID, healthFactors);

        }

        /// <summary>
        ///A test for ExportCrisisAler
        ///</summary>
        [TestMethod()]
        public void ExportCrisisAlertTest()
        {
            BmxScreeningResultsRepository target = new BmxScreeningResultsRepository();
            int patientID = 16009;
            int visitID = 217367;
            List<CrisisAlert> list = new List<CrisisAlert>
            {
                new CrisisAlert
                {
                    Title = "CLINICAL WARNING",
                    DocumentTypeID = 15,
                    Author = "FrontDesk BHS",
                    EntryDate = new DateTime(2012, 5, 10, 16, 01, 15),
                    DateOfNote = new DateTime(2012, 5, 7, 12, 00, 15),
                    Details = @"Patient answered ""Unit Test"" to: ""Over the LAST 12 MONTHS, how often did your partner, family member, or caregiver Physically HURT you?"""
                },
                //new CrisisAlert
                //{
                //    Title = "CLINICAL WARNING",
                //    DocumentTypeID = 15,
                //   Author = "FrontDesk BHS",
                //    EntryDate = new DateTime(2012, 5, 10, 16, 01, 15),
                //    DateOfNote = new DateTime(2012, 5, 7, 12, 00, 15),
                //    Details = @"Patient answered ""Unit Test"" to ""Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems? Thoughts that you would be better off dead or of hurting yourself in some way."""
                //}
            };
            target.ExportCrisisAlerts(patientID, visitID, list);

            int testCase = 0;
            //read crisis alert
            foreach(var expected in list)
            {
                var actualResult = target.GetJustInsertedRecord(patientID, visitID, expected);

                Assert.IsTrue(actualResult.ID > 0, "ID is null. Case:" + testCase);
                Assert.AreEqual(expected.Details, actualResult.Details, "Case:" + testCase);


                ++testCase;
            }

        }
    }
}

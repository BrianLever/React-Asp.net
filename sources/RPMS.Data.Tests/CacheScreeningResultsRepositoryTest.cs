using RPMS.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using RPMS.Common.Models;
using FrontDesk;
using System.Collections.Generic;

namespace RPMS.Data.Tests
{


    /// <summary>
    ///This is a test class for CacheScreeningResultsRepositoryTest and is intended
    ///to contain all CacheScreeningResultsRepositoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CacheScreeningResultsRepositoryTest
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
        ///A test for ExportHealthFactors
        ///</summary>
        [TestMethod()]
        public void ExportHealthFactorsTest()
        {
            CacheScreeningResultsRepository target = new CacheScreeningResultsRepository();
            int patientID = 15859; //Demo,Patient A
            int visitID = 217696;
            List<HealthFactor> healthFactors = new List<HealthFactor> {
                new HealthFactor
                {
                    Factor = "CAGE 0/4",
                    FactorID = 19,
                    Code = "F018",
                    Comment = "Exported from FrontDesk BHS."
                },
                new HealthFactor
                {
                    Factor = "CURRENT SMOKELESS",
                    FactorID = 3,
                    Code = "F003",
                    Comment = "Exported from FrontDesk BHS."
                },
            
            };
            target.ExportHealthFactors(patientID, visitID, healthFactors);
           
        }

        /// <summary>
        ///A test for ExportExams
        ///</summary>
        [TestMethod()]
        public void ExportExamsTest()
        {
            CacheScreeningResultsRepository target = new CacheScreeningResultsRepository();
            int patientID = 15859; //Demo,Patient A
            int visitID = 217696;
            List<Exam> list = new List<Exam>
            {
                new Exam
                {
                    ExamName = "DEPRESSION SCREENING",
                    ExamID = 34,
                    Code = "36",
                    Result = "PR",
                    Comment = "Score: {0}. Level of Risk: {1}. Exported from FrontDesk BHS.".FormatWith(1, "Minimal Depression")
                },
                new Exam
                {
                    ExamName = "ALCOHOL SCREENING",
                    ExamID = 33,
                    Code = "33",
                    Result = "PO",
                    Comment = "Score: {0}. Level of Risk: {1}. Exported from FrontDesk BHS.".FormatWith(2, "Current Problem")
                }
            }; 
            target.ExportExams(patientID, visitID, list);
        }
    }
}

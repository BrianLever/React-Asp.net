using RPMS.Common.Export;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FrontDesk;
using System.Collections.Generic;
using RPMS.Common.Models;
using FluentAssertions;

namespace RPMS.UnitTest.Export
{
    
    
    /// <summary>
    ///This is a test class for DepressionCrisisAlertCalculatorTest and is intended
    ///to contain all DepressionCrisisAlertCalculatorTest Unit Tests
    ///</summary>
    [DeploymentItem(@"Configuration\rpmsExportConfiguration.config", "Configuration")]
    [TestClass()]
    public class DepressionCrisisAlertCalculatorTest
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



        [TestMethod()]
        public void Calculate_should_return_null_for_non_asked()
        {
            ScreeningSectionResult sectionResult = ScreeningResultHelper.GetAlcoholAllYes();
            DepressionCrisisAlertCalculator target = new DepressionCrisisAlertCalculator();
            IList<CrisisAlert> actual;

          
            actual = target.Calculate(new ScreeningSectionResult[] { sectionResult });
            actual.Should().NotBeNull();
            actual.Should().BeEmpty();
        }

        [TestMethod()]
        public void Calculate_should_return_null_for_no()
        {
            ScreeningResult sectionResult = ScreeningResultHelper.GetAllNoAnswers();
            DepressionCrisisAlertCalculator target = new DepressionCrisisAlertCalculator();
            IList<CrisisAlert> actual;


            actual = target.Calculate(sectionResult.SectionAnswers);
            
            actual.Should().NotBeNull();
            actual.Should().BeEmpty();
        }


        [TestMethod()]
        public void Calculate_should_return_null_for_never()
        {
            ScreeningSectionResult sectionResult = ScreeningResultHelper.GetDepressionHurtYouselfNotAtAll();
            DepressionCrisisAlertCalculator target = new DepressionCrisisAlertCalculator();
            IList<CrisisAlert> actual;


            actual = target.Calculate(new ScreeningSectionResult[] { sectionResult });
            
            actual.Should().NotBeNull();
            actual.Should().BeEmpty();

        }

        [TestMethod()]
        public void Calculate_should_return_for_rarery()
        {
            ScreeningSectionResult sectionResult = ScreeningResultHelper.GetDepressionHurtYouselfSeveralDays();
            DepressionCrisisAlertCalculator target = new DepressionCrisisAlertCalculator();
            IList<CrisisAlert> actual;


            actual = target.Calculate(new ScreeningSectionResult[] { sectionResult });

            actual.Should().NotBeNull();
            actual.Should().NotBeEmpty();
            actual.Count.Should().Be(1);

            var actualAlert = actual[0];

            Assert.AreEqual("CLINICAL WARNING", actualAlert.Title, "Title failed");
            Assert.AreEqual(15, actualAlert.DocumentTypeID, "DocumentTypeID failed");
            Assert.AreEqual(@"Patient answered ""Several days"" to ""Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems? Thoughts that you would be better off dead or of hurting yourself in some way.""", actualAlert.Details, "Detail failed");

        }
    }
}

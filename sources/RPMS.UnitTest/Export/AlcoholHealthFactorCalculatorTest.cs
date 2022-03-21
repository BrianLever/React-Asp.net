using RPMS.Common.Export;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FrontDesk;
using RPMS.Common.Models;
using FluentAssertions;
using System.Collections.Generic;


namespace RPMS.UnitTest.Export
{


    [DeploymentItem(@"Configuration\rpmsExportConfiguration.config", "Configuration")]
    [TestClass()]
    public class AlcoholHealthFactorCalculatorTest
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
        public void Calculate_should_return_null()
        {
            ScreeningSectionResult sectionResult = ScreeningResultHelper.GetAlcoholAllNo();
            AlcoholHealthFactorCalculator target = new AlcoholHealthFactorCalculator();
            IList<HealthFactor> actual;

            HealthFactor expected = new HealthFactor
            {
                Factor = "CAGE 0/4",
                FactorID = 19,
                Code = "F018",
                Comment = "Exported from Screendox."
            };

            actual = target.Calculate(new ScreeningSectionResult[]{sectionResult});
            actual.Should().NotBeNull();
            actual.Should().NotBeEmpty();
            actual.Count.Should().Be(1);

            var actualFactor = actual[0];

            Assert.AreEqual(expected.Factor, actualFactor.Factor, "Factor is not equal");
            Assert.AreEqual(expected.FactorID, actualFactor.FactorID, "FactorID is not equal");
            Assert.AreEqual(expected.Code, actualFactor.Code, "Code is not equal");

            Assert.AreEqual(expected.Comment, actualFactor.Comment, "Comment is not equal");
        }




        [TestMethod()]
        public void Calculate_should_work_with_all_yes()
        {
            ScreeningSectionResult sectionResult = ScreeningResultHelper.GetAlcoholAllYes();
            AlcoholHealthFactorCalculator target = new AlcoholHealthFactorCalculator();
            HealthFactor expected = new HealthFactor
            {
                Factor = "CAGE 4/4",
                FactorID = 23,
                Code = "F022",
                Comment = "Exported from Screendox."
            };
            IList<HealthFactor> actual;
            actual = target.Calculate(new ScreeningSectionResult[] { sectionResult });
            
            actual.Should().NotBeNull();
            actual.Should().NotBeEmpty();
            actual.Count.Should().Be(1);

            
            var actualHealthFactor = actual[0];

            Assert.AreEqual(expected.Factor, actualHealthFactor.Factor, "Factor is not equal");
            Assert.AreEqual(expected.FactorID, actualHealthFactor.FactorID, "FactorID is not equal");
            Assert.AreEqual(expected.Code, actualHealthFactor.Code, "Code is not equal");

            Assert.AreEqual(expected.Comment, actualHealthFactor.Comment, "Comment is not equal");

        }

    }
}

using RPMS.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace RPMS.UnitTest
{


    [DeploymentItem(@"Configuration\rpmsExportConfiguration.config", "Configuration")]
    [TestClass()]
    public class RpmsExportConfigurationTest
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
        ///A test for GetConfiguration
        ///</summary>
        [TestMethod()]
        public void Can_read_factors()
        {
            RpmsExportConfiguration actual;

            actual = RpmsExportConfiguration.GetConfiguration();
            
            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.HealthFactors, "Health factors are empty");
            var factors = actual.HealthFactors;
            Assert.AreEqual<int>(12, factors.Count, "factor length failed");

            HealthFactorElement element;

            element = factors[0];
            Assert.IsNotNull(element);

            Assert.AreEqual(12, element.Id, "Id failed");
            Assert.AreEqual("SMOKER IN HOME", element.Factor, "Factor failed");
            Assert.AreEqual("F006", element.Code, "Code failed");

            element = factors[7];
            Assert.IsNotNull(element);

            Assert.AreEqual(19, element.Id, "Id failed");
            Assert.AreEqual("CAGE 0/4", element.Factor, "Factor failed");
            Assert.AreEqual("F018", element.Code, "Code failed");


        
        }


        [TestMethod()]
        public void Can_read_crisisAlerts()
        {
            RpmsExportConfiguration expected = RpmsExportConfiguration.GetConfiguration();
            RpmsExportConfiguration actual;

            actual = RpmsExportConfiguration.GetConfiguration();

            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.CrisisAlerts, "Crisis alerts are empty");
            var alerts = actual.CrisisAlerts;
            Assert.AreEqual<int>(1, alerts.Count, "alert length failed");

            CrisisAlertElement element;

            element = alerts[0];
            Assert.IsNotNull(element);

            Assert.AreEqual(15, element.Id, "Id failed");
            Assert.AreEqual("CLINICAL WARNING", element.Name, "Name failed");
            Assert.AreEqual("CW", element.Abbr, "Abbr failed");



        }
    }
}

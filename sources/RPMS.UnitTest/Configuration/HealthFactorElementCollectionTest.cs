using RPMS.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RPMS.UnitTest
{
    [DeploymentItem(@"Configuration\rpmsExportConfiguration.config", "Configuration")]
    [TestClass()]
    public class HealthFactorElementCollectionTest
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

        string[] keys = new string[]{
            HealthFactorKeys.AlcoholCage0,
            HealthFactorKeys.AlcoholCage1,
HealthFactorKeys.AlcoholCage2,
HealthFactorKeys.AlcoholCage3,
HealthFactorKeys.AlcoholCage4,
HealthFactorKeys.TobaccoCeremonialUseOnly,
HealthFactorKeys.TobaccoCurrentNonSmoker,
HealthFactorKeys.TobaccoCurrentSmokeless,
HealthFactorKeys.TobaccoCurrentSmoker,
HealthFactorKeys.TobaccoCurrentSmokerAndSmokeless,
HealthFactorKeys.TobaccoSmokerFreeHome,
HealthFactorKeys.TobaccoSmokerInHome,
        };

        int[] ids = new int[]{
19,
20,
21,
22,
23,
69,
1,
3,
2,
14,
13,
12
        };



        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod()]
        public void Check_all_keys()
        {
            RpmsElementCollection<HealthFactorElement> target = RpmsExportConfiguration.GetConfiguration().HealthFactors;

            for (int i = 0; i < keys.Length; i++)
            {
                Assert.AreEqual<int>(ids[i], target[keys[i]].Id, "Failed on " + i + ". key: " + keys[i]);
            }

        }
    }
}

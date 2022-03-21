using FrontDesk.Server.Screening;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FrontDesk_UnitTest
{


    /// <summary>
    ///This is a test class for IndicatorReportItemTest and is intended
    ///to contain all IndicatorReportItemTest Unit Tests
    ///</summary>
    [TestClass()]
    public class IndicatorReportItemTest
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

        //#region Yes/No Percent tests


        //[TestMethod()]
        ////[DeploymentItem("FrontDesk.Server.dll")]
        //public void Can_Calculate_YesNoPercent()
        //{
        //    IndicatorReportItem item = new IndicatorReportItem("TCC", string.Empty,
        //        yesCount: 4,
        //        noCount: 13,
        //        positiveCount: 0,
        //        negativeCount: 0
        //    );

        //    double yesPerc = 23.53;
        //    double noPerc = 76.47;
        //    long yesNoCount = 17;

        //    Assert.AreEqual(yesNoCount, item.TotalYesNoCount, "TotalYesNoCount failed");
        //    Assert.AreEqual(yesPerc, item.YesPercent, "YesPercent failed");
        //    Assert.AreEqual(noPerc, item.NoPercent, "NoPercent failed");

        //}


        //[TestMethod()]
        ////[DeploymentItem("FrontDesk.Server.dll")]
        //public void Can_Calculate_YesNoPercent_with_yes_zero()
        //{
        //    IndicatorReportItem item = new IndicatorReportItem("TCC", string.Empty,
        //        yesCount: 0,
        //        noCount: 13,
        //        positiveCount: 0,
        //        negativeCount: 0
        //    );

        //    double yesPerc = 0;
        //    double noPerc = 100;
        //    long yesNoCount = 13;

        //    Assert.AreEqual(yesNoCount, item.TotalYesNoCount, "TotalYesNoCount failed");
        //    Assert.AreEqual(yesPerc, item.YesPercent, "YesPercent failed");
        //    Assert.AreEqual(noPerc, item.NoPercent, "NoPercent failed");

        //}


        //[TestMethod()]
        //public void Can_Calculate_YesNoPercent_with_no_zero()
        //{
        //    IndicatorReportItem item = new IndicatorReportItem("TCC", string.Empty,
        //        yesCount: 13,
        //        noCount: 0,
        //        positiveCount: 0,
        //        negativeCount: 0
        //    );

        //    double yesPerc = 100;
        //    double noPerc = 0;
        //    long yesNoCount = 13;

        //    Assert.AreEqual(yesNoCount, item.TotalYesNoCount, "TotalYesNoCount failed");
        //    Assert.AreEqual(yesPerc, item.YesPercent, "YesPercent failed");
        //    Assert.AreEqual(noPerc, item.NoPercent, "NoPercent failed");

        //}

        //[TestMethod()]
        //public void Can_Calculate_YesNoPercent_both_zero()
        //{
        //    IndicatorReportItem item = new IndicatorReportItem("TCC", string.Empty,
        //        yesCount: 0,
        //        noCount: 0,
        //        positiveCount: 0,
        //        negativeCount: 0
        //    );

        //    double yesPerc = Double.NaN;
        //    double noPerc = Double.NaN;
        //    long yesNoCount = 0;

        //    Assert.AreEqual(yesNoCount, item.TotalYesNoCount, "TotalYesNoCount failed");
        //    Assert.AreEqual(yesPerc, item.YesPercent, "YesPercent failed");
        //    Assert.AreEqual(noPerc, item.NoPercent, "NoPercent failed");

        //}

        //#endregion


        #region Positive/Negative Percent tests

        [TestMethod()]
        public void Can_Calculate_PosNegPercent()
        {
            IndicatorReportItem item = new IndicatorReportItem("TCC", 0, string.Empty, String.Empty,
                 positiveCount: 4,
                 negativeCount: 13
            );

            double posPerc = 23.53;
            double negPerc = 76.47;
            long posNegCount = 17;

            Assert.AreEqual(posNegCount, item.TotalCount, "TotalCount failed");
            Assert.AreEqual(posPerc, item.PositivePercent, "PositivePercent failed");
            Assert.AreEqual(negPerc, item.NegativePercent, "NegativePercent failed");

        }
      
        [TestMethod()]
        public void Can_Calculate_PosNegPercent_with_pos_zero()
        {
            IndicatorReportItem item = new IndicatorReportItem("TCC", 0, string.Empty, String.Empty,
                  positiveCount: 0,
                  negativeCount: 13
             );
           
            double posPerc = 0;
            double negPerc = 100;
            long posNegCount = 13;

            Assert.AreEqual(posNegCount, item.TotalCount, "TotalCount failed");
            Assert.AreEqual(posPerc, item.PositivePercent, "PositivePercent failed");
            Assert.AreEqual(negPerc, item.NegativePercent, "NegativePercent failed");

        }
       
        [TestMethod()]
        public void Can_Calculate_PosNegPercent_with_negative_zero()
        {
            IndicatorReportItem item = new IndicatorReportItem("TCC", 0, string.Empty, string.Empty,
                 positiveCount: 13,
                 negativeCount: 0
            );
          
            double posPerc = 100;
            double negPerc = 0;
            long posNegCount = 13;

            Assert.AreEqual(posNegCount, item.TotalCount, "TotalCount failed");
            Assert.AreEqual(posPerc, item.PositivePercent, "PositivePercent failed");
            Assert.AreEqual(negPerc, item.NegativePercent, "NegativePercent failed");

        }

        [TestMethod()]
        public void Can_Calculate_PosNegPercent_both_zero()
        {
            IndicatorReportItem item = new IndicatorReportItem("TCC", 0, string.Empty, string.Empty,
                 positiveCount: 0,
                 negativeCount: 0
            );

            double posPerc = Double.NaN;
            double negPerc = Double.NaN;
            long posNegCount = 0;

            Assert.AreEqual(posNegCount, item.TotalCount, "TotalCount failed");
            Assert.AreEqual(posPerc, item.PositivePercent, "PositivePercent failed");
            Assert.AreEqual(negPerc, item.NegativePercent, "NegativePercent failed");

        }

        #endregion
    }
}

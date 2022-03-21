using FrontDesk.Server.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using System;
using FrontDesk;
using System.Collections.Generic;

namespace FrontDesk_UnitTest
{
    
    
    /// <summary>
    ///This is a test class for KioskEndpointTest and is intended
    ///to contain all KioskEndpointTest Unit Tests
    ///</summary>
    [TestClass()]
    public class KioskEndpointTest
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


        ///// <summary>
        /////A test for GetModifiedAgeSettings
        /////</summary>
        //// TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        //// http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        //// whether you are testing a page, web service, or a WCF service.
        //[TestMethod()]
        //[HostType("ASP.NET")]
        //[AspNetDevelopmentServerHost("C:\\Projects\\FrontDesk\\FrontDeskServer", "/FrontDeskServer")]
        //[UrlToTest("http://localhost/FrontDeskServer")]
        //public void GetModifiedAgeSettingsTest()
        //{
        //    KioskEndpoint_Accessor target = new KioskEndpoint_Accessor();
        //    DateTime lastModifiedDateUTC = new DateTime(2000,1,1); 
        //    List<ScreeningSectionAgeView> actual;
        //    actual = target.GetModifiedAgeSettings(lastModifiedDateUTC);
        //    Assert.IsTrue(actual.Count > 0);
        //}
    }
}

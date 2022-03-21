using FrontDesk.Server.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace FrontDesk_UnitTest
{


    /// <summary>
    ///This is a test class for BaseWebFormTest and is intended
    ///to contain all BaseWebFormTest Unit Tests
    ///</summary>
    [TestClass()]
    public class BaseWebFormTest
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


        internal virtual BaseWebForm CreateBaseWebForm()
        {
            // TODO: Instantiate an appropriate concrete class.
            BaseWebForm target = null;
            return target;
        }

        /// <summary>
        ///A test for GetRedirectToPageUrl
        ///</summary>
        [TestMethod()]
        public void GetRedirectToPageUrlTest_can_replace()
        {
           
            string url = "https://localhost:8888/frontdesk/ExportWizard.aspx?id=1&rpmsPatientId=12497&rpmsVisitId=165501&step=2";
            IDictionary<string, object> parameters = new Dictionary<string, object>{
            {"step", 3}, 
            {"id", 1},
            {"rpmsPatientId", 12497}, 
            {"rpmsVisitId",1701}
        };
            string expected = "https://localhost:8888/frontdesk/ExportWizard.aspx?id=1&rpmsPatientId=12497&rpmsVisitId=1701&step=3";
            string actual;
            actual = BaseWebForm.GetRedirectToPageUrl(url, parameters);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetRedirectToPageUrlTest_can_clean()
        {

            string url = "http://jlwa.3sicorp.com:8443/frontdesk/ExportWizard.aspx?id=1&rpmsPatientId=12497&rpmsVisitId=165501&step=2";
            IDictionary<string, object> parameters = new Dictionary<string, object>{
            {"step", 3}, 
            {"id", 1},
            {"rpmsPatientId", 1245}, 
            {"rpmsVisitId",null}
        };
            string expected = "http://jlwa.3sicorp.com:8443/frontdesk/ExportWizard.aspx?id=1&rpmsPatientId=1245&rpmsVisitId=&step=3";
            string actual;
            actual = BaseWebForm.GetRedirectToPageUrl(url, parameters);
            Assert.AreEqual(expected, actual);
        }
    }
}

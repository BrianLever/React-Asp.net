using FrontDesk.Deployment.SQLServerInstaller;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
namespace FrontDesk_UnitTest
{
    
    
    /// <summary>
    ///This is a test class for ProgramTest and is intended
    ///to contain all ProgramTest Unit Tests
    ///</summary>
    [TestClass()]
    [Ignore]
    public class SQLServerInstallerTest
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
        ///A test for Main
        ///</summary>
        [TestMethod()]
        [DeploymentItem("SQLServerInstaller.exe")]
        public void MainTest()
        {
            var form = new FrontDesk.Deployment.SQLServerInstaller.Form1();
           form.IsDemoMode = true;
            var res = form.ShowDialog();

            Assert.AreEqual(DialogResult.Cancel, DialogResult.Cancel);
        }
    }
}

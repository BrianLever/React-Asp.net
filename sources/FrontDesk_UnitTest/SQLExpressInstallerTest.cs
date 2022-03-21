using FrontDesk.Deployment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace FrontDesk_UnitTest
{
    
    
    /// <summary>
    ///This is a test class for SQLExpressInstallerTest and is intended
    ///to contain all SQLExpressInstallerTest Unit Tests
    ///</summary>
    [TestClass()]
    [Ignore]
    public class SQLExpressInstallerTest
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
        ///A test for GetPathToSqlInstallationx86
        ///</summary>
        [TestMethod()]
        public void GetPathToSqlInstallationx86Test()
        {
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = SQLExpressInstaller.GetPathToSqlInstallationx86();
            Assert.IsTrue(!string.IsNullOrEmpty(actual));
            
        }

        /// <summary>
        ///A test for CheckSQLInstanceExists
        ///</summary>
        [TestMethod()]
        public void CheckSQLInstanceExistsTest()
        {
            string instanceName = "SQLExpress"; 
            string version = "10."; 
            bool expected = true; 
            bool actual;
            actual = SQLExpressInstaller.CheckSQLInstanceExists(instanceName, version);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CreateDatabase
        ///</summary>
        [TestMethod()]
        [DeploymentItem("install_server.sql", "SQL")]
        public void CreateDatabaseTest()
        {
            string serverName = @".\SQLExpress"; // TODO: Initialize to an appropriate value
            string username = "frontdesk_appuser_unittest"; // TODO: Initialize to an appropriate value
            string password = "frontDeks123456"; 
            string sqlFileName = @"SQL\install_server.sql"; 
            SQLExpressInstaller.CreateDatabase(serverName, username, password, sqlFileName);
            
        }

        /// <summary>
        ///A test for GetInstalledMSIVersion
        ///</summary>
        [TestMethod()]
        public void GetInstalledMSIVersionTest()
        {
          
            ProductVersion expected = new ProductVersion( "5.0");
            

            if (System.Environment.OSVersion.Version.Major < 6 ) //less then Windows 7
                expected.VersionAsString = "4.5";

            ProductVersion actual;
            actual = SQLExpressInstaller.GetInstalledMsiVersion();
            Assert.IsTrue(actual >= expected);
        }

        /// <summary>
        ///A test for GetMsi45InstallationFile
        ///</summary>
        [TestMethod()]
        public void GetMsi45InstallationFileTest()
        {
            string expected = string.Empty;
            if (System.Environment.OSVersion.Version.Major == 6 ) 
            {
                if(OSInfo.GetOsInfo().Platform == PlatformArchitecture.x86)
                {
                    expected = "Windows6.0-KB942288-v2-x86.MSU";
                }
                else{
                    expected = "Windows6.0-KB942288-v2-x64.MSU";
                }
            }
            else if( System.Environment.OSVersion.Version.Major == 5)
            {
                if (OSInfo.GetOsInfo().Platform == PlatformArchitecture.x86)
                {
                    if (System.Environment.OSVersion.Version.Major == 5 && System.Environment.OSVersion.Version.Minor < 2)
                    {
                        expected = "WindowsXP-KB942288-v3-x86.exe";
                    }
                    else
                    {
                        expected = "WindowsServer2003-KB942288-v4-x86.exe";
                    }
                }
                else
                {
                    expected = "WindowsServer2003-KB942288-v4-x64.exe";
                }
            }
            string actual;
            actual = SQLExpressInstaller.GetMsi45InstallationFile();
            Assert.AreEqual(expected, actual);
           
        }
    }
}

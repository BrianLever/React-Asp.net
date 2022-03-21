using FrontDesk.Deployment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Data.SqlClient;

namespace FrontDesk_UnitTest
{


    /// <summary>
    ///This is a test class for ConfigurationHelperTest and is intended
    ///to contain all ConfigurationHelperTest Unit Tests
    ///</summary>
    [TestClass()]
    [Ignore]
    public class ConfigurationHelperTest
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
        ///A test for SetSQL2008ConnectionStringSqlAuthentication
        ///</summary>
        [TestMethod()]
        [DeploymentItem("configs/web.config", "")]
        public void SetSQL2008ConnectionStringSqlAuthenticationTest()
        {
            ConfigurationHelper target = new ConfigurationHelper(); 

            string applicationDirectory = System.Environment.CurrentDirectory;

            Configuration configFile = target.OpenWebConfigurationByPhysicalPath(applicationDirectory);

            string name = "LocalConnection";
            string servername = @".\SQLEXPRESS";
            string databasename = "FrontDesk"; // TODO: Initialize to an appropriate value
            string username = "frontdesk_appuser";
            string password = "Q1w2e3r4t5";
            string applicationName = "FrontDesk Server";
            bool clearAll = false;
            target.SetSQL2008ConnectionStringSqlAuthentication(configFile, name, servername, databasename, username, password, applicationName, clearAll);
            configFile.Save();

            SqlConnectionStringBuilder strB = new SqlConnectionStringBuilder(configFile.ConnectionStrings.ConnectionStrings[0].ConnectionString);

            Assert.AreEqual(servername, strB.DataSource);
            Assert.AreEqual(databasename, strB.InitialCatalog);
            Assert.AreEqual(username, strB.UserID);
            Assert.AreEqual(password, strB.Password);
            Assert.AreEqual(applicationName, strB.ApplicationName);
            Assert.AreEqual(false, strB.IntegratedSecurity);

        }

        /// <summary>
        ///A test for EncryptConnectionString
        ///</summary>
        [TestMethod()]
        public void EncryptConnectionStringTest()
        {
            ConfigurationHelper target = new ConfigurationHelper(); 

            
            string applicationDirectory = System.Environment.CurrentDirectory;
            
            string installationPath = applicationDirectory; 
            target.EncryptConnectionString(installationPath);
        }

        /// <summary>
        ///A test for EncryptConnectionString
        ///</summary>
        [TestMethod()]
        public void EncryptConnectionString2Test()
        {
            ConfigurationHelper target = new ConfigurationHelper(); 

            string applicationDirectory = @"c:\inetpub\frontdesk.kappa.blacksea.globaltides.com\";

            string installationPath = applicationDirectory;
            target.EncryptConnectionString(installationPath);
        }

        /// <summary>
        ///A test for EncryptConnectionString
        ///</summary>
        [TestMethod()]
        public void DecryptConnectionString()
        {
            ConfigurationHelper target = new ConfigurationHelper(); 


            string applicationDirectory = @"C:\inetpub\frontdesk.kappa.blacksea.globaltides.com\";

            string installationPath = applicationDirectory;
            target.DecryptConnectionString(installationPath);
        }

        /// <summary>
        ///A test for EncryptConnectionString
        ///</summary>
        [TestMethod()]
        public void EncryptMachineKey2Test()
        {
            ConfigurationHelper target = new ConfigurationHelper(); 


            string applicationDirectory = @"c:\inetpub\frontdesk.kappa.blacksea.globaltides.com\";

            string installationPath = applicationDirectory;
            target.EncryptMachineKeyString(installationPath);
        }

        /// <summary>
        ///A test for EncryptConnectionString
        ///</summary>
        [TestMethod()]
        public void DecryptMachineKey()
        {
            ConfigurationHelper target = new ConfigurationHelper(); 

            string applicationDirectory = @"C:\inetpub\frontdesk.kappa.blacksea.globaltides.com";

            string installationPath = applicationDirectory;
            target.DecryptMachineKeyString(installationPath);
        }

        
    
        /// <summary>
        ///A test for ChangeWinServiceStartMode
        ///</summary>
        [TestMethod()]
        [Ignore]
        public void ChangeWinServiceStartModeTest()
        {
            ConfigurationHelper target = new ConfigurationHelper(); 

            string serviceName = "aspnet_state"; 
            ConfigurationHelper.Win32ServiceStartMode startMode = ConfigurationHelper.Win32ServiceStartMode.Manual;
            ConfigurationHelper.Win32ServiceReturnValue expected = ConfigurationHelper.Win32ServiceReturnValue.Success; 
            ConfigurationHelper.Win32ServiceReturnValue actual;
            actual = target.ChangeWinServiceStartMode(serviceName, startMode);
            Assert.AreEqual(expected, actual);

            startMode = ConfigurationHelper.Win32ServiceStartMode.Automatic;
            actual = target.ChangeWinServiceStartMode(serviceName, startMode);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for StartWinService
        ///</summary>
        [TestMethod()]
        public void StartWinServiceTest()
        {
            ConfigurationHelper target = new ConfigurationHelper(); 

            string serviceName = "aspnet_state";
            ConfigurationHelper.Win32ServiceReturnValue expected = ConfigurationHelper.Win32ServiceReturnValue.ServiceAlreadyRunning;
            ConfigurationHelper.Win32ServiceReturnValue actual;
            actual = target.StartWinService(serviceName);
            Assert.AreEqual(expected, actual);
           
        }

        /// <summary>
        ///A test for IsXP
        ///</summary>
        [TestMethod()]
        public void IsXPTest()
        {
            ConfigurationHelper target = new ConfigurationHelper(); 

            bool expected = false; 
            bool actual;
            actual = target.IsXP();
            Assert.AreEqual(expected, actual);
          
        }

        /// <summary>
        ///A test for RegisterWCFScripts
        ///</summary>
        [TestMethod()]
        public void RegisterWCFScriptsTest()
        {
            ConfigurationHelper target = new ConfigurationHelper(); 

            target.RegisterWCFScripts();
        }

        /// <summary>
        ///A test for CreateLocalGroup
        ///</summary>
        [TestMethod()]
        public void CreateLocalGroupTest()
        {
            ConfigurationHelper target = new ConfigurationHelper(); 

            string groupName = "FrontDesk Administrators";
            string description = "FrontDesk License Management Tool users";
            bool expected = true;
            bool actual;
            actual = target.CreateLocalGroup(groupName, description);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ModifyIISAuthenticationMode
        ///</summary>
        [TestMethod()]
        public void ModifyIISAuthenticationModeTest()
        {
/*
            ConfigurationHelper_Accessor target = new ConfigurationHelper_Accessor(); // TODO: Initialize to an appropriate value
            string websiteId = string.Empty; // TODO: Initialize to an appropriate value
            uint allowAuthFlags = 0; // TODO: Initialize to an appropriate value
            target.ModifyIISAuthenticationMode(websiteId, allowAuthFlags);
*/            
        }

        /// <summary>
        ///A test for GetWebsiteIdByName
        ///</summary>
        //[TestMethod()]
        //public void GetWebsiteIdByNameTest()
        //{
        //    ConfigurationHelper_Accessor target = new ConfigurationHelper_Accessor(); 
        //    string websiteName = "Default Web Site"; 
        //    string expected = "1"; // TODO: is default site always #1?
        //    string actual;
        //    actual = target.GetWebsiteIdByName(websiteName);
        //    Assert.AreEqual(expected, actual);            
        //}

        /// <summary>
        ///A test for ParseWebsiteIdFromMSI
        ///</summary>
        [TestMethod()]
        public void ParseWebsiteIdFromMSITest()
        {
            ConfigurationHelper target = new ConfigurationHelper(); 

            string msiWebsitePath = "/LM/W3SVC/1"; 
            string expected = "1";
            string actual;
            actual = target.ParseWebsiteIdFromMSI(msiWebsitePath);
            Assert.AreEqual(expected, actual);
        }
    }
}

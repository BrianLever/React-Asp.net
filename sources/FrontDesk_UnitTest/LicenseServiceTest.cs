using FrontDesk.Server.Licensing.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace FrontDesk_UnitTest
{
    
    
    /// <summary>
    ///This is a test class for LicenseServiceTest and is intended
    ///to contain all LicenseServiceTest Unit Tests
    ///</summary>
    
    [TestClass()]
    public class LicenseServiceTest
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
        /// <summary>
        ///A test for HasLicenseKey
        ///</summary>
        [TestMethod()]
        [Ignore]
        [TestCategory("E2E")]
        public void HasLicenseKeyTest()
        {
            LicenseService target = LicenseService.Current; 
            bool expected = true; 
            bool actual;
            actual = target.HasLicenseKey();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for RegisterProductLicense
        ///</summary>
        [Ignore]
        [TestMethod()]
        public void RegisterProductLicenseValidTest()
        {
            LicenseService target = LicenseService.Current;
            string licenseKey = "6AVUW-TGLP1-L3CTE";
            LicenseService.RegisterProductLicenseResult expected = LicenseService.RegisterProductLicenseResult.RegisteredNewLicenseKey;
            LicenseService.RegisterProductLicenseResult actual;
            actual = target.RegisterProductLicense(licenseKey);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for RegisterProductLicense
        ///</summary>
        
        [TestMethod()]
        public void RegisterProductLicenseInvalidTest()
        {
            LicenseService target = LicenseService.Current;
            string licenseKey = "6AVUW-TGLP1-13C2E";
            LicenseService.RegisterProductLicenseResult expected = LicenseService.RegisterProductLicenseResult.InvalidLicenseKey;
            LicenseService.RegisterProductLicenseResult actual;
            actual = target.RegisterProductLicense(licenseKey);
            Assert.AreEqual(expected, actual);
        }
      
        [Ignore]
        [TestMethod()]
        public void RegisterProductLicenseDuplicateTest()
        {
            LicenseService target = LicenseService.Current;
            string licenseKey = "6AVUWTGLP1L3CTE";
            LicenseService.RegisterProductLicenseResult expected = LicenseService.RegisterProductLicenseResult.DuplicateLicenseKey;
            LicenseService.RegisterProductLicenseResult actual;
            actual = target.RegisterProductLicense(licenseKey);
            Assert.AreEqual(expected, actual);
        }
    }
}

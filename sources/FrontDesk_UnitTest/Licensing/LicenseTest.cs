using FrontDesk.Server.Licensing;

using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace FrontDesk_UnitTest
{


    [DeploymentItem("configs/screendox.Secrets.config", "configs")]
    [TestClass()]
    public class LicenseTest
    {

        /// <summary>
        ///A test for CreateLicenseKeyString
        ///</summary>
        [TestMethod()]
        public void CreateLicenseKeyStringTest()
        {
            License license = new License()
            {
                SerialNumber = 0x11111,
                Years = 0x2,
                MaxKiosks = 0x333,
                MaxBranchLocations = 0x444
            };
            string expected = "LYNBX-MR35K-AWX";  // 2 last letters are actually random
            string actual;
            actual = License.CreateLicenseKeyString(license);
            Assert.AreEqual(expected.Substring(0, 15), actual.Substring(0, 15));
        }


    }
}

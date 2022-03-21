using System;

using FrontDesk.Server.Licensing;
using FrontDesk.Server.Licensing.Management;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrontDesk_UnitTest
{
    [TestClass]
    [DeploymentItem("configs/screendox.Secrets.config", "configs")]
    public class ActivationTest
    {

        /// <summary>
        ///     A test for CreateActivationRequestString
        /// </summary>
        [TestMethod]
        public void CreateActivationRequestStringTest()
        {
            var license = new License
            {
                SerialNumber = 0x11111,
                Years = 0x2,
                MaxKiosks = 0x333,
                MaxBranchLocations = 0x444
            };

            string fullWindowsProductId = "89576-355-1727301-71760";
            var target = new Activation(license, fullWindowsProductId);

            string expected = "GBPY5-K9BBY-UMT2T-HL29T-2XFYM"; // last 4 letters are random
            string actual;
            //actual = target.ToActivationRequestString();
            actual = Activation.CreateActivationRequest(target);

            Assert.AreEqual(expected.Substring(0, 29), actual.Substring(0, 29));
        }

        /// <summary>
        ///     A test for Activation Constructor
        /// </summary>
        [TestMethod]
        public void ActivationConstructorTest1()
        {
            var license = new License
            {
                SerialNumber = 0x11111,
                Years = 0x2,
                MaxKiosks = 0x333,
                MaxBranchLocations = 0x444
            };

            string fullWindowsProductId = "89576-355-1727301-71760";
            var target = new Activation(license, fullWindowsProductId);

            Assert.AreEqual(target.WindowsProductIDPart, "172730171760");
        }
    }
}
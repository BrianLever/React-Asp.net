using FrontDesk.Deployment;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrontDesk_UnitTest
{
    /// <summary>
    ///     This is a test class for ProductVersionTest and is intended
    ///     to contain all ProductVersionTest Unit Tests
    /// </summary>
    [TestClass]
    public class ProductVersionTest
    {
        /// <summary>
        ///     Gets or sets the test context which provides
        ///     information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

       
        /// <summary>
        ///     A test for ParseVersionString
        /// </summary>
        [TestMethod]
        [DeploymentItem("FrontDesk.Deployment.dll")]
        public void ParseVersionStringTest()
        {
            ProductVersion target;
            string version = string.Empty;
            var expected = new int[4] {0, 0, 0, 0};
            int[] actual;

            actual = new ProductVersion(version).Version;
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < actual.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }

            version = "10";
            expected = new int[4] {10, 0, 0, 0};
            actual = new ProductVersion(version).Version;
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < actual.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
            version = "10.123";
            expected = new int[4] {10, 123, 0, 0};
            actual = new ProductVersion(version).Version;
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < actual.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }

            version = "10.14.0";
            expected = new int[4] {10, 14, 0, 0};
            actual = new ProductVersion(version).Version;
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < actual.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }

            version = "10.14.50";
            expected = new int[4] {10, 14, 50, 0};
            actual = new ProductVersion(version).Version;
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < actual.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }

            version = "10.14.50.4365";
            expected = new int[4] {10, 14, 50, 4365};
            actual = new ProductVersion(version).Version;
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < actual.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        /// <summary>
        ///     A test for CompareTo
        /// </summary>
        [TestMethod]
        public void CompareToTest()
        {
            var target = new ProductVersion();
            var other = new ProductVersion();
            int expected = 0;
            int actual;

            target.VersionAsString = "1.12.0.2345";
            other.VersionAsString = "1.12.0.2345";

            actual = target.CompareTo(other);
            Assert.AreEqual(expected, actual); //equal

            expected = 0;
            target.VersionAsString = "11.12";
            other.VersionAsString = "11.12.0.0";

            actual = target.CompareTo(other);
            Assert.AreEqual(expected, actual); //equal

            expected = 1;
            target.VersionAsString = "10.1";
            other.VersionAsString = "9.2.0";

            actual = target.CompareTo(other);
            Assert.AreEqual(expected, actual); //greater than

            expected = -1;
            target.VersionAsString = "9.0";
            other.VersionAsString = "12.0";

            actual = target.CompareTo(other);
            Assert.AreEqual(expected, actual); //less than

            expected = -1;
            target.VersionAsString = "12.0";
            other.VersionAsString = "12.1";

            actual = target.CompareTo(other);
            Assert.AreEqual(expected, actual); //less than
        }


        /// <summary>
        ///     A test for CompareTo
        /// </summary>
        [TestMethod]
        public void CompareTo2Test()
        {
            var target = new ProductVersion();
            var other = new ProductVersion();

            target.VersionAsString = "1.12.0.2345";
            other.VersionAsString = "1.12.0.2345";
            Assert.IsTrue(target == other); //equal

            target.VersionAsString = "10.1";
            other.VersionAsString = "9.2.0";
            Assert.IsTrue(target > other); //greater than

            target.VersionAsString = "9.0";
            other.VersionAsString = "12.0";
            Assert.IsTrue(target < other); //less than

            target.VersionAsString = "12.0";
            other.VersionAsString = "12.1";
            Assert.IsTrue(target < other); //less than
        }
    }
}
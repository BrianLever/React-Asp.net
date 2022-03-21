using FrontDesk.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Collections.Generic;

namespace FrontDesk_UnitTest
{
    
    
    /// <summary>
    ///This is a test class for StringExtensionsTest and is intended
    ///to contain all StringExtensionsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class StringExtensionsTest
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

        #region Phone extensions

        [TestMethod()]
        public void Can_parse_rpms_phone()
        {
            string phone = "760 742-3257"; 
            string expected = "7607423257";
            string actual;
            actual = StringExtensions.AsRawPhoneNumber(phone);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void Can_parse_frontdesk_phone()
        {
            string phone = "760-742-3257";
            string expected = "7607423257";
            string actual;
            actual = StringExtensions.AsRawPhoneNumber(phone);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void Can_parse_whitespaced_phone()
        {
            string phone = "760 742 3257";
            string expected = "7607423257";
            string actual;
            actual = StringExtensions.AsRawPhoneNumber(phone);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void Can_parse_digits_phone()
        {
            string phone = "7607423257";
            string expected = "7607423257";
            string actual;
            actual = StringExtensions.AsRawPhoneNumber(phone);
            Assert.AreEqual(expected, actual);
        }

        #endregion

        [TestMethod()]
        public void Can_split_pobox_at_start()
        {
            string address = "P.O. BOX 82 100100 TERMINATOR DR 100";
            int[] stringSegmentLengths = new int[] { 35, 30, 30 };
            string[] expected = new string[] { "P.O. BOX 82", "100100 TERMINATOR DR 100" };
            string[] actual;
            actual = StringExtensions.SplitTo3AddressLines(address, stringSegmentLengths).ToArray();

            Assert.AreEqual(expected.Length, actual.Length, "Lengths are not the same");

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i], "segment " + i + " failed");
            }
        }

        [TestMethod()]
        public void Can_split_pobox_at_the_end()
        {
            string address = "64777 ELM ST.  PO BOX 110";
            int[] stringSegmentLengths = new int[] { 35, 30, 30 };
            string[] expected = new string[] { "64777 ELM ST.", "PO BOX 110" };
            string[] actual;
            actual = StringExtensions.SplitTo3AddressLines(address, stringSegmentLengths).ToArray();

            Assert.AreEqual(expected.Length, actual.Length, "Lengths are not the same");

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i], "segment " + i + " failed");
            }
        }

        [TestMethod()]
        public void Can_split_single_line_short()
        {
            string address = "245 WEST END STREET";
            int[] stringSegmentLengths = new int[] { 35, 30, 30 };
            string[] expected = new string[] { "245 WEST END STREET"};
            string[] actual;
            actual = StringExtensions.SplitTo3AddressLines(address, stringSegmentLengths).ToArray();

            Assert.AreEqual(expected.Length, actual.Length, "Lengths are not the same");

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i], "segment " + i + " failed");
            }
        }

        [TestMethod()]
        public void Can_split_single_line_long_2()
        {
            string address = "358008 PALA TEMECULA RD. PMB#450";
            int[] stringSegmentLengths = new int[] { 15, 10, 10 };
            string[] expected = new string[] { "358008 PALA", "TEMECULA", "RD. PMB#450" };
            string[] actual;
            actual = StringExtensions.SplitTo3AddressLines(address, stringSegmentLengths).ToArray();

            Assert.AreEqual(expected.Length, actual.Length, "Lengths are not the same");

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i], "segment " + i + " failed");
            }
        }
    }
}

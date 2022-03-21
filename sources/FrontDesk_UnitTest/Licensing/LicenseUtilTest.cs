using FrontDesk.Licensing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FrontDesk.Common;
namespace FrontDesk_UnitTest
{
    
    
    /// <summary>
    ///This is a test class for LicenseUtilTest and is intended
    ///to contain all LicenseUtilTest Unit Tests
    ///</summary>
    [TestClass()]
    public class LicenseUtilTest
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
        ///A test for UnpackStringInt32
        ///</summary>
        [TestMethod()]
        public void UnpackStringInt32Test()
        {
            string licenseString = "QLZ9-PAW";
            int expected = 0x11223344;
            int actual;

            actual = TextFormatHelper.UnpackStringInt32(licenseString);
            Assert.AreEqual(expected, actual);            
        }

        [TestMethod()]
        public void UnpackStringInt32Test1()
        {
            string licenseString = "QLZ9-PAWR-ANDOM";
            int expected = 0x11223344;
            int actual;
            actual = TextFormatHelper.UnpackStringInt32(licenseString);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for UnpackStringInt16
        ///</summary>
        [TestMethod()]
        public void UnpackStringInt16Test()
        {
            string licenseString = "QLZ0";
            short expected = 0x1122;
            short actual;
            actual = TextFormatHelper.UnpackStringInt16(licenseString);
            Assert.AreEqual(expected, actual);            
        }

        [TestMethod()]
        public void UnpackStringInt16Test1()
        {
            string licenseString = "QLZ0R-ANDOM";
            short expected = 0x1122;
            short actual;
            actual = TextFormatHelper.UnpackStringInt16(licenseString);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for PackString
        ///</summary>
        [TestMethod()]
        public void PackStringTest1()
        {
            short int16 = 0x1122;
            string expected = "QLZR"; 
            string actual;
            actual = TextFormatHelper.PackString(int16);
            Assert.AreEqual(expected, actual);            
        }

        /// <summary>
        ///A test for PackString
        ///</summary>
        [TestMethod()]
        public void PackStringTest()
        {
            int int32 = 0x11223344;
            string expected = "QLZ9PAW";
            string actual;
            actual = TextFormatHelper.PackString(int32);
            Assert.AreEqual(expected, actual);            
        }


        /// <summary>
        ///A test for FormatWithGroups
        ///</summary>
        [TestMethod()]
        public void FormatWithGroupsTest()
        {
            string source = "1111222233";
            int lettersInGroup = 4; 
            bool padWithRandomLetters = false; 
            bool mergeLastIncompleteGroup = false; 
            string expected = "1111-2222-33";
            string actual;
            actual = TextFormatHelper.FormatWithGroups(source, lettersInGroup, padWithRandomLetters, mergeLastIncompleteGroup);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void FormatWithGroupsTest1()
        {
            string source = "1111222233";
            int lettersInGroup = 4;
            bool padWithRandomLetters = false;
            bool mergeLastIncompleteGroup = true;
            string expected = "1111-222233";
            string actual;
            actual = TextFormatHelper.FormatWithGroups(source, lettersInGroup, padWithRandomLetters, mergeLastIncompleteGroup);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void FormatWithGroupsTest2()
        {
            string source = "11112222";
            int lettersInGroup = 4;
            bool padWithRandomLetters = false;
            bool mergeLastIncompleteGroup = false;
            string expected = "1111-2222";
            string actual;
            actual = TextFormatHelper.FormatWithGroups(source, lettersInGroup, padWithRandomLetters, mergeLastIncompleteGroup);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void FormatWithGroupsTest3()
        {
            string source = "1111";
            int lettersInGroup = 4;
            bool padWithRandomLetters = false;
            bool mergeLastIncompleteGroup = false;
            string expected = "1111";
            string actual;
            actual = TextFormatHelper.FormatWithGroups(source, lettersInGroup, padWithRandomLetters, mergeLastIncompleteGroup);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void FormatWithGroupsTest4()
        {
            string source = "1111222233";
            int lettersInGroup = 4;
            bool padWithRandomLetters = true;
            bool mergeLastIncompleteGroup = false;
            //string expected = "1111-2222-33xx";
            string actual;
            actual = TextFormatHelper.FormatWithGroups(source, lettersInGroup, padWithRandomLetters, mergeLastIncompleteGroup);
            Assert.AreEqual(14, actual.Length);
        }

    }
}

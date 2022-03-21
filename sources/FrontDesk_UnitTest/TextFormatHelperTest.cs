using FrontDesk.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace FrontDesk_UnitTest
{
    
    
    /// <summary>
    ///This is a test class for TextFormatHelperTest and is intended
    ///to contain all TextFormatHelperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TextFormatHelperTest
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
        ///A test for FormatWithGroups
        ///</summary>
        [TestMethod()]
        public void FormatWithGroupsTest()
        {
            string source = string.Empty; 
            int lettersInGroup = 5; 
            bool padWithRandomLetters = true; 
            bool mergeLastIncompleteGroup = false; 
            string expected = string.Empty;
            string actual;

            source = "6AVUW-TGLP1-L3CTE";
            expected = "6AVUW-TGLP1-L3CTE";
            actual = TextFormatHelper.FormatWithGroups(source, lettersInGroup, padWithRandomLetters, mergeLastIncompleteGroup);
            Assert.AreEqual(expected, actual);

            source = "6AVUWTGLP1L3CTE";
            expected = "6AVUW-TGLP1-L3CTE";
            actual = TextFormatHelper.FormatWithGroups(source, lettersInGroup, padWithRandomLetters, mergeLastIncompleteGroup);
            Assert.AreEqual(expected, actual);


            source = "6AV-UWTGL-P1L3C-TE";
            expected = "6AVUW-TGLP1-L3CTE";
            actual = TextFormatHelper.FormatWithGroups(source, lettersInGroup, padWithRandomLetters, mergeLastIncompleteGroup);
            Assert.AreEqual(expected, actual);

        }
    }
}

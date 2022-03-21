using FrontDesk;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FrontDesk_UnitTest
{
    
    
    /// <summary>
    ///This is a test class for ScreeningResultTest and is intended
    ///to contain all ScreeningResultTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ScreeningResult_IsUnderFifteen
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
    ///A test for IsChildren
    ///</summary>
    [TestMethod()]
    public void IsChildrenTest()
    {
        //today date constanta 05/04/2010 13:40:05
        DateTime todayDate = new DateTime(2010, 5, 4, 13, 40, 5);
        DateTime birthday;
        int expected;
        int actual;

        ScreeningResult target = new ScreeningResult(); // TODO: Initialize to an appropriate value

        #region calculate datetime
        // test age = 15
        //birthday today
        birthday = new DateTime(todayDate.Year - 15, todayDate.Month, todayDate.Day);
        expected = 15;
        actual = ScreeningResult.GetAge(birthday, todayDate);
        Assert.AreEqual(expected, actual);

        //test age > 15
        //birthday today
        birthday = new DateTime(todayDate.Year - 16, todayDate.Month, todayDate.Day);
        expected = 16;
        actual = ScreeningResult.GetAge(birthday, todayDate);
        Assert.AreEqual(expected, actual);

        //test age < 15
        //birthday today
        birthday = new DateTime(todayDate.Year - 14, todayDate.Month, todayDate.Day);
        expected = 14;
        actual = ScreeningResult.GetAge(birthday, todayDate);
        Assert.AreEqual(expected, actual);

        //birthday will be tomorrow
        birthday = new DateTime(todayDate.Year - 15, todayDate.Month, todayDate.Day + 1);
        expected = 14;
        actual = ScreeningResult.GetAge(birthday, todayDate);
        Assert.AreEqual(expected, actual);

        //birthday was yesterday
        birthday = new DateTime(todayDate.Year - 15, todayDate.Month, todayDate.Day - 1);
        expected = 15;
        actual = ScreeningResult.GetAge(birthday, todayDate);
        Assert.AreEqual(expected, actual);

        #endregion

        #region constant datetime
        //bithday now
        birthday = new DateTime(2010, 5, 4, 13, 40, 5);

        expected = 0;
        actual = ScreeningResult.GetAge(birthday, todayDate);
        Assert.AreEqual(expected, actual);

        //age < 15
        //bithday will be tomorrow
        birthday = new DateTime(1995, 5, 5, 13, 40, 5);
        expected = 14;
        actual = ScreeningResult.GetAge(birthday, todayDate);
        Assert.AreEqual(expected, actual);

        //age == 15
        //bithday today
        birthday = new DateTime(1995, 5, 4, 13, 40, 5);
        expected = 15;
        actual = ScreeningResult.GetAge(birthday, todayDate);
        Assert.AreEqual(expected, actual);
        
        //age == 20
        //bithday today
        //today hours and bithday hours are different
        birthday = new DateTime(1990, 5, 4, 12, 40, 5);
        expected = 20;
        actual = ScreeningResult.GetAge(birthday, todayDate);
        Assert.AreEqual(expected, actual);

        //age == 15
        //bithday today
        //today minutes and bithday minutes are different
        birthday = new DateTime(1995, 5, 4, 13, 0, 0);
        expected = 15;
        actual = ScreeningResult.GetAge(birthday, todayDate);
        Assert.AreEqual(expected, actual);

        //age > 15
        birthday = new DateTime(1994, 5, 4, 13, 40, 5);
        expected = 16;
        actual = ScreeningResult.GetAge(birthday, todayDate);
        Assert.AreEqual(expected, actual);
        


        #endregion
    }
   }
}

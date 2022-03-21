using FrontDesk.Server.Screening;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FrontDesk;
using System;
using System.Collections.Generic;

namespace FrontDesk_UnitTest
{


    /// <summary>
    ///This is a test class for ScreeningResultHelperTest and is intended
    ///to contain all ScreeningResultHelperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ScreeningResultHelperTest
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
        /// Answer on all answers. Answer choises are organized as stages
        /// </summary>
        /// <returns></returns>
        protected ScreeningResult CreateStagingScreeningResult()
        {
            ScreeningResult screeningResult = new ScreeningResult();
            screeningResult.ScreeningID = "BHS";
            screeningResult.LastName = "Doe";
            screeningResult.FirstName = "John";
            screeningResult.MiddleName = "Michael";
            screeningResult.Birthday = new DateTime(1981, 9, 22);
            screeningResult.StreetAddress = "1015 Hill Street";
            screeningResult.City = "San Diego";
            screeningResult.StateID = "CA";
            screeningResult.ZipCode = "92050";
            screeningResult.Phone = "(619) 345-5678";

            var section = new ScreeningSectionResult("SIH", 1);
            section.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 1)); 
            screeningResult.AppendSectionAnswer(section);

            section = new ScreeningSectionResult("TCC", 1);
            section.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 1)); //Do you use tobacco?

            screeningResult.AppendSectionAnswer(section);
            section.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 1));

            section = new ScreeningSectionResult("CAGE", 1);
            section.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 1)); //Do you drink alcohol?
            screeningResult.AppendSectionAnswer(section);

            for (int i = 1; i <= 4; i++)
            {
                section.AppendQuestionAnswer(new ScreeningSectionQuestionResult(i, 1 % 2));
            }

            section = new ScreeningSectionResult("PHQ-9", 1);
            screeningResult.AppendSectionAnswer(section);

            for (int i = 1; i <= 10; i++)
            {
                section.AppendQuestionAnswer(new ScreeningSectionQuestionResult(i, 1 % 5));
            }

            section = new ScreeningSectionResult("HITS", 1);
            section.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 1)); //Did a partner, family member, or caregiver\n hurt, insult, threaten, or scream at you?
            screeningResult.AppendSectionAnswer(section);

            for (int i = 1; i <= 4; i++)
            {
                section.AppendQuestionAnswer(new ScreeningSectionQuestionResult(i, 1 % 6));
            }

            return screeningResult;
        }
        /// <summary>
        /// Answer 'No' for all section
        /// </summary>
        /// <returns></returns>
        protected ScreeningResult CreateEmptyScreeningResult()
        {
            ScreeningResult screeningResult = new ScreeningResult();
            screeningResult.ScreeningID = "BHS";
            screeningResult.LastName = "Doe";
            screeningResult.FirstName = "John";
            screeningResult.MiddleName = "Michael";
            screeningResult.Birthday = new DateTime(1981, 9, 22);
            screeningResult.StreetAddress = "1015 Hill Street";
            screeningResult.City = "San Diego";
            screeningResult.StateID = "CA";
            screeningResult.ZipCode = "92050";
            screeningResult.Phone = "(619) 345-5678";

            var section = new ScreeningSectionResult("SIH", 0);
            section.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 0)); //Do you use tobacco?
            screeningResult.AppendSectionAnswer(section);

            section = new ScreeningSectionResult("TCC", 0);
            section.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 0)); //Do you use tobacco?
            screeningResult.AppendSectionAnswer(section);

            section = new ScreeningSectionResult("CAGE", 0);
            section.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 0)); //Do you drink alcohol?
            screeningResult.AppendSectionAnswer(section);


            section = new ScreeningSectionResult("PHQ-9", 0);
            section.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 0)); //Little interest or pleasure in doing things
            section.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 0)); //Feeling down, depressed, or hopeless
            screeningResult.AppendSectionAnswer(section);


            section = new ScreeningSectionResult("HITS", 0);
            section.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 0)); //Did a partner, family member, or caregiver\n hurt, insult, threaten, or scream at you?
            screeningResult.AppendSectionAnswer(section);


            return screeningResult;
        }

        /// <summary>
        /// Create screening result without answers
        /// questions will be skipped 
        /// </summary>
        /// <returns></returns>
        protected ScreeningResult CreateChildrenScreeningResult()
        {
            ScreeningResult screeningResult = new ScreeningResult();
            screeningResult.ScreeningID = "BHS";
            screeningResult.LastName = "Doe";
            screeningResult.FirstName = "John";
            screeningResult.MiddleName = "Michael";
            DateTime today = DateTime.Today;
            screeningResult.Birthday = new DateTime(today.Year - 15, today.Month, (today.Day + 2)/30);
            screeningResult.StreetAddress = "1015 Hill Street";
            screeningResult.City = "San Diego";
            screeningResult.StateID = "CA";
            screeningResult.ZipCode = "92050";
            screeningResult.Phone = "(619) 345-5678";

            return screeningResult;

        }

        /// <summary>
        ///A test for InsertScreeningResult
        ///</summary>
        [TestMethod()]
        [TestCategory("E2E")]
        public void InsertScreeningResultTest()
        {
            ScreeningResult screeningResult = CreateStagingScreeningResult();


            long expected = 0; // TODO: Initialize to an appropriate value
            long actual;
            screeningResult.KioskID = 1000;
            actual = ScreeningResultHelper.InsertScreeningResult(screeningResult);
            Assert.AreNotEqual(expected, actual);

        }

        /// <summary>
        ///A test for InsertScreeningResult with all Negative answers
        ///</summary>
        [TestMethod()]
        [TestCategory("E2E")]
        public void InsertScreeningResultNegativeTest()
        {
            ScreeningResult screeningResult = CreateEmptyScreeningResult();


            long expected = 0; // TODO: Initialize to an appropriate value
            long actual;
            screeningResult.KioskID = 1000;
            actual = ScreeningResultHelper.InsertScreeningResult(screeningResult);
            Assert.AreNotEqual(expected, actual);

        }

        /// <summary>
        ///A test for PerformUpdateScreeningResultScores
        ///</summary>
        [TestMethod()]
        public void PerformUpdateScreeningResultScoresTest()
        {
            string screeningID = "BHS";
            ScreeningResultHelper.PerformUpdateScreeningResultScores(screeningID);
        }

        
        [TestCategory("E2E")]
        [Ignore]
        [TestMethod]
        public void InsertChildrenScreeningResult()
        {
            ScreeningResult screeningResult = CreateChildrenScreeningResult();
            long expected = 0; // TODO: Initialize to an appropriate value
            long actual;
            actual = ScreeningResultHelper.InsertScreeningResult(screeningResult);
            Assert.AreNotEqual(expected, actual);
            ScreeningResult result = ScreeningResultHelper.GetScreeningResult(actual);
            Assert.AreNotEqual(result, null);
        }
    }
}

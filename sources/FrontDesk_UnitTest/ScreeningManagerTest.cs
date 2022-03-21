using FrontDesk.Kiosk.Workflow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FrontDesk;

namespace FrontDesk_UnitTest
{
    
    
    /// <summary>
    ///This is a test class for ScreeningManagerTest and is intended
    ///to contain all ScreeningManagerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ScreeningManagerTest
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


        protected ScreeningSection GetTobacoSectionHeader()
        {
            return new ScreeningSection()
            {
                ScreeningID = "BHS",
                ScreeningSectionID = "TCC",
                ScreeningSectionName = "Tobacco Cessation Counseling",
                QuestionText = "Do you use tobacco?"

            };
        
        }


        /// <summary>
        ///A test for GetFirstSection
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FrontDeskKiosk.exe")]
        [DeploymentItem("Data/FrontDeskKiosk.sdf", "Data")]
        public void GetFirstSectionTest()
        {
            ScreeningManager_Accessor target = new ScreeningManager_Accessor(); // TODO: Initialize to an appropriate value
            ScreeningSection expected = GetTobacoSectionHeader();

            ScreeningSection actual;
            actual = target.GetFirstSection();
            Assert.AreEqual(expected.ScreeningID, actual.ScreeningID);
            Assert.AreEqual(expected.ScreeningSectionID, actual.ScreeningSectionID);
            Assert.AreEqual(expected.ScreeningSectionName, actual.ScreeningSectionName);
            Assert.AreEqual(expected.QuestionText, actual.QuestionText);

        }

        /// <summary>
        ///A test for GetFirstSectionQuestion
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FrontDeskKiosk.exe")]
        [DeploymentItem("Data/FrontDeskKiosk.sdf", "Data" )]
        public void GetFirstSectionQuestionTest()
        {
            ScreeningManager_Accessor target = new ScreeningManager_Accessor(); // TODO: Initialize to an appropriate value

            ScreeningSectionQuestion expected = new ScreeningSectionQuestion()
                {
                    QuestionID = 1,
                    AnswerScaleID = 1,
                    QuestionText = "Are you interested in receiving tobacco cessation counseling?",
                    ScreeningSectionID = "TCC",
                    PreambleText = string.Empty
                };
            ScreeningSectionQuestion actual;
            actual = target.GetFirstSectionQuestion("TCC");
            Assert.AreEqual(expected.QuestionID, actual.QuestionID);
            Assert.AreEqual(expected.QuestionText, actual.QuestionText);
            Assert.AreEqual(expected.PreambleText, actual.PreambleText);
            Assert.AreEqual(expected.AnswerScaleID, actual.AnswerScaleID);
            Assert.AreEqual(expected.ScreeningSectionID, actual.ScreeningSectionID);

           
        }

        /// <summary>
        ///A test for GetNextSection
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FrontDeskKiosk.exe")]
        [DeploymentItem("Data/FrontDeskKiosk.sdf", "Data")]
        public void GetNextSectionTest()
        {
            ScreeningManager_Accessor target = new ScreeningManager_Accessor(); // TODO: Initialize to an appropriate value
            string currentSectionID = "TCC";
            ScreeningSection expected = new ScreeningSection()
            {
                ScreeningSectionID="CAGE",
                ScreeningSectionName = "CAGE (Alcohol) Screening Tool"
            }; 

            ScreeningSection actual;
            actual = target.GetNextSection(currentSectionID);
           
            Assert.AreEqual(expected.ScreeningSectionID, actual.ScreeningSectionID);
            Assert.AreEqual(expected.ScreeningSectionName, actual.ScreeningSectionName);
        }

        /// <summary>
        ///A test for GetNextSectionQuestion
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FrontDeskKiosk.exe")]
        [DeploymentItem("Data/FrontDeskKiosk.sdf", "Data")]
        public void GetNextSectionQuestionTest()
        {
            ScreeningManager_Accessor target = new ScreeningManager_Accessor(); // TODO: Initialize to an appropriate value
            string currentSectionID = "CAGE"; 
            int currentSectionQuestionID = 1; //first question

            ScreeningSectionQuestion expected = new ScreeningSectionQuestion()
            {
                QuestionID = 2,
                QuestionText = "Have people annoyed you by criticizing your drinking?",
                ScreeningSectionID = "CAGE",
                PreambleText = string.Empty,
                AnswerScaleID = 1
            }; // TODO: Initialize to an appropriate value
            ScreeningSectionQuestion actual;
            actual = target.GetNextSectionQuestion(currentSectionID, currentSectionQuestionID);


            Assert.AreEqual(expected.QuestionID, actual.QuestionID);
            Assert.AreEqual(expected.QuestionText, actual.QuestionText);
            Assert.AreEqual(expected.PreambleText, actual.PreambleText);
            Assert.AreEqual(expected.AnswerScaleID, actual.AnswerScaleID);
            Assert.AreEqual(expected.ScreeningSectionID, actual.ScreeningSectionID);
        }


        ///</summary>
        [TestMethod()]
        [DeploymentItem("FrontDeskKiosk.exe")]
        [DeploymentItem("Data/FrontDeskKiosk.sdf", "Data")]
        public void GetNextSectionQuestionLastTest()
        {
            ScreeningManager_Accessor target = new ScreeningManager_Accessor(); // TODO: Initialize to an appropriate value
            string currentSectionID = "CAGE";
            int currentSectionQuestionID = 4; //first question

            ScreeningSectionQuestion actual;
            actual = target.GetNextSectionQuestion(currentSectionID, currentSectionQuestionID);


            Assert.IsNull(actual);
        }

        /// <summary>
        ///A test for GoToNextQuestion
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Data/FrontDeskKiosk.sdf", "Data")]
        public void GoToNextQuestionTest()
        {
            ScreeningManager_Accessor target = new ScreeningManager_Accessor(); // TODO: Initialize to an appropriate value
            ScreeningSectionQuestion expected = null; 

            ScreeningSectionQuestion actual;
            target.GoToNextSection();

            actual = target.GoToNextQuestion();
            Assert.AreEqual(actual.ScreeningSectionID, "TCC");
            Assert.AreEqual(actual.QuestionID, 1);
            Assert.AreEqual(actual.QuestionText, "Are you interested in receiving tobacco cessation counseling?");

            actual = target.GoToNextQuestion();
            Assert.IsNull(actual);
        }

        /// <summary>
        ///A test for GoToNextSection
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Data/FrontDeskKiosk.sdf", "Data")]
        public void GoToNextSectionTest()
        {
            ScreeningManager_Accessor target = new ScreeningManager_Accessor(); // TODO: Initialize to an appropriate value
          
            ScreeningSection actual;
            actual = target.GoToNextSection();
            Assert.AreEqual(actual.ScreeningSectionID, "TCC");
            actual = target.GoToNextSection();
            Assert.AreEqual(actual.ScreeningSectionID, "CAGE");
            actual = target.GoToNextSection();
            Assert.AreEqual(actual.ScreeningSectionID, "PHQ-9");
            actual = target.GoToNextSection();
            Assert.AreEqual(actual.ScreeningSectionID, "HITS");

            actual = target.GoToNextSection();
            Assert.IsNull(actual);

        }


        /// <summary>
        ///A test for GoToNextSection
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Data/FrontDeskKiosk.sdf", "Data")]
        public void BHSScreeningWorkflowTest()
        {
            ScreeningManager_Accessor target = new ScreeningManager_Accessor(); // TODO: Initialize to an appropriate value

            ScreeningSection actual;
            ScreeningSectionQuestion question = null;

            for (int loop = 0; loop < 3; loop++) //do it 2 times
            {
                actual = target.GoToNextSection();
                Assert.AreEqual(actual.ScreeningSectionID, "TCC");

                question = target.GoToNextQuestion();
                Assert.AreEqual(question.QuestionID, 1);

                actual = target.GoToNextSection();
                Assert.AreEqual(actual.ScreeningSectionID, "CAGE");

                for (int i = 1; i <= 4; i++)
                {
                    question = target.GoToNextQuestion();
                    Assert.AreEqual(question.QuestionID, i);
                }
                actual = target.GoToNextSection();
                Assert.AreEqual(actual.ScreeningSectionID, "PHQ-9");

                for (int i = 1; i <= 10; i++)
                {
                    question = target.GoToNextQuestion();
                    Assert.AreEqual(question.QuestionID, i);

                    if (i < 10)
                    {
                        Assert.IsFalse(string.IsNullOrEmpty(question.PreambleText));
                    }
                    else
                    {
                        Assert.IsTrue(string.IsNullOrEmpty(question.PreambleText));

                    }
                }
                actual = target.GoToNextSection();
                Assert.AreEqual(actual.ScreeningSectionID, "HITS");

                for (int i = 1; i <= 4; i++)
                {
                    question = target.GoToNextQuestion();
                    Assert.AreEqual(question.QuestionID, i);
                    Assert.IsFalse(string.IsNullOrEmpty(question.PreambleText));
                }

                actual = target.GoToNextSection();
                Assert.IsNull(actual);


                target.RestartScreening();
            }

        }
    }
}

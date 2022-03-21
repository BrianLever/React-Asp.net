using System.Linq;
using FluentAssertions;
using FrontDesk;
using FrontDesk.Kiosk.Workflow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FrontdDeskKiosk.Tests.ScreenerControllerTests
{

    [TestClass]
    public class WorkflowSequenceTest : ScreeningConrollerContextBase<ScreeningController>
    {
        protected override void construct()
        {
            sut = new ScreeningController(_screeningFrequencySpecificationMock.Object, _resultStateMock.Object,
                _screeningSectionAgeServiceMock.Object);
        }


        protected override void given()
        {
            base.given();

            _screeningFrequencySpecificationMock.Setup(x => x.IsSkipRequiredForSection(It.IsAny<string>()))
                .Returns(false);

            sut.CurrentState.Reset();
        }



        /// <summary>
        ///A test for GetFirstSection
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FrontDeskKiosk.exe")]
        [DeploymentItem("Data/FrontDeskKiosk.sdf", "Data")]
        public void GetFirstSectionTest()
        {
            var actual = sut.GetFirstSection();
            actual.Should().BeSameAs(_screeningMetaData.FindSectionByID(ScreeningSectionDescriptor.SmokerInHome));

        }

        /// <summary>
        ///A test for GetFirstSectionQuestion
        ///</summary>
        [TestMethod()]
        public void GetFirstSectionQuestionTest()
        {
            var actual = sut.GetFirstSectionQuestion(ScreeningSectionDescriptor.Tobacco);

            actual.Should()
                .BeSameAs(_screeningMetaData.FindSectionByID(ScreeningSectionDescriptor.Tobacco).Questions.First());
        }

        [TestMethod()]
        public void GetNextSectionTest()
        {

            var actual = sut.GetNextSection(ScreeningSectionDescriptor.Tobacco);
            actual.Should().BeSameAs(_screeningMetaData.FindSectionByID(ScreeningSectionDescriptor.Alcohol));

        }

       
        [TestMethod()]
        public void GetNextSectionQuestionTest()
        {

            const string currentSectionId = ScreeningSectionDescriptor.Alcohol;
            const int currentSectionQuestionId = 1; //first question

            var actual = sut.GetNextSectionQuestion(currentSectionId, currentSectionQuestionId);
            actual.Should()
                .BeSameAs(_screeningMetaData.FindSectionByID(ScreeningSectionDescriptor.Alcohol).NotMainSectionQuestions[1]); //2nd

        }


        [TestMethod()]
        public void GetNextSectionQuestionLastTest()
        {
            const string currentSectionId = ScreeningSectionDescriptor.Alcohol;
            var currentSectionQuestionId =
                _screeningMetaData.FindSectionByID(ScreeningSectionDescriptor.Alcohol).Questions.Last().QuestionID;

            sut.GetNextSectionQuestion(currentSectionId, currentSectionQuestionId).Should().BeNull();
        }


        [TestMethod()]
        public void WhenGoToNextQuestion_MainQuestionSelected()
        {
            sut.GoToNextSection();
            var actual = sut.GoToNextSection();
            actual.ScreeningSectionID.Should().Be(ScreeningSectionDescriptor.Tobacco);

            sut.CurrentState.SectionQuestion.Should().NotBeNull("Should select main question.");
            sut.CurrentState.SectionQuestion.QuestionID.Should().Be(4, "Main question should be the first");
        }

        /// <summary>
        ///A test for GoToNextQuestion
        ///</summary>
        [TestMethod()]
        public void GoToNextQuestionTest()
        {
            sut.GoToNextSection();
            sut.GoToNextSection();
            
            var actual = sut.GoToNextQuestion();
            actual.ScreeningSectionID.Should().Be(ScreeningSectionDescriptor.Tobacco);
            actual.QuestionID.Should().Be(1);

            actual = sut.GoToNextQuestion();
            actual.ScreeningSectionID.Should().Be(ScreeningSectionDescriptor.Tobacco);
            actual.QuestionID.Should().Be(2);


            actual = sut.GoToNextQuestion();
            actual.ScreeningSectionID.Should().Be(ScreeningSectionDescriptor.Tobacco);
            actual.QuestionID.Should().Be(3);

            actual = sut.GoToNextQuestion();
            actual.Should().BeNull();
        }

        /// <summary>
        ///A test for GoToNextSection
        ///</summary>
        [TestMethod()]
        public void GoToNextSectionTest()
        {

            var actual = sut.GoToNextSection();
            actual.ScreeningSectionID.Should().Be("SIH");

            actual = sut.GoToNextSection();
            actual.ScreeningSectionID.Should().Be("TCC");

            actual = sut.GoToNextSection();
            actual.ScreeningSectionID.Should().Be("CAGE");

            actual = sut.GoToNextSection();
            actual.ScreeningSectionID.Should().Be("DAST");
            actual = sut.GoToNextSection();
            actual.ScreeningSectionID.Should().Be("PHQ-9");

            actual = sut.GoToNextSection();
            actual.ScreeningSectionID.Should().Be("HITS");

            actual = sut.GoToNextSection();
            actual.Should().BeNull();

        }


        /// <summary>
        ///A test for GoToNextSection
        ///</summary>
        [TestMethod()]
        public void Bhs_ScreeningWorkflow_Can_Loop_Two_Times()
        {
            ScreeningSection actual;
            ScreeningSectionQuestion question = null;

            for (int loop = 0; loop < 3; loop++) //do it 2 times
            {
                actual = sut.GoToNextSection();
                actual.ScreeningSectionID.Should().Be("SIH");

                actual = sut.GoToNextSection();
                actual.ScreeningSectionID.Should().Be("TCC");

                question = sut.GoToNextQuestion();
                question.QuestionID.Should().Be(1);

                actual = sut.GoToNextSection();
                actual.ScreeningSectionID.Should().Be("CAGE");

                for (int i = 1; i <= 4; i++)
                {
                    question = sut.GoToNextQuestion();
                    question.QuestionID.Should().Be(i);
                }



                actual = sut.GoToNextSection();
                actual.ScreeningSectionID.Should().Be("DAST");

                for (int i = 1; i <= 2; i++)
                {
                    question = sut.GoToNextQuestion();
                    question.QuestionID.Should().Be(i, "DAST question id failed");

                    question.QuestionText.Should().NotBeNullOrEmpty("DAST QuestionText failed for question " + i);
                }


                actual = sut.GoToNextSection();
                actual.ScreeningSectionID.Should().Be("PHQ-9");

                for (int i = 2; i <= 3; i++)
                {
                    question = sut.GoToNextQuestion();
                    question.QuestionID.Should().Be(i, "PHQ-9 question id failed");

                    if (i < 3)
                    {
                        question.PreambleText.Should().NotBeEmpty();
                    }
                    else
                    {
                        question.PreambleText.Should().BeNullOrEmpty();
                    }
                }
                actual = sut.GoToNextSection();
                actual.ScreeningSectionID.Should().Be("HITS");

                for (int i = 1; i <= 2; i++)
                {
                    question = sut.GoToNextQuestion();
                    question.QuestionID.Should().Be(i, "HITS question id failed");
                    question.PreambleText.Should().NotBeEmpty();
                }

                actual = sut.GoToNextSection();
                actual.Should().BeNull();



                sut.RestartScreening();
            }
        }
    }

}

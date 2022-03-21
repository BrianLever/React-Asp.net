using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FrontDesk.Services;
using FluentAssertions;

namespace FrontDesk.Server.Tests.Services
{
    [TestClass]
    public class WhenDepression_BhsVisitFactoryTests
    {
        protected BhsVisitFactory Sut()
        {
            return new BhsVisitFactory();
        }

        [TestMethod]
        public void Should_DepressionFlag_BeOff()
        {
            var result = MotherObjects.ScreeningResultMotherObject.GetAllNoAnswers();
            var info = MotherObjects.ScreeningInfoMotherObject.GetFullScreening();
            var actual = Sut().Create(result, info);

            actual.Should().NotBeNull();
            actual.DepressionFlag.ScoreLevel.Should().Be(0);
        }

        [TestMethod]
        public void Should_DepressionFlag_BeMild()
        {
            var info = MotherObjects.ScreeningInfoMotherObject.GetFullScreening();
            var result = MotherObjects.ScreeningResultMotherObject.GetAllNoAnswers();
            var sectionResult = result.FindSectionByID(ScreeningSectionDescriptor.Depression);
            sectionResult.ScoreLevel = 2;
            sectionResult.Score = 5;
            sectionResult.ScoreLevelLabel = "MILD depression severity";

            sectionResult.AppendQuestionAnswer(new ScreeningSectionQuestionResult
            {
                QuestionID = 1,
                AnswerValue = 3
            });
            sectionResult.AppendQuestionAnswer(new ScreeningSectionQuestionResult
            {
                QuestionID = 8,
                AnswerValue = 2
            });
            sectionResult.AppendQuestionAnswer(new ScreeningSectionQuestionResult
            {
                QuestionID = 10,
                AnswerValue = 2
            });


            var actual = Sut().Create(result, info);

            actual.Should().NotBeNull();
            actual.DepressionFlag.ScoreLevel.Should().Be(5);
            actual.DepressionFlag.ScoreLevelLabel.Should().Contain("MILD");

        }

       
    }
}

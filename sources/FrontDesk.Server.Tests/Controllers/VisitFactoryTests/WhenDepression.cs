using Microsoft.VisualStudio.TestTools.UnitTesting;
using FrontDesk.Server.Tests.MotherObjects;
using FluentAssertions;

namespace FrontDesk.Server.Tests.Controllers.VisitFactoryTests
{
    [TestClass]
    public class WhenDepression: VisitFactoryTestsBase
    {
        [TestMethod]
        public void When_Depression_PositiveButNotSuicide_DepressionFlagIsOn()
        {
            var result = ScreeningResultMotherObject.GetAllNoAnswers();
            var section = result.FindSectionByID(ScreeningSectionDescriptor.Depression);

            section.ScoreLevel = 1;
            section.Score = 1;
            section.AppendQuestionAnswer(new ScreeningSectionQuestionResult(0, 3));
            
            var sut = Sut();
            var visit = sut.Create(result, _screeningInfo);

            visit.DepressionFlag.ScoreLevel.Should().BeGreaterThan(0);
        }
        [TestMethod]
        public void When_Depression_PositiveButNotSuicide_DepressionThinkOfDeathFlagIsOff()
        {
            var result = ScreeningResultMotherObject.GetAllNoAnswers();
            var section = result.FindSectionByID(ScreeningSectionDescriptor.Depression);

            section.ScoreLevel = 1;
            section.AppendQuestionAnswer(new ScreeningSectionQuestionResult(0, 1));

            var sut = Sut();
            var visit = sut.Create(result, _screeningInfo);

            visit.DepressionThinkOfDeathAnswer.Should().BeEmpty();
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void When_Depression_PositiveSuicide_DepressionFlagisOn()
        {
            var result = ScreeningResultMotherObject.GetAllNoAnswers();
            var section = result.FindSectionByID(ScreeningSectionDescriptor.Depression);

            section.ScoreLevel = 1;
            section.Score = 1;

            section.AppendQuestionAnswer(new ScreeningSectionQuestionResult(ScreeningSectionDescriptor.DepressionThinkOfDeathQuestionID, 1));

            var sut = Sut();
            var visit = sut.Create(result, _screeningInfo);

            visit.DepressionFlag.ScoreLevel.Should().BeGreaterThan(0);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void When_Depression_PositiveSuicide_DepressionThinkOfDeathFlagisOn()
        {
            var result = ScreeningResultMotherObject.GetAllNoAnswers();
            var section = result.FindSectionByID(ScreeningSectionDescriptor.Depression);

            section.ScoreLevel = 1;
            section.AppendQuestionAnswer(new ScreeningSectionQuestionResult(ScreeningSectionDescriptor.DepressionThinkOfDeathQuestionID, 1));

            var sut = Sut();
            var visit = sut.Create(result, _screeningInfo);

            visit.DepressionThinkOfDeathAnswer.Should().NotBeEmpty();
        }

        [TestMethod]
        public void When_Depression_Negative_FlagIsOff()
        {
            var result = ScreeningResultMotherObject.GetAllNoAnswers();

            var sut = Sut();
            var visit = sut.Create(result, _screeningInfo);

            visit.DepressionFlag.ScoreLevel.Should().Be(0);
        }

        [TestMethod]
        public void When_Depression_Negative_ThinkingOfDeathFlagIsOff()
        {
            var result = ScreeningResultMotherObject.GetAllNoAnswers();

            var sut = Sut();
            var visit = sut.Create(result, _screeningInfo);

            visit.DepressionThinkOfDeathAnswer.Should().BeEmpty();
        }

        [TestMethod]
        public void When_Depression_Skipped_FlagIsOff()
        {
            var result = ScreeningResultMotherObject.GetAllNoAnswersForSections();

            var sut = Sut();
            var visit = sut.Create(result, _screeningInfo);

            visit.DepressionFlag.Should().BeNull();
        }

    }
}

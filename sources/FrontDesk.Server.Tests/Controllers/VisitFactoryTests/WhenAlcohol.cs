using Microsoft.VisualStudio.TestTools.UnitTesting;
using FrontDesk.Server.Tests.MotherObjects;
using FluentAssertions;

namespace FrontDesk.Server.Tests.Controllers.VisitFactoryTests
{
    [TestClass]
    public class WhenAlcohol: VisitFactoryTestsBase
    {
        [TestMethod]
        public void When_Alcohol_Positive_FlagIsOn()
        {
            var result = ScreeningResultMotherObject.GetAllNoAnswers();
            var section = result.FindSectionByID(ScreeningSectionDescriptor.Alcohol);

            section.ScoreLevel = 1;
            section.Score = 1;
            section.AppendQuestionAnswer(new ScreeningSectionQuestionResult(0, 1));
            
            var sut = Sut();
            var visit = sut.Create(result, _screeningInfo);

            visit.AlcoholUseFlag.ScoreLevel.Should().BeGreaterThan(0);
        }

        [TestMethod]
        public void When_Alcohol_Negative_FlagIsOff()
        {
            var result = ScreeningResultMotherObject.GetAllNoAnswers();

            var sut = Sut();
            var visit = sut.Create(result, _screeningInfo);

            visit.AlcoholUseFlag.ScoreLevel.Should().Be(0);
        }

        [TestMethod]
        public void When_Alcohol_Skipped_FlagIsOff()
        {
            var result = ScreeningResultMotherObject.GetAllNoAnswersForSections();
            
            var sut = Sut();
            var visit = sut.Create(result, _screeningInfo);

            visit.AlcoholUseFlag.Should().BeNull();
        }

    }
}

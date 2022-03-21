using Microsoft.VisualStudio.TestTools.UnitTesting;
using FrontDesk.Server.Tests.MotherObjects;
using FluentAssertions;

namespace FrontDesk.Server.Tests.Controllers.VisitFactoryTests
{
    [TestClass]
    public class WhenSmokerInHome: VisitFactoryTestsBase
    {
        [TestMethod]
        public void When_SmokerInHome_Positive_FlagIsOn()
        {
            var result = ScreeningResultMotherObject.GetAllNoAnswers();
            var section = result.FindSectionByID(ScreeningSectionDescriptor.SmokerInHome);

            section.ScoreLevel = 1;
            section.Score = 1;
            section.FindQuestionByID(1).AnswerValue = 1;
            
            var sut = Sut();
            var visit = sut.Create(result, _screeningInfo);

            visit.TobacoExposureSmokerInHomeFlag.Should().BeTrue();
        }

        [TestMethod]
        public void When_SmokerInHome_Positive_OtherTobaccoIsOff()
        {
            var result = ScreeningResultMotherObject.GetAllNoAnswers();
            var section = result.FindSectionByID(ScreeningSectionDescriptor.SmokerInHome);

            section.ScoreLevel = 1;
            section.AppendQuestionAnswer(new ScreeningSectionQuestionResult(0, 1));

            var sut = Sut();
            var visit = sut.Create(result, _screeningInfo);

            visit.TobacoExposureSmokingFlag.Should().BeFalse();
            visit.TobacoExposureSmoklessFlag.Should().BeFalse();
            visit.TobacoExposureCeremonyUseFlag.Should().BeFalse();

        }

        [TestMethod]
        public void When_SmokerInHome_Negative_FlagIsOff()
        {
            var result = ScreeningResultMotherObject.GetAllNoAnswers();

            var sut = Sut();
            var visit = sut.Create(result, _screeningInfo);

            visit.TobacoExposureSmokerInHomeFlag.Should().BeFalse();
        }

        [TestMethod]
        public void When_SmokerInHome_Skipped_FlagIsOff()
        {
            var result = ScreeningResultMotherObject.GetAllNoAnswersForSections();

            var sut = Sut();
            var visit = sut.Create(result, _screeningInfo);

            visit.TobacoExposureSmokerInHomeFlag.Should().BeFalse();
        }

    }
}

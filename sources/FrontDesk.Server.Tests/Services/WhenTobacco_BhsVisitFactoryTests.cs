using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FrontDesk.Services;
using FluentAssertions;

namespace FrontDesk.Server.Tests.Services
{
    [TestClass]
    public class WhenTobacco_BhsVisitFactoryTests
    {
        protected BhsVisitFactory Sut()
        {
            return new BhsVisitFactory();
        }

        [TestMethod]
        public void Should_TobacoExposureCeremonyUseFlag_BeOff()
        {
            var result = MotherObjects.ScreeningResultMotherObject.GetAllNoAnswers();
            var info = MotherObjects.ScreeningInfoMotherObject.GetFullScreening();
            var actual = Sut().Create(result, info);

            actual.Should().NotBeNull();
            actual.TobacoExposureCeremonyUseFlag.Should().BeFalse();
        }

        [TestMethod]
        public void Should_TobacoExposureCeremonyUseFlag_BeOn()
        {
            var info = MotherObjects.ScreeningInfoMotherObject.GetFullScreening();
            var result = MotherObjects.ScreeningResultMotherObject.GetAllNoAnswers();
            var sectionResult = result.FindSectionByID(ScreeningSectionDescriptor.Tobacco);

            sectionResult.AppendQuestionAnswer(new ScreeningSectionQuestionResult
            {
                QuestionID = TobaccoQuestionsDescriptor.UseForCeremonyOnlyQuestionID,
                AnswerValue = 1
            });


            var actual = Sut().Create(result, info);

            actual.Should().NotBeNull();
            actual.TobacoExposureCeremonyUseFlag.Should().BeTrue();
        }

        [TestMethod]
        public void Should_TobacoExposureSmoklessFlag_BeOn()
        {
            var info = MotherObjects.ScreeningInfoMotherObject.GetFullScreening();
            var result = MotherObjects.ScreeningResultMotherObject.GetAllNoAnswers();
            var sectionResult = result.FindSectionByID(ScreeningSectionDescriptor.Tobacco);

            sectionResult.AppendQuestionAnswer(new ScreeningSectionQuestionResult
            {
                QuestionID = TobaccoQuestionsDescriptor.DoYouSmokeSmokelessQuestionID,
                AnswerValue = 1
            });


            var actual = Sut().Create(result, info);

            actual.Should().NotBeNull();
            actual.TobacoExposureSmoklessFlag.Should().BeTrue();
        }

        [TestMethod]
        public void Should_TobacoExposureSmokingFlag_BeOn()
        {
            var info = MotherObjects.ScreeningInfoMotherObject.GetFullScreening();
            var result = MotherObjects.ScreeningResultMotherObject.GetAllNoAnswers();
            var sectionResult = result.FindSectionByID(ScreeningSectionDescriptor.Tobacco);

            sectionResult.AppendQuestionAnswer(new ScreeningSectionQuestionResult
            {
                QuestionID = TobaccoQuestionsDescriptor.DoYouSmokeQuestionID,
                AnswerValue = 1
            });


            var actual = Sut().Create(result, info);

            actual.Should().NotBeNull();
            actual.TobacoExposureSmokingFlag.Should().BeTrue();
        }
    }
}

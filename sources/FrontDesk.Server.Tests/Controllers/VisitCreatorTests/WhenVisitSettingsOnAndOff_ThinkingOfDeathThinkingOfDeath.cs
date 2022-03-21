using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FrontDesk.Server.Tests.MotherObjects;
using System.Linq;
using FrontDesk.Server.Screening.Models;
using FluentAssertions;
using Moq;

namespace FrontDesk.Server.Tests.Controllers.VisitCreatorTests
{
    [TestClass]
    public class WhenVisitSettingsOnAndOff_ThinkingOfDeathThinkingOfDeath : VisitCreatorChecksSystemSettingsTestsBase
    {
        private ScreeningResult GetPositiveResult()
        {
            var result = ScreeningResultMotherObject.GetAllNoAnswers();
            var depressionResult = result.FindSectionByID(ScreeningSectionDescriptor.Depression);
            depressionResult.AppendQuestionAnswer(new ScreeningSectionQuestionResult
            {
                QuestionID = ScreeningSectionDescriptor.DepressionThinkOfDeathQuestionID,
                AnswerValue = 1
            });
            return result;
        }

        private ScreeningResult GetPositiveNearlyEveryDayResult()
        {
            var result = ScreeningResultMotherObject.GetAllNoAnswers();
            var depressionResult = result.FindSectionByID(ScreeningSectionDescriptor.Depression);
            depressionResult.AppendQuestionAnswer(new ScreeningSectionQuestionResult
            {
                QuestionID = ScreeningSectionDescriptor.DepressionThinkOfDeathQuestionID,
                AnswerValue = 3
            });
            return result;
        }

        [TestMethod]
        public void When_ThinkingOfDeath_isOn_AndPositive_VisitCreated()
        {
            var settings = VisitSettingsMotherObject.GetAllOff();
            settings.First(x => x.Id == VisitSettingsDescriptor.DepressionThinkOfDeath).IsEnabled = true;

            _visitSettingsMock.Setup(x => x.GetAll()).Returns(settings);

            var result = GetPositiveResult();
            var sut = Sut();

            sut.Create(result, _screeningInfo);

            _bhsVisitFactoryMock.Verify(x => x.Create(result, _screeningInfo), Times.Once());
        }

        [TestMethod]
        public void When_ThinkingOfDeath_isOn_AndNegative_VisitNotCreated()
        {
            var settings = VisitSettingsMotherObject.GetAllOff();
            settings.First(x => x.Id == VisitSettingsDescriptor.DepressionThinkOfDeath).IsEnabled = true;

            _visitSettingsMock.Setup(x => x.GetAll()).Returns(settings);
            var result = ScreeningResultMotherObject.GetAllNoAnswers();

            var sut = Sut();
            sut.Create(result, _screeningInfo);

            _bhsVisitFactoryMock.Verify(x => x.Create(result, _screeningInfo), Times.Never());
        }

        [TestMethod]
        public void When_ThinkingOfDeath_isOff_AndPositive_VisitNotCreated()
        {
            var settings = VisitSettingsMotherObject.GetAllOn();
            settings.First(x => x.Id == VisitSettingsDescriptor.DepressionThinkOfDeath).IsEnabled = false;

            _visitSettingsMock.Setup(x => x.GetAll()).Returns(settings);

            var result = GetPositiveResult();
            var sut = Sut();
            sut.Create(result, _screeningInfo);

            _bhsVisitFactoryMock.Verify(x => x.Create(result, _screeningInfo), Times.Never());

        }

        [TestMethod]
        public void When_ThinkingOfDeath_isOn_AndPositive_But_Below_CutScore_VisitNotCreated()
        {
            var settings = VisitSettingsMotherObject.GetSucideThreshold3();
            
            _visitSettingsMock.Setup(x => x.GetAll()).Returns(settings);

            var result = GetPositiveResult();
            var sut = Sut();

            sut.Create(result, _screeningInfo);

            _bhsVisitFactoryMock.Verify(x => x.Create(result, _screeningInfo), Times.Never());
        }

        [TestMethod]
        public void When_ThinkingOfDeath_isOn_AndPositive_And_Equal_CutScore_VisitCreated()
        {
            var settings = VisitSettingsMotherObject.GetSucideThreshold3();

            _visitSettingsMock.Setup(x => x.GetAll()).Returns(settings);

            var result = GetPositiveNearlyEveryDayResult();
            var sut = Sut();

            sut.Create(result, _screeningInfo);

            _bhsVisitFactoryMock.Verify(x => x.Create(result, _screeningInfo), Times.Once());
        }
    }
}

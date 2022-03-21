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
    public class WhenVisitSettingsOnAndOff_Depression : VisitCreatorChecksSystemSettingsTestsBase
    {
        [TestMethod]
        public void When_Depression_isOn_AndPositive_VisitCreated()
        {
            var settings = VisitSettingsMotherObject.GetAllOff();
            settings.First(x => x.Id == VisitSettingsDescriptor.Depression).IsEnabled = true;

            _visitSettingsMock.Setup(x => x.GetAll()).Returns(settings);
            var result = ScreeningResultMotherObject.GetAllNoAnswers();
            var item = result.FindSectionByID(ScreeningSectionDescriptor.Depression);
            item.ScoreLevel = 1;
            item.Score = 2;


            var sut = Sut();
            sut.Create(result, _screeningInfo);
            _bhsVisitFactoryMock.Verify(x => x.Create(result, _screeningInfo), Times.Once());
        }

        [TestMethod]
        public void When_Depression_isOn_AndNegative_VisitNotCreated()
        {
            var settings = VisitSettingsMotherObject.GetAllOff();
            settings.First(x => x.Id == VisitSettingsDescriptor.Depression).IsEnabled = true;

            _visitSettingsMock.Setup(x => x.GetAll()).Returns(settings);
            var result = ScreeningResultMotherObject.GetAllNoAnswers();

            var sut = Sut();
            sut.Create(result, _screeningInfo);
            _bhsVisitFactoryMock.Verify(x => x.Create(result, _screeningInfo), Times.Never());
        }

        [TestMethod]
        public void When_Depression_isOff_AndPositive_VisitNotCreated()
        {
            var settings = VisitSettingsMotherObject.GetAllOn();
            settings.First(x => x.Id == VisitSettingsDescriptor.Depression).IsEnabled = false;

            _visitSettingsMock.Setup(x => x.GetAll()).Returns(settings);
            var result = ScreeningResultMotherObject.GetAllNoAnswers();
            result.FindSectionByID(ScreeningSectionDescriptor.Depression).ScoreLevel = 1;
            result.FindSectionByID(ScreeningSectionDescriptor.Depression).Score = 1;

            var sut = Sut();
            sut.Create(result, _screeningInfo);
            _bhsVisitFactoryMock.Verify(x => x.Create(result, _screeningInfo), Times.Never());
        }

        [TestMethod]
        public void When_Depression_isOn_And_CutScore_BelowThreshold_AndPositive_VisitNotCreated()
        {
            var settings = VisitSettingsMotherObject.GetDepressionThreshold21();
           
            _visitSettingsMock.Setup(x => x.GetAll()).Returns(settings);

            var result = ScreeningResultMotherObject.GetAllNoAnswers();
            var sectionResult = result.FindSectionByID(ScreeningSectionDescriptor.Depression);
            sectionResult.ScoreLevel = 3;
            sectionResult.Score = 20;

            var sut = Sut();
            sut.Create(result, _screeningInfo);
            _bhsVisitFactoryMock.Verify(x => x.Create(result, _screeningInfo), Times.Never());
        }

        [TestMethod]
        public void When_Depression_isOn_And_CutScore_MeetsThreshold_AndPositive_VisitCreated()
        {
            var settings = VisitSettingsMotherObject.GetDepressionThreshold21();

            _visitSettingsMock.Setup(x => x.GetAll()).Returns(settings);

            var result = ScreeningResultMotherObject.GetAllNoAnswers();
            
            var sectionResult = result.FindSectionByID(ScreeningSectionDescriptor.Depression);
            sectionResult.ScoreLevel = 3;
            sectionResult.Score = 21;


            var sut = Sut();
            sut.Create(result, _screeningInfo);

            _bhsVisitFactoryMock.Verify(x => x.Create(result, _screeningInfo), Times.Once());
        }
    }
}

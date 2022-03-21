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
    public class WhenVisitSettingsOnAndOff_PartnerViolence : VisitCreatorChecksSystemSettingsTestsBase
    {
        [TestMethod]
        public void When_PartnerViolence_isOn_AndPositive_VisitCreated()
        {
            var settings = VisitSettingsMotherObject.GetAllOff();
            settings.First(x => x.Id == VisitSettingsDescriptor.PartnerViolence).IsEnabled = true;

            _visitSettingsMock.Setup(x => x.GetAll()).Returns(settings);
            var result = ScreeningResultMotherObject.GetAllNoAnswers();
            result.FindSectionByID(ScreeningSectionDescriptor.PartnerViolence).ScoreLevel = 1;
            result.FindSectionByID(ScreeningSectionDescriptor.PartnerViolence).Score = 1;

            var sut = Sut();
            sut.Create(result, _screeningInfo);
            _bhsVisitFactoryMock.Verify(x => x.Create(result, _screeningInfo), Times.Once());
        }

        [TestMethod]
        public void When_PartnerViolence_isOn_AndNegative_VisitNotCreated()
        {
            var settings = VisitSettingsMotherObject.GetAllOff();
            settings.First(x => x.Id == VisitSettingsDescriptor.PartnerViolence).IsEnabled = true;

            _visitSettingsMock.Setup(x => x.GetAll()).Returns(settings);
            var result = ScreeningResultMotherObject.GetAllNoAnswers();

            var sut = Sut();
            sut.Create(result, _screeningInfo);
            _bhsVisitFactoryMock.Verify(x => x.Create(result, _screeningInfo), Times.Never());
        }

        [TestMethod]
        public void When_PartnerViolence_isOff_AndPositive_VisitNotCreated()
        {
            var settings = VisitSettingsMotherObject.GetAllOn();
            settings.First(x => x.Id == VisitSettingsDescriptor.PartnerViolence).IsEnabled = false;

            _visitSettingsMock.Setup(x => x.GetAll()).Returns(settings);
            var result = ScreeningResultMotherObject.GetAllNoAnswers();
            result.FindSectionByID(ScreeningSectionDescriptor.PartnerViolence).ScoreLevel = 1;
            result.FindSectionByID(ScreeningSectionDescriptor.PartnerViolence).Score = 1;

            var sut = Sut();
            sut.Create(result, _screeningInfo);
            _bhsVisitFactoryMock.Verify(x => x.Create(result, _screeningInfo), Times.Never());

        }
    }
}

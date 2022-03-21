using Microsoft.VisualStudio.TestTools.UnitTesting;
using FrontDesk.Server.Tests.MotherObjects;
using System.Linq;
using Moq;
using System.Collections.Generic;
using FrontDesk.Configuration;

namespace FrontDesk.Server.Tests.Controllers.VisitCreatorTests
{

    public abstract class WhenVisitSettingsOnAndOff_TobaccoBase : VisitCreatorChecksSystemSettingsTestsBase
    {

        protected abstract string TobaccoQuestionSettingId { get; }
        protected abstract int TobaccoQuestionId { get; }

        protected ScreeningResult GetPositiveResult()
        {
            var result = ScreeningResultMotherObject.GetAllNoAnswers();
            var tobacco = result.FindSectionByID(ScreeningSectionDescriptor.Tobacco);
            tobacco.AppendQuestionAnswer(new ScreeningSectionQuestionResult
            {
                QuestionID = TobaccoQuestionId,
                AnswerValue = 1
            });
            return result;
        }

        protected List<VisitSettingItem> GetOnSettings()
        {
            var settings = VisitSettingsMotherObject.GetAllOff();
            settings.First(x => x.Id == TobaccoQuestionSettingId).IsEnabled = true;
            return settings;
        }

        protected List<VisitSettingItem> GetOffSettings()
        {
            var settings = VisitSettingsMotherObject.GetAllOff();
            return settings;
        }

        protected void When_Tobacco_isOn_AndPositive_VisitCreated()
        {

            var settings = GetOnSettings();
            _visitSettingsMock.Setup(x => x.GetAll()).Returns(settings);

            var result = GetPositiveResult();
            var sut = Sut();

            sut.Create(result, _screeningInfo);

            _bhsVisitFactoryMock.Verify(x => x.Create(result, _screeningInfo), Times.Once());
        }

        protected void When_Tobacco_isOn_AndNegative_VisitNotCreated()
        {
            var settings = GetOnSettings();
            _visitSettingsMock.Setup(x => x.GetAll()).Returns(settings);

            var result = ScreeningResultMotherObject.GetAllNoAnswers();

            var sut = Sut();
            sut.Create(result, _screeningInfo);

            _bhsVisitFactoryMock.Verify(x => x.Create(result, _screeningInfo), Times.Never());
        }

        protected void When_Tobacco_isOff_AndPositive_VisitNotCreated()
        {
            var settings = GetOffSettings();
            _visitSettingsMock.Setup(x => x.GetAll()).Returns(settings);

            var result = GetPositiveResult();
            var sut = Sut();
            sut.Create(result, _screeningInfo);

            _bhsVisitFactoryMock.Verify(x => x.Create(result, _screeningInfo), Times.Never());

        }
    }
}

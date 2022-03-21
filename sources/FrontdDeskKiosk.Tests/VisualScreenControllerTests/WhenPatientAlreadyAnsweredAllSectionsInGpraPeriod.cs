using System;
using FluentAssertions;
using FrontdDeskKiosk.Tests.VisualScreenControllerTests;
using FrontDesk;
using FrontDesk.Kiosk;
using FrontDesk.Kiosk.Screens;
using FrontDesk.Kiosk.Services;
using FrontDesk.Kiosk.Workflow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RPMS.Common.Models;

namespace FrontdDeskKiosk.Tests
{





    [TestClass]
    public class WhenPatientAlreadyAnsweredAllSectionsInGpraPeriod : ScreeningConrollerContextBase<VisualScreenController>
    {

        Mock<IVisualScreenFactory> _visualScreenFactoryMock = new Mock<IVisualScreenFactory>();
        Mock<IUserSessionTimeoutController> _userSessionTimeoutControllerMock = new Mock<IUserSessionTimeoutController>();
        protected Mock<IPatientNameValidationService> _patientNameValidationServiceMock = new Mock<IPatientNameValidationService>();

        protected override void construct()
        {
            _patientNameValidationServiceMock.Setup(x => x.Validate(It.IsAny<IScreeningPatientIdentity>())).
                Returns<PatientSearch>(null);

            var screeningController = new ScreeningController(_screeningFrequencySpecificationMock.Object,
                _resultStateMock.Object, _screeningSectionAgeServiceMock.Object);

            sut = new VisualScreenController(
                screeningController,
                _userSessionTimeoutControllerMock.Object,
                _visualScreenFactoryMock.Object,
                _screeningSectionAgeServiceMock.Object,
                _resultStateMock.Object,
                 _patientNameValidationServiceMock.Object
                );
        }

        protected override void given()
        {
            base.given();
            _screeningSectionAgeServiceMock.Setup(x => x.GetMinimalAgeForScreeningSection(It.IsAny<string>()))
                .Returns(new ScreeningSectionAge
                {
                    IsEnabled = true,
                    MinimalAge = 0,

                });

            _screeningFrequencySpecificationMock.Setup(x => x.IsSkipRequiredForSection(It.IsAny<string>())).Returns(true);

            _visualScreenFactoryMock.Setup(x => x.CreateScreenForStep(It.IsAny<ScreeningStep>(), It.IsAny<ScreeningSection>(), It.IsAny<IScreeningResultState>()))
               .Returns<ScreeningStep, ScreeningSection, IScreeningResultState>(
                   (step, section, state) => new FakeVisualScreen
                   {
                       Step = step
                   });

        }


        protected override void when()
        {
            var eventArgs = new NextScreenClickedEventArg(null, "03/24/1971");
            sut.CurrentState.Step = ScreeningStep.PatientDateOfBirth;
            sut.OnNextScreenClicked(eventArgs);

        }

        [TestMethod()]
        public void CurrentStep_Is_SendResult()
        {
            ((FakeVisualScreen)sut.CurrentState.Screen).Step.Should().Be(ScreeningStep.SendResult);
        }

        [TestMethod()]
        public void Birthday_Is_In_Results()
        {
            Result.Birthday.Should().Be(new DateTime(1971, 03, 24));
        }

    }

}

using FrontDesk;
using FrontDesk.Kiosk;
using FrontDesk.Kiosk.Services;
using FrontDesk.Kiosk.Workflow;
using Moq;
using RPMS.Common.Models;

namespace FrontdDeskKiosk.Tests.VisualScreenControllerTests
{
    public class VisualScreeningControllerContextBase : ScreeningConrollerContextBase<VisualScreenController>
    {
        protected Mock<IVisualScreenFactory> _visualScreenFactoryMock = new Mock<IVisualScreenFactory>();
        protected Mock<IUserSessionTimeoutController> _userSessionTimeoutControllerMock = new Mock<IUserSessionTimeoutController>();
        protected Mock<IPatientNameValidationService> _patientNameValidationServiceMock = new Mock<IPatientNameValidationService>();

        protected override void construct()
        {
            _patientNameValidationServiceMock.Setup(x => x.Validate(It.IsAny<IScreeningPatientIdentity>())).
                Returns<PatientSearch>(null);

            var screeningController = new ScreeningController(
                _screeningFrequencySpecificationMock.Object,
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

            _visualScreenFactoryMock.Setup(x => x.CreateScreenForStep(It.IsAny<ScreeningStep>(), It.IsAny<ScreeningSection>(), It.IsAny<IScreeningResultState>()))
                .Returns<ScreeningStep, ScreeningSection, IScreeningResultState>(
                    (step, section, state) => new FakeVisualScreen
                    {
                        Step = step
                    });
        }
    }
}
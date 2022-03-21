using FrontDesk;
using FrontDesk.Configuration;
using FrontDesk.Kiosk.Screens;
using FrontDesk.Kiosk.Workflow;
using Moq;

namespace FrontdDeskKiosk.Tests.VisualScreenControllerTests.WhenScreeningPatientContactQuestions
{
    public class WhenScreeningPatientContactQuestionsContext : VisualScreeningControllerContextBase
    {
        protected virtual string DateOfBirth
        {
            get { return "03/24/1974"; }
        }

        protected virtual byte MinimalAge
        {
            get { return 0; }
        }

        protected override void given()
        {
            base.given();

            _screeningSectionAgeServiceMock.Setup(x => x.GetMinimalAgeForScreeningSection(It.IsAny<string>()))
                .Returns(new ScreeningSectionAge
                {
                    IsEnabled = true
                });

            _screeningSectionAgeServiceMock.Setup(x => x.GetMinimalAgeForScreeningSection(ScreeningFrequencyDescriptor.ContactFrequencyID))
                .Returns(new ScreeningSectionAge
                {
                    IsEnabled = true,
                    MinimalAge = MinimalAge,
                });


            _screeningFrequencySpecificationMock.Setup(x => x.IsSkipRequiredForSection(It.IsAny<string>())).Returns(false);

            sut.CurrentState.Step = ScreeningStep.PatientDateOfBirth;
            sut.NextState.Step = ScreeningStep.PatientStreet;
        }

        protected override void when()
        {
            var eventArgs = new NextScreenClickedEventArg(null, DateOfBirth);
            sut.OnNextScreenClicked(eventArgs);
        }
    }
}
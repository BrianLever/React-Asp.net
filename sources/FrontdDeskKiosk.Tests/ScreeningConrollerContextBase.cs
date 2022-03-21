using FrontdDeskKiosk.Tests.MotherObjects;
using FrontDesk;
using FrontDesk.Common.Screening;
using FrontDesk.Kiosk.Workflow;
using FrontDesk.Services;
using FrontDesk.Tests.Common;
using Moq;

namespace FrontdDeskKiosk.Tests
{
    public abstract class ScreeningConrollerContextBase<T> : BaseUnitTest<T>
    {
        protected readonly Mock<IScreeningFrequencySpecification> _screeningFrequencySpecificationMock = new Mock<IScreeningFrequencySpecification>();
        protected readonly Mock<IScreeningResultState> _resultStateMock = new Mock<IScreeningResultState>();
        protected readonly Mock<IScreeningSectionAgeService> _screeningSectionAgeServiceMock = new Mock<IScreeningSectionAgeService>();
        protected readonly Screening _screeningMetaData = ScreeningMotherObject.GetFullScreening();
        protected readonly ScreeningTimeLog _screeningTimeLog = new ScreeningTimeLog();


        protected ScreeningResult Result;

        protected override void given()
        {

            _resultStateMock.SetupGet(x => x.ScreeningMetaData).Returns(_screeningMetaData);
            _resultStateMock.SetupGet(x => x.ScreeningTimeLog).Returns(_screeningTimeLog);
            _resultStateMock.SetupGet(x => x.PatientNameValidatedOnServer).Returns(true);

            Result = ScreeningResultMotherObject.GetPatientNameAndBirthday();

            _resultStateMock.SetupGet(x => x.Result).Returns(Result);
            _screeningSectionAgeServiceMock.Setup(x => x.GetMinimalAgeForScreeningSection(It.IsAny<string>()))
                .Returns<string>(id => new ScreeningSectionAge
                {
                    IsEnabled = true,
                    MinimalAge = 0,
                    ScreeningSectionID = id
                });

        }
    }
}
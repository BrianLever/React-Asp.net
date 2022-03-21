using System.Linq;
using FluentAssertions;
using FrontdDeskKiosk.Tests.MotherObjects;
using FrontDesk;
using FrontDesk.Kiosk.Workflow;
using FrontDesk.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FrontdDeskKiosk.Tests.ScreenerControllerTests.WhenScreeningSmokerInHome
{
    [TestClass]
    public class WhenSmokerInHomeIsDisabled : ScreeningConrollerContextBase<ScreeningController>
    {
        protected override void construct()
        {

            sut = new ScreeningController(_screeningFrequencySpecificationMock.Object, _resultStateMock.Object, _screeningSectionAgeServiceMock.Object);
        }

        protected override void given()
        {
            base.given();

            _screeningFrequencySpecificationMock.Setup(x => x.IsSkipRequiredForSection("SIH")).Returns(true);
           
            sut.CurrentState.Reset();
        }

        protected override void when()
        {
            sut.GoToNextSection();
        }



        [TestMethod]
        public void CurrentSection_Not_Null()
        {
            sut.CurrentState.Section.Should().NotBeNull();
        }

        [TestMethod]
        public void CurrentSection_Is_Tobacco()
        {
            sut.CurrentState.Section.Should().BeSameAs(_screeningMetaData.FindSectionByID("TCC"));
        }
        [TestMethod]
        public void CurrentQuestion_Is_Not_Null()
        {
            sut.CurrentState.SectionQuestion.Should().NotBeNull();
        }

        public void CurrentQuestion_Is_MainQuestion()
        {
            sut.CurrentState.SectionQuestion.QuestionID.Should().Be(4);
        }
    }
}

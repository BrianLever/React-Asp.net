using System;
using FluentAssertions;
using FrontDesk;
using FrontDesk.Kiosk;
using FrontDesk.Kiosk.Screens;
using FrontDesk.Kiosk.Workflow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FrontdDeskKiosk.Tests.VisualScreenControllerTests
{


    [TestClass]
    public class WhenAllScreeningsDesabled_AfterBirthdayGoesSendResuts : VisualScreeningControllerContextBase
    {
        protected override void given()
        {
            base.given();
            _screeningSectionAgeServiceMock.Setup(x => x.GetMinimalAgeForScreeningSection(It.IsAny<string>()))
                .Returns(new ScreeningSectionAge
                {
                    IsEnabled = false
                });

            _screeningFrequencySpecificationMock.Setup(x => x.IsSkipRequiredForSection(It.IsAny<string>())).Returns(false);

            

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

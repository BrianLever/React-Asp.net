using System;
using FluentAssertions;
using FrontDesk;
using FrontDesk.Configuration;
using FrontDesk.Kiosk;
using FrontDesk.Kiosk.Screens;
using FrontDesk.Kiosk.Workflow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FrontdDeskKiosk.Tests.VisualScreenControllerTests.WhenScreeningPatientContactQuestions
{


    [TestClass]
    public class WhenPatientAgeIsBelowTheLimitAndAlreadyScreenedThisPeriod : WhenScreeningPatientContactQuestionsContext
    {
        protected override string DateOfBirth
        {
            get { return "03/24/1999"; }
        }

        protected override byte MinimalAge
        {
            get { return 18; }
        }


        protected override void given()
        {
            base.given();

            _screeningFrequencySpecificationMock.Setup(x => x.IsSkipRequiredForSection(ScreeningFrequencyDescriptor.ContactFrequencyID)).Returns(true);
        }


        [TestMethod]
        public void CurrentStep_Is_ScreeningSection()
        {
            ((FakeVisualScreen)sut.CurrentState.Screen).Step.Should().Be(ScreeningStep.ScreeningSection);
        }

    }

}

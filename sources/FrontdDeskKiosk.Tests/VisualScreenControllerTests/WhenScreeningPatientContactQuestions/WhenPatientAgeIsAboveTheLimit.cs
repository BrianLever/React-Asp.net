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
    public class WhenPatientAgeIsAboveTheLimit : WhenScreeningPatientContactQuestionsContext
    {
        protected override string DateOfBirth
        {
            get { return "03/24/1983"; }
        }

        protected override byte MinimalAge
        {
            get { return 18; }
        }


        [TestMethod]
        public void CurrentStep_Is_PatientStreet()
        {
            ((FakeVisualScreen)sut.CurrentState.Screen).Step.Should().Be(ScreeningStep.PatientStreet);
        }

    }

}

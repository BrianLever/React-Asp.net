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
    public class WhenPatientAgeIsBelowTheLimit : WhenScreeningPatientContactQuestionsContext
    {
        protected override string DateOfBirth
        {
            get { return DateTime.Today.AddYears(-17).ToShortDateString(); }
        }

        protected override byte MinimalAge
        {
            get { return 18; }
        }


        [TestMethod]
        public void CurrentStep_Is_ScreeningSection()
        {
            ((FakeVisualScreen)sut.CurrentState.Screen).Step.Should().Be(ScreeningStep.ScreeningSection);
        }

    }

}

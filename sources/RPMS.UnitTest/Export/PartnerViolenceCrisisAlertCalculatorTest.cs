using RPMS.Common.Export;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FrontDesk;
using System.Collections.Generic;
using RPMS.Common.Models;
using FluentAssertions;

namespace RPMS.UnitTest.Export
{
    
    
    /// <summary>
    ///This is a test class for PartnerViolenceCrisisAlertCalculatorTest and is intended
    ///to contain all PartnerViolenceCrisisAlertCalculatorTest Unit Tests
    ///</summary>
    [DeploymentItem(@"Configuration\rpmsExportConfiguration.config", "Configuration")]
    [TestClass()]
    public class PartnerViolenceCrisisAlertCalculatorTest
    {

        [TestMethod()]
        public void Calculate_should_return_null_for_non_asked()
        {
            ScreeningSectionResult sectionResult = ScreeningResultHelper.GetAlcoholAllYes();
            PartnerViolenceCrisisAlertCalculator target = new PartnerViolenceCrisisAlertCalculator();
            IList<CrisisAlert> actual;

          
            actual = target.Calculate(new ScreeningSectionResult[] { sectionResult });

            actual.Should().NotBeNull();
            actual.Should().BeEmpty();

        }

        [TestMethod()]
        public void Calculate_should_return_null_for_no()
        {
            ScreeningResult sectionResult = ScreeningResultHelper.GetAllNoAnswers();
            PartnerViolenceCrisisAlertCalculator target = new PartnerViolenceCrisisAlertCalculator();
            IList<CrisisAlert> actual;


            actual = target.Calculate(sectionResult.SectionAnswers);

            actual.Should().NotBeNull();
            actual.Should().BeEmpty();
        }


        [TestMethod()]
        public void Calculate_should_return_null_for_never()
        {
            ScreeningSectionResult sectionResult = ScreeningResultHelper.GetViolenceHurtNever();
            PartnerViolenceCrisisAlertCalculator target = new PartnerViolenceCrisisAlertCalculator();
            IList<CrisisAlert> actual;


            actual = target.Calculate(new ScreeningSectionResult[] { sectionResult });

            actual.Should().NotBeNull();
            actual.Should().BeEmpty();
        }

        [TestMethod()]
        public void Calculate_should_return_for_rarery()
        {
            ScreeningSectionResult sectionResult = ScreeningResultHelper.GetViolenceHurtRarely();
            PartnerViolenceCrisisAlertCalculator target = new PartnerViolenceCrisisAlertCalculator();
            IList<CrisisAlert> actual;


            actual = target.Calculate(new ScreeningSectionResult[] { sectionResult });
            actual.Should().NotBeNull();
            actual.Should().NotBeEmpty();
            actual.Count.Should().Be(1);

            var actualAlert = actual[0];

            Assert.AreEqual("CLINICAL WARNING", actualAlert.Title, "Title failed");
            Assert.AreEqual(15, actualAlert.DocumentTypeID, "DocumentTypeID failed");
            Assert.AreEqual(@"Patient answered ""Rarely"" to: ""Over the LAST 12 MONTHS, how often did your partner, family member, or caregiver Physically HURT you?""", actualAlert.Details, "Detail failed");

        }
    }
}

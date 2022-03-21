using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace FrontDesk.Server.Tests.GPRAReportingTimeTests
{


    /// <summary>
    ///This is a test class for GPRAReportingTimeTest and is intended
    ///to contain all GPRAReportingTimeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class GetGPRAFrequencyEffectiveDateInMonthsTests
    {
 
        /// <summary>
        ///A test for GetGPRAFrequencyEffectiveDate
        ///</summary>
        [TestMethod()]
        public void Can_calculate_montly_effective_date()
        {
            GPRAReportingTime target = new GPRAReportingTime(2011);
            DateTime currentDate = new DateTime(2012, 09, 20, 13, 10, 05);
            int screeningFrequencyInMonths = 1;
            DateTime expected = new DateTime(2012, 09, 01);
            DateTime actual;

            actual = target.GetGPRAFrequencyEffectiveDateInMonths(currentDate, screeningFrequencyInMonths);

            actual.Should().Be(expected);
        }

        /// <summary>
        ///A test for GetGPRAFrequencyEffectiveDate
        ///</summary>
        [TestMethod()]
        public void Can_calculate_quaterly_effective_date()
        {
            GPRAReportingTime target = new GPRAReportingTime(2011);
            DateTime currentDate = new DateTime(2012, 12, 4, 13, 10, 05);
            int screeningFrequencyInMonths = 3;
            DateTime expected = new DateTime(2012, 10, 01);
            DateTime actual;
            actual = target.GetGPRAFrequencyEffectiveDateInMonths(currentDate, screeningFrequencyInMonths);
            Assert.AreEqual(expected, actual);
        }

      
        [TestMethod()]
        public void Can_calculate_quaterly_effective_date_when_just_started_grpa()
        {
            GPRAReportingTime target = new GPRAReportingTime(2011);
            
            DateTime currentDate = new DateTime(2012, 03, 4, 13, 10, 05);
            int screeningFrequencyInMonths = 3;
            DateTime expected = new DateTime(2012, 01, 01);
            
            var actual = target.GetGPRAFrequencyEffectiveDateInMonths(currentDate, screeningFrequencyInMonths);
            
            actual.Should().Be(expected);
        }

        /// <summary>
        ///A test for GetGPRAFrequencyEffectiveDate
        ///</summary>
        [TestMethod()]
        public void Can_calculate_everyvisit_effective_date()
        {
            GPRAReportingTime target = new GPRAReportingTime(2011);
            DateTime currentDate = new DateTime(2012, 10, 4, 13, 10, 05);
            int screeningFrequencyInMonths = 0;
            DateTime expected = new DateTime(2012, 10, 01);
            DateTime actual;
            actual = target.GetGPRAFrequencyEffectiveDateInMonths(currentDate, screeningFrequencyInMonths);
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        ///A test for GetGPRAFrequencyEffectiveDate
        ///</summary>
        [TestMethod()]
        public void Can_calculate_annual_effective_date()
        {
            GPRAReportingTime target = new GPRAReportingTime(2011);
            DateTime currentDate = new DateTime(2012, 10, 4, 13, 10, 05);
            int screeningFrequencyInMonths = 12;
            DateTime expected = new DateTime(2012, 10, 01);
            
            var actual = target.GetGPRAFrequencyEffectiveDateInMonths(currentDate, screeningFrequencyInMonths);
            
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for GetGPRAFrequencyEffectiveDate
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Can_check_invalid_frequency_interval()
        {
            GPRAReportingTime target = new GPRAReportingTime(2011);
            DateTime currentDate = new DateTime(2012, 10, 4, 13, 10, 05);
            int screeningFrequencyInMonths = 13;
            DateTime actual;

            target.GetGPRAFrequencyEffectiveDateInMonths(currentDate, screeningFrequencyInMonths);

        }



    }
}

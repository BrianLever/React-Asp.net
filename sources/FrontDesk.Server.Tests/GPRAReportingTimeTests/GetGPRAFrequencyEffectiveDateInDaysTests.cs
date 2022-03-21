using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace FrontDesk.Server.Tests.GPRAReportingTimeTests
{

    [TestClass()]
    public class GetGPRAFrequencyEffectiveDateInDaysTests
    {
 
        /// <summary>
        ///A test for GetGPRAFrequencyEffectiveDate
        ///</summary>
        [TestMethod()]
        public void Can_calculate_today_effective_date()
        {
            GPRAReportingTime target = new GPRAReportingTime(2020);
            DateTime currentDate = new DateTime(2020, 10, 09, 09, 10, 05);
            int screeningFrequencyInDays = 1;
            
            var expected = new DateTime(2020, 10, 09, 0, 0, 0);
           
            var actual = target.GetGPRAFrequencyEffectiveDateInDays(currentDate, screeningFrequencyInDays);

            actual.Should().Be(expected);
        }


        /// <summary>
        ///A test for GetGPRAFrequencyEffectiveDate
        ///</summary>
        [TestMethod()]
        public void Can_calculate_weekly_effective_date()
        {
            GPRAReportingTime target = new GPRAReportingTime(2020);
            DateTime currentDate = new DateTime(2020, 10, 9, 09, 10, 05);
            int screeningFrequencyInDays = 7;

            var expected = new DateTime(2020, 10, 03, 0, 0, 0);

            var actual = target.GetGPRAFrequencyEffectiveDateInDays(currentDate, screeningFrequencyInDays);

            actual.Should().Be(expected);
        }


        /// <summary>
        ///A test for GetGPRAFrequencyEffectiveDate
        ///</summary>
        [TestMethod()]
        public void Can_calculate_weekly_just_after_Grpa_effective_date()
        {
            GPRAReportingTime target = new GPRAReportingTime(2020);
            DateTime currentDate = new DateTime(2020, 10, 04, 09, 10, 05);
            int screeningFrequencyInDays = 7;

            var expected = new DateTime(2020, 10, 01, 0, 0, 0);

            var actual = target.GetGPRAFrequencyEffectiveDateInDays(currentDate, screeningFrequencyInDays);

            actual.Should().Be(expected);
        }

    }
}

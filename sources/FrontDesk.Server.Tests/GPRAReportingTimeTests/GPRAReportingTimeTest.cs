using FluentAssertions;

using FrontDesk.Server;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;

namespace FrontDesk.Server.Tests.GPRAReportingTimeTests
{


    /// <summary>
    ///This is a test class for GPRAReportingTimeTest and is intended
    ///to contain all GPRAReportingTimeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class GPRAReportingTimeTest
    {

        [TestMethod()]
        public void Can_return_periods_since_May2008_to_June2012()
        {
            DateTime startDate = new DateTime(2008, 5, 1);
            DateTime today = new DateTime(2012, 6, 12);
            List<GPRAReportingTime> expected = new List<GPRAReportingTime>
            {
                new GPRAReportingTime(2011),
                new GPRAReportingTime(2010),
                new GPRAReportingTime(2009),
                new GPRAReportingTime(2008),
                new GPRAReportingTime(2007),
            };
            List<GPRAReportingTime> actual;
            
            actual = GPRAReportingTime.GetPeriodsSince(startDate, today);
            
            Assert.AreEqual(expected.Count, actual.Count, "Count failed");
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual<DateTime>(expected[i].StartDate, actual[i].StartDate, "Start date failed");
                Assert.AreEqual<DateTime>(expected[i].EndDate, actual[i].EndDate, "End date failed");
            }
        }

        [TestMethod()]
        public void Can_return_periods_since_May2008_to_July2012()
        {
            DateTime starteDate = new DateTime(2008, 5, 1);
            DateTime today = new DateTime(2012, 10, 1);
            List<GPRAReportingTime> expected = new List<GPRAReportingTime>
            {
                new GPRAReportingTime(2012),
                new GPRAReportingTime(2011),
                new GPRAReportingTime(2010),
                new GPRAReportingTime(2009),
                new GPRAReportingTime(2008),
                new GPRAReportingTime(2007),

            };
            List<GPRAReportingTime> actual;
            actual = GPRAReportingTime.GetPeriodsSince(starteDate, today);
            Assert.AreEqual(expected.Count, actual.Count, "Count failed");
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual<DateTime>(expected[i].StartDate, actual[i].StartDate, "Start date failed");
                Assert.AreEqual<DateTime>(expected[i].EndDate, actual[i].EndDate, "End date failed");
            }
        }

        [TestMethod()]
        public void Can_return_periods_since_Sep2008_to_May2012()
        {
            DateTime starteDate = new DateTime(2008, 10, 1);
            DateTime today = new DateTime(2012, 5, 1);
            List<GPRAReportingTime> expected = new List<GPRAReportingTime>
            {
                new GPRAReportingTime(2011),
                new GPRAReportingTime(2010),
                new GPRAReportingTime(2009),
                new GPRAReportingTime(2008),

            };
            List<GPRAReportingTime> actual;
            actual = GPRAReportingTime.GetPeriodsSince(starteDate, today);

            Assert.AreEqual(expected.Count, actual.Count, "Count failed");

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual<DateTime>(expected[i].StartDate, actual[i].StartDate, "Start date failed");
                Assert.AreEqual<DateTime>(expected[i].EndDate, actual[i].EndDate, "End date failed");
            }
        }

        [TestMethod()]
        public void Can_return_periods_since_single_2012()
        {
            DateTime starteDate = new DateTime(2012, 2, 1);
            DateTime today = new DateTime(2012, 6, 12);
            List<GPRAReportingTime> expected = new List<GPRAReportingTime>
            {
                new GPRAReportingTime(2011),
           };
            List<GPRAReportingTime> actual;
            actual = GPRAReportingTime.GetPeriodsSince(starteDate, today);
            Assert.AreEqual(expected.Count, actual.Count, "Count failed");
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual<DateTime>(expected[i].StartDate, actual[i].StartDate, "Start date failed");
                Assert.AreEqual<DateTime>(expected[i].EndDate, actual[i].EndDate, "End date failed");
            }
        }

        [TestMethod]
        public void Can_return_periods_Oct_Nov_in_2012()
        {

            DateTime starteDate = new DateTime(2012, 10, 11, 23, 15, 21, 375, DateTimeKind.Local);
            DateTime today = new DateTime(2012, 11, 13);
            List<GPRAReportingTime> expected = new List<GPRAReportingTime>
            {
                new GPRAReportingTime(2012),
           };
            List<GPRAReportingTime> actual;
            actual = GPRAReportingTime.GetPeriodsSince(starteDate, today);
            Assert.AreEqual(expected.Count, actual.Count, "Count failed");
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual<DateTime>(expected[i].StartDate, actual[i].StartDate, "Start date failed");
                Assert.AreEqual<DateTime>(expected[i].EndDate, actual[i].EndDate, "End date failed");
            }
        }


        /// <summary>
        ///A test for GPRAReportingTime Constructor
        ///</summary>
        [TestMethod()]
        public void Can_create_from_year()
        {
            int year = 2011;
            GPRAReportingTime expected = new GPRAReportingTime
            {
                StartDate = new DateTime(2011, 10, 1),
                EndDate = new DateTime(2012, 9, 30)
            };
            GPRAReportingTime actual = new GPRAReportingTime(year);
            Assert.AreEqual<DateTime>(expected.StartDate, actual.StartDate, "Start date failed");
            Assert.AreEqual<DateTime>(expected.EndDate, actual.EndDate, "End date failed");

        }

        /// <summary>
        ///A test for Label
        ///</summary>
        [TestMethod()]
        public void Has_valid_label()
        {
            GPRAReportingTime target = new GPRAReportingTime
            {
                StartDate = new DateTime(2012, 10, 1),
                EndDate = new DateTime(2013, 9, 30)
            };
            string expected = "10/01/2012 - 09/30/2013";
            string actual;
            actual = target.Label;
            Assert.AreEqual(expected, actual);
        }


        [TestMethod()]
        public void Can_return_current_May2012()
        {
            DateTime today = new DateTime(2012, 5, 15);
            GPRAReportingTime expected = new GPRAReportingTime
            {
                StartDate = new DateTime(2011, 10, 1),
                EndDate = new DateTime(2012, 9, 30)
            };
            GPRAReportingTime actual;
            actual = GPRAReportingTime.GetCurrent(today);
            Assert.AreEqual<DateTime>(expected.StartDate, actual.StartDate, "Start date failed");
            Assert.AreEqual<DateTime>(expected.EndDate, actual.EndDate, "End date failed");
        }

        [TestMethod()]
        public void Can_return_current_June302012()
        {
            DateTime today = new DateTime(2012, 6, 30);
            GPRAReportingTime expected = new GPRAReportingTime
            {
                StartDate = new DateTime(2011, 10, 1),
                EndDate = new DateTime(2012, 09, 30)
            };
            GPRAReportingTime actual;
            actual = GPRAReportingTime.GetCurrent(today);
            Assert.AreEqual<DateTime>(expected.StartDate, actual.StartDate, "Start date failed");
            Assert.AreEqual<DateTime>(expected.EndDate, actual.EndDate, "End date failed");
        }

        [TestMethod()]
        public void Can_return_current_July2012()
        {
            DateTime today = new DateTime(2012, 10, 1);
            GPRAReportingTime expected = new GPRAReportingTime
            {
                StartDate = new DateTime(2012, 10, 1),
                EndDate = new DateTime(2013, 9, 30)
            };
            GPRAReportingTime actual;
            actual = GPRAReportingTime.GetCurrent(today);
            Assert.AreEqual<DateTime>(expected.StartDate, actual.StartDate, "Start date failed");
            Assert.AreEqual<DateTime>(expected.EndDate, actual.EndDate, "End date failed");
        }





        #region Frequency logic tests
        /// <summary>
        ///A test for GetGPRAFrequencyEffectiveDate
        ///</summary>
        [TestMethod()]
        public void Can_calculate_montly_effective_date()
        {
            GPRAReportingTime target = new GPRAReportingTime(2011);
            DateTime currentDate = new DateTime(2012, 09, 20);
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
            DateTime currentDate = new DateTime(2012, 12, 4);
            int screeningFrequencyInMonths = 3;
            DateTime expected = new DateTime(2012, 10, 01);
            DateTime actual;
            actual = target.GetGPRAFrequencyEffectiveDateInMonths(currentDate, screeningFrequencyInMonths);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetGPRAFrequencyEffectiveDate
        ///</summary>
        [TestMethod()]
        public void Can_calculate_everyvisit_effective_date()
        {
            GPRAReportingTime target = new GPRAReportingTime(2011);
            DateTime currentDate = new DateTime(2012, 10, 4);
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
            DateTime currentDate = new DateTime(2012, 10, 4);
            int screeningFrequencyInMonths = 12;
            DateTime expected = new DateTime(2012, 10, 01);
            DateTime actual;
            actual = target.GetGPRAFrequencyEffectiveDateInMonths(currentDate, screeningFrequencyInMonths);
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
            DateTime currentDate = new DateTime(2012, 10, 4);
            int screeningFrequencyInMonths = 13;
            DateTime actual;
            actual = target.GetGPRAFrequencyEffectiveDateInMonths(currentDate, screeningFrequencyInMonths);

        }


        #endregion
    }
}

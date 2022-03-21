using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FrontDesk.Server.Screening.Models;
using System.Collections.Generic;
using FluentAssertions;

namespace FrontDesk.Server.Tests.Screening.Models
{
    [TestClass]
    public class IndicatorReportByAgeItemViewModelTest
    {

        protected IndicatorReportByAgeItemViewModel Sut()
        {
            return new IndicatorReportByAgeItemViewModel() { };
        }

        [TestMethod]
        public void TotalIsSumOfAllValues()
        {
            var sut = Sut();
            sut.PositiveScreensByAge = new Dictionary<int, long>
            {
                { 0, 1},
                { 10, 2},
                { 12, 3},
                { 18, 4},
                { 25, 3 },
                {55, 100 }
            };

            sut.Total.Should().Be(113);
        }

        [TestMethod]
        public void TotalIsZeroWhenAllZeros()
        {
            var sut = Sut();
            sut.PositiveScreensByAge = new Dictionary<int, long>
            {
                { 0, 0},
                { 10, 0},
                { 12, 0},
                { 18, 0},
                { 25, 0 },
                {55, 0 }
            };

            sut.Total.Should().Be(0);
        }
    }
}

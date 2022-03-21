using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FrontDesk.Common.Screening;
using FrontDesk.Server.Screening;
using FrontDesk.Server.Screening.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrontDesk.Server.Tests.Screening.Mappers
{
    [TestClass]
    public class IndicatorReportByAgeItemMapperForDepressionTests
    {
        private ScreeningAgesSettingsProvider _ageGroupsProvider = new ScreeningAgesSettingsProvider();
        private List<IndicatorReportByAgeItem> GetItems()
        {
            return new List<IndicatorReportByAgeItem>
            {
                new IndicatorReportByAgeItem
                {
                    ScreeningSectionID = ScreeningSectionDescriptor.DepressionPhq2ID,
                    ScreeningSectionQuestion = "NONE-MINIMAL depression severity",
                    PositiveCount = 1,
                    Age = 15,

                },

                new IndicatorReportByAgeItem
                {
                    ScreeningSectionID = ScreeningSectionDescriptor.DepressionPhq2ID,
                    ScreeningSectionQuestion = "NONE-MINIMAL depression severity",
                    PositiveCount = 1,
                    Age = 33,

                },
                 new IndicatorReportByAgeItem
                {
                    ScreeningSectionID = ScreeningSectionDescriptor.DepressionPhq2ID,
                    ScreeningSectionQuestion = "NONE-MINIMAL depression severity",
                    PositiveCount = 1,
                    Age = 36,

                },
                  new IndicatorReportByAgeItem
                {
                    ScreeningSectionID = ScreeningSectionDescriptor.DepressionPhq2ID,
                    ScreeningSectionQuestion = "NONE-MINIMAL depression severity",
                    PositiveCount = 1,
                    Age = 38,

                },

                new IndicatorReportByAgeItem
                {
                    ScreeningSectionID = ScreeningSectionDescriptor.Depression,
                    ScreeningSectionQuestion = "NONE-MINIMAL depression severity",
                    QuestionID = 0,
                    PositiveCount = 1,
                    Age = 36,

                },
                new IndicatorReportByAgeItem
                {
                    ScreeningSectionID = ScreeningSectionDescriptor.Depression,
                    ScreeningSectionQuestion = "NONE-MINIMAL depression severity",
                    QuestionID = 0,
                    PositiveCount = 0,
                    Age = 37,

                },

                new IndicatorReportByAgeItem
                {
                    ScreeningSectionID = ScreeningSectionDescriptor.Depression,
                    ScreeningSectionQuestion = "MILD depression severity",
                    QuestionID = 1,
                    PositiveCount = 1,
                    Age = 15,

                },


            };
        }



        [TestMethod]
        public void Depression_WhenPositiveResultsModelNotEmpty()
        {
            var actual = GetItems().ToViewModel(_ageGroupsProvider.AgeGroups);

            actual.Should().NotBeEmpty();
        }

        [TestMethod]
        public void Depression_WhenAllGroups_CountIsCorrect()
        {
            var actual = GetItems().ToViewModel(_ageGroupsProvider.AgeGroups).ToList();

            actual.Count.Should().Be(3);
        }


        [TestMethod]
        public void Depression_WhenAllGroups_PositiveCountsForPhq2_AreCorrect()
        {
            var actual = GetItems().ToViewModel(_ageGroupsProvider.AgeGroups).First(x => x.ScreeningSectionID == ScreeningSectionDescriptor.DepressionPhq2ID);

            actual.PositiveScreensByAge.Should().NotBeEmpty();
            actual.PositiveScreensByAge.Should().BeEquivalentTo(new Dictionary<int, long>
            {
                {0, 0}, {15, 1}, {18, 0}, {26, 3}, {55, 0}
            });
        }


        [TestMethod]
        public void Depression_WhenAllGroups_PositiveCounts_ForNone_ForPhq9_AreCorrect()
        {
            var actual = GetItems().ToViewModel(_ageGroupsProvider.AgeGroups).First(x => x.ScreeningSectionID == ScreeningSectionDescriptor.Depression);

            actual.PositiveScreensByAge.Should().NotBeEmpty();
            actual.PositiveScreensByAge.Should().BeEquivalentTo(new Dictionary<int, long>
            {
                {0, 0}, {15, 0}, {18, 0}, {26, 1}, {55, 0}
            });
        }

        [TestMethod]
        public void Depression_WhenAllGroups_PositiveCounts_ForMild_ForPhq9_AreCorrect()
        {
            var actual = GetItems().ToViewModel(_ageGroupsProvider.AgeGroups).First(x => x.ScreeningSectionID == ScreeningSectionDescriptor.Depression && x.ScreeningSectionQuestion.StartsWith("MILD"));

            actual.PositiveScreensByAge.Should().NotBeEmpty();
            actual.PositiveScreensByAge.Should().BeEquivalentTo(new Dictionary<int, long>
            {
                {0, 0}, {15, 1}, {18, 0}, {26, 0}, {55, 0}
            });
        }
    }
}

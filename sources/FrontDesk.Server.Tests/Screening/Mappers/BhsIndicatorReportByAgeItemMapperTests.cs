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
    public class BhsIndicatorReportByAgeItemMapperTests
    {
        private ScreeningAgesSettingsProvider _ageGroupsProvider = new ScreeningAgesSettingsProvider();
        private List<BhsIndicatorReportByAgeItem> GetItems()
        {
            return new List<BhsIndicatorReportByAgeItem>
            {
                new BhsIndicatorReportByAgeItem
                {
                    CategoryID = "Race",
                    IndicatorID = 1,
                    IndicatorName = "Alaska Native",
                    Count = 10,
                    Age = 14,

                },

               new BhsIndicatorReportByAgeItem
                {
                    CategoryID = "Race",
                    IndicatorID = 1,
                    IndicatorName = "Alaska Native",
                    Count = 3,
                    Age = 18,

                },

                new BhsIndicatorReportByAgeItem
                {
                    CategoryID = "Race",
                    IndicatorID = 1,
                    IndicatorName = "Alaska Native",
                    Count = 12,
                    Age = 55,

                },
                new BhsIndicatorReportByAgeItem
                {
                    CategoryID = "Race",
                    IndicatorID = 2,
                    IndicatorName = "American Indian",
                    Count = 5,
                    Age = 34,

                },
                new BhsIndicatorReportByAgeItem
                {
                    CategoryID = "Race",
                    IndicatorID = 2,
                    IndicatorName = "American Indian",
                    Count = 5,
                    Age = 54,

                },

                new BhsIndicatorReportByAgeItem
                {
                    CategoryID = "Race",
                    IndicatorID = 2,
                    IndicatorName = "American Indian",
                    Count = 19,
                    Age = 3,

                },
                new BhsIndicatorReportByAgeItem
                {
                    CategoryID = "Race",
                    IndicatorID = 2,
                    IndicatorName = "American Indian",
                    Count = 19,
                    Age = 3,

                },

                new BhsIndicatorReportByAgeItem
                {
                    CategoryID = "Gender",
                    IndicatorID = 1,
                    IndicatorName = "Male",
                    Count = 5,
                    Age = 54,

                },
                new BhsIndicatorReportByAgeItem
                {
                    CategoryID = "SexualOrientation",
                    IndicatorID = 2,
                    IndicatorName = "Lesbian",
                    Count = 35,
                    Age = 13,

                },
            };
        }

        [TestMethod]
        public void BhsIndicatorReport_Should_NotBeEmpty()
        {
            var actual = GetItems().ToViewModel(_ageGroupsProvider.AgeGroups);

            actual.Should().NotBeEmpty();
        }

        [TestMethod]
        public void BhsIndicatorReport_WhenAllGroups_CountIsCorrect()
        {
            var actual = GetItems().ToViewModel(_ageGroupsProvider.AgeGroups).ToList();

            actual.Count.Should().Be(4);
        }

        [TestMethod]
        public void BhsIndicatorReport_WhenAllGroups_Should_BeTwoGroupsForRace()
        {
            var actual = GetItems().ToViewModel(_ageGroupsProvider.AgeGroups).Where(x => x.CategoryID == "Race").ToList();

            actual.Count.Should().Be(2);
            actual[0].IndicatorId.Should().Be(1, "first group should be for RaceID eq to 2");
            actual[1].IndicatorId.Should().Be(2, "second group should be for RaceID eq to 2");

        }
        

        [TestMethod]
        public void WhenAllGroups_AllGroupsAreSetFor_Race_AlaskaNative()
        {
            var actual = GetItems().ToViewModel(_ageGroupsProvider.AgeGroups).First(x => x.CategoryID ==  "Race");

            actual.TotalByAge.Keys.Should().BeEquivalentTo(new[] { 0, 15, 18, 26, 55 });
        }

        [TestMethod]
        public void BhsIndicatorReport_WhenAllGroups_Count_For_Race_AlaskaNative_Correct()
        {
            var actual = GetItems().ToViewModel(_ageGroupsProvider.AgeGroups).First(x => x.CategoryID == "Race");

            actual.TotalByAge.Should().NotBeEmpty();
            actual.TotalByAge.Should().BeEquivalentTo(new Dictionary<int, long>
            {
                {0, 10}, {15, 0}, {18, 3}, {26, 0}, {55, 12}
            });
        }

        [TestMethod]
        public void BhsIndicatorReport_WhenAllGroups_Count_For_Gender_Correct()
        {

            var actual = GetItems().ToViewModel(_ageGroupsProvider.AgeGroups).First(x => x.CategoryID == "Gender");

            actual.TotalByAge.Should().NotBeEmpty();
            actual.TotalByAge.Should().BeEquivalentTo(new Dictionary<int, long>
            {
                {0, 0}, {15, 0}, {18, 0}, {26, 5}, {55, 0}
            });
        }

        
    }
}

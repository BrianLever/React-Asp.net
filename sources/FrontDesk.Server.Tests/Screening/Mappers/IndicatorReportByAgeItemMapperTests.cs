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
    public class IndicatorReportByAgeItemMapperTests
    {
        private ScreeningAgesSettingsProvider _ageGroupsProvider = new ScreeningAgesSettingsProvider();
        private List<IndicatorReportByAgeItem> GetItems()
        {
            return new List<IndicatorReportByAgeItem>
            {
                new IndicatorReportByAgeItem
                {
                    ScreeningSectionID = ScreeningSectionDescriptor.SmokerInHome,
                    ScreeningSectionQuestion = "Does anyone in the home smoke tobacco (such as cigarettes, cigars, pipes, electronic nicotine delivery devices, etc.)?",
                    PositiveCount = 1,
                    Age = 10,

                },
               
                new IndicatorReportByAgeItem
                {
                    ScreeningSectionID = ScreeningSectionDescriptor.SmokerInHome,
                    ScreeningSectionQuestion = "Does anyone in the home smoke tobacco (such as cigarettes, cigars, pipes, electronic nicotine delivery devices, etc.)?",
                    PositiveCount = 11,
                    Age = 15,

                },
                
                new IndicatorReportByAgeItem
                {
                    ScreeningSectionID = ScreeningSectionDescriptor.SmokerInHome,
                    ScreeningSectionQuestion = "Does anyone in the home smoke tobacco (such as cigarettes, cigars, pipes, electronic nicotine delivery devices, etc.)?",
                    PositiveCount = 5,
                    Age = 18,

                },
                new IndicatorReportByAgeItem
                {
                    ScreeningSectionID = ScreeningSectionDescriptor.SmokerInHome,
                    ScreeningSectionQuestion = "Does anyone in the home smoke tobacco (such as cigarettes, cigars, pipes, electronic nicotine delivery devices, etc.)?",
                    PositiveCount = 4,
                    Age = 24,

                },
                new IndicatorReportByAgeItem
                {
                    ScreeningSectionID = ScreeningSectionDescriptor.SmokerInHome,
                    ScreeningSectionQuestion = "Does anyone in the home smoke tobacco (such as cigarettes, cigars, pipes, electronic nicotine delivery devices, etc.)?",
                    PositiveCount = 12,
                    Age = 55,

                },

                 new IndicatorReportByAgeItem
                {
                    ScreeningSectionID = ScreeningSectionDescriptor.SmokerInHome,
                    ScreeningSectionQuestion = "Does anyone in the home smoke tobacco (such as cigarettes, cigars, pipes, electronic nicotine delivery devices, etc.)?",
                    PositiveCount = 16,
                    Age = 57,

                },


                new IndicatorReportByAgeItem
                {
                    ScreeningSectionID = ScreeningSectionDescriptor.SmokerInHome,
                    ScreeningSectionQuestion = "Does anyone in the home smoke tobacco (such as cigarettes, cigars, pipes, electronic nicotine delivery devices, etc.)?",
                    PositiveCount = 5,
                    Age = 19,

                },
                new IndicatorReportByAgeItem
                {
                    ScreeningSectionID = ScreeningSectionDescriptor.Tobacco,
                    ScreeningSectionQuestion = "Do you use tobacco?",
                    PositiveCount = 4,
                    Age = 14,

                },
                new IndicatorReportByAgeItem
                {
                    ScreeningSectionID = ScreeningSectionDescriptor.Tobacco,
                    ScreeningSectionQuestion = "Do you use tobacco?",
                    PositiveCount = 12,
                    Age = 15,

                },

                 new IndicatorReportByAgeItem
                {
                    ScreeningSectionID = ScreeningSectionDescriptor.PartnerViolence,
                    ScreeningSectionQuestion = "Do you feel UNSAFE in your home?",
                    PositiveCount = 3,
                    Age = 27,

                },


            };
        }



        [TestMethod]
        public void WhenPositiveResultsModelNotEmpty()
        {
            var actual = GetItems().ToViewModel(_ageGroupsProvider.AgeGroups);

            actual.Should().NotBeEmpty();
        }

        [TestMethod]
        public void WhenAllGroups_CountIsCorrect()
        {
            var actual = GetItems().ToViewModel(_ageGroupsProvider.AgeGroups).ToList();

            actual.Count.Should().Be(3);
        }


        [TestMethod]
        public void WhenAllGroups_AllGroupsAreSetForSmokerInHome()
        {
            var actual = GetItems().ToViewModel(_ageGroupsProvider.AgeGroups).First(x => x.ScreeningSectionID == ScreeningSectionDescriptor.SmokerInHome);
            
            actual.PositiveScreensByAge.Keys.Should().BeEquivalentTo(new[] { 0, 15, 18, 26, 55 });
        }

        [TestMethod]
        public void WhenAllGroups_PositiveCountsForSmokerInHomeCorrect()
        {
            var actual = GetItems().ToViewModel(_ageGroupsProvider.AgeGroups).First(x => x.ScreeningSectionID == ScreeningSectionDescriptor.SmokerInHome);
            
            actual.PositiveScreensByAge.Should().NotBeEmpty();
            actual.PositiveScreensByAge.Should().BeEquivalentTo(new Dictionary<int, long>
            {
                {0, 1}, {15, 11}, {18, 14}, {26, 0}, {55, 28}
            });
        }

        [TestMethod]
        public void WhenAllGroups_PositiveCountsForTobaccoCorrect()
        {

            var actual = GetItems().ToViewModel(_ageGroupsProvider.AgeGroups).First(x => x.ScreeningSectionID == ScreeningSectionDescriptor.Tobacco);
            
            actual.PositiveScreensByAge.Should().NotBeEmpty();
            actual.PositiveScreensByAge.Should().BeEquivalentTo(new Dictionary<int, long>
            {
                {0, 4}, {15, 12}, {18, 0}, {26, 0}, {55, 0}
            });
        }

        [TestMethod]
        public void WhenAllGroups_PositiveCountsForPartnerViolanceCorrect()
        {
            var actual = GetItems().ToViewModel(_ageGroupsProvider.AgeGroups).First(x => x.ScreeningSectionID == ScreeningSectionDescriptor.PartnerViolence);
            
            actual.PositiveScreensByAge.Should().NotBeEmpty();
            actual.PositiveScreensByAge.Should().BeEquivalentTo(new Dictionary<int, long>
            {
                {0, 0}, {15, 0}, {18, 0}, {26, 3}, {55, 0}
            });
        }
    }
}

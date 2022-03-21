using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FrontDesk.Common.Screening;
using FluentAssertions;

namespace FrontDesk.Common.Tests.Screening
{
    [TestClass]
    public class ScreeningAgesSettingsProviderTest
    {
        [TestMethod]
        public void ScreeningAgesSettingsProvider_Parse_AppSettingString()
        {
            var sut = new ScreeningAgesSettingsProvider("0 - 14; 15 - 17; 18 - 25; 26 - 54; 55 or Older");

            sut.AgeGroupsLabels.Should().NotBeEmpty();
        }

        [TestMethod]
        public void ScreeningAgesSettingsProvider_AgeGroupsLabels_NumberOfGroupsCorrect()
        {
            var sut = new ScreeningAgesSettingsProvider("0 - 9; 10 - 11; 12 - 17; 18 - 24; 25 - 54; 55 or Older");

            sut.AgeGroupsLabels.Length.Should().Be(6);
        }

        [TestMethod]
        public void ScreeningAgesSettingsProvider_AgeGroupsLabels_ParsedWithoutLeadingSpace()
        {
            var sut = new ScreeningAgesSettingsProvider("0 - 9; 10 - 11; 12 - 17; 18 - 24; 25 - 54; 55 or Older");

            sut.AgeGroupsLabels.Should().BeEquivalentTo(new[] { "0 - 9", "10 - 11", "12 - 17", "18 - 24", "25 - 54", "55 or Older"});
        }

        [TestMethod]
        public void ScreeningAgesSettingsProvider_AgeGroups_WithLeadingSpace_ParsedCorrectly()
        {
            var sut = new ScreeningAgesSettingsProvider("0 - 9; 10 - 11; 12 - 17; 18 - 24; 25 - 54; 55 or Older");

            sut.AgeGroups.Should().BeEquivalentTo(new[] { 0, 10, 12, 18, 25, 55 });
        }

        [TestMethod]
        public void ScreeningAgesSettingsProvider_AgeGroups_WithoutSpaces_ParsedCorrectly()
        {
            var sut = new ScreeningAgesSettingsProvider("0-9;10-11;12-17;18-24;25-54;55 or Older");

            sut.AgeGroups.Should().BeEquivalentTo(new[] { 0, 10, 12, 18, 25, 55 });
        }
    }
}

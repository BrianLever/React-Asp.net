using FluentAssertions;
using FrontDesk.Server.Screening.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontDesk.Server.Tests.Screening.Models
{
    [TestClass]
    public class ScreeningsByScoreLevelCountResultTest
    {
        public ScreeningsByScoreLevelCountResult Sut()
        {
            return new ScreeningsByScoreLevelCountResult();
        }

        [TestMethod]
        public void CanInsertOneResult()
        {
            var sut = Sut().Insert("TCC", 1, 10);

            sut.Results.Count.Should().Be(1);
            sut.Results[0].SectionID.Should().Be("TCC");
        }

        [TestMethod]
        public void CanInsertSeveralResultInSameSection()
        {
            var sut = Sut()
                .Insert("TCC", 1, 10)
                .Insert("TCC", 2, 11); 

            sut.Results.Count.Should().Be(1);
            sut.Results[0].ScoreLevelCount.Keys.Count.Should().Be(2);
        }

        [TestMethod]
        public void WhenTwoSectionsHasSeveralResultsRecords()
        {
            var sut = Sut()
                .Insert("TCC", 1, 10)
                .Insert("DAST-10", 1, 3)
                .Insert("TCC", 2, 11);

            sut.Results.Count.Should().Be(2);
        }

        [TestMethod]
        public void WhenUpdatingSameScoreTwiceSecondValueWins()
        {
            var sut = Sut()
                .Insert("TCC", 1, 10)
                .Insert("DAST-10", 1, 3)
                .Insert("TCC", 1, 11);

            sut.Results[0].ScoreLevelCount[1].Should().Be(11);
        }

        [TestMethod]
        public void WhenExistsReturnResult()
        {
            var sut = Sut()
                .Insert("TCC", 1, 10);

            sut.GetSection("TCC").SectionID.Should().Be("TCC");
        }

        [TestMethod]
        public void WhenEmptyReturnsNull()
        {
            var sut = Sut();

            sut.GetSection("TCC").Should().BeNull();
        }

        [TestMethod]
        public void WhenNotExistsReturnsNull()
        {
            var sut = Sut()
                 .Insert("TCC", 1, 10);

            sut.GetSection("DAST-1)").Should().BeNull();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using FrontDesk.Server.Data;
using FrontDesk.Server.Screening;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Screening.Services;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace FrontDesk.Server.Tests.Services
{
    [TestClass]
    public class ScreeningScoreLevelServiceTests
    {
        private readonly Mock<IScreeningScoreRepository> _repositoryMock = new Mock<IScreeningScoreRepository>();


        public ScreeningScoreLevelServiceTests()
        {
            var scoreList = new List<ScreeningScoreLevel>
            {
                new ScreeningScoreLevel{ScreeningSectionID = "TCC", ScoreLevel = 0, Label = "Negative" },
                new ScreeningScoreLevel{ScreeningSectionID = "TCC", ScoreLevel = 1, Label = "Positive" },
                new ScreeningScoreLevel{ScreeningSectionID = "CAGE", ScoreLevel = 0, Label = "Negative" },
                new ScreeningScoreLevel{ScreeningSectionID = "CAGE", ScoreLevel = 1, Label = "At Risk" },
                new ScreeningScoreLevel{ScreeningSectionID = "CAGE", ScoreLevel = 2, Label = "Current Problem" },
                new ScreeningScoreLevel{ScreeningSectionID = "CAGE", ScoreLevel = 3, Label = "Dependence" },
                new ScreeningScoreLevel{ScreeningSectionID = "PHQ-9", ScoreLevel = 0, Label = "None-Minimal" },
                new ScreeningScoreLevel{ScreeningSectionID = "PHQ-9", ScoreLevel = 2, Label = "Mild" },
                new ScreeningScoreLevel{ScreeningSectionID = "PHQ-9", ScoreLevel = 3, Label = "Moderate" },
                new ScreeningScoreLevel{ScreeningSectionID = "PHQ-9", ScoreLevel = 4, Label = "Moderately Severe" },
                new ScreeningScoreLevel{ScreeningSectionID = "PHQ-9", ScoreLevel = 5, Label = "Severe" },
                new ScreeningScoreLevel{ScreeningSectionID = "HITS", ScoreLevel = 0, Label = "Negative" },
                new ScreeningScoreLevel{ScreeningSectionID = "HITS", ScoreLevel = 1, Label = "Current Problem" },
            };

            _repositoryMock.Setup(x => x.GetAllScoreLevels()).Returns(scoreList);

            _repositoryMock.Setup(x => x.GetScoreLevelsToQuestion("PHQ-9", 9)).Returns(new List<ScreeningScoreLevel>
            {
                new ScreeningScoreLevel{ScreeningSectionID = VisitSettingsDescriptor.DepressionThinkOfDeath, ScoreLevel = 1, Label = "Several days" },
                new ScreeningScoreLevel{ScreeningSectionID = VisitSettingsDescriptor.DepressionThinkOfDeath, ScoreLevel = 2, Label = "More than half the days" },
                new ScreeningScoreLevel{ScreeningSectionID = VisitSettingsDescriptor.DepressionThinkOfDeath, ScoreLevel = 3, Label = "Nearly every day" },
            });
            
        }

        protected ScreeningScoreLevelService Sut()
        {
            return new ScreeningScoreLevelService(_repositoryMock.Object);
        }

        [TestMethod]
        public void GetAllScoreHints_NotEmpty()
        {
            Sut().GetAllScoreHints().Should().NotBeEmpty();
        }

        [TestMethod]
        public void GetAllScoreHints_When_Tobacco_Returns_Empty()
        {
            var actual = Sut().GetAllScoreHints().First(x => x.ScreeningSectionID == ScreeningSectionDescriptor.Tobacco);

            actual.ScoreHint.Should().BeEmpty("screen tool has 0 or 1 measures");
        }

        [TestMethod]
        public void GetAllScoreHints_When_Alcohol_Returns_ValidHint()
        {
            var actual = Sut().GetAllScoreHints().First(x => x.ScreeningSectionID == ScreeningSectionDescriptor.Alcohol);

            actual.ScoreHint.Should().Be("1 = At Risk, 2 = Current Problem, 3-4 = Dependence");
        }

        [TestMethod]
        public void GetAllScoreHints_When_Depression_Returns_ValidHint()
        {
            var actual = Sut().GetAllScoreHints().First(x => x.ScreeningSectionID == ScreeningSectionDescriptor.Depression);

            actual.ScoreHint.Should().Be("1-4 = None-Minimal, 5-9 = Mild, 10-14 = Moderate, 15-19 = Moderately Severe, 20-27 = Severe");
        }

        [TestMethod]
        public void GetAllScoreHints_When_Violence_Returns_ValidHint()
        {
            var actual = Sut().GetAllScoreHints().First(x => x.ScreeningSectionID == ScreeningSectionDescriptor.PartnerViolence);

            actual.ScoreHint.Should().Be("1-10 = Negative, 11 or greater = Current Problem");
        }


        [TestMethod]
        public void GetAllScoreHints_When_SucideDepressionQuestion_Returns_ValidHint()
        {
            var actual = Sut().GetAllScoreHints().First(x => x.ScreeningSectionID == VisitSettingsDescriptor.DepressionThinkOfDeath);

            actual.ScoreHint.Should().Be("1 = Several Days, 2 = More Than Half The Days, 3 = Nearly Every Day");
        }
    }
}

using FluentAssertions;

using FrontDesk.Common.InfrastructureServices;
using FrontDesk.Server.Data.ScreeningProfile;
using FrontDesk.Server.Screening.Services;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using System;
using System.Collections.Generic;
using System.Linq;

namespace FrontDesk.Server.Tests.Services
{
    [TestClass]
    public class ScreeningProfileMinimalAgeServiceGroupTests
    {
        private readonly Mock<IScreeningProfileMinimalAgeRepository> _repository = new Mock<IScreeningProfileMinimalAgeRepository>();
        private readonly Mock<ITimeService> _timeServiceMock = new Mock<ITimeService>();

        public ScreeningProfileMinimalAgeServiceGroupTests()
        {
            _timeServiceMock.Setup(x => x.GetUtcNow()).Returns(new DateTime(2020, 08, 06, 0,0,0, DateTimeKind.Utc));
        }

        protected ScreeningProfileMinimalAgeService Sut()
        {
            return new ScreeningProfileMinimalAgeService(_repository.Object, _timeServiceMock.Object);
        }

        [TestMethod]
        public void ScreeningMinimalAgeService_DisablesAlternatives_When_BothEnables()
        {
            List<ScreeningSectionAge> result = null;

            _repository
                .Setup(x => x.UpdateMinimalAgeSettings(It.IsAny<int>(), It.IsAny<IEnumerable<ScreeningSectionAge>>()))
                .Callback<int, IEnumerable<ScreeningSectionAge>>((id, settings) => result = settings.ToList());

            var ageSettings = new List<ScreeningSectionAge>()
            {
                new ScreeningSectionAge
                {
                    ScreeningSectionID=ScreeningSectionDescriptor.Depression,
                    IsEnabled = true
                },
                new ScreeningSectionAge
                {
                    ScreeningSectionID=ScreeningSectionDescriptor.DepressionAllQuestions,
                    IsEnabled = true
                },
            };



            Sut().UpdateMinimalAgeSettings(ScreeningProfile.DefaultProfileID, ageSettings);

            result.First().IsEnabled.Should().BeTrue();
            result.Last().IsEnabled.Should().BeFalse();

        }

        [TestMethod]
        public void ScreeningMinimalAgeService_PrimaryAndAlternative_CanBeDisabled()
        {
            List<ScreeningSectionAge> result = null;

            _repository
                .Setup(x => x.UpdateMinimalAgeSettings(ScreeningProfile.DefaultProfileID, It.IsAny<IEnumerable<ScreeningSectionAge>>()))
               .Callback<int, IEnumerable<ScreeningSectionAge>>((id, settings) => result = settings.ToList());

            var ageSettings = new List<ScreeningSectionAge>()
            {
                new ScreeningSectionAge
                {
                    ScreeningSectionID=ScreeningSectionDescriptor.Depression,
                    IsEnabled = false,
                },
                new ScreeningSectionAge
                {
                    ScreeningSectionID=ScreeningSectionDescriptor.DepressionAllQuestions,
                    IsEnabled = false,
                },
            };



            Sut().UpdateMinimalAgeSettings(ScreeningProfile.DefaultProfileID, ageSettings);

            result.First().IsEnabled.Should().BeFalse();
            result.Last().IsEnabled.Should().BeFalse();
        }

        [TestMethod]
        public void ScreeningMinimalAgeService_WhenPrimaryIsEnabled()
        {
            List<ScreeningSectionAge> result = null;

            _repository
                .Setup(x => x.UpdateMinimalAgeSettings(ScreeningProfile.DefaultProfileID, It.IsAny<IEnumerable<ScreeningSectionAge>>()))
                .Callback<int, IEnumerable<ScreeningSectionAge>>((id, settings) => result = settings.ToList());

            var ageSettings = new List<ScreeningSectionAge>()
            {
                new ScreeningSectionAge
                {
                    ScreeningSectionID=ScreeningSectionDescriptor.Depression,
                    IsEnabled = true,
                },
                new ScreeningSectionAge
                {
                    ScreeningSectionID=ScreeningSectionDescriptor.DepressionAllQuestions,
                    IsEnabled = false,
                },
            };

            Sut().UpdateMinimalAgeSettings(ScreeningProfile.DefaultProfileID, ageSettings);

            result.First().IsEnabled.Should().Be(ageSettings.First().IsEnabled);
            result.Last().IsEnabled.Should().Be(ageSettings.Last().IsEnabled);
        }

        [TestMethod]
        public void ScreeningMinimalAgeService_WhenAlternativeIsEnabled()
        {
            List<ScreeningSectionAge> result = null;

            _repository
                .Setup(x => x.UpdateMinimalAgeSettings(ScreeningProfile.DefaultProfileID, It.IsAny<IEnumerable<ScreeningSectionAge>>()))
                .Callback<int, IEnumerable<ScreeningSectionAge>>((id, settings) => result = settings.ToList());

            var ageSettings = new List<ScreeningSectionAge>()
            {
                new ScreeningSectionAge
                {
                    ScreeningSectionID=ScreeningSectionDescriptor.Depression,
                    IsEnabled = false,
                },
                new ScreeningSectionAge
                {
                    ScreeningSectionID=ScreeningSectionDescriptor.DepressionAllQuestions,
                    IsEnabled = true,
                },
            };

            Sut().UpdateMinimalAgeSettings(ScreeningProfile.DefaultProfileID, ageSettings);

            result.First().IsEnabled.Should().Be(ageSettings.First().IsEnabled);
            result.Last().IsEnabled.Should().Be(ageSettings.Last().IsEnabled);
        }



        [TestMethod]
        public void ScreeningMinimalAgeService_DepententSections_HasZeroInMinimalAge()
        {
            List<ScreeningSectionAge> result = null;

            _repository
                .Setup(x => x.UpdateMinimalAgeSettings(ScreeningProfile.DefaultProfileID, It.IsAny<IEnumerable<ScreeningSectionAge>>()))
                .Callback<int, IEnumerable<ScreeningSectionAge>>((id, settings) => result = settings.ToList());

            var ageSettings = new List<ScreeningSectionAge>()
            {
                new ScreeningSectionAge
                {
                    ScreeningSectionID=ScreeningSectionDescriptor.Depression,
                    IsEnabled = false,
                    MinimalAge = 14
                },
                new ScreeningSectionAge
                {
                    ScreeningSectionID=ScreeningSectionDescriptor.DepressionAllQuestions,
                    IsEnabled = true,
                    MinimalAge = 15
                },
            };

            Sut().UpdateMinimalAgeSettings(ScreeningProfile.DefaultProfileID, ageSettings);

            result.First().MinimalAge.Should().Be(ageSettings.First().MinimalAge);
            result.Last().MinimalAge.Should().Be(14);
        }
    }
}

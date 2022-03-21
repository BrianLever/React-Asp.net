using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FrontDesk.Data;
using FrontDesk.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FrontDesk.Tests.Services
{
    [TestClass]
    public class ScreeningSectionAgeServiceGroupTests
    {
        private readonly Mock<IScreeningKioskDb> _screeningKioskDb = new Mock<IScreeningKioskDb>();

        protected ScreeningSectionAgeService Sut()
        {
            return new ScreeningSectionAgeService(_screeningKioskDb.Object);
        }

        [TestMethod]
        public void ScreeningSectionAgeService_KeepAlternatives_AsInContract()
        {
            ICollection<ScreeningSectionAge> result = null;

            _screeningKioskDb.Setup(x => x.UpdateAgeSettings(It.IsAny<ICollection<ScreeningSectionAge>>())).
                Callback<ICollection<ScreeningSectionAge>>(x => result = x);

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



            Sut().UpdateAgeSettings(ageSettings);

            result.First().IsEnabled.Should().BeTrue();
            result.Last().IsEnabled.Should().BeTrue();

        }

        [TestMethod]
        public void ScreeningSectionAgeService_DependentSections_HasMinimalAgeAsProvided()
        {
            ICollection<ScreeningSectionAge> result = null;

            _screeningKioskDb.Setup(x => x.UpdateAgeSettings(It.IsAny<ICollection<ScreeningSectionAge>>())).
               Callback<ICollection<ScreeningSectionAge>>(x => result = x);

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

            Sut().UpdateAgeSettings(ageSettings);

            result.First().MinimalAge.Should().Be(14);
            result.Last().MinimalAge.Should().Be(15);
        }



        [TestMethod]
        public void ScreeningSectionAgeService_KeepAlternativesAnxiety_AsInContract()
        {
            ICollection<ScreeningSectionAge> result = null;

            _screeningKioskDb.Setup(x => x.UpdateAgeSettings(It.IsAny<ICollection<ScreeningSectionAge>>())).
                Callback<ICollection<ScreeningSectionAge>>(x => result = x);

            var ageSettings = new List<ScreeningSectionAge>()
            {
                new ScreeningSectionAge
                {
                    ScreeningSectionID=ScreeningSectionDescriptor.Anxiety,
                    IsEnabled = true
                },
                new ScreeningSectionAge
                {
                    ScreeningSectionID=ScreeningSectionDescriptor.AnxietyAllQuestions,
                    IsEnabled = true
                },
            };

            Sut().UpdateAgeSettings(ageSettings);

            result.First().IsEnabled.Should().BeTrue();
            result.Last().IsEnabled.Should().BeTrue();

        }
    }
}

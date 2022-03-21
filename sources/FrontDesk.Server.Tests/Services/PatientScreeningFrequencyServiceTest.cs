using FluentAssertions;

using FrontDesk.Configuration;
using FrontDesk.Server.Data;
using FrontDesk.Server.Data.ScreenngProfile;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using System;
using System.Collections.Generic;

namespace FrontDesk.Server.Tests
{

    [TestClass()]
    public class PatientScreeningFrequencyServiceTest
    {
        Mock<IPatientScreeningFrequencyRepository> _patientRepository;
        Mock<IScreeningProfileFrequencyRepository> _settingsRepository;


        public PatientScreeningFrequencyServiceTest()
        {
            _settingsRepository = new Mock<IScreeningProfileFrequencyRepository>();
            _settingsRepository.Setup(x => x.GetAll(It.IsAny<int>())).Returns(GetFrequencyItems());


            _patientRepository = new Mock<IPatientScreeningFrequencyRepository>();

            _patientRepository.Setup(x => x.GetPatientContactInfoScreeningCount(It.IsAny<ScreeningPatientIdentity>(), It.IsAny<DateTimeOffset>())).Returns(2);
            //_patientRepository.Setup(x => x.GetPatientSectionScreeningCount(It.IsAny<ScreeningPatientIdentity>(), It.Is<string>(p => p == ScreeningSectionDescriptor.SmokerInHome), It.IsAny<DateTimeOffset>())).Returns(0);
            _patientRepository.Setup(x => x.GetPatientSectionScreeningCount(It.IsAny<ScreeningPatientIdentity>(), It.Is<string>(p => p == ScreeningSectionDescriptor.Tobacco), It.IsAny<DateTimeOffset>())).Returns(1);
            _patientRepository.Setup(x => x.GetPatientSectionScreeningCount(It.IsAny<ScreeningPatientIdentity>(), It.Is<string>(p => p == ScreeningSectionDescriptor.Alcohol), It.IsAny<DateTimeOffset>())).Returns(2);
            _patientRepository.Setup(x => x.GetPatientSectionScreeningCount(It.IsAny<ScreeningPatientIdentity>(), It.Is<string>(p => p == ScreeningSectionDescriptor.Depression), It.IsAny<DateTimeOffset>())).Returns(3);
            _patientRepository.Setup(x => x.GetPatientSectionScreeningCount(It.IsAny<ScreeningPatientIdentity>(), It.Is<string>(p => p == ScreeningSectionDescriptor.PartnerViolence), It.IsAny<DateTimeOffset>())).Returns(4);
            _patientRepository.Setup(x => x.GetPatientSectionScreeningCount(It.IsAny<ScreeningPatientIdentity>(), It.Is<string>(p => p == ScreeningSectionDescriptor.SubstanceAbuse), It.IsAny<DateTimeOffset>())).Returns(5);
            _patientRepository.Setup(x => x.GetPatientSectionScreeningCount(It.IsAny<ScreeningPatientIdentity>(), It.Is<string>(p => p == ScreeningSectionDescriptor.DrugOfChoice), It.IsAny<DateTimeOffset>())).Returns(6);
            _patientRepository.Setup(x => x.GetPatientSectionScreeningCount(It.IsAny<ScreeningPatientIdentity>(), It.Is<string>(p => p == ScreeningSectionDescriptor.Anxiety), It.IsAny<DateTimeOffset>())).Returns(8);


        }

        private List<ScreeningFrequencyItem> GetFrequencyItems()
        {
            return new List<ScreeningFrequencyItem>()
            {
                new ScreeningFrequencyItem{ ID = ScreeningFrequencyDescriptor.ContactFrequencyID, Frequency = 100},
                new ScreeningFrequencyItem{ ID = ScreeningSectionDescriptor.SmokerInHome, Frequency = 0},
                new ScreeningFrequencyItem{ ID = ScreeningSectionDescriptor.Tobacco, Frequency = 300},
                new ScreeningFrequencyItem{ ID = ScreeningSectionDescriptor.Alcohol, Frequency = 200},
                new ScreeningFrequencyItem{ ID = ScreeningSectionDescriptor.Depression, Frequency = 1200},
                new ScreeningFrequencyItem{ ID = ScreeningSectionDescriptor.PartnerViolence, Frequency = 240000},
                new ScreeningFrequencyItem{ ID = ScreeningSectionDescriptor.SubstanceAbuse, Frequency = 7}, // weekly
                new ScreeningFrequencyItem{ ID = ScreeningSectionDescriptor.DrugOfChoice, Frequency = 1}, // daily
                new ScreeningFrequencyItem{ ID = ScreeningSectionDescriptor.Anxiety, Frequency = 1200},
            };
        }

        protected PatientScreeningFrequencyService Sut()
        {
            return new PatientScreeningFrequencyService(() => _patientRepository.Object, _settingsRepository.Object);
        }

        protected ScreeningPatientIdentity GetPatient()
        {
            return new ScreeningPatientIdentity
            {
                FirstName = "Unit",
                LastName = "Test",
                MiddleName = "User",
                Birthday = new DateTime(2000, 10, 20)
            };
        }

        [TestMethod]
        public void GetGPRAScreeningCount_Calls_Repositories()
        {

            DateTimeOffset today = new DateTimeOffset(2012, 06, 23, 13, 05, 35, TimeSpan.FromHours(3));

            ScreeningPatientIdentity patient = new ScreeningPatientIdentity
            {
                FirstName = "Unit",
                LastName = "Test",
                MiddleName = "User",
                Birthday = new DateTime(2000, 10, 20)
            };
            Dictionary<string, int> expected = new Dictionary<string, int>{
                {ScreeningFrequencyDescriptor.ContactFrequencyID, 2},
                {ScreeningSectionDescriptor.SmokerInHome, 0},
                {ScreeningSectionDescriptor.Tobacco, 1},
                {ScreeningSectionDescriptor.Alcohol, 2},
                {ScreeningSectionDescriptor.Depression, 3},
                {ScreeningSectionDescriptor.DepressionAllQuestions, 3},
                { ScreeningSectionDescriptor.PartnerViolence, 4},
                {ScreeningSectionDescriptor.SubstanceAbuse, 5},
                {ScreeningSectionDescriptor.DrugOfChoice, 6},
                {ScreeningSectionDescriptor.Anxiety, 8},
                {ScreeningSectionDescriptor.AnxietyAllQuestions, 8},

            };
            Dictionary<string, int> actual = Sut().GetGPRAScreeningCount(patient, today, ScreeningProfile.DefaultProfileID);


            var actualKeys = actual.Keys;
            var expectedKeys = expected.Keys;

            actualKeys.Count.Should().Be(expectedKeys.Count);

            foreach (var kp in expected)
            {
                var actualValue = actual[kp.Key];

                Assert.AreEqual(kp.Value, actualValue, "Value failed for " + kp.Key);
            }

        }


        [TestMethod]
        public void GetGPRAScreeningCountTest_Every_Visit()
        {
            DateTimeOffset today = new DateTimeOffset(2012, 06, 23, 13, 05, 35, TimeSpan.FromHours(-5));

            Dictionary<string, int> actual = Sut().GetGPRAScreeningCount(GetPatient(), today, ScreeningProfile.DefaultProfileID);


            actual[ScreeningSectionDescriptor.SmokerInHome].Should().Be(0);

            _patientRepository.Verify(x => x.GetPatientSectionScreeningCount(It.IsAny<ScreeningPatientIdentity>(), ScreeningSectionDescriptor.SmokerInHome, It.IsAny<DateTimeOffset>()), Times.Never);
        }



        [TestMethod]
        public void GetGPRAScreeningCountTest_Once()
        {
            DateTimeOffset today = new DateTimeOffset(2012, 06, 23, 13, 05, 35, TimeSpan.FromHours(-5));

            DateTimeOffset expected = today.AddMonths(-2400);

            Dictionary<string, int> actual = Sut().GetGPRAScreeningCount(GetPatient(), today, ScreeningProfile.DefaultProfileID);

            _patientRepository.Verify(x => x.GetPatientSectionScreeningCount(
                It.IsAny<ScreeningPatientIdentity>(),
                ScreeningSectionDescriptor.PartnerViolence,
                expected),
                Times.Once);

        }

        [TestMethod]
        public void GetGPRAScreeningCountTest_Monthly()
        {
            DateTimeOffset today = new DateTimeOffset(2012, 06, 23, 13, 05, 35, TimeSpan.FromHours(-5));

            DateTimeOffset expected = new DateTimeOffset(2012, 06, 01, 0, 0, 0, today.Offset);

            Dictionary<string, int> actual = Sut().GetGPRAScreeningCount(GetPatient(), today, ScreeningProfile.DefaultProfileID);

            _patientRepository.Verify(x => x.GetPatientContactInfoScreeningCount(
                It.IsAny<ScreeningPatientIdentity>(),
                expected),
                Times.Once);

        }

        [TestMethod]
        public void GetGPRAScreeningCountTest_Daily()
        {
            DateTimeOffset today = new DateTimeOffset(2012, 06, 23, 13, 05, 35, TimeSpan.FromHours(-5));

            DateTimeOffset expected = new DateTimeOffset(2012, 06, 23, 0, 0, 0, today.Offset);

            Dictionary<string, int> actual = Sut().GetGPRAScreeningCount(GetPatient(), today, ScreeningProfile.DefaultProfileID);

            _patientRepository.Verify(x => x.GetPatientSectionScreeningCount(
                It.IsAny<ScreeningPatientIdentity>(),
                ScreeningSectionDescriptor.DrugOfChoice,
                expected),
                Times.Once);

        }

        [TestMethod]
        public void GetGPRAScreeningCountTest_Weekly()
        {
            DateTimeOffset today = new DateTimeOffset(2020, 10, 09, 13, 05, 35, TimeSpan.FromHours(-5));

            DateTimeOffset expected = new DateTimeOffset(2020, 10, 03, 0, 0, 0, today.Offset);

            Dictionary<string, int> actual = Sut().GetGPRAScreeningCount(GetPatient(), today, ScreeningProfile.DefaultProfileID);

            _patientRepository.Verify(x => x.GetPatientSectionScreeningCount(
                It.IsAny<ScreeningPatientIdentity>(),
                ScreeningSectionDescriptor.SubstanceAbuse,
                expected),
                Times.Once);

        }
        
    }
}
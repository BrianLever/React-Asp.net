using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RPMS.Common;
using RPMS.Common.Models;
using RPMS.Tests.MotherObjects;
using RPMS.UnitTest.Factory;
using ScreenDox.EHR.Common.SmartExport;

namespace FrontDesk.Server.SmartExport.Tests
{
    [TestClass]
    public class PatientServiceFindBestMatchTests
    {
        private Mock<IPatientRepository> _repositoryMock = new Mock<IPatientRepository>();

        protected PatientService Sut()
        {
            return new PatientService(_repositoryMock.Object);
        }

        [TestMethod]
        public void WhenFoundGeminiMatch_BestResult_ShouldReturnBestMatch()
        {
            var patientMatch = PatientFactory.CreateANGELA()
                    .WithMiddleName("A");

            var list = new List<Patient>
            {
                PatientFactory.CreateANGELA()
                    .WithFirstName("DAVID")
                    .WithMiddleName("A"),
                patientMatch.Clone().ToPatient(),
                PatientFactory.CreateANGELA()
                    .WithFirstName("ANGEL")
                    .WithMiddleName("B"),

            };
            _repositoryMock.Setup(x => x.GetMatchedPatients(It.IsAny<Patient>())).
                Returns(list);

            var bestMatch = Sut().GetMatchedPatients(patientMatch, 0, 100)
                .FindBestMatch();

            bestMatch.BestResult.FirstName.Should().Be(patientMatch.FirstName);
        }

        [TestMethod]
        public void WhenFoundGeminiMatch_AndDifferentMiddle_ShouldReturnBestMatch()
        {
            var patientMatch = PatientFactory.CreateANGELA()
                    .WithMiddleName("A");

            var list = new List<Patient>
            {
                PatientFactory.CreateANGELA()
                    .WithFirstName("DAVID")
                    .WithMiddleName("A"),
                patientMatch.Clone().ToPatient(),
                PatientFactory.CreateANGELA()
                    .WithMiddleName("B"),

            };
            _repositoryMock.Setup(x => x.GetMatchedPatients(It.IsAny<Patient>())).
                Returns(list);

            var bestMatch = Sut().GetMatchedPatients(patientMatch, 0, 100)
                .FindBestMatch();

            bestMatch.BestResult.FirstName.Should().Be(patientMatch.FirstName);
            bestMatch.BestResult.MiddleName.Should().Be(patientMatch.MiddleName);
        }
    }
}

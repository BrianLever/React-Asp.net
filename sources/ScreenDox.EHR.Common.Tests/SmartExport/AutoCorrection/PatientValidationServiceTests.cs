using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using RPMS.Common;
using RPMS.Common.Models;
using RPMS.Tests.MotherObjects;
using ScreenDox.EHR.Common.SmartExport;
using System.Collections.Generic;

namespace ScreenDox.EHR.Common.Tests.SmartExport.AutoCorrection
{
    [TestClass]
    public class PatientValidationServiceTests
    {
        private readonly Mock<IPatientService> patientServiceMock = new Mock<IPatientService>();
        private readonly Mock<IPatientInfoMatchService> patientInfoMatchServiceMock = new Mock<IPatientInfoMatchService>();
        private readonly Mock<IPatientNameCorrectionLogService> patientNameCorrectionLogServiceMock = new Mock<IPatientNameCorrectionLogService>();


        public PatientValidationServiceTests()
        {
            patientServiceMock.Setup(x => x.GetMatchedPatients(It.IsAny<Patient>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new List<Patient>());

            patientInfoMatchServiceMock.Setup(x => x.GetBestMatch(It.IsAny<PatientSearch>()))
                .Returns<PatientSearch>(null);

            patientInfoMatchServiceMock.Setup(x => x.CorrectNamesWithMap(It.IsAny<PatientSearch>()))
               .Returns((PatientSearch p) => p);
        }

        protected PatientValidationService Sut()
        {
            return new PatientValidationService(
                patientServiceMock.Object, 
                patientInfoMatchServiceMock.Object, 
                patientNameCorrectionLogServiceMock.Object);
        }


        [TestMethod]
        public void ValidatePatientRecord_Returns_Result()
        {
            var sut = Sut();

            var patient = PatientMotherObject.CreateANDREA();

            sut.ValidatePatientRecord(patient).Should().NotBeNull();
        }


        [TestMethod]
        public void ValidatePatientRecord_Check_Strategies_When_No_Match()
        {
            // assert
            var sut = Sut();
            var patient = PatientMotherObject.CreateANDREA();

            // act
            sut.ValidatePatientRecord(patient);

            // assert
            patientServiceMock.Verify(x => x.GetMatchedPatients(It.IsAny<Patient>(), 0, 100), Times.AtLeast(3));
        }

        [TestMethod]
        public void ValidatePatientRecord_Check_All_Possible_Strategies_When_No_Match()
        {
            // assert
            var sut = Sut();
            var patient = PatientMotherObject.CreateANDREA();

            // act
            sut.ValidatePatientRecord(patient);

            // assert
            patientServiceMock.Verify(x => x.GetMatchedPatients(It.IsAny<Patient>(), 0, 100), Times.Exactly(8));
        }

        [TestMethod]
        public void ValidatePatientRecord_Succeed_And_Stop_When_Original_Found()
        {
            // assert
            var sut = Sut();
            var patient = PatientMotherObject.CreateANDREA();
            var returnValue = patient.ToPatient();

            patientServiceMock.Setup(x => x.GetMatchedPatients(It.Is<Patient>(p=> p.MiddleName == returnValue.MiddleName), It.IsAny<int>(), It.IsAny<int>()))
               .Returns(new List<Patient> { returnValue });


            // act
            var actual = sut.ValidatePatientRecord(patient);

            // assert
            patientServiceMock.Verify(x => x.GetMatchedPatients(It.IsAny<Patient>(), 0, 100), Times.Exactly(1));
            actual.PatientRecord.Should().BeEquivalentTo(returnValue);
            actual.IsMatchFound().Should().BeTrue();
        }

        [TestMethod]
        public void ValidatePatientRecord_Succeed_When_Original_Found()
        {
            // assert
            var sut = Sut();
            var patient = PatientMotherObject.CreateANDREA();
            var returnValue = patient.ToPatient();

            patientServiceMock.Setup(x => x.GetMatchedPatients(It.Is<Patient>(p => p.MiddleName == returnValue.MiddleName), It.IsAny<int>(), It.IsAny<int>()))
               .Returns(new List<Patient> { returnValue });

            // act
            var actual = sut.ValidatePatientRecord(patient);

            // assert
            actual.IsMatchFound().Should().BeTrue();
            actual.PatientRecord.Should().BeEquivalentTo(returnValue);
        }


        [TestMethod]
        public void ValidatePatientRecord_WhenFoundInScreenDox_Skip_EHRCall()
        {
            // assert
            var sut = Sut();
            var patient = PatientMotherObject.CreateANDREA();
            var returnValue = patient;

            patientInfoMatchServiceMock.Setup(x => x.GetBestMatch(It.IsAny<PatientSearch>()))
               .Returns(returnValue);


            // act
            var actual = sut.ValidatePatientRecord(patient);

            // assert
            patientServiceMock.Verify(x => x.GetMatchedPatients(It.IsAny<Patient>(), 0, 100), Times.Never());
        }

        [TestMethod]
        public void ValidatePatientRecord_WhenFoundInScreenDox_Succeed()
        {
            // assert
            var sut = Sut();
            var patient = PatientMotherObject.CreateANDREA();
            var returnValue = patient;

            patientInfoMatchServiceMock.Setup(x => 
                x.GetBestMatch(It.Is<PatientSearch>(p => p.LastName == returnValue.LastName)))
               .Returns(returnValue);


            // act
            var actual = sut.ValidatePatientRecord(patient);

            // assert
            actual.IsMatchFound().Should().BeTrue();
            actual.PatientRecord.Should().NotBeNull();
            actual.PatientRecord.FullName().Should().Be(returnValue.FullName());
            actual.PatientRecord.DateOfBirth.Should().Be(returnValue.DateOfBirth);

        }

        [TestMethod]
        public void ValidatePatientRecord_CapitalizeName()
        {
            // assert
            var sut = Sut();
            var patient = PatientMotherObject.CreateANDREA()
                .WithLastName("Dow")
                .WithFirstName("John")
                .WithMiddleName("Jr.");


            var expectedValue = PatientMotherObject.CreateANDREA()
                .WithLastName("DOW")
                .WithFirstName("JOHN")
                .WithMiddleName("JR.");

            patientServiceMock.Setup(x =>
                x.GetMatchedPatients(It.Is<Patient>(p => p.LastName == patient.LastName),0, 100))
               .Returns(new List<Patient>() { patient });

            // act
            var actual = sut.ValidatePatientRecord(patient);

            // assert
            actual.IsMatchFound().Should().BeTrue();
            actual.PatientRecord.FullName().Should().Be(expectedValue.FullName());

        }
    }
}

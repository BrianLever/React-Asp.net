using FluentAssertions;

using FrontDesk.Server.Services.Security;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using RPMS.Common.Models;
using RPMS.Tests.MotherObjects;

using System;
using System.Collections.Generic;

namespace FrontDesk.Server.IntegrationTests.KioskEndpoint.ValidatePatient
{
    [TestClass]
    public class ValidatePatientTests : KioskEndpointTestBase
    {
     
        [TestMethod]
        public void ValidatePatient_Fail_When_Empty_Authentication()
        {
            PatientSearch patient = PatientMotherObject.GetMaryTest();
            Dictionary<string, string> headers = new Dictionary<string, string>
            {
            };

            Action a = () => ExecuteSut<PatientSearch>((c) => c.ValidatePatient(patient), headers);

            a.Should().Throw<System.ServiceModel.FaultException>();
        }

        [TestMethod]
        public void ValidatePatient_Fail_When_SecretNotProvided()
        {
            PatientSearch patient = PatientMotherObject.GetMaryTest();
            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                {KioskEndpointHeaderDescriptor.KioskIDHeader, "PRUW"}
            };

            Action a = () => ExecuteSut<PatientSearch>((c) => c.ValidatePatient(patient), headers);

            a.Should().Throw<System.ServiceModel.FaultException>();
        }

        [TestMethod]
        public void ValidatePatient_Can_Find_Marry_Test_Not_Empty()
        {
            // assign
            PatientSearch patient = PatientMotherObject.GetMaryTest();
           
            //act
            var result = ExecuteSut<PatientSearch>((c) => c.ValidatePatient(patient));

            // assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void ValidatePatient_UpperCase_When_Find_ExactMatch_Marry_Test()
        {
            // assign
            PatientSearch patient = PatientMotherObject.GetMaryTest();

            //act
            var result = ExecuteSut<PatientSearch>((c) => c.ValidatePatient(patient));

            // assert
            result.FirstName.Should().Be("MARY");
            result.LastName.Should().Be("TEST");
        }


        [TestMethod]
        public void ValidatePatient_Can_Find_Marry_Test_When_Reverted()
        {
            // assign
            PatientSearch patient = PatientMotherObject.GetMaryTest();

            patient.LastName = "MARY";
            patient.FirstName = "TEST";

            //act
            var result = ExecuteSut<PatientSearch>((c) => c.ValidatePatient(patient));

            // assert
            result.Should().NotBeNull();
            result.FirstName.Should().Be("MARY");
            result.LastName.Should().Be("TEST");
        }


        [TestMethod]
        public void ValidatePatient_Can_Find_Marry_Test_When_Reverted_and_Invalid_DOB()
        {
            // assign
            PatientSearch patient = PatientMotherObject.GetMaryTest()
                .SetBirthday(new DateTime(1950, 10, 01));

            patient.LastName = "MARY";
            patient.FirstName = "TEST";

            //act
            var result = ExecuteSut<PatientSearch>((c) => c.ValidatePatient(patient));

            // assert
            result.Should().NotBeNull();
            result.FirstName.Should().Be("MARY");
            result.LastName.Should().Be("TEST");
            result.DateOfBirth.Should().Be(new DateTime(1950, 01, 01));
        }

        [TestMethod]
        public void ValidatePatient_Can_Find_Marry_Test_When_Corrected_With_Soundex()
        {
            // assign
            PatientSearch patient = PatientMotherObject.GetMaryTest();
            patient.FirstName = "MARRY";

            //act
            var result = ExecuteSut<PatientSearch>((c) => c.ValidatePatient(patient));

            // assert
            result.Should().NotBeNull();
            result.FirstName.Should().Be("MARY");
            result.LastName.Should().Be("TEST");
            result.DateOfBirth.Should().Be(new DateTime(1950, 01, 01));
        }
        [TestMethod]
        public void ValidatePatient_Can_Find_Marry_Test_When_Corrected_With_AutoCorrection()
        {
            // assign
            PatientSearch patient = PatientMotherObject.GetMaryTest();

            patient.LastName = "MARY";
            patient.FirstName = "TDST";

            //act
            var result = ExecuteSut<PatientSearch>((c) => c.ValidatePatient(patient));

            // assert
            result.Should().NotBeNull();
            result.FirstName.Should().Be("MARY");
            result.LastName.Should().Be("TEST");
            result.DateOfBirth.Should().Be(new DateTime(1950, 01, 01));
        }
    }
}

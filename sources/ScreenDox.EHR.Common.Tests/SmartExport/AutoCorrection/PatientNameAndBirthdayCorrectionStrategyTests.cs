using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RPMS.Common.Models;
using RPMS.Tests.MotherObjects;
using ScreenDox.EHR.Common.SmartExport.AutoCorrection;

namespace ScreenDox.EHR.Common.Tests.SmartExport.AutoCorrection
{
    [TestClass]
    public class PatientNameAndBirthdayCorrectionStrategyTests :  CorrectionStrategyTestsBase<PatientNameAndBirthdayCorrectionStrategy>
    {
        [TestMethod]
        public void PatientNameAndBirthdayCorrection_When_Single_Names_Count_Correct()
        {
            var patient = PatientMotherObject.CreateANDREA();
            patient.SetBirthday(DateTime.Parse("10/02/1955"));

            var actual = Sut().Apply(patient).ToList();

            actual.Count().Should().Be(24);
        }


        [TestMethod]
        public void PatientNameAndBirthdayCorrection_Strategy_Sequence_Correct()
        {
            var patient = PatientMotherObject.CreateANDREA();
            patient.SetBirthday(DateTime.Parse("10/02/1955"));

            var actual = Sut().Apply(patient).ToList();

            actual[0].DateOfBirth.Should().Be(new DateTime(1955, 10, 02));
            actual[0].LastName.Should().Be(patient.LastName);

            actual[6].DateOfBirth.Should().Be(new DateTime(1955, 01, 02));
            actual[6].LastName.Should().Be(patient.LastName);

            actual[12].DateOfBirth.Should().Be(new DateTime(1955, 10, 20));
            actual[12].LastName.Should().Be(patient.LastName);

            actual[18].DateOfBirth.Should().Be(new DateTime(1955, 02, 10));
            actual[18].LastName.Should().Be(patient.LastName);
        }


        [TestMethod]
        public void PatientNameAndBirthdayCorrection_Fix_Name_And_DOB_Swap()
        {
            // assign 
            var patientOriginal = (PatientSearch)PatientMotherObject.GetMaryTest();

            var patient = patientOriginal.Clone()
                .SetBirthday(new DateTime(1950, 10, 01));
            patient.LastName = "MARY";
            patient.FirstName = "TEST";

            // act
            var result = Sut().Apply(patient).ToList();

            // assert
            result.Should().Contain(p => 
                p.LastName == patientOriginal.LastName.ToUpper() &&
                p.FirstName == patientOriginal.FirstName.ToUpper() &&
                p.DateOfBirth == patientOriginal.DateOfBirth);
        }
    }
}

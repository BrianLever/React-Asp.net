using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RPMS.Tests.MotherObjects;
using ScreenDox.EHR.Common.SmartExport.AutoCorrection;

namespace ScreenDox.EHR.Common.Tests.SmartExport.AutoCorrection
{
    [TestClass]
    public class PatientBirthdayCorrectionStrategyTests : CorrectionStrategyTestsBase<PatientBirthdayCorrectionStrategy>
    {

        [TestMethod]
        public void PatientBirthdayCorrection_Returns_Valid_Count()
        {
            var patient = PatientMotherObject.CreateANDREA();
            patient.DateOfBirth = DateTime.Parse("10/12/1955");

            var actual = Sut().Apply(patient).ToList();

            actual.Count.Should().Be(4);
        }


        [TestMethod]
        public void PatientBirthdayCorrection_Returns_Valid_Dates()
        {
            var patient = PatientMotherObject.CreateANDREA();
            patient.DateOfBirth = DateTime.Parse("10/02/1955");
            var expected = new List<DateTime>
            {
                new DateTime(1955, 10, 02), // original
                new DateTime(1955, 01, 02), // swap digits in month
                new DateTime(1955, 10, 20), //swap digits in day
                new DateTime(1955, 02, 10)  // swap month and day
             };


            var actual = Sut().Apply(patient).Select(x => x.DateOfBirth).ToList();


            actual.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void PatientBirthdayCorrection_Returns_Ignores_Invalid_Dates()
        {
            var patient = PatientMotherObject.CreateANDREA();
            patient.DateOfBirth = DateTime.Parse("10/15/1955");
            var expected = new List<DateTime>
            {
                new DateTime(1955, 10, 15), // original
                new DateTime(1955, 01, 15), // swap digits in month
             };


            var actual = Sut().Apply(patient).Select(x => x.DateOfBirth).ToList();


            actual.Should().BeEquivalentTo(expected);
        }

    }
}

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using RPMS.Common.Models;
using RPMS.Tests.MotherObjects;

using ScreenDox.EHR.Common.SmartExport.AutoCorrection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScreenDox.EHR.Common.Tests.SmartExport.AutoCorrection
{
    [TestClass]
    public class PatientNameCorrectionStrategyTests : CorrectionStrategyTestsBase<PatientNameCorrectionStrategy>
    {


        [TestMethod]
        public void PatientNameCorrection_When_Single_Names_Count_Correct()
        {
            var patient = PatientMotherObject.CreateANDREA();

            var actual = Sut().Apply(patient).ToList();

            actual.Count().Should().Be(6);
        }

        [TestMethod]
        public void PatientNameCorrection_When_Single_Names_Options_Correct()
        {
            var patient = PatientMotherObject.CreateANDREA();

            var actual = Sut().Apply(patient).Select(x =>
                new PatientSearch
                {
                    LastName = x.LastName,
                    FirstName = x.FirstName,
                    MiddleName = x.MiddleName
                });

            actual.Should().BeEquivalentTo(new List<PatientSearch>
            {
                new PatientSearch{LastName = "DEMO", FirstName = "ANDREA", MiddleName = "C"},
                new PatientSearch{LastName = "DEMO", FirstName = "CHRIS", MiddleName = "A"},
                new PatientSearch{LastName = "ANDREA", FirstName = "DEMO", MiddleName = "C"},
                new PatientSearch{LastName = "ANDREA", FirstName = "CHRIS", MiddleName = "D"},
                new PatientSearch{LastName = "CHRIS", FirstName = "DEMO", MiddleName = "A"},
                new PatientSearch{LastName = "CHRIS", FirstName = "ANDREA", MiddleName = "D"},
            });
        }
        [TestMethod]
        public void PatientNameCorrection_When_Empty_MiddleName_No_Exception()
        {
            var patient = PatientMotherObject.CreateANDREA();
            patient.MiddleName = null;

            var action = (Action)(() => Sut().Apply(patient).ToList());

            action.Should().NotThrow();
        }

        [TestMethod]
        public void PatientNameCorrection_When_Empty_LastName_No_Exception()
        {
            var patient = PatientMotherObject.CreateANDREA();
            patient.LastName = null;

            var action = (Action)(() => Sut().Apply(patient).ToList());

            action.Should().NotThrow();
        }


        [TestMethod]
        public void PatientNameCorrection_When_Empty_FirstName_No_Exception()
        {
            var patient = PatientMotherObject.CreateANDREA();
            patient.FirstName = null;

            var action = (Action)(() => Sut().Apply(patient).ToList());

            action.Should().NotThrow();
        }


        [TestMethod]
        public void PatientNameCorrection_When_Compound_Names_Count_Correct()
        {
            var patient = PatientMotherObject.CreateANDREA()
                .WithLastName("Corona")
                .WithFirstName("Johnny Corona")
                .WithMiddleName("Lee");

            var actual = Sut().Apply(patient).ToList();

            actual.Count().Should().Be(60);
        }


        [TestMethod]
        public void PatientNameCorrection_Orignal_Name_With_Cut_Middle_Comes_First()
        {
            var patient = PatientMotherObject.CreateANDREA()
                .WithFirstName("Johnny Corona");

            var expected = patient.Clone();
            expected.MiddleName = "C";

            var actual = Sut().Apply(patient).ToList();

            actual.First().Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void PatientNameCorrection_Orignal_LastName_With_Hyphen_Replaced_With_Space()
        {
            var patient = PatientMotherObject.CreateANDREA()
                .WithLastName("HARRISON-JOHNSON");


            var actual = Sut().Apply(patient).ToList();

            actual.Select(x => x.LastName).Should().Contain("HARRISON JOHNSON");
        }

        [TestMethod]
        public void PatientNameCorrection_Orignal_FirstName_With_Hyphen_Replaced_With_Spaces()
        {
            var patient = PatientMotherObject.CreateANDREA()
                .WithFirstName("HARRISON-JOHNSON");


            var actual = Sut().Apply(patient).ToList();

            actual.Select(x => x.FirstName).Should().Contain("HARRISON JOHNSON");
        }

        [TestMethod]
        public void PatientNameCorrection_Orignal_LastName_With_Space_Replaced_With_Hyphen()
        {
            var patient = PatientMotherObject.CreateANDREA()
                .WithLastName("HARRISON JOHNSON");


            var actual = Sut().Apply(patient).ToList();

            actual.Select(x => x.LastName).Should().Contain("HARRISON-JOHNSON");
        }


        [TestMethod]
        public void PatientNameCorrection_Dublicates_Last_and_First()
        {
            var patient = PatientMotherObject.CreateANDREA()
                .WithLastName("DEMO")
                .WithMiddleName("PAM")
                .WithFirstName("DEMO");

            var expected = new PatientSearch
            {
                LastName = "DEMO",
                FirstName = "PAM",
                MiddleName = "",
                DateOfBirth = patient.DateOfBirth
            };

            var actual = Sut().Apply(patient).ToList();

            actual.Should().Contain(expected);
        }

        [TestMethod]
        public void PatientNameCorrection_Dublicates_Last_and_First_Returns_2_Patterns()
        {
            var patient = PatientMotherObject.CreateANDREA()
                .WithLastName("DEMO")
                .WithMiddleName("PAM")
                .WithFirstName("DEMO");

            var actual = Sut().Apply(patient).ToList();

            actual.Count.Should().Be(2); // DEMO, PAM and PAM, DEMO
        }


        [TestMethod]
        public void PatientNameCorrection_Empty_Middle_Returns_2_Patterns()
        {
            var patient = PatientMotherObject.CreateANDREA()
                .WithLastName("PAM")
                .WithMiddleName("")
                .WithFirstName("DEMO");

            var actual = Sut().Apply(patient).ToList();

            actual.Count.Should().Be(2); // DEMO, PAM and PAM, DEMO
        }


        [TestMethod]
        public void PatientNameCorrection_Lastname_Has_Period_And_Hyphen()
        {
            var patient = PatientMotherObject.CreateANDREA()
                .WithLastName("ST. MARIE")
                .WithFirstName("SHE'RAE")
                 .WithMiddleName("MAXINE");

            var actual = Sut().Apply(patient).ToList();


            actual.Select(x => x.LastName).Should().Contain("ST MARIE");
            //actual.Should().Contain(new PatientSearch
            //{
            //    LastName = "ST MARIE",
            //    FirstName = "SHE'RAE",
            //    MiddleName = "MAXINE",
            //    DateOfBirth = patient.DateOfBirth
            //});
        }

        [TestMethod]
        public void PatientNameCorrection_Lastname_Has_Stop_Replace_With_Space()
        {
            var patient = PatientMotherObject.CreateANDREA()
                .WithLastName("ST.MARIE")
                .WithFirstName("SHE'RAE")
                 .WithMiddleName("MAXINE");

            var actual = Sut().Apply(patient).ToList();


            actual.Select(x => x.LastName).Should().Contain("ST MARIE");
            //actual.Should().Contain(new PatientSearch
            //{
            //    LastName = "ST MARIE",
            //    FirstName = "SHE'RAE",
            //    MiddleName = "MAXINE",
            //    DateOfBirth = patient.DateOfBirth
            //});
        }
    }
}

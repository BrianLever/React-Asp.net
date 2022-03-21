using RPMS.Common.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using RPMS.Common.Comparers;
using FluentAssertions;

namespace RPMS.UnitTest
{


    /// <summary>
    ///This is a test class for PatientTest and is intended
    ///to contain all PatientTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PatientComparerTest
    {

        const int MAX_MATCH_VALUE = 0x1FF801;

        private PatientComparer comparer = new PatientComparer();

        [TestCategory("Patient_Rank_Match")]
        [TestMethod()]
        public void Can_match_by_lastname()
        {
            Patient item = Factory.PatientFactory.CreateANDREA();
            Patient pattern = Factory.PatientFactory.CreateGARERD();
            pattern.LastName = item.LastName;
            int expected = MAX_MATCH_VALUE - 0x100000;
            int actual;

            actual = comparer.GetMatchRank(item, pattern);

            Assert.AreEqual(expected, actual);
        }

        [TestCategory("Patient_Rank_Match")]
        [TestMethod()]
        public void Can_match_by_birthday()
        {
            Patient target = Factory.PatientFactory.CreateANDREA();
            Patient pattern = Factory.PatientFactory.CreateGARERD();
            pattern.DateOfBirth = target.DateOfBirth;
            int expected = MAX_MATCH_VALUE - 0x80000;
            int actual;

            actual = comparer.GetMatchRank(target, pattern);

            Assert.AreEqual(expected, actual);
        }

        [TestCategory("Patient_Rank_Match")]
        [TestMethod()]
        public void Can_match_by_lastname_and_birthday()
        {
            Patient target = Factory.PatientFactory.CreateANDREA();
            Patient pattern = Factory.PatientFactory.CreateGARERD();
            pattern.LastName = target.LastName;
            pattern.DateOfBirth = target.DateOfBirth;
            int expected = MAX_MATCH_VALUE - 0x100000 - 0x80000;
            int actual;

            actual = comparer.GetMatchRank(target, pattern);

            Assert.AreEqual(expected, actual);
        }

        [TestCategory("Patient_Rank_Match")]
        [TestMethod()]
        public void Can_match_by_first_short()
        {
            Patient target = Factory.PatientFactory.CreateANDREA();
            Patient pattern = Factory.PatientFactory.CreateGARERD();
            pattern.LastName = target.LastName;
            pattern.DateOfBirth = target.DateOfBirth;
            pattern.FirstName = target.FirstName.Substring(0, 3);

            int expected = MAX_MATCH_VALUE - 0x100000 - 0x80000 - 0x20000;
            int actual;

            actual = comparer.GetMatchRank(target, pattern);

            Assert.AreEqual(expected, actual);
        }

        [TestCategory("Patient_Rank_Match")]
        [TestMethod()]
        public void Can_match_by_first_full()
        {
            Patient target = Factory.PatientFactory.CreateANDREA();
            Patient pattern = Factory.PatientFactory.CreateGARERD();
            pattern.LastName = target.LastName;
            pattern.DateOfBirth = target.DateOfBirth;
            pattern.FirstName = target.FirstName;

            int expected = MAX_MATCH_VALUE - 0x100000 - 0x80000 - 0x40000 - 0x20000;
            int actual;

            actual = comparer.GetMatchRank(target, pattern);

            Assert.AreEqual(expected, actual);
        }

        [TestCategory("Patient_Rank_Match")]
        [TestMethod()]
        public void When_first_name_partially_match_has_better_value()
        {
            //Chris and Angela has the same last name and DOB
            Patient pattern1 = Factory.PatientFactory.CreateCHRIS();
            Patient pattern2 = Factory.PatientFactory.CreateANGELA();

            //source does not have any address info
            Patient source = new Patient
            {
                LastName = pattern1.LastName,
                FirstName = "CHRISTOPHER",
                DateOfBirth = pattern1.DateOfBirth
            };

            var rankChris = comparer.GetMatchRank(source, pattern1);
            var rankAngela = comparer.GetMatchRank(source, pattern2);

            rankChris.Should().BeLessThan(rankAngela);
        }

        [TestCategory("Patient_Rank_Match")]
        [TestMethod()]
        public void Diff_First_higher_than_Street_And_Middle()
        {
            Patient target1 = Factory.PatientFactory.CreateGARERD();
            Patient target2 = Factory.PatientFactory.CreateGARERD();

            Patient pattern = Factory.PatientFactory.CreateGARERD();


            target1.MiddleName = "Jr.";
            target1.StreetAddress = "Fake street fake house";

            target2.FirstName = "Fake Name";

            int actual1 = comparer.GetMatchRank(target1, pattern);
            int actual2 = comparer.GetMatchRank(target2, pattern);

            Assert.IsTrue(actual1 < actual2, "Diff in first name is not higher than in middle and street. Actual <{0}> and <{1}>", actual1, actual2);
        }


        [TestCategory("Patient_Rank_Match")]
        [TestMethod()]
        public void Equal_not_zero()
        {
            Patient target = Factory.PatientFactory.CreateGARERD();
            Patient pattern = Factory.PatientFactory.CreateGARERD();
            int expected = 1;

            int actual = comparer.GetMatchRank(target, pattern);
           

            Assert.AreEqual(expected, actual);
        }

        [TestCategory("Patient_Rank_Match")]
        [TestMethod()]
        public void Can_identify_different_street_address()
        {
            Patient target = Factory.PatientFactory.CreateANDREA();
            Patient pattern = Factory.PatientFactory.CreateANDREA();
            pattern.StreetAddressLine1 = "Address Line 1";
            int expected = 0x1001;
            int actual;

            actual = comparer.GetMatchRank(target, pattern);

            Assert.AreEqual(expected, actual);
        }


        [TestCategory("Patient_Rank_Match")]
        [TestMethod()]
        public void Can_identify_equal_street_address_Addi()
        {
            Patient repoItem = Factory.PatientFactory.CreateAddi();
            Patient pattern = Factory.PatientFactory.CreateAddi();

            repoItem.StreetAddress = "P.O. BOX 82";

            int expected = 0x1;
            int actual;

            actual = comparer.GetMatchRank(repoItem, pattern);

            Assert.AreEqual(expected, actual);
        }

        [TestCategory("Patient_Rank_Match")]
        [TestMethod()]
        public void Can_identify_equal_street_address_Sadra()
        {
            Patient rpmsItem = Factory.PatientFactory.CreateSANDRA();
            Patient pattern = Factory.PatientFactory.CreateSANDRA();

            rpmsItem.StreetAddress = "64777 ELM ST";

            int expected = 0x1;
            int actual;

            actual = comparer.GetMatchRank(rpmsItem, pattern);

            Assert.AreEqual(expected, actual);
        }

        [TestCategory("Patient_Rank_Match")]
        [TestMethod()]
        public void Can_identify_equal_street_address_Sadra_with_leading_space()
        {
            Patient rpmsItem = Factory.PatientFactory.CreateSANDRA();
            Patient pattern = Factory.PatientFactory.CreateSANDRA();

            rpmsItem.StreetAddress = " 64777 ELM ST";

            int expected = 0x1;
            int actual;

            actual = comparer.GetMatchRank(rpmsItem, pattern);

            Assert.AreEqual(expected, actual);
        }

        [TestCategory("Patient_Rank_Match")]
        [TestMethod()]
        public void Can_identify_equal_street_address_if_additional_symbols_in_Rpms()
        {
            Patient rpmsItem = Factory.PatientFactory.CreateSANDRA();
            Patient pattern = Factory.PatientFactory.CreateSANDRA();

            rpmsItem.StreetAddress = "1000 CATUS RD.";
            pattern.StreetAddress = "1000 CATUS RD";

            int expected = 0x1;
            int actual;

            actual = comparer.GetMatchRank(rpmsItem, pattern);

            Assert.AreEqual(expected, actual);
        }

        [TestCategory("Patient_Rank_Match")]
        [TestMethod()]
        public void Can_match_equal_patient()
        {
            Patient rpmsItem = Factory.PatientFactory.CreateSANDRA();
            Patient pattern = Factory.PatientFactory.CreateSANDRA();

          
            int expected = PatientComparer.FULL_MATCH;
            int actual;

            actual = comparer.GetMatchRank(rpmsItem, pattern);

            Assert.AreEqual(expected, actual);
        }

        [TestCategory("Patient_Rank_Match")]
        [TestMethod()]
        public void Can_match_condition_when_fullname_and_dob_are_equal()
        {
            Patient rpmsItem = Factory.PatientFactory.CreateSANDRA();
            Patient pattern = Factory.PatientFactory.CreateSANDRA();

            rpmsItem.StreetAddress = "1111";
            rpmsItem.City = "City";
            rpmsItem.StateID = "NV";
            rpmsItem.PhoneHome = "111-111-22222";
            rpmsItem.ZipCode = "92042";

            int expected = PatientComparer.FULL_NAME_AND_DOB_MATCH;
            int actual;

            actual = comparer.GetMatchRank(rpmsItem, pattern);

            actual.Should().BeLessOrEqualTo(expected);
        }

        [TestCategory("Patient_Rank_Match")]
        [TestMethod()]
        public void Can_match_condition_when_middle_name_not_match()
        {
            Patient rpmsItem = Factory.PatientFactory.CreateSANDRA();
            Patient pattern = Factory.PatientFactory.CreateSANDRA();

            rpmsItem.MiddleName = "JOHN JR";
            rpmsItem.StreetAddress = "1111";
            rpmsItem.City = "City";
            rpmsItem.StateID = "NV";
            rpmsItem.PhoneHome = "111-111-22222";
            rpmsItem.ZipCode = "92042";

            int expected = PatientComparer.LAST_AND_FIRST_NAME_AND_DOB_MATCH_THRESHOLD;
            int actual;

            actual = comparer.GetMatchRank(rpmsItem, pattern);

            actual.Should().BeLessOrEqualTo(expected);
        }

        [TestCategory("Patient_Rank_Match")]
        [TestMethod()]
        public void Can_match_condition_when_only_middle_name_not_match()
        {
            Patient rpmsItem = Factory.PatientFactory.CreateSANDRA();
            Patient pattern = Factory.PatientFactory.CreateSANDRA();

            rpmsItem.MiddleName = "JOHN JR";
           

            int expected = PatientComparer.LAST_AND_FIRST_NAME_AND_DOB_MATCH_THRESHOLD;
            int actual;

            actual = comparer.GetMatchRank(rpmsItem, pattern);

            actual.Should().BeLessOrEqualTo(expected);
        }

        [TestCategory("Patient_Rank_Match")]
        [TestMethod()]
        public void Can_match_condition_when_only_last_name_not_match()
        {
            Patient rpmsItem = Factory.PatientFactory.CreateAddi();
            Patient pattern = Factory.PatientFactory.CreateAddi();

            rpmsItem.LastName = rpmsItem.LastName.Substring(0, rpmsItem.LastName.Length-1);


            int expected = PatientComparer.LAST_AND_FIRST_NAME_AND_DOB_MATCH_THRESHOLD;
            int actual;

            actual = comparer.GetMatchRank(rpmsItem, pattern);

            actual.Should().BeGreaterThan(expected);
        }

        [TestCategory("Patient_Rank_Match")]
        [TestMethod()]
        public void Can_match_when_hypen_in_screendox_name()
        {
            Patient target = Factory.PatientFactory.CreateANDREA();
            Patient pattern = Factory.PatientFactory.CreateANDREA();


            target.LastName = "RODRIGUEZ RESINGER";
            pattern.LastName = "RODRIGUEZ-RESINGER";

            int expected = PatientComparer.FULL_MATCH;
            int actual;

            actual = comparer.GetMatchRank(target, pattern);

            Assert.AreEqual(expected, actual);
        }

        [TestCategory("Patient_Rank_Match")]
        [TestMethod()]
        public void Can_match_when_hypen_in_rpms_name()
        {
            Patient target = Factory.PatientFactory.CreateANDREA();
            Patient pattern = Factory.PatientFactory.CreateANDREA();


            target.LastName = "RODRIGUEZ-RESINGER";
            pattern.LastName = "RODRIGUEZ RESINGER";

            int expected = PatientComparer.FULL_MATCH;
            int actual;

            actual = comparer.GetMatchRank(target, pattern);

            Assert.AreEqual(expected, actual);
        }

        [TestCategory("Patient_Rank_Match")]
        [TestMethod()]
        public void Can_match_when_diffcase_in_address()
        {
            Patient rpmsItem = Factory.PatientFactory.CreateSANDRA();
            Patient pattern = Factory.PatientFactory.CreateSANDRA();

            rpmsItem.StreetAddressLine1 = "64777 Elm st PO Box 110 South of Hospital";
            int expected = PatientComparer.FULL_MATCH;
            int actual;

            actual = comparer.GetMatchRank(rpmsItem, pattern);

            actual.Should().Be(expected);
        }

        [TestCategory("Patient_Rank_Match")]
        [TestMethod()]
        public void Can_match_when_diffcase_in_city()
        {
            Patient rpmsItem = Factory.PatientFactory.CreateSANDRA();
            Patient pattern = Factory.PatientFactory.CreateSANDRA();

            rpmsItem.City = "South Gate";
            int expected = PatientComparer.FULL_MATCH;
            int actual;

            actual = comparer.GetMatchRank(rpmsItem, pattern);

            actual.Should().Be(expected);
        }


        [TestCategory("Patient_Rank_Match")]
        [TestMethod()]
        public void Can_match_Middle_Has_Two_Words_And_Less_in_screendox()
        {
            Patient rpmsItem = Factory.PatientFactory.CreateNopah();
            Patient pattern = Factory.PatientFactory.CreateNopah();

            pattern.MiddleName = "NOPAH G";
            int expected = PatientComparer.LAST_AND_FIRST_NAME_AND_DOB_MATCH_THRESHOLD;
            int actual;

            actual = comparer.GetMatchRank(rpmsItem, pattern);

            actual.Should().BeLessThan(expected);
        }



        [TestCategory("Patient_Rank_Match")]
        [TestMethod()]
        public void Can_match_FirstNamePartial_Chris_And_Cristopher()
        {
            Patient rpmsItem = Factory.PatientFactory.CreateCHRIS();
            Patient pattern = Factory.PatientFactory.CreateCHRIS();

            rpmsItem.FirstName = "CHRISTOPHER";
            int expected = PatientComparer.FULL_MATCH_LAST_AND_DOB_AND_SHORT_FIRST_NAME;
            int actual;

            actual = comparer.GetMatchRank(rpmsItem, pattern);

            actual.Should().Be(expected);
        }
    }
}

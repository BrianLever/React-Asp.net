using System;
using FluentAssertions;
using FrontDesk.Common.Bhservice.Import;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrontDesk.Common.Tests.Import.PatientDemographicsKioskResultTests
{
    [TestClass]
    public class CountyOfResidenceInPatientDemographicsKioskResultTests
    {

        protected PatientDemographicsKioskResult Sut()
        {
            return new PatientDemographicsKioskResult();
        }

        [TestMethod]
        public void WhenCountyAndStateAreEmpty_ReturnsEmpty()
        {
            var sut = Sut();

            sut.CountyNameOfResidence = null;
            sut.CountyStateOfResidence = null;

            sut.CountyOfResidence.Should().BeEmpty(); 
        }

        [TestMethod]
        public void WhenSetCountyOfREsidence_ParcesStateCorrectly()
        {
            var sut = Sut();

            sut.CountyNameOfResidence = "Poquoson";
            sut.CountyStateOfResidence = "Virginia";

            sut.CountyOfResidence.Should().Be("Poquoson, Virginia");
        }


        [TestMethod]
        public void WhenIndependentCity_ParcesNameCorrectly()
        {
            var sut = Sut();

            sut.CountyOfResidence = "Waynesboro, City of, Virginia";

            sut.CountyNameOfResidence.Should().Be("Waynesboro, City of");

        }

        [TestMethod]
        public void WhenIndependentCity_PersistCorrectly()
        {
            var sut = Sut();

            sut.CountyOfResidence = "Waynesboro, City of, Virginia";

            sut.CountyOfResidence.Should().Be("Waynesboro, City of, Virginia");
        }

        [TestMethod]
        public void WhenIndependentCity_ParcesStateCorrectly()
        {
            var sut = Sut();

            sut.CountyOfResidence = "Waynesboro, City of, Virginia";

            sut.CountyStateOfResidence.Should().Be("Virginia");
        }

        [TestMethod]
        public void WhenIndependentCityAndSpecialCharacter_ParcesNameCorrectly()
        {
            var sut = Sut();

            sut.CountyOfResidence = "St. Louis, City of, Missouri";

            sut.CountyNameOfResidence.Should().Be("St. Louis, City of");
        }

        [TestMethod]
        public void WhenStateIsEmpty_CountyRendersCorrectly()
        {
            var sut = Sut();

            sut.CountyNameOfResidence = "Organized Village of Grayling (Holikachuk)";

            sut.CountyOfResidence.Should().Be("Organized Village of Grayling (Holikachuk)");
        }




    }
}

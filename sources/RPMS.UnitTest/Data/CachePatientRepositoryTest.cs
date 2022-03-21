using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RPMS.Data;
using RPMS.Common;
using RPMS.Common.Models;

namespace RPMS.UnitTest.Data
{
    [TestClass]
    [Ignore]
    public class CachePatientRepositoryTest
    {
        [TestMethod]
        public void GetMatchedPatients_Always_Returns_List()
        {

            CachePatientRepository repository = new CachePatientRepository();
            var matches = repository.GetMatchedPatients(new Patient { LastName = "Bla,Bla-bla", DateOfBirth = DateTime.Now });
            Assert.IsNotNull(matches, "Result should be null");
        }

        [TestMethod]
        public void GetMatchedPatientsCount_Returns_Zero()
        {

            CachePatientRepository repository = new CachePatientRepository();
            var matches = repository.GetMatchedPatientsCount(new Patient{LastName = "Bla,Bla-bla", DateOfBirth = DateTime.Now});
            Assert.AreEqual(0, matches);
        }

        [TestMethod]
        public void GetMatchedPatients_Can_Parse_Patient()
        {

            CachePatientRepository repository = new CachePatientRepository();

            Patient expected = new Patient
            {
                ID = 12497,
                EHR = "12683",
                LastName = "GARERD",
                FirstName = "ADELA",
                MiddleName = null,
                DateOfBirth = new DateTime(1965, 9, 9),
                City = "AMADOR COUNTY",
                StateID = "CA",
                ZipCode = "92061",
                StreetAddress = "26028 NORTH LAKE WOHLFORD",
                PhoneHome = "760 742-3257",
                PhoneOffice = "760 741-5766"

            };

            var matches = repository.GetMatchedPatients(expected);

            Assert.IsNotNull(matches, "Result is null");
            Assert.AreEqual(1, matches.Count, "Matched rows are not equal to 1");

            AssertPatient(expected, matches[0]);
        }

        protected void AssertPatient(Patient expected, Patient actual)
        {
            Assert.AreEqual(expected.ID, actual.ID, "ID not equal");
            Assert.AreEqual(expected.EHR, actual.EHR, "EHR not equal");
            Assert.AreEqual(expected.LastName, actual.LastName, "LastName not equal");
            Assert.AreEqual(expected.FirstName, actual.FirstName, "FirstName not equal");
            Assert.AreEqual(expected.MiddleName, actual.MiddleName, "MiddleName not equal");
            Assert.AreEqual(expected.StreetAddress, actual.StreetAddress, "StreetAddress not equal");

            Assert.AreEqual(expected.StreetAddressLine1, actual.StreetAddressLine1, "StreetAddressLine1 not equal");
            Assert.AreEqual(expected.StreetAddressLine2, actual.StreetAddressLine2, "StreetAddressLine2 not equal");
            Assert.AreEqual(expected.StreetAddressLine3, actual.StreetAddressLine3, "StreetAddressLine3 not equal");


            Assert.AreEqual(expected.StateID, actual.StateID, "StateID not equal");
            Assert.AreEqual(expected.ZipCode, actual.ZipCode, "ZipCode not equal");
            Assert.AreEqual(expected.PhoneHome, actual.PhoneHome, "PhoneHome not equal");
            Assert.AreEqual(expected.PhoneOffice, actual.PhoneOffice, "PhoneOffice not equal");
            //Assert.AreEqual(expected.Phone3, actual.Phone3, "Phone3 not equal");
            Assert.AreEqual(expected.DateOfBirth, actual.DateOfBirth, "DateOfBirth not equal");
            Assert.AreEqual(expected.City, actual.City, "City not equal");
        }

    }
}

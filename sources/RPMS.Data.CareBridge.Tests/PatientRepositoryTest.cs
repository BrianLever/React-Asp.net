using System;
using System.Collections.Generic;
using System.Net.Http;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using RPMS.Common.Models;
using RPMS.Common.Security;
using RPMS.Data.CareBridge.Infrastructure;
using RPMS.Tests.MotherObjects;

namespace RPMS.Data.CareBridge.Tests
{
    /// <summary>
    ///     This is a test class for BmxPatientRepositoryTest and is intended
    ///     to contain all BmxPatientRepositoryTest Unit Tests
    /// </summary>
    [TestClass]
    [TestCategory("Rpms_Integration")]
    [TestCategory("CareBridge_NextGen")]

    public class PatientRepositoryTest
    {
        private readonly Mock<IApiCredentialsService> _carebridgeCredentialsServiceMock = new Mock<IApiCredentialsService>();
        private readonly Mock<IHttpService> _httpServiceInterfaceMock = new Mock<IHttpService>();
        private readonly Mock<HttpService> _httpServiceClientMock = new Mock<HttpService>();


        public PatientRepositoryTest()
        {
            _carebridgeCredentialsServiceMock.Setup(x => x.GetCredentials())
                    .Returns(new BasicAuthCredentials { Username = "user", Password = "passw" });

            _httpServiceInterfaceMock.Setup(x => x.Create()).Returns(_httpServiceClientMock.Object);


            var patientSearchResponse = @"
{
  ""patients"": [
    {
      ""RowID"": ""1661"",
      ""HealthRecordNumber"": ""000000000539"",
      ""PatientName"": {
                       ""LastName"": ""Test"",
                       ""FirstName"": ""Dental Interview 5"",
                       ""MiddleName"": """"
                   },
                   ""DOB"": ""19720715"",
                   ""StreetAddress"": {
        ""Address1"": ""1305 Remington Road"",
        ""Address2"": ""Suite P"",
        ""City"": ""Schaumburg"",
        ""State"": ""IL "",
        ""Zip"": ""60173""
      },
      ""HomePhone"": ""8474906869"",
      ""OfficePhone"": """"
    },
    {
      ""RowID"": ""1574"",
      ""HealthRecordNumber"": ""000000000465"",
      ""PatientName"": {
        ""LastName"": ""Test"",
        ""FirstName"": ""Mother"",
        ""MiddleName"": """"
      },
      ""DOB"": ""19650201"",
      ""StreetAddress"": {
        ""Address1"": ""101 Main St"",
        ""Address2"": """",
        ""City"": ""Chandler"",
        ""State"": ""AZ"",
        ""Zip"": ""85226""
      },
      ""HomePhone"": ""4805555555"",
      ""OfficePhone"": ""4802222222""
    },
    {
      ""RowID"": ""1664"",
      ""HealthRecordNumber"": ""000000000542"",
      ""PatientName"": {
        ""LastName"": ""Test"",
        ""FirstName"": ""Mary"",
        ""MiddleName"": """"
      },
      ""DOB"": ""19500101"",
      ""StreetAddress"": {
        ""Address1"": ""123 Test Road"",
        ""Address2"": """",
        ""City"": ""Schaumburg"",
        ""State"": ""IL "",
        ""Zip"": ""60173""
      },
      ""HomePhone"": ""8474906869"",
      ""OfficePhone"": """"
    }
   ]
}";

            var responseMsg = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(patientSearchResponse)
            };

     
            _httpServiceClientMock.Setup(x => x.Post("FindPatient", It.IsAny<HttpContent>()))
                .Returns(responseMsg);

        }

        private PatientRepository CreateRepository()
        {
            return new PatientRepository(_carebridgeCredentialsServiceMock.Object, 
                _httpServiceInterfaceMock.Object,
                "https://localhost/CareBridgeAPI/");
        }


        private Patient GetPatientSearchPatern()
        {
            return PatientMotherObject.GetMaryTest();
            /*
            return new Patient
            {
                ID = 1664,
                LastName = "Test",
                FirstName = "Mary",
                EHR = "000000000542",
                DateOfBirth = new DateTime(1950, 01, 01),
                StreetAddress = "123 Test Road",
                PhoneHome = "8474906869",
                PhoneOffice = "",
                StateID = "NV",
                ZipCode = "89436",
                City = "Schaumburg"
            };
            */
        }

        private void AssertPatientEntity(Patient expected, Patient actual)
        {
            actual.FullName.Should().Be(expected.FullName, "Fullname should match");
            actual.ID.Should().Be(expected.ID, "ID should match");
            actual.EHR.Should().Be(expected.EHR, "EHR should match");
            actual.DateOfBirth.Should().Be(expected.DateOfBirth, "DateOfBirth should match");
            actual.PhoneOffice.Should().Be(expected.PhoneOffice, "PhoneOffice should match");
        }

        /// <summary>
        ///     A test for GetMatchedPatients
        /// </summary>
        [TestMethod]
        public void GetMatchedPatientsCountTest()
        {
            PatientRepository target = CreateRepository();

            Patient patientSearchPatern = GetPatientSearchPatern();

            int actual;
            actual = target.GetMatchedPatientsCount(patientSearchPatern);

            actual.Should().Be(1);
        }


        [TestMethod]
        public void GetMatchedPatientsTest()
        {
            PatientRepository target = CreateRepository();

            Patient patientSearchPatern = GetPatientSearchPatern();

            var expected = new List<Patient> { patientSearchPatern };

            List<Patient> actual;
            actual = target.GetMatchedPatients(patientSearchPatern);

            actual.Count.Should().Be(expected.Count, "Count failed");

            AssertPatientEntity(expected[0], actual[0]);
        }

        /*
        /// <summary>
        ///     A test for UpdatePatientRecordFields
        /// </summary>
        [TestMethod]
        public void Can_Update_All_Fields_Except_line2_line3_in_Address()
        {
            int patientId = 16009; //DEMO,ANDREA
            int rand = (new Random()).Next() % 10;

            PatientRepository target = CreateRepository();
            var modifications = new List<PatientRecordModification>
                        {
                                new PatientRecordModification
                                {
                                        Field = PatientRecordExportFields.AddressLine1,
                                        CurrentValue = "6385 WEST OTTAWA",
                                        UpdateWithValue = "6386 WEST OTTAWA " + rand
                                },
                                new PatientRecordModification
                                {
                                        Field = PatientRecordExportFields.City,
                                        CurrentValue = "SPANISH SPRINGS",
                                        UpdateWithValue = "SPANISH " + rand
                                },
                                new PatientRecordModification
                                {
                                        Field = PatientRecordExportFields.ZipCode,
                                        CurrentValue = "89436",
                                        UpdateWithValue = "9210" + rand
                                },
                                new PatientRecordModification
                                {
                                        Field = PatientRecordExportFields.StateID,
                                        CurrentValue = "NV",
                                        UpdateWithValue = "CALIFORNIA"
                                },
                                new PatientRecordModification
                                {
                                        Field = PatientRecordExportFields.Phone,
                                        CurrentValue = "(775) 219-8620",
                                        UpdateWithValue = "(775) 219-000" + rand
                                },
                        };
            int expected = 1;
            int actual;
            actual = target.UpdatePatientRecordFields(modifications, patientId, 0);
            Assert.AreEqual(expected, actual);

            Patient patient = target.GetPatientRecord(patientId);

            Assert.IsNotNull(patient);

            Assert.AreEqual(modifications[0].UpdateWithValue, patient.StreetAddressLine1, "StreetAddressLine1 failed");
            Assert.AreEqual(modifications[1].UpdateWithValue, patient.City, "City failed");

            Assert.AreEqual(modifications[2].UpdateWithValue, patient.ZipCode, "ZipCode failed");
            Assert.AreEqual(modifications[4].UpdateWithValue, patient.PhoneHome, "Phone failed");
            Assert.AreEqual("CA", patient.StateID, "StateID failed");
        }

        /// A test for UpdatePatientRecordFields
        /// </summary>
        [TestMethod]
        public void Can_Update_Address_All_Lines()
        {
            int patientId = 16009; //DEMO,ANDREA
            int rand = (new Random()).Next() % 10;

            PatientRepository target = CreateRepository();
            var modifications = new List<PatientRecordModification>
                        {
                                new PatientRecordModification
                                {
                                        Field = PatientRecordExportFields.AddressLine1,
                                        CurrentValue = "6385 WEST OTTAWA",
                                        UpdateWithValue = "6386 WEST OTTAWA " + rand
                                },
                                new PatientRecordModification
                                {
                                        Field = PatientRecordExportFields.AddressLine2,
                                        CurrentValue = null,
                                        UpdateWithValue = "LINE 2"
                                },
                                new PatientRecordModification
                                {
                                        Field = PatientRecordExportFields.AddressLine3,
                                        CurrentValue = null,
                                        UpdateWithValue = "LINE 3"
                                },
                        };
            int expected = 1;
            int actual;
            actual = target.UpdatePatientRecordFields(modifications, patientId, 0);
            Assert.AreEqual(expected, actual);

            Patient patient = target.GetPatientRecord(patientId);

            Assert.IsNotNull(patient);

            Assert.AreEqual(modifications[0].UpdateWithValue, patient.StreetAddressLine1, "StreetAddressLine1 failed");
            Assert.AreEqual(modifications[1].UpdateWithValue, patient.StreetAddressLine2, "StreetAddressLine2 failed");
            Assert.AreEqual(modifications[2].UpdateWithValue, patient.StreetAddressLine3, "StreetAddressLine3 failed");
        }


        [TestMethod]
        public void Can_Update_Address_With_String_50_chars()
        {
            int patientId = 16009; //DEMO,ANDREA
            int rand = (new Random()).Next() % 10;

            PatientRepository target = CreateRepository();
            var modifications = new List<PatientRecordModification>
                        {
                                new PatientRecordModification
                                {
                                        Field = PatientRecordExportFields.AddressLine1,
                                        CurrentValue = "6385 WEST OTTAWA",
                                        UpdateWithValue = "6386 WEST OTTAWA  1111111111111111111111111111111" + rand
                                },
                        };
            int expected = 1;
            int actual;
            actual = target.UpdatePatientRecordFields(modifications, patientId, 0);
            Assert.AreEqual(expected, actual);

            Patient patient = target.GetPatientRecord(patientId);

            Assert.IsNotNull(patient);

            Assert.AreEqual(modifications[0].UpdateWithValue, patient.StreetAddressLine1, "StreetAddressLine1 failed");
        }

        /// <summary>
        ///     A test for UpdatePatientRecordFields
        /// </summary>
        [TestMethod]
        public void Can_Update_Address_With_String_35_chars()
        {
            int patientId = 16009; //DEMO,ANDREA
            int rand = (new Random()).Next() % 10;

            PatientRepository target = CreateRepository();
            var modifications = new List<PatientRecordModification>
                        {
                                new PatientRecordModification
                                {
                                        Field = PatientRecordExportFields.AddressLine1,
                                        CurrentValue = "6385 WEST OTTAWA",
                                        UpdateWithValue = "6386 WEST OTTAWA  1111111111111111" + rand
                                },
                        };
            int expected = 1;
            int actual;
            actual = target.UpdatePatientRecordFields(modifications, patientId, 0);
            Assert.AreEqual(expected, actual);

            Patient patient = target.GetPatientRecord(patientId);

            Assert.IsNotNull(patient);

            Assert.AreEqual(modifications[0].UpdateWithValue, patient.StreetAddressLine1, "StreetAddressLine1 failed");
        }

        /// <summary>
        ///     A test for UpdatePatientRecordFields
        /// </summary>
        [TestMethod]
        public void Can_Update_Address_With_String_40_chars()
        {
            int patientId = 16009; //DEMO,ANDREA
            int rand = (new Random()).Next() % 10;

            PatientRepository target = CreateRepository();
            var modifications = new List<PatientRecordModification>
                        {
                                new PatientRecordModification
                                {
                                        Field = PatientRecordExportFields.AddressLine1,
                                        CurrentValue = "6385 WEST OTTAWA",
                                        UpdateWithValue = "6386 WEST OTTAWA  111111111111111122222" + rand
                                },
                        };
            int expected = 1;
            int actual;
            actual = target.UpdatePatientRecordFields(modifications, patientId, 0);
            Assert.AreEqual(expected, actual);

            Patient patient = target.GetPatientRecord(patientId);

            Assert.IsNotNull(patient);

            Assert.AreEqual(modifications[0].UpdateWithValue, patient.StreetAddressLine1, "StreetAddressLine1 failed");
        }

        /// <summary>
        ///     A test for UpdatePatientRecordFields
        /// </summary>
        [TestMethod]
        public void Can_Update_Address_With_String_36_chars()
        {
            int patientId = 16009; //DEMO,ANDREA
            int rand = (new Random()).Next() % 10;

            PatientRepository target = CreateRepository();
            var modifications = new List<PatientRecordModification>
                        {
                                new PatientRecordModification
                                {
                                        Field = PatientRecordExportFields.AddressLine1,
                                        CurrentValue = "6385 WEST OTTAWA",
                                        UpdateWithValue = "6386 WEST OTTAWA  11111111111111112" + rand
                                },
                        };
            int expected = 1;
            int actual;
            actual = target.UpdatePatientRecordFields(modifications, patientId, 0);
            Assert.AreEqual(expected, actual);

            Patient patient = target.GetPatientRecord(patientId);

            Assert.IsNotNull(patient);

            Assert.AreEqual(modifications[0].UpdateWithValue, patient.StreetAddressLine1, "StreetAddressLine1 failed");
        }

        [TestMethod]
        public void Can_Update_Address_The_Longest_line2_in_rpms()
        {
            int patientId = 16009; //DEMO,ANDREA
            int rand = (new Random()).Next() % 10;

            PatientRepository target = CreateRepository();
            var modifications = new List<PatientRecordModification>
                        {
                                new PatientRecordModification
                                {
                                        Field = PatientRecordExportFields.AddressLine1,
                                        CurrentValue = "6385 WEST OTTAWA",
                                        UpdateWithValue = "P.O. BOX 82 100100 TERMINATOR DR 100" + rand
                                },
                        };
            int expected = 1;
            int actual;
            actual = target.UpdatePatientRecordFields(modifications, patientId, 0);
            Assert.AreEqual(expected, actual);

            Patient patient = target.GetPatientRecord(patientId);

            Assert.IsNotNull(patient);

            Assert.AreEqual(modifications[0].UpdateWithValue, patient.StreetAddressLine1, "StreetAddressLine1 failed");
        }

        /// <summary>
        ///     A test for UpdatePatientRecordFields
        /// </summary>
        [TestMethod]
        public void Can_Update_Address_With_String_35_chars_and_line_2()
        {
            int patientId = 16009; //DEMO,ANDREA
            int rand = (new Random()).Next() % 10;

            PatientRepository target = CreateRepository();
            var modifications = new List<PatientRecordModification>
                        {
                                new PatientRecordModification
                                {
                                        Field = PatientRecordExportFields.AddressLine1,
                                        CurrentValue = "6385 WEST OTTAWA",
                                        UpdateWithValue = "6386 WEST OTTAWA  1111111111111111" + rand
                                },
                                new PatientRecordModification
                                {
                                        Field = PatientRecordExportFields.AddressLine2,
                                        CurrentValue = null,
                                        UpdateWithValue = "LINE 2"
                                },
                        };
            int expected = 1;
            int actual;
            actual = target.UpdatePatientRecordFields(modifications, patientId, 0);
            Assert.AreEqual(expected, actual);

            Patient patient = target.GetPatientRecord(patientId);

            Assert.IsNotNull(patient);

            Assert.AreEqual(modifications[0].UpdateWithValue, patient.StreetAddressLine1, "StreetAddressLine1 failed");
        }
        */
    }
}
using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RPMS.Common.Models;
using RPMS.Common.Security;

namespace RPMS.Data.BMXNet.Tests
{
    /// <summary>
    ///     This is a test class for BmxPatientRepositoryTest and is intended
    ///     to contain all BmxPatientRepositoryTest Unit Tests
    /// </summary>
    [TestClass]

    public class BmxPatientRepositoryTest
    {
        private readonly Mock<IRpmsCredentialsService> _rpmsCredentialsServiceMock = new Mock<IRpmsCredentialsService>();

        public BmxPatientRepositoryTest()
        {
            _rpmsCredentialsServiceMock.Setup(x => x.GetCredentials())
                    .Returns(new RpmsCredentials { AccessCode = "rpullen1", VerifyCode = "Frontdesk-05" });

            _rpmsCredentialsServiceMock.Setup(x => x.GetCredentialsCached())
                    .Returns(new RpmsCredentials { AccessCode = "rpullen1", VerifyCode = "Frontdesk-05" });
        }


        private BmxPatientRepository CreateRepository()
        {
            return new BmxPatientRepository(_rpmsCredentialsServiceMock.Object);
        }


        private Patient GetPatientSearchPatern()
        {
            return new Patient
            {
                ID = 16009,
                LastName = "DEMO",
                FirstName = "ANDREA",
                EHR = "908070",
                DateOfBirth = new DateTime(1972, 2, 22),
                StreetAddress = "6385 WEST OTTAWA",
                PhoneHome = "(775) 219-8620",
                PhoneOffice = "775.788.7600",
                StateID = "NV",
                ZipCode = "89436",
                City = "SPANISH SPRINGS"
            };
        }

        private void AssertPatientEntity(Patient expected, Patient actual)
        {
            Assert.AreEqual(expected.FullName, actual.FullName, "Fullname failed");
            Assert.AreEqual(expected.ID, actual.ID, "ID failed");
            Assert.AreEqual(expected.EHR, actual.EHR, "EHR failed");
            Assert.AreEqual(expected.DateOfBirth, actual.DateOfBirth, "DateOfBirth failed");
            Assert.AreEqual(expected.PhoneOffice, actual.PhoneOffice, "PhoneOffice failed");
        }

        /// <summary>
        ///     A test for GetMatchedPatients
        /// </summary>
        [TestMethod]
        [Ignore]
        public void GetMatchedPatientsCountTest()
        {
            BmxPatientRepository target = CreateRepository();

            Patient patientSearchPatern = GetPatientSearchPatern();

            int actual;
            actual = target.GetMatchedPatientsCount(patientSearchPatern);

            Assert.AreEqual(1, actual);
        }


        [TestMethod]
        [Ignore]
        public void GetMatchedPatientsTest()
        {
            BmxPatientRepository target = CreateRepository();

            Patient patientSearchPatern = GetPatientSearchPatern();

            var expected = new List<Patient> { patientSearchPatern };

            List<Patient> actual;
            actual = target.GetMatchedPatients(patientSearchPatern);

            Assert.AreEqual(expected.Count, actual.Count, "Count failed");

            AssertPatientEntity(expected[0], actual[0]);
        }

        /// <summary>
        ///     A test for UpdatePatientRecordFields
        /// </summary>
        [TestMethod]
        [Ignore]
        public void Can_Update_All_Fields_Except_line2_line3_in_Address()
        {
            int patientId = 16009; //DEMO,ANDREA
            var patientSearch = new PatientSearch
            {
                ID = patientId
            };
            int rand = (new Random()).Next() % 10;

            BmxPatientRepository target = CreateRepository();
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

            Patient patient = target.GetPatientRecord(patientSearch);

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
        [Ignore]
        public void Can_Update_Address_All_Lines()
        {
            int patientId = 16009; //DEMO,ANDREA
            var patientSearch = new PatientSearch
            {
                ID = patientId
            };

            int rand = (new Random()).Next() % 10;

            BmxPatientRepository target = CreateRepository();
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

            Patient patient = target.GetPatientRecord(patientSearch);

            Assert.IsNotNull(patient);

            Assert.AreEqual(modifications[0].UpdateWithValue, patient.StreetAddressLine1, "StreetAddressLine1 failed");
            Assert.AreEqual(modifications[1].UpdateWithValue, patient.StreetAddressLine2, "StreetAddressLine2 failed");
            Assert.AreEqual(modifications[2].UpdateWithValue, patient.StreetAddressLine3, "StreetAddressLine3 failed");
        }

        [TestMethod]
        public void Can_create_connection_from_credentials_section()
        {
            var credentials = new RpmsCredentials
            {
                AccessCode = "accessCode1",
                VerifyCode = "myCode",
                ExpireAt = new DateTime(2013, 05, 30)
            };

            _rpmsCredentialsServiceMock.Setup(x => x.GetCredentialsCached())
                 .Returns(credentials);

            var repo = CreateRepository();

            repo.Proxy.ConnectionParams.AccessCode.Should().Be(credentials.AccessCode);

            repo.Proxy.ConnectionParams.VerifyCode.Should().Be(credentials.VerifyCode);
        }

        [TestMethod]
        public void Can_create_connection_with_credentials_section_when_connection_not_open()
        {
            var credentials = new RpmsCredentials
            {
                AccessCode = "accessCode1",
                VerifyCode = "myCode",
                ExpireAt = new DateTime(2013, 05, 30)
            };

            _rpmsCredentialsServiceMock.Setup(x => x.GetCredentialsCached())
                 .Returns(credentials);

            var repo = CreateRepository();

            repo.Proxy.ConnectionParams.AccessCode.Should().Be(credentials.AccessCode);
            repo.Proxy.ConnectionParams.VerifyCode.Should().Be(credentials.VerifyCode);

            //now create with another credentials
            credentials = new RpmsCredentials
            {
                AccessCode = "accessCode1",
                VerifyCode = "myCode2",
                ExpireAt = new DateTime(2013, 05, 30)
            };

            _rpmsCredentialsServiceMock.Setup(x => x.GetCredentialsCached())
                 .Returns(credentials);

            repo = CreateRepository();

            repo.Proxy.ConnectionParams.VerifyCode.Should().Be(credentials.VerifyCode, "verify code should change");

        }

        [TestMethod]
        public void Can_reuse_existing_connection_with_credentials_are_the_same()
        {
            var credentials = new RpmsCredentials
            {
                AccessCode = "accessCode1",
                VerifyCode = "myCode",
                ExpireAt = new DateTime(2013, 05, 30)
            };

            _rpmsCredentialsServiceMock.Setup(x => x.GetCredentialsCached())
                 .Returns(credentials);

            var repo = CreateRepository();

            var proxy1 = repo.Proxy;

            repo = CreateRepository();

            var proxy2 = repo.Proxy;

            proxy1.Should().IsSameOrEqualTo(proxy2);

        }


        [TestMethod]
        [Ignore]
        public void Can_Update_Address_With_String_50_chars()
        {
            int patientId = 16009; //DEMO,ANDREA
            var patientSearch = new PatientSearch
            {
                ID = patientId
            };
            int rand = (new Random()).Next() % 10;

            BmxPatientRepository target = CreateRepository();
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

            Patient patient = target.GetPatientRecord(patientSearch);

            Assert.IsNotNull(patient);

            Assert.AreEqual(modifications[0].UpdateWithValue, patient.StreetAddressLine1, "StreetAddressLine1 failed");
        }

        /// <summary>
        ///     A test for UpdatePatientRecordFields
        /// </summary>
        [TestMethod]
        [Ignore]
        public void Can_Update_Address_With_String_35_chars()
        {
            int patientId = 16009; //DEMO,ANDREA
            var patientSearch = new PatientSearch
            {
                ID = patientId
            };

            int rand = (new Random()).Next() % 10;

            BmxPatientRepository target = CreateRepository();
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

            Patient patient = target.GetPatientRecord(patientSearch);

            Assert.IsNotNull(patient);

            Assert.AreEqual(modifications[0].UpdateWithValue, patient.StreetAddressLine1, "StreetAddressLine1 failed");
        }

        /// <summary>
        ///     A test for UpdatePatientRecordFields
        /// </summary>
        [Ignore]
        [TestMethod]
        public void Can_Update_Address_With_String_40_chars()
        {
            int patientId = 16009; //DEMO,ANDREA
            var patientSearch = new PatientSearch
            {
                ID = patientId
            };
            int rand = (new Random()).Next() % 10;

            BmxPatientRepository target = CreateRepository();
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

            Patient patient = target.GetPatientRecord(patientSearch);

            Assert.IsNotNull(patient);

            Assert.AreEqual(modifications[0].UpdateWithValue, patient.StreetAddressLine1, "StreetAddressLine1 failed");
        }

        /// <summary>
        ///     A test for UpdatePatientRecordFields
        /// </summary>
        [TestMethod]
        [Ignore]
        public void Can_Update_Address_With_String_36_chars()
        {
            int patientId = 16009; //DEMO,ANDREA
            var patientSearch = new PatientSearch
            {
                ID = patientId
            };

            int rand = (new Random()).Next() % 10;

            BmxPatientRepository target = CreateRepository();
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

            Patient patient = target.GetPatientRecord(patientSearch);

            Assert.IsNotNull(patient);

            Assert.AreEqual(modifications[0].UpdateWithValue, patient.StreetAddressLine1, "StreetAddressLine1 failed");
        }

        [TestMethod]
        [Ignore]
        public void Can_Update_Address_The_Longest_line2_in_rpms()
        {
            int patientId = 16009; //DEMO,ANDREA
            var patientSearch = new PatientSearch
            {
                ID = patientId
            };
            int rand = (new Random()).Next() % 10;

            BmxPatientRepository target = CreateRepository();
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

            Patient patient = target.GetPatientRecord(patientSearch);

            Assert.IsNotNull(patient);

            Assert.AreEqual(modifications[0].UpdateWithValue, patient.StreetAddressLine1, "StreetAddressLine1 failed");
        }

        /// <summary>
        ///     A test for UpdatePatientRecordFields
        /// </summary>
        [TestMethod]
        [Ignore]
        public void Can_Update_Address_With_String_35_chars_and_line_2()
        {
            int patientId = 16009; //DEMO,ANDREA
            var patientSearch = new PatientSearch
            {
                ID = patientId
            };

            int rand = (new Random()).Next() % 10;

            BmxPatientRepository target = CreateRepository();
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

            Patient patient = target.GetPatientRecord(patientSearch);

            Assert.IsNotNull(patient);

            Assert.AreEqual(modifications[0].UpdateWithValue, patient.StreetAddressLine1, "StreetAddressLine1 failed");
        }
    }
}
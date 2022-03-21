using RPMS.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Collections.Generic;
using Moq;
using RPMS.Data.FakeObjects;
using RPMS.Common.Models;

namespace RPMS.UnitTest.Common
{


    /// <summary>
    ///This is a test class for PatientServiceTest and is intended
    ///to contain all PatientServiceTest Unit Tests
    ///</summary>

    public class PatientServiceTest
    {
        private Mock<IPatientRepository> _repository;


        [TestClass()]
        public class CanSelectPatient
        {
            private TestContext testContextInstance;
            /// <summary>
            ///Gets or sets the test context which provides
            ///information about and functionality for the current test run.
            ///</summary>
            public TestContext TestContext
            {
                get
                {
                    return testContextInstance;
                }
                set
                {
                    testContextInstance = value;
                }
            }

            #region Additional test attributes
           
            //
            //Use TestInitialize to run code before running each test
            [TestInitialize()]
            public void MyTestInitialize()
            {

                var patients = new List<Patient>{
                new Patient()
                {
                ID = 1,
                FirstName = "John",
                MiddleName = "",
                LastName = "Smith",
                DateOfBirth = new DateTime(1982, 1, 5),
                PhoneHome = "(111) 111-1111",
                StreetAddress = "5252 BALBOA",
                City = "SAN DIEGO",
                ZipCode = "10001",
                StateID = "CA"
            },
            new Patient()
            {
                ID = 2,
                FirstName = "John",
                MiddleName = "",
                LastName = "Smith",
                DateOfBirth = new DateTime(1902, 11, 5),
                PhoneHome = "+3 8(095) 555-6663",
                StreetAddress = "10, Riverside",
                City = "New York",
                ZipCode = "10001",
                StateID = "NY"
            },
            new Patient()
            {
                ID = 4,
                FirstName = "John",
                MiddleName = "",
                LastName = "Smith",
                DateOfBirth = new DateTime(1982, 12, 5),
                PhoneHome = "+3 8(095) 555-6664",
                StreetAddress = "22, Riverside",
                City = "New York",
                ZipCode = "10001",
                StateID = "NY"
            },
            new Patient()
            {
                ID = 5,
                FirstName = "John",
                MiddleName = "",
                LastName = "Smith",
                DateOfBirth = new DateTime(1999, 1, 5),
                PhoneHome = "+3 8(095) 555-6665",
                StreetAddress = "22, 96th St",
                City = "New York",
                ZipCode = "10001",
                StateID = "NY"
            },
            new Patient()
            {
                ID = 7,
                FirstName = "John",
                MiddleName = "",
                LastName = "Smith",
                DateOfBirth = new DateTime(1982, 1, 5),
                PhoneHome = "+3 8(095) 555-6666",
                StreetAddress = "2, 112th St",
                City = "California",
                ZipCode = "312433",
                StateID = "CA"
            },
            new Patient()
            {
                ID = 8,
                FirstName = "John",
                MiddleName = "",
                LastName = "Smith",
                DateOfBirth = new DateTime(1982, 1, 5),
                PhoneHome = "+3 8(095) 555-6667",
                StreetAddress = "55, Steirway St",
                City = "New York",
                ZipCode = "10001",
                StateID = "NY"
            },
            new Patient()
            {
                ID = 9,
                FirstName = "John",
                MiddleName = "",
                LastName = "Smith",
                DateOfBirth = new DateTime(1977, 1, 5),
                PhoneHome = "+3 8(095) 555-6668",
                StreetAddress = "11, Corona Ave",
                City = "New York",
                ZipCode = "10001",
                StateID = "NY"
            },
            new Patient()
            {
                ID = 10,
                FirstName = "John",
                MiddleName = "",
                LastName = "Smith",
                DateOfBirth = new DateTime(1963, 1, 5),
                PhoneHome = "+3 8(095) 555-6669",
                StreetAddress = "99, Park Ave",
                City = "New York",
                ZipCode = "10001",
                StateID = "NY"
            },
            new Patient()
            {
                ID = 11,
                FirstName = "John",
                MiddleName = "",
                LastName = "Smith",
                DateOfBirth = new DateTime(1982, 1, 5),
                PhoneHome = "+3 8(095) 555-66612",
                StreetAddress = "88, Park Ave",
                City = "New York",
                ZipCode = "10001",
                StateID = "NY"
            },
            new Patient()
            {
                ID = 12,
                FirstName = "John",
                MiddleName = "",
                LastName = "Smith",
                DateOfBirth = new DateTime(1982, 1, 5),
                PhoneHome = "+3 8(095) 555-66622",
                StreetAddress = "2, Green Point Ave",
                City = "New York",
                ZipCode = "10001",
                StateID = "NY"
            },
            new Patient()
            {
                ID = 13,
                FirstName = "John",
                MiddleName = "",
                LastName = "Smith",
                DateOfBirth = new DateTime(1895, 1, 5),
                PhoneHome = "+3 8(095) 555-66656",
                StreetAddress = "55, Green Point Ave",
                City = "New York",
                ZipCode = "10001",
                StateID = "NY"
            },
            new Patient()
            {
                ID = 14,
                FirstName = "John",
                MiddleName = "",
                LastName = "Smith",
                DateOfBirth = new DateTime(1982, 1, 5),
                PhoneHome = "+3 8(095) 555-1163",
                StreetAddress = "20, Green Point Ave",
                City = "New York",
                ZipCode = "10001",
                StateID = "NY"
            },
            new Patient()
            {
                ID = 15,
                FirstName = "John",
                MiddleName = "",
                LastName = "Smith",
                DateOfBirth = new DateTime(1982, 1, 5),
                PhoneHome = "+3 8(095) 555-12663",
                StreetAddress = "22, E 34th St",
                City = "New York",
                ZipCode = "10001",
                StateID = "NY"
            }};
            }

            //Use TestCleanup to run code after each test has run
            //[TestCleanup()]
            //public void MyTestCleanup()
            //{
            //}
            //
            #endregion


            /// <summary>
            ///A test for GetMatchedPatients
            ///</summary>
            [TestMethod()]
            public void GetMatchedPatientsTest()
            {
                IPatientRepository repository = new FakePatientRepository();
                PatientService target = new PatientService(repository);
                Patient searchPattern = new Patient
                {
                    LastName = "GARERD",
                    FirstName = "ADELA",
                    DateOfBirth = new DateTime(1965, 9, 9),
                    StreetAddress = "Street",
                    PhoneHome = "111-111-11111",
                    StateID = "CA",
                    ZipCode = "92061",
                    City = "AMADOR COUNTY"

                };
                int startRow = 1;
                int rowsPerPage = 5;

                List<Patient> actual;
                actual = target.GetMatchedPatients(searchPattern, startRow, rowsPerPage);
                Assert.AreEqual(1, actual.Count);
            }

            /// <summary>
            ///A test for GetMatchedPatientsCount
            ///</summary>
            [TestMethod()]
            public void GetMatchedPatientsCountTest()
            {
                IPatientRepository repository = new FakePatientRepository();
                PatientService target = new PatientService(repository);
                Patient searchPattern = new Patient
                {
                    LastName = "GARERD",
                    FirstName = "ADELA",
                    DateOfBirth = new DateTime(1965, 9, 9),
                    StreetAddress = "Street",
                    PhoneHome = "111-111-11111",
                    StateID = "CA",
                    ZipCode = "92061",
                    City = "AMADOR COUNTY"

                };
                int expected = 2;
                int actual;
                actual = target.GetMatchedPatientsCount(searchPattern);
                Assert.AreEqual(expected, actual);

            }
        }

        [TestClass]
        public class CanSortPatient
        {
            private Mock<IPatientRepository> _repository;

            protected Patient CreatePatient()
            {
                return new Patient
                {
                    LastName = "GARERD",
                    FirstName = "CHRIS",
                    MiddleName = "SR.",
                    DateOfBirth = new DateTime(1965, 9, 9),
                    StreetAddress = "Street",
                    PhoneHome = "111-111-11111",
                    StateID = "CA",
                    ZipCode = "92061",
                    City = "AMADOR COUNTY"

                };
            }


            public CanSortPatient()
            {
                _repository = new Mock<IPatientRepository>();

                _repository.Setup(x => x.GetMatchedPatients(It.IsAny<Patient>())).Returns(
                    new List<Patient>{
                        new Patient{
                            ID = 1, 
                            LastName = "GARERD",
                            FirstName = "CHRISTOPHER", //contains
                            MiddleName = "ANTONY SR.", //diff
                            DateOfBirth = new DateTime(1965, 9, 9),
                            StreetAddress = "Street",
                            PhoneHome = "111-111-11111",
                            StateID = "CA",
                            ZipCode = "92061",
                            City = "AMADOR COUNTY"
                        },
                        new Patient{
                            ID = 3, 
                            LastName = "GARERD",
                            FirstName = "ADELA", //diff
                            MiddleName = null, //diff
                            DateOfBirth = new DateTime(1966, 5, 12), //diff
                            StreetAddress = "Street",
                            PhoneHome = "111-111-11111",
                            StateID = "CA",
                            ZipCode = "92061",
                            City = "AMADOR COUNTY"
                        },
                        new Patient{
                            ID = 2, 
                            LastName = "GARERD",
                            FirstName = "ADELA", //diff
                            MiddleName = null, //diff
                            DateOfBirth = new DateTime(1965, 9, 9),
                            StreetAddress = "Street",
                            PhoneHome = "111-111-11111",
                            StateID = "CA",
                            ZipCode = "92061",
                            City = "AMADOR COUNTY"
                        },
                        
                        new Patient{
                            ID = 4, 
                            LastName = "GARERD",
                            FirstName = "CHRIS",
                            MiddleName = null, //diff
                            DateOfBirth = new DateTime(1965, 9, 9),
                            StreetAddress = "Fake Street", //diff
                            PhoneHome = "111-111-11111",
                            StateID = "CA",
                            ZipCode = "92061",
                            City = "AMADOR COUNTY"
                        },
                        new Patient{
                            ID = 5, 
                            LastName = "GARERD",
                            FirstName = "CHRIS",
                            MiddleName = null, //diff
                            DateOfBirth = new DateTime(1965, 9, 9),
                            StreetAddress = "Fake Street",  //diff
                            PhoneHome = "222-222-11111",  //diff
                            StateID = "NY", //diff
                            ZipCode = "92161",
                            City = "FAKE COUNTY"  //diff
                        },
                        new Patient
                        {
                            ID = 8,
                            LastName = "GARERD",
                            FirstName = "CHRIS",
                            MiddleName = "SR.",
                            DateOfBirth = new DateTime(1965, 9, 9),
                            StreetAddress = "Street",
                            PhoneHome = "111-111-11111",
                            StateID = "CA",
                            ZipCode = "92061",
                            City = "AMADOR COUNTY"

                        },
                        new Patient{
                            ID = 6, 
                            LastName = "GARERD",
                            FirstName = "CHRISTOPHER",  //contains
                            MiddleName = "SR.",
                           DateOfBirth = new DateTime(1965, 9, 9),
                            StreetAddress = "Fake Street",  //diff
                            PhoneHome = "222-222-11111",  //diff
                            StateID = "NV",
                            ZipCode = "92061",
                            City = "AMADOR COUNTY"
                        },


                    });
            }

            [TestMethod]
            public void Can_Sort_Patients_Corrently()
            {
                PatientService target = new PatientService(_repository.Object);
                Patient searchPattern = CreatePatient();

                var patientList = target.GetMatchedPatients(searchPattern, 0, 30);

                int[] expectedOrder = new int[] { 8, 4, 5, 6, 1, 2, 3 };

                for (int i = 0; i < patientList.Count; i++)
                {
                    if (patientList[i].ID != expectedOrder[i])
                    {
                        Assert.Fail("Expected <{0}>, actual <{1}>. Full ordered list: {2}",
                            expectedOrder[i],
                            patientList[i].ID,
                            string.Join(", ", patientList.Select(x => x.ID))
                            );
                    }
                }
            }


            [TestMethod]
            public void Pagin_with_Sort_Works_Corrently()
            {
                PatientService target = new PatientService(_repository.Object);
                Patient searchPattern = CreatePatient();
                int[] expectedOrder = new int[] { 5, 6, 1, 2 };
                
                var patientList = target.GetMatchedPatients(searchPattern, 2, 4);


                Assert.AreEqual(expectedOrder.Length, patientList.Count, "Count failed");

                for (int i = 0; i < patientList.Count; i++)
                {
                    if (patientList[i].ID != expectedOrder[i])
                    {
                        Assert.Fail("Expected <{0}>, actual <{1}>. Full ordered list: {2}",
                            expectedOrder[i],
                            patientList[i].ID,
                            string.Join(", ", patientList.Select(x => x.ID))
                            );
                    }
                }
            }




        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPMS.Common;
using RPMS.Common.Models;

namespace RPMS.Data.FakeObjects
{
    public class FakePatientRepository : IPatientRepository
    {
        static List<Patient> patients = new List<Patient>
        {
            new Patient
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

            },
            new Patient
            {
                ID = 12400,
                EHR = "29601",
                LastName = "GARERD",
                FirstName = "ADELA",
                MiddleName = null,
                DateOfBirth = new DateTime(1965, 9, 9),
                City = "SAN DIEGO",
                StateID = "CA",
                ZipCode = "92061",
                StreetAddress = "26028 NORTH LAKE WOHLFORD",
                PhoneHome = "111 111-1111",
                PhoneOffice = ""

            },
             
            new Patient
            {
                ID = 12401,
                EHR = "29606",
                LastName = "KRYSHTOP",
                FirstName = "SERGII",
                MiddleName = null,
                DateOfBirth = new DateTime(1983, 12, 4),
                City = "SAN DIEGO",
                StateID = "CA",
                ZipCode = "92061",
                StreetAddress = "26028 NORTH LAKE WOHLFORD",
                PhoneHome = "111 111-1111",
                PhoneOffice = ""

            }
            /*
             *  "ID": 15988,
            "City": "LOS ANGELES",
            "Phone": "213-202-3970",
            "StateID": "CA",
            "StateName": "CA",
            "StreetAddress": "1125 WEST 6TH STREET, SUITE 103",
            "ZipCode": "90017",
            "Birthday": "1985-01-01T00:00:00",
            "FirstName": "PAM",
            "LastName": "DEMO",
            "MiddleName": null,
            "FullName": "DEMO, PAM",
            "Age": 36
            
             
            "ID": 15987,
            "City": "LOS ANGELES",
            "Phone": "213-688-7777",
            "StateID": "CA",
            "StateName": "CA",
            "StreetAddress": "1125 WEST 6TH STREET, SUITE 103",
            "ZipCode": "90017",
            "Birthday": "1985-01-01T00:00:00",
            "FirstName": "LORA",
            "LastName": "DEMO",
            "MiddleName": null,
            "FullName": "DEMO, LORA",
            "Age": 36



            "ID": 15994,
            "City": "LOS ANGELES",
            "Phone": "213-202-3970",
            "StateID": "CA",
            "StateName": "CA",
            "StreetAddress": "1125 W. 6TH ST, STE 103",
            "ZipCode": "90017",
            "Birthday": "1985-01-01T00:00:00",
            "FirstName": "ANGELA",
            "LastName": "DEMO",
            "MiddleName": null,
            "FullName": "DEMO, ANGELA",
            "Age": 36


             "ID": 15989,
            "City": "LOS ANGELES",
            "Phone": "2132023970",
            "StateID": "CA",
            "StateName": "CA",
            "StreetAddress": "1125 W 6TH",
            "ZipCode": "90017",
            "Birthday": "1985-01-01T00:00:00",
            "FirstName": "BONNIE",
            "LastName": "DEMO",
            "MiddleName": null,
            "FullName": "DEMO, BONNIE",
            "Age": 36
             */
        };


        public List<Patient> GetMatchedPatients(Patient patient)
        {
            if (patient == null)
            {
                throw new ArgumentNullException("patient");
            }

            return patients;
        }

        public int GetMatchedPatientsCount(Patient patient)
        {
            if (patient == null)
            {
                throw new ArgumentNullException("patient");
            }

            return patients.Count();
        }

        
        public Patient GetPatientRecord(PatientSearch patientSearch)
        {
            return patients.FirstOrDefault(p => p.ID == patientSearch.ID);
        }

        
        public int UpdatePatientRecordFields(IEnumerable<PatientRecordModification> modifications, int patientID, int visitID)
        {
            return 0;
        }

   


        public string GetPatientName(PatientSearch patientSearch)
        {
            return patients.FirstOrDefault(p => p.ID == patientSearch.ID).FullName;
        }
    }
}

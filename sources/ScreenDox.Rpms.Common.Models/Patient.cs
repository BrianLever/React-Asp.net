using System;
using System.Text;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using RPMS.Common.Comparers;
using FrontDesk;

namespace RPMS.Common.Models
{
    [DataContract(Name = "Patient", Namespace = "http://www.screendox.com")]
    public class Patient : PatientSearch, IComparable<Patient>
    {

        /// <summary>
        /// Electronic health record
        /// </summary>
        [DataMember]
        public string EHR { get; set; }


        /// <summary>
        /// Get or set full patient name in EHR format
        /// </summary>
        public string FullName
        {
            get
            {

                StringBuilder name = new StringBuilder();
                name.Append(LastName);
                name.Append(", ");
                name.Append(FirstName);
                if (!String.IsNullOrEmpty(MiddleName))
                {
                    name.Append(" ");
                    name.Append(MiddleName);
                }

                return name.ToString();

            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException("FullName cannot be empty string");

                value = value.Trim();
                Regex re = new Regex(@"\A(?<last>[^,]+),\s*(?<first>\S+?)(?:\s+(?<middle>.+))?\Z");
                var match = re.Match(value);

                if (match != null && match.Groups.Count >= 3)
                {
                    if (match.Groups["last"].Success)
                        this.LastName = match.Groups["last"].Value;
                    if (match.Groups["first"].Success)
                        this.FirstName = match.Groups["first"].Value;
                    if (match.Groups["middle"].Success)
                        this.MiddleName = match.Groups["middle"].Value;
                }
            }
        }


        [DataMember]
        public string PhoneHome { get; set; }

        [DataMember]
        public string PhoneOffice { get; set; }

        //[DataMember]
        //public string Phone3 { get; set; }

        #region Street Address

        /// <summary>
        /// Address line 1
        /// </summary>
        public string StreetAddressLine1 { get; set; }

        /// <summary>
        /// Address line 2
        /// </summary>
        public string StreetAddressLine2 { get; set; }

        /// <summary>
        /// Address line 3
        /// </summary>
        public string StreetAddressLine3 { get; set; }


        [DataMember]
        public string StreetAddress
        {
            get
            {
                StringBuilder address = new StringBuilder();
                address.Append(StreetAddressLine1);
                if (!String.IsNullOrEmpty(StreetAddressLine2))
                {
                    address.Append(" ");
                    address.Append(StreetAddressLine2);
                }
                if (!String.IsNullOrEmpty(StreetAddressLine3))
                {
                    address.Append(" ");
                    address.Append(StreetAddressLine3);
                }

                return address.ToString();
            }
            set
            {
                StreetAddressLine1 = value;
                StreetAddressLine2 = string.Empty;
                StreetAddressLine3 = string.Empty;
                //throw new NotImplementedException("Street address parsing is not implemented");
            }
        }

        #endregion


        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string StateID { get; set; }

        [DataMember]
        public string ZipCode { get; set; }

        /// <summary>
        /// Evaluate MatchRank property
        /// </summary>
        /// <param name="matchPattern">Matching patient record from FrontDesk database</param>
        public void SetMatchRank(Patient matchPattern)
        {
            if (matchPattern != null)
            {

                this.matchRank = new PatientComparer().GetMatchRank(this, matchPattern);
            }
        }


        #region IComparer<Patient> Members

        public int CompareTo(Patient other)
        {
            return new PatientComparer().Compare(this, other);
        }

        #endregion


    }



    public static class PatientExtensions
    {

        public static Patient ToPatient(this ScreeningResult screeningResult)
        {
            var patient = new Patient();
            if (screeningResult == null) return patient;

            patient.LastName = screeningResult.LastName;
            patient.FirstName = screeningResult.FirstName;
            patient.MiddleName = screeningResult.MiddleName;
            patient.DateOfBirth = screeningResult.Birthday;
            patient.StateID = screeningResult.StateID;
            patient.City = screeningResult.City;
            patient.ZipCode = screeningResult.ZipCode;
            patient.StreetAddress = screeningResult.StreetAddress;
            patient.PhoneHome = screeningResult.Phone;

            return patient;
        }



        public static Patient ToPatient(this PatientSearch patientSearch)
        {
            var patient = new Patient();
            if (patientSearch == null) return patient;

            patient.LastName = patientSearch.LastName;
            patient.FirstName = patientSearch.FirstName;
            patient.MiddleName = patientSearch.MiddleName;
            patient.DateOfBirth = patientSearch.DateOfBirth;
            patient.StateID = string.Empty;
            patient.City = string.Empty;
            patient.ZipCode = string.Empty;
            patient.StreetAddress = string.Empty;
            patient.PhoneHome = string.Empty;

            return patient;
        }

        public static Patient ToPatient(this Patient patient)
        {
            return patient;
        }


        public static PatientSearch Clone(this PatientSearch patient)
        {
            var clonedPatient = new PatientSearch();
            if (clonedPatient == null) return clonedPatient;

            clonedPatient.LastName = patient.LastName;
            clonedPatient.FirstName = patient.FirstName;
            clonedPatient.MiddleName = patient.MiddleName;
            clonedPatient.DateOfBirth = patient.DateOfBirth;

            return clonedPatient;
        }
    }
}

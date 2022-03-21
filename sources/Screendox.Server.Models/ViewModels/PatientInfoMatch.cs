using FrontDesk;
using FrontDesk.Common;
using FrontDesk.Common.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Models.ViewModels
{
    public struct PatientInfoField
    {
        public const string FirstName = "FirstName";
        public const string LastName = "LastName";
        public const string MiddleName = "MiddleName";
        public const string Birthday = "Birthday";
        public const string Phone = "Phone";
        public const string Street = "Street";
        public const string City = "City";
        public const string StateID = "StateID";
        public const string ZipCode = "ZipCode";
    }

    /// <summary>
    /// DTO for rendering Matched Patient list from EHR system
    /// </summary>
    public class PatientInfoMatch : PatientInfo
    {
        [DataMember]
        public List<string> NotMatchesFields { get; set; } = new List<string>();

        [DataMember]
        public int ID { get; set; }


        [DataMember]
        public string BirthdayFormatted
        {
            get
            {
                return Birthday.FormatAsDate();
            }
        }

        /// <summary>
        /// All non-matched fields in the selected matched results set in red color
        /// </summary>
        public void SetNotMatchesFields(IScreeningPatientIdentityWithAddress screendoxPatient)
        {
            this.NotMatchesFields.Clear();

            if (screendoxPatient == null) return;

            var patient = this;

            // Compare first name
            if (patient != null)
            {
                if (string.Compare(screendoxPatient.FirstName, patient.FirstName, StringComparison.OrdinalIgnoreCase) != 0)
                {
                    NotMatchesFields.Add(PatientInfoField.FirstName);
                }

                // Compare last name
                if (String.Compare(screendoxPatient.LastName, patient.LastName, StringComparison.OrdinalIgnoreCase) != 0)
                {
                    NotMatchesFields.Add(PatientInfoField.LastName);
                }
                // Compare middle name
                if (String.Compare(screendoxPatient.MiddleName, patient.MiddleName, StringComparison.OrdinalIgnoreCase) != 0)
                {
                    NotMatchesFields.Add(PatientInfoField.MiddleName);
                }

                // Compare birthday
                if (screendoxPatient.Birthday != patient.Birthday)
                {
                    NotMatchesFields.Add(PatientInfoField.Birthday);
                }
                // Compare phone
                if (string.Compare(screendoxPatient.Phone.AsRawPhoneNumber(), patient.Phone.AsRawPhoneNumber(), StringComparison.OrdinalIgnoreCase) != 0)
                {
                    NotMatchesFields.Add(PatientInfoField.Phone);
                }
                //Compressin street
                if (String.Compare(screendoxPatient.StreetAddress, patient.StreetAddress, StringComparison.OrdinalIgnoreCase) != 0)
                {
                    NotMatchesFields.Add(PatientInfoField.Street);
                }
                // Compare city
                if (String.Compare(screendoxPatient.City, patient.City, StringComparison.OrdinalIgnoreCase) != 0)
                {
                    NotMatchesFields.Add(PatientInfoField.City);
                }
                // Compare state
                if (String.Compare(screendoxPatient.StateID, patient.StateID, StringComparison.OrdinalIgnoreCase) != 0)
                {
                    NotMatchesFields.Add(PatientInfoField.StateID);
                }
                // Compare zip code
                if (String.Compare(screendoxPatient.ZipCode, patient.ZipCode, StringComparison.OrdinalIgnoreCase) != 0)
                {
                    NotMatchesFields.Add(PatientInfoField.ZipCode);
                }
            }
        }
    }
}

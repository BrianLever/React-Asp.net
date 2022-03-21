using System;
using System.Text;


namespace RPMS.Common.Models
{
    /// <summary>
    /// Extensions to Patient class
    /// </summary>
    public static class PatientNameExtensions
    {
        /// <summary>
        /// Get or set full patient name in EHR  format
        /// </summary>
        public static string FullName(this PatientSearch patient)
        {
            StringBuilder name = new StringBuilder();
            name.Append(patient.LastName);
            name.Append(",");
            name.Append(patient.FirstName);
            if (!String.IsNullOrEmpty(patient.MiddleName))
            {
                name.Append(" ");
                name.Append(patient.MiddleName);
            }

            return name.ToString();
        }

        public static PatientSearch SetBirthday(this PatientSearch patient, int year, int month, int day)
        {
            patient.DateOfBirth = new DateTime(year, month, day, 0, 0, 0, patient.DateOfBirth.Kind);

            return patient;
        }

        public static PatientSearch SetBirthday(this PatientSearch patient, DateTime birthday)
        {
            patient.DateOfBirth = birthday;

            return patient;
        }

        public static Patient CapitalizeName(this Patient patient)
        {
            if (patient == null)
            {
                return null;
            }

            patient.LastName = patient.LastName.ToUpperInvariant();
            patient.FirstName = patient.FirstName.ToUpperInvariant();
            patient.MiddleName = patient.MiddleName?.ToUpperInvariant() ?? string.Empty;

            return patient;
        }
    }
}
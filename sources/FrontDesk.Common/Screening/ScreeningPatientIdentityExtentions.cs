using System;
using System.Text;

namespace FrontDesk.Common.Screening
{
    /// <summary>
    /// Extentions for interface IScreeningPatientIdentityWithAddress
    /// </summary>
    public static class ScreeningPatientIdentityExtentions
    {
        /// <summary>
        /// Get full name
        /// </summary>
        /// <param name="patient"></param>
        /// <returns>String with the full name</returns>
        public static string GetFullName(this IPersonName patient)
        {
            if (patient is null)
            {
                return string.Empty ;
            }

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
    }
}

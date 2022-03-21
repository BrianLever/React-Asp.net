using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using RPMS.Common.Models;


namespace EhrInterface
{
    /// <summary>
    /// Extensions to Patient class
    /// </summary>
    public static class PatientExtensions
    {
        /// <summary>
        /// Get or set full patient name in EHR format
        /// </summary>
        public static string FullName(this Patient patient)
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
    }
}
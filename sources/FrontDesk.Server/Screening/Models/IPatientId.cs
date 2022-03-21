using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Screening.Models
{
    public interface IPatientId
    {
        string PatientName { get; }
        DateTime Birthday { get; }
    }

    public static class IPatientIdExtensions
    {
        public static string GetUniquePatientKey(this IPatientId patient)
        {
            if (patient == null) return String.Empty;

            return "{0:yyyyMMdd}|{1}".FormatWith(patient.Birthday, patient.PatientName);
        }
    }
}

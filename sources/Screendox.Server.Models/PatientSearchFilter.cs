using FrontDesk;

using System;

namespace ScreenDox.Server.Models
{
    /// <summary>
    /// Search parameters to find existing Patient in Screendox or EHR
    /// </summary>
    public class PatientSearchFilter : IScreeningPatientIdentity
    {
        public DateTime Birthday { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
    }
}

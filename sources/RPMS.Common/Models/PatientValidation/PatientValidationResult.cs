using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RPMS.Common.Models.PatientValidation
{
    [KnownType(typeof(Patient))]
    [DebuggerDisplay("Result:{PatientRecord}")]
    [DataContract(Name = "PatientValidationResult", Namespace = "http://www.screendox.com")]
    public class PatientValidationResult
    {
        /// <summary>
        /// Patient Record found. Null if record not found.
        /// </summary>
        [DataMember]
        public Patient PatientRecord { get; set; }

        [DataMember]
        public List<string> CorrectionsLog { get; set; } = new List<string>();

        /// <summary>
        /// Returns true if matched EHR patient records was found
        /// </summary>
        public bool IsMatchFound() { return PatientRecord != null; }
    }
}

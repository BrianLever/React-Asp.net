using RPMS.Common.Comparers;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RPMS.Common.Models
{
    [DebuggerDisplay("ID:{ID}. {LastName}, {FirstName} {MiddleName}, DOB: {DateOfBirth}")]
    [KnownType(typeof(Patient))]
    [DataContract(Name = "PatientSearch", Namespace = "http://www.screendox.com")]
    public class PatientSearch : IEquatable<PatientSearch>
    {
        /// <summary>
        ///ID
        /// </summary>
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string MiddleName { get; set; }

        [DataMember]
        public DateTime DateOfBirth { get; set; }

        [DataMember]
        protected int matchRank = 0;

        public int MatchRank { get { return matchRank; } }
        /// <summary>
        /// Evaluate MatchRank property
        /// </summary>
        /// <param name="matchPattern">Matching patient record from FrontDesk database</param>
        public void SetMatchRank(PatientSearch matchPattern)
        {
            if (matchPattern != null)
            {
                this.matchRank = new PatientSearchComparer().GetMatchRank(this, matchPattern);
            }
        }

        public bool Equals(PatientSearch other)
        {
            return
                this.ID == other.ID &&
                this.LastName == other.LastName &&
                this.FirstName == other.FirstName &&
                this.MiddleName == other.MiddleName &&
                this.DateOfBirth == other.DateOfBirth; 
        }
    }

}

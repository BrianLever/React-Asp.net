using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RPMS.Common.Models
{
    [DataContract(Name = "ScreeningResultRecord", Namespace = "http://www.screendox.com")]
    public class ScreeningResultRecord
    {
        /// <summary>
        /// Patient ID
        /// </summary>
        [DataMember]
        public int PatientID { get; set; }

        /// <summary>
        /// Visit ID key
        /// </summary>
        [DataMember]
        public int VisitID { get; set; }

        [DataMember]
        public long ScreendoxRecordNo { get; set; }


        [DataMember]
        public DateTime ScreeningDate { get; set; }

        [DataMember]
        public List<ScreeningResultSectionRecord> Sections = new List<ScreeningResultSectionRecord>();
    }
}

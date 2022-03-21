using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ScreenDox.Server.Models
{
    [DataContract(Name = "ScreendoxScreeningResultRecord", Namespace = "http://www.screendox.com")]
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
        public List<ScreeningResultSectionResponse> Sections = new List<ScreeningResultSectionResponse>();
    }
}

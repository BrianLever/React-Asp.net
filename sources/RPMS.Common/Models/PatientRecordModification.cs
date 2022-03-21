using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace RPMS.Common.Models
{
    [DataContract(Name = "PatientRecordModification", Namespace = "http://www.screendox.com")]
    public class PatientRecordModification
    {
        [DataMember]
        public PatientRecordExportFields Field { get; set; }

        [DataMember]
        public string CurrentValue { get; set; }

        [DataMember]
        public string UpdateWithValue { get; set; }

    }
}

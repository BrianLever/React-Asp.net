using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace RPMS.Common.Models
{
    [DataContract(Name = "ExportPatientTask", Namespace = "http://www.screendox.com")]
    public class ExportPatientTask
    {
        [DataMember]
        public PatientRecordExportFields Field { get; set; }
        [DataMember]
        public string Current { get; set; }
        [DataMember]
        public string After { get; set; }

    }
}

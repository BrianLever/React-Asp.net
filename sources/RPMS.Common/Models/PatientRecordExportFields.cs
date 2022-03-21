using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace RPMS.Common.Models
{

    [DataContract(Name = "PatientRecordExportFields", Namespace = "http://www.screendox.com")]
    public enum PatientRecordExportFields
    {
        [EnumMember]
        AddressLine1,
        [EnumMember]
        AddressLine2,
        [EnumMember]
        AddressLine3,
        [EnumMember]
        Phone,
        [EnumMember]
        City,
        [EnumMember]
        StateID,
        [EnumMember]
        ZipCode
    }
}

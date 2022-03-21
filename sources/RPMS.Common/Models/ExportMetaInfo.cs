using System.Runtime.Serialization;

namespace RPMS.Common.Models
{
    [DataContract(Name = "ExportMetaInfo", Namespace = "http://www.screendox.com")]
    public class ExportMetaInfo
    {
        [DataMember]
        public bool IsRpmsMode { get; set; }

        [DataMember]
        public bool IsNextGenMode { get; set; }

    }
}

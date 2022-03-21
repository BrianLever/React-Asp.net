using System.Runtime.Serialization;

namespace RPMS.Common.Models
{
    [DataContract(Name = "ExportScreeningSectionPreview", Namespace = "http://www.screendox.com")]
    public class ExportScreeningSectionPreview
    {
        [DataMember]
        public string ScreeningSectionID { get; set; }

        [DataMember]
        public string ScoreLevelLabel { get; set; }

    }
}

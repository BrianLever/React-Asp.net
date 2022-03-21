using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RPMS.Common.Models
{
    [DataContract(Name = "ScreeningResultSectionRecord", Namespace = "http://www.screendox.com")]
    public class ScreeningResultSectionRecord
    {
        [DataMember]
        public string ScreeningSectionID { get; set; }

        [DataMember]
        public int Score { get; set; }

        [DataMember]
        public int ScoreLevel { get; set; }

        [DataMember]
        public string ScoreLevelLabel { get; set; }

        [DataMember]
        public string Indicates { get; set; }

       

        [DataMember]
        public List<ScreeningResultSectionAnswerRecord> Answers { get; set; } = new List<ScreeningResultSectionAnswerRecord>();
    }
}

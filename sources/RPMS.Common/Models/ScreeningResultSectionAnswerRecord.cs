using System.Runtime.Serialization;

namespace RPMS.Common.Models
{
    [DataContract(Name = "ScreeningResultSectionAnswerRecord", Namespace = "http://www.screendox.com")]
    public class ScreeningResultSectionAnswerRecord
    {
        [DataMember]
        public int QuestionID { get; set; }

        [DataMember]
        public int AnswerValue { get; set; }

        [DataMember]
        public string QuestionText { get; set; }

        [DataMember]
        public string AnswerText { get; set; }
    }
}

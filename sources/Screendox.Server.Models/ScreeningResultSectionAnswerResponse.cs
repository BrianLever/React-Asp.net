using System.Runtime.Serialization;

namespace ScreenDox.Server.Models
{
    [DataContract(Name = "ScreeningResultSectionAnswerResponse", Namespace = "http://www.screendox.com")]
    public class ScreeningResultSectionAnswerResponse
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

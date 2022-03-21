using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ScreenDox.Server.Models
{
    [DataContract(Name = "ScreeningResultSectionResponse", Namespace = "http://www.screendox.com")]
    public class ScreeningResultSectionResponse
    {
        [DataMember]
        public string ScreeningSectionID { get; set; }
        
        /// <summary>
        /// Long name of the screening Section used to render section headers
        /// </summary>
        [DataMember]
        public string ScreeningSectionName { get; set; }

        /// <summary>
        /// Short name of the screening Section used to render a score label in the report.
        /// </summary>
        [DataMember]
        public string ScreeningSectionShortName { get; set; }

        [DataMember]
        public int Score { get; set; }

        [DataMember]
        public int ScoreLevel { get; set; }

        [DataMember]
        public string ScoreLevelLabel { get; set; }

        [DataMember]
        public string Indicates { get; set; }

        /// <summary>
        /// Copyright string
        /// </summary>
        [DataMember]
        public string Copyright { get; set; }


        [DataMember]
        public List<ScreeningResultSectionAnswerResponse> Answers { get; set; } = new List<ScreeningResultSectionAnswerResponse>();
    }
}

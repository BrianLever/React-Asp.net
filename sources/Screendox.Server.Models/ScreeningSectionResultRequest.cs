using System.Collections.Generic;

namespace ScreenDox.Server.Models
{

    /// <summary>
    /// Contract to receive entry values from the UI
    /// </summary>
    public class ScreeningSectionResultRequest
    {
        public string ScreeningSectionID { get; set; }

        public List<ScreeningSectionQuestionResultRequest> Answers { get; set; } = new List<ScreeningSectionQuestionResultRequest>();
    }
}

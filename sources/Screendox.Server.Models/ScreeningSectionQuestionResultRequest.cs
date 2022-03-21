namespace ScreenDox.Server.Models
{

    /// <summary>
    /// Contract to receive entry values from the UI
    /// </summary>
    public class ScreeningSectionQuestionResultRequest
    {
        public int QuestionID { get; set; }

        public int AnswerValue { get; set; }

    }
}

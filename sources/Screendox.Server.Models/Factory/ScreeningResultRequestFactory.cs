using FrontDesk;

using System.Collections.Generic;

namespace ScreenDox.Server.Models.Factory
{
    /// <summary>
    /// Factory method for converting request lightweight models into Screening Result Domain Models
    /// </summary>
    public static class ScreeningResultRequestFactory
    {
        /// <summary>
        /// Create Screening Section Result 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="screeningSectionId"></param>
        /// <returns></returns>
       
        public static ScreeningSectionResult Create(ScreeningSectionResultRequest request, long screeningSectionId)
        {
            return Create(new ScreeningSectionResult(), request, screeningSectionId);
        }

        /// <summary>
        /// Update Screening Section Result object with data in request payload
        /// </summary>
        /// <param name="result"></param>
        /// <param name="request"></param>
        /// <param name="screeningSectionId"></param>
        /// <returns>Updated model</returns>

        public static ScreeningSectionResult Create(ScreeningSectionResult result, ScreeningSectionResultRequest request, long screeningSectionId)
        {
            if (request == null)
            {
                return result;
            }


            result.ScreeningResultID = screeningSectionId;
            result.ScreeningSectionID = request.ScreeningSectionID;

            result.Answers.Clear();

            request.Answers = request.Answers ?? new List<ScreeningSectionQuestionResultRequest>();

            foreach (var answerRequest in request.Answers)
            {

                var answerRecord = new ScreeningSectionQuestionResult(
                    request.ScreeningSectionID, answerRequest.QuestionID, answerRequest.AnswerValue);


                result.AppendQuestionAnswer(answerRecord);
            }

            return result;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CuttingEdge.Conditions;
using FrontDesk;

namespace RPMS.Common.Models.Factory
{
    /// <summary>
    /// Factory method for initializing ScreeningResultRecord
    /// </summary>
    public static class ScreeningResultRecordFactory
    {
        public static ScreeningResultRecord Create(int ehrPatientId, int ehrVisitId, ScreeningResult screeningResult, Screening screeningInfo)
        {
            if (screeningResult == null)
            {
                throw new ArgumentNullException(nameof(screeningResult));
            }

            if (screeningInfo == null)
            {
                throw new ArgumentNullException(nameof(screeningInfo));
            }

            var result = new ScreeningResultRecord
            {
                PatientID = ehrPatientId,
                VisitID = ehrVisitId,
                ScreendoxRecordNo = screeningResult.ID,
                ScreeningDate = screeningResult.CreatedDate.Date
            };

            foreach (var section in screeningResult.SectionAnswers)
            {
                if(section.ScreeningSectionID == ScreeningSectionDescriptor.DrugOfChoice)
                {
                    continue;
                }

                var sectionRecord = new ScreeningResultSectionRecord
                {
                    ScreeningSectionID = section.ScreeningSectionID,
                    Score = section.Score ?? 0,
                    ScoreLevel = section.ScoreLevel ?? 0,
                    ScoreLevelLabel = section.ScoreLevelLabel,
                    Indicates = section.Indicates
                };

                var sectionInfo = screeningInfo.FindSectionByID(sectionRecord.ScreeningSectionID);

                Condition.Ensures(sectionInfo).IsNotNull($"Section with ID {sectionRecord.ScreeningSectionID} was not found.");

                foreach (var question in section.Answers)
                {
                    var answerRecord = new ScreeningResultSectionAnswerRecord
                    {
                        QuestionID = question.QuestionID,
                        AnswerValue = question.AnswerValue
                    };

                    var questionInfo = sectionInfo.FindQuestionByID(answerRecord.QuestionID);
                    Condition.Ensures(questionInfo).IsNotNull($"Question with ID {answerRecord.QuestionID} was not found.");


                    answerRecord.QuestionText = questionInfo.QuestionText;
                    answerRecord.AnswerText = questionInfo.AnswerOptions
                        .Where(x => x.AnswerScaleID == questionInfo.AnswerScaleID 
                            && x.Value == question.AnswerValue).FirstOrDefault()?.Text;
                   

                    sectionRecord.Answers.Add(answerRecord);
                }


                result.Sections.Add(sectionRecord);
            }


            return result;
        }
    }
}
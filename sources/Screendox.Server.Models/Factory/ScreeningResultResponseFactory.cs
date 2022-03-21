using FrontDesk;
using FrontDesk.Common.Screening;

using ScreenDox.Server.Models.Resources;

using System;
using System.Linq;

namespace ScreenDox.Server.Models.Factory
{
    /// <summary>
    /// Factory method for initializing ScreeningResultResponseFactory
    /// </summary>
    public static class ScreeningResultResponseFactory
    {
        /// <summary>
        /// Create screening result resonse from screening result.
        /// </summary>
        /// <param name="screeningResult"></param>
        /// <param name="screeningInfo">When not null, used to get question text and answer text</param>
        /// <returns></returns>
        public static ScreeningResultResponse Create(ScreeningResult screeningResult, Screening screeningInfo)
        {
            if (screeningResult == null)
            {
                throw new ArgumentNullException(nameof(screeningResult));
            }

            var result = new ScreeningResultResponse
            {
                ID = screeningResult.ID,

                CreatedDate = screeningResult.CreatedDate,
                WithErrors = screeningResult.WithErrors,
                                
                ExportDate = screeningResult.ExportDate,
                ExportedToHRN = screeningResult.ExportedToHRN,
                ExportedToPatientID = screeningResult.ExportedToPatientID,
                ExportedToVisitID = screeningResult.ExportedToVisitID,
                ExportedToVisitLocation = screeningResult.ExportedToVisitLocation,

                IsEligible4Export = screeningResult.IsEligible4Export,
                IsPassedAnySection = screeningResult.IsPassedAnySection,

                KioskID = screeningResult.KioskID,
                LocationID = screeningResult.LocationID,
                LocationLabel = screeningResult.LocationLabel
            };

            if(result.WithErrors)
            {
                result.WithErrorsMessage = TextMessages.CheckInHasValidationErrors;
            }

            result.PatientInfo = CreatePatientInfo(screeningResult);

            foreach (var section in screeningResult.SectionAnswers)
            {
                var sectionRecord = new ScreeningResultSectionResponse
                {
                    ScreeningSectionID = section.ScreeningSectionID,
                    ScreeningSectionName = string.Empty,
                    ScreeningSectionShortName = string.Empty,
                    Score = section.Score ?? 0,
                    ScoreLevel = section.ScoreLevel ?? 0,
                    ScoreLevelLabel = section.ScoreLevelLabel,
                    Indicates = section.Indicates
                };

                if (screeningInfo != null) // when screening info provided
                {
                    var sectionInfo = screeningInfo?.FindSectionByID(sectionRecord.ScreeningSectionID);

                    // set section screening name for UI
                    SetScreeningSectionNames(sectionRecord, section, sectionInfo);

                    foreach (var question in section.Answers)
                    {
                        var answerRecord = new ScreeningResultSectionAnswerResponse
                        {
                            QuestionID = question.QuestionID,
                            AnswerValue = question.AnswerValue
                        };

                        if (sectionInfo != null)
                        {
                            var questionInfo = sectionInfo.FindQuestionByID(answerRecord.QuestionID);


                            answerRecord.QuestionText = questionInfo?.QuestionText;
                            answerRecord.AnswerText = questionInfo?.AnswerOptions
                                .Where(x => x.AnswerScaleID == questionInfo?.AnswerScaleID
                                    && x.Value == question.AnswerValue).FirstOrDefault()?.Text;

                        }
                        sectionRecord.Answers.Add(answerRecord);
                    }
                }

                result.Sections.Add(sectionRecord);
            }

            return result;
        }
        /// <summary>
        /// Set screening section name
        /// </summary>
        /// <param name="sectionResponse"></param>
        /// <param name="screeningSectionResult"></param>
        /// <param name="sectionInfo"></param>
        private static void SetScreeningSectionNames(ScreeningResultSectionResponse sectionResponse,
                                                     ScreeningSectionResult screeningSectionResult,
                                                     ScreeningSection sectionInfo)
        {
            //update screening section name
            sectionResponse.ScreeningSectionName = sectionInfo.ScreeningSectionName;
            sectionResponse.ScreeningSectionShortName = sectionInfo.ScreeningSectionShortName;

            // custom rules for sections with alternative logic (DAST, GAD).
            if (sectionResponse.ScreeningSectionID == ScreeningSectionDescriptor.Depression)
            {
                var depressionSectionResult = screeningSectionResult.AsDepressionSection();

                sectionResponse.ScreeningSectionName = depressionSectionResult.ScreeningSectionName;
                sectionResponse.ScreeningSectionShortName = depressionSectionResult.ScreeningSectionShortName;
            }

            if (sectionResponse.ScreeningSectionID == ScreeningSectionDescriptor.Anxiety)
            {
                var anxietySectionResult = screeningSectionResult.AsAnxietySection();

                sectionResponse.ScreeningSectionName = anxietySectionResult.ScreeningSectionName;
                sectionResponse.ScreeningSectionShortName = anxietySectionResult.ScreeningSectionShortName;
            }

        }

        /// <summary>
        /// Create screening result response w/o question and answer texts
        /// </summary>
        /// <param name="screeningResult"></param>
        /// <returns></returns>
        public static ScreeningResultResponse CreateSlim(ScreeningResult screeningResult)
        {
            return Create(screeningResult, null);
        }
        public static PatientInfo CreatePatientInfo(ScreeningResult screeningResut)
        {
            return new PatientInfo
            {
                FirstName = screeningResut.FirstName,
                LastName = screeningResut.LastName,
                MiddleName = screeningResut.MiddleName,
                Birthday = screeningResut.Birthday,
                FullName = screeningResut.FullName,

                StreetAddress = screeningResut.StreetAddress,
                City = screeningResut.City,
                Phone = screeningResut.Phone,
                StateID = screeningResut.StateID,
                StateName = screeningResut.StateName,
                ZipCode = screeningResut.ZipCode,

                Age = screeningResut.Age
            };
        }


       
    }
}

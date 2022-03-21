using FrontDesk.Common.Bhservice;
using FrontDesk.Common.Bhservice.Export;
using FrontDesk.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FrontDesk.Services
{
    public interface IBhsVisitFactory
    {
        BhsVisit Create(ScreeningResult screeningResult, Screening screeningInfo);

        BhsVisit InitFromReader(IDataReader reader);

    }


    public class BhsVisitFactory : IBhsVisitFactory
    {
        public BhsVisit Create(ScreeningResult screeningResult, Screening screeningInfo)
        {
            if (!screeningResult.LocationID.HasValue)
            {
                throw new ArgumentNullException("screeningResult.LocationID");
            }

            var result = new BhsVisit
            {
                LocationID = screeningResult.LocationID.Value,
                ScreeningResultID = screeningResult.ID,
                CreatedDate = DateTimeOffset.Now,
                Result = screeningResult,
                ScreeningDate = screeningResult.CreatedDate,
            };

            //calculate flags
            result.TobacoExposureSmokerInHomeFlag =
                CheckRuleSectionPositive(ScreeningSectionDescriptor.SmokerInHome, screeningResult);

            //tobacco

            result.TobacoExposureCeremonyUseFlag =
                CheckRuleAnswerPositive(
                    ScreeningSectionDescriptor.Tobacco,
                    TobaccoQuestionsDescriptor.UseForCeremonyOnlyQuestionID,
                    screeningResult);
            result.TobacoExposureSmokingFlag =
                CheckRuleAnswerPositive(
                    ScreeningSectionDescriptor.Tobacco,
                    TobaccoQuestionsDescriptor.DoYouSmokeQuestionID,
                    screeningResult);
            result.TobacoExposureSmoklessFlag =
                CheckRuleAnswerPositive(
                    ScreeningSectionDescriptor.Tobacco,
                    TobaccoQuestionsDescriptor.DoYouSmokeSmokelessQuestionID,
                    screeningResult);

            //calculate indicators
            result.AlcoholUseFlag =
                GetSectionResult(ScreeningSectionDescriptor.Alcohol, screeningResult);

            result.PartnerViolenceFlag =
                GetSectionResult(ScreeningSectionDescriptor.PartnerViolence, screeningResult);

            result.SubstanceAbuseFlag =
                GetSectionResult(ScreeningSectionDescriptor.SubstanceAbuse, screeningResult);

            result.DepressionFlag =
                GetSectionResult(ScreeningSectionDescriptor.Depression, screeningResult);

            result.DepressionThinkOfDeathAnswer =
                GetAnswerResult(
                    ScreeningSectionDescriptor.Depression,
                    ScreeningSectionDescriptor.DepressionThinkOfDeathQuestionID,
                    screeningResult,
                    screeningInfo);

            result.AnxietyFlag =
               GetSectionResult(ScreeningSectionDescriptor.Anxiety, screeningResult);

            result.ProblemGamblingFlag =
              GetSectionResult(ScreeningSectionDescriptor.ProblemGambling, screeningResult);


            return result;
        }


        private bool CheckRuleAnswerPositive(string sectionId, int questionId, ScreeningResult result)
        {
            var questionValue = GetQuestionAnswer(sectionId, questionId, result) ?? 0;

            return (questionValue > 0);
        }

        private int? GetQuestionAnswer(string sectionId, int questionId, ScreeningResult result)
        {
            var target = result.FindSectionByID(sectionId);
            if (target == null) return null;

            return target.FindQuestionByID(questionId)?.AnswerValue;

        }

        private bool CheckRuleSectionPositive(string sectionId, ScreeningResult result)
        {
            var target = result.FindSectionByID(sectionId);
            return (target?.Score ?? 0) > 0;
        }

        private ScreeningResultValue GetSectionResult(string sectionId, ScreeningResult result)
        {
            var target = result.FindSectionByID(sectionId);

            if (target == null) return null;

            return new ScreeningResultValue
            {
                ScoreLevel = target.Score ?? 0,
                ScoreLevelLabel = target.ScoreLevelLabel ?? string.Empty
            };
        }

        private string GetAnswerResult(string sectionId, int questionId, ScreeningResult result, Screening screeningInfo)
        {
            var target = result.FindSectionByID(sectionId);
            if (target == null) return string.Empty;

            var question = target.FindQuestionByID(questionId);
            if (question == null) return string.Empty;

            var questionInfo = screeningInfo.FindSectionByID(sectionId)?.FindQuestionByID(questionId);

            return questionInfo.AnswerOptions.First(x => x.Value == question.AnswerValue).Text;
        }

        public BhsVisit InitFromReader(IDataReader reader)
        {
            var model = new BhsVisit();
            return InitFromReader(reader, model);
        }


        protected BhsVisit InitFromReader(IDataReader reader, BhsVisit model)
        {
            model.ID = reader.Get<long>("ID");
            model.ScreeningResultID = reader.Get<long>("ScreeningResultID");
            model.ScreeningDate = reader.Get<DateTimeOffset>("ScreeningDate");
            model.CreatedDate = reader.Get<DateTimeOffset>("CreatedDate");
            model.LocationID = reader.Get<int>("LocationID");
            model.BhsStaffNameCompleted = reader.Get<string>("BhsStaffNameCompleted");
            model.CompleteDate = reader.GetNullable<DateTimeOffset>("CompleteDate");

            model.TobacoExposureSmokerInHomeFlag = reader.Get<Boolean>("TobacoExposureSmokerInHomeFlag");
            model.TobacoExposureCeremonyUseFlag = reader.Get<Boolean>("TobacoExposureCeremonyUseFlag");
            model.TobacoExposureSmokingFlag = reader.Get<Boolean>("TobacoExposureSmokingFlag");
            model.TobacoExposureSmoklessFlag = reader.Get<Boolean>("TobacoExposureSmoklessFlag");
            model.Notes = reader.Get<string>("Notes");
            model.NewVisitDate = reader.GetNullable<DateTimeOffset>("NewVisitDate");
            model.FollowUpDate = reader.GetNullable<DateTimeOffset>("FollowUpDate");



            if (reader.GetNullable<int>("AlcoholUseFlagScoreLevel").HasValue)
            {
                model.AlcoholUseFlag = new ScreeningResultValue
                {
                    ScoreLevel = reader.Get<int>("AlcoholUseFlagScoreLevel"),
                    ScoreLevelLabel = reader.Get<string>("AlcoholUseFlagScoreLevelLabel")
                };
            }

            if (reader.GetNullable<int>("SubstanceAbuseFlagScoreLevel").HasValue)
            {
                model.SubstanceAbuseFlag = new ScreeningResultValue
                {
                    ScoreLevel = reader.Get<int>("SubstanceAbuseFlagScoreLevel"),
                    ScoreLevelLabel = reader.Get<string>("SubstanceAbuseFlagScoreLevelLabel")
                };
            }

            if (reader.GetNullable<int>("AnxietyFlagScoreLevel").HasValue)
            {
                model.AnxietyFlag = new ScreeningResultValue
                {
                    ScoreLevel = reader.Get<int>("AnxietyFlagScoreLevel"),
                    ScoreLevelLabel = reader.Get<string>("AnxietyFlagScoreLevelLabel"),
                };
            }

            if (reader.GetNullable<int>("DepressionFlagScoreLevel").HasValue)
            {
                model.DepressionFlag = new ScreeningResultValue
                {
                    ScoreLevel = reader.Get<int>("DepressionFlagScoreLevel"),
                    ScoreLevelLabel = reader.Get<string>("DepressionFlagScoreLevelLabel"),
                };
                model.DepressionThinkOfDeathAnswer = reader.Get<string>("DepressionThinkOfDeathAnswer");
            }

            if (reader.GetNullable<int>("PartnerViolenceFlagScoreLevel").HasValue)
            {
                model.PartnerViolenceFlag = new ScreeningResultValue
                {
                    ScoreLevel = reader.Get<int>("PartnerViolenceFlagScoreLevel"),
                    ScoreLevelLabel = reader.Get<string>("PartnerViolenceFlagScoreLevelLabel")
                };
            }

            if (reader.GetNullable<int>("ProblemGamblingFlagScoreLevel").HasValue)
            {
                model.ProblemGamblingFlag = new ScreeningResultValue
                {
                    ScoreLevel = reader.Get<int>("ProblemGamblingFlagScoreLevel"),
                    ScoreLevelLabel = reader.Get<string>("ProblemGamblingFlagScoreLevelLabel"),
                };
            }

            model.NewVisitReferralRecommendation = new Common.LookupValueWithDescription
            {
                Id = reader.Get<int>("NewVisitReferralRecommendationID", 0),
                Name = reader.Get<string>("NewVisitReferralRecommendationName"),
                Description = reader.Get<string>("NewVisitReferralRecommendationDescription")
            };

            model.NewVisitReferralRecommendationAccepted = new Common.LookupValue
            {
                Id = reader.Get<int>("NewVisitReferralRecommendationAcceptedID", 0),
                Name = reader.Get<string>("NewVisitReferralRecommendationAcceptedName"),
            };

            model.ReasonNewVisitReferralRecommendationNotAccepted = new Common.LookupValue
            {
                Id = reader.Get<int>("ReasonNewVisitReferralRecommendationNotAcceptedID", 0),
                Name = reader.Get<string>("ReasonNewVisitReferralRecommendationNotAcceptedName"),
            };

            model.Discharged = new Common.LookupValue
            {
                Id = reader.Get<int>("DischargedID", 0),
                Name = reader.Get<string>("DischargedName"),
            };

            var otherToolsString = reader.Get<string>("OtherScreeningTools");
            if (!string.IsNullOrWhiteSpace(otherToolsString))
            {
                model.OtherScreeningTools.AddRange(otherToolsString.FromXmlString<List<ManualScreeningResultValue>>());
            }

            //read treatment actions
            {
                for (int index = 1; index < 6; index++)
                {

                    int? treatmentActionId = reader.GetNullable<int>(string.Concat("TreatmentAction", index, "ID"));
                    if (treatmentActionId.HasValue)
                    {
                        model.TreatmentActions.Add(new TreatmentAction
                        {
                            Id = treatmentActionId.Value,
                            Name = reader.Get<string>(string.Concat("TreatmentAction", index, "Name")),
                            Description = reader.Get<string>(string.Concat("TreatmentAction", index, "Description"))
                        });
                    }

                }

            }

            return model;

        }

        public BhsVisitExtendedWithIdentity InitExtendedModelFromReader(IDataReader reader)
        {
            var model = new BhsVisitExtendedWithIdentity();
            InitFromReader(reader, model);

            model.FirstName = reader.Get<string>("LastName");
            model.LastName = reader.Get<string>("LastName");
            model.MiddleName = reader.Get<string>("MiddleName");
            model.Birthday = reader.Get<DateTime>("Birthday");
            model.ExportedToHRN = reader.Get<string>("ExportedToHRN");
            model.DemographicsID = reader.Get<long>("DemographicsID");
            model.LocationName = reader.Get<string>("BranchLocationName");

            return model;

        }
    }
}

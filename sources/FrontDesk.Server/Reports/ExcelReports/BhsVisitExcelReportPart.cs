using FrontDesk.Common.Bhservice.Export;
using FrontDesk.Common.Extensions;
using FrontDesk.Server.Extensions;

namespace FrontDesk.Server.Reports.ExcelReports
{
    public class BhsVisitExcelReportPart : ExcelReportPartModelBase<BhsVisitExtendedWithIdentity>
    {
      

       
        public BhsVisitExcelReportPart(BhsReportExcelReport parentReport): base(parentReport, "visits")
        {
            Row
                .Add("LastName", x => x.LastName)
                .Add("FirstName", x => x.FirstName)
                .Add("MiddleName", x => x.MiddleName)
                .Add("Birthday", x => x.Birthday.FormatAsDate(), HorizontalCellAllignment.Left, 20, DataType.Text)
                .Add("HRN", x => x.ExportedToHRN)
                .Add("DemographicsId", x => x.ID, DataType.Number)
                .Add("CreatedDate", x => x.CreatedDate.FormatAsDateWithTimeWithoutTimeZone(), DataType.Text)
                .Add("ScreeningDate", x => x.ScreeningDate.FormatAsDateWithTimeWithoutTimeZone(), DataType.Text)

                .Add("BranchLocationID", x => x.LocationID, DataType.Number)
                .Add("BranchLocationName", x => x.LocationName)


                .Add("NewVisitReferralRecommendationId", x => x.NewVisitReferralRecommendation.Id, DataType.Number)
                .Add("NewVisitReferralRecommendationName", x => x.NewVisitReferralRecommendation.Name)
                .Add("NewVisitReferralRecommendationDescription", x => x.NewVisitReferralRecommendation.Description)
                .Add("NewVisitReferralRecommendationAcceptedId", x => x.NewVisitReferralRecommendationAccepted.Id, DataType.Number)
                .Add("NewVisitReferralRecommendationAcceptedName", x => x.NewVisitReferralRecommendationAccepted.Name)
                .Add("ReasonNewVisitReferralRecommendationNotAcceptedId", x => x.ReasonNewVisitReferralRecommendationNotAccepted.Id, DataType.Number)
                .Add("ReasonNewVisitReferralRecommendationNotAcceptedName", x => x.ReasonNewVisitReferralRecommendationNotAccepted.Name)
                .Add("DischargedId", x => x.Discharged.Id, DataType.Number)
                .Add("DischargedName", x => x.Discharged.Name)

                .Add("NewVisitDate", x => x.NewVisitDate.FormatAsDate())
                .Add("ThirtyDatyFollowUpFlag", x => x.ThirtyDatyFollowUpFlag)
                .Add("FollowUpDate", x => x.FollowUpDate.FormatAsDate())
                .Add("CompleteDate", x => x.CompleteDate.FormatAsDateWithTimeWithoutTimeZone(), DataType.Text)
                .Add("BhsStaffNameCompleted", x => x.BhsStaffNameCompleted)
                ;



            for (int i = 0; i < 5; i++)
            {
                {
                    var index = i;
                    Row
                    .Add($"TreatmentAction{index + 1}Id", x => x.TreatmentActions.Count > index ? x.TreatmentActions[index].Id : 0, DataType.Number)
                    .Add($"TreatmentAction{index + 1}Name", x => x.TreatmentActions.Count > index ? x.TreatmentActions[index].Name : string.Empty)
                    .Add($"TreatmentAction{index + 1}Description", x => x.TreatmentActions.Count > index ? x.TreatmentActions[index].Description : string.Empty)
                    ;
                }
            }

            for (int i = 0; i < 4; i++)
            {
                {
                    var index = i;
                    Row
                    .Add($"OtherScreeningTool{index + 1}ScoreOrResult", x => x.OtherScreeningTools.Count > index ? x.OtherScreeningTools[index].ScoreOrResult : string.Empty)
                    .Add($"OtherScreeningTool{index + 1}ToolName", x => x.OtherScreeningTools.Count > index ? x.OtherScreeningTools[index].ToolName : string.Empty)
                    ;
                }
            }

            Row
                .Add("TobacoExposureSmokerInHomeFlag", x => x.TobacoExposureSmokerInHomeFlag ? 1 : 0, DataType.Number)
                .Add("TobacoExposureCeremonyUseFlag", x => x.TobacoExposureCeremonyUseFlag ? 1 : 0, DataType.Number)
                .Add("TobacoExposureSmokingFlag", x => x.TobacoExposureSmokingFlag ? 1 : 0, DataType.Number)
                .Add("TobacoExposureSmoklessFlag", x => x.TobacoExposureSmoklessFlag ? 1 : 0, DataType.Number)

                .Add("AlcoholUseFlagScoreLevel", x => x.AlcoholUseFlag?.ScoreLevel, DataType.Number)
                .Add("AlcoholUseFlagScoreLevelLabel", x => x.AlcoholUseFlag?.ScoreLevelLabel)

                .Add("SubstanceAbuseFlagLevel", x => x.SubstanceAbuseFlag?.ScoreLevel, DataType.Number)
                .Add("SubstanceAbuseFlagLevelLabel", x => x.SubstanceAbuseFlag?.ScoreLevelLabel)
                .Add("DepressionFlagLevel", x => x.DepressionFlag?.ScoreLevel, DataType.Number)
                .Add("DepressionFlagLevelLabel", x => x.DepressionFlag?.ScoreLevelLabel)
                .Add("DepressionThinkOfDeathAnswer", x => x.DepressionThinkOfDeathAnswer)
                .Add("PartnerViolenceFlagLevel", x => x.PartnerViolenceFlag?.ScoreLevel, DataType.Number)
                .Add("PartnerViolenceFlagLevelLabel", x => x.PartnerViolenceFlag?.ScoreLevelLabel)
                ;
        }
    }
}

using FrontDesk.Common.Bhservice.Export;
using FrontDesk.Common.Extensions;
using FrontDesk.Server.Extensions;

namespace FrontDesk.Server.Reports.ExcelReports
{
    public class BhsFollowUpExcelReportPart : ExcelReportPartModelBase<BhsFollowUpExtendedWithIdentity>
    {
      

       
        public BhsFollowUpExcelReportPart(BhsReportExcelReport parentReport): base(parentReport, "followups")
        {
            Row
                .Add("LastName", x => x.LastName)
                .Add("FirstName", x => x.FirstName)
                .Add("MiddleName", x => x.MiddleName)
                .Add("Birthday", x => x.Birthday.FormatAsDate(), HorizontalCellAllignment.Left, 20, DataType.Text)
                .Add("HRN", x => x.ExportedToHRN)
                .Add("DemographicsId", x => x.ID, DataType.Number)
                .Add("CreatedDate", x => x.CreatedDate.FormatAsDateWithTimeWithoutTimeZone(), DataType.Text)
                .Add("ScreeningDate", x => x.Visit.ScreeningDate.FormatAsDateWithTimeWithoutTimeZone(), DataType.Text)

                .Add("BranchLocationID", x => x.Visit.LocationID, DataType.Number)
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

            Row
                .Add("PatientAttendedVisitId", x => x.PatientAttendedVisit?.Id, DataType.Number)
                .Add("PatientAttendedVisitName", x => x.PatientAttendedVisit?.Name)
                .Add("BhsVisitId", x => x.BhsVisitID, DataType.Number)
                .Add("ScheduledVisitDate", x => x.ScheduledVisitDate.FormatAsDate())
                .Add("FollowUpDate", x => x.FollowUpDate?.FormatAsDate())
                .Add("FollowUpContactDate", x => x.FollowUpContactDate?.FormatAsDate())
                .Add("FollowUpContactOutcomeId", x => x.FollowUpContactOutcome?.Id, DataType.Number)
                .Add("FollowUpContactOutcomeName", x => x.FollowUpContactOutcome?.Name)
                ;
        }
    }
}

using FrontDesk.Common;
using FrontDesk.Common.Bhservice;
using FrontDesk.Common.Extensions;
using FrontDesk.Server.Extensions;

namespace FrontDesk.Server.Reports.ExcelReports
{
    public class BhsDemographicsExcelReportPart : ExcelReportPartModelBase<BhsDemographics>
    {
        public BhsDemographicsExcelReportPart(BhsReportExcelReport parentReport) : base(parentReport, "demographics")
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
                .Add("BranchLocationName", x => x.LocationLabel)
                .Add("RaceId", x => x.Race.Id, DataType.Number)
                .Add("RaceName", x => x.Race.Name)
                .Add("GenderId", x => x.Gender.Id, DataType.Number)
                .Add("GenderName", x => x.Gender.Name)
                .Add("SexualOrientationId", x => x.SexualOrientation.Id, DataType.Number)
                .Add("SexualOrientationName", x => x.SexualOrientation.Name)
                .Add("TribalAffiliation", x => x.TribalAffiliation)
                .Add("MaritalStatusId", x => x.MaritalStatus.Id, DataType.Number)
                .Add("MaritalStatusName", x => x.MaritalStatus.Name)
                .Add("EducationLevelId", x => x.EducationLevel.Id, DataType.Number)
                .Add("EducationLevelName", x => x.EducationLevel.Name)
                .Add("LivingOnReservationId", x => x.LivingOnReservation.Id, DataType.Number)
                .Add("LivingOnReservationName", x => x.LivingOnReservation.Name)
                .Add("CountyOfResidence", x => x.CountyOfResidence)
                .Add("MilitaryExperienceId", x => x.MilitaryExperience.ToCsv(i => i.Id), DataType.Text)
                .Add("MilitaryExperienceName", x => x.MilitaryExperience.ToCsv(i => i.Name), DataType.Text)

                .Add("CompleteDate", x => x.CompleteDate.FormatAsDateWithTimeWithoutTimeZone(), DataType.Text)
                .Add("BhsStaffNameCompleted", x => x.BhsStaffNameCompleted)
                ;
        }
    }
}

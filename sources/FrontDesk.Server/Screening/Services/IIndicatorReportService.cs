using FrontDesk.Server.Screening.Models;

namespace FrontDesk.Server.Screening.Services
{
    public interface IIndicatorReportService
    {
        IndicatorReportByAgeViewModel GetBhsIndicatorReportByAge(FrontDesk.Screening screeningInfo, SimpleFilterModel filter, int[] ageGroups, bool uniquePatientsMode);
        IndicatorReportByAgeViewModel GetDrugsOfChoiceByAge(FrontDesk.Screening screeningInfo, SimpleFilterModel filter, int[] ageGroups, bool uniquePatientsMode);
        IndicatorReportViewModel GetBhsIndicatorReportByProblem(FrontDesk.Screening screeningInfo, SimpleFilterModel filter, bool uniquePatientsMode);

    }
}
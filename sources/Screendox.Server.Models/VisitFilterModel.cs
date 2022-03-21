using FrontDesk.Server.Printouts.Bhs;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Screening.Services;

namespace ScreenDox.Server.Models
{
    public class VisitFilterModel : ScreeningResultFilterModel
    {
        /// <summary>
        /// Report type
        /// </summary>
        public BhsReportType ReportType;

        public BhsSearchFilterModel ToSearchFilterModel()
        {
            return new BhsSearchFilterModel(
                FirstName,
                LastName,
                Location,
                StartDate,
                EndDate,
                ReportType,
                ScreeningResultID
            );
        }
    }
}

using FrontDesk.Server.Screening.Services;

namespace ScreenDox.Server.Models
{
    public class PagedVisitFilterModel : PagedScreeningResultFilterModel
    {
        /// <summary>
        /// Report type
        /// </summary>
        public BhsReportType ReportType;
    }
}

using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Screening.Services;

namespace ScreenDox.Server.Models
{
    public class PagedFilterModel : SimpleFilterModel, IPagedSearchFilter
    {

        /// <summary>
        /// Patient's firt name
        /// </summary>
        public string FirstName;
        /// <summary>
        /// Patient's last name
        /// </summary>
        public string LastName;

        /// <summary>
        /// Start row for paged result
        /// </summary>
        public int StartRowIndex { get; set; }
        /// <summary>
        /// Max number of items in result
        /// </summary>
        public int MaximumRows { get; set; } = 20;
        /// <summary>
        /// Order by expression
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// Report type
        /// </summary>
        public BhsReportType ReportType;
    }
}

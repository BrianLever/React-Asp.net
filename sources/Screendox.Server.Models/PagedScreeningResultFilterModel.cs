using FrontDesk.Server.Screening.Models;

namespace ScreenDox.Server.Models
{
    public class PagedScreeningResultFilterModel: ScreeningResultFilterModel, IPagedSearchFilter
    {
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
    }
}

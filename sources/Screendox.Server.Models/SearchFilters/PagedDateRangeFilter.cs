using System;

namespace ScreenDox.Server.Models.SearchFilters
{
    /// <summary>
    /// Filter by date range with pagination support
    /// </summary>
    public class PagedDateRangeNameFilter : IPagedSearchFilter
    {
        public DateTimeOffset? StartDate;
        public DateTimeOffset? EndDate;
        public string nameFilter { get; set; }
        public string OrderBy { get; set; }
        public int StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
       
    }
}

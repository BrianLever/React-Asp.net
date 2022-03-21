using FrontDesk.Server.Screening.Services;

using System;

namespace FrontDesk.Server.Screening.Models
{
    public class SearchRelatedItemsFilter
    {
        public long MainRowID;
        public int? LocationId;
        public DateTime? StartDate;
        public DateTime? EndDate;
        public BhsReportType ReportType;
    }
}
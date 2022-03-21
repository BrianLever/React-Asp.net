using FrontDesk.Server.Screening.Services;

using System;

namespace FrontDesk.Server.Screening.Models
{
    public class BhsSearchRelatedItemsFilter : SearchRelatedItemsFilter
    {
        public long? ScreeningResultID;

        public BhsSearchRelatedItemsFilter()
        {

        }
        public BhsSearchRelatedItemsFilter(
            long mainRowID,
            int? locationId,
            DateTime? startDate,
            DateTime? endDate,
            BhsReportType reportType,
            long? screeningResultID
            )
        {
            MainRowID = mainRowID;
            LocationId = locationId;
            StartDate = startDate;
            EndDate = endDate;
            ReportType = reportType;
            ScreeningResultID = screeningResultID;
        }
    }
}

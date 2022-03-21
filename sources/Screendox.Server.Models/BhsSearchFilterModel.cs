using FrontDesk.Server.Screening.Services;
using System;

namespace FrontDesk.Server.Printouts.Bhs
{
    public struct BhsSearchFilterModel
    {
        public string FirstName;
        public string LastName;
        public long? ScreeningResultID;
        public int? LocationId;
        public DateTime? StartDate;
        public DateTime? EndDate;
        public BhsReportType ReportType;

        public BhsSearchFilterModel(
            string firstName,
            string lastName,
            int? locationId,
            DateTime? startDate,
            DateTime? endDate,
            BhsReportType reportType,
            long? screeningResultID
        )
        {
            FirstName = firstName;
            LastName = lastName;
            LocationId = locationId;
            StartDate = startDate;
            EndDate = endDate;
            ReportType = reportType;
            ScreeningResultID = screeningResultID;
        }
    }
}

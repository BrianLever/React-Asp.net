using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Screening.Services
{
    public enum BhsReportType: int
    {
        AllReports = 0,
        CompletedReports = 1,
        IncompleteReports = 2
    }

    public static class BhsReportTypeExtensions
    {
        public static BhsReportType ConvertIntToBhsReportType(this int? filter)
        {
            if (!filter.HasValue) return BhsReportType.AllReports;

            if (Enum.IsDefined(typeof(BhsReportType), filter))
            {
                return (BhsReportType)filter;
            }

            return BhsReportType.AllReports;
        }
    }
}

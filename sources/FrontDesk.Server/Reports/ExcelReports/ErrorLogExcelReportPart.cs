using FrontDesk.Server.Extensions;

using ScreenDox.Server.Models;

namespace FrontDesk.Server.Reports.ExcelReports
{
    public class ErrorLogExcelReportPart : ExcelReportPartModelBase<ErrorLogItem>
    {
        public ErrorLogExcelReportPart(IExcelReport parentReport) : base(parentReport, "demographics")
        {

            Row
                .Add("ErrorLogID", x => x.ErrorLogID, DataType.Number)
                .Add("KioskID", x => x.KioskID, DataType.Number)
                .Add("ErrorMessage", x => x.ErrorMessage, HorizontalCellAllignment.Left, 70, DataType.Text)
                .Add("ErrorTraceLog", x => x.ErrorTraceLog, HorizontalCellAllignment.Left, 140, DataType.Text)
                .Add("CreatedDate", x => x.CreatedDate.FormatAsDateWithTimeWithoutTimeZone(), DataType.Text)
                .Add("KioskLabel", x => x.KioskLabel, DataType.Text);
        }
    }
}

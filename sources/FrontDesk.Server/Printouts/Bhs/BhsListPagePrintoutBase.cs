using FrontDesk.Server.Messages;
using FrontDesk.Server.Screening.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Printouts.Bhs
{
    public abstract class BhsListPagePrintoutBase<TModel> : BhsListPrintoutBase
    {
        public BhsSearchFilterModel Filter { get; protected set; }
        public TModel Model { get; protected set; }

        protected IBranchLocationService BranchLocationService = new BranchLocationService();

        protected override void PrintReportFilter(Document document)
        {
            float[] headerWidth = new float[] { 0.3f, 0.15f, 0.25f, 0.15f, 0.15f };
            var filterTable = new PdfPTable(headerWidth) { WidthPercentage = 100 };

            AddLabelCell(filterTable, "Report Period");
            AddLabelCell(filterTable, "Report Type");
            AddLabelCell(filterTable, "Branch Locations");
            AddLabelCell(filterTable, "First Name");
            AddLabelCell(filterTable, "Last Name");



            AddValueCell(filterTable, "{0:MM/dd/yyyy} to {1:MM/dd/yyyy}".FormatWith(
                Filter.StartDate != null ? Filter.StartDate : null,
                Filter.EndDate != null ? Filter.EndDate : DateTime.Now)
                );

            string reportTypeLabel = Filter.ReportType == BhsReportType.AllReports ? TextStrings.BHS_REPORTTYPE_ALL_REPORTS :
                Filter.ReportType == BhsReportType.CompletedReports ?
                    TextStrings.BHS_REPORTTYPE_COMPLETED : TextStrings.BHS_REPORTTYPE_INCOMPLETE;

            AddValueCell(filterTable, reportTypeLabel);
            AddValueCell(filterTable, Filter.LocationId != null ? BranchLocationService.Get(Filter.LocationId.Value).Name : "<< All >>");

            AddValueCell(filterTable, Filter.FirstName);
            AddValueCell(filterTable, Filter.LastName);


            document.Add(filterTable);
        }

    }
}

using FrontDesk.Common.Extensions;
using FrontDesk.Server.Extensions;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Screening.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;

namespace FrontDesk.Server.Printouts.Bhs
{
    public class BhsVisitListPdfPrintout : BhsListPagePrintoutBase<List<BhsVisitListItemPrintoutModel>>
    {
        protected override string[] Header => new string[] { "Visit List" };

        protected override TableColumnDefinition[] Columns
        {
            get
            {
                return new TableColumnDefinition[] {
                    new TableColumnDefinition{ Name = "Patient Name" },
                    new TableColumnDefinition{Name = "Date of Birth" },
                    new TableColumnDefinition{Name= "Screening Date", HorizontalAlignment = Element.ALIGN_RIGHT },
                    new TableColumnDefinition{Name= "File Completion Date", HorizontalAlignment = Element.ALIGN_RIGHT },
                };
            }
        }
        protected override float[] ColumnWidths
        {
            get
            {
                return new float[] { 0.4f, 0.2f, 0.2f, 0.2f };
            }
        }
        
        public BhsVisitListPdfPrintout(BhsVisitService visitService, BhsSearchFilterModel filter)
        {
            var service = visitService ?? throw new ArgumentNullException(nameof(visitService));
            Filter = filter;
            Model = service.GetAllForPrintout(Filter);
        }

        public BhsVisitListPdfPrintout(List<BhsVisitListItemPrintoutModel> model)
        {
            Model = model ?? new List<BhsVisitListItemPrintoutModel>();
        }

        public BhsVisitListPdfPrintout(BhsSearchFilterModel filter) : this(new BhsVisitService(), filter)
        {
        }

        protected override void PrintItems(PdfPTable parentTable)
        {
            bool normalRow = true;
            var incrementalLineNumber = 0;
            int rowCounter = 0;
            float additionalLeftPadding = 20f;

            foreach (var item in Model)
            {

                PrintBodyCell(parentTable, item.PatientName, normalRow, Columns[0].HorizontalAlignment);
                PrintBodyCell(parentTable, item.Birthday.FormatAsDate(), normalRow, Columns[1].HorizontalAlignment);
                PrintBodyCell(parentTable, item.LastScreeningDate.FormatAsDateWithTimeWithoutTimeZone(), normalRow, Columns[2].HorizontalAlignment);
                PrintBodyCell(parentTable, item.LastCompletionDate.FormatAsDateWithTimeWithoutTimeZone(), normalRow, Columns[3].HorizontalAlignment);

                foreach (var reportItem in item.ReportItems)
                {
                    PrintBodyCell(parentTable, "{0} ID: {1}".FormatWith(reportItem.ReportName, reportItem.ID), true, Columns[0].HorizontalAlignment, additionalLeftPadding);
                    PrintBodyCell(parentTable, string.Empty, true);
                    PrintBodyCell(parentTable, reportItem.ScreeningDateLabel, true, Columns[2].HorizontalAlignment);
                    PrintBodyCell(parentTable, reportItem.CompleteDateLabel, true, Columns[3].HorizontalAlignment);

                    incrementalLineNumber++;
                }


                normalRow = !normalRow;

                incrementalLineNumber++;
            }

            if (rowCounter >= 1000)
            {
                PrintThousandRowsLimitMessage(parentTable);
            }
           
        }

      
    }
}

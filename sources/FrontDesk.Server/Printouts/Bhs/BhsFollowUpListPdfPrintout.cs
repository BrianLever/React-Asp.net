using FrontDesk.Common.Extensions;
using FrontDesk.Server.Extensions;
using FrontDesk.Server.Messages;
using FrontDesk.Server.Screening;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Screening.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Printouts.Bhs
{
    public class BhsFollowUpListPdfPrintout : BhsListPagePrintoutBase<List<BhsFollowUpListItemPrintoutModel>>
    {
        private readonly BhsFollowUpService _service;
        
        protected override string[] Header => new string[] { "Follow-Up List" };
        protected override TableColumnDefinition[] Columns
        {
            get
            {
                return new TableColumnDefinition[] {
                    new TableColumnDefinition{ Name = "Patient Name" },
                    new TableColumnDefinition{Name = "Date of Birth" },
                    new TableColumnDefinition{Name= "Follow-Up Date", HorizontalAlignment = Element.ALIGN_RIGHT },
                    new TableColumnDefinition{Name= "Visit Date", HorizontalAlignment = Element.ALIGN_RIGHT },
                    new TableColumnDefinition{Name= "File Completion Date", HorizontalAlignment = Element.ALIGN_RIGHT },
                };
            }
        }

        protected override float[] ColumnWidths
        {
            get
            {
                return new float[] { 0.4f, 0.15f, 0.15f, 0.15f, 0.15f };
            }
        }

        public BhsFollowUpListPdfPrintout(BhsFollowUpService visitService, BhsSearchFilterModel filter)
        {
            _service = visitService ?? throw new ArgumentNullException(nameof(visitService));
            Filter = filter;
            Model = _service.GetAllForPrintout(Filter);
        }

        public BhsFollowUpListPdfPrintout(List<BhsFollowUpListItemPrintoutModel> model)
        {
            Model = model ?? new List<BhsFollowUpListItemPrintoutModel>();
        }


        public BhsFollowUpListPdfPrintout(BhsSearchFilterModel filter) : this(new BhsFollowUpService(), filter)
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
                PrintBodyCell(parentTable, item.LastFollowUpDate.FormatAsDate(), normalRow, Columns[2].HorizontalAlignment);
                PrintBodyCell(parentTable, item.LastVisitDate.FormatAsDate(), normalRow, Columns[3].HorizontalAlignment);
                PrintBodyCell(parentTable, item.LastCompletionDate.FormatAsDateWithTimeWithoutTimeZone(), normalRow, Columns[4].HorizontalAlignment);

                foreach (var reportItem in item.ReportItems)
                {
                    PrintBodyCell(parentTable, "{0} ID: {1}".FormatWith(reportItem.ReportName, reportItem.ID), true, Columns[0].HorizontalAlignment, additionalLeftPadding);
                    PrintBodyCell(parentTable, string.Empty, true);
                    PrintBodyCell(parentTable, reportItem.FollowUpDateLabel, true, Columns[2].HorizontalAlignment);
                    PrintBodyCell(parentTable, reportItem.ScheduledVisitDateLabel, true, Columns[3].HorizontalAlignment);
                    PrintBodyCell(parentTable, reportItem.CompleteDateLabel, true, Columns[4].HorizontalAlignment);

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

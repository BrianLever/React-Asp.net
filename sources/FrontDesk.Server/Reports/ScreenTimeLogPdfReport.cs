using System;
using System.Collections.Generic;
using System.Linq;

using FrontDesk.Common.Extensions;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Screening.Services;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace FrontDesk.Server.Reports
{
    public class ScreenTimeLogPdfReport : BhsIndicatorPdfReportBase<ScreeningTimeLogReportViewModel>
    {
        private readonly IScreeningTimeLogService _timeLogService = new ScreeningTimeLogService();


        #region constructor
        public ScreenTimeLogPdfReport(
           SimpleFilterModel filter)
             : base(filter, false)
        {
            if (filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate = filter.EndDate.Value.AddDays(1);
            }

            Model = _timeLogService.GetReport(filter);

        }

        public override int ValuesCoumnsCount { get { return 3; } }

        #endregion

        /// <summary>
        /// Create PDF report
        /// </summary>
        public override void PrintReport()
        {
            //print header section
            PrintHeader(document, new string[] { "Behavioral Health Indicator Report", "Screen Time Log" });
            //Print report settings
            PrintReportSettings(document);


            PrintScoreSection(document, Model);


        }



        /// <summary>
        /// print tool section
        /// </summary>
        public void PrintScoreSection(Document document, ScreeningTimeLogReportViewModel sectionModel)
        {

            var table = CreateTable();
            var headers = new string[] {
                Resources.Labels.ScreenTimeLog_ScreeningMeasure_Label,
                Resources.Labels.ScreenTimeLog_NumberOfReports_Label,
                Resources.Labels.ScreenTimeLog_TotalTime_Label,
                Resources.Labels.ScreenTimeLog_AverageTime_Label
            }; 
            //print header
            PrintSectionHeader(table, headers);

            PrintItems(table, sectionModel.SectionMeasures);
            PrintSectionFooter(table, sectionModel.EntireScreeningsMeasures);

            document.Add(table);


        }

        private void PrintSectionHeader(PdfPTable table, string[] headers)
        {
            int index = 0;
            foreach (var item in headers)
            {
                PrintHeaderCell(table, item, index++ == 0? Element.ALIGN_LEFT: Element.ALIGN_CENTER);
            }
        }

        private void PrintSectionFooter(PdfPTable table, ScreeningTimeLogReportItem item)
        {
            PrintHeaderCell(table, "Totals for All Screening Measures", Element.ALIGN_LEFT);

            PrintHeaderCell(table, item.NumberOfReports.ToString(), Element.ALIGN_CENTER);
            PrintHeaderCell(table, item.TotalTime.AsFormattedString(), Element.ALIGN_CENTER);
            PrintHeaderCell(table, item.AverageTime.AsFormattedAverageString(), Element.ALIGN_CENTER);
        }

        private void PrintItems(PdfPTable table, List<ScreeningTimeLogReportItem> items)
        {
            bool normalRow = true;
            var incrementalLineNumber = 0;

            foreach (var item in items)
            {
                PrintBodyCell(table, item.ScreeningSectionName, normalRow, Element.ALIGN_LEFT);

                PrintBodyCell(table, item.NumberOfReports.ToString(), normalRow, Element.ALIGN_CENTER);
                PrintBodyCell(table, item.TotalTime.AsFormattedString(), normalRow, Element.ALIGN_CENTER);
                PrintBodyCell(table, item.AverageTime.AsFormattedAverageString(), normalRow, Element.ALIGN_CENTER);
                normalRow = !normalRow;

                incrementalLineNumber++;
            }
        }

        protected override PdfPTable CreateTable()
        {

            var valueCellCount = ValuesCoumnsCount;
            float valuesCellWidth = 0.45F / valueCellCount;

            var tableColumnWidths = new List<float>(valueCellCount + 1);
            tableColumnWidths.Add(0.5f);//measure
            tableColumnWidths.AddRange(Enumerable.Repeat<float>(valuesCellWidth, valueCellCount));
            

            return new PdfPTable(tableColumnWidths.ToArray())
            {
                WidthPercentage = 100,
                SpacingBefore = 10,
                KeepTogether = true
            };
        }
       
        private PdfPCell PrintHeaderCell(string value, int horizontalAlignment, PdfPTable container, Action<PdfPCell> propertySetter = null)
        {
            cell = new PdfPCell(new Phrase(value, headerBlueFont))
            {
                HorizontalAlignment = horizontalAlignment,
                VerticalAlignment = Rectangle.ALIGN_MIDDLE,
                Border = Element.RECTANGLE | Rectangle.TOP_BORDER,
                BackgroundColor = HeaderBackground,
                Padding = 3
            };

            if (value == Resources.TextMessages.REPORT_INDICATOR_HEADER_PERCENT_POSITIVE || value == Resources.TextMessages.REPORT_INDICATOR_HEADER_PERCENT_NEGATIVE)
            {
                cell.PaddingLeft = 6;
                cell.PaddingRight = 6;
            }

            if (propertySetter != null)
            {
                propertySetter(cell);
            }

            container.AddCell(cell);

            return cell;
        }

        protected override void PrintHeaderOtherCollumns(PdfPTable table)
        {
          
        }
    }
}

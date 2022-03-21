using System;
using System.Globalization;
using FrontDesk.Server.Extensions;
using FrontDesk.Server.Reports;
using FrontDesk.Server.Screening;
using iTextSharp.text;
using iTextSharp.text.pdf;
using FrontDesk.Server.Screening.Services;
using FrontDesk.Server.Messages;
using FrontDesk.Server.Screening.Models;

namespace FrontDesk.Server
{
    public abstract class BhsIndicatorPdfReportBase<TModel> : PdfReport
        where TModel : class
    {
        protected readonly IScreeningResultService ScreeningResultService;
        protected readonly IScreeningDefinitionService ScreeningDefinitionService;
        protected readonly IBranchLocationService _branchLocationService;

        public TModel Model { get; protected set; } = null;

        public BhsIndicatorPdfReportBase(
            SimpleFilterModel filter,
            bool uniquePatientsReportType
        ) :
        this(filter, uniquePatientsReportType,
               new ScreeningResultService(),
               new ScreeningDefinitionService(),
               new BranchLocationService()
               )
        {
        }


        public BhsIndicatorPdfReportBase(
            SimpleFilterModel filter,
            bool uniquePatientsReportType,
            IScreeningResultService screeningResultService,
            IScreeningDefinitionService screeningDefinitionService,
            IBranchLocationService branchLocationService) :
            base()
        {
            this.filter = filter ?? new SimpleFilterModel();

            UniquePatientsReportType = uniquePatientsReportType;

            ScreeningResultService = screeningResultService ?? throw new ArgumentNullException(nameof(screeningResultService));
            ScreeningDefinitionService = screeningDefinitionService ?? throw new ArgumentNullException(nameof(screeningDefinitionService));
            _branchLocationService = branchLocationService ?? throw new ArgumentNullException(nameof(branchLocationService));
            ScreeningInfo = ScreeningDefinitionService.Get();


        }

        protected BhsIndicatorPdfReportBase() : this(new SimpleFilterModel(), true)
        {
        }

        protected SimpleFilterModel filter { get; set; }

        public bool UniquePatientsReportType { get; set; }

        public abstract int ValuesCoumnsCount { get; }

        public void PrintReportSettings(Document document)
        {
            float[] headerWidth = new float[] { 0.3f, 0.3f, 0.4f, };
            table = new PdfPTable(headerWidth) { WidthPercentage = 100 };

            AddLabelCell("Report Period");
            AddLabelCell("Report Type");
            AddLabelCell("Branch Locations");


            DateTimeOffset? dbMinDate = ScreeningResultService.GetMinDate();

            AddValueCell(String.Format("{0:MM/dd/yyyy} to {1:MM/dd/yyyy}",
                filter.StartDate.HasValue ? filter.StartDate : (dbMinDate.HasValue ? dbMinDate : DateTime.Now),
                filter.EndDate.HasValue ? filter.EndDate : DateTime.Now));

            AddValueCell(UniquePatientsReportType ? TextStrings.REPORT_INDICATOR_TYPE_UNIQUE_PATIENTS : TextStrings.REPORT_INDICATOR_TYPE_TOTAL_REPORTS);

            AddValueCell(filter.Location.HasValue? _branchLocationService.Get(filter.Location.Value).Name : "<< All >>");


            document.Add(table);
        }

        protected abstract PdfPTable CreateTable();


        /// <summary>
        /// Print total section
        /// </summary>
        protected void PrintTotal(Document document, long totalNumberOfPatientScreenings)
        {
            var table = new PdfPTable(new float[] { 0.9f, 0.1f });
            table.WidthPercentage = 100;
            table.SpacingBefore = 10;

            cell = new PdfPCell(new Phrase("Total number of patient records", headerBlueFont));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
            cell.BackgroundColor = HeaderBackground;
            cell.Padding = 3;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase(totalNumberOfPatientScreenings.ToString(), headerBlueFont));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = HeaderBackground;
            cell.Border = PdfPCell.RIGHT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
            cell.Padding = 3;
            table.AddCell(cell);

            document.Add(table);
        }

        protected void PrintBodyCell(PdfPTable table, string value, bool normalRow, int horizontalAlignment = Element.ALIGN_LEFT)
        {
            var cell = new PdfPCell(new Phrase(value, labelFont));
            cell.HorizontalAlignment = horizontalAlignment;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Border = PdfPCell.RECTANGLE;
            cell.Padding = 3;
            cell.BackgroundColor = normalRow ? NormalRowBackground : AltRowBackground;
            table.AddCell(cell);
        }

        protected void PrintBodyCellWithP(PdfPTable table, string text, string indicates, bool normalRow, int horizontalAlignment = Element.ALIGN_LEFT, float additionalLeftPadding = 0f)
        {
            var cell = new PdfPCell();
            text.PrintAsMainQuestionWithPreamble(cell);
            if (!String.IsNullOrEmpty(indicates))
            {
                Paragraph p = new Paragraph(indicates, labelFont);
                p.SetAlignment(Element.ALIGN_JUSTIFIED.ToString());
                p.IndentationLeft = 20;
                cell.AddElement(p);
            }
            cell.HorizontalAlignment = horizontalAlignment;
            cell.Border = Element.RECTANGLE;
            cell.Padding = 3;
            cell.PaddingLeft += additionalLeftPadding;
            cell.BackgroundColor = normalRow ? NormalRowBackground : AltRowBackground;
            table.AddCell(cell);
        }

        protected abstract void PrintHeaderOtherCollumns(PdfPTable table);

        protected void PrintHeaderCell(PdfPTable table, string value, int horizontalAlignment = Element.ALIGN_LEFT)
        {
            cell = new PdfPCell(new Phrase(value, headerBlueFont))
            {
                HorizontalAlignment = horizontalAlignment,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Border = PdfPCell.RECTANGLE | Rectangle.TOP_BORDER,
                BackgroundColor = HeaderBackground,
                Padding = 3
            };

            if (value == Resources.TextMessages.REPORT_INDICATOR_HEADER_PERCENT_POSITIVE || value == Resources.TextMessages.REPORT_INDICATOR_HEADER_PERCENT_NEGATIVE)
            {
                cell.PaddingLeft = 6;
                cell.PaddingRight = 6;
            }

            table.AddCell(cell);
        }

        protected void PrintHeaderQuestionOnfocusCell(PdfPTable table, string preamble, string text, int horizontalAlignment = Element.ALIGN_LEFT)
        {
            var cell = new PdfPCell();
            cell.AddElement(new Paragraph(preamble, labelFont));
            if (!String.IsNullOrEmpty(text))
            {
                Paragraph p = new Paragraph(text, labelFont);
                p.SetAlignment(Element.ALIGN_JUSTIFIED.ToString());
                p.IndentationLeft = 20;
                cell.AddElement(p);
            }
            cell.HorizontalAlignment = horizontalAlignment;
            cell.Border = Element.RECTANGLE | Rectangle.TOP_BORDER;
            cell.Padding = 3;
            cell.BackgroundColor = HeaderBackground;
            table.AddCell(cell);

        }

        protected void PrintCopyRight(string value)
        {
            float[] headerWidth = new float[] { 1f };
            var table = new PdfPTable(headerWidth);
            table.WidthPercentage = 100;
            table.SpacingBefore = 10;
            var cell = new PdfPCell(new Phrase(value, labelFont));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = PdfPCell.NO_BORDER;
            table.AddCell(cell);
            document.Add(table);
        }
    }
}
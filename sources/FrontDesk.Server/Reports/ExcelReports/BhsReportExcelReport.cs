using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

using FrontDesk.Server.Screening.Models.Export;

using System;
using System.IO;
using System.Linq;

namespace FrontDesk.Server.Reports.ExcelReports
{
    public class BhsReportExcelReport : ExcelExportBase, IExcelReport
    {
        public BhsReportExport Model { get; protected set; }


        private BhsDemographicsExcelReportPart _demographicsReport;
        private BhsVisitExcelReportPart _visitReport;
        private BhsFollowUpExcelReportPart _followUpReport;
        private DrugsOfChoiceResultsExcelReportPart _drugsOfChoiceReport;
        private CombinedResultsExcelReportPart _combinedReport;

        private ScreeningResultsExcelReportPart _screeningReport;

        public BhsReportExcelReport(BhsReportExport model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));

            _screeningReport = new ScreeningResultsExcelReportPart(this);
            _demographicsReport = new BhsDemographicsExcelReportPart(this);
            _visitReport = new BhsVisitExcelReportPart(this);
            _followUpReport = new BhsFollowUpExcelReportPart(this);
            _drugsOfChoiceReport = new DrugsOfChoiceResultsExcelReportPart(this);
            _combinedReport = new CombinedResultsExcelReportPart(this);
        }

        protected override void CreateContent(Stream ms)
        {
            using (var doc = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook, true))
            {
                WorkbookPart1 = doc.AddWorkbookPart();
                WorkbookPart1.Workbook = new Workbook();
                WorkbookPart1.Workbook.Save();
                Sheets sheets = doc.WorkbookPart.Workbook.AppendChild(new Sheets());

                bool sheetCreated = false;

                uint sheetIndex = 1;

                if (Model.Screenings.Rows.Count > 0)
                {
                    _screeningReport.CreateContent(sheets, sheetIndex++, Model.Screenings);

                    sheetCreated = true;
                }

                if (Model.Demographics.Any())
                {
                    _demographicsReport.CreateContent(sheets, sheetIndex++, Model.Demographics);

                    sheetCreated = true;
                }

                if (Model.Visits.Any())
                {
                    _visitReport.CreateContent(sheets, sheetIndex++, Model.Visits);

                    sheetCreated = true;
                }

                if (Model.FollowUps.Any())
                {
                    _followUpReport.CreateContent(sheets, sheetIndex++, Model.FollowUps);

                    sheetCreated = true;
                }

                if (Model.DrugsOfChoice.Rows.Count > 0)
                {
                    _drugsOfChoiceReport.CreateContent(sheets, sheetIndex++, Model.DrugsOfChoice);

                    sheetCreated = true;
                }
                if (Model.Combined.Rows.Count > 0)
                {
                    _combinedReport.CreateContent(sheets, sheetIndex++, Model.Combined);

                    sheetCreated = true;
                }

                if (!sheetCreated)
                {
                    //create default empty sheet
                    var worksheetPart = WorkbookPart1.AddNewPart<WorksheetPart>();
                    worksheetPart.Worksheet = new Worksheet(new SheetData());
                    SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
                    var sheet = CreateSheet(worksheetPart, "default", sheetIndex++);
                    sheets.Append(sheet);
                    WorkbookPart1.Workbook.Save();
                }
                WorkbookPart1.Workbook.Save();
            }
        }
        
    }

   
}

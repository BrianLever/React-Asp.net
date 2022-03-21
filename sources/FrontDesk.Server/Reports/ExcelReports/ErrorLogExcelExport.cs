using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

using ScreenDox.Server.Models;

using System;
using System.IO;

namespace FrontDesk.Server.Reports.ExcelReports
{
    public class ErrorLogExcelExport : ExcelExportBase, IExcelReport
    {
        public SearchResponse<ErrorLogItem> Model { get; protected set; }


        private readonly ErrorLogExcelReportPart _reportSheet;

        public ErrorLogExcelExport(SearchResponse<ErrorLogItem> model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));

            _reportSheet = new ErrorLogExcelReportPart(this);
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

                if (Model.Items.Count > 0)
                {
                    _reportSheet.CreateContent(sheets, sheetIndex++, Model.Items);

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

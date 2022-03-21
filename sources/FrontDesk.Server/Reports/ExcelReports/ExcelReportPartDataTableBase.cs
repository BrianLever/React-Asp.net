using Common.Logging;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontDesk.Server.Reports.ExcelReports
{
    public abstract class ExcelReportPartDataTableBase
    {
        private ILog _logger = LogManager.GetLogger<ExcelReportPartDataTableBase>();
        protected IExcelReport _parentReport;

        protected TableRowDefinition<DataRow> Row = new TableRowDefinition<DataRow>();

        public string SheetName { get; }

        public ExcelReportPartDataTableBase(IExcelReport parentReport, string sheetName)
        {
            if (string.IsNullOrEmpty(sheetName))
            {
                throw new ArgumentException("sheetName should not be empty", nameof(sheetName));
            }

            _parentReport = parentReport ?? throw new ArgumentNullException(nameof(parentReport));
            SheetName = sheetName;
        }

        protected abstract void InitTableRowDefinition(DataTable model);

        public void CreateContent(Sheets sheets, uint sheetIndex, DataTable model)
        {
            var worksheetPart = _parentReport.WorkbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            InitTableRowDefinition(model);


            var sheet = _parentReport.CreateSheet(worksheetPart, SheetName, sheetIndex);

            CreateContent(worksheetPart, model);
            sheets.Append(sheet);
        }

        protected void CreateContent(WorksheetPart worksheetPart, DataTable items)
        {
            SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            DefineColumnProperties(worksheetPart);

            RenderHeaderRow(worksheetPart, sheetData);

            RenderDataRows(worksheetPart, sheetData, items);

        }



        protected void DefineColumnProperties(WorksheetPart worksheetPart)
        {
            //render columns
            uint colIndex = 1;
            var excelColumns = worksheetPart.Worksheet.GetFirstChild<Columns>() ?? new Columns();

            foreach (var col in Row.Columns)
            {
                var colProp = new Column
                {
                    Min = colIndex,
                    Max = colIndex,
                    Width = 16,
                    BestFit = true,
                    CustomWidth = true
                };
                if (col.Width > 0)
                {
                    colProp.Width = col.Width;
                }

                excelColumns.Append(colProp);

                colIndex++;
            }

            worksheetPart.Worksheet.InsertAt(excelColumns, 0);
        }

        private void RenderHeaderRow(WorksheetPart worksheetPart, SheetData sheetData)
        {
            var row = new Row() { RowIndex = 1 };
            sheetData.AppendChild(row);
        
            //add header row
            foreach (var col in Row.Columns)
            {
                row.AppendChild(AddCellWithText(col.Title));
            }
        }

        private void RenderDataRows(WorksheetPart worksheetPart, SheetData sheetData, DataTable table)
        {
            uint rowIndex = 2;

            foreach (DataRow item in table.Rows)
            {
                var row = new Row() { RowIndex = rowIndex++ };
                sheetData.Append(row);

                //add header row
                foreach (var col in Row.Columns)
                {
                    _logger.DebugFormat("[ExcelReports] Rendering column {0}", col.Title);

                    row.Append(ExcelCellFactory.Create(col, item));

                    _logger.DebugFormat("[ExcelReports] Rendering column {0} completed", col.Title);

                }
            }
        }

        internal Cell AddCellWithText(string text, string cellReference = null)
        {
            InlineString inlineString = new InlineString();

            inlineString.AppendChild(new Text
            {
                Text = text ?? string.Empty
            });

            var cell = new Cell
            {
                CellReference = cellReference,
                DataType = CellValues.InlineString,
            };
            cell.AppendChild(inlineString);

            return cell;
        }

    }
}

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
    public abstract class BhsListPrintoutBase : PdfReport
    {
     
        protected abstract TableColumnDefinition[] Columns { get; }

        protected abstract float[] ColumnWidths { get; }

        protected abstract string[] Header { get; }


        public override void PrintReport()
        {
            //print header section
            PrintHeader(document, Header);

            //Print report filter
            PrintReportFilter(document);

            PrintResults(document);
        }

        protected abstract void PrintReportFilter(Document document);
        

        protected virtual void PrintHeader(PdfPTable parentTable)
        {
            foreach (var column in Columns)
            {
                PrintHeaderCell(parentTable, column.Name, column.HorizontalAlignment);
            }
        }


        protected void PrintHeaderCell(PdfPTable parentTable, string value, int horizontalAlignment = Element.ALIGN_LEFT)
        {
            cell = new PdfPCell(new Phrase(value, headerBlueFont))
            {
                HorizontalAlignment = horizontalAlignment,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Border = PdfPCell.RECTANGLE | Rectangle.TOP_BORDER,
                BackgroundColor = HeaderBackground,
                Padding = 3
            };

            parentTable.AddCell(cell);
        }

        protected void PrintBodyCell(PdfPTable parentTable, string value, bool normalRow, int horizontalAlignment = Element.ALIGN_LEFT, float additionalLeftPadding = 0f)
        {
            var cell = new PdfPCell(new Phrase(value, labelFont));
            cell.HorizontalAlignment = horizontalAlignment;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Border = PdfPCell.RECTANGLE;
            cell.Padding = 3;
            if (additionalLeftPadding > 0)
            {
                cell.PaddingLeft = additionalLeftPadding;
            }
            cell.BackgroundColor = normalRow ? NormalRowBackground : AltRowBackground;
            parentTable.AddCell(cell);
        }

        /// <summary>
        /// print tool section
        /// </summary>
        protected void PrintResults(Document document)
        {

            table = CreateTable();

            //print header
            PrintHeader(table);

            PrintItems(table);

            document.Add(table);
        }

        protected PdfPTable CreateTable()
        {

            return new PdfPTable(ColumnWidths)
            {
                WidthPercentage = 100,
                SpacingBefore = 10,
                KeepTogether = false
            };
        }

        protected abstract void PrintItems(PdfPTable parentTable);

        protected void PrintThousandRowsLimitMessage(PdfPTable parentTable)
        {
            var cell = new PdfPCell(new Phrase(TextStrings.PDF_LIST_FIRST1000_LIMIT_MESSAGE, labelFont));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Border = PdfPCell.NO_BORDER;
            cell.Padding = 3;
            cell.Colspan = Columns.Length;
            parentTable.AddCell(cell);
        }
    }
}

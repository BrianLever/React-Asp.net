using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace FrontDesk.Server.Reports.ExcelReports
{
    public interface IExcelReport
    {
        WorkbookPart WorkbookPart { get; }
        Sheet CreateSheet(WorksheetPart worksheetPart, string name, UInt32 index);
    }


    public abstract class ExcelExportBase : IExcelReport
    {
        //protected List<TableColumnDefinition<TItem, object>> Columns;
        public string Title { get; set; }

        public WorkbookPart WorkbookPart { get { return WorkbookPart1; } }

        public WorkbookPart WorkbookPart1 { get; set; }


        public Sheet CreateSheet(WorksheetPart worksheetPart, string name, UInt32 index)
        {
            Sheet sheet = new Sheet()
            {
                Id = WorkbookPart1.GetIdOfPart(worksheetPart),
                SheetId = index,
                Name = name
            };

            return sheet;
        }

        public void Create(HttpContext context, string filename)
        {
            filename = AddExtensionToFilename(filename);

            context.Response.AppendHeader("Content-Disposition", "attachment;filename=\"{0}\"".FormatWith(filename));
            context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            context.Response.BinaryWrite(CreateContent());
        }

        public void Create(HttpResponseMessage httpResponseMessage, string filename)
        {
            filename = AddExtensionToFilename(filename);

            // make sure we close the memory stream if CreatePDF will fail
            using (var ms = new MemoryStream())
            {
                // generate excel  file
                var buffer = CreateContent();
                // create new memory stream that will be closed at caller level.
                httpResponseMessage.Content = new StreamContent(new MemoryStream(buffer));
                httpResponseMessage.Content.Headers.ContentLength = buffer.Length;

                httpResponseMessage.Content.Headers.ContentDisposition
                    = ContentDispositionHeaderValue.Parse("attachment;filename=\"{0}\"".FormatWith(filename));

                httpResponseMessage.Content.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
                httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }

        }

        private string AddExtensionToFilename(string filename, string extension = ".xlsx")
        {
            if (!string.IsNullOrEmpty(Path.GetExtension(filename)))
            {
                return filename;
            }

            return filename + extension;
        }

        protected byte[] CreateContent()
        {
            var filename = Path.GetTempFileName();

            using (var ms = File.Open(filename, FileMode.OpenOrCreate))
            {
                CreateContent(ms);
            }

            return File.ReadAllBytes(filename);
        }

        protected abstract void CreateContent(Stream ms);
    }
}
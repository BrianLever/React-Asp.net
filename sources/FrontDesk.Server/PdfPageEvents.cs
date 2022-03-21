using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrontDesk.Server.Reports;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace FrontDesk.Server
{
    public class PdfPageEvents : IPdfPageEvent
    {
        public PdfReport Report;
        protected PdfTemplate total;
        protected PdfTemplate header;
        protected List<PdfTemplate> pageNumberTemplates = new List<PdfTemplate>();

        public PdfPageEvents(PdfReport report)
        {
            Report = report;
        }

        #region IPdfPageEvent Members

        public void OnChapter(PdfWriter writer, Document document, float paragraphPosition, Paragraph title)
        {
            //throw new NotImplementedException();
        }

        public void OnChapterEnd(PdfWriter writer, Document document, float paragraphPosition)
        {
            //throw new NotImplementedException();
        }

        public void OnCloseDocument(PdfWriter writer, Document document)
        {
            if (document.PageNumber <= 2) return;
            var count = 1;
            foreach (var template in pageNumberTemplates)
            {
                template.BeginText();
                template.SetFontAndSize(PdfDocumentSettings.valueFont.BaseFont, PdfDocumentSettings.valueFont.Size);

                template.SetTextMatrix(0, 0);
                template.ShowText("Page " + count + " of " + (writer.PageNumber - 1));

                template.EndText();
                count++;
            }
        }

        protected void PutPageNumberFooterOnPage(PdfWriter writer, Document document, Font font, PdfTemplate template, bool centered = false)
        {
            var textSize = font.BaseFont.GetWidthPoint("Page 0 of 00", font.Size);
            var textBase = document.Bottom - 10;

            float xPos;

            if (!centered)
            {
                xPos = document.Right - textSize - 50; //leave space for document version
            }
            else
            {
                xPos = document.Left + ((document.Right - document.Left) - (textSize)) / 2.0F;
            }

            writer.DirectContent.AddTemplate(template, xPos, textBase);
        }


        public void OnEndPage(PdfWriter writer, Document document)
        {
            Report.PrintFooter(writer, document, total);

            // Add a unique (empty) template for each page here
            PdfTemplate t = writer.DirectContent.CreateTemplate(200, 50);
            pageNumberTemplates.Add(t);

            PutPageNumberFooterOnPage(writer, document, PdfDocumentSettings.labelFont, t);


        }




        public void OnGenericTag(PdfWriter writer, Document document, Rectangle rect, string text)
        {
            //throw new NotImplementedException();
        }

        public void OnOpenDocument(PdfWriter writer, Document document)
        {
            total = writer.DirectContent.CreateTemplate(100, 100);
            total.BoundingBox = new Rectangle(-20, -20, 100, 100);
            header = writer.DirectContent.CreateTemplate(200, 200);
            header.BoundingBox = new Rectangle(20, 20, 200, 200);
        }

        public void OnParagraph(PdfWriter writer, Document document, float paragraphPosition)
        {
            //throw new NotImplementedException();
        }

        public void OnParagraphEnd(PdfWriter writer, Document document, float paragraphPosition)
        {
            //throw new NotImplementedException();
        }

        public void OnSection(PdfWriter writer, Document document, float paragraphPosition, int depth, Paragraph title)
        {
            //throw new NotImplementedException();
        }

        public void OnSectionEnd(PdfWriter writer, Document document, float paragraphPosition)
        {
            //throw new NotImplementedException();
        }

        public void OnStartPage(PdfWriter writer, Document document)
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}

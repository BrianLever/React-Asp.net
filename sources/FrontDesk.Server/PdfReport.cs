using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web;
using System.IO;
using FrontDesk.Server.Reports;
using FrontDesk.Server.Utils;
using System.Net.Http;
using System.Net.Http.Headers;
using FrontDesk.Server.Configuration;

namespace FrontDesk.Server
{
    public abstract class PdfReport
    {

        protected virtual bool RenderVersionInFooter { get { return true; } }

        #region property for create pdf

        protected PdfWriter writer;
        public Document document { get; set; }
        public PdfPTable table;
        public PdfPCell cell;

        public ScreeningResult ScreeningResult { get; set; }
        public FrontDesk.Screening ScreeningInfo;

        #region Report`s font and table parameters

        public static Font headerFont = FontFactory.GetFont(BaseFont.HELVETICA, 13, Font.BOLD);
        public static Font headerSubFont = FontFactory.GetFont(BaseFont.HELVETICA, 12, Font.NORMAL);

        public static Font labelFont = FontFactory.GetFont(BaseFont.HELVETICA, 8, Font.NORMAL);
        public static Font valueFont = FontFactory.GetFont(BaseFont.HELVETICA, 8, Font.NORMAL, new Color(0x3D, 0x56, 0xA7));
        public static Font headerBlueFont = FontFactory.GetFont(BaseFont.HELVETICA, 8, Font.BOLD, new Color(0x3D, 0x56, 0xA7));
        public static Font boldFont = FontFactory.GetFont(BaseFont.HELVETICA, 8, Font.BOLD);
        public static Font sectionTitleFont = FontFactory.GetFont(BaseFont.HELVETICA, 9, Font.BOLD, Color.WHITE);
        public static Font sectionTitleBigFont = FontFactory.GetFont(BaseFont.HELVETICA, 10, Font.BOLD, Color.WHITE);
        public static Font commentFont = FontFactory.GetFont(BaseFont.HELVETICA, 6, Font.ITALIC);
        public static Font preambleFont = FontFactory.GetFont(BaseFont.HELVETICA, 9, Font.BOLD | Font.ITALIC);
        public static Font depresionCommentFont = FontFactory.GetFont(BaseFont.HELVETICA, 6, Font.NORMAL);
        public static Font depresionCommentBFont = FontFactory.GetFont(BaseFont.HELVETICA, 6, Font.BOLD);
        protected Font symbolFont;

        public Color blueBackground = new Color(0xAD, 0xD5, 0xF2);
        public Color grayBackground = new Color(0xe0, 0xe0, 0xe0);
        public Color whiteBackground = Color.WHITE;

        public Color HeaderBackground = new Color(0xBC, 0xBC, 0xBC);
        public Color NormalRowBackground = new Color(0xFF, 0xFF, 0xFF);
        public Color AltRowBackground = new Color(0xEE, 0xEE, 0xEE);
        public Color GreenBackground = new Color(0xbe, 0xd5, 0xaa);
        public Color GrayBorder = new Color(0xee, 0xee, 0xee);


        public readonly string PathToFooterLogo;
        public readonly string PathToHeaderLogo;
        #endregion

        #endregion

        protected PdfReport()
        {
            string TtfFontsDirectory = FilePathResolver.ResolveFilePath("~/App_Data/TTFFonts/");
            BaseFont bfArial = BaseFont.CreateFont(TtfFontsDirectory + "arial.ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
            BaseFont bfArialbd = BaseFont.CreateFont(TtfFontsDirectory + "arialbd.ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
            BaseFont bfArialbi = BaseFont.CreateFont(TtfFontsDirectory + "arialbi.ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
            BaseFont bfAriali = BaseFont.CreateFont(TtfFontsDirectory + "ariali.ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.EMBEDDED);
            BaseFont bfTimesi = BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1252, BaseFont.EMBEDDED);
            BaseFont bfSybbol = BaseFont.CreateFont(BaseFont.SYMBOL, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            headerFont = FontFactory.GetFont(bfArialbd.ToString(), 13, Font.BOLD);
            labelFont = FontFactory.GetFont(bfArial.ToString(), 8, Font.NORMAL);
            valueFont = FontFactory.GetFont(bfArial.ToString(), 8, Font.NORMAL, new Color(0x3D, 0x56, 0xA7));
            boldFont = FontFactory.GetFont(bfArialbd.ToString(), 8, Font.BOLD);
            sectionTitleFont = FontFactory.GetFont(bfArialbd.ToString(), 9, Font.BOLD, Color.WHITE);
            sectionTitleBigFont = FontFactory.GetFont(bfArialbd.ToString(), 10, Font.BOLD, Color.WHITE);
            depresionCommentFont = FontFactory.GetFont(bfArial.ToString(), 6, Font.NORMAL);
            depresionCommentBFont = FontFactory.GetFont(bfArialbd.ToString(), 6, Font.BOLD);
            commentFont = FontFactory.GetFont(BaseFont.TIMES_ITALIC, 7, Font.ITALIC);
            preambleFont = FontFactory.GetFont(bfArialbi.ToString(), 9, Font.BOLD | Font.ITALIC);

            symbolFont = new Font(bfSybbol, 9, Font.NORMAL);

            PathToFooterLogo = FilePathResolver.ResolveFilePath(AppSettings.GetStringValue("LogoPath", string.Empty));
            PathToHeaderLogo = FilePathResolver.ResolveFilePath(AppSettings.GetStringValue("HeaderLogoPath", string.Empty));
        }

        /// <summary>
        /// Print PDF document sections
        /// </summary>
        public abstract void PrintReport();

        public void CreatePDF(System.Web.HttpContext context, string fileName)
        {
            context.Response.AppendHeader("Content-Disposition", "attachment;filename=\"" + fileName + "\"");
            context.Response.ContentType = "application/pdf";

            CreatePDF(context.Response.OutputStream);
        }

        /// <summary>
        /// Return pdf binary stream as Http Message content
        /// </summary>
        /// <param name="httpResponseMessage"></param>
        /// <param name="fileName"></param>
        public void CreatePDF(HttpResponseMessage httpResponseMessage, string fileName)
        {
            // make sure we close the memory stream if CreatePDF will fail
            using (var ms = new MemoryStream())
            {
                // generate pdf file
                CreatePDF(ms);

                // return new stream using binary data
                var buffer = ms.ToArray();
                // create new memory stream that will be closed at caller level.
                httpResponseMessage.Content = new StreamContent(new MemoryStream(buffer));
                httpResponseMessage.Content.Headers.ContentLength = buffer.Length;

                httpResponseMessage.Content.Headers.ContentDisposition
                    = ContentDispositionHeaderValue.Parse("attachment;filename=\"{0}\"".FormatWith(fileName));
                
                httpResponseMessage.Content.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
                httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            }

        }

        public void CreatePDF(Stream output)
        {
            document = new Document(PageSize.LETTER);
            document.SetMargins(0.5F * 40, 0.5F * 40, 0.5F * 50, 0.5F * 60);
            writer = PdfWriter.GetInstance(document, output);
            writer.ViewerPreferences = PdfWriter.PageLayoutSinglePage;


            PdfPageEvents events = new PdfPageEvents(this);
            writer.PageEvent = events;


            document.Open();

            PrintReport();

            document.Close();
        }

        /// <summary>
        /// Print PDF document header section
        /// </summary>
        public void PrintHeader(Document document, string[] headerText)
        {
            if (headerText == null)
            {
                throw new ArgumentNullException("headerText");
            }

            table = new PdfPTable(new float[] { 0.3f, 0.4f, 0.3f });
            table.WidthPercentage = 100;
            table.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;

            //print logo
            Image logoImage = iTextSharp.text.Image.GetInstance(PathToHeaderLogo);
            logoImage.Alignment = Element.ALIGN_LEFT;
            logoImage.ScaleToFit(1F * 72, 0.5F * 72);



            cell = new PdfPCell(logoImage, false);
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Border = PdfPCell.BOTTOM_BORDER;
            cell.PaddingBottom = 15;
            table.AddCell(cell);

            Phrase headerPhrase = null;
            //Header Text
            if (headerText.Length == 0)
            { //single header line
                headerPhrase = new Phrase(headerText[0], headerFont);
            }
            else
            { //multi-line header
                headerPhrase = new Paragraph();
                headerPhrase.Add(new Phrase(headerText[0], headerFont));
                for (int i = 1; i < headerText.Length; i++)
                {
                    headerPhrase.Add("\n");
                    headerPhrase.Add(new Phrase(headerText[i], headerSubFont));
                }
            }

            cell = new PdfPCell(headerPhrase);
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 15;
            cell.Border = PdfPCell.BOTTOM_BORDER;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("", headerFont));
            cell.Border = PdfPCell.BOTTOM_BORDER;
            table.AddCell(cell);

            document.Add(table);
        }


        /// <summary>
        /// print PDF document footer section
        /// </summary>
        public void PrintFooter(PdfWriter writer, Document document, PdfTemplate total)
        {

            PdfPTable footerTable = GetFooterTable();

            footerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.BottomMargin, writer.DirectContent);

        }


        protected virtual string GetFooterDatePlaceholderContent()
        {
            return String.Format("{0:MM/dd/yyyy HH:mm:ss zzz}", ScreeningResult != null ? ScreeningResult.CreatedDate : DateTime.Now);
        }

        protected virtual PdfPTable GetFooterTable()
        {
            float[] footerWidth = new float[] { 0.3f, 0.4f, 0.3f };
            PdfPTable footerTable = new PdfPTable(footerWidth);
            footerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin; ;

            footerTable.SpacingBefore = 10;
            footerTable.WidthPercentage = 100;

            //print logo
            Image logoImage = iTextSharp.text.Image.GetInstance(PathToFooterLogo);
            logoImage.Alignment = Element.ALIGN_LEFT;
            logoImage.ScaleToFit(0.7F * 72, 0.15F * 72);

            PdfPCell footercell = new PdfPCell(logoImage, false);
            footercell.HorizontalAlignment = Element.ALIGN_LEFT;
            footercell.PaddingTop = 2;
            footercell.Border = PdfPCell.TOP_BORDER;
            footerTable.AddCell(footercell);

            //print create date
            footercell = new PdfPCell(new Phrase(GetFooterDatePlaceholderContent(), labelFont));
            footercell.HorizontalAlignment = Element.ALIGN_CENTER;
            footercell.Border = PdfPCell.TOP_BORDER;
            footerTable.AddCell(footercell);

            //print version and create date
            footercell = new PdfPCell(new Phrase(RenderVersionInFooter ? "V.3.10" : string.Empty, boldFont));
            footercell.HorizontalAlignment = Element.ALIGN_RIGHT;
            footercell.Border = PdfPCell.TOP_BORDER;
            footerTable.AddCell(footercell);

            return footerTable;
        }

        protected PdfPCell AddLabelCell(string cellLabel)
        {
            return AddLabelCell(table, cellLabel);
        }

        protected PdfPCell AddLabelCell(PdfPTable parentTable, string callLabel, int border = Rectangle.NO_BORDER)
        {
            var cell = new PdfPCell(new Phrase(callLabel, labelFont));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = border;
            cell.Padding = 1;
            parentTable.AddCell(cell);
            return cell;
        }

        protected PdfPCell AddValueCell(string cellValue)
        {
            return AddValueCell(table, cellValue);
        }
        protected PdfPCell AddValueCell(PdfPTable parentTable, string callValue)
        {
            var cell = new PdfPCell(new Phrase(callValue, valueFont));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.BOTTOM_BORDER;
            cell.Padding = 1;
            cell.PaddingBottom = 2;
            cell.PaddingTop = 0;
            parentTable.AddCell(cell);

            return cell;
        }

    }
}

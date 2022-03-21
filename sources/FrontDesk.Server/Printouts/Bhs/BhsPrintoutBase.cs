using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using FrontDesk.Common.Bhservice;
using FrontDesk.Common.Extensions;
using FrontDesk.Server.Extensions;
using FrontDesk.Server.Utils;

namespace FrontDesk.Server.Printouts.Bhs
{
    public abstract class BhsPdfPagePrintoutBase : PdfReport
    {
        protected abstract string[] PageTitle {get;}
        protected override bool RenderVersionInFooter => false;

        protected virtual void PrintFollowUpSchedule(PdfPTable containerTable, IBhsFollowUpSchedule model)
        {
            PrintFormRow(containerTable, "New visit/referral recommendation accepted", model.NewVisitReferralRecommendationAccepted.Name);
            PrintFormRow(containerTable, "Reason recommendation NOT accepted", model.ReasonNewVisitReferralRecommendationNotAccepted.Name);
            PrintFormRow(containerTable, "New visit date", model.NewVisitDate.FormatAsDate());
            PrintFormRow(containerTable, "Discharged", model.Discharged.Name);

            PrintFormRow(containerTable, "Create follow-up?", model.ThirtyDatyFollowUpFlag);
            if (model.ThirtyDatyFollowUpFlag)
            {
                PrintFormRow(containerTable, "Follow-up date", model.FollowUpDate.FormatAsDate());
            }

            PrintFormRowFormatted(containerTable, "Notes", model.Notes);

        }

        /// <summary>
        /// Print PDF document header section with patient info
        /// </summary>
        public void PrintPatientInfo(Document document)
        {
            //Create header
            PrintHeader(document, PageTitle);

            float[] headerWidth = new float[] { 0.27f, 0.23f, 0.15f, 0.17f, 0.18f };
            var table = new PdfPTable(headerWidth);
            table.WidthPercentage = 100;
            table.SpacingAfter = 5;

            //Patient information
            //first row labels
            AddLabelCell(table, "Patient Last Name");
            AddLabelCell(table, "First Name");
            AddLabelCell(table, "Middle Name");
            AddLabelCell(table, "Date of Birth");
            AddLabelCell(table, "Record Number");
            //first row values
            AddValueCell(table, ScreeningResult.LastName);
            AddValueCell(table, ScreeningResult.FirstName);
            AddValueCell(table, ScreeningResult.MiddleName);
            AddValueCell(table, String.Format("{0:MM/dd/yyyy}", ScreeningResult.Birthday));
            AddValueCell(table, ScreeningResult.ID.ToString());
            //second row labels
            AddLabelCell(table, "Mailing Address");
            AddLabelCell(table, "City");
            AddLabelCell(table, "State");
            AddLabelCell(table, "ZIP Code");
            AddLabelCell(table, "Primary Phone Number");
            //second row values
            AddValueCell(table, ScreeningResult.StreetAddress.FormatAsNullableString());
            AddValueCell(table, ScreeningResult.City.FormatAsNullableString());
            AddValueCell(table, ScreeningResult.StateName.FormatAsNullableString().ToUpperInvariant());
            AddValueCell(table, ScreeningResult.ZipCode.FormatAsNullableString());
            AddValueCell(table, ScreeningResult.Phone.FormatAsNullableString());

            document.Add(table);

            PrintHeaderLine(document);
        }


        protected abstract void PrintHeaderLine(Document document);
       
        protected void PrintFormRow(PdfPTable table, string label, string value)
        {
            table.AddCell(CreateLabelCell(label));
            table.AddCell(CreateValueCell(value));

        }

        protected void PrintFormRowFormatted(PdfPTable table, string label, string value)
        {
            table.AddCell(CreateLabelCellForMultiline(label));
            table.AddCell(CreateRichFormattedCell(value?? String.Empty));

        }

        protected void PrintFormRow(PdfPTable table, string label, bool value)
        {
            table.AddCell(CreateLabelCell(label));
            table.AddCell(CreateValueCell(value));

        }

        protected void PrintFormRow(PdfPTable table, string label, ScreeningResultValue value)
        {
            table.AddCell(CreateLabelCell(label));

            var contentCell = new PdfPCell
            {
                Border = Rectangle.NO_BORDER,
                Padding = 0
            };


            if (value != null)
            {
                var contentTable = new PdfPTable(new[] { 0.04f, 0.96f })
                {
                    WidthPercentage = 100,
                    SpacingAfter = 0

                };
                contentTable.AddCell(new PdfPCell(new Phrase(value.ScoreLevel.ToString(), labelFont))
                {
                    Border = Rectangle.NO_BORDER,
                    PaddingTop = 5,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                });
                contentTable.AddCell(new PdfPCell(new Phrase(value.ScoreLevelLabel, labelFont))
                {
                    Border = Rectangle.NO_BORDER,
                    PaddingTop = 5,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                });
                contentCell.AddElement(contentTable);
            }

            table.AddCell(contentCell);
        }

        protected PdfPCell CreateLabelCell(string text)
        {
            var labelText = string.IsNullOrEmpty(text) ? text : text + ":";

            PdfPCell cell = new PdfPCell(new Phrase(labelText, labelFont))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingRight = 2,
                PaddingTop = 5
            };
            return cell;
        }

        protected PdfPCell CreateLabelCellForMultiline(string text)
        {
            var labelText = string.IsNullOrEmpty(text) ? text : text + ":";

            PdfPCell cell = new PdfPCell(new Phrase(labelText, labelFont))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                VerticalAlignment = Element.ALIGN_TOP,
                PaddingRight = 2,
                PaddingTop = 7
            };
            return cell;
        }

        protected PdfPCell CreateValueCell(string text)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, labelFont))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingTop = 5
            };
            return cell;
        }

        protected PdfPCell CreateRichFormattedCell(string text)
        {
            PdfPCell cell = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                VerticalAlignment = Element.ALIGN_TOP,
                PaddingTop = 0,
            };

            PdfReachTextFormatter.Init(text, cell).Process();

            return cell;
        }

        protected Image GetFlagValueImage(bool selected)
        {
            if (selected)
            {
                return Image.GetInstance(FilePathResolver.ResolveFilePath(System.Web.Configuration.WebConfigurationManager.AppSettings["ChekedImagePath"]));
            }

            return Image.GetInstance(FilePathResolver.ResolveFilePath(System.Web.Configuration.WebConfigurationManager.AppSettings["UnchekedImagePath"]));

        }


        protected PdfPCell CreateValueCell(bool value)
        {
            var image = GetFlagValueImage(value);
            image.ScaleToFit(0.125F * 72, 0.125F * 72);
            image.Alignment = Element.ALIGN_TOP;

            PdfPCell cell = new PdfPCell(image)
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingLeft = 2,
                PaddingTop = 5
            };
            return cell;
        }


        protected void PrintTwoColumnScreeningResult(PdfPTable table, string label, string value, string description)
        {

            table.AddCell(CreateLabelCell(label));

            var contentCell = new PdfPCell
            {
                Border = Rectangle.NO_BORDER,
                Padding = 0
            };


            var contentTable = new PdfPTable(new[] { 0.4f, 0.6f })
            {
                WidthPercentage = 100,
                SpacingAfter = 0

            };

            contentTable.AddCell(new PdfPCell(new Phrase(value, labelFont))
            {
                Border = Rectangle.NO_BORDER,
                PaddingTop = 5,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE
            });
            contentTable.AddCell(new PdfPCell(new Phrase(description, labelFont))
            {
                Border = Rectangle.NO_BORDER,
                PaddingTop = 5,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE
            });
            contentCell.AddElement(contentTable);

            table.AddCell(contentCell);

        }

        protected override string GetFooterDatePlaceholderContent()
        {
            return DateTimeOffset.Now.FormatAsDateWithTime();
        }
    }
}

using System;
using System.Text;
using FrontDesk.Server.Extensions;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace FrontDesk.Server.Reports
{
    public class BhsPdfReport : PdfReport
    {
       

        #region constructor

        public BhsPdfReport()
            : base()
        {
        }

        public BhsPdfReport(ScreeningResult screeningResult) :
            base()
        {
            ScreeningResult = screeningResult;
            if (ScreeningResult != null)
            {

                ScreeningInfo = ServerScreening.GetByID(this.ScreeningResult.ScreeningID);
            }
        }

        #endregion

        public override void PrintReport()
        {
            //print header section
            PrintPatientInfo(document);
            if (ScreeningResult.IsPassedAnySection)
            {
                //print Tobacco section
                new TobaccoBhsPdfReportSection(this).RenderSectionContent(document);

                //print Alcohol section
                new AlcoholBhsPdfReportSection(this).RenderSectionContent(document);
                //print DAST-10
                new SubstanceAbuseBhsPdfReportSection(this).RenderSectionContent(document);
                //print DrugOfChoice
                new DrugOfChoicePdfReportSection(this).RenderSectionContent(document);

                ////print Anxiety section
                new AnxietyBhsPdfReportSection(this).RenderSectionContent(document);

                ////print Depression section
                new DepressionBhsPdfReportSection(this).RenderSectionContent(document);
                ////print Violence section
                new ViolenceBhsPdfReportSection(this).RenderSectionContent(document);

                // print Problem Gambling section
                new ProblemGamblingBhsPdfReportSection(this).RenderSectionContent(document);
            }
        }
        
        /// <summary>
        /// Print PDF document header section with patient info
        /// </summary>
        public void PrintPatientInfo(Document document)
        {
            //Create header
            PrintHeader(document, new string[]{ "Behavioral Health Screening Report"});

            float[] headerWidth = new float[] { 0.27f, 0.23f, 0.15f, 0.17f, 0.18f };
            table = new PdfPTable(headerWidth);
            table.WidthPercentage = 100;
            table.SpacingAfter = 5;

            //Patient information
            //first row labels
            AddLabelCell("Patient Last Name");
            AddLabelCell("First Name");
            AddLabelCell("Middle Name");
            AddLabelCell("Date of Birth");
            AddLabelCell("Record Number");
            //first row values
            AddValueCell(ScreeningResult.LastName);
            AddValueCell(ScreeningResult.FirstName);
            AddValueCell(ScreeningResult.MiddleName);
            AddValueCell(String.Format("{0:MM/dd/yyyy}", ScreeningResult.Birthday));
            AddValueCell(String.Empty);
            //second row labels
            AddLabelCell("Mailing Address");
            AddLabelCell("City");
            AddLabelCell("State");
            AddLabelCell("ZIP Code");
            AddLabelCell("Primary Phone Number");
            //second row values
            AddValueCell(ScreeningResult.StreetAddress.FormatAsNullableString());
            AddValueCell(ScreeningResult.City.FormatAsNullableString());
            AddValueCell(ScreeningResult.StateName.FormatAsNullableString().ToUpperInvariant());
            AddValueCell(ScreeningResult.ZipCode.FormatAsNullableString());
            AddValueCell(ScreeningResult.Phone.FormatAsNullableString());

            document.Add(table);
        }
       
    }
}

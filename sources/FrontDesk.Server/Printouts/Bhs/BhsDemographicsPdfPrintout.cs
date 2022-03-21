using System;
using iTextSharp.text;
using iTextSharp.text.pdf;
using FrontDesk.Common.Bhservice;
using FrontDesk.Common.Extensions;
using System.Linq;
using FrontDesk.Common;

namespace FrontDesk.Server.Printouts.Bhs
{
    public class BhsDemographicsPdfPrintout : BhsPdfPagePrintoutBase
    {
        private readonly BhsDemographics Model;

        protected override string[] PageTitle => new[] { "Patient Demographics" };

        #region constructor

        public BhsDemographicsPdfPrintout(BhsDemographics bhsModel) :
            base()
        {
            if (bhsModel == null)
            {
                throw new ArgumentNullException(nameof(bhsModel));
            }

            ScreeningResult =  bhsModel.ToScreeningResultModel();
            Model = bhsModel;

        }

        #endregion

        public override void PrintReport()
        {
            //print header section

            PrintPatientInfo(document);

            float[] containerTableWidth = new float[] { 0.4f, 0.6f };
            var containerTable = new PdfPTable(containerTableWidth)
            {
                WidthPercentage = 100,
                SpacingAfter = 0,
                SpacingBefore = 10,
                KeepTogether = false
            };

            PrintFormRow(containerTable, "Race", Model.Race.Name);
            PrintFormRow(containerTable, "Gender", Model.Gender.Name);
            PrintFormRow(containerTable, "Sexual Orientation", Model.SexualOrientation.Name);
            PrintFormRow(containerTable, "Tribal affiliation", Model.TribalAffiliation);
            PrintFormRow(containerTable, "Marital Status", Model.MaritalStatus.Name);
            PrintFormRow(containerTable, "Education Level", Model.EducationLevel.Name);
            PrintFormRow(containerTable, @"Living ""on"" or ""off"" reservation", Model.LivingOnReservation.Name);
            PrintFormRow(containerTable, "County of residence", Model.CountyOfResidence);
            PrintFormRow(containerTable, "Military experience", Model.MilitaryExperience.ToCsv(x => x.Name, ", "));
         
            PrintFormRow(containerTable, string.Empty, string.Empty);

            PrintFormRow(containerTable, "Staff name", Model.BhsStaffNameCompleted);
            PrintFormRow(containerTable, "Complete date", Model.CompleteDate.FormatAsDateWithTime());


            document.Add(containerTable);
        }

        protected override void PrintHeaderLine(Document document)
        {
            float[] headerWidth = new float[] { 0.15f, 0.35f, 0.5f };
            var table = new PdfPTable(headerWidth);
            table.WidthPercentage = 100;
            table.SpacingAfter = 5;
            table.SpacingBefore = 5;


            AddLabelCell(table, "Record No.: {0}".FormatWith(Model.ID));
            AddLabelCell(table, "Created Date: {0}".FormatWith(Model.CreatedDate.FormatAsDateWithTime()));
            AddLabelCell(table, "Branch Location: {0}".FormatWith(Model.LocationLabel));

            document.Add(table);
        }


    }
}

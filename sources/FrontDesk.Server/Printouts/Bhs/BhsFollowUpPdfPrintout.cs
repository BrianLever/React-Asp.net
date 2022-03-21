using System;
using iTextSharp.text;
using iTextSharp.text.pdf;
using FrontDesk.Common.Bhservice;
using FrontDesk.Common.Extensions;

namespace FrontDesk.Server.Printouts.Bhs
{
    public class BhsFollowUpPdfPrintout : BhsPdfPagePrintoutBase
    {
        private readonly BhsFollowUp Model;

        protected override string[] PageTitle => new[] { "Follow-Up Report" };

        #region constructor


        public BhsFollowUpPdfPrintout(BhsFollowUp bhsFollowUp) :
            base()
        {
            if (bhsFollowUp == null)
            {
                throw new ArgumentNullException(nameof(bhsFollowUp));
            }

            ScreeningResult = bhsFollowUp.Result;
            Model = bhsFollowUp;

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

            PrintFormRow(containerTable, "Visit/referral recommendation", Model.VisitRefferalRecommendation);
            PrintFormRow(containerTable, "Scheduled visit date", Model.ScheduledVisitDate.FormatAsDate());
            PrintFormRow(containerTable, "Scheduled follow-up date", Model.ScheduledFollowUpDate.FormatAsDate());
            PrintFormRow(containerTable, "Patient attended visit", Model.PatientAttendedVisit.Name);
            PrintFormRow(containerTable, "Follow-up contact date", Model.FollowUpContactDate.FormatAsDate());
            PrintFormRow(containerTable, "Follow-up contact outcome", Model.FollowUpContactOutcome.Name);

            PrintFormRow(containerTable, "New visit/referral recommendation", Model.NewVisitReferralRecommendation.Name);
            PrintFormRow(containerTable, "New visit/referral recommendation (description)", Model.NewVisitReferralRecommendation.Description);

            PrintFollowUpSchedule(containerTable, Model);

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
            AddLabelCell(table, "Branch Location: {0}".FormatWith(Model.Result.LocationLabel));

            document.Add(table);
        }


    }
}

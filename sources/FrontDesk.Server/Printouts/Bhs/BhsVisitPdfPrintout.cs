using System;
using FrontDesk.Server.Extensions;
using iTextSharp.text;
using iTextSharp.text.pdf;
using FrontDesk.Common.Bhservice;
using FrontDesk.Common.Extensions;
using FrontDesk.Server.Utils;
using System.Collections.Generic;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Screening.Services;
using System.Linq;

namespace FrontDesk.Server.Printouts.Bhs
{
    public class BhsVisitPdfPrintout : BhsPdfPagePrintoutBase
    {
        private readonly BhsVisit Model;
        private readonly IScreeningDefinitionService _screeningInfoService;

        protected override string[] PageTitle => new[] { "Visit Report" };

        #region constructor

        public BhsVisitPdfPrintout(BhsVisit bhsVisit, IScreeningDefinitionService screeningInfoService) :
            base()
        {
            if (bhsVisit == null)
            {
                throw new ArgumentNullException(nameof(bhsVisit));
            }

            _screeningInfoService = screeningInfoService ?? throw new ArgumentNullException(nameof(screeningInfoService));

            ScreeningResult = bhsVisit.Result;
            Model = bhsVisit;

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


            PrintFormRow(containerTable, "Screening Date", Model.ScreeningDate.FormatAsDate());

            PrintFormRow(containerTable, "Tobacco exposure (smoker in the home)", Model.TobacoExposureSmokerInHomeFlag);
            PrintFormRow(containerTable, "Tobacco use (ceremony)", Model.TobacoExposureCeremonyUseFlag);
            PrintFormRow(containerTable, "Tobacco use (smoking)", Model.TobacoExposureSmokingFlag);
            PrintFormRow(containerTable, "Tobacco use (smokeless)", Model.TobacoExposureSmokingFlag);

            PrintFormRow(containerTable, "Alcohol drug use (CAGE)", Model.AlcoholUseFlag);
            PrintFormRow(containerTable, "Non-medical drug use (DAST-10)", Model.SubstanceAbuseFlag);

            PrintDrugOfChoice(containerTable);

            PrintFormRow(containerTable, "Anxiety (GAD-7)", Model.AnxietyFlag);
            PrintFormRow(containerTable, "Depression (PHQ-9)", Model.DepressionFlag);
            PrintFormRow(containerTable, "Suicidal ideation (PHQ-9)", Model.SubstanceAbuseFlag);
            PrintFormRow(containerTable, "Domestic/intimate partner violence (HITS)", Model.PartnerViolenceFlag);
            PrintFormRow(containerTable, "Problem Gambling (BBGS)", Model.ProblemGamblingFlag);

            PrintFormRow(containerTable, string.Empty, string.Empty);

            PrintOtherScreeningTool(containerTable, Model.OtherScreeningTools);

            PrintTreatingActions(containerTable, Model.TreatmentActions);

            PrintFormRow(containerTable, string.Empty, string.Empty);

            PrintTwoColumnScreeningResult(containerTable, "New visit/referral recommendation", Model.NewVisitReferralRecommendation.Name, Model.NewVisitReferralRecommendation.Description);
            PrintFollowUpSchedule(containerTable, Model);

            PrintFormRow(containerTable, string.Empty, string.Empty);

            PrintFormRow(containerTable, "Staff name", Model.BhsStaffNameCompleted);
            PrintFormRow(containerTable, "Complete date", Model.CompleteDate.FormatAsDateWithTime());


            document.Add(containerTable);
        }


        protected void PrintDrugOfChoice(PdfPTable containerTable)
        {
            PrintFormRow(containerTable, "Drug Use", string.Empty);

            var docModel = new DrugOfChoiceModel(Model.Result);
            var drugOfChoiceOptions =
                   _screeningInfoService.Get().FindSectionByID(ScreeningSectionDescriptor.DrugOfChoice);


            //labels
            var answerOptions = drugOfChoiceOptions.FindQuestionByID(DrugOfChoiceDescriptor.SecondaryQuestionId).AnswerOptions;
            var defaultAnswer = answerOptions.First().Text;


            Func<int, string> getAnswerTextFunc = (int value) => answerOptions
                .Where(x => x.Value == value)
                .Select(x => x.Text)
                .FirstOrDefault() ?? defaultAnswer;



            PrintFormRow(containerTable, "Primary", 
                getAnswerTextFunc(docModel.Primary));

            PrintFormRow(containerTable, "Secondary",
                     getAnswerTextFunc(docModel.Secondary));
            PrintFormRow(containerTable, "Tertiary",
                 getAnswerTextFunc(docModel.Tertiary));

            PrintFormRow(containerTable, string.Empty, string.Empty);
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



        private void PrintOtherScreeningTool(PdfPTable table, List<ManualScreeningResultValue> otherScreeningTools)
        {

            PrintTwoColumnScreeningResult(table, string.Empty, "Score or Result", "Name of Tool");
            for (int index = 0; index < 4; index++)
            {
                var item = otherScreeningTools.Count > index ? otherScreeningTools[index] : new ManualScreeningResultValue();

                PrintTwoColumnScreeningResult(table, "Other screening tool {0}".FormatWith(index + 1), item.ScoreOrResult, item.ToolName);
            }

        }

        private void PrintTreatingActions(PdfPTable table, List<TreatmentAction> treatmentActions)
        {

            PrintTwoColumnScreeningResult(table, string.Empty, string.Empty, "Description");
            for (int index = 0; index < 5; index++)
            {
                var item = treatmentActions.Count > index ? treatmentActions[index] : new TreatmentAction();

                PrintTwoColumnScreeningResult(table, "Treatment action {0} (delivered)".FormatWith(index + 1), item.Name, item.Description);
            }

        }




    }
}

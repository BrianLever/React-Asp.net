using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using FrontDesk.Server.Screening;
using FrontDesk.Server.Extensions;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Screening.Services;

namespace FrontDesk.Server
{
    public sealed class BhsIndicatorPdfReport : BhsIndicatorPdfReportBase<IndicatorReportViewModel>
    {
        public override int ValuesCoumnsCount => 6;

        #region constructor

        public BhsIndicatorPdfReport()
                : base()
        {
        }

        public BhsIndicatorPdfReport(SimpleFilterModel filter, bool uniquePatientsReportType)
            : base(filter, uniquePatientsReportType)
        {
            if (filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate = filter.EndDate.Value.AddDays(1);
            }

            Model = ScreeningResultHelper.GetBhsIndicatorReport(ScreeningInfo, filter, UniquePatientsReportType);
        }


        public BhsIndicatorPdfReport(SimpleFilterModel filter,
                                     bool uniquePatientsReportType,
                                     IScreeningResultService screeningResultService,
                                     IScreeningDefinitionService screeningDefinitionService,
                                     IBranchLocationService branchLocationService)
            : base(filter,
                   uniquePatientsReportType,
                   screeningResultService,
                   screeningDefinitionService,
                   branchLocationService)
        {
            if (filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate = filter.EndDate.Value.AddDays(1);
            }

            Model = ScreeningResultHelper.GetBhsIndicatorReport(ScreeningInfo, filter, UniquePatientsReportType);
        }

        #endregion

        /// <summary>
        /// Create PDF report
        /// </summary>
        public override void PrintReport()
        {
            //print header section
            PrintHeader(document, new string[] { "Behavioral Health Indicator Report", "Screening Results by Problem" });
            //Print report settings
            PrintReportSettings(document);

            PrintScoreSection(document, Model.TobaccoSection, 2);
            PrintScoreSection(document, Model.AlcoholSection, 1);
            PrintScoreSection(document, Model.SubstanceAbuseSection, 1);



            var anxietyGad2 = Model.AnxietySectionGad2;
            anxietyGad2.MainQuestions.Clear();
            PrintScoreSection(document, anxietyGad2, 0);

            var anxietyGad7 = Model.AnxietySectionPhq7;
            anxietyGad7.MainQuestions.Clear();
            PrintScoreSection(document, anxietyGad7, 0);



            var depressionPhq2 = Model.DepressionSectionPhq2;
            depressionPhq2.MainQuestions.Clear();
            PrintScoreSection(document, depressionPhq2, 0);

            var depressionPhq9 = Model.DepressionSectionPhq9;
            depressionPhq9.MainQuestions.Clear();
            PrintScoreSection(document, depressionPhq9, 0);

            PrintScoreSection(document, Model.PartnerViolenceSection, 1);

            PrintScoreSection(document, Model.ProblemGamblingSection, 1);

            PrintTotal(document, Model.TotalPatientScreenings);

        }


        private List<IndicatorReportItem> PrepareItemsForRendering(BHIReportSectionViewModel sectionModel)
        {
            var items = sectionModel.Items;
            if (sectionModel.MainQuestions != null && sectionModel.MainQuestions.Any())
            {
                items.InsertRange(0, sectionModel.MainQuestions);
            }

            return items;
        }

        protected override PdfPTable CreateTable()
        {
            return new PdfPTable(new float[] { 0.5f, 0.10f, 0.1f, 0.1f, 0.1f, 0.1f })
            {
                WidthPercentage = 100,
                SpacingBefore = 10,
                KeepTogether = true,
                HeaderRows = 1
            };
        }

        protected override void PrintHeaderOtherCollumns(PdfPTable table)
        {
            PrintHeaderCell(table, Resources.TextMessages.REPORT_INDICATOR_HEADER_TOTAL_POSITIVE, Element.ALIGN_CENTER);
            PrintHeaderCell(table, Resources.TextMessages.REPORT_INDICATOR_HEADER_PERCENT_POSITIVE, Element.ALIGN_CENTER);
            PrintHeaderCell(table, Resources.TextMessages.REPORT_INDICATOR_HEADER_TOTAL_NEGATIVE, Element.ALIGN_CENTER);
            PrintHeaderCell(table, Resources.TextMessages.REPORT_INDICATOR_HEADER_PERCENT_NEGATIVE, Element.ALIGN_CENTER);
            PrintHeaderCell(table, Resources.TextMessages.REPORT_INDICATOR_HEADER_TOTAL, Element.ALIGN_CENTER);
        }

        /// <summary>
        /// print tool section
        /// </summary>
        public void PrintScoreSection(Document document, BHIReportSectionViewModel sectionModel, int? applyPaddingForItemsStartingFromLine = null)
        {
            var table = CreateTable();


            //print header
            PrintHeaderCell(table, sectionModel.Header);
            PrintHeaderOtherCollumns(table);
            var items = PrepareItemsForRendering(sectionModel);
            PrintItems(table, items, applyPaddingForItemsStartingFromLine);
            document.Add(table);

            var questionOnFocus = sectionModel.QuestionOnFocus;
            if (questionOnFocus != null && questionOnFocus.Items.Any())
            {
                table = CreateTable();

                PrintHeaderQuestionOnfocusCell(table, questionOnFocus.Question.PreambleText,
                        questionOnFocus.Question.QuestionText);

                PrintHeaderOtherCollumns(table);

                PrintItems(table, questionOnFocus.Items, 0, 40f);

                document.Add(table);
            }

            if (!String.IsNullOrEmpty(sectionModel.Copyrights))
            {
                PrintCopyRight(sectionModel.Copyrights);
            }
        }

        private void PrintItems(PdfPTable table, IEnumerable<IndicatorReportItem> items, int? applyPaddingForItemsStartingFromLine = null, float additionalLeftPadding = 10f)
        {
            bool normalRow = true;
            var incrementalLineNumber = 0;

            foreach (IndicatorReportItem item in items)
            {
                float firstColumnAdditionLeftPadding = 0;
                if (applyPaddingForItemsStartingFromLine.HasValue &&
                        incrementalLineNumber >= applyPaddingForItemsStartingFromLine.Value)
                {
                    firstColumnAdditionLeftPadding = additionalLeftPadding;
                }


                PrintBodyCellWithP(table, item.ScreeningSectionQuestion.ToString(CultureInfo.InvariantCulture)
                        , item.ScreeningSectionIndicates == null
                                ? ""
                                : item.ScreeningSectionIndicates.ToString(CultureInfo.InvariantCulture)
                        , normalRow, Element.ALIGN_LEFT, firstColumnAdditionLeftPadding);

                PrintBodyCell(table, item.PositiveCount.ToString(CultureInfo.InvariantCulture), normalRow, Element.ALIGN_CENTER);
                PrintBodyCell(table, item.PositivePercent.FormatAsPercentage(), normalRow, Element.ALIGN_CENTER);
                PrintBodyCell(table, item.NegativeCount.ToString(CultureInfo.InvariantCulture), normalRow, Element.ALIGN_CENTER);
                PrintBodyCell(table, item.NegativePercent.FormatAsPercentage(), normalRow, Element.ALIGN_CENTER);
                PrintBodyCell(table, item.TotalCount.ToString(CultureInfo.InvariantCulture), normalRow, Element.ALIGN_CENTER);

                normalRow = !normalRow;


                incrementalLineNumber++;
            }


        }
    }
}

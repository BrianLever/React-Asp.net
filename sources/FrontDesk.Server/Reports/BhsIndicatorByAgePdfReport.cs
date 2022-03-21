using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FrontDesk.Common.Screening;
using FrontDesk.Server.Configuration;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Screening.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace FrontDesk.Server.Reports
{
    public class BhsIndicatorByAgePdfReport : BhsIndicatorPdfReportBase<IndicatorReportByAgeViewModel>
    {
        private readonly IndicatorReportService _indicatorReportService = new IndicatorReportService();

        private readonly IScreeningAgesSettingsProvider _ageGroupsProvider = new ScreeningAgesDbProvider();

        public override int ValuesCoumnsCount
        {
            get
            {
                return (_ageGroupsProvider.AgeGroupsLabels.Length + 1);
            }
        }


        #region constructor
        public BhsIndicatorByAgePdfReport(
            SimpleFilterModel filter,
            bool uniquePatientsReportType)
             :base(filter, uniquePatientsReportType)
        {
            if(filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate = filter.EndDate.Value.AddDays(1);
            }

            Model = _indicatorReportService.GetBhsIndicatorReportByAge(ScreeningInfo, filter, _ageGroupsProvider.AgeGroups, UniquePatientsReportType);

        }

        public BhsIndicatorByAgePdfReport(
            SimpleFilterModel filter,
            bool uniquePatientsReportType,
            IScreeningResultService screeningResultService,
            IScreeningDefinitionService screeningDefinitionService,
            IBranchLocationService branchLocationService,
            IScreeningAgesSettingsProvider ageGroupsProvider
            ) :
            base(filter, uniquePatientsReportType, screeningResultService,
                screeningDefinitionService, branchLocationService)
        {
            if (filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate = filter.EndDate.Value.AddDays(1);
            }

            _ageGroupsProvider = ageGroupsProvider ?? throw new ArgumentNullException(nameof(ageGroupsProvider));

            Model = _indicatorReportService.GetBhsIndicatorReportByAge(ScreeningInfo, filter, _ageGroupsProvider.AgeGroups, UniquePatientsReportType);
        }

        #endregion

        /// <summary>
        /// Create PDF report
        /// </summary>
        public override void PrintReport()
        {
            //print header section
            PrintHeader(document, new string[] { "Behavioral Health Indicator Report", "Screening Results by Age" });
            //Print report settings
            PrintReportSettings(document);

            PrintScoreSection(document, Model.TobaccoSection, 2);
            PrintScoreSection(document, Model.AlcoholSection, 1);
            PrintScoreSection(document, Model.SubstanceAbuseSection, 1);

            // anxiety
            var anxietyGad2 = Model.AnxietySectionGad2;
            anxietyGad2.MainQuestions.Clear();
            PrintScoreSection(document, anxietyGad2, 0);

            var anxietyGad9 = Model.AnxietySectionGad7;
            anxietyGad9.MainQuestions.Clear();
            PrintScoreSection(document, anxietyGad9, 0);



            // depression
            var depressionPhq2 = Model.DepressionSectionPhq2;
            depressionPhq2.MainQuestions.Clear();
            PrintScoreSection(document, depressionPhq2, 0);

            var depressionPhq9 = Model.DepressionSectionPhq9;
            depressionPhq9.MainQuestions.Clear();
            PrintScoreSection(document, depressionPhq9, 0);

            PrintScoreSection(document, Model.PartnerViolenceSection, 1);

            PrintScoreSection(document, Model.ProblemGamblingSection, 1);
        }

        private List<IndicatorReportByAgeItemViewModel> PrepareItemsForRendering(BHIReportSectionByAgeViewModel sectionModel)
        {
            var items = sectionModel.Items;
            if (sectionModel.MainQuestions != null && sectionModel.MainQuestions.Any())
            {
                items.InsertRange(0, sectionModel.MainQuestions);
            }

            return items;
        }

        /// <summary>
        /// print tool section
        /// </summary>
        public void PrintScoreSection(Document document, BHIReportSectionByAgeViewModel sectionModel, int? applyPaddingForItemsStartingFromLine = null)
        {

            var table = CreateTable();

            //print header
            PrintHeaderCell(table, sectionModel.Header);
            PrintHeaderOtherCollumns(table);

            var items = PrepareItemsForRendering(sectionModel);
            //PrintItems(table, items, sectionModel.NumberOfUniquePatients, applyPaddingForItemsStartingFromLine);

            PrintItems(table, items, applyPaddingForItemsStartingFromLine);

            document.Add(table);


            table = CreateTable();
            var questionOnFocus = sectionModel.QuestionOnFocus;
            if (questionOnFocus != null && questionOnFocus.Items.Any())
            {

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

        private void PrintItems(PdfPTable table, List<IndicatorReportByAgeItemViewModel> items, int? applyPaddingForItemsStartingFromLine = null, float additionalLeftPadding = 20f)
        {
            bool normalRow = true;
            var incrementalLineNumber = 0;

            foreach (var item in items)
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

                foreach (var age in item.PositiveScreensByAge)
                {
                    PrintBodyCell(table, age.Value.ToString(CultureInfo.InvariantCulture), normalRow, Element.ALIGN_CENTER);
                }
                //total
                PrintBodyCell(table, item.Total.ToString(CultureInfo.InvariantCulture), normalRow, Element.ALIGN_CENTER);

                normalRow = !normalRow;

                incrementalLineNumber++;
            }
        }

        protected override PdfPTable CreateTable()
        {
            var ageCellCount = ValuesCoumnsCount;
            float ageCellWidth = 0.5F / ageCellCount;

            var tableColumnWidths = new List<float>(ageCellCount + 1);
            tableColumnWidths.Add(0.5f);//question
            tableColumnWidths.AddRange(Enumerable.Repeat<float>(ageCellWidth, ageCellCount));


            return new PdfPTable(tableColumnWidths.ToArray())
            {
                WidthPercentage = 100,
                SpacingBefore = 10,
                KeepTogether = true
            };
        }
        protected override void PrintHeaderOtherCollumns(PdfPTable table)
        {
            var nestedTable = new PdfPTable(
                Enumerable.Repeat<float>(0.083f, ValuesCoumnsCount).ToArray())
            {
                WidthPercentage = 100,
                SpacingBefore = 0
            };

            foreach (var range in _ageGroupsProvider.AgeGroupsLabels)
            {
                PrintHeaderCell(range, Element.ALIGN_CENTER, nestedTable);
            }

            PrintHeaderCell("Total", Element.ALIGN_CENTER, nestedTable);

            var conainerCell = new PdfPCell(nestedTable)
            {
                Colspan = _ageGroupsProvider.AgeGroups.Length + 1 /* adding total column*/,
                Padding = 0
            };
            table.AddCell(conainerCell);
        }
        private PdfPCell PrintHeaderCell(string value, int horizontalAlignment, PdfPTable container, Action<PdfPCell> propertySetter = null)
        {
            cell = new PdfPCell(new Phrase(value, headerBlueFont))
            {
                HorizontalAlignment = horizontalAlignment,
                VerticalAlignment = Rectangle.ALIGN_MIDDLE,
                Border = Element.RECTANGLE | Rectangle.TOP_BORDER,
                BackgroundColor = HeaderBackground,
                Padding = 3
            };

            if (value == Resources.TextMessages.REPORT_INDICATOR_HEADER_PERCENT_POSITIVE || value == Resources.TextMessages.REPORT_INDICATOR_HEADER_PERCENT_NEGATIVE)
            {
                cell.PaddingLeft = 6;
                cell.PaddingRight = 6;
            }

            if (propertySetter != null)
            {
                propertySetter(cell);
            }

            container.AddCell(cell);

            return cell;
        }
    }
}

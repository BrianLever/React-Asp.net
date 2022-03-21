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
    public abstract class BhsPdfReportBase<TModel> : BhsIndicatorPdfReportBase<TModel>
        where TModel: class
    {
        protected readonly IScreeningAgesSettingsProvider _ageGroupsProvider = new ScreeningAgesDbProvider();

        public override int ValuesCoumnsCount
        {
            get
            {
                return (_ageGroupsProvider.AgeGroupsLabels.Length + 1);
            }
        }

        protected override bool RenderVersionInFooter => false;

        #region constructor
        public BhsPdfReportBase(
            SimpleFilterModel filter,
            bool uniquePatientsReportType)
             :base(filter, uniquePatientsReportType)
        {
        }

        public BhsPdfReportBase(
            SimpleFilterModel filter,
            bool uniquePatientsReportType,
            IScreeningResultService screeningResultService,
            IScreeningDefinitionService screeningDefinitionService,
            BranchLocationService branchLocationService
            ) :
            base(filter, uniquePatientsReportType, screeningResultService,
                screeningDefinitionService, branchLocationService)
        {
        }

        #endregion

       

        /// <summary>
        /// print tool section
        /// </summary>
        public void PrintScoreSection(Document document, BhsBHIReportSectionByAgeViewModel sectionModel)
        {

            var table = CreateTable();

            //print header
            PrintHeaderCell(table, sectionModel.Header);
            PrintHeaderOtherCollumns(table);
          
            PrintItems(table, sectionModel.Items);
            document.Add(table);


        }

        private void PrintItems(PdfPTable table, List<BhsIndicatorReportByAgeItemViewModel> items)
        {
            bool normalRow = true;
            var incrementalLineNumber = 0;

            foreach (var item in items)
            {
                PrintBodyCell(table, item.Indicator, normalRow, Element.ALIGN_LEFT);

                foreach (var age in item.TotalByAge)
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
        protected override void PrintHeaderOtherCollumns(PdfPTable parentTable)
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
            parentTable.AddCell(conainerCell);
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

using System;
using System.Collections.Generic;
using System.Linq;

using FrontDesk.Common;
using FrontDesk.Common.Extensions;
using FrontDesk.Common.Resources;
using FrontDesk.Server.Data.BhsVisits;
using FrontDesk.Server.Extensions;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Screening.Services;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace FrontDesk.Server.Printouts.Bhs
{
    public class BhsCheckInBySortPdfPrintout : BhsListPrintoutBase
    {
        private readonly IScreeningResultService _service;
        private readonly ILookupListsDataSource _lookupListsDataSource;
        private readonly IBranchLocationService _branchLocationService;

        public ScreeningResultFilterModel Filter { get; protected set; }
        public List<BhsCheckInItemPrintoutModel> Model { get; protected set; }

        protected override string[] Header => new string[] { "Screening Results by Sort" };

        protected override TableColumnDefinition[] Columns
        {
            get
            {
                return new TableColumnDefinition[] {
                    new TableColumnDefinition{ Name = "Patient Name" },
                    new TableColumnDefinition{Name = "Date of Birth" },
                    new TableColumnDefinition{Name= "Check-In Time", HorizontalAlignment = Element.ALIGN_RIGHT },
                };
            }
        }
        protected override float[] ColumnWidths
        {
            get
            {
                return new float[] { 0.6f, 0.2f, 0.2f };
            }
        }


        public BhsCheckInBySortPdfPrintout(IScreeningResultService visitService, ILookupListsDataSource lookupListsDataSource, 
            ScreeningResultFilterModel filter, IBranchLocationService branchLocationService)
        {
            _service = visitService ?? throw new ArgumentNullException(nameof(visitService));
            _lookupListsDataSource = lookupListsDataSource ?? throw new ArgumentNullException(nameof(lookupListsDataSource));
            _branchLocationService = branchLocationService ?? throw new ArgumentNullException(nameof(branchLocationService));

            Filter = filter;
           
            Model = _service.GetPatientScreeningsBySort(Filter);
        }

        public BhsCheckInBySortPdfPrintout(ScreeningResultFilterModel filter) : this(new ScreeningResultService(), new LookupListsDataSource(), filter, new BranchLocationService())
        {
        }

        protected override void PrintItems(PdfPTable parentTable)
        {
            bool normalRow = true;
            var incrementalLineNumber = 0;
            int rowCounter = 0;
            float additionalLeftPadding = 20f;

            foreach (var item in Model)
            {

                PrintBodyCell(parentTable, item.PatientName, normalRow, Columns[0].HorizontalAlignment);
                PrintBodyCell(parentTable, item.Birthday.FormatAsDate(), normalRow, Columns[1].HorizontalAlignment);
                PrintBodyCell(parentTable, item.LastCreatedDate.FormatAsDateWithTimeWithoutTimeZone(), normalRow, Columns[2].HorizontalAlignment);

                foreach (var reportItem in item.ReportItems)
                {
                    PrintBodyCell(parentTable, "{0} Report ID: {1}".FormatWith(reportItem.ScreeningName, reportItem.ScreeningResultID), true, Columns[0].HorizontalAlignment, additionalLeftPadding);
                    PrintBodyCell(parentTable, string.Empty, true);
                    PrintBodyCell(parentTable, reportItem.CreatedDate.FormatAsDateWithTimeWithoutTimeZone(), true, Columns[2].HorizontalAlignment);

                    incrementalLineNumber++;
                }


                normalRow = !normalRow;

                incrementalLineNumber++;
            }

            if (rowCounter >= 1000)
            {
                PrintThousandRowsLimitMessage(parentTable);
            }

        }


        protected override void PrintReportFilter(Document document)
        {
            float[] headerWidth = new float[] { 0.3f, 0.45f, 0.25f };
            var filterTable = new PdfPTable(headerWidth) { WidthPercentage = 100 };

            AddLabelCell(filterTable, "Report Period");
            AddLabelCell(filterTable, "Problem Sort");
            AddLabelCell(filterTable, "Branch Locations");


            AddValueCell(filterTable, "{0:MM/dd/yyyy} to {1:MM/dd/yyyy}".FormatWith(
                Filter.StartDate != null ? Filter.StartDate : null,
                Filter.EndDate != null ? Filter.EndDate : DateTime.Now)
                );

            AddValueCell(filterTable, String.Join(" \r\nand ", DescribeProblemScopeFilter(Filter.ProblemScoreFilter)));
            AddValueCell(filterTable, Filter.Location != null ? _branchLocationService.Get(Filter.Location.Value).Name : "<< All >>");

            document.Add(filterTable);
        }

        protected string[] DescribeProblemScopeFilter(ScreeningResultByProblemFilter filterModel)
        {
            var result = new List<string>();

            List<LookupValue> drugsOfChoiceOptions = null;


            foreach (var filter in filterModel.Filters)
            {

                if (filter.ScreeningSection == ScreeningResultByProblemFilter.AnySectionName) return new []{ "<< All >>"};

                string sectionFiendlyName = string.Empty;
                string severityLevel = string.Empty;

                if (filter.ScreeningSection == ScreeningSectionDescriptor.Tobacco)
                {
                    sectionFiendlyName = "Tobacco";
                    severityLevel = "Tobacco Exposure";
                }
                else if (filter.ScreeningSection == VisitSettingsDescriptor.TobaccoUseCeremony)
                {
                    sectionFiendlyName = "Tobacco";
                    severityLevel = "Tobacco Use (Ceremony)";
                }
                else if (filter.ScreeningSection == VisitSettingsDescriptor.TobaccoUseSmokeless)
                {
                    sectionFiendlyName = "Tobacco";
                    severityLevel = "Tobacco Use (Smokeless)";
                }
                else if (filter.ScreeningSection == VisitSettingsDescriptor.TobaccoUseSmoking)
                {
                    sectionFiendlyName = "Tobacco";
                    severityLevel = "Tobacco Use (Smoking)";
                }
                else if (filter.ScreeningSection == VisitSettingsDescriptor.Alcohol)
                {
                    sectionFiendlyName = "Alcohol Use (CAGE)";

                    switch (filter.MinScoreLevel)
                    {
                        case 1:
                            severityLevel = "At Risk";
                            break;
                        case 2:
                            severityLevel = "Current Problem";
                            break;
                        case 3:
                            severityLevel = "Dependence";
                            break;
                    }
                }
                else if (filter.ScreeningSection == VisitSettingsDescriptor.SubstanceAbuse)
                {
                    sectionFiendlyName = "Non-Medical Drug Use (DAST-10)";

                    switch (filter.MinScoreLevel)
                    {
                        case 1:
                            severityLevel = "Low";
                            break;
                        case 2:
                            severityLevel = "Moderate";
                            break;
                        case 3:
                            severityLevel = "Substantial";
                            break;
                        case 4:
                            severityLevel = "Severe";
                            break;
                    }
                }
                else if (filter.ScreeningSection == VisitSettingsDescriptor.DrugOfChoice)
                {
                    sectionFiendlyName = "Drug Use";
                    drugsOfChoiceOptions = drugsOfChoiceOptions ?? _lookupListsDataSource.GetDrugOfChoice();

                    severityLevel = drugsOfChoiceOptions.FirstOrDefault(x => x.Id == filter.MinScoreLevel)?.Name;

                }
                else if (filter.ScreeningSection == VisitSettingsDescriptor.Depression)
                {
                    sectionFiendlyName = "Depression (PHQ-9)";

                    switch (filter.MinScoreLevel)
                    {

                        case 2:
                            severityLevel = "Mild";
                            break;
                        case 3:
                            severityLevel = "Moderate";
                            break;
                        case 4:
                            severityLevel = "Moderate-Severe";
                            break;
                        case 5:
                            severityLevel = "Severe";
                            break;
                    }
                }
                else if (filter.ScreeningSection == VisitSettingsDescriptor.DepressionThinkOfDeath)
                {
                    sectionFiendlyName = "Suicidal Ideation (PHQ-9)";

                    switch (filter.MinScoreLevel)
                    {
                        case 1:
                            severityLevel = "Several Days";
                            break;
                        case 2:
                            severityLevel = "More Than Half the Days";
                            break;
                        case 3:
                            severityLevel = "Nearly Every Day";
                            break;
                    }
                }
                else if (filter.ScreeningSection == VisitSettingsDescriptor.PartnerViolence)
                {
                    sectionFiendlyName = "Domestic/Intimate Partner Violence (HITS)";

                    severityLevel = "Current problem";
                }
                else if (filter.ScreeningSection == ScreeningSectionDescriptor.AnxietyAllQuestions)
                {
                    sectionFiendlyName = ScreeningLabels.Screening_Report_Section_GAD7;

                    switch (filter.MinScoreLevel)
                    {

                        case 1:
                            severityLevel = "Mild";
                            break;
                        case 2:
                            severityLevel = "Moderate";
                            break;
                        case 3:
                            severityLevel = "Severe";
                            break;
                    }
                }
                else if (filter.ScreeningSection == VisitSettingsDescriptor.ProblemGambling)
                {
                    sectionFiendlyName = "Problem Gambling (BBGS)";

                    severityLevel = "Evidence of PROBLEM GAMBLING";
                }


                result.Add($"{sectionFiendlyName}: {severityLevel}");
            }

            return result.ToArray();
        }

    }
}

using System;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Screening.Services;

namespace FrontDesk.Server.Reports
{
    public class BhsDemopgraphicsVisitsPdfReport : BhsPdfReportBase<BhsDemographicsReportByAgeViewModel>
    {
        private readonly BhsDemographicsIndicatorReportService _indicatorReportService = new BhsDemographicsIndicatorReportService();

        #region constructor
        public BhsDemopgraphicsVisitsPdfReport(
            SimpleFilterModel filter,
            bool uniquePatientsReportType)
             : base(filter, uniquePatientsReportType)
        {

            if (filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate = filter.EndDate.Value.AddDays(1);
            }

            Model = _indicatorReportService.GetDemographicsReportByAge(filter, _ageGroupsProvider.AgeGroups, UniquePatientsReportType);

        }

        public BhsDemopgraphicsVisitsPdfReport(
            SimpleFilterModel filter,
            bool uniquePatientsReportType,
            IScreeningResultService screeningResultService,
            IScreeningDefinitionService screeningDefinitionService,
            BranchLocationService branchLocationService
            ) :
            base(filter, uniquePatientsReportType, screeningResultService,
                screeningDefinitionService, branchLocationService)
        {
            Model = _indicatorReportService.GetDemographicsReportByAge(filter, _ageGroupsProvider.AgeGroups, UniquePatientsReportType);
        }

        #endregion

        /// <summary>
        /// Create PDF report
        /// </summary>
        public override void PrintReport()
        {
            //print header section
            PrintHeader(document, new string[] { "Patient Demographics Report", "Visits" });
            //Print report settings
            PrintReportSettings(document);

            PrintScoreSection(document, Model.RaceSection);
            PrintScoreSection(document, Model.GenderSection);
            PrintScoreSection(document, Model.SexualOrientationSection);
            PrintScoreSection(document, Model.MaritalStatusSection);
            PrintScoreSection(document, Model.EducationLevelSection);
            PrintScoreSection(document, Model.LeavingOnOrOffReservationSection);
            PrintScoreSection(document, Model.MilitaryExperienceSection);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FrontDesk.Common.Screening;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Screening.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace FrontDesk.Server.Reports
{
    public class BhsFollowUpOutcomesPdfReport : BhsPdfReportBase<BhsFollowUpOutcomesViewModel>
    {
        private readonly BhsFollowUpIndicatorReportService _indicatorReportService = new BhsFollowUpIndicatorReportService();

     
        #region constructor
        public BhsFollowUpOutcomesPdfReport(
            SimpleFilterModel filter,
            bool uniquePatientsReportType)
             :base(filter, uniquePatientsReportType)
        {

            if (filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate = filter.EndDate.Value.AddDays(1);
            }

            Model = _indicatorReportService.GetOutcomesReportByAge(filter, _ageGroupsProvider.AgeGroups, UniquePatientsReportType);

        }

        public BhsFollowUpOutcomesPdfReport(
            SimpleFilterModel filter,
            bool uniquePatientsReportType,
            IScreeningResultService screeningResultService,
            IScreeningDefinitionService screeningDefinitionService,
            BranchLocationService branchLocationService
            ) :
            base(filter, uniquePatientsReportType, screeningResultService,
                screeningDefinitionService, branchLocationService)
        {
            if (filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate = filter.EndDate.Value.AddDays(1);
            }

            Model = _indicatorReportService.GetOutcomesReportByAge(filter, _ageGroupsProvider.AgeGroups, UniquePatientsReportType);
        }

        #endregion

        /// <summary>
        /// Create PDF report
        /// </summary>
        public override void PrintReport()
        {
            //print header section
            PrintHeader(document, new string[] { "Follow-Up Outcomes" });
            //Print report settings
            PrintReportSettings(document);

            PrintScoreSection(document, Model.PatientAttendedVisit);
            PrintScoreSection(document, Model.FollowUpContactOutcome);
            PrintScoreSection(document, Model.NewVisitReferralRecommendation);
            PrintScoreSection(document, Model.NewVisitReferralRecommendationAccepted);
            PrintScoreSection(document, Model.ReasonNewVisitReferralRecommendationNotAccepted);
            PrintScoreSection(document, Model.Discharged);
            PrintScoreSection(document, Model.FollowUps);

        }
    }
}

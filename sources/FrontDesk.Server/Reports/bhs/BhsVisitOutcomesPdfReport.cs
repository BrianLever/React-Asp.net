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
    public class BhsVisitOutcomesPdfReport : BhsPdfReportBase<BhsVisitOutcomesViewModel>
    {
        private readonly BhsVisitIndicatorReportService _indicatorReportService = new BhsVisitIndicatorReportService();

        #region constructor
        public BhsVisitOutcomesPdfReport(
            SimpleFilterModel filter,
            bool uniquePatientsReportType)
             :base(filter, uniquePatientsReportType)
        {
            if (filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate = filter.EndDate.Value.AddDays(1);
            }

            Model = _indicatorReportService.GetTreatmentOutcomesReportByAge(filter, _ageGroupsProvider.AgeGroups, UniquePatientsReportType);

        }

        public BhsVisitOutcomesPdfReport(
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

            Model = _indicatorReportService.GetTreatmentOutcomesReportByAge(filter, _ageGroupsProvider.AgeGroups, UniquePatientsReportType);
        }

        #endregion

        /// <summary>
        /// Create PDF report
        /// </summary>
        public override void PrintReport()
        {
            //print header section
            PrintHeader(document, new string[] { "Visit Outcomes" });
            //Print report settings
            PrintReportSettings(document);

            PrintScoreSection(document, Model.TreatmentAction1);
            PrintScoreSection(document, Model.TreatmentAction2);
            PrintScoreSection(document, Model.TreatmentAction3);
            PrintScoreSection(document, Model.TreatmentAction4);
            PrintScoreSection(document, Model.TreatmentAction5);
            PrintScoreSection(document, Model.NewVisitReferralRecommendation);
            PrintScoreSection(document, Model.NewVisitReferralRecommendationAccepted);
            PrintScoreSection(document, Model.ReasonNewVisitReferralRecommendationNotAccepted);
            PrintScoreSection(document, Model.Discharged);
            PrintScoreSection(document, Model.FollowUps);

        }

    }
}

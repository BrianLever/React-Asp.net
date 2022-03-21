using Common.Logging;

using FrontDesk.Common.Screening.Bhs;
using FrontDesk.Server.Data.BhsVisits;
using FrontDesk.Server.Screening.Mappers;
using FrontDesk.Server.Screening.Models;

using System;
using System.Linq;

namespace FrontDesk.Server.Screening.Services
{
    public class BhsVisitIndicatorReportService
    {
        private readonly IBhsVisitReportRepository _bhsVisitRepository;

        private readonly ILog _logger = LogManager.GetLogger<BhsVisitIndicatorReportService>();

        public BhsVisitIndicatorReportService() : this(new BhsVisitReportDb())
        {
        }

        public BhsVisitIndicatorReportService(IBhsVisitReportRepository bhsVisitRepository)
        {

            _bhsVisitRepository = bhsVisitRepository ?? throw new ArgumentNullException(nameof(bhsVisitRepository));
        }

        public BhsVisitOutcomesViewModel GetTreatmentOutcomesReportByAge(SimpleFilterModel filter, int[] ageGroups, bool renderUniquePatientsReportType)
        {

            var db = _bhsVisitRepository;
            var model = new BhsVisitOutcomesViewModel();
            db.Connect();
            try
            {
                db.BeginTransaction();
                db.StartConnectionSharing();
                model.Items.AddRange(
                (renderUniquePatientsReportType ?
                    db.GetTreatmentActionsUniquePatientReportByAge(filter.Location, filter.StartDate, filter.EndDate) :
                    db.GetTreatmentActionsReportByAge(filter.Location, filter.StartDate, filter.EndDate))
                .ToViewModel(ageGroups));

                var entries = new[]
                               {
                    nameof(BhsVisitReportDescriptor.NewVisitReferralRecommendation),
                    nameof(BhsVisitReportDescriptor.NewVisitReferralRecommendationAccepted),
                    nameof(BhsVisitReportDescriptor.ReasonNewVisitReferralRecommendationNotAccepted),
                    nameof(BhsVisitReportDescriptor.Discharged)
                };

                foreach (var item in entries)
                {
                    model.Items.AddRange(
                    (renderUniquePatientsReportType ?
                        db.GetUniquePatientReportByAge(item, filter.Location, filter.StartDate, filter.EndDate) :
                        db.GetPatientReportByAge(item, filter.Location, filter.StartDate, filter.EndDate))
                    .ToViewModel(ageGroups));

                }

                model.Items.AddRange(db.GetFollowUpPatientReportByAge(renderUniquePatientsReportType, filter.Location, filter.StartDate, filter.EndDate).ToViewModel(ageGroups));

                db.StopConnectionSharing();
                db.CommitTransaction();


                //remove None from "TreatmentAction1"

                var noneInTreatmentAction1 = model.Items.FirstOrDefault(i => i.CategoryID == "TreatmentAction1" && i.Indicator == "None");
                if(noneInTreatmentAction1 != null)
                {
                    model.Items.Remove(noneInTreatmentAction1);
                }
            }
            catch (Exception ex)
            {
                db.StopConnectionSharing();
                db.RollbackTransaction();
                throw new Exception("Failed to create BHI report", ex);
            }
            finally
            {
                db.Disconnect();
            }

            return model;

        }

    }
}


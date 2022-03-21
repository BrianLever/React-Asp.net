using System;
using FrontDesk.Server.Data.BhsVisits;
using Common.Logging;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Screening.Mappers;
using FrontDesk.Common.Screening.Bhs;

namespace FrontDesk.Server.Screening.Services
{
    public class BhsFollowUpIndicatorReportService
    {
        private readonly IBhsFollowUpReportRepository _bhsRepository;

        private readonly ILog _logger = LogManager.GetLogger<BhsFollowUpIndicatorReportService>();

        public BhsFollowUpIndicatorReportService() : this(new BhsFollowUpReportDb())
        {
        }

        public BhsFollowUpIndicatorReportService(IBhsFollowUpReportRepository bhsRepository)
        {

            _bhsRepository = bhsRepository ?? throw new ArgumentNullException(nameof(bhsRepository));
        }

        public BhsFollowUpOutcomesViewModel GetOutcomesReportByAge(SimpleFilterModel filter, int[] ageGroups, bool renderUniquePatientsReportType)
        {

            var db = _bhsRepository;
            var model = new BhsFollowUpOutcomesViewModel();
            db.Connect();
            try
            {
                db.BeginTransaction();
                db.StartConnectionSharing();


                var entries = new[]
                               {
                    nameof(BhsFollowUpReportDescriptor.PatientAttendedVisit),
                    nameof(BhsFollowUpReportDescriptor.FollowUpContactOutcome),
                    nameof(BhsFollowUpReportDescriptor.NewVisitReferralRecommendation),
                    nameof(BhsFollowUpReportDescriptor.NewVisitReferralRecommendationAccepted),
                    nameof(BhsFollowUpReportDescriptor.ReasonNewVisitReferralRecommendationNotAccepted),
                    nameof(BhsFollowUpReportDescriptor.Discharged)
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


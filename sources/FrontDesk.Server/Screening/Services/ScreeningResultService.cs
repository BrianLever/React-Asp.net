using Common.Logging;

using FrontDesk.Server.Data;
using ScreenDox.Server.Common.Infrastructure;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Screening.Mappers;
using FrontDesk.Server.Screening.Models;

using RPMS.Common.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using ScreenDox.Server.Models.ViewModels;
using ScreenDox.Server.Models;

namespace FrontDesk.Server.Screening.Services
{
    public interface IScreeningResultService
    {
        DateTimeOffset? GetMinDate();
        List<BhsCheckInItemPrintoutModel> GetPatientScreeningsBySort(ScreeningResultFilterModel filter);
        ScreeningsByScoreLevelCountResult GetScreeningsCountByScoreLevel(SimpleFilterModel filter);
        ScreeningResult Get(long screeningResultID);
        void UpdateExportInfo(ScreeningResult screeningResult, ExportOperationStatus exportStatus, Patient patient, Visit visit, int userIdentityID);
        List<PatientCheckInViewModel> GetRelatedPatientScreeningsByProblemSort(long mainRowID, ScreeningResultFilterModel searchOptions);
        long InsertScreeningResult(ScreeningResult result);
        long? GetLatestForKiosk(short kioskID, ScreeningPatientIdentity result);
        long GetTotalRecordCount();
        void Update(ScreeningResult patient);
    }


    public class ScreeningResultService : IScreeningResultService
    {
        private static ILog _logger = LogManager.GetLogger("ScreeningResultService");
        private readonly IScreenerResultRepository _repository;
        private readonly DataCacheService<ScreeningsByScoreLevelCountResult> _cache = new DataCacheService<ScreeningsByScoreLevelCountResult>();
        private readonly DataCacheService<List<PatientCheckInViewModel>> _cacheScreenings = new DataCacheService<List<PatientCheckInViewModel>>();


        public ScreeningResultService() : this(new ScreeningResultDb())
        {

        }

        public ScreeningResultService(IScreenerResultRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public ScreeningResult Get(long screeningResultID)
        {
            return ScreeningResultHelper.GetScreeningResult(screeningResultID);
        }

        public long? GetLatestForKiosk(short kioskID, ScreeningPatientIdentity result)
        {
            return _repository.GetLatestForKiosk(kioskID, result);
        }

        public DateTimeOffset? GetMinDate()
        {
            return ScreeningResultHelper.GetMinDate();
        }


        public List<BhsCheckInItemPrintoutModel> GetPatientScreeningsBySort(ScreeningResultFilterModel filter)
        {
            if (filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate.Value.Date.AddDays(1); // we use "Less than" operator, so we add one day
            }
            if (filter.StartDate.HasValue)
            {
                filter.StartDate = filter.StartDate.Value.Date;
            }

            _logger.Trace("[ScreeningResultService][GetPatientScreeningsBySort] Method called.");
            List<PatientCheckInDtoModel> items = _repository.GetPatientScreeningsBySort(filter);
            var result = items.ToViewModel().ToList();
            return result;
        }

        public List<PatientCheckInViewModel> GetRelatedPatientScreeningsByProblemSort(long mainRowID, ScreeningResultFilterModel filter)
        {
            var cache = _cacheScreenings;
            if (filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate.Value.Date.AddDays(1); // we use "Less than" operator, so we add one day
            }

            var key = mainRowID + "^" + filter.ToString();
            //add caching
            var result = cache.Get(key);
            if (result == null)
            {
                _logger.Trace("[ScreeningResultService][GetRelatedPatientScreeningsByProblemSort] Cache is empty. Updating.");

                result = _repository.GetRelatedPatientScreeningsByProblemSort(mainRowID, filter);
                cache.Add(key, result);

                _logger.Trace("[ScreeningResultService][GetRelatedPatientScreeningsByProblemSort] Cache updated.");
            }
            return result;

        }

        public ScreeningsByScoreLevelCountResult GetScreeningsCountByScoreLevel(SimpleFilterModel filter)
        {
            if (filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate.Value.Date.AddDays(1); // we use "Less than" operator, so we add one day
            }

            if (filter.StartDate.HasValue)
            {
                filter.StartDate = filter.StartDate.Value.Date;
            }


            var key = filter.ToString();

            //add caching
            var result = _cache.Get(key);

            if (result == null)
            {

                _logger.Trace("[ScreeningResultService][GetScreeningsCountByScoreLevel] Cache is empty. Updating.");

                result = _repository.GetScreeningsCountByScoreLevel(filter);

                _cache.Add(key, result);

                _logger.Trace("[ScreeningResultService][GetScreeningsCountByScoreLevel] Cache updated.");
            }

            return result;
        }

        public long GetTotalRecordCount()
        {
            return _repository.GetTotalCount();
        }

        public long InsertScreeningResult(ScreeningResult result)
        {
            return ScreeningResultHelper.InsertScreeningResult(result);
        }

        public void Update(ScreeningResult patient)
        {
                _repository.Update(patient);
            
        }

        public void UpdateExportInfo(ScreeningResult screeningResult, ExportOperationStatus exportStatus, Patient patient, Visit visit, int userIdentityID)
        {
            screeningResult.ExportDate = DateTimeOffset.Now;
            screeningResult.ExportedBy = userIdentityID;
            screeningResult.ExportedToHRN = patient.EHR;
            screeningResult.ExportedToPatientID = patient.ID;
            screeningResult.ExportedToVisitID = visit.ID;
            screeningResult.ExportedToVisitDate = visit.Date;
            screeningResult.ExportedToVisitLocation = visit.Location.Name;


            SecurityLog.Add(new SecurityLog(SecurityEvents.Export,
                   String.Format("#{0}, {1} => EHR:{2}, Visit:{3:MM'/'dd'/'yyyy' 'HH':'mm}, {4} | Status: {5}",
                       screeningResult.ID,
                       screeningResult.FullName,
                       screeningResult.ExportedToHRN,
                       screeningResult.ExportedToVisitDate,
                       screeningResult.ExportedToVisitLocation,
                       exportStatus
                       ),
                   screeningResult.LocationID),
                   userIdentityID);

            // update patient's name and DOB if changed during the export
            if (exportStatus == ExportOperationStatus.AllSucceed || exportStatus == ExportOperationStatus.SomeOperationsFailed)
            {
                _repository.UpdateExportInfo(screeningResult);

                // updates name (skip address as we clean it up above)
                _repository.UpdatePatientInfo(screeningResult);
            }

        }
    }

}

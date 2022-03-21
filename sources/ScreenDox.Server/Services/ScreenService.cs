using Common.Logging;

using FrontDesk;
using FrontDesk.Server.Data;
using FrontDesk.Server.Data.BhsVisits;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Messages;
using FrontDesk.Server.Screening;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Screening.Services;

using ScreenDox.Server.Data;
using ScreenDox.Server.Logging;
using ScreenDox.Server.Models;
using ScreenDox.Server.Models.Factory;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScreenDox.Server.Services
{
    public class ScreenService : IScreenService
    {
        private readonly ILog _logger = LogManager.GetLogger<ScreenService>();
        private readonly IScreensRepository _repository;
        private readonly IScreenerResultReadRepository _screenerResultReadRepository;
        private readonly IScreeningDefinitionService _screeningDefinitionService;
        private readonly IUserPrincipalRepository _userPrincipalRepository;
        private readonly IBhsVisitRepository _visitRepository;
        private readonly IBhsDemographicsService _demographicsService;
        private readonly ISecurityLogService _securityLogService;


        public ScreenService(IScreensRepository repository,
            IScreenerResultReadRepository screenerResultReadRepository,
            IUserPrincipalRepository userPrincipalRepository,
            IBhsVisitRepository visitRepository,
            IScreeningDefinitionService screeningDefinitionService,
            IBhsDemographicsService demographicsService,
            ISecurityLogService securityLogService
        )
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _screenerResultReadRepository = screenerResultReadRepository ?? throw new ArgumentNullException(nameof(screenerResultReadRepository));
            _screeningDefinitionService = screeningDefinitionService ?? throw new ArgumentNullException(nameof(screeningDefinitionService));
            _demographicsService = demographicsService ?? throw new ArgumentNullException(nameof(demographicsService));
            _userPrincipalRepository = userPrincipalRepository ?? throw new ArgumentNullException(nameof(userPrincipalRepository));
            _visitRepository = visitRepository ?? throw new ArgumentNullException(nameof(visitRepository));
            _securityLogService = securityLogService ?? throw new ArgumentNullException(nameof(securityLogService));
        }

        /// <summary>
        /// Get unique patient records that has screenings in given period of time
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public SearchResponse<UniquePatientScreenViewModel> GetUniquePatientScreens(PagedScreeningResultFilterModel filter)
        {
            var result = new SearchResponse<UniquePatientScreenViewModel>();

            if (filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate.Value.Date.AddDays(1); // we use "Less than" operator, so we add one day
            }
            _logger.Trace("[ScreenService][GetUniquePatientScreens] Method called.");

            result.TotalCount = _repository.GetLatestCheckinsForDisplayCount(filter);

            result.Items = _repository.GetLatestCheckinsForDisplay(filter);

            return result;
        }

        /// <summary>
        /// Get patient's screenings with EHR export status
        /// </summary>
        /// <param name="mainRowID">Screendox ID of the Screening Result which is used to get patient identity.</param>
        /// <param name="filter">Filter condition.</param>
        /// <returns>List of patient's screenings</returns>
        public List<PatientCheckInViewModel> GetRelatedPatientScreenings(long mainRowID, ScreeningResultFilterModel filter)
        {
            if (filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate.Value.Date.AddDays(1); // we use "Less than" operator, so we add one day
            }

            _logger.Trace("[ScreeningResult][GetRelatedPatientScreenings] Method called.");
            List<PatientCheckInViewModel> result = _repository.GetRelatedPatientScreenings(mainRowID, filter);

            return result;
        }

        /// <summary>
        /// Returns the date of the first screening in the database
        /// </summary>
        /// <returns>Date time with offset</returns>
        public DateTimeOffset? GetMinDate()
        {
            return _repository.GetMinDate();
        }

        /// <summary>
        /// Get screening result view model for UI
        /// </summary>
        public ScreeningResultResponse GetScreeningResultView(long screeningResultID)
        {
            var screeningResult = _screenerResultReadRepository.GetScreeningResult(screeningResultID);

            if (screeningResult == null) return null;

            var info = _screeningDefinitionService.Get();

            var result = ScreeningResultResponseFactory.Create(screeningResult, info);

            if (result == null) return null;

            // add additional information
            result.ExportedByFullName = screeningResult.ExportedBy.HasValue ?
                _userPrincipalRepository.GetUserByID(screeningResult.ExportedBy.Value)?.FullName :
                string.Empty;

            Parallel.Invoke(
                () =>
                {
                    // add bhs visit status
                    var visitId = _visitRepository.FindByScreeningResultId(result.ID);
                    result.BhsVisitStatus = visitId.HasValue ? "Open Visit" : "Create Visit";
                    result.BhsVisitID = visitId;
                },  // close visits

                () =>
                {
                    // add patient demographics ID
                    result.PatientDemographicsID = _demographicsService.Find(result.PatientInfo);
                } //patient demographics
            );

            return result;
        }


        /// <summary>
        /// Get screening result object
        /// </summary>
        public ScreeningResult GetScreeningResult(long screeningResultID)
        {
            var result = _screenerResultReadRepository.GetScreeningResult(screeningResultID);

            if (result == null) return null;

            _securityLogService.Add(new SecurityLog(SecurityEvents.ViewBHR, result.ID, result.LocationID));


            return result;
        }

        /// <summary>
        /// Delete screening result if no completed visit
        /// </summary>
        /// <param name="id"></param>
        public bool Delete(long id)
        {

            var result = _screenerResultReadRepository.GetScreeningResult(id);

            if (result == null) return false; // object not found

            //check that there is no completed visit
            var visitId = _visitRepository.FindByScreeningResultId(id);
            if (visitId.HasValue)
            {
                var visit = _visitRepository.Get(visitId.Value);

                if (visit != null && visit.IsCompleted)
                {
                    _logger.Warn(TextStrings.SCREENING_RESULT_DELETEFAIL_VISIT_EXISTS.FormatWith(id));

                    throw new ApplicationException(TextStrings.SCREENING_RESULT_DELETEFAIL_VISIT_EXISTS.FormatWith(id));
                }

            }
            if (visitId.HasValue)
            {
                // Bhs visit has not been completed, so we can remove it
                _visitRepository.Delete(visitId.Value);
            }

            _repository.Delete(id);

            _securityLogService.Add(new SecurityLog(SecurityEvents.BHRDeleted,
                          String.Format("{0}~{1}", result.FullName, result.Birthday),
                          result.LocationID));

            return true;

        }
    }
}

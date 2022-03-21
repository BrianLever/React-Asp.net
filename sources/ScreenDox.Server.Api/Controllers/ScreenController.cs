using CacheCow.Server.WebApi;

using Common.Logging;

using FrontDesk;
using FrontDesk.Common.InfrastructureServices;
using FrontDesk.Common.Messages;
using FrontDesk.Server;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Reports;
using FrontDesk.Server.Screening;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Screening.Services;

using ScreenDox.Server.Api.Infrastructure;
using ScreenDox.Server.Api.Models;
using ScreenDox.Server.Formatters;
using ScreenDox.Server.Logging;
using ScreenDox.Server.Models;
using ScreenDox.Server.Security;
using ScreenDox.Server.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace ScreenDox.Server.Api.Controllers
{
    /// <summary>
    /// Patient Screenings API
    /// </summary>
    public class ScreenController : ApiController
    {

        private readonly IScreenService _screenService;

        private readonly IScreeningDefinitionService _screeningInfoService;
        private readonly ITimeService _timeService;
        private readonly IVisitService _visitService;
        private readonly TodayDateFormatter _todayDateFormatter;
        private readonly IUserPrincipalService _userService;
        private readonly ISecurityLogService _securityLogService;

        private readonly ILog _logger = LogManager.GetLogger<ScreenController>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service"></param>
        /// <param name="timeService"></param>
        /// <param name="screeningInfoService"></param>
        /// <param name="visitService"></param>
        /// <param name="todayDateFormatter"></param>
        /// <param name="securityLogService"></param>
        /// <param name="userService"></param>
        public ScreenController(IScreenService service,
                                ITimeService timeService,
                                IScreeningDefinitionService screeningInfoService,
                                IVisitService visitService,
                                TodayDateFormatter todayDateFormatter,
                                ISecurityLogService securityLogService,
                                IUserPrincipalService userService)
        {
            this._screenService = service ?? throw new ArgumentNullException(nameof(service));
            this._timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
            _screeningInfoService = screeningInfoService ?? throw new ArgumentNullException(nameof(screeningInfoService));
            _visitService = visitService ?? throw new ArgumentNullException(nameof(visitService));
            _todayDateFormatter = todayDateFormatter ?? throw new ArgumentNullException(nameof(todayDateFormatter));
            _securityLogService = securityLogService ?? throw new ArgumentNullException(nameof(securityLogService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        /// <summary>
        /// Get items for Patient Screens result grid
        /// </summary>
        /// <param name="filter">Filter with paging.
        /// Order by options:
        ///     - LastCheckinDate DESC (default)
        ///     - FullName ASC
        ///     - Birthday
        ///     - ExportDate DESC
        /// </param>
        /// <returns>Paged list of patient that had screenings in given filter conditions.</returns>
        [HttpPost]
        [Route("api/screen/search")]
        public SearchResponse<UniquePatientScreenViewModel> GetUniquePatientScreens([FromBody] PagedScreeningResultFilterModel filter)
        {

            filter = filter ?? new PagedScreeningResultFilterModel();

            Guard.ValidateOrderByClause(filter, new[] { "LastCheckinDate", "FullName", "Birthday", "ExportDate" });

            var result = _screenService.GetUniquePatientScreens(filter);

            result.ShouldNotBeNull();

            result.Items.ForEach(x =>
            {
                x.LastCheckinDateLabel = _todayDateFormatter.Format(x.LastCheckinDate);
                x.ExportDateLabel = _todayDateFormatter.Format(x.ExportDate);
            });

            return result;
        }



        /// <summary>
        /// Get related patient screenings with export status by screen id
        /// </summary>
        /// <param name="id">Screendox Id.</param>
        /// <param name="filter">Filter conditions.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/screen/search/{id}")]
        public List<PatientCheckInViewModel> GetRelatedPatientScreenings(
            [FromUri] long id,
            [FromBody] ScreeningResultFilterModel filter
        )
        {
            filter = filter ?? new PagedScreeningResultFilterModel();

            id.ShouldNotBeDefault();

            var result = _screenService.GetRelatedPatientScreenings(id, filter);

            result.ForEach(x =>
            {
                x.CreatedDateLabel = _todayDateFormatter.Format(x.CreatedDate);
                x.ExportDateLabel = _todayDateFormatter.Format(x.ExportDate);
            });

            return result;
        }


        /// <summary>
        /// Get items for the GPRA period filter.
        /// </summary>
        /// <returns>List of available GPRA periods for the filter.</returns>
        [HttpCache(DefaultExpirySeconds = 86400)] // 24 hours
        [Route("api/screen/gpra")]
        [HttpGet]
        public List<GPRAReportingTime> GetGpraReportingTime()
        {
            return GPRAReportingTime.GetPeriodsSince((_screenService.GetMinDate() ?? _timeService.GetDateTimeOffsetNow()).LocalDateTime);
        }

        /// <summary>
        /// Get the time of the oldest screening in the database
        /// </summary>
        /// <returns>Date time with time zone.</returns>
        [HttpCache(DefaultExpirySeconds = 86400)] // 24 hours
        [Route("api/screen/mindate")]
        [HttpGet]
        public DateTimeOffset GetMinDate()
        {
            return _screenService.GetMinDate() ?? _timeService.GetDateTimeOffsetNow();
        }


        /// <summary>
        /// Get screening result by id.
        /// </summary>
        /// <param name="id">Screendox Id.</param>
        /// <returns>Screening Result details.</returns>
        [HttpCache(DefaultExpirySeconds = 0)] // cache disabled
        [Route("api/screen/{id}")]
        [HttpGet]
        public ScreeningResultResponse Get(long id)
        {
            var result = _screenService.GetScreeningResultView(id);

            result.ShouldNotBeNull();

            var principle = _userService.GetCurrent();

            _securityLogService.Add(new SecurityLog(SecurityEvents.ViewBHR, principle.UserID, result.LocationID));

            return result;
        }

        /// <summary>
        /// Get pdf version of Screening Report.
        /// </summary>
        /// <param name="id">Screendox Id.</param>
        /// <returns>Pdf file with screening result details.</returns>
        [HttpCache(DefaultExpirySeconds = 0)] // cache disabled
        [Route("api/screen/{id}/print")]
        [HttpGet, HttpPost]
        public HttpResponseMessage Print(long id)
        {
            var report = _screenService.GetScreeningResult(id);

            if (report == null)
            {
                ResponseDataFactory.ThrowNotFound(id);
            }

            //Print report into context
            BhsPdfReport BhsPdfReport = new BhsPdfReport(report);

            var response = Request.CreateResponse(HttpStatusCode.OK);

            BhsPdfReport.CreatePDF(
                response, "{0}_Report_{1}.pdf".FormatWith(report.ScreeningID, report.ID));

            return response;
        }

        /// <summary>
        /// Delete Screening Report that does not have completed Visits.
        /// </summary>
        /// <param name="id">Screendox Id.</param>
        /// <returns>Pdf file with screening result details.</returns>
        [Route("api/screen/{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(long id)
        {
            id.ShouldNotBeDefault();

            bool result = false;

            try
            {
                result = _screenService.Delete(id);
            }
            catch (ApplicationException ex)
            {
                _logger.WarnFormat("[Screen Api] Validation error while deletion. Message: {0}.", ex, ex.Message);

                ResponseDataFactory.ThrowBadRequestMessage("Invalid operation", new[] { ex.Message });
            }

            return result ? Ok(CustomMessage.GetDeleteMessage("Screen")) : (IHttpActionResult)NotFound();
        }

        /// <summary>
        /// Get screening sections and questions defition information.
        /// </summary>
        /// <returns>Screening section questions meta information.</returns>
        [HttpCache(DefaultExpirySeconds = 300)] // 5 minutes
        [Route("api/screen/definition")]
        [HttpGet]
        public List<ScreeningSection> GetScreeningInfo()
        {
            return _screeningInfoService.GetSections()
                .Where(x =>
                x.ScreeningSectionID != ScreeningSectionDescriptor.AnxietyAllQuestions &&
                x.ScreeningSectionID != ScreeningSectionDescriptor.DepressionAllQuestions
                ).ToList();
        }



        /// <summary>
        /// Create visit record from screening result
        /// </summary>
        /// <param name="id">Screendox Id (Screening Result ID).</param>
        /// <returns>Returns ID of the created visit. If visit already exists, it returns it's Id.</returns>
        [HttpPost]
        [Route("api/screen/{id}/visit")]
        public IHttpActionResult CreateVisit([FromUri] long id)
        {
            id.ShouldNotBeDefault();

            var visitId = _visitService.FindByScreeningResultId(id);

            if (!visitId.HasValue)
            {
                _logger.InfoFormat("Creating new Visit from Screen Details page.");

                var visit = _visitService.Create(id);
                visitId = visit != null ? visit.ID : (long?)null;

                _logger.InfoFormat("Visit created from Screen Details page. Screening result id: {0}. Visit Id: {1}", id, visitId.Value);

            }
            else
            {
                _logger.WarnFormat("Visit aleady exists. Screening result id: {0}. Visit Id: {1}", id, visitId);

            }

            return Ok(visitId ?? 0);
        }


        
    }
}

using AutoMapper;

using CacheCow.Server.WebApi;

using Common.Logging;

using FrontDesk;
using FrontDesk.Common.Bhservice;
using FrontDesk.Common.Extensions;
using FrontDesk.Common.InfrastructureServices;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Printouts.Bhs;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Screening.Services;

using ScreenDox.Server.Api.Infrastructure;
using ScreenDox.Server.Api.Models;
using ScreenDox.Server.Formatters;
using ScreenDox.Server.Logging;
using ScreenDox.Server.Models;
using ScreenDox.Server.Models.ColumbiaReports;
using ScreenDox.Server.Models.Factory;
using ScreenDox.Server.Models.ViewModels;
using ScreenDox.Server.Models.ViewModels.Validators;
using ScreenDox.Server.Security;
using ScreenDox.Server.Services;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ScreenDox.Server.Api.Controllers
{
    /// <summary>
    /// Columbia SSRS resource
    /// </summary>
    [Authorize]
    public class ColumbiaSuicideController : ApiController
    {
        private readonly IVisitService _visitService;
        private readonly IColumbiaSuicideReportService _service;
        private readonly IUserPrincipalService _userService;
        private readonly ISecurityLogService _securityLogService;
        private readonly TodayDateFormatter _todayDateFormatter;
        private readonly ITimeService _timeService;

        private readonly ILog _logger = LogManager.GetLogger<ColumbiaSuicideController>();

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="service"></param>
        /// <param name="visitService"></param>
        /// <param name="userService"></param>
        /// <param name="securityLogService"></param>
        /// <param name="todayDateFormatter"></param>
        /// <param name="timeService"></param>
        public ColumbiaSuicideController(IColumbiaSuicideReportService service,
                                         IVisitService visitService,
                                         IUserPrincipalService userService,
                                         ISecurityLogService securityLogService,
                                         TodayDateFormatter todayDateFormatter,
                                         ITimeService timeService)
        {
            _visitService = visitService ?? throw new ArgumentNullException(nameof(visitService));
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _securityLogService = securityLogService ?? throw new ArgumentNullException(nameof(securityLogService));
            _todayDateFormatter = todayDateFormatter ?? throw new ArgumentNullException(nameof(todayDateFormatter));
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
        }


        /// <summary>
        /// Get items for Columbia Suicide  result grid - Patient info grouped by Patient Info
        /// </summary>
        /// <param name="filter">Filter with paging.
        /// Order by options:
        ///     - LastCreatedDate DESC (default)
        ///     - FullName ASC
        ///     - Birthday
        /// </param>
        /// <returns>Paged list of visits with given filter conditions.</returns>
        [HttpCache(DefaultExpirySeconds = 0)] // No cache
        [HttpPost]
        [Route("api/columbiasuicide/search")]
        public SearchResponse<UniqueColumbiaReportViewModel> GetReports([FromBody] PagedFilterModel filter)
        {
            filter = filter ?? new PagedFilterModel();

            Guard.ValidateOrderByClause(filter, new[] { "LastCreatedDate", "FullName", "Birthday" });

            var result = _service.GetUniqueReports(filter);

            result.Items.ForEach(x =>
           {
               x.LastCreatedDateLabel = _todayDateFormatter.Format(x.LastCreatedDate);
               x.LastCompleteDateLabel = _todayDateFormatter.Format(x.LastCompleteDate);
           });

            return result;
        }


        /// <summary>
        /// Get related columbia reports filtered by patient group.
        /// </summary>
        /// <param name="id">Columbia Report Id that is the source of the patient info.</param>
        /// <param name="filter">Filter conditions.</param>
        /// <returns></returns>
        [HttpCache(DefaultExpirySeconds = 0)] // No cache
        [HttpPost]
        [Route("api/columbiasuicide/search/{id}")]
        public List<ColumbiaSuicideReportSearchResponse> GetReports(
            [FromUri] long id,
            [FromBody] PagedFilterModel filter
        )
        {
            filter = filter ?? new PagedFilterModel();

            id.ShouldNotBeDefault();

            return _service.GetRelatedReports(id, filter);
        }


        /// <summary>
        /// Create new C-SSRS record from Visit record
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/columbiasuicide/visit")]
        public long CreateFromVisit([FromBody] ColumbiaSuicideCreateVisitRequest request)
        {
            request.ShouldNotBeNull();
            request.BhsVisitID.ShouldNotBeDefault();

            var visit = _visitService.Get(request.BhsVisitID);

            visit.ShouldNotBeNull();

            var id = _service.Create(visit, _userService.GetCurrent());

            return id;
        }


        /// <summary>
        /// Create new C-SSRS record from another Columbia report (use the same patient info)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/columbiasuicide/copy")]
        public long CreateFromReport([FromBody] ColumbiaSuicideCopyFromRequest request)
        {
            request.ShouldNotBeNull();
            request.ID.ShouldNotBeDefault();

            var columbiaSource = _service.Get(request.ID);

            columbiaSource.ShouldNotBeNull();

            ColumbiaSuicideReport report = ColumbiaSuicideReportFactory.CopyPatientFrom(columbiaSource);

            var id = _service.Add(report, _userService.GetCurrent());

            _logger.Info("Created new C-SSRS from existing record.");

            return id;
        }


        /// <summary>
        /// Create new C-SSRS record
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/columbiasuicide")]
        public long Add([FromBody] ColumbiaSuicideCreateRequest request)
        {
            request.ShouldNotBeNull();

            Validate(request);

            ColumbiaSuicideReport report = ColumbiaSuicideReportFactory.Create(request);

            var id = _service.Add(report, _userService.GetCurrent());

            _logger.Info("Created new C-SSRS.");

            return id;
        }


        /// <summary>
        /// Get C-SSRS record
        /// </summary>
        /// <param name="id">Suicide Report Id.</param>
        /// <returns></returns>
        [HttpCache(DefaultExpirySeconds = 0)] // No cache
        [HttpGet]
        [Route("api/columbiasuicide/{id}")]
        public ColumbiaSuicideReportResponse Get([FromUri] long id)
        {
            id.ShouldNotBeDefault();

            var model = _service.Get(id);

            model.ShouldNotBeNull(id);

            var result = Mapper.Map<ColumbiaSuicideReportResponse>(model);

            if (string.IsNullOrEmpty(result.StaffNameCompleted))
            {
                result.StaffNameCompleted = _userService.GetCurrent().FullName;
            }
            return result;
        }


        /// <summary>
        /// Update visit record
        /// </summary>
        /// <param name="id">Visit Id.</param>
        /// <param name="requestModel">Updated visit model</param>
        /// <returns></returns>
        [HttpCache(DefaultExpirySeconds = 0)] // No cache
        [HttpPut]
        [Route("api/columbiasuicide/{id}")]
        public IHttpActionResult Update([FromUri] long id, [FromBody] ColumbiaSuicideReportUpdateRequest requestModel)
        {
            id.ShouldNotBeDefault();
            requestModel.ShouldNotBeNull();


            // get current state
            var currentState = _service.Get(id);

            // validate it exists
            currentState.ShouldNotBeNull(id);

            

            // update current state with values from the request
            var updatedModel = Mapper.Map(requestModel, currentState);

            //// get current user
            var currentPrincipal = _userService.GetCurrent();

            updatedModel.BhsStaffNameCompleted = currentPrincipal.FullName;
            updatedModel.CompleteDate = _timeService.GetDateTimeOffsetNow();

            //// save changes
            _service.Update(updatedModel, _userService.GetCurrent());

            //// add record in audit log
            //_securityLogService.Add(new SecurityLog(SecurityEvents.UpdateBhsVisitInformation, "{0}~{1}~{2}"
            //    .FormatWith(
            //        id,
            //        currentState.Result.FullName,
            //        currentState.Result.Birthday.FormatAsDate()
            //    ),
            //    currentState.LocationID)
            //);

            return Ok(FrontDesk.Common.Messages.CustomMessage.GetUpdateMessage("Columbia SSRS"));
        }


        protected void Validate(ColumbiaSuicideCreateRequest request)
        {
            var result = new ColumbiaSuicideCreateRequestValidator().Validate(request);

            if (result.IsValid) return;

            ResponseDataFactory.ThrowInvalidModelMessage(result.Errors);
        }


        /// <summary>
        /// Get pdf version of C-SSRS report.
        /// </summary>
        /// <returns>Pdf file with visit report.</returns>
        [HttpCache(DefaultExpirySeconds = 0)] // cache disabled
        [Route("api/columbiasuicide/{id}/print")]
        [HttpGet, HttpPost]
        public HttpResponseMessage Print(long id)
        {

            var model = _service.Get(id);

            if (model == null)
            {
                ResponseDataFactory.ThrowNotFound(id);
            }

            //Print report into context

            //var pdfReport = new BhsVisitPdfPrintout(model, _screeningInfoService);

            var response = Request.CreateResponse(HttpStatusCode.OK);


            //pdfReport.CreatePDF(
            //    response, "BhsVisitReport_{0}.pdf".FormatWith(id));

            _securityLogService.Add(new SecurityLog(SecurityEvents.PrintBhsVisit, model.ID, model.BranchLocationID));

            return response;
        }


    }
}

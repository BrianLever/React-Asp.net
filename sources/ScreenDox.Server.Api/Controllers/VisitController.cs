using AutoMapper;

using CacheCow.Server.WebApi;

using FrontDesk;
using FrontDesk.Common.Bhservice;
using FrontDesk.Common.Extensions;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Printouts.Bhs;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Screening.Services;

using ScreenDox.Server.Api.Infrastructure;
using ScreenDox.Server.Api.Models;
using ScreenDox.Server.Formatters;
using ScreenDox.Server.Logging;
using ScreenDox.Server.Models;
using ScreenDox.Server.Models.Factory;
using ScreenDox.Server.Models.ViewModels;
using ScreenDox.Server.Models.ViewModels.Validators;
using ScreenDox.Server.Screening;
using ScreenDox.Server.Security;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ScreenDox.Server.Api.Controllers
{
    /// <summary>
    /// Patient Visit resource
    /// </summary>
    public class VisitController : ApiController
    {
        private readonly IVisitService _visitService;
        private readonly IScreeningDefinitionService _screeningInfoService;
        private readonly IUserPrincipalService _userService;
        private readonly ISecurityLogService _securityLogService;
        private readonly TodayDateFormatter _todayDateFormatter;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="visitService"></param>
        /// <param name="screeningInfoService"></param>
        /// <param name="userService"></param>
        /// <param name="securityLogService"></param>
        /// <param name="todayDateFormatter"></param>
        public VisitController(IVisitService visitService,
                               IScreeningDefinitionService screeningInfoService,
                               IUserPrincipalService userService,
                               ISecurityLogService securityLogService,
                               TodayDateFormatter todayDateFormatter)
        {
            _visitService = visitService ?? throw new ArgumentNullException(nameof(visitService));
            _screeningInfoService = screeningInfoService ?? throw new ArgumentNullException(nameof(screeningInfoService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _securityLogService = securityLogService ?? throw new ArgumentNullException(nameof(securityLogService));
            _todayDateFormatter = todayDateFormatter ?? throw new ArgumentNullException(nameof(todayDateFormatter));
        }


        /// <summary>
        /// Get items for Patient Screens result grid
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
        [Route("api/visit/search")]
        public SearchResponse<UniqueVisitViewModel> GetVisits([FromBody] PagedVisitFilterModel filter)
        {
            filter = filter ?? new PagedVisitFilterModel();

            Guard.ValidateOrderByClause(filter, new[] { "LastCreatedDate", "FullName", "Birthday" });

            var result = _visitService.GetUniqueVisits(filter);

            result.Items.ForEach(x =>
           {
               x.LastCreatedDateLabel = _todayDateFormatter.Format(x.LastCreatedDate);
               x.LastCompleteDateLabel = _todayDateFormatter.Format(x.LastCompleteDate);
           });

            return result;
        }


        /// <summary>
        /// Get related visits and patient demographics screen id
        /// </summary>
        /// <param name="id">Screendox Id.</param>
        /// <param name="filter">Filter conditions.</param>
        /// <returns></returns>
        [HttpCache(DefaultExpirySeconds = 0)] // No cache
        [HttpPost]
        [Route("api/visit/search/{id}")]
        public List<VisitViewModel> GetRelatedPatientScreenings(
            [FromUri] long id,
            [FromBody] VisitFilterModel filter
        )
        {
            filter = filter ?? new VisitFilterModel();

            id.ShouldNotBeDefault();

            return _visitService.GetRelatedVisits(id, filter);
        }


        /// <summary>
        /// Get visit record
        /// </summary>
        /// <param name="id">Visit Id.</param>
        /// <returns></returns>
        [HttpCache(DefaultExpirySeconds = 0)] // No cache
        [HttpGet]
        [Route("api/visit/{id}")]
        public VisitResponse Get([FromUri] long id)
        {
            id.ShouldNotBeDefault();

            var model = _visitService.Get(id);

            model.ShouldNotBeNull(id);

            var result = Mapper.Map<VisitResponse>(model);

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
        [Route("api/visit/{id}")]
        public IHttpActionResult Update([FromUri] long id, [FromBody] VisitRequest requestModel)
        {
            id.ShouldNotBeDefault();

            // get current state
            var currentState = _visitService.Get(id);

            // validate it exists
            currentState.ShouldNotBeNull(id);

            if (!ValidateDrugOfChoiceUpdate(currentState, requestModel, id))
            {
                ResponseDataFactory.ThrowBadRequestMessage("Invalid DrugOfChoice model. Model is invalid or DAST-10 response is missing or negative.");
            }


            // update current state with values from the request
            var updatedModel = Mapper.Map<VisitRequest, BhsVisit>(requestModel, currentState);

            // get current user
            var currentPrincipal = _userService.GetCurrent();



            // save changes
            _visitService.Update(updatedModel, currentPrincipal);

            // add record in audit log
            _securityLogService.Add(new SecurityLog(SecurityEvents.UpdateBhsVisitInformation, "{0}~{1}~{2}"
                .FormatWith(
                    id,
                    currentState.Result.FullName,
                    currentState.Result.Birthday.FormatAsDate()
                ),
                currentState.LocationID)
            );

            UpdateDurgOfChoice(updatedModel, requestModel, id);

            _visitService.UpdateDrugOfChoiceInResult(updatedModel);

            return Ok(FrontDesk.Common.Messages.CustomMessage.GetUpdateMessage("Visit"));
        }

        private bool ValidateDrugOfChoiceUpdate(BhsVisit currentState, VisitRequest requestModel, long id)
        {
            var currentDrugOfChoice = new DrugOfChoiceModel(currentState.Result);
            var updatedDrugOfChoice = new DrugOfChoiceModel(ScreeningResultRequestFactory.Create(requestModel.DrugOfChoice, id));

            if (updatedDrugOfChoice == null || updatedDrugOfChoice?.Primary == 0)
            {
                return true; // drug of choice modification not sent
            }


            if (currentState.SubstanceAbuseFlag?.ScoreLevel > 0 || currentDrugOfChoice?.Primary > 0)
            {
                return true; // allow drugs update
            }

            return false;
        }

        private void UpdateDurgOfChoice(BhsVisit updatedModel, VisitRequest requestModel, long id)
        {
            var requestDrugOfChoiceSection = ScreeningResultRequestFactory.Create(requestModel.DrugOfChoice, id);

            if (requestDrugOfChoiceSection == null)
            {
                return; // no changes, do nothing
            }

            var existingDrugOfChoice = new DrugOfChoiceModel(updatedModel.Result);

            if (existingDrugOfChoice == null)
            { // adding new section
                updatedModel.Result.AppendSectionAnswer(requestDrugOfChoiceSection);
            }
            else
            {
                var requestedDrugOfChoice = new DrugOfChoiceModel(requestDrugOfChoiceSection);

                existingDrugOfChoice.Primary = requestedDrugOfChoice.Primary;
                existingDrugOfChoice.Secondary = requestedDrugOfChoice.Secondary;
                existingDrugOfChoice.Tertiary = requestedDrugOfChoice.Tertiary;

            }
        }

        /// <summary>
        /// Update patient's address for the visit
        /// </summary>
        /// <param name="visitid">Visit Id.</param>
        /// <param name="requestModel">Patient's address from EHR system</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/visit/patientaddress/{visitid}")]
        public IHttpActionResult SetPatientAddress([FromUri] long visitid, [FromBody] PatientAddressRequest requestModel)
        {
            visitid.ShouldNotBeDefault();

            Validate(requestModel);

            // get current state
            var currentState = _visitService.Get(visitid);

            // validate it exists
            currentState.ShouldNotBeNull(visitid);


            var screeningResultCurrentState = currentState.Result;


            // update current state with values from the request
            var updatedModel = Mapper.Map<PatientAddressRequest, ScreeningResult>(requestModel, screeningResultCurrentState);

            // save changes
            _visitService.FullfilPatientAddress(updatedModel);

            _securityLogService.Add(new SecurityLog(SecurityEvents.PatientAddressHasBeenAddedFromBhsVisit, "{0}~{1}"
                .FormatWith(updatedModel.FullName, updatedModel.Birthday.FormatAsDate()),
                    updatedModel.LocationID));

            return Ok(FrontDesk.Common.Messages.CustomMessage.GetUpdateMessage("Patient Address"));
        }

        protected void Validate(PatientAddressRequest request)
        {

            var result = new PatientAddressRequestValidator().Validate(request);

            if (result.IsValid) return;

            ResponseDataFactory.ThrowInvalidModelMessage(result.Errors);
        }


        /// <summary>
        /// Get pdf version of Visit Search.
        /// </summary>
        /// <param name="filter">Filter conditions.</param>
        /// <returns>Pdf file with visit search results.</returns>
        [HttpCache(DefaultExpirySeconds = 0)] // cache disabled
        [Route("api/visit/search/print")]
        [HttpPost]
        public HttpResponseMessage PrintSearch([FromBody] VisitFilterModel filter)
        {

            var data = _visitService.GetAllForPrintout(filter.ToSearchFilterModel());

            if (data == null)
            {
                data = new List<BhsVisitListItemPrintoutModel>();
            }

            //Print report into context

            var pdfReport = new BhsVisitListPdfPrintout(data);

            var response = Request.CreateResponse(HttpStatusCode.OK);


            pdfReport.CreatePDF(
                response, "BhsVisitList.pdf");

            return response;
        }

        /// <summary>
        /// Get pdf version of Visit report.
        /// </summary>
        /// <returns>Pdf file with visit report.</returns>
        [HttpCache(DefaultExpirySeconds = 0)] // cache disabled
        [Route("api/visit/{id}/print")]
        [HttpGet, HttpPost]
        public HttpResponseMessage Print(long id)
        {

            var model = _visitService.Get(id);

            if (model == null)
            {
                ResponseDataFactory.ThrowNotFound(id);
            }

            //Print report into context

            var pdfReport = new BhsVisitPdfPrintout(model, _screeningInfoService);

            var response = Request.CreateResponse(HttpStatusCode.OK);


            pdfReport.CreatePDF(
                response, "BhsVisitReport_{0}.pdf".FormatWith(id));

            _securityLogService.Add(new SecurityLog(SecurityEvents.PrintBhsVisit, model.ID, model.Result.LocationID));

            return response;
        }


    }
}

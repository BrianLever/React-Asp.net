using AutoMapper;

using CacheCow.Server.WebApi;

using FrontDesk;
using FrontDesk.Common.Bhservice;
using FrontDesk.Common.Extensions;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Printouts.Bhs;
using FrontDesk.Server.Screening;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Screening.Services;

using ScreenDox.Server.Api.Infrastructure;
using ScreenDox.Server.Api.Models;
using ScreenDox.Server.Formatters;
using ScreenDox.Server.Logging;
using ScreenDox.Server.Models;
using ScreenDox.Server.Security;

using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ScreenDox.Server.Api.Controllers
{
    /// <summary>
    ///  Follow-up resource
    /// </summary>
    public class FollowUpController : ApiController
    {

        private readonly IFollowUpService _followUpService;
        private readonly IUserPrincipalService _userService;
        private readonly ISecurityLogService _securityLogService;
        private readonly TodayDateFormatter _todayDateFormatter;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="followUpService"></param>
        /// <param name="userService"></param>
        /// <param name="securityLogService"></param>
        /// <param name="todayDateFormatter"></param>
        public FollowUpController(IFollowUpService followUpService,
                                  IUserPrincipalService userService,
                                  ISecurityLogService securityLogService,
                                  TodayDateFormatter todayDateFormatter)
        {
            _followUpService = followUpService ?? throw new System.ArgumentNullException(nameof(followUpService));
            _userService = userService ?? throw new System.ArgumentNullException(nameof(userService));
            _securityLogService = securityLogService ?? throw new System.ArgumentNullException(nameof(securityLogService));
            _todayDateFormatter = todayDateFormatter ?? throw new System.ArgumentNullException(nameof(todayDateFormatter));
        }

        /// <summary>
        /// Get items for Patient Screens result grid
        /// </summary>
        /// <param name="filter">Filter with paging.
        /// Order by options:
        ///     - LastFollowUpDate DESC (default)
        ///     - LastVisitDate
        ///     - LastCompleteDate
        ///     - FullName ASC
        ///     - Birthday
        /// </param>
        /// <returns>Paged list of visits with given filter conditions.</returns>
        [HttpCache(DefaultExpirySeconds = 0)] // No cache
        [HttpPost]
        [Route("api/followup/search")]
        public SearchResponse<UniqueFollowUpViewModel> GetUniqueItems([FromBody] PagedVisitFilterModel filter)
        {
            filter = filter ?? new PagedVisitFilterModel();

            Guard.ValidateOrderByClause(filter, new[] {
                "LastFollowUpDate",
                "LastVisitDate",
                "LastCompleteDate",
                "FullName",
                "Birthday"
            });

            var result = _followUpService.GetUniqueItems(filter);

            bool useTodayLabelInFormattedDate = true;

            result.Items.ForEach(x =>
            {
                x.LastCompleteDateLabel = _todayDateFormatter.Format(x.LastCompleteDate);
                x.LastFollowUpDateLabel = _todayDateFormatter.FormatAsDate(x.LastFollowUpDate, useTodayLabelInFormattedDate);
                x.LastVisitDateLabel = _todayDateFormatter.FormatAsDate(x.LastVisitDate, useTodayLabelInFormattedDate);
            });

            return result;
        }

        /// <summary>
        /// Get related follow ups
        /// </summary>
        /// <param name="id">Screendox Id.</param>
        /// <param name="filter">Filter conditions.</param>
        /// <returns></returns>
        [HttpCache(DefaultExpirySeconds = 0)] // No cache
        [HttpPost]
        [Route("api/followup/search/{id}")]
        public List<BhsFollowUpViewModel> GetRelatedItems(
            [FromUri] long id,
            [FromBody] VisitFilterModel filter
        )
        {
            filter = filter ?? new VisitFilterModel();

            id.ShouldNotBeDefault();

            return _followUpService.GetRelatedItems(id, filter);
        }


        [HttpCache(DefaultExpirySeconds = 0)] // No cache
        [HttpGet]
        [Route("api/followup/visit/{visitId}")]
        public List<BhsFollowUpViewModel> GetFollowUpsByVisitId(long visitId)
        {
            visitId.ShouldNotBeDefault();

            return _followUpService.GetFollowUpsForVisit(visitId);
        }

        /// <summary>
        /// Get pdf version of Follow-Up search.
        /// </summary>
        /// <param name="filter">Filter conditions.</param>
        /// <returns>Pdf file with follow-up search results.</returns>
        [HttpCache(DefaultExpirySeconds = 0)] // cache disabled
        [Route("api/followup/search/print")]
        [HttpPost]
        public HttpResponseMessage PrintSearch([FromBody] VisitFilterModel filter)
        {

            var data = _followUpService.GetAllForPrintout(filter.ToSearchFilterModel());

            if (data == null)
            {
                data = new List<BhsFollowUpListItemPrintoutModel>();
            }

            //Print report into context

            var pdfReport = new BhsFollowUpListPdfPrintout(data);

            var response = Request.CreateResponse(HttpStatusCode.OK);


            pdfReport.CreatePDF(
                response, "BhsFollowUpList.pdf");

            return response;
        }


        /// <summary>
        /// Get follow-up record
        /// </summary>
        /// <param name="id">Follow Up Id.</param>
        /// <returns></returns>
        [HttpCache(DefaultExpirySeconds = 0)] // No cache
        [HttpGet]
        [Route("api/followup/{id}")]
        public FollowUpResponse Get([FromUri] long id)
        {
            id.ShouldNotBeDefault();

            var model = _followUpService.Get(id);

            if (model == null)
            {
                ResponseDataFactory.ThrowNotFound();
            }

            var result = Mapper.Map<FollowUpResponse>(model);

            if (string.IsNullOrEmpty(result.StaffNameCompleted))
            {
                result.StaffNameCompleted = _userService.GetCurrent().FullName;
            }
            return result;
        }

        /// <summary>
        /// Update follow-up record
        /// </summary>
        /// <param name="id">Follow-Up record Id.</param>
        /// <param name="requestModel">Updated follow-up model</param>
        /// <returns></returns>
        [HttpCache(DefaultExpirySeconds = 0)] // No cache
        [HttpPut]
        [Route("api/followup/{id}")]
        public IHttpActionResult Update([FromUri] long id, [FromBody] FollowUpRequest requestModel)
        {
            id.ShouldNotBeDefault();

            // get current state
            var currentState = _followUpService.Get(id);

            // update current state with values from the request
            var updatedModel = Mapper.Map<FollowUpRequest, BhsFollowUp>(requestModel, currentState);

            // get current user
            var currentPrincipal = _userService.GetCurrent();
            // save changes
            _followUpService.Update(updatedModel, currentPrincipal);

            // add record in audit log
            _securityLogService.Add(new SecurityLog(
                SecurityEvents.UpdateBhsThirtyDayFollowUpInformation,
                "{0}~{1}~{2}".FormatWith(id,
                currentState.Result.FullName,
                currentState.Result.Birthday.FormatAsDate()),
                currentState.Visit.LocationID)
            );

            return Ok(FrontDesk.Common.Messages.CustomMessage.GetUpdateMessage("Follow-up"));
        }


        /// <summary>
        /// Get pdf version of Follow-Up report.
        /// </summary>
        /// <returns>Pdf file with visit report.</returns>
        [HttpCache(DefaultExpirySeconds = 0)] // cache disabled
        [Route("api/followup/{id}/print")]
        [HttpGet, HttpPost]
        public HttpResponseMessage Print(long id)
        {

            var model = _followUpService.Get(id);

            if (model == null)
            {
                ResponseDataFactory.ThrowNotFound(id);
            }

            //Print report into context

            var pdfReport = new BhsFollowUpPdfPrintout(model);

            var response = Request.CreateResponse(HttpStatusCode.OK);


            pdfReport.CreatePDF(
                response, "BhsFollowUpReport_{0}.pdf".FormatWith(id));

            _securityLogService.Add(new SecurityLog(SecurityEvents.PrintBhsFollowUp, model.ID, model.Result.LocationID));

            return response;
        }
    }
}
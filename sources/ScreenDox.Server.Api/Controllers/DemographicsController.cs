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
using ScreenDox.Server.Logging;
using ScreenDox.Server.Models;
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
    public class DemographicsController : ApiController
    {
        private readonly IBhsDemographicsService _demographicService;
        private readonly IUserPrincipalService _userService;
        private readonly ISecurityLogService _securityLogService;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="visitService"></param>
        /// <param name="screeningInfoService"></param>
        /// <param name="userService"></param>
        /// <param name="securityLogService"></param>
        public DemographicsController(IBhsDemographicsService visitService,
            IScreeningDefinitionService screeningInfoService,
            IUserPrincipalService userService,
            ISecurityLogService securityLogService)
        {
            _demographicService = visitService ?? throw new ArgumentNullException(nameof(visitService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _securityLogService = securityLogService ?? throw new ArgumentNullException(nameof(securityLogService));
        }


        /// <summary>
        /// Get visit record
        /// </summary>
        /// <param name="id">Visit Id.</param>
        /// <returns></returns>
        [HttpCache(DefaultExpirySeconds = 0)] // No cache
        [HttpGet]
        [Route("api/demographics/{id}")]
        public DemographicsResponse Get([FromUri] long id)
        {
            id.ShouldNotBeDefault();

            var model = _demographicService.Get(id);

            model.ShouldNotBeNull(id);

            return Mapper.Map<DemographicsResponse>(model);
        }


        /// <summary>
        /// Update demographics record
        /// </summary>
        /// <param name="id">Demographics Id.</param>
        /// <param name="requestModel">Updated demographics model</param>
        /// <returns></returns>
        [HttpCache(DefaultExpirySeconds = 0)] // No cache
        [HttpPut]
        [Route("api/demographics/{id}")]
        public IHttpActionResult Update([FromUri] long id, [FromBody] DemographicsRequest requestModel)
        {
            id.ShouldNotBeDefault();

            // get current state
            var currentState = _demographicService.Get(id);

            // update current state with values from the request
            var updatedModel = Mapper.Map<DemographicsRequest, BhsDemographics>(requestModel, currentState);
            // get current user
            var currentPrincipal = _userService.GetCurrent();
            // save changes
            _demographicService.Update(updatedModel, currentPrincipal);

            // add record in audit log
            _securityLogService.Add(new SecurityLog(SecurityEvents.UpdateBhsDemographicsInformation, "{0}~{1}~{2}".FormatWith(
                currentState.FullName,
                currentState.Birthday.FormatAsDate(),
                currentState.LocationID
                ),
                currentState.LocationID)
            );

            return Ok(FrontDesk.Common.Messages.CustomMessage.GetUpdateMessage("Patient Demographics"));
        }

        /// <summary>
        /// Get pdf version of Demographics report.
        /// </summary>
        /// <returns>Pdf file with visit report.</returns>
        [HttpCache(DefaultExpirySeconds = 0)] // cache disabled
        [Route("api/demographics/{id}/print")]
        [HttpGet, HttpPost]
        public HttpResponseMessage Print(long id)
        {

            var model = _demographicService.Get(id);

            model.ShouldNotBeNull(id);

            //Print report into context

            var pdfReport = new BhsDemographicsPdfPrintout(model);

            var response = Request.CreateResponse(HttpStatusCode.OK);


            pdfReport.CreatePDF(
                response, "PatientDemographicsReport_{0}.pdf".FormatWith(id));

            _securityLogService.Add(new SecurityLog(SecurityEvents.PrintBhsDemographics, model.ID, model.LocationID));

            return response;
        }


    }
}

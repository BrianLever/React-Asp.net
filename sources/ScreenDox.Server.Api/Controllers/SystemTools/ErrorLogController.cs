
using CacheCow.Server.WebApi;

using Common.Logging;

using FrontDesk.Common.InfrastructureServices;
using FrontDesk.Server.Membership;
using FrontDesk.Server.Reports.ExcelReports;

using ScreenDox.Server.Api.Infrastructure;
using ScreenDox.Server.Api.Security;
using ScreenDox.Server.Logging;
using ScreenDox.Server.Models;
using ScreenDox.Server.Models.SearchFilters;
using ScreenDox.Server.Services;

using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ScreenDox.Server.Api.Controllers.SystemTools
{
    /// <summary>
    /// Manages Visit creation system settings
    /// </summary>
    [Authorize()]
    [RestrictAccessToRoles(UserRoles.SuperAdministrator, UserRoles.BranchAdministrator)]
    public class ErrorLogController : ApiController
    {
        private readonly IErrorLogService _service;
        private readonly ILog _logger = LogManager.GetLogger<ErrorLogController>();

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="service"></param>
        public ErrorLogController(
            IErrorLogService service
        )
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }


        /// <summary>
        /// Get current state of visit settings
        /// </summary>
        /// <returns></returns>
        [HttpCache(DefaultExpirySeconds = 0)] // No cache
        [HttpPost]
        public SearchResponse<ErrorLogItem> GetAll(PagedDateRangeNameFilter filter)
        {
            //return results
            var result = _service.GetAll(filter);

            return result;
        }


        /// <summary>
        /// Get current state of visit settings
        /// </summary>
        /// <returns></returns>
        [HttpCache(DefaultExpirySeconds = 0)] // No cache
        [HttpGet]
        public ErrorLogItem Get(long id)
        {
            //return results
            var result = _service.Get(id);

            result.ShouldNotBeNull();

            return result;
        }


        [HttpDelete]
        [Route("api/errorlog/clear")]
        public IHttpActionResult DeleteAll()
        {
            //return results
            _service.DeleteAll();

            return Ok();
        }


        /// <summary>
        /// Get excel export of Error log
        /// </summary>
        /// <returns>Excel file with screening result details.</returns>
        [Route("api/errorlog/export")]
        [HttpPost]
        public HttpResponseMessage Export(PagedDateRangeNameFilter filter)
        {
            var report = GetAll(filter);

            //Print report into context
            ErrorLogExcelExport excelReport = new ErrorLogExcelExport(report);

            var response = Request.CreateResponse(HttpStatusCode.OK);

            excelReport.Create(
                response, "Error_Log.xlsx");

            return response;
        }

    }
}

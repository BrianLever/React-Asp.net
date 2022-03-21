using CacheCow.Server.WebApi;

using Common.Logging;

using Frontdesk.Server.SmartExport.Data;
using Frontdesk.Server.SmartExport.Models;

using ScreenDox.EHR.Common;
using ScreenDox.EHR.Common.Models.PatientValidation;
using ScreenDox.Server.Models.SearchFilters;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ScreenDox.Server.Api.Controllers.SystemTools
{
    public class AutoExportDashboardController : ApiController
    {
        private readonly PatientNameCorrectionLogService _patientNameCorrectionLogService;
        private readonly SmartExportDb _smartExportDb;
        private readonly ILog _logger = LogManager.GetLogger<AutoExportDashboardController>();


        public AutoExportDashboardController(PatientNameCorrectionLogService patientNameCorrectionLogService,
                                             SmartExportDb smartExportDb)
        {
            _patientNameCorrectionLogService = patientNameCorrectionLogService ?? throw new ArgumentNullException(nameof(patientNameCorrectionLogService));
            _smartExportDb = smartExportDb ?? throw new ArgumentNullException(nameof(smartExportDb));
        }

        /// <summary>
        /// Get Patient Name Auto-Correction Log
        /// </summary>
        /// <returns></returns>
        [HttpCache(DefaultExpirySeconds = 0)] // No cache
        [Route("api/patientname/autocorrectionlog")]
        [HttpPost]
        public List<PatientNameCorrectionLogItem> GetAll(PagedDateRangeNameFilter filter)
        {
            filter = filter ?? new PagedDateRangeNameFilter();
            var result = _patientNameCorrectionLogService.Get(filter.StartDate, filter.EndDate, filter.nameFilter, filter.OrderBy, filter.StartRowIndex, filter.MaximumRows);
            return result;
        }

        /// <summary>
        /// Get Count
        /// </summary>
        /// <returns></returns>
        [HttpCache(DefaultExpirySeconds = 0)] // No cache
        [Route("api/getcount")]
        [HttpPost]
        public int GetCount(PagedDateRangeNameFilter filter)
        {
            filter = filter ?? new PagedDateRangeNameFilter();
            var result = _patientNameCorrectionLogService.GetCount(filter.StartDate, filter.EndDate, filter.nameFilter);
            return result;
        }

        /// <summary>
        /// Get Auto-Export Statistics
        /// </summary>
        /// <returns></returns>
        [HttpCache(DefaultExpirySeconds = 0)] // No cache
        [Route("api/statistics")]
        [HttpPost]
        public ExportSummary GetStatistics(DateRangeFilter filter)
        {
            var result = _smartExportDb.GetExportSummary(filter.StartDate, filter.EndDate);
            return result;
        }
    }
}
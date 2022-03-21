using CacheCow.Server.WebApi;

using FrontDesk.Server.Configuration;
using FrontDesk.Server.Controllers;
using FrontDesk.Server.Licensing.Services;
using FrontDesk.Server.Screening.Services;

using ScreenDox.Server.Api.Models;
using ScreenDox.Server.Common.Infrastructure;
using ScreenDox.Server.Common.Services;
using ScreenDox.Server.Models;

using System;
using System.Web.Http;

namespace ScreenDox.Server.Api.Controllers
{
    /// <summary>
    /// Get and set system settings
    /// </summary>
    public class SystemSettingsController : ApiController
    {

        private readonly ILicenseService _licenseService;
        private readonly IScreeningResultService _screeningResultService;
        private readonly IBranchLocationService _branchLocationService;
        private readonly IKioskService _kioskService;
        private readonly IDataCacheService<LicenseResponse> _licenseCacheService;
        private readonly RpmsExportController _exportController;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SystemSettingsController(ILicenseService licenseService,
                                        IScreeningResultService screeningResultService,
                                        IBranchLocationService branchLocationService,
                                        IKioskService kioskService,
                                        IDataCacheService<LicenseResponse> licenseCacheService,
                                        RpmsExportController exportController)
        {
            _licenseService = licenseService ?? throw new ArgumentNullException(nameof(licenseService));
            _screeningResultService = screeningResultService ?? throw new ArgumentNullException(nameof(screeningResultService));
            _branchLocationService = branchLocationService ?? throw new ArgumentNullException(nameof(branchLocationService));
            _kioskService = kioskService ?? throw new ArgumentNullException(nameof(kioskService));
            _licenseCacheService = licenseCacheService ?? throw new ArgumentNullException(nameof(licenseCacheService));
            _exportController = exportController ?? throw new ArgumentNullException(nameof(exportController));
        }

        /// <summary>
        /// Get system info
        /// </summary>
        /// <returns>System info object.</returns>
        [HttpGet]
        [HttpCache(DefaultExpirySeconds = 300)] // 5 minutes
        [Route("api/systeminfo")]
        public SystemInfoResponse GetSystemInfo()
        {
            var result = new SystemInfoResponse
            {
                AppVersion = FrontDesk.Common.Versioning.AppVersion.CurrentAppVersion
            };

            result.License = GetLicense();

            result.HasActiveLicense = result.License != null;

            result.Summary = new SystemSummary
            {
                CheckInRecordCount = _screeningResultService.GetTotalRecordCount(),
                KioskCount = _kioskService.GetNotDisabledCount(),
                BranchLocationCount = _branchLocationService.GetNotDisabledCount()
            };

            result.CentralLoggingUrl = AppSettings.CentralLoggingUrl;

            // set EHR Credentials Alert properties
            result.IsEhrCredentialsExpirationAlert = _exportController.ShouldDisplayAlertMessage();
            result.EhrCredentialsExpirationAlertMessage = _exportController.GetAlertMessageText();

            return result;
        }

        private LicenseResponse GetLicense()
        {

            var key = "activeLicense";

            LicenseResponse result = _licenseCacheService.Get(key);

            if (result == null)
            {
                var license = _licenseService.GetActivatedLicense();

                if (license != null)
                {
                    result = new LicenseResponse
                    {
                        LicenseString = license.License.LicenseString,
                        MaxKiosk = license.License.MaxKiosks,
                        MaxBranchLocations = license.License.MaxBranchLocations,
                        ExpirationDate = license.ExpirationDate
                    };

                    _licenseCacheService.Add(key, result, TimeSpan.FromHours(4));
                }
            }

            return result;
        }
    }
}

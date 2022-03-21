using Common.Logging;
using ScreenDoxKioskLauncher.Infrastructure;
using ScreenDoxKioskLauncher.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDoxKioskLauncher.Controllers
{
    /// <summary>
    /// Verify new version exists and register upgrade job
    /// </summary>
    public sealed class CheckNewVersionAvailableController : TimerEventController
    {
        private readonly IPackageDownloadService _packageDownloadService;
        private readonly IEnvironmentProvider _environmentProvider;
        private readonly IUpgradeScheduleService _upgradeScheduleService;
        private readonly IApplicationStateService _applicationStateService;

        private ILog _logger = LogManager.GetLogger<CheckNewVersionAvailableController>();

        /// <summary>
        /// Interval
        /// </summary>
        protected override TimeSpan TimerInterval
        {
            get { return _environmentProvider.CheckUpgradeTimeInterval; }
        }

        /// <summary>
        /// Controller name
        /// </summary>
        protected override string ControllerName => nameof(CheckNewVersionAvailableController);

        /// <summary>
        /// Constructor for dependency injection
        /// </summary>
        /// <param name="packageDownloadService"></param>
        /// <param name="upgradeScheduleService"></param>
        /// <param name="environmentProvider"></param>
        /// <param name="applicationStateService"></param>
        public CheckNewVersionAvailableController(
            IPackageDownloadService packageDownloadService,
            IUpgradeScheduleService upgradeScheduleService,
            IEnvironmentProvider environmentProvider,
            IApplicationStateService applicationStateService)
        {
            _packageDownloadService = packageDownloadService ?? throw new ArgumentNullException(nameof(packageDownloadService));
            _upgradeScheduleService = upgradeScheduleService ?? throw new ArgumentNullException(nameof(upgradeScheduleService));
            _environmentProvider = environmentProvider ?? throw new ArgumentNullException(nameof(environmentProvider));
            _applicationStateService = applicationStateService ?? throw new ArgumentNullException(nameof(applicationStateService));
        }

        /// <summary>
        /// Run job
        /// </summary>
        public override void OnTimerTickAction()
        {
            if (!_applicationStateService.IsInNormalState())
            {
                _logger.Info("Application is not in the normal state. Skipping this cycle.");
                return;
            }

            var currentVersion = _packageDownloadService.GetCurrentVersion();

            _logger.Info($"Current version: {currentVersion}");

            var versionToInstall = _packageDownloadService.CheckNewVersionAvailable();

            if (versionToInstall == null)
            {
                _logger.InfoFormat("Current version is up to date");

                return;
            }

            // new version has found on the server.
           

            // check the version has not been downloaded yet
            if (_upgradeScheduleService.CheckVersionIsReadyToInstall(versionToInstall.Version))
            {
                _logger.Info($"Version has been downloaded already. Version: {versionToInstall.Version}");
                return;
            }

            try
            {
                // download the package
                _packageDownloadService.DownloadPackage(versionToInstall.Version);

                // register schedule to install
                _upgradeScheduleService.RegisterInstallationJob(versionToInstall);

                _logger.Info($"Version has been scheduled to install. Version: {versionToInstall.Version}. Time: {versionToInstall.InstallOn}.");
            }
            catch (System.IO.IOException ex)
            {
                _logger.Error("Failed to write the file. Permissions issues.", ex);
            }
            catch (DownloadPackageException ex)
            {
                _logger.Error("Failed to download new version.", ex);
            }
        }
    }
}

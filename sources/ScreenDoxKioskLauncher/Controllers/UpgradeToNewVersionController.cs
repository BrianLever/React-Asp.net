using Common.Logging;

using ScreenDoxKioskLauncher.Infrastructure;
using ScreenDoxKioskLauncher.Models;
using ScreenDoxKioskLauncher.Services;

using System;
using System.Diagnostics;
using System.Threading;

namespace ScreenDoxKioskLauncher.Controllers
{
    /// <summary>
    /// Verify new version exists and register upgrade job
    /// </summary>
    public sealed class UpgradeToNewVersionController : TimerEventController
    {
        private readonly IEnvironmentProvider _environmentProvider;
        private readonly IUpgradeScheduleService _upgradeScheduleService;
        private readonly IKioskAppManagerService _kioskAppManagerService;
        private readonly IFileManagementService _fileManagementService;
        private readonly IKioskHealthService _kioskHeathService;
        private readonly IApplicationStateService _applicationStateService;


        private readonly ILog _logger = LogManager.GetLogger<UpgradeToNewVersionController>();

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
        protected override string ControllerName => nameof(UpgradeToNewVersionController);

        /// <summary>
        /// Constructor for dependency injection
        /// </summary>
        /// <param name="upgradeScheduleService"></param>
        /// <param name="environmentProvider"></param>
        /// <param name="kioskAppManagerService"></param>
        /// <param name="fileManagementService"></param>
        /// <param name="kioskHeathService"></param>
        /// <param name="applicationStateService"></param>
        public UpgradeToNewVersionController(
            IUpgradeScheduleService upgradeScheduleService,
            IEnvironmentProvider environmentProvider,
            IKioskAppManagerService kioskAppManagerService,
            IFileManagementService fileManagementService,
            IKioskHealthService kioskHeathService,
            IApplicationStateService applicationStateService
            )
        {
            _upgradeScheduleService = upgradeScheduleService ?? throw new ArgumentNullException(nameof(upgradeScheduleService));
            _environmentProvider = environmentProvider ?? throw new ArgumentNullException(nameof(environmentProvider));
            _kioskAppManagerService = kioskAppManagerService ?? throw new ArgumentNullException(nameof(kioskAppManagerService));
            _fileManagementService = fileManagementService ?? throw new ArgumentNullException(nameof(fileManagementService));
            _kioskHeathService = kioskHeathService ?? throw new ArgumentNullException(nameof(kioskHeathService));
            _applicationStateService = applicationStateService ?? throw new ArgumentNullException(nameof(applicationStateService));
        }

        /// <summary>
        /// Run job
        /// </summary>
        public override void OnTimerTickAction()
        {
            bool installationSucceeded;
            _logger.Debug($"Entered UpgradeToNewVersionController.OnTimerTickAction action.");

            if (_applicationStateService.GetState() != KioskApplicationState.Normal)
            {
                _logger.Info($"Kiosk is not in Normal mode. Skipping this cycle.");

                return;
            }
            var operation = "INSTALLATION";

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            _logger.Info($"[{operation}] Checking there is a version ready for install.");

            InstallationPackageInfo appVersionInfo = _upgradeScheduleService.GetRegisteredVersionToInstall();

            if (appVersionInfo == null)
            {
                _logger.Info($"[{operation}] No version to install or schedule in future.");
                return;
            }

            try
            {
                _logger.Debug($"[{operation}] Setting application state to KioskApplicationState.Upgrading...");
                _applicationStateService.SetState(KioskApplicationState.Upgrading);


                _logger.Info($"[{operation}] Upgrading to new version. Version: {appVersionInfo.Version}. Scheduled On: {appVersionInfo.InstallOn}.");

                string version = appVersionInfo.Version.ToString();
                
                // Lock installation job file and prevent from another job start in parallel
                {
                    _logger.Info($"[{operation}] Lock upgrade job schedule file. Re-schedule next run in 2 cycles.");
                    var nextJobSchedule = appVersionInfo.Clone();
                    nextJobSchedule.InstallOn = appVersionInfo.InstallOn.AddMinutes(TimerInterval.TotalMinutes * 2);
                    _upgradeScheduleService.RegisterInstallationJob(nextJobSchedule);

                    _logger.Info($"[{operation}] Next attempt was scheduled on {nextJobSchedule.InstallOn}");
                }

                _logger.Info($"[{operation}] Creating app configuraion file.");
                _fileManagementService.ApplyTransformationToAppConfigurationFile(version);

                _logger.Info($"[{operation}] Stopping kiosk application.");
                _kioskAppManagerService.StopKioskApp();
                _logger.Info($"[{operation}] Kiosk app has stopped.");

                var kioskLastActivityBeforeInstallation = _kioskHeathService.GetKioskLastActivity().LastActivityUtc ?? DateTime.MinValue;
                _logger.Info($"[{operation}] Read kiosk last activity time before upgrade: UTC time: {kioskLastActivityBeforeInstallation}.");


                _logger.Info($"[{operation}] Backup kiosk application folder.");
                _fileManagementService.BackupKioskApplicationDirectory();

                _logger.Info($"[{operation}] Installing kiosk application package. Version: {version}.");
                _fileManagementService.ReplaceKioskAppFromPackage(version);


                _logger.Info($"[{operation}] Installation is complete. Starting kiosk application. Version: {version}.");
                _kioskAppManagerService.StartKioskApp();
                _logger.Info($"[{operation}] Kiosk application has been started. Version: {version}.");


                _logger.Info($"[{operation}] Begin verification that kiosk application can register on the server.");
                Thread.Sleep(TimeSpan.FromSeconds(60)); // wait while kiosk app will connect and got registered

                try
                {
                    var kioskLastActivityAfterInstallation = _kioskHeathService.GetKioskLastActivity().LastActivityUtc ?? DateTime.MinValue;

                    _logger.Info($"[{operation}][{version}] Read kiosk last activity after upgrade. UTC time: {kioskLastActivityAfterInstallation}.");


                    installationSucceeded = kioskLastActivityAfterInstallation > kioskLastActivityBeforeInstallation;
                    _logger.InfoFormat("[{0}][{1}] Kiosk verification is complete. Result: {2}.", operation, version, installationSucceeded ? "Succeeded" : "Failed");
                }
                catch(System.Net.Sockets.SocketException ex)
                {
                    // ScreenDox server is not available to verify kiosk installation.
                    // Assume the installation was not successful - revert changes

                    _logger.Error($"[{operation}][{version}] ScreenDox Installation API is not available. Cannot verify success. Reverting installation.", ex);
                    installationSucceeded = false;
                }
                catch (Exception ex)
                {
                    _logger.Error($"[{operation}][{version}] Unexpected error has occured during installation validation. Reverting installation.", ex);
                    installationSucceeded = false;
                }

                if (installationSucceeded)
                {
                    // Completing installation

                    _logger.Info($"[{operation}][{version}] Kiosk installation verification succeeded. Removing job from the schedule.");

                    _upgradeScheduleService.FinalizeInstallationJob();

                    stopwatch.Stop();

                    _logger.Info($"[{operation}][SUCCESS][{version}] Upgrade procedure is complete. Duration: {stopwatch.Elapsed.TotalSeconds} seconds. Version: {appVersionInfo.Version}.");

                    return;
                }


                // installation has failed
                _logger.Error($"[{ operation}][{version}] Failed to verify kiosk installation was successful. Begin rollback.");

                // rollback
                Rollback(kioskLastActivityBeforeInstallation, version);

                // disable installation retry
                _upgradeScheduleService.FinalizeInstallationJob();

                // stop timer
                stopwatch.Stop();

                _logger.Info($"[{operation}][FAILED][{version}] Upgrade procedure is complete. Duration: {stopwatch.Elapsed.TotalSeconds} seconds.");
            }
            catch (System.IO.IOException ex)
            {
                _logger.Error($"[{operation}] Failed to write file during application package upgrade. Permissions issues.", ex);
            }
            catch (DownloadPackageException ex)
            {
                _logger.Error($"[{operation}] Failed to download new version.", ex);
            }
            finally
            {
                _applicationStateService.SetState(KioskApplicationState.Normal);
            }
        }


        private void Rollback(DateTime kioskLastActivityBeforeInstallation, string version)
        {
            bool rollbackSucceeded;
            string operation = "[ROLLBACK]";

            _logger.Info($"{operation}[{version}] Restoring previous version from backup.");

            try
            { 
                _logger.Info($"{operation}[{version}] Stopping kiosk application.");
                _kioskAppManagerService.StopKioskApp();
                _logger.Info($"{operation}[{version}] Kiosk app has stopped.");


                _logger.Info($"{operation}[{version}] Begin kiosk application folder rollback from backup.");
                _fileManagementService.RestoreKioskApplicationFolderFromBackup();


                _logger.Info($"{operation}[{version}] Rollback is complete. Starting kiosk application.");
                _kioskAppManagerService.StartKioskApp();
                _logger.Info($"{operation}[{version}] Kiosk application has been started.");


                _logger.Info($"{operation}[{version}] Begin verification kiosk application registered on the server.");
                Thread.Sleep(TimeSpan.FromSeconds(20)); // wait while kiosk app will connect and got registered


                try
                {
                    var kioskLastActivityAfterInstallation = _kioskHeathService.GetKioskLastActivity().LastActivityUtc ?? DateTime.MinValue;

                    _logger.Info($"{operation}[{version}] Read kiosk last activity after upgrade. UTC time: {kioskLastActivityAfterInstallation}.");


                    rollbackSucceeded = kioskLastActivityAfterInstallation > kioskLastActivityBeforeInstallation;

                    _logger.InfoFormat("{0}[{1}] Kiosk verification is complete. Result: {2}.", operation, version, rollbackSucceeded ? "Succeeded" : "Failed");

                    rollbackSucceeded = true;
                }
                catch (System.Net.Sockets.SocketException ex)
                {
                    _logger.ErrorFormat($"{operation}[{version}] ScreenDox Installation API is not available. Cannot verify success.", ex);

                    rollbackSucceeded = false;
                }

                if (rollbackSucceeded)
                {
                    _logger.Info($"{operation}[{version}] Recovered kiosk application connected to the server. Please check error logs to troubleshoot installation errors.");
                }
                else
                {
                    _logger.Fatal($"{operation}[{version}] Kiosk installation recovery from backup has failed. Kiosk has not connected to the server. Please review Kiosk Activity and resolve the issue remotelly.");
                }

                _logger.Info($"{operation}[{version}] Recovery from backup has been completed. Result: {rollbackSucceeded}");

            }
            catch (Exception ex)
            {
                _logger.Fatal($"{operation}[{version}] Unexpected error has occured.", ex);
            }
        }

    }
}

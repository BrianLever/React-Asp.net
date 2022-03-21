using ScreenDoxKioskLauncher.Models;

using System;

namespace ScreenDoxKioskLauncher.Services
{
    /// <summary>
    /// Service for managing installation schedule
    /// </summary>
    public interface IUpgradeScheduleService
    {
        /// <summary>
        /// Verify the version is ready to install
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        bool CheckVersionIsReadyToInstall(Version version);

        /// <summary>
        /// Create a schedule to install new version after successful download
        /// </summary>
        /// <param name="versionToInstall"></param>
        void RegisterInstallationJob(InstallationPackageInfo versionToInstall);

        InstallationPackageInfo GetRegisteredVersionToInstall();

        /// <summary>
        /// Remove upgrade job from the schedule
        /// </summary>
        void FinalizeInstallationJob();
    }
}

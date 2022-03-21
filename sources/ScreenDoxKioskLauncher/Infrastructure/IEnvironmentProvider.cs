using System;

namespace ScreenDoxKioskLauncher.Infrastructure
{
    /// <summary>
    /// Environment variables and applicaion settings
    /// </summary>
    public interface IEnvironmentProvider
    {

        /// <summary>
        /// Kiosk application directory
        /// </summary>
        string KioskApplicationDirectoryPath { get; }

        /// <summary>
        /// Kiosk App file name
        /// </summary>
        string KioskExeName { get; }

        /// <summary>
        /// Path to the remote REST service
        /// </summary>
        string ScreenDoxInstallationServiceBaseUrl { get; }

        /// <summary>
        /// Path to the data file with the current state
        /// </summary>
        string StateFileFullPath { get; }

        string ApplicationDataDirectoryPath { get; }

        /// <summary>
        /// Path to the file that stores upgrade job instructions
        /// </summary>
        string JobScheduleFileFullPath { get; }

        /// <summary>
        /// Path to the folder with installation packages
        /// </summary>
        string PackagesRootDirectoryPath{ get; }

        /// <summary>
        /// Interval for checking new version on the server
        /// </summary>
        TimeSpan CheckUpgradeTimeInterval { get; }
        /// <summary>
        /// Unique kiosk key
        /// </summary>
        string KioskKey { get; }

        /// <summary>
        /// Kiosk Security Token
        /// </summary>
        string KioskSecret { get; }

        /// <summary>
        /// Get time interval in minutes from scheduled time to start installation
        /// </summary>
        int InstallationTimeIntervalInMinutes { get; }
        string ApplicationBackupDirectoryPath { get; }
        string ScreendoxServerBaseUrl { get; }
        
        /// <summary>
        /// Timer interval in seconds for checking kiosk app is running
        /// </summary>
        int KioskAppAutoStartIntervalInSeconds { get; }
    }
}
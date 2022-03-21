using Common.Logging;

using FrontDesk.Common.Configuration;

using ScreenDox.Kiosk.Configuration;

using System;
using System.IO;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace ScreenDoxKioskLauncher.Infrastructure
{

    /// <summary>
    /// Implementation of IEnvironmentProvider interface
    /// </summary>
    public class EnvironmentProvider : AppSettingsProxy, IEnvironmentProvider
    {
        private readonly ILog _logger = LogManager.GetLogger<EnvironmentProvider>();

        private readonly IAppConfigurationRepository _appConfigurationRepository = new YamlAppConfigurationRepository();

        private Lazy<AppConfiguration> _appConfiguration;

        public EnvironmentProvider()
        {
            _appConfiguration = new Lazy<AppConfiguration>(
            () => _appConfigurationRepository.Read(AppConfigurationFileFullPath));
        }

        /// <summary>
        /// Get full path to application data folder
        /// </summary>
        public string ApplicationDataDirectory
        {
            get
            {
                var filepath = GetStringValue("ApplicationDataDirectory", "Data");
                var currentDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

                return Path.Combine(currentDirectory, filepath);
            }
        }

        public string StateFileFullPath
        {
            get
            {
                var fileName = GetStringValue("StateFileName", @"version.yaml");

                return Path.Combine(ApplicationDataDirectoryPath, fileName);
            }
        }

        /// <summary>
        /// Path to the file that stores upgrade job instructions. Default location is in Packages root folder
        /// </summary>
        public virtual string JobScheduleFileFullPath
        {
            get
            {
                var filepath = GetStringValue("JobScheduleFileName", @"upgrade-job.yaml");

                return Path.Combine(ApplicationDataDirectoryPath, filepath);
            }
        }

        /// <summary>
        /// Path to the file that stores upgrade job instructions
        /// </summary>
        public virtual string AppConfigurationFileFullPath
        {
            get
            {
                var filepath = GetStringValue("AppConfigurationFileName", @"configuration.yaml");

                return Path.Combine(ApplicationDataDirectory, filepath);
            }
        }

        /// <summary>
        /// Directory name that contains all downloaded packages (e.g. ScreenDox/packages)
        /// </summary>
        public virtual string PackagesDirectoryName
        {
            get
            {
                return GetStringValue("PackagesDirectoryName", ApplicationDataRootName + @"\packages");
            }
        }

        public virtual string ApplicationDataRootName
        {
            get
            {
                return GetStringValue("ApplicationDataRootPath", @"ScreenDox");
            }
        }

        /// <summary>
        /// File to the root folder with distributives
        /// </summary>
        public virtual string PackagesRootDirectoryPath
        {
            get
            {
                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                    PackagesDirectoryName);
            }
        }

        /// <summary>
        /// Path to Root folder where application data is located. By Default: C:\ProgramData\ScreenDox
        /// </summary>
        public virtual string ApplicationDataDirectoryPath
        {
            get
            {
                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                            ApplicationDataRootName);
            }
        }

        /// <summary>
        /// Interval for checking new version on the server
        /// </summary>
        public TimeSpan CheckUpgradeTimeInterval
        {
            get
            {
                TimeSpan result;

                var intervalString = GetStringValue("CheckNewVersionInterval", "0:20:00"); // by default, every hour
                try
                {
                    result = TimeSpan.Parse(intervalString);
                }
                catch (FormatException ex)
                {
                    _logger.WarnFormat("Invalid settings value for {0}. Value: {1}.", ex, "CheckNewVersionInterval", intervalString);

                    result = TimeSpan.FromHours(1);
                }

                return result;
            }
        }



        /// YAML file configuration


        public string KioskApplicationDirectoryPath
        {
            get
            {
                return Path.GetFullPath(_appConfiguration.Value.KioskApplicationDirectory);
            }
        }

        public string KioskExeName
        {
            get
            {
                return _appConfiguration.Value.KioskExeName;
            }
        }

        /// <summary>
        /// Path to ScreenDox Server base URL
        /// </summary>
        public string ScreenDoxInstallationServiceBaseUrl
        {
            get
            {
                return _appConfiguration.Value.ScreenDoxInstallationServiceBaseUrl;
            }
        }


        public string KioskKey
        {
            get
            {
                return _appConfiguration.Value.Key;
            }
        }


        public string KioskSecret
        {
            get
            {
                return _appConfiguration.Value.Secret;
            }
        }

        public virtual int InstallationTimeIntervalInMinutes
        {
            get
            {
                return GetIntValue("InstallationTimeIntervalInMinutes", 25);
            }
        }

        /// <summary>
        /// Backup folder to keep the copy files for reo
        /// </summary>

        public string ApplicationBackupDirectoryPath
        {
            get
            {
                return Path.Combine(PackagesRootDirectoryPath, "recovery-backup");
            }
        }

        public string ScreendoxServerBaseUrl
        {
            get
            {
                return _appConfiguration.Value.ScreendoxServerBaseUrl;
            }
        }


        public int KioskAppAutoStartIntervalInSeconds
        {
            get
            {
                return GetIntValue("KioskAppAutoStartIntervalInSeconds", 60);
            }
        }
    }
}

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

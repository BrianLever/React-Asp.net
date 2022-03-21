using Common.Logging;

using FrontDesk.Common.InfrastructureServices;

using ScreenDoxKioskLauncher.Infrastructure;
using ScreenDoxKioskLauncher.Models;

using System;
using System.IO;
using System.Text;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ScreenDoxKioskLauncher.Services
{
    /// <summary>
    /// Default implementation of IUpgradeScheduleService
    /// </summary>
    public class UpgradeScheduleService : IUpgradeScheduleService
    {
        private readonly IEnvironmentProvider _environmentProvider;
        private readonly ITimeService _timeService;
        private ILog _logger = LogManager.GetLogger<UpgradeScheduleService>();

        /// <summary>
        /// Default contructor
        /// </summary>
        public UpgradeScheduleService(IEnvironmentProvider environmentProvider, ITimeService timeService)
        {
            _environmentProvider = environmentProvider ?? throw new ArgumentNullException(nameof(environmentProvider));
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
        }

        /// <summary>
        /// Default contructor
        /// </summary>
        internal UpgradeScheduleService(): this(new EnvironmentProvider(), new TimeService())
        {

        }

        private InstallationPackageInfo GetJobSchedule()
        {
            InstallationPackageInfo result = null;

            var filePath = _environmentProvider.JobScheduleFileFullPath;

            if(!File.Exists(filePath))
            {
                _logger.Warn($"File with job schedule does not exists. File path: {filePath}");

                return null;
            }


            var serializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            using (var sr = File.OpenText(filePath))
            {
                result = serializer.Deserialize<InstallationPackageInfo>(sr);
            }

            return result;
        }

        private void SaveJobSchedule(InstallationPackageInfo model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var filePath = _environmentProvider.JobScheduleFileFullPath;


            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            using (var sw = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                serializer.Serialize(sw, new {
                    Version = model.Version?.ToString(),
                    InstallOn = model.InstallOn
                });
            }

        }

        private void SaveApplicationState(ApplicationState model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var filePath = _environmentProvider.StateFileFullPath;


            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            try
            {
                using (var sw = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    serializer.Serialize(sw, model);
                }
            }
            catch(IOException ex)
            {
                _logger.ErrorFormat("Failed to write application state. File: {0}", ex);
            }

        }
        /// <summary>
        /// Returns true if given version is registered for future upgrade job
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public bool CheckVersionIsReadyToInstall(Version version)
        {
            var versionInSchedule = GetJobSchedule()?.Version;

            return versionInSchedule == version;
        }

        /// <summary>
        /// Update upgrade job schedule
        /// </summary>
        /// <param name="versionToInstall"></param>
        public void RegisterInstallationJob(InstallationPackageInfo versionToInstall)
        {
            if (versionToInstall == null)
            {
                // delete file
                var filePath = _environmentProvider.JobScheduleFileFullPath;
                File.WriteAllText(filePath, string.Empty);

                _logger.Info("Cleaned up file with scheduled job details.");

                return;
            }

            SaveJobSchedule(versionToInstall);
        }

        /// <summary>
        /// Check if there is active schedule and schedule time within certain minutes
        /// </summary>
        /// <returns></returns>
        public InstallationPackageInfo GetRegisteredVersionToInstall()
        {
            var versionInSchedule = GetJobSchedule();

            if(versionInSchedule == null)
            {
                return null;
            }

            var scheduledTime = versionInSchedule.InstallOn;
            var currentTime = _timeService.GetLocalNow();

            var timeLeft = (scheduledTime - currentTime).TotalMinutes;

            if(timeLeft > 0 && timeLeft > _environmentProvider.InstallationTimeIntervalInMinutes)
            {
                _logger.Info($"Waiting for the package to install. Schedule time: {scheduledTime}. Current time: {currentTime}.");
                return null;
            }

            return versionInSchedule;

        }


        /// <summary>
        /// Finlize the installation and update the application state with current version and remove installation schedule
        /// </summary>
        public void FinalizeInstallationJob()
        {
            // set the current version to the installed one
            // clean up schedule file

            var schedule = GetRegisteredVersionToInstall();

            if(schedule == null)
            {
                _logger.Warn("Schedule model is empty but expected to have the installed version number.");
                return;
            }
            ApplicationState model = new ApplicationState
            {
                Version = schedule.Version.ToString()
            };

            SaveApplicationState(model);
            _logger.Info($"Set app version. Version: {schedule.Version}");

            RegisterInstallationJob(null);

            _logger.Info($"Removed installation schedule. Version: {schedule.Version}");

        }
    }
}

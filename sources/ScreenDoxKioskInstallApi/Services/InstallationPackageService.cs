using Common.Logging;
using NSwag.Annotations;
using ScreenDoxKioskInstallApi.Infrastructure;
using ScreenDoxKioskInstallApi.Models;

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ScreenDoxKioskInstallApi.Services
{
    public class InstallationPackageService
    {
        private readonly ILog _logger = LogManager.GetLogger<InstallationPackageService>();
        private readonly IAppSettingsProvider _appSettings;
        private string ConfigFileName = "kiosk-installation-config.yaml";

        public InstallationPackageService(IAppSettingsProvider appSettings)
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public InstallationPackageService() : this(new AppSettingsProvider())
        {

        }

        /// <summary>
        /// Return installation package info where Kiosk installation instructions are filtered by kioskKey id
        /// </summary>
        /// <param name="kioskKey"></param>
        /// <returns>Returns InstallationPackageInfo as result, where InstallOn is overriten by values from Kiosk</returns>
        [OpenApiOperation("Get current version and installation time for specific kiosk")]
        public InstallationPackageInfo Get(string kioskKey)
        {
            InstallationPackageInfo result = Get();

            if (result == null) return null;

            var kioskInstructions = result.Kiosks.FirstOrDefault(x => string.Compare(x.Key, kioskKey, true) == 0);
            if (kioskInstructions != null)
            {
                result.InstallOn = kioskInstructions.InstallOn;
            }

            result.Kiosks = null;

            return result;
        }


        public InstallationPackageInfo Get()
        {
            InstallationPackageInfo result;

            var configFilePath = Path.Combine(_appSettings.KioskInstallationDirectoryRoot, ConfigFileName);

            if (!File.Exists(configFilePath))
            {
                throw new FileNotFoundException($"Configuration file ${configFilePath} not found");
            }

            var serializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .IgnoreUnmatchedProperties()
                .Build();
            try
            {
                using (var sr = File.OpenText(configFilePath))
                {
                    result = serializer.Deserialize<InstallationPackageInfo>(sr);
                }
            }
            catch(Exception ex)
            {
                _logger.Error("Failed to deserialize YAML configuration file.", ex);
                throw;
            }

            return result;
        }

        /// <summary>
        /// Get installation file for download
        /// </summary>
        /// <param name="version">package version (e.g. 8.0.0.0)</param>
        /// <returns></returns>
        public InstallationPackageFile GetFile(string version)
        {
            // get all zip files in the folder
            var files = Directory.GetFiles(_appSettings.KioskInstallationDirectoryRoot, "*.zip");

            // find the file with specific version
            Regex re = new Regex($".*-{version}.zip", RegexOptions.IgnoreCase);

            var filename = files.FirstOrDefault(x => re.IsMatch(x));

            if (string.IsNullOrEmpty(filename))
            {
                return null;
            }

            InstallationPackageFile result = new InstallationPackageFile
            {
                FileName = Path.GetFileName(filename),
                Content = File.OpenRead(filename)
            };

            return result;
        }
    }
}
using Common.Logging;

using ICSharpCode.SharpZipLib.Zip;

using ScreenDoxKioskInstallApi.Models;

using ScreenDoxKioskLauncher.Infrastructure;
using ScreenDoxKioskLauncher.Models;

using System;
using System.IO;
using System.Linq;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ScreenDoxKioskLauncher.Services
{
    /// <summary>
    /// Services communicates with remote Server and downloads new installation packages if any
    /// </summary>
    public class PackageDownloadService : IPackageDownloadService
    {
        private readonly IEnvironmentProvider _environmentProvider;
        private readonly IKioskInstallApiClient _client;

        private ILog _logger = LogManager.GetLogger<PackageDownloadService>();


        /// <summary>
        /// Default contructor with all depedencies
        /// </summary>
        /// <param name="environmentProvider"></param>
        /// <param name="client"></param>
        public PackageDownloadService(IEnvironmentProvider environmentProvider, IKioskInstallApiClient client)
        {
            _environmentProvider = environmentProvider ?? throw new ArgumentNullException(nameof(environmentProvider));
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }


        /// <summary>
        /// Get local kiosk app version
        /// </summary>
        /// <returns></returns>
        public Version GetCurrentVersion()
        {
            ApplicationState model = null;

            if(!File.Exists(_environmentProvider.StateFileFullPath))
            {
                return new Version();
            }

            var serializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            using (var sr = System.IO.File.OpenText(_environmentProvider.StateFileFullPath))
            {
                model = serializer.Deserialize<ApplicationState>(sr);
            }

            return model?.GetVersion();
        }

        /// <summary>
        /// Check if there is new version on the server
        /// </summary>
        /// <returns>
        /// Returns InstallationPackageInfo object with the version and installation time if server has new version.
        /// Otherwise returns null.
        /// </returns>
        public InstallationPackageInfo CheckNewVersionAvailable()
        {
            var packageOnServer = _client.GetAvailableVersionInfo();

            if (packageOnServer == null || packageOnServer?.Version == null)
            {
                _logger.Warn("CheckNewVersionAvailable returned empty result from server.");
                return null;
            }

            var versionOnServer = packageOnServer.Version;
            var localVersion = GetCurrentVersion();

            // if version has been installed, return null
            if (versionOnServer <= localVersion)
            {
                return null;
            }
            _logger.InfoFormat("New installation package found on the server: {0}. Current version: {1}",
                versionOnServer, localVersion);

            return packageOnServer;
        }

        /// <summary>
        /// Download installation package of certain version and extract into target directory
        /// </summary>
        /// <param name="version">version to download</param>
        /// <returns></returns>
        public void DownloadPackage(Version version)
        {
            FileContent file = _client.DownloadPackage(version);

            if (file == null || file.Content == null || file.Content.Length == 0 || string.IsNullOrWhiteSpace(file.FileName))
            {
                throw new DownloadPackageException($"Failed to download installation file. Invalid payload received. Name: {file?.FileName}. Length: {file?.Content?.Length}");
            }

            _logger.Info($"Downloaded installation file. Name: {file.FileName}. Length: {file.Content.Length}");

            //save file to the package folder
            string packageRootDirectory = _environmentProvider.PackagesRootDirectoryPath;
            
            // replacing extention to zip and ignoring other path as additional security rule
            string zipFileName = Path.ChangeExtension(Path.GetFileName(file.FileName), "zip");

            var destinationZipFile = Path.Combine(packageRootDirectory, zipFileName);

            //create target directory if not exists
            try
            {
                Directory.CreateDirectory(packageRootDirectory);
            }
            catch (IOException ex)
            {
                throw new DownloadPackageException($"Failed to create package root directory. Directory:{packageRootDirectory}", ex);
            }

            // saving zip file to destination folder for troubleshooting
            try
            {
                System.IO.File.WriteAllBytes(destinationZipFile, file.Content);
            }
            catch (IOException ex)
            {
                throw new DownloadPackageException($"Failed to write downloaded file into package directory. Target path: {destinationZipFile}", ex);
            }

            // extract zip to folder
            var destinationExtractDirectory = Path.Combine(packageRootDirectory, version.ToString());

            _logger.DebugFormat("Extracting zip file \"{0}\" to target directory \"{1}\"",
                destinationZipFile, destinationExtractDirectory);

            FastZip fastZip = new FastZip();
            string fileFilter = null;
            fastZip.ExtractZip(destinationZipFile, destinationExtractDirectory, fileFilter);

            //validate target folder exists
            if(!Directory.Exists(destinationExtractDirectory))
            {
                throw new DownloadPackageException($"Failed to extract the package. Target directory not created. Directory path: {destinationExtractDirectory}");
            }

            //validate target folder has any file
            if (!Directory.EnumerateFiles(destinationExtractDirectory).Any())
            {
                throw new DownloadPackageException($"Failed to extract the package. Target directory is empty. Directory path: {destinationExtractDirectory}");
            }
        }
    }
}

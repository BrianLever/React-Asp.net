using Common.Logging;

using FrontDesk.Common.Configuration;

using ScreenDoxKioskLauncher.Infrastructure;

using System;
using System.Collections.Generic;
using System.IO;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace ScreenDoxKioskLauncher.Services
{
    /// <summary>
    /// Incorporates logic working with application and installation files
    /// </summary>
    public class FileManagementService : IFileManagementService
    {
        private ILog _logger = LogManager.GetLogger<FileManagementService>();
        private readonly IEnvironmentProvider _environmentProvider;

        private const string AppConfigFileTemplateName = "app_setup.config";

        /// <summary>
        /// Default constructor
        /// </summary>
        public FileManagementService(IEnvironmentProvider environmentProvider)
        {
            _environmentProvider = environmentProvider ?? throw new ArgumentNullException(nameof(environmentProvider));
        }
        /// <summary>
        /// Create app configuration file from template and environment variables
        /// </summary>
        /// <param name="version">Version of the installation package</param>
        public void ApplyTransformationToAppConfigurationFile(string version)
        {
            _logger.Info($"Begin kiosk configuration file parametrization.  Version: {version}.");


            var pathToAppConfigTemplate = Path.Combine(_environmentProvider.PackagesRootDirectoryPath, version, AppConfigFileTemplateName);
            string kioskAppConfigFileName = string.Concat(_environmentProvider.KioskExeName, ".config");
            var pathToTargetAppConfigFile = Path.Combine(_environmentProvider.PackagesRootDirectoryPath, version, kioskAppConfigFileName);

            if (!File.Exists(pathToAppConfigTemplate))
            {
                throw new Exception($"Failed to find application configuration file to replace. Path: {pathToAppConfigTemplate}");
            }

            // replace "KIOSK_KEY", "KIOSK_SECRET", "{SERVICE_ADDRESS}", "{VERSION}"

            Dictionary<string, string> paramaters = new Dictionary<string, string>
            {
                {"KIOSK_KEY", _environmentProvider.KioskKey},
                {"KIOSK_SECRET", _environmentProvider.KioskSecret},
                {"{SERVICE_ADDRESS}", ScreenDoxServerConfigurationHelper
                    .GetFqdnKioskEndpointAddress(_environmentProvider.ScreendoxServerBaseUrl)},
                {"{VERSION}", version }
            };

            // replacing parameters in file and writing to target app config file
            using (FileStream inputStream = File.OpenRead(pathToAppConfigTemplate))
            {
                StreamReader inputReader = new StreamReader(inputStream);

                using (StreamWriter outputWriter = File.CreateText(pathToTargetAppConfigFile))
                {
                    string tempLineValue;

                    while (null != (tempLineValue = inputReader.ReadLine()))
                    {
                        foreach (var param in paramaters)
                        {
                            tempLineValue = tempLineValue.Replace(param.Key, param.Value);
                        }
                        outputWriter.WriteLine(tempLineValue);
                    }
                }

            }

            _logger.Info($"Completed kiosk configuration file parametrization.  Version: {version}.");
        }


        public void BackupKioskApplicationDirectory()
        {
            var targetDirectory = _environmentProvider.ApplicationBackupDirectoryPath;
            var sourceDirectory = _environmentProvider.KioskApplicationDirectoryPath;

            DeleteAllFilesInDirectory(targetDirectory);

            DirectoryCopy(sourceDirectory, targetDirectory, true);


            _logger.Info($"Backup kiosk application files is complete. Backup folder: {targetDirectory}. Kiosk app folder: {sourceDirectory}");
        }


        public void RestoreKioskApplicationFolderFromBackup()
        {
            var sourceDirectory = _environmentProvider.ApplicationBackupDirectoryPath;
            var targetDirectory = _environmentProvider.KioskApplicationDirectoryPath;

            _logger.Info($"[ROLLBACK] Restoring kiosk files from backup. Backup folder: {sourceDirectory}. Kiosk app folder: {targetDirectory}");

            DeleteAllFilesInDirectory(targetDirectory);

            DirectoryCopy(sourceDirectory, targetDirectory, true);

            _logger.Info($"[ROLLBACK] Backup kiosk application files is complete. Backup folder: {targetDirectory}.");
        }

        public void ReplaceKioskAppFromPackage(string version)
        {
            var targetDirectrory = _environmentProvider.KioskApplicationDirectoryPath;
            var sourceDirectory = Path.Combine(_environmentProvider.PackagesRootDirectoryPath, version);

            _logger.Info($"Replacing kiosk app files from installation package. Source: {sourceDirectory}. Target: {targetDirectrory}");

            // clean up folder but keep the folder because of permissions
            DeleteAllFilesInDirectory(targetDirectrory);

            DirectoryCopy(sourceDirectory, targetDirectrory, true);

            _logger.Info($"Kiosk app files replaced from the installation package. Version: {version}.");
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException($"Source directory does not exist or could not be found: {sourceDirName}");
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        /// <summary>
        /// Clean up directory. If directory not exists, creates a new one
        /// </summary>
        /// <param name="directoryPath"></param>
        private static void DeleteAllFilesInDirectory(string directoryPath)
        {
            var directory = new DirectoryInfo(directoryPath);
            if (!directory.Exists)
            {
                directory.Create();
                return;
            }

            foreach (FileInfo file in directory.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in directory.GetDirectories())
            {
                dir.Delete(true);
            }
        }
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
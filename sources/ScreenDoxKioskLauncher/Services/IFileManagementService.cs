#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace ScreenDoxKioskLauncher.Services
{
    public interface IFileManagementService

    {
        void ApplyTransformationToAppConfigurationFile(string version);
        void BackupKioskApplicationDirectory();
        void ReplaceKioskAppFromPackage(string version);
        void RestoreKioskApplicationFolderFromBackup();
    }
}

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
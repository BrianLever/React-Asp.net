using System;
using ScreenDoxKioskLauncher.Models;

namespace ScreenDoxKioskLauncher.Services
{
    public interface IPackageDownloadService
    {
        InstallationPackageInfo CheckNewVersionAvailable();
        void DownloadPackage(Version version);
        Version GetCurrentVersion();
    }
}
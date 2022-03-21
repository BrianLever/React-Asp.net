using ScreenDoxKioskInstallApi.Models;

using ScreenDoxKioskLauncher.Models;

using System;

namespace ScreenDoxKioskLauncher.Infrastructure
{
    public interface IKioskInstallApiClient
    {
        InstallationPackageInfo GetAvailableVersionInfo();
        FileContent DownloadPackage(Version version);

        KioskLastActivity GeTimeSinceLastKioskActivity();
    }
}

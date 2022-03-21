using ScreenDoxKioskInstallApi.Models;
using ScreenDoxKioskLauncher.Models;
using System;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace ScreenDoxKioskLauncher.Infrastructure
{
    public class KioskInstallApiClient : HttpRestClient, IKioskInstallApiClient
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="environmentProvider"></param>
        public KioskInstallApiClient(IEnvironmentProvider environmentProvider) : base(environmentProvider)
        {
        }

        public FileContent DownloadPackage(Version version)
        {
            FileContent file = GetFile($"api/version/{version.ToString()}");

            return file;
        }

        public InstallationPackageInfo GetAvailableVersionInfo()

        {
            return GetResult<InstallationPackageInfo>("api/version");
        }


        public KioskLastActivity GeTimeSinceLastKioskActivity()

        {
            return GetResult<KioskLastActivity>("api/lastactivity");
        }
    }
}

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

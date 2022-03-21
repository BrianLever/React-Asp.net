using ScreenDox.Server.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScreenDoxKioskInstallApi.Infrastructure
{
    public class AppSettingsProvider : IAppSettingsProvider
    {
        public string KioskInstallationDirectoryRoot
        {
            get
            {
                return SystemSettings.GetStringValue("KioskInstallationDirectoryRoot", @"c:\ScreenDox\KioskInstallation");
            }
        }
    }
}
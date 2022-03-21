using ScreenDox.Server.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Configuration
{
    /// <summary>
    /// Helper class to read AppSettings from the configuration file
    /// </summary>
    public class AppSettings : FrontDesk.Common.Configuration.AppSettingsProxy
    {
        /// <summary>
        /// Application name
        /// </summary>
        /// <remarks>Default value 10 seconds</remarks>
        public static string ApplicationName
        {
            get
            {
                return GetStringValue("ApplicationName", "ScreenDox - Health Behavioral Screener");
            }
        }

        public static int PasswordRenewalPeriodDays
        {
            get
            {
                return SystemSettings.GetIntValue("PasswordRenewalPeriodDays", 30);
            }
        }

        public static int ExportedSecurityReportMaximumLength
        {
            get
            {
                return SystemSettings.GetIntValue("ExportedSecurityReportMaximumLength", 5000);
            }
        }

        public static int RPMSCredentialsExpirationNotificationInDays
        {
            get
            {
                return SystemSettings.GetIntValue("RPMSCredentialsExpirationNotificationInDays", 7);
            }
        }

        /// <summary>
        /// Path to ELK central logging
        /// </summary>
        public static string CentralLoggingUrl
        {
            get
            {
                return GetStringValue("CentralLoggingUrl", string.Empty);
            }
        }

    }
}

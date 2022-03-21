using ScreenDox.Server.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScreenDox.Server.Api.Models
{
    /// <summary>
    /// System state information
    /// </summary>
    public class SystemInfoResponse
    {
        /// <summary>
        /// App version
        /// </summary>
        public string AppVersion { get; set; }

        /// <summary>
        /// True if applcation has active license. Otherwise returns has False value.
        /// </summary>
        public bool HasActiveLicense { get; set; }
        /// <summary>
        /// Active license details
        /// </summary>
        public LicenseResponse License { get; set; }
        /// <summary>
        /// System Summary info
        /// </summary>
        public SystemSummary Summary { get; set; }


        public string CentralLoggingUrl { get; set; }

        /// <summary>
        /// When True, EHR credentials are going to be expired soon and need to show alert message from <para>EhrCredentialsExpirationAlertMessage</para> 
        /// </summary>
        public bool IsEhrCredentialsExpirationAlert { get; set; }
        /// <summary>
        /// When <para>IsEhrCredentialsExpirationAlert</para> is True, this property include Alert message. Otherwise it's empty string.
        /// </summary>
        public string EhrCredentialsExpirationAlertMessage { get; set; }
    }
}
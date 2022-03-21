using FrontDesk.Server.Licensing.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScreenDox.Server.Api.Models
{
    /// <summary>
    /// License info
    /// </summary>
    public class LicenseResponse
    {
        /// <summary>
        /// License string
        /// </summary>
        public string LicenseString { get; set; }
        /// <summary>
        /// Max kiosks allowed
        /// </summary>
        public int MaxKiosk { get; set; }
        /// <summary>
        /// Max branch locations allowed
        /// </summary>
        public int MaxBranchLocations { get; set; }
        /// <summary>
        /// Expiration date
        /// </summary>
        public DateTime? ExpirationDate { get; set; }
    }
}
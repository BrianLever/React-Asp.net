using FrontDesk.Common.Extensions;

using System;

namespace ScreenDox.Server.Models.ViewModels
{
    /// <summary>
    /// View model for LicenseCertificate
    /// </summary>
    public class LicenseCertificateResponse
    {
        public string LicenseString { get; set; }
        /// <summary>
        /// Created date
        /// </summary>
        public DateTimeOffset RegisteredDate { get; set; }

        public string RegisteredDateLabel { get { return RegisteredDate.FormatAsDate(); } }

        public int MaxBranchLocations { get; set; }

        public int MaxKiosks { get; set; }

        public int DurationInYears { get; set; }

        public bool IsLicenseExpired { get; set; }

        public DateTimeOffset? ActivatedDate { get; set; }
        /// <summary>
        /// Formatted date string for ActivatedDate
        /// </summary>
        public string ActivatedDateLabel { get { return ActivatedDate?.FormatAsDate() ?? string.Empty; } }

        public DateTimeOffset? ExpirationDate { get; set; }
        /// <summary>
        /// Formatted date string for ExpirationDate
        /// </summary>
        public string ExpirationDateLabel
        {
            get { return ExpirationDate?.FormatAsDate() ?? string.Empty; }

        }
    }
}

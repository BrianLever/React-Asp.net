using FrontDesk.Common.Extensions;

using System;

namespace ScreenDox.Server.Models
{
    /// <summary>
    /// Error Log Entry
    /// </summary>
    public class ErrorLogItem
    {
        /// <summary>
        /// ID
        /// </summary>
        public long ErrorLogID { get; set; }
        /// <summary>
        /// Kiosk ID (nullable)
        /// </summary>
        public short? KioskID { get; set; }
        /// <summary>
        /// Kiosk label
        /// </summary>
        public string KioskLabel { get; set; }
        /// <summary>
        /// Error message
        /// </summary>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// Error trace log text
        /// </summary>
        public string ErrorTraceLog { get; set; }
        /// <summary>
        /// Created date
        /// </summary>
        public DateTimeOffset CreatedDate { get; set; }

        /// <summary>
        /// Formatted created date
        /// </summary>
        public string CreatedDateFormatted { get { return CreatedDate.FormatAsDateWithTime(); } }


    }
}

using System;

namespace ScreenDox.Server.Models
{
    public class UniquePatientScreenViewModel
    {
        public string PatientName { get; set; }
        public long? ScreeningResultID { get; set; }
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// Last Check-in date
        /// </summary>
        public DateTimeOffset? LastCheckinDate { get; set; }

        /// <summary>
        /// Formatted value of LastCheckinDate field
        /// </summary>
        public string LastCheckinDateLabel { get; set; }
        /// <summary>
        /// True when there are not exported patient screening records. Othwerwise it's false.
        /// </summary>
        public bool? HasExport { get; set; }

        /// <summary>
        /// Exported date
        /// </summary>
        public DateTimeOffset? ExportDate { get; set; }
        /// <summary>
        /// Formatted value of Export Date field
        /// </summary>
        public string ExportDateLabel { get; set; }
    }
}

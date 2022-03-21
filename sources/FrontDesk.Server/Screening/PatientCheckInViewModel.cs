using System;

namespace FrontDesk.Server.Screening
{
    public class PatientCheckInViewModel
    {
        public Int64 ScreeningResultID { get; set; }
        public string ScreeningName { get; set; } = "Behavioral Health Screening";
        public DateTimeOffset CreatedDate { get; set; }

        /// <summary>
        /// Formatted date value to render on UI
        /// </summary>
        public string CreatedDateLabel { get; set; }
        public bool IsPositive { get; set; }
        public DateTimeOffset? ExportDate { get; set; }
        /// <summary>
        /// Formatted date value to render on UI
        /// </summary>
        public string ExportDateLabel { get; set; }
        public string ExportedToHRN { get; set; }
        public bool HasAnyScreening { get; set; }
        public bool HasAddress { get; set; }

        /// <summary>
        /// True when result can be exported
        /// </summary>
        public bool ShowBeginExportButton
        {
            get
            {
                return (HasAddress || HasAnyScreening) && !ExportDate.HasValue;
            }
        }

        #region Labels
       
        public string IsPositiveCssClass
        {
            get
            {
                return IsPositive ? "risk" : "";
            }
        }

        #endregion

    }
}

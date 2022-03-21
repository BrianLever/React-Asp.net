using FrontDesk.Server.Extensions;
using ScreenDox.Server.Models;

using System;

namespace ScreenDox.Server.Models.ViewModels
{
    public class ColumbiaSuicideReportSearchResponse
    {
        /// <summary>
        /// Columbia report ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// Related Screening Report ID
        /// </summary>
        public long? ScreeningResultID { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public DateTimeOffset? CompletedDate { get; set; }

        public bool HasAddress { get; set; }

        public string LocationName { get; set; }

        public string StaffNameCompleted { get; set; }


        public virtual string ReportName
        {
            get
            {
                return "C-SSRS Lifetime/Recent Report";
            }
        }

        #region Labels

        public string CreatedDateLabel
        {
            get
            {
                return CreatedDate.FormatAsDateWithTimeWithoutTimeZone();
            }
        }

        public string CompleteDateLabel
        {
            get
            {
                return CompletedDate.FormatAsDateWithTimeWithoutTimeZone();
            }
        }

        #endregion

    }
}
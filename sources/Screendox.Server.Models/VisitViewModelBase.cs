using FrontDesk.Server.Extensions;

using System;

namespace ScreenDox.Server.Models
{
    public class VisitViewModelBase
    {
        public long? ID { get; set; }
        public Int64 ScreeningResultID { get; set; }
        
        public DateTimeOffset ScreeningDate { get; set; }
        
        public DateTimeOffset CreatedDate { get; set; }
        
        public DateTimeOffset? CompletedDate { get; set; }

        public bool HasAddress { get; set; }

        public bool IsVisitRecordType { get; set; }
        public string LocationName { get; set; }


        public virtual string ReportName
        {
            get
            {
                return IsVisitRecordType ? "Visit Report" : "Patient Demographics Report";
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

        public string ScreeningDateLabel
        {
            get
            {
                return ScreeningDate.FormatAsDateWithTimeWithoutTimeZone();
            }
        }

        public string IsDemographicCssClass
        {
            get
            {
                return IsVisitRecordType ? string.Empty : "demographic-report " + (CompletedDate.HasValue ? String.Empty : "incomplete");
            }

            #endregion

        }
    }
}

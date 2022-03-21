using FrontDesk.Common.Extensions;
using FrontDesk.Server.Descriptors;
using FrontDesk.Server.Extensions;

using System;

namespace FrontDesk.Server.Screening
{
    public class BhsFollowUpViewModel
    {
        public Int64 ID { get; set; }
        public Int64 ScreeningResultID { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset VisitDate { get; set; }
        public DateTimeOffset? CompletedDate { get; set; }
        public DateTimeOffset FollowUpDate { get; internal set; }

        public string CompleteDateLabel
        {
            get
            {
                return CompletedDate.FormatAsDateWithTimeWithoutTimeZone();
            }
        }

        public string ScheduledVisitDateLabel
        {
            get
            {
                return VisitDate.FormatAsDate();
            }
        }

        public string FollowUpDateLabel
        {
            get
            {
                return FollowUpDate.FormatAsDate();
            }
        }


        public string DetailsPageUrl
        {
            get
            {
                return string.Format(BhsWebPagesDescriptor.FollowUpPageUrlTemplate, ID);
            }
        }

        public string ReportName
        {
            get
            {
                return "Follow-Up Report";
            }
        }

        public string Status
        {
            get
            {
                return CompletedDate.HasValue ? "Completed" : "Created";
            }
        }

        
    }
}
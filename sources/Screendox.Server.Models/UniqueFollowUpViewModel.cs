using System;

namespace ScreenDox.Server.Models
{
    /// <summary>
    /// View model for Follow-up top level list.
    /// </summary>
    public class UniqueFollowUpViewModel
    {
        public string PatientName { get; set; }
        public long? ScreeningResultID { get; set; }
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// VISIT DATE	
        /// </summary>
        public DateTimeOffset? LastVisitDate { get; set; }

        /// <summary>
        /// VISIT DATE formatted
        /// </summary>
        public string LastVisitDateLabel { get; set; }
        
        /// <summary>
        /// FOLLOW-UP DATE
        /// </summary>
        public DateTimeOffset? LastFollowUpDate { get; set; }

        /// <summary>
        /// FOLLOW-UP DATE formatted
        /// </summary>
        public string LastFollowUpDateLabel { get; set; }
        /// <summary>
        /// FILE COMPLETION DATE
        /// </summary>
        public DateTimeOffset? LastCompleteDate { get; set; }
        /// <summary>
        /// FILE COMPLETION DATE formatted
        /// </summary>
        public string LastCompleteDateLabel { get; set; }
    }
}

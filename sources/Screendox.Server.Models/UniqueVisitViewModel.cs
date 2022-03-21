using System;

namespace ScreenDox.Server.Models
{
    public class UniqueVisitViewModel
    {
        /// <summary>
        /// Visit ID
        /// </summary>
        public string PatientName { get; set; }
        public long? ScreeningResultID { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTimeOffset? LastCreatedDate { get; set; }
        public string LastCreatedDateLabel { get; set; }

        public DateTimeOffset? LastCompleteDate { get; set; }
        public string LastCompleteDateLabel { get; set; }

        public string LocationName { get; set; }
    }
}

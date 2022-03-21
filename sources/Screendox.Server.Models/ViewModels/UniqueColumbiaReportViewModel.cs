using System;

namespace ScreenDox.Server.Models
{
    public class UniqueColumbiaReportViewModel
    {
        public string PatientName { get; set; }
        public long ID { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTimeOffset? LastCreatedDate { get; set; }
        public string LastCreatedDateLabel { get; set; }

        public DateTimeOffset? LastCompleteDate { get; set; }
        public string LastCompleteDateLabel { get; set; }

        public string LocationName { get; set; }

        public string CompletedByName { get; set; }
    }
}

using System;

namespace ScreenDox.Server.Models
{
    public class BhsResponseBase
    {
        public long ID { get; set; }
        public string StaffNameCompleted { get; set; }
        public DateTimeOffset? CompleteDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
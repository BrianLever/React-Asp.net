using System;

namespace FrontDesk.Common.Bhservice
{
    public interface IBhsEntry
    {
        string BhsStaffNameCompleted { get; set; }
        DateTimeOffset? CompleteDate { get; set; }
        DateTimeOffset CreatedDate { get; set; }
        long ID { get; set; }
    }
}
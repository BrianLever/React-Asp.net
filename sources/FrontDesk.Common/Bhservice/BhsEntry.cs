using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FrontDesk.Common.Bhservice
{
    [Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]
    public class BhsEntry : IBhsEntry
    {
        public long ID { get; set; }
        public string BhsStaffNameCompleted{ get; set; }
        public DateTimeOffset? CompleteDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }


        //TODO: Implement audit log for changes in bhsentry
    }
}

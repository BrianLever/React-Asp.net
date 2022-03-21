using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontdesk.Server.SmartExport.Models
{
    public class SmartExportLog
    {
        public Int64 ID { get; set; }
        public bool Succeed { get; set; }
        public DateTimeOffset ExportDate { get; set; }
        public int? FailedAttemptCount { get; set; }

        public string FailedReason { get; set; }
        public string LastError { get; set; }
        public bool Completed { get; set; }
    }
}

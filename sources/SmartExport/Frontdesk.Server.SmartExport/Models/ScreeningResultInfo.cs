using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontdesk.Server.SmartExport.Models
{
    public class ScreeningResultInfo
    {
        public long ID { get; set; }
        public string PatientName { get; set; }
        public DateTime Birthday { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
    }
}

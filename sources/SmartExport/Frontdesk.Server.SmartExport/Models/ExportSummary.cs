using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontdesk.Server.SmartExport.Models
{
    public class ExportSummary
    {
        public int Succeed { get; set; }
        public int Failed { get; set; }

        public int Total
        {
            get
            {
                return Succeed + Failed;
            }
        }
    }
}

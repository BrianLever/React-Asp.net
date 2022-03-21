using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Models.ViewModels
{
    public class EhrExportRequest
    {
        public int PatientId { get; set; }
        public int VisitId { get; set; }
    }
}

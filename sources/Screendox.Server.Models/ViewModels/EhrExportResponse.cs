using RPMS.Common.Models;

using ScreenDox.Server.Models.Resources;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Models.ViewModels
{
    public class EhrExportResponse
    {
        /// <summary>
        /// One of the values:
        ///  0 - Unknown,
        ///  1 - AllSucceed,
        ///  2 - AllFailed,
        ///  3 - SomeOperationsFailed
        /// </summary>
        public ExportOperationStatus Status { get; set; }

        public bool IsSuccessful
        {
            get
            {
                return Status == ExportOperationStatus.AllSucceed;
            }
        }

        public string OperationStatusText
        {
            get
            {
                var ExportStatus = this.ExportResults.GetExportOperationStatus();
                switch (ExportStatus)
                {
                    case ExportOperationStatus.AllSucceed:
                        return TextMessages.ExportWizard_ExportCompletedSuccessfully;
                       
                    case ExportOperationStatus.AllFailed:
                       return  Resources.TextMessages.ExportWizard_ExportFailed;
                       
                    case ExportOperationStatus.SomeOperationsFailed:
                        return Resources.TextMessages.ExportWizard_ExportCompletedWithErrors;
                }

                return "Unknown";
            }
        }


        public List<string> Errors { get; set; } = new List<string>();

        public List<ExportResult> ExportResults { get; set; } = new List<ExportResult>();
        public ExportTask ExportScope { get; set; }
    }
}

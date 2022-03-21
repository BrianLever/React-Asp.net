using FrontDesk.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontdesk.Server.SmartExport.Models
{
    public static class SmartExportLogFactory
    {
        public static SmartExportLog CreateFromReader(IDataReader reader)
        {
            return new SmartExportLog
            {
                ID = reader.Get<Int64>("ScreeningResultID"),
                Succeed = reader.Get<int>("Succeed") > 0,
                ExportDate = reader.Get<DateTimeOffset>("ExportDate"),
                FailedAttemptCount = reader.Get<int>("FailedAttemptCount"),
                FailedReason = reader.Get<string>("FailedReason"),
                LastError = reader.Get<string>("LastError"),
                Completed = reader.Get<int>("Completed") > 0,
            };
        }
    }
}

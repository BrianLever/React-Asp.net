using System;
using System.Collections.Generic;
using System.Text;

namespace Frontdesk.Server.SmartExport.Jobs
{
    public class SmartExportJob
    {
        private long _screeningResultID;

        public SmartExportJob(long screeningResultID)
        {
            _screeningResultID = screeningResultID;
        }

        public void Execute()
        {

        }
    }
}

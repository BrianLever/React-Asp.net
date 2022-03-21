sing Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScreenDox.Rpms.SmartExport.App.Jobs
{
    public static class JobsBootstrapper
    {
        public static void Initialize()
        {
            //RecurringJob.AddOrUpdate("Fetch-Results-4-Export", () => Console.WriteLine(), Cron.Hourly);

        }
    }
}
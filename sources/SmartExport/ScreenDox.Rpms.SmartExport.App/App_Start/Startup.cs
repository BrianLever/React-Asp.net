using System;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Owin;
using Owin;
using Hangfire.Dashboard;
using Hangfire.RecurringJobExtensions;
using Frontdesk.Server.SmartExport.Jobs;

[assembly: OwinStartup(typeof(ScreenDox.Rpms.SmartExport.App.App_Start.Startup))]

namespace ScreenDox.Rpms.SmartExport.App.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            GlobalConfiguration.Configuration
               .UseSqlServerStorage("JobScheduler")
               .UseRecurringJob("Configuration\\job_schedule.json")
               //.UseRecurringJob(typeof(ScheduleSmartExportJob))
               .UseDefaultActivator();



            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                Queues = new[] { "export_jobs", "orchestrator", "jobs", "default" },
                WorkerCount = Environment.ProcessorCount
            });

            var options = new DashboardOptions
            {
                Authorization = new[]
                {
                    new LocalRequestsOnlyAuthorizationFilter()
                }
            };

            app.UseHangfireDashboard("/jobs", options);

        }
    }
}

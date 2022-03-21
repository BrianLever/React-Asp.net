using Hangfire.RecurringJobExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hangfire.Server;
using Hangfire;
using Common.Logging;
using FrontDesk.Common.Extensions;
using Frontdesk.Server.SmartExport.Services;

namespace Frontdesk.Server.SmartExport.Jobs
{
    public class ScheduleSmartExportJob : IRecurringJob
    {
        private readonly ILog _logger = LogManager.GetLogger<ScheduleSmartExportJob>();
        private readonly ISmartExportService _service;



        public ScheduleSmartExportJob(): this(new SmartExportService() )
        {

        }


        public ScheduleSmartExportJob(ISmartExportService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [AutomaticRetry(Attempts = 0)]
        [DisableConcurrentExecution(90)]
        public void Execute(PerformContext context)
        {
            _logger.Info($"[ScheduleSmartExportJob]{DateTimeOffset.Now.FormatAsDateWithTime()} ScheduleSmartExportJob Running ...");

            var batchSize = context.GetJobData<int>("BatchSize");

            _logger.DebugFormat($"[ScheduleSmartExportJob] job data parameter-> BatchSize: {batchSize}");


            var records4Export = _service.GetScreeningResultIdForExport(batchSize);

            foreach(var id in records4Export)
            {
                BackgroundJob.Enqueue(() => new SmartExportJob(id).Execute());
            }

           
        }
    }
}
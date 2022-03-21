using Common.Logging;
using Frontdesk.Server.SmartExport.Services;
using FrontDesk;
using FrontDesk.Common.Extensions;
using Hangfire;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;
using System;
using System.ComponentModel;

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

        [DisplayName("EHR  Smart Export Orchestrator")]
        [AutomaticRetry(Attempts = 1)]
        [DisableConcurrentExecution(90)]
        public void Execute(PerformContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            _logger.Info($"[ScheduleSmartExportJob]{DateTimeOffset.Now.FormatAsDateWithTime()} ScheduleSmartExportJob Running ...");

            var batchSize = context.GetJobData<int>("BatchSize");

            _logger.DebugFormat($"[ScheduleSmartExportJob] job data parameter-> BatchSize: {batchSize}");


            var records4Export = _service.GetScreeningResultIdForExport(batchSize);

            foreach(var item in records4Export)
            {
                BackgroundJob.Enqueue<SmartExportJob>(x => x.Execute(item.ID, item.PatientName.AsMaskedFullName(), item.Birthday.FormatAsDate("")));
            }

           
        }
    }
}
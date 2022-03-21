using Common.Logging;
using Frontdesk.Server.SmartExport.Services;
using FrontDesk;
using FrontDesk.Common.Extensions;
using FrontDesk.Server.Screening.Services;
using Hangfire;
using System;
using System.ComponentModel;

namespace Frontdesk.Server.SmartExport.Jobs
{
    public class SmartExportJob
    {
        private long _screeningResultID;
        private readonly IScreeningResultService _screeningResultService;
        private readonly ISmartExportService _smartExportService;
        private readonly IAppConfigurationService _configService;

        private readonly ILog _logger = LogManager.GetLogger<SmartExportJob>();

        public SmartExportJob(): this(new ScreeningResultService(), new SmartExportService(), new AppConfigurationService())
        {
            
        }

        public SmartExportJob(IScreeningResultService screeningResultService, ISmartExportService smartExportService, IAppConfigurationService configService)
        {
            _screeningResultService = screeningResultService ?? throw new ArgumentNullException(nameof(screeningResultService));
            _smartExportService = smartExportService ?? throw new ArgumentNullException(nameof(smartExportService));
            _configService = configService ?? throw new ArgumentNullException(nameof(configService));
        }



        [AutomaticRetry(Attempts = 1, LogEvents = true, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        [Queue("export_jobs")]
        [DisplayName("Export screening. ID:{0}, Name: {1}")]
        public void Execute(long screeningResultID, string patientName, string birthday)
        {
            _screeningResultID = screeningResultID;


            var screeningResult = _screeningResultService.Get(_screeningResultID);

            if(screeningResult == null)
            {
                _logger.WarnFormat("[SmartExportJob] Screening Result record not found for ID:{0}, Name: {1}", screeningResultID, patientName);
                return;
            }

            // if survey is empty, continue export to update the status and log warning
            if(!screeningResult.IsEligibleForExportWithoutAddress)
            {
                string reason = "[SmartExportJob] Screening Result is not eligible for export. Screening data is empty. ID:{0}, Name: {1}, Export Date: {2}"
                    .FormatWith(
                    screeningResultID, patientName.AsMaskedFullName(), screeningResult.ExportDate?.FormatAsDateWithTime()
                    );
                _logger.Warn(reason);

            }
            
            _smartExportService.ExecuteExport(screeningResult, _configService.GetRunIsTestModeFlag());
        }
    }
}

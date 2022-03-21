using Common.Logging;
using Frontdesk.Server.SmartExport.Data;
using Frontdesk.Server.SmartExport.EhrInterfaceService;
using Frontdesk.Server.SmartExport.Models;
using Frontdesk.Server.SmartExport.Services.Testing;
using Frontdesk.Server.SmartExport.SmartExtentions;

using FrontDesk;
using FrontDesk.Common.Extensions;
using FrontDesk.Server.Membership;
using FrontDesk.Server.Screening.Services;

using RPMS.Common.Models;
using ScreenDox.EHR.Common.SmartExport;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Frontdesk.Server.SmartExport.Services
{
    public interface ISmartExportService
    {
        List<ScreeningResultInfo> GetScreeningResultIdForExport(int batchSize);
        List<ScreeningResultInfo> TestGetScreeningResultIdForExport(DateTime startDate, int batchSize);

        void LogExportSucceed(ScreeningResult screeningInfo);
        void LogExportIgnored(ScreeningResult screeningInfo, string reason);

        bool ExecuteExport(ScreeningResult screeningResult, bool simulationMode);

    }

    public class SmartExportService : ISmartExportService
    {
        public const int DefaultBatchSize = 100;

        private readonly ILog _logger;

        private readonly ISmartExportRepository _repository;
        private readonly IEhrInterfaceProxy _ehrProxy;
        private readonly IScreeningResultService _screeningResultService;
        private readonly IUserService _userService;
        private readonly IScreeningDefinitionService _screeningDefinitionService;
        private readonly IAppConfigurationService _configurationService;

        // fields
        private readonly string[] _visitCategoriesForExport;
        private readonly Screening _screeningInfo;

        //protected readonly string[] VisitCategoriesForExport = new[] { "AMBULATORY", "TELECOMMUNICATIONS", "CHART REVIEW", "EVENT (HISTORICAL)" };



        public SmartExportService(
            ISmartExportRepository repository,
            IEhrInterfaceProxy rpmsProxy,
            IScreeningResultService screeningResultService,
            IUserService userService,
            ILog logger,
            IAppConfigurationService configurationService,
            IScreeningDefinitionService screeningDefinitionService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _ehrProxy = rpmsProxy ?? throw new ArgumentNullException(nameof(rpmsProxy));
            _screeningResultService = screeningResultService ?? throw new ArgumentNullException(nameof(screeningResultService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
            _screeningDefinitionService = screeningDefinitionService ?? throw new ArgumentNullException(nameof(screeningDefinitionService));

            //init constants
            _visitCategoriesForExport = configurationService.GetAllowedVisitCategories();
            _screeningInfo = _screeningDefinitionService.Get();
        }

        public SmartExportService() : this(
            new SmartExportDb(),
            EhrInterfaceProxy.Instance,
            new ScreeningResultService(),
            new UserService(),
            LogManager.GetLogger<SmartExportService>(),
            new AppConfigurationService(),
            new ScreeningDefinitionService())
        {
        }

        public List<ScreeningResultInfo> GetScreeningResultIdForExport(int batchSize)
        {
            _logger.DebugFormat($"[SmartExportService] Calling GetDataForExport, batchSize: {0}", batchSize);


            if (batchSize <= 0) { batchSize = DefaultBatchSize; }

            var result = _repository.GetScreeningResultsForExport(batchSize);

            _logger.InfoFormat("[SmartExportService] Found {0} items for export.", result.Count);

            return result;
        }


        public List<ScreeningResultInfo> TestGetScreeningResultIdForExport(DateTime startDate, int batchSize)
        {

            if (batchSize <= 0) { batchSize = DefaultBatchSize; }

            var result = _repository.GetScreeningResults(startDate, batchSize);

            return result;
        }

        private Patient GetEhrPatientRecord(ScreeningResult screeningResult)
        {
            //find matched patient in EHR
            var ehrPatientRecord = _ehrProxy.FindEhrPatientRecord(screeningResult);

            if (ehrPatientRecord == null || ehrPatientRecord.ID == 0)
            {
                // not found
                string reason = "[SmartExportService] Export aborted because best match not found. ScreenDox ID: {0}, Patient: {1}, DOB: {2}".FormatWith(
                   screeningResult.ID,
                   screeningResult.FullName.AsMaskedFullName(),
                   screeningResult.Birthday.FormatAsDate()
                  );

                _logger.Warn(reason);
                LogExportIgnored(screeningResult, reason);

                return null;
            }

            return ehrPatientRecord;
        }


        public bool ExecuteExport(ScreeningResult screeningResult, bool simulationMode)
        {
            try
            {
                // find matched EHR record using Auto-correction rules
                var ehrPatientRecord = GetEhrPatientRecord(screeningResult);

                if (ehrPatientRecord == null)
                {
                    return false;
                }

                _logger.InfoFormat("[SmartExportService] Patient record found. Screendox ID: {0}, Patient: ID: {1}, {2}. DOB: {3}",
                    screeningResult.ID,
                    ehrPatientRecord.ID,
                    ehrPatientRecord.FullName().AsMaskedFullName(),
                    ehrPatientRecord.DateOfBirth);

                var allVisits = _ehrProxy.GetScheduledVisitsByPatient(ehrPatientRecord, 0, 100);

                _logger.InfoFormat("[SmartExportService] Found {0} scheduled visits for SD ID: {1}. EHR ID: {2}",
                    allVisits.Count, screeningResult.ID, ehrPatientRecord.ID);

                var visitLookupResult = allVisits
                    .FindBestMatch(screeningResult, _visitCategoriesForExport);

                if (visitLookupResult.BestResult == null)
                {
                    string reason = "[SmartExportService] Export aborted because confidence level on visit match is low ({0}). ID: {1}, Patient: {2}. All options: [{3}]"
                        .FormatWith(visitLookupResult.Confidence, screeningResult.ID, screeningResult.FullName.AsMaskedFullName(),
                         string.Join("| ", allVisits.Select(x => "{0}, {1}, {2}".FormatWith(x.ID, x.Date, x.ServiceCategory))));

                    _logger.Warn(reason);
                    LogExportIgnored(screeningResult, reason);
                    return false;
                }

                if (!simulationMode)  //Production Mode
                {
                    if (!screeningResult.IsEligible4Export)
                    {
                        _logger.WarnFormat("[SmartExportService] Screening has been already exported. Overlapping scheduled job. ID: {0}", screeningResult.ID);
                        return true;
                    }

                    //clean-up address
                    screeningResult.CleanUpAddress();

                    // rewrite Screening Result patient name and DOB with the date from EHR
                    screeningResult.FirstName = ehrPatientRecord.FirstName;
                    screeningResult.LastName = ehrPatientRecord.LastName;
                    screeningResult.MiddleName = ehrPatientRecord.MiddleName;
                    screeningResult.Birthday = ehrPatientRecord.DateOfBirth;

                    // perform export
                    var exportTask = _ehrProxy.PreviewExportResult(screeningResult, ehrPatientRecord, visitLookupResult.BestResult.ID);

                    //double check no patient record changes generated
                    if (exportTask.PatientRecordModifications != null && exportTask.PatientRecordModifications.Count > 0)
                    {
                        exportTask.PatientRecordModifications = new List<PatientRecordModification>();
                        _logger.WarnFormat("[SmartExportService] PreviewExportResult generated patient record modifications. ID: {0}", screeningResult.ID);
                    }

                    //export begin

                    var exportResults = _ehrProxy.CommitExportTask(ehrPatientRecord.ID, visitLookupResult.BestResult.ID, exportTask) ?? new List<ExportResult>();

                    exportResults.AddRange(
                        _ehrProxy.CommitExportResult(
                            ehrPatientRecord.ID,
                            visitLookupResult.BestResult.ID,
                            screeningResult,
                            _screeningInfo));


                    var operationResult = exportResults.GetExportOperationStatus();

                    _logger.InfoFormat("[SmartExportService] Updating export info in the ScreenDox database. ID: {0}. Status: {1}", screeningResult.ID, operationResult);

                    //update screening result info in ScreenDox database
                    // update patient name and DOB as well
                    _screeningResultService.UpdateExportInfo(
                        screeningResult,
                        operationResult,
                        ehrPatientRecord,
                        visitLookupResult.BestResult,
                        _userService.GetExportSystemUserID());

                    bool succceed = operationResult == ExportOperationStatus.AllSucceed || operationResult == ExportOperationStatus.SomeOperationsFailed;

                    if (succceed)
                    {

                        LogExportSucceed(screeningResult);

                        _logger.InfoFormat("[EXPORT][SmartExportService] [Succeed] ID: {0}, Patient: {1}", screeningResult.ID, screeningResult.FullName.AsMaskedFullName());

                        return true;
                    }
                    else
                    {

                        LogExportFailed(screeningResult, "Export to EHR has failed. Operation Status: " + operationResult, null);

                        _logger.InfoFormat("[EXPORT][SmartExportService] [Failed] ID: {0}, Patient: {1} ", screeningResult.ID, screeningResult.FullName.AsMaskedFullName());

                        return false;
                    }


                }
                else
                {
                    return LogExportSimulationResults(screeningResult, ehrPatientRecord, visitLookupResult);
                }
            }
            catch (Exception ex)
            {
                string reason = "[SmartExportService] Failed to export. ID: {0}, Patient: {1}".FormatWith(screeningResult.ID, screeningResult.FullName.AsMaskedFullName());

                LogExportFailed(screeningResult, reason, ex);
                _logger.Error(reason, ex);

                throw;
            }
        }

        private bool LogExportSimulationResults(ScreeningResult screeningResult, Patient patientResult, SmartLookupResult<Visit> visitLookupResult)
        {
            if (screeningResult.ExportDate.HasValue)
            {
                return LogExportCompareResult(screeningResult, patientResult, visitLookupResult);
            }
            else
            {
                //log export result in sumilation mode
                LogExportSimulation(screeningResult, patientResult, visitLookupResult);
                return true;
            }
        }

        private bool LogExportCompareResult(ScreeningResult screeningResult, Patient patientResult, SmartLookupResult<Visit> visitLookupResult)
        {
            //compare results
            var comparer = new ExportTestResultComparer(screeningResult, patientResult, visitLookupResult, _logger);
            if (comparer.Compare())
            {
                _logger.InfoFormat("[EXPORT][TEST] {0}", comparer.Result);

                return true;
            }
            else
            {
                _logger.WarnFormat("[EXPORT][TEST] ID: {0}, {1}", screeningResult.ID, comparer.Result);
                return false;
            }
        }

        private void LogExportSimulation(ScreeningResult screeningResult, Patient patientResult, SmartLookupResult<Visit> visitLookupResult)
        {
            _logger.Info($"[EXPORT][TEST][SmartExportService] [Succeed] ID: {screeningResult.ID}, Name: {screeningResult.FullName.AsMaskedFullName()}, Patient ID: {patientResult.ID}, Check-In Time: {screeningResult.CreatedDate.FormatAsDateWithTime()},EHR: {patientResult.EHR}, Visit ID: {visitLookupResult.BestResult.ID}, Visit Date: {visitLookupResult.BestResult.Date.FormatAsDateWithTime()}, Visit Category: {visitLookupResult.BestResult.ServiceCategory}, Visit Location: {visitLookupResult.BestResult.Location.Name}");
        }

        public ExportSummary GetExportSummary(DateTime? startDate, DateTime? endDate)
        {
            if (endDate.HasValue)
            {
                endDate = endDate.Value.Date.AddDays(1);
            }

            return _repository.GetExportSummary(startDate, endDate);
        }

        public void LogExportSucceed(ScreeningResult screeningResult)
        {
            var model = new SmartExportLog
            {
                ID = screeningResult.ID,
                ExportDate = DateTimeOffset.Now,
                Succeed = true,
                Completed = true,
                FailedAttemptCount = null,

            };
            _repository.LogExportResult(model);
        }

        public void LogExportIgnored(ScreeningResult screeningResult, string reason)
        {
            var lastResult = _repository.GetLogExportResult(screeningResult.ID);

            var model = new SmartExportLog
            {
                ID = screeningResult.ID,
                ExportDate = DateTimeOffset.Now,
                Succeed = false,
                FailedAttemptCount = 1 + (lastResult != null ? lastResult.FailedAttemptCount : 0),
                FailedReason = reason
            };

            model.Completed = model.FailedAttemptCount >= _configurationService.ExportAttemptCountOnIgnore; //stop trying

            _repository.LogExportResult(model);
        }

        private void LogExportFailed(ScreeningResult screeningResult, string reason, Exception ex)
        {
            var lastResult = _repository.GetLogExportResult(screeningResult.ID);

            var model = new SmartExportLog
            {
                ID = screeningResult.ID,
                ExportDate = DateTimeOffset.Now,
                Succeed = false,
                FailedAttemptCount = 1 + (lastResult != null ? lastResult.FailedAttemptCount : 0),
                FailedReason = reason,
                LastError = ex?.ToString()
            };

            model.Completed = model.FailedAttemptCount >= _configurationService.ExportAttemptCountOnFailure; //stop trying

            _repository.LogExportResult(model);
        }
    }
}

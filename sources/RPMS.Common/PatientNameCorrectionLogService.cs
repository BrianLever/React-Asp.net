using Common.Logging;

using FrontDesk;
using FrontDesk.Common.InfrastructureServices;
using RPMS.Common.Models;

using ScreenDox.EHR.Common.Models.PatientValidation;
using ScreenDox.EHR.Common.SmartExport.Repository;

using System;
using System.Collections.Generic;

namespace ScreenDox.EHR.Common
{
    public interface IPatientNameCorrectionLogService
    {
        void Add(PatientSearch originalPatientInfo, PatientSearch mappedPatientSearch, string comments);

        List<PatientNameCorrectionLogItem> Get(DateTimeOffset? startDate, DateTimeOffset? endDate, string nameFilter, string orderBy, int startRowIndex, int maximumRows);
        int GetCount(DateTimeOffset? startDate, DateTimeOffset? endDate, string nameFilter);
    }

    public class PatientNameCorrectionLogService : IPatientNameCorrectionLogService
    {
        private readonly IPatientNameCorrectionLogRepository _repository;
        private readonly ITimeService _timeService;

        private readonly ILog _log = LogManager.GetLogger<PatientNameCorrectionLogService>();


        public PatientNameCorrectionLogService(IPatientNameCorrectionLogRepository repository, ITimeService timeService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
        }

        public PatientNameCorrectionLogService(): this(new PatientNameCorrectionLogDb(), new TimeService())
        {
        }

        public void Add(PatientSearch originalPatientInfo, PatientSearch mappedPatientSearch, string comments)
        {
            _log.InfoFormat("[PatientValidationService] Successful match after correction with inernal map. Original: {0}. Mapped: {1}",
                   originalPatientInfo.FullName().AsMaskedFullName(),
                   mappedPatientSearch.FullName().AsMaskedFullName()
                   );

            _repository.Add(new PatientNameCorrectionLogItem
            {
                OriginalPatientName = originalPatientInfo.FullName(),
                OriginalBirthday = originalPatientInfo.DateOfBirth,
                CorrectedPatientName = mappedPatientSearch.FullName(),
                CorrectedBirthday = mappedPatientSearch.DateOfBirth,
                CreatedDate = _timeService.GetDateTimeOffsetNow(),
                Comments = comments
            });
        }

        public List<PatientNameCorrectionLogItem> Get(DateTimeOffset? startDate, DateTimeOffset? endDate, string nameFilter, string orderBy, int startRowIndex, int maximumRows)
        {
            if (endDate.HasValue)
            {
                endDate = endDate.Value.Date.AddDays(1); // we use "Less than" operator, so we add one day
            }

            return _repository.Get(startDate, endDate, nameFilter, orderBy, startRowIndex, maximumRows);
        }

        public int GetCount(DateTimeOffset? startDate, DateTimeOffset? endDate, string nameFilter)
        {
            if (endDate.HasValue)
            {
                endDate = endDate.Value.Date.AddDays(1); // we use "Less than" operator, so we add one day
            }

            return _repository.GetCount(startDate, endDate, nameFilter);
        }
    }
}

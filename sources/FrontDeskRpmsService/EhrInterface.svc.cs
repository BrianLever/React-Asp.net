using FrontDesk;
using FrontDesk.Common.Debugging;

using RPMS.Common;
using RPMS.Common.Configuration;
using RPMS.Common.GlobalConfiguration;
using RPMS.Common.Models;
using RPMS.Common.Models.PatientValidation;

using ScreenDox.EHR.Common;
using ScreenDox.EHR.Common.Properties;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace RPMS.FrontDesk
{
    public class EhrInterface : IEhrInterface
    {

        private readonly IPatientService _patientService;
        private readonly IVisitService _visitService;
        private readonly IScreeningExportService _exportService;
        private readonly IGlobalSettingsService _globalSettingsService;
        private readonly IPatientValidationService _patientValidationService;

        /// <summary>
        /// Constructor with injected parameters
        /// </summary>
        public EhrInterface(
            IPatientService patientService,
            IVisitService visitService,
            IScreeningExportService exportService,
            GlobalSettingsService globalSettingsService,
            IPatientValidationService patientValidationService)
        {
            _patientService = patientService;
            _visitService = visitService;
            _exportService = exportService;
            _globalSettingsService = globalSettingsService ?? throw new ArgumentNullException(nameof(globalSettingsService));
            _patientValidationService = patientValidationService ?? throw new ArgumentNullException(nameof(patientValidationService));
        }

        #region Patients

        /// <summary>
        /// Get the list of patient matches
        /// </summary>
        public List<Patient> GetMatchedPatients(Patient searchPattern, int startRow, int maxRows)
        {

            return _patientService.GetMatchedPatients(searchPattern, startRow, maxRows);
        }

        /// <summary>
        /// Method can be get count of candidats for export from RPMS database
        /// </summary>
        /// <returns></returns>
        public int GetPatientCount(Patient searchPattern)
        {

            return _patientService.GetMatchedPatientsCount(searchPattern);
        }

        public Patient GetPatientRecord(PatientSearch searchPattern)
        {
            if (searchPattern == null)
            {
                throw new ArgumentNullException(nameof(searchPattern));
            }

            return _patientService.GetPatientRecord(searchPattern);
        }

        #endregion


        #region Visits


        public List<Visit> GetScheduledVisitsByPatient(PatientSearch patientSearch, int startRow, int maxRows)
        {
            if (patientSearch == null)
            {
                throw new ArgumentNullException(nameof(patientSearch));
            }

            if (_globalSettingsService.IsRpmsMode)
            {
                // BMX specific flow
                var patientName = _patientService.GetPatientName(patientSearch.ID);
                if (string.IsNullOrEmpty(patientName))
                {
                    throw new ApplicationException("Patient record could not be found");
                }
                patientSearch.LastName = patientName;
            }

            return _visitService.GetVisitsByPatient(patientSearch, startRow, maxRows);

        }

        public int GetScheduledVisitsByPatientCount(PatientSearch patientSearch)
        {
            if (patientSearch == null)
            {
                throw new ArgumentNullException(nameof(patientSearch));
            }

            if (_globalSettingsService.IsRpmsMode)
            {
                // BMX specific flow
                var patientName = _patientService.GetPatientName(patientSearch.ID);
                if (string.IsNullOrEmpty(patientName))
                {
                    throw new ApplicationException("Patient record could not be found");
                }
                patientSearch.LastName = patientName;
            }
            return _visitService.GetVisitsByPatientCount(patientSearch);
        }

        public Visit GetVisitRecord(int visitID, PatientSearch patientRecord)
        {
            return _visitService.GetVisitRecord(visitID, patientRecord);
        }

        #endregion

        #region Export

        public ExportTask PreviewExportResult(ScreeningResult screeningResult, PatientSearch selectedPatient, int selectedVisitRowId)
        {
            if (screeningResult == null)
            {
                throw new ArgumentNullException(nameof(screeningResult));
            }

            if (selectedPatient == null)
            {
                throw new ArgumentNullException(nameof(selectedPatient));
            }


            var patientRecord = _patientService.GetPatientRecord(selectedPatient);

            var visit = _visitService.GetVisitRecord(selectedVisitRowId, selectedPatient);


            ExportTask task = _exportService.CreateExportTask(screeningResult, patientRecord);

            if (patientRecord == null)
            {
                task.Errors.Add(Resources.PatientRecordNotFound);
            }

            if (visit == null)
            {
                task.Errors.Add(Resources.VisitNotFound);
            }

            return task;
        }


        public List<ExportResult> CommitExportTask(int patientID, int visitID, ExportTask exportTask)
        {
            DebugLogger.WriteTraceMessage("Enter CommitExportTask");

            return _exportService.CommitExportTask(patientID, visitID, exportTask);
        }

        public virtual List<ExportResult> ExportScreeningData(ScreeningResultRecord screeningResultRecord)
        {

            DebugLogger.WriteTraceMessage("Enter ExportScreeningData method");

            return _exportService.ExportScreeningData(screeningResultRecord);

        }

        public ExportMetaInfo GetMeta()
        {
            return new ExportMetaInfo
            {
                IsRpmsMode = _globalSettingsService.IsRpmsMode,
                IsNextGenMode = _globalSettingsService.IsNextGenMode
            };
        }

        #endregion

        #region Validation

        /// <summary>
        /// Validate patient info can be exported, make a dry-run for export
        /// </summary>
        public PatientValidationResult ValidatePatientRecord(PatientSearch patientSearch)
        {
            var result = _patientValidationService.ValidatePatientRecord(patientSearch);

            return result;
        }

        #endregion



    }
}

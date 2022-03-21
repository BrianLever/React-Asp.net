using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using FrontDesk;
using RPMS.Data;
using RPMS.Common;
using RPMS.Common.Models;
using RPMS.Common.Properties;

namespace RPMS.FrontDesk
{
    public class FrontDeskInterface : IFrontDeskInterface
    {

        private readonly IPatientService _patientService;
        private readonly IVisitService _visitService;
        private readonly IScreeningExportService _exportService;

        /// <summary>
        /// Constructor with injected parameters
        /// </summary>
        public FrontDeskInterface(IPatientService patientService, IVisitService visitService, IScreeningExportService exportService)
        {
            _patientService = patientService;
            _visitService = visitService;
            _exportService = exportService;
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

        public Patient GetPatientRecord(int patientID)
        {
            return _patientService.GetPatientRecord(patientID);
        }

        #endregion


        #region Visits


        public List<Visit> GetScheduledVisitsByPatient(int patientID, int startRow, int maxRows)
        {
            return _visitService.GetVisitsByPatient(patientID, startRow, maxRows);
        }

        public int GetScheduledVisitsByPatientCount(int patientID)
        {
            return _visitService.GetVisitsByPatientCount(patientID);
        }

        public Visit GetVisitRecord(int visitID)
        {
            return _visitService.GetVisitRecord(visitID);
        }

        #endregion

        #region Export

        public ExportTask PreviewExportResult(ScreeningResult screeningResult, int selectedPatientRowId, int selectedVisitRowId)
        {
            var patientRecord = _patientService.GetPatientRecord(selectedPatientRowId);
            var visit = _visitService.GetVisitRecord(selectedVisitRowId);

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
            return _exportService.CommitExportTask(patientID, visitID, exportTask);
        }

        #endregion

    }
}

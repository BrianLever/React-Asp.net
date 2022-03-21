using System.Collections.Generic;
using FrontDesk;
using FrontDesk.Server.Services;

using RPMS.Common.Models;

namespace Frontdesk.Server.SmartExport.EhrInterfaceService
{
    public interface IEhrInterfaceProxy: IValidatePatientRecordService
    {
       
        //List<Patient> GetMatchedPatients(Patient patient, int startRow, int maxRows);
        List<Patient> GetMatchedPatients(ScreeningResult screeningResult, int startRow, int maxRows);
        Patient GetPatientRecord(PatientSearch patientSearch);
        Visit GetScheduledVisit(int visitID, PatientSearch patient);
        List<Visit> GetScheduledVisitsByPatient(PatientSearch rpmsPatient, int startRow, int maxRows);
        int GetScheduledVisitsByPatientCount(PatientSearch patient);

        ExportTask PreviewExportResult(ScreeningResult screeningResult, PatientSearch selectedPatient, int selectedVisitRowId);
        List<ExportResult> CommitExportTask(int patientID, int visitID, ExportTask exportTask);
        List<ExportResult> CommitExportResult(int patientID, int visitID, ScreeningResult screeningResult, Screening screeningInfo);

        Patient FindEhrPatientRecord(ScreeningResult screeningResult);

        void ResetCache4GetMatchedPatients(ScreeningResult screeningResult);
        ExportMetaInfo GetMeta();
        int GetPatientCount(ScreeningResult screeningResult);
    }
}
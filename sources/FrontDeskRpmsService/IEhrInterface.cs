using FrontDesk;

using RPMS.Common.Models;
using RPMS.Common.Models.PatientValidation;

using System.Collections.Generic;
using System.ServiceModel;

namespace RPMS.FrontDesk
{
    // NOTE: If you change the interface name "IEhrInterface" here, you must also update the reference to "IEhrInterface" in Web.config.
    [ServiceContract]
    public interface IEhrInterface
    {
        #region Patients

        [OperationContract]
        List<Patient> GetMatchedPatients(Patient searchPattern, int currentPageIndex, int maxRows);

        [OperationContract]
        int GetPatientCount(Patient searchPattern);

        [OperationContract]
        Patient GetPatientRecord(PatientSearch searchPattern);



        #endregion

        #region Scheduled Visits

        [OperationContract]
        List<Visit> GetScheduledVisitsByPatient(PatientSearch patientRecord, int currentPageIndex, int maxRows);

        [OperationContract]
        int GetScheduledVisitsByPatientCount(PatientSearch patientRecord);

        [OperationContract]
        Visit GetVisitRecord(int visitID, PatientSearch patientRecord);

        #endregion

        #region Export
        [OperationContract]
        ExportTask PreviewExportResult(ScreeningResult screeningResult, PatientSearch selectedPatient, int selectedVisitRowId);

        [OperationContract]
        List<ExportResult> CommitExportTask(int patientID, int visitID, ExportTask exportTask);

        [OperationContract]
        List<ExportResult> ExportScreeningData(ScreeningResultRecord screeningResultRecord);


        #endregion

        #region Validation

        [OperationContract]
        PatientValidationResult ValidatePatientRecord(PatientSearch patientSearch);

        #endregion

        //meta
        [OperationContract]
        ExportMetaInfo GetMeta();

    }
}

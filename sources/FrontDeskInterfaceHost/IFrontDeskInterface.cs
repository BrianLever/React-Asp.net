using System.Collections.Generic;
using System.ServiceModel;
using RPMS.Common;
using RPMS.Common.Models;
using FrontDesk;

namespace RPMS.FrontDesk
{
    // NOTE: If you change the interface name "IFrontDeskInterface" here, you must also update the reference to "IFrontDeskInterface" in Web.config.
    [ServiceContract]
    public interface IFrontDeskInterface
    {
        #region Patients

        [OperationContract]
        List<Patient> GetMatchedPatients(Patient searchPattern, int currentPageIndex, int maxRows);

        [OperationContract]
        int GetPatientCount(Patient searchPattern);

        [OperationContract]
        Patient GetPatientRecord(int patientID);


        #endregion

        #region Scheduled Visits

        [OperationContract]
        List<Visit> GetScheduledVisitsByPatient(int patientID, int currentPageIndex, int maxRows);

        [OperationContract]
        int GetScheduledVisitsByPatientCount(int patientID);

        [OperationContract]
        Visit GetVisitRecord(int visitID);

        #endregion

        #region Export
        [OperationContract]
        ExportTask PreviewExportResult(ScreeningResult screeningResult, int selectedPatientRowId, int selectedVisitRowId);

        [OperationContract]
        List<ExportResult> CommitExportTask(int patientID, int visitID, ExportTask exportTask);
        #endregion
    }
}

using System.Collections.Generic;
using RPMS.Common.Models;

namespace RPMS.Common
{
    public interface IVisitRepository
    {
        List<Visit> GetVisitsByPatient(PatientSearch patientSearch, int startRow, int maxRows);
        List<Visit> GetVisitsByPatient(int patientID, int startRow, int maxRows);

        int GetVisitsByPatientCount(PatientSearch patientSearch);
        int GetVisitsByPatientCount(int patientID);

        Visit GetVisitRecord(int visitId, PatientSearch patientSearch);
    }
}

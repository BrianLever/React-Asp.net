using System.Collections.Generic;
using RPMS.Common.Models;

namespace RPMS.Common
{
    public interface IVisitService
    {
        List<Visit> GetVisitsByPatient(PatientSearch patientSearch, int startRow, int maxRows);

        int GetVisitsByPatientCount(PatientSearch patientSearch);

        Visit GetVisitRecord(int selectedVisitRowId, PatientSearch patientSearch);
    }
}

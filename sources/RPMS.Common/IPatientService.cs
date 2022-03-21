using System.Collections.Generic;
using RPMS.Common.Models;

namespace RPMS.Common
{
    public interface IPatientService
    {
        List<Patient> GetMatchedPatients(Patient searchPattern, int currentPageIndex, int rowsPerPage);

        int GetMatchedPatientsCount(Patient searchPattern);

        Patient GetPatientRecord(int patientID);

        string GetPatientName(int patientID);

        Patient GetPatientRecord(PatientSearch patientSearch);

        string GetPatientName(PatientSearch patientSearch);
    }
}

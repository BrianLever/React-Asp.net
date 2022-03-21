using System.Collections.Generic;
using RPMS.Common.Models;

namespace RPMS.Common
{
    public interface IPatientRepository
    {
        List<Patient> GetMatchedPatients(Patient patientSearch);

        int GetMatchedPatientsCount(Patient patientSearch);

        Patient GetPatientRecord(PatientSearch patientSearch);

        int UpdatePatientRecordFields(IEnumerable<PatientRecordModification> modifications, int patientId, int visitId);

        string GetPatientName(PatientSearch patientSearch);
    }
}

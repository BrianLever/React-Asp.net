using RPMS.Common.Models;

namespace ScreenDox.EHR.Common.SmartExport
{
    public interface IPatientInfoMatchService
    {
        Patient GetBestMatch(PatientSearch patientSearch);

        PatientSearch CorrectNamesWithMap(PatientSearch patient);
    }
}
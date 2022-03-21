using System.Collections.Generic;
using RPMS.Common.Models;

namespace ScreenDox.EHR.Common.SmartExport.Repository
{
    public interface IPatientMatchRepository
    {
        List<Patient> ValidatePatientInfo(PatientSearch patientSearch);
        Dictionary<string, string> GetNameCorrectionMap();
    }
}
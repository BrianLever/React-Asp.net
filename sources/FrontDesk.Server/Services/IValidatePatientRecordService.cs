using RPMS.Common.Models;

namespace FrontDesk.Server.Services
{
    public interface IValidatePatientRecordService
    {
        PatientSearch ValidatePatientRecord(PatientSearch patient);
    }
}
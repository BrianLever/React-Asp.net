using RPMS.Common.Models;

namespace FrontDesk.Kiosk.Services
{
    public interface IPatientNameValidationService
    {
        /// <summary>
        /// Validate patient name through ScreenDox EHR database
        /// </summary>
        /// <param name="patientScreeningInfo">User's entry</param>
        /// <returns>Patient name and DOB according to EHR</returns>
        /// <exception cref="PatientNameValidationException">Issues getting response from Kiosk Endpoint Name Validation API</exception>
        PatientSearch Validate(IScreeningPatientIdentity patientScreeningInfo);
    }
}
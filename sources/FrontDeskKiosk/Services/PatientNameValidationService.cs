using Common.Logging;
using FrontDesk.Kiosk.Controllers;
using RPMS.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontDesk.Kiosk.Services
{
    public class PatientNameValidationService : IPatientNameValidationService
    {
        private ILog _logger = LogManager.GetLogger<PatientNameValidationService>();

        public PatientNameValidationService()
        {
            _logger.Debug("Initialized PatientNameValidationService.");
        }

        /// <summary>
        /// Validate patient name through ScreenDox EHR database
        /// </summary>
        /// <param name="patientScreeningInfo">User's entry</param>
        /// <returns>Patient name and DOB according to EHR</returns>
        /// <exception cref="PatientNameValidationException">Issues getting response from Kiosk Endpoint Name Validation API</exception>
        public PatientSearch Validate(IScreeningPatientIdentity patientScreeningInfo)
        {
            PatientSearch validationResult = null;

            var patientInfo = new PatientSearch
            {
                LastName = patientScreeningInfo.LastName,
                FirstName = patientScreeningInfo.FirstName,
                MiddleName = patientScreeningInfo.MiddleName,
                DateOfBirth = patientScreeningInfo.Birthday
            };
            try
            {
                validationResult = KioskEndpointServiceClientFactory.Execute(c => c.ValidatePatient(patientInfo));
            }
            catch(Exception ex)
            {
                _logger.ErrorFormat("Failed to validation patient info through EHR system.", ex);

                throw new PatientNameValidationException("Failed to validation patient info through EHR system.", ex);
            }

            return validationResult;
        }
    }
}

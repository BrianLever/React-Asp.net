using RPMS.Common.Models;
using RPMS.Common.Models.PatientValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.EHR.Common
{
    /// <summary>
    /// Allows patient record validation at the moment of screening, that improve screening exportability rate
    /// </summary>
    public interface IPatientValidationService
    {
        /// <summary>
        /// Validate patient info against EHR system.
        /// </summary>
        /// <param name="patientSearch"></param>
        /// <returns>Returns the non-empty object if match has found. </returns>
        PatientValidationResult ValidatePatientRecord(PatientSearch patientSearch);
    }
}

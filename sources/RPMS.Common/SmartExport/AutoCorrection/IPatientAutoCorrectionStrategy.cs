using RPMS.Common.Models;
using System.Collections.Generic;

namespace ScreenDox.EHR.Common.SmartExport.AutoCorrection
{

    public interface IPatientAutoCorrectionStrategy
    {
        /// <summary>
        /// Get all possible options for patient record using given strategy
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        IEnumerable<PatientSearch> Apply(PatientSearch patient);
        IEnumerable<string> GetModificationsLogDescription();
    }
}

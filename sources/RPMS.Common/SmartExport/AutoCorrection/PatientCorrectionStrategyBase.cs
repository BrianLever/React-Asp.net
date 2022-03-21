using RPMS.Common.Models;

using System.Collections.Generic;

namespace ScreenDox.EHR.Common.SmartExport.AutoCorrection
{
    /// <summary>
    /// Base patient correction strategy
    /// </summary>
    public abstract class PatientCorrectionStrategyBase : IPatientAutoCorrectionStrategy
    {
        public abstract IEnumerable<PatientSearch> Apply(PatientSearch patient);

        public abstract IEnumerable<string> GetModificationsLogDescription();
    }
}

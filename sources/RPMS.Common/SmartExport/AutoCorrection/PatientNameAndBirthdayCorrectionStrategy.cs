using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.EHR.Common.SmartExport.AutoCorrection
{
    public class PatientNameAndBirthdayCorrectionStrategy : CompositePatientCorrectionStrategy
    {
        public override IReadOnlyList<IPatientAutoCorrectionStrategy> Strategies { get; } = new List<IPatientAutoCorrectionStrategy>
        {
            new PatientBirthdayCorrectionStrategy(),
            new PatientNameCorrectionStrategy()
        };
    }
}

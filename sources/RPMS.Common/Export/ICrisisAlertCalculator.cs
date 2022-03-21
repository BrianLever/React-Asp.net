using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPMS.Common.Export
{
    public interface ICrisisAlertCalculator
    {
        IList<Models.CrisisAlert> Calculate(IEnumerable<FrontDesk.ScreeningSectionResult> sectionResults);
    }
}

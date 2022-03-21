using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrontDesk;

namespace RPMS.Common.Export
{
    public interface IHealthFactorCalculator
    {
        IList<Models.HealthFactor> Calculate(IEnumerable<ScreeningSectionResult> sectionResults);
    }
}

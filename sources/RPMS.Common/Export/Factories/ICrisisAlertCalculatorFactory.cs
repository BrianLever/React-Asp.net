
using System.Collections.Generic;
namespace RPMS.Common.Export.Factories
{
    public interface ICrisisAlertCalculatorFactory
    {
        IEnumerable<ICrisisAlertCalculator> GetCrisisAlertCalculators();
    }
}


using System.Collections.Generic;
namespace RPMS.Common.Export.Factories
{
    public interface IHealthFactorCalculatorFactory
    {
        IEnumerable<IHealthFactorCalculator> GetHealthFactorCalculators();
    }
}

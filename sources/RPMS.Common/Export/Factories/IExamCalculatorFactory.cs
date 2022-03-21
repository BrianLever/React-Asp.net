
using System.Collections.Generic;
namespace RPMS.Common.Export.Factories
{
    public interface IExamCalculatorFactory
    {
        IEnumerable<IExamCalculator> GetExamCalculators();
    }
}

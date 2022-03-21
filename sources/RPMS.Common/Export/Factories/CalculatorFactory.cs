using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPMS.Common.Export;

namespace RPMS.Common.Export.Factories
{
    public class CalculatorFactory : IHealthFactorCalculatorFactory, IExamCalculatorFactory, ICrisisAlertCalculatorFactory
    {
        public IEnumerable<IHealthFactorCalculator> GetHealthFactorCalculators()
        {

            return new IHealthFactorCalculator[]{
                new TobaccoHealthFactorCalculator(),
                new AlcoholHealthFactorCalculator(),

            };
        }

        public IEnumerable<IExamCalculator> GetExamCalculators()
        {

            return new IExamCalculator[]{
                new DepressionExamCalculator(),
                new PartnerViolenceExamCalculator(),
                new AlcoholExamCalculator()

            };
        }


        public IEnumerable<ICrisisAlertCalculator> GetCrisisAlertCalculators()
        {
            return new ICrisisAlertCalculator[]{
                new PartnerViolenceCrisisAlertCalculator(),
                new DepressionCrisisAlertCalculator()
            };
        }

    }
}

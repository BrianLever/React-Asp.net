
namespace RPMS.Common.Export.Factories
{
    /// <summary>
    /// Default implementation of ScreeningResultProcessorFactory
    /// </summary>
    public class ScreeningResultProcessorFactory : IScreeningResultProcessorFactory
    {
        private readonly CalculatorFactory _factory = new CalculatorFactory();


        #region IScreeningResultProcessorFactory Members

        public IHealthFactorCalculatorFactory CreateHealthFactorCalculatorFactory()
        {
            return _factory;
        }

        public IExamCalculatorFactory CreateExamCalculatorFactory()
        {
            return _factory;
        }

        public ICrisisAlertCalculatorFactory CreateCrisisAlertCalculatorFactory()
        {
            return _factory;
        }

        #endregion
    }
}

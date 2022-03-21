
namespace RPMS.Common.Export.Factories
{
    /// <summary>
    /// Creates factories for processing screening sections results and generate EHR patient's health factors, exams and crisis alerts
    /// </summary>
    public interface IScreeningResultProcessorFactory
    {
        IHealthFactorCalculatorFactory CreateHealthFactorCalculatorFactory();

        IExamCalculatorFactory CreateExamCalculatorFactory();

        ICrisisAlertCalculatorFactory CreateCrisisAlertCalculatorFactory();
    }
}

using System.Collections.Generic;
namespace FrontDesk.Kiosk.Workflow
{
	public interface IPatientScreeningFrequencyRepository
    {
        Dictionary<string, int> GetPatientScreeningFrequencyStatistics(FrontDesk.ScreeningPatientIdentity patient);
    }
}

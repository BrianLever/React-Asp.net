using System;

namespace FrontDesk.Server.Data
{
    public interface IPatientScreeningFrequencyRepository
    {
        int GetPatientContactInfoScreeningCount(ScreeningPatientIdentity patient, DateTimeOffset currentGpraPeriodStartDate);

        int GetPatientDemographicsScreeningCount(ScreeningPatientIdentity patient, DateTimeOffset currentGpraPeriodStartDate);

        int GetPatientSectionScreeningCount(ScreeningPatientIdentity patient, string screeningSectionID, DateTimeOffset currentGpraPeriodStartDate);
        
    }
}

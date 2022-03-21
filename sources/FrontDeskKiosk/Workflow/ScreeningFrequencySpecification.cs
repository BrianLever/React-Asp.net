using System;
using System.Collections.Generic;

using Common.Logging;

using FrontDesk.Common.Debugging;

namespace FrontDesk.Kiosk.Workflow
{
    public class ScreeningFrequencySpecification : FrontDesk.Kiosk.Workflow.IScreeningFrequencySpecification
    {
        private readonly IPatientScreeningFrequencyRepository _patientScreeningFrequencyStatisticsProvider = null;
        private readonly ILog _logger = LogManager.GetLogger<ScreeningFrequencySpecification>();

        Dictionary<string, int> _patientSectionScreeningCountPerCurrentGpraPeriod = null;

        #region Conststructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public ScreeningFrequencySpecification(IPatientScreeningFrequencyRepository patientScreeningFrequencyStatisticsProvider)
        {
            if (patientScreeningFrequencyStatisticsProvider == null)
            {
                throw new ArgumentNullException("patientScreeningFrequencyStatisticsProvider");
            }

            _patientScreeningFrequencyStatisticsProvider = patientScreeningFrequencyStatisticsProvider;

            _logger.Debug("Initialized ScreeningFrequencySpecification.");
        }
        

        #endregion

        #region Skip Screening steps depending on screening frequency settings

        /// <summary>
        /// Load patient's screening statistics for current GPRA period from remote service
        /// </summary>
        public void LoadPatientScreeningsStatistics()
        {
            _patientSectionScreeningCountPerCurrentGpraPeriod = _patientScreeningFrequencyStatisticsProvider.GetPatientScreeningFrequencyStatistics((ScreeningPatientIdentity)ScreeningResultState.Instance.Result);
        }

        /// <summary>
        /// Check if patient has passed required number of screening for certain section during current interval in GPRA reporting period
        /// </summary>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        public bool IsSkipRequiredForSection(string sectionId)
        {
            try
            {
                int count;
                if (_patientSectionScreeningCountPerCurrentGpraPeriod != null && _patientSectionScreeningCountPerCurrentGpraPeriod.TryGetValue(sectionId, out count))
                {
                    return count > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                DebugLogger.TraceException(ex);
                return false;
            }
        }

        #endregion
    }
}

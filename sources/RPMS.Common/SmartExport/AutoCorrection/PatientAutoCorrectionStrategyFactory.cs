namespace ScreenDox.EHR.Common.SmartExport.AutoCorrection
{
    public class PatientAutoCorrectionStrategyFactory
    {
        /// <summary>
        /// Get all known strategies to automatically correct patient info to match EHR
        /// </summary>
        /// <returns>Sprted list of strategies</returns>
        public IPatientAutoCorrectionStrategy[] Create()
        {
            return new IPatientAutoCorrectionStrategy[]
            {
                new PatientNameAndBirthdayCorrectionStrategy()
            };
        }
    }
}

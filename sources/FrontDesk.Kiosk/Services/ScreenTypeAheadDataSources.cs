using System;

namespace FrontDesk.Kiosk.Services
{
    public class ScreenTypeAheadDataSources
    {
        private static Lazy<ScreenTypeAheadDataSources> Instance = new Lazy<ScreenTypeAheadDataSources>(true);

        public static ScreenTypeAheadDataSources Default => Instance.Value;

        public ScreenTypeAheadDataSources()
        {

        }

        public PatientStateService States { get; } = new PatientStateService();
        public DemographicsTribeService Tribes { get; } = new DemographicsTribeService();
        
        public DemographicsCountyNameService CountyNames { get; } = new DemographicsCountyNameService();
        public DemographicsCountyStateService CountyStates { get; } = new DemographicsCountyStateService();

        public void Refresh()
        {
            States.Refresh();
            Tribes.Refresh();
            
            CountyNames.Refresh();
            
        }
    }
}

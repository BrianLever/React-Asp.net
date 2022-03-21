using System;
namespace FrontDesk.Services
{
    public interface IScreeningSectionAgeService
    {
        DateTime? GetMaxAgeSettingsModifiedDateUTC();
        global::FrontDesk.ScreeningSectionAge GetMinimalAgeForScreeningSection(string screeningSectionId);
        void UpdateAgeSettings(global::System.Collections.Generic.ICollection<global::FrontDesk.ScreeningSectionAge> ageSettings);
    }
}

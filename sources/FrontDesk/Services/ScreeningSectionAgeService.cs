using System;
using System.Collections.Generic;
using System.Linq;
using FrontDesk.Data;

namespace FrontDesk.Services
{
    /// <summary>
    /// Applicable for kiosk app
    /// </summary>
    public class ScreeningSectionAgeService : IScreeningSectionAgeService
    {
        private readonly IScreeningKioskDb _db;

        public ScreeningSectionAgeService(IScreeningKioskDb kioskDb)
        {
            _db = kioskDb ?? throw new ArgumentNullException(nameof(kioskDb));
        }

        public ScreeningSectionAgeService() : this(new ScreeningKioskDb())
        {

        }



        /// <summary>
        /// Get minimal age parameter for screening section
        /// </summary>
        /// <param name="screeningSectionId"></param>
        public ScreeningSectionAge GetMinimalAgeForScreeningSection(string screeningSectionId)
        {
            return _db.GetMinimalAgeForScreeningSection(screeningSectionId) ?? new ScreeningSectionAge { MinimalAge = 0, IsEnabled = true };
        }
        /// <summary>
        /// Update minimal age settings from server
        /// </summary>
        public void UpdateAgeSettings(ICollection<ScreeningSectionAge> ageSettings)
        {
            if (ageSettings == null) return;

            _db.UpdateAgeSettings(ageSettings);
        }

    /// <summary>
        /// get last date when minimal age parameters have been saved in the kiosk database
        /// </summary>
        public DateTime? GetMaxAgeSettingsModifiedDateUTC()
        {
            return _db.GetMaxAgeSettingsModifiedDateUTC();
        }
    }
}

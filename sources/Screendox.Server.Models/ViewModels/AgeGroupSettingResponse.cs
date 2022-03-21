using ScreenDox.Server.Models.Configuration;

namespace ScreenDox.Server.Models.ViewModels
{
    /// <summary>
    /// View model for Age Groups Settings management.
    /// Age Groups are used in Reports where there is a breakdown on age groups.
    /// </summary>
    public class AgeGroupSettingResponse
    {
        /// <summary>
        /// Current system value of Age Groups system setting
        /// </summary>
        public SystemSettingItem Value { get; set; }

        /// <summary>
        /// Default value of Age Group system settings
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Formatted labels for the currently configured age groups
        /// </summary>
        public string[] Labels { get; set; }
    }
}

namespace ScreenDox.Server.Models.ViewModels
{
    /// <summary>
    /// Request model for Security Log Settings API
    /// </summary>
    public class SecurityLogEventSettingItemRequest
    {
        /// <summary>
        /// Event ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// True if tracking is enabled
        /// </summary>
        public bool? IsEnabled { get; set; }

    }
}

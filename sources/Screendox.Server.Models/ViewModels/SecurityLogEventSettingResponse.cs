namespace ScreenDox.Server.Models.ViewModels
{
    /// <summary>
    /// Response model for Security Log Settings API
    /// </summary>
    public class SecurityLogEventSettingResponse
    {
        /// <summary>
        /// Event ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Event description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// ID of the Event Category
        /// </summary>
        public int CategoryID { get; set; }

        /// <summary>
        /// True if tracking is enabled
        /// </summary>
        public bool IsEnabled { get; set; }

    }
}

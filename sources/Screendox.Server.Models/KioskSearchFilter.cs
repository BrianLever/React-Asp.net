namespace ScreenDox.Server.Models
{
    /// <summary>
    /// Filter for kiosk search
    /// </summary>
    public class KioskSearchFilter : PagedSearchFilter
    {
        /// <summary>
        /// Filter by name. Null or empty string if include all
        /// </summary>
        public string NameOrKey { get; set; } = string.Empty;

        /// <summary>
        /// Filter by Screen Profile ID (Optional)
        /// </summary>
        public int? ScreeningProfileId { get; set; } = null;
        /// <summary>
        /// Filter by branch location
        /// </summary>
        public int? BranchLocationId { get; set; } = null;
        /// <summary>
        /// Show only active records (the default) when True, or all items when False.
        /// </summary>
        public bool ShowDisabled { get; set; } = false;
    }
}

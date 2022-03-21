namespace ScreenDox.Server.Models
{
    /// <summary>
    /// Filter for branch location search
    /// </summary>
    public class BranchLocationSearchFilter : SearchByNamePagedSearchFilter
    {
        /// <summary>
        /// Filter by Screen Profile ID (Optional)
        /// </summary>
        public int? ScreeningProfileId { get; set; } = null;
        /// <summary>
        /// Show only active records (the default) when True, or all items when False.
        /// </summary>
        public bool ShowDisabled { get; set; } = false;
    }
}

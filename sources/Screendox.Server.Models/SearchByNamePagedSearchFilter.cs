namespace ScreenDox.Server.Models
{
    /// <summary>
    /// Filter by name search
    /// </summary>
    public class SearchByNamePagedSearchFilter : PagedSearchFilter
    {
        /// <summary>
        /// Filter by name. Null or empty string if include all
        /// </summary>
        public string FilterByName { get; set; } = string.Empty;
    }
}

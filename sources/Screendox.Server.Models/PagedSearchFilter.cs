using System.Collections.Generic;
using System.Linq;

namespace ScreenDox.Server.Models
{
    public interface IPagedSearchFilter
    {
        int StartRowIndex { get; set; }

        int MaximumRows { get; set; }

        string OrderBy { get; set; }

    }


    public class PagedSearchFilter : IPagedSearchFilter
    {
        /// <summary>
        /// The index of the first records returned.
        /// </summary>
        public int StartRowIndex { get; set; } = 0;

        /// <summary>
        /// Max number of returned records
        /// </summary>
        public int MaximumRows { get; set; } = 20;
        /// <summary>
        /// Order by condition
        /// </summary>
        public string OrderBy { get; set; } = string.Empty;
    }

    public static class PagedSearchFilterExtensions
    {
        public static IEnumerable<T> FilterItems<T>(this IPagedSearchFilter filter, IEnumerable<T> items)
        {
            if (filter.StartRowIndex > 0)
            {
                items = items.Skip(filter.StartRowIndex);
            }

            if (filter.MaximumRows > 0)
            {
                items = items.Take(filter.MaximumRows);
            }

            return items;
        }
    }
}

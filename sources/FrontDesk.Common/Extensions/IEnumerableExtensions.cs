using System.Collections.Generic;
using System.Linq;

namespace FrontDesk.Common.Extensions
{
    public static class IEnumerableExtensions
    {
        public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> source)
        {
            return source == null || source.Any();
        }

    }
}

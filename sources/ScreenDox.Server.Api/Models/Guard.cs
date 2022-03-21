using ScreenDox.Server.Api.Infrastructure;
using ScreenDox.Server.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ScreenDox.Server.Api.Models
{
    /// <summary>
    /// Guard class with input models validation rules
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Guard that validates OrderBy field in the PagedScreeningResultFilterModel model
        /// </summary>
        /// <param name="filter">Filter object</param>
        /// <param name="supportedColumns">Allowed columns for sorting</param>
        public static void ValidateOrderByClause(
            IPagedSearchFilter filter,
            IEnumerable<string> supportedColumns)
        {

            if (String.IsNullOrWhiteSpace(filter.OrderBy)) return;

            var allowedEntries = new List<string> { "asc", "desc" };
            allowedEntries.AddRange(supportedColumns);

            // validate order by
            var re = new Regex(@"\w+(\s*(desc|asc))?", RegexOptions.IgnoreCase);

            if (!re.IsMatch(filter.OrderBy))
            {
                "OrderBy format is invalid. Specify only single column sorting.".ThrowBadRequestMessage();
            }

            var orderBySegments = filter.OrderBy.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

            var hasOnlyValidTokens = orderBySegments.All(
                x => allowedEntries.Any(
                    y => string.Compare(x, y, StringComparison.OrdinalIgnoreCase) == 0
                ));
            hasOnlyValidTokens.ShouldBeTrue("OrderBy has insupported field name or clause");
        }
    }
}
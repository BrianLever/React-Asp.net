using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace RPMS.Data.CareBridge.Formatting
{
    public static class StringFormat
    {
        public static string AsPhone(string rawString)
        {

            if (string.IsNullOrWhiteSpace(rawString)) return string.Empty;

            Regex rg = new Regex(@"\(?(\d{3})\)?[-\s]?(\d{3})[ -]?(\d+)", RegexOptions.IgnoreCase);

            var match = rg.Match(rawString);

            if (match.Groups.Count < 4)
            {
                  return string.Empty;
            }

            return string.Format("({0}) {1}-{2}", match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value);

        }
    }
}

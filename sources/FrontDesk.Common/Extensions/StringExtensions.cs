using System.Diagnostics;
using System.Text.RegularExpressions;

namespace FrontDesk
{
    public static class StringExtensions
    {
        public static string FormatWith(this string pattern, params object[] args)
        {
            return string.Format(pattern, args);
        }


        public static string AsMaskedPassword(this string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return string.Empty;
            }
            string[] result = new string[3];
            if (password.Length > 4)
            {
                result[0] = password.Substring(0, 2);
                result[1] = password.Substring(2, password.Length - 4);
                result[2] = password.Substring(password.Length - 2);
            }
            else if (password.Length > 3)
            {
                result[0] = password.Substring(0, 1);
                result[1] = password.Substring(1, password.Length - 2);
                result[2] = password.Substring(password.Length - 1);
            }
            else
            {
                result[0] = password.Substring(0, 1);
                result[1] = password.Substring(1, password.Length - 1);
                result[2] = string.Empty;
            }

            result[1] = Regex.Replace(result[1], ".", "*");
            return string.Join("", result);
        }


        public static string AsPhoneFormattedString(this string rawString)
        {
            if (string.IsNullOrWhiteSpace(rawString)) return string.Empty;

            Regex rg = new Regex(@"\(?(\d{3})\)?[-\s]?(\d{3})[ -]?(\d+)", RegexOptions.IgnoreCase);

            var match = rg.Match(rawString);

            if (match.Groups.Count < 4)
            {
                Trace.TraceWarning("Phone number canot be parsed. Phone: {0}", rawString);
                return string.Empty;
            }

            return "({0}) {1}-{2}".FormatWith(match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value);
        }
    }
}

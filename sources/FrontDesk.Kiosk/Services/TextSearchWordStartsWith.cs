using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FrontDesk.Kiosk.Services
{
    public class TextSearchWordStartsWith
    {
        public bool IsMatched(string matchPattern, string lookupItem)
        {
            var patterns = matchPattern.Split(new char[] { ' ', ',', '.' }, StringSplitOptions.RemoveEmptyEntries);

            int patternCount = patterns.Length;
            int succeedMatches = 0;

            foreach (var pattern in patterns)
            {
                var re = new Regex($"(?<!\\w+){pattern}", RegexOptions.IgnoreCase);

                succeedMatches +=  re.IsMatch(lookupItem)? 1: 0;
            }

            return succeedMatches == patternCount;
        }
    }
}

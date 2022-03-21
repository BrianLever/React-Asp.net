using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontDesk.Common.Extensions
{
    public static class CountyTextFunctions
    {
        public static (string Name, string State) ParseCounty(string demographicsCounty)
        {
            var result = (Name: string.Empty, State: string.Empty);

            //split on county
            var entry = demographicsCounty ?? string.Empty;
            var segments = entry.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim()).ToArray();

            switch (segments.Length)
            {
                case 1:
                    result.Name = segments.FirstOrDefault();
                    break;

                case 2:
                    result.Name = segments[0];
                    result.State = segments[1];
                    break;
                default:
                    result.Name = string.Join(", ", segments.Take(segments.Length - 1));
                    result.State = segments.Last();
                    break;
            }

            return result;
        }
    }
}

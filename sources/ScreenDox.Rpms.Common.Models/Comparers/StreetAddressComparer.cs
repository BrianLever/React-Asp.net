using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPMS.Common.Comparers
{
    public class StreetAddressComparer : IComparer<string>
    {
        public int Compare(string repoItemAddress, string patternAddress)
        {
            repoItemAddress = string.IsNullOrEmpty(repoItemAddress) ? string.Empty : repoItemAddress.Trim();
            patternAddress = string.IsNullOrEmpty(patternAddress) ? string.Empty : patternAddress.Trim();


            int result = String.Compare(repoItemAddress, patternAddress, StringComparison.OrdinalIgnoreCase);

            if (result != 0)
            {
                if (patternAddress.StartsWith(repoItemAddress, StringComparison.OrdinalIgnoreCase) ||
                    patternAddress.EndsWith(repoItemAddress, StringComparison.OrdinalIgnoreCase) ||

                    repoItemAddress.StartsWith(patternAddress, StringComparison.OrdinalIgnoreCase)
                    )
                {
                    return 0;
                }
             
            }
            return result;
        }
    }
}

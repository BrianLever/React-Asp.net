using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPMS.Common.Models;
using FrontDesk.Common;


namespace RPMS.Common.Comparers
{
    public class PatientSearchComparer : IComparer<PatientSearch>
    {
        protected StringComparison stringComparison = StringComparison.OrdinalIgnoreCase;
        protected StreetAddressComparer streetAddressComparer = new StreetAddressComparer();

        public const int FULL_NAME_AND_DOB_MATCH = 0xFF00;
        public const int LAST_AND_FIRST_NAME_AND_DOB_MATCH_THRESHOLD = 0x20000;
        public const int FULL_MATCH_LAST_AND_DOB_AND_SHORT_FIRST_NAME = 0x40000 | 1;

        public const int FULL_MATCH = 0x1;


        /// <summary>
        /// Compare patient to the matching patient details
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int GetMatchRank(PatientSearch item, PatientSearch pattern)
        {
            int result = 0;
            int mask = 0x100000;

            result = MatchNameFields(item, pattern, result, ref mask);

            result |= 0x1; //just return not zero rank to identify that ranks has been evaluated

            return result;
        }

        protected int MatchNameFields(PatientSearch item, PatientSearch pattern, int result, ref int mask)
        {
            //replace space in last name with hyphen
            {
                var itemLastName = item.LastName.Trim().Replace(' ', '-');
                var patternLastName = pattern.LastName.Trim().Replace(' ', '-');

                if (String.Compare(itemLastName, patternLastName, stringComparison) != 0)
                {
                    result |= mask; // 0x100000
                }
            }

            mask >>= 1;
            if (DateTime.Compare(item.DateOfBirth, pattern.DateOfBirth) != 0)
            {
                result |= mask; // 0x80000
            }
            mask >>= 1;
            if (String.Compare(item.FirstName, pattern.FirstName, stringComparison) != 0)
            {
                result |= mask; //0x40000
            }

            mask >>= 1;
            if (!string.IsNullOrEmpty(item.FirstName) && !string.IsNullOrEmpty(pattern.FirstName))
            {
                var itemFirstName = item.FirstName.ToUpperInvariant();
                var patternFirstName = pattern.FirstName.ToUpperInvariant();


                if (!itemFirstName.Contains(pattern.FirstName) && !patternFirstName.StartsWith(item.FirstName))
                {
                    result |= mask; //0x20000
                }
            }

            mask >>= 1;
            if (String.Compare(item.MiddleName, pattern.MiddleName, stringComparison) != 0)
            {
                result |= mask; //0x10000
            }


            return result;
        }

        public int Compare(PatientSearch x, PatientSearch y)
        {
            if (x.MatchRank == 0 || y.MatchRank == 0)
            {
                throw new InvalidOperationException("PatientSearch.CompareTo requires that MatchRank must be evaluated beforehand");
            }

            int result = x.MatchRank.CompareTo(y.MatchRank);

            if (result == 0)
                String.Compare(x.LastName, y.LastName, stringComparison);

            if (result == 0)
                result = DateTime.Compare(x.DateOfBirth, y.DateOfBirth);

            if (result == 0)
                result = String.Compare(x.FirstName, y.FirstName, stringComparison);

            if (result == 0)
                result = String.Compare(x.MiddleName, y.MiddleName, stringComparison);
            
            return result;
        }
    }
}

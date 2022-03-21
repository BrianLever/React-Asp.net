using FrontDesk.Common;

using RPMS.Common.Models;

using System;
using System.Collections.Generic;


namespace RPMS.Common.Comparers
{
    public class PatientComparer : PatientSearchComparer, IComparer<Patient>
    {
     
        /// <summary>
        /// Compare patient to the matching patient details
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int GetMatchRank(Patient item, Patient pattern)
        {
            int result = 0;
            int mask = 0x100000;

            result = MatchNameFields(item, pattern, result, ref mask);

            result = MatchGeneralAddress(item, pattern, result, ref mask);

            result = MatchStreetAddress(item, pattern, result, ref mask);

            result = MatchPhoneField(item, pattern, result, ref mask);

            result |= 0x1; //just return not zero rank to identify that ranks has been evaluated

            return result;
        }

        private int MatchGeneralAddress(Patient item, Patient pattern, int result, ref int mask)
        {
            mask >>= 1;
            if (String.Compare(item.StateID, pattern.StateID, stringComparison) != 0)
            {
                result |= mask; //0x8000
            }
            mask >>= 1;
            if (String.Compare(item.City, pattern.City, stringComparison) != 0)
            {
                result |= mask; ///0x4000
            }
            mask >>= 1;
            if (String.Compare(item.ZipCode, pattern.ZipCode, stringComparison) != 0)
            {
                result |= mask; //0x2000
            }

            return result;

        }

        private int MatchStreetAddress(Patient item, Patient pattern, int result, ref int mask)
        {
            mask >>= 1;
            if (streetAddressComparer.Compare(item.StreetAddress, pattern.StreetAddress) != 0)
            {
                result |= mask; //0x1000
            }

            return result;
        }

        private int MatchPhoneField(Patient item, Patient pattern, int result, ref int mask)
        {
            mask >>= 1;
            //in pattern object we have only PhoneHome field from the FrontDesk database
            if (string.Compare(item.PhoneHome.AsRawPhoneNumber(), pattern.PhoneHome.AsRawPhoneNumber(), stringComparison) != 0
                && String.Compare(item.PhoneOffice.AsRawPhoneNumber(), pattern.PhoneHome.AsRawPhoneNumber(), stringComparison) != 0
                )
            {
                result |= mask; //0x8
            }

            return result;
        }

        public int Compare(Patient x, Patient y)
        {
            if (x.MatchRank == 0 || y.MatchRank == 0)
            {
                throw new InvalidOperationException("Patient.CompareTo requires that MatchRank must be evaluated beforehand");
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
            if (result == 0)
                result = String.Compare(x.StateID, y.StateID, stringComparison);
            if (result == 0)
                result = String.Compare(x.City, y.City, stringComparison);
            if (result == 0)
                result = String.Compare(x.ZipCode, y.ZipCode, stringComparison);
            if (result == 0)
                result = streetAddressComparer.Compare(x.StreetAddress, y.StreetAddress);
            if (result == 0)
                result = String.Compare(x.PhoneHome.AsRawPhoneNumber(), y.PhoneHome.AsRawPhoneNumber(), stringComparison);
            return result;
        }
    }
}

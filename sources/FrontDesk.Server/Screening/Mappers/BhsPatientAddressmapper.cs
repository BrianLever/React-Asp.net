using RPMS.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Screening.Mappers
{
    public static class BhsPatientAddressmapper
    {
        public static void ImportPatientAddressFromEhr(ScreeningResult screeningResult, Patient ehrRecord)
        {
            if(screeningResult.IsEmptyContactInfo())
            {
                screeningResult.StreetAddress = ehrRecord.StreetAddress;
                screeningResult.City = ehrRecord.City;
                screeningResult.StateID = ehrRecord.StateID;
                screeningResult.ZipCode = ehrRecord.ZipCode;
                screeningResult.Phone = (!string.IsNullOrWhiteSpace(ehrRecord.PhoneHome) ? ehrRecord.PhoneHome : ehrRecord.PhoneOffice).AsPhoneFormattedString();
            }

            if(string.IsNullOrEmpty(screeningResult.ExportedToHRN))
            {
                screeningResult.ExportedToHRN = ehrRecord.EHR;
            }
        }
    }
}

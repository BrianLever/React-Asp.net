using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPMS.Common.Builders
{
    /// <summary>
    /// Descriptor for patient db fields
    /// </summary>
    public static class PatientRepositoryDescriptor
    {
        public static string RowIdColumn = "RowId";
        public static string ElectronicHealthRecordColumn = "HEALTH_RECORD_NO_";
        public static string NameColumn = "NAME";
        public static string DateOfBirthColumn = "DATE_OF_BIRTH";
        public static string StateIdColumn = "StateID";
        public static string CityColumn = "CITY";
        public static string ZipCodeColumn = "ZIP_CODE";
        public static string StreetAddressLine1 = "STREET_ADDRESS_LINE_1";
        public static string StreetAddressLine2 = "STREET_ADDRESS_LINE_2";
        public static string StreetAddressLine3 = "STREET_ADDRESS_LINE_3";
        public static string PhoneHomeColumn = "PHONE_NUMBER_RESIDENCE";
        public static string PhoneOfficeColumn = "PHONE_NUMBER_WORK";
       

    }
}

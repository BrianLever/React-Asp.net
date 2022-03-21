using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPMS.Common.Models;

namespace RPMS.Common.Builders
{
    public class PatientBuilder : IEntityBuilder<Patient>
    {
        #region IEntityBuilder<Patient> Members

        public Patient CreateFromDbReader(System.Data.IDataReader reader)
        {
            return new Patient
            {
                ID = Convert.ToInt32(reader[PatientRepositoryDescriptor.RowIdColumn]),
                EHR = Convert.ToString(reader[PatientRepositoryDescriptor.ElectronicHealthRecordColumn]),
                FullName = Convert.ToString(reader[PatientRepositoryDescriptor.NameColumn]),
                DateOfBirth = Convert.ToDateTime(reader[PatientRepositoryDescriptor.DateOfBirthColumn]),
                StateID = Convert.ToString(reader[PatientRepositoryDescriptor.StateIdColumn]),
                City = Convert.ToString(reader[PatientRepositoryDescriptor.CityColumn]),
                ZipCode = Convert.ToString(reader[PatientRepositoryDescriptor.ZipCodeColumn]),
                StreetAddressLine1 = Convert.ToString(reader[PatientRepositoryDescriptor.StreetAddressLine1]),
                //StreetAddressLine2 = Convert.ToString(reader[PatientRepositoryDescriptor.StreetAddressLine2]),
                //StreetAddressLine3 = Convert.ToString(reader[PatientRepositoryDescriptor.StreetAddressLine3]),
                PhoneHome = Convert.ToString(reader[PatientRepositoryDescriptor.PhoneHomeColumn]),
                PhoneOffice = Convert.ToString(reader[PatientRepositoryDescriptor.PhoneOfficeColumn])

            };
        }

        #endregion

        #region IEntityBuilder<Patient> Members

        /// <summary>
        /// Pass custom field mapping to default column names from PatientRepositoryDescriptor as Dictionary
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="customFieldMapping"></param>
        /// <returns></returns>
        public Patient CreateFromDbReader(System.Data.IDataReader reader, IDictionary<string, string> customFieldMapping)
        {
            if (customFieldMapping == null)
                throw new ArgumentNullException("customFieldMapping");
            

            return new Patient
            {
                ID = Convert.ToInt32(reader[customFieldMapping[PatientRepositoryDescriptor.RowIdColumn]]),
                EHR = Convert.ToString(reader[customFieldMapping[PatientRepositoryDescriptor.ElectronicHealthRecordColumn]]),
                FullName = Convert.ToString(reader[customFieldMapping[PatientRepositoryDescriptor.NameColumn]]),
                DateOfBirth = Convert.ToDateTime(reader[customFieldMapping[PatientRepositoryDescriptor.DateOfBirthColumn]]),
                StateID = Convert.ToString(reader[customFieldMapping[PatientRepositoryDescriptor.StateIdColumn]]),
                City = Convert.ToString(reader[customFieldMapping[PatientRepositoryDescriptor.CityColumn]]),
                ZipCode = Convert.ToString(reader[customFieldMapping[PatientRepositoryDescriptor.ZipCodeColumn]]),
                StreetAddressLine1 = Convert.ToString(reader[customFieldMapping[PatientRepositoryDescriptor.StreetAddressLine1]]),
                //StreetAddressLine2 = Convert.ToString(reader[customFieldMapping[PatientRepositoryDescriptor.StreetAddressLine2]]),
                //StreetAddressLine3 = Convert.ToString(reader[customFieldMapping[PatientRepositoryDescriptor.StreetAddressLine3]]),
                PhoneHome = Convert.ToString(reader[customFieldMapping[PatientRepositoryDescriptor.PhoneHomeColumn]]),
                PhoneOffice = Convert.ToString(reader[customFieldMapping[PatientRepositoryDescriptor.PhoneOfficeColumn]])

            };
        }

        #endregion
    }
}

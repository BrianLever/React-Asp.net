using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using RPMS.Common;
using RPMS.Common.Builders;
using RPMS.Common.Models;
using ScreenDox.EHR.Common.Properties;
using RPMS.Common.Security;
using RPMS.Data.BMXNet.Framework;

namespace RPMS.Data.BMXNet
{
    public class BmxPatientRepository : BMXNetDatabase, IPatientRepository
    {

        public BmxPatientRepository() { }
		
        public BmxPatientRepository(IRpmsCredentialsService credentialsService) : base(credentialsService) { }

        private readonly Dictionary<string, string> _bmxPatientRecordMapping = new Dictionary<string, string>
        {
            {"RowId", "RowId"},
            {"HEALTH_RECORD_NO_", "HEALTH_RECORD_NO_"},
            {"NAME", "NAME"},
            {"DATE_OF_BIRTH", "DATE_OF_BIRTH"},
            {"StateID", "StateID"},
            {"CITY", "CITY"},
            {"ZIP_CODE", "ZIP_CODE"},
            {"STREET_ADDRESS_LINE_1", "STREET_ADDRESS_LINE_1"},
            //{"STREET_ADDRESS_LINE_2", "VA_PATIENT.STREET_ADDRESS_[LINE_2]"},
            //{"STREET_ADDRESS_LINE_3", "VA_PATIENT.STREET_ADDRESS_[LINE_3]"},
            {"PHONE_NUMBER_RESIDENCE", "PHONE_NUMBER_RESIDENCE"},
            {"PHONE_NUMBER_WORK", "PHONE_NUMBER_WORK"},
        };

        private readonly Dictionary<PatientRecordExportFields, string> _bmxPatientRecordFieldIds = new Dictionary<PatientRecordExportFields, string>
        {
            {PatientRecordExportFields.AddressLine1, ".111"},
            {PatientRecordExportFields.AddressLine2, ".112"},
            {PatientRecordExportFields.AddressLine3, ".113"},
            {PatientRecordExportFields.City, ".114"},
            {PatientRecordExportFields.StateID, ".115"},
            {PatientRecordExportFields.ZipCode, ".116"},
            {PatientRecordExportFields.Phone, ".131"}
        };

        enum PatientSystemFields
        {
            ADDRESS_CHANGE_DT,
            ADDRESS_CHANGE_SOURCE,
            ADDRESS_CHANGE_SITE,
            COUNTY
        }

        private static Dictionary<PatientSystemFields, string> _bmxPatientRecordSystemFieldIds = new Dictionary<PatientSystemFields, string>
        {
            {PatientSystemFields.ADDRESS_CHANGE_DT, ".118"},
            {PatientSystemFields.ADDRESS_CHANGE_SOURCE, ".119"},
            {PatientSystemFields.ADDRESS_CHANGE_SITE, ".12"},
            {PatientSystemFields.COUNTY, ".117"},
        };



      
        public virtual List<Common.Models.Patient> GetMatchedPatients(Patient patient)
        {

            if (patient == null)
            {
                throw new ArgumentNullException("patient");
            }
            List<Patient> patients = new List<Patient>();
            int MAX_RETURN_ROWS = 1000;

			
            string sqlTempl = @"SELECT 
VA_PATIENT.BMXIEN 'RowID',
VA_PATIENT.NAME 'NAME',
VA_PATIENT.ZIP_CODE 'ZIP_CODE',
VA_PATIENT.CITY 'CITY',
PATIENT.DOB DATE_OF_BIRTH,
PATIENT.HEALTH_RECORD_NO..HEALTH_RECORD_NO. 'HEALTH_RECORD_NO_',
STATE.ABBREVIATION StateID,
PATIENT.MAILING_ADDRESS-STREET 'STREET_ADDRESS_LINE_1',
PATIENT.HOME_PHONE 'PHONE_NUMBER_RESIDENCE',
PATIENT.OFFICE_PHONE 'PHONE_NUMBER_WORK'
FROM VA_PATIENT, PATIENT, STATE
WHERE VA_PATIENT.BMXIEN =* INTERNAL[PATIENT.NAME] 
    AND INTERNAL[VA_PATIENT.STATE] = STATE.BMXIEN 
    AND VA_PATIENT.NAME LIKE '{1}' 
MAXRECORDS:{0}";


            var lastNameSearchPattern = patient.LastName + ",";

            if(lastNameSearchPattern.Contains('-'))
            {
                lastNameSearchPattern = lastNameSearchPattern.Substring(0, lastNameSearchPattern.IndexOf('-'));
            }
            if (lastNameSearchPattern.Contains(' '))
            {
                lastNameSearchPattern = lastNameSearchPattern.Substring(0, lastNameSearchPattern.IndexOf(' '));
            }
            if (lastNameSearchPattern.Contains("'"))
            {
                lastNameSearchPattern = lastNameSearchPattern.Replace("'", "''");
            }

            string sql = string.Format(sqlTempl,
                MAX_RETURN_ROWS,
                SqlParameterSafe(SqlLikeStringPrepeare(lastNameSearchPattern, LikeCondition.StartsWith))
                );

            try
            {
                Connect();

                #region Paging parameters
                    
                //CommandObject.Parameters.Clear();
                //AddParameter("@Name", DbType.String).Value = SqlLikeStringPrepeare(name, LikeCondition.StartsWith);

                #endregion


                using (var reader = RunSelectQuery(sql))
                {
                    DateTime dateOfBirth;

                    while (reader.Read() && (patients.Count > 0 || reader.IsValidRecordInResult()))
                    {
                        dateOfBirth = GetDataOfBirthFromCursor(reader);

                        if (patient.DateOfBirth == dateOfBirth)
                        {
                            patients.Add(CreatePatientFromCursor(reader));
                        }
                    }

                }
            }
            finally
            {
                Disconnect();
            }
            return patients;
        }

        IEntityBuilder<Patient> _patientBuilder = null;
        IEntityBuilder<DateTime> _dateOfBirthBuilder = null;

        private Patient CreatePatientFromCursor(IDataReader reader)
        {
            if (_patientBuilder == null)
            {
                _patientBuilder = EntityBuilderFactory.GetPatientBuilder();
            }
            return _patientBuilder.CreateFromDbReader(reader, _bmxPatientRecordMapping);
        }

        private DateTime GetDataOfBirthFromCursor(IDataReader reader)
        {
            if (_dateOfBirthBuilder == null)
            {
                _dateOfBirthBuilder = EntityBuilderFactory.GetPatientDateOfBirthBuilder();
            }
            return _dateOfBirthBuilder.CreateFromDbReader(reader, _bmxPatientRecordMapping);
        }

        /// <summary>
        /// Get # of matched patients
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        public virtual int GetMatchedPatientsCount(Patient patient)
        {
            if (patient == null)
            {
                throw new ArgumentNullException("patient");
            }

            int count = 0;
            DateTime dob;

            string sqlTempl = @"SELECT 
VA_PATIENT.BMXIEN, PATIENT.DOB DATE_OF_BIRTH 
FROM VA_PATIENT, PATIENT, STATE
WHERE VA_PATIENT.BMXIEN =* INTERNAL[PATIENT.NAME] 
    AND INTERNAL[VA_PATIENT.STATE] = STATE.BMXIEN 
    AND VA_PATIENT.NAME LIKE '{0}'";


            string sql = string.Format(sqlTempl, SqlParameterSafe(SqlLikeStringPrepeare(patient.LastName + ",", LikeCondition.StartsWith)));

            try
            {
                Connect();

                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read() && (count > 0 || reader.IsValidRecordInResult()))
                    {
                        dob = GetDataOfBirthFromCursor(reader);
                        if (patient.DateOfBirth == dob)
                        {
                            ++count;
                        }
                    }
                }
            }
            finally
            {
                Disconnect();
            }
            return count;
        }

        public virtual Common.Models.Patient GetPatientRecord(PatientSearch patientSearch)
        {
            if (patientSearch == null)
            {
                throw new ArgumentNullException(nameof(patientSearch));
            }

            Patient patient = null;

            int patientId = patientSearch.ID;


            string sql = string.Format(@"SELECT 
VA_PATIENT.BMXIEN 'RowID',
VA_PATIENT.NAME 'NAME',
VA_PATIENT.ZIP_CODE 'ZIP_CODE',
VA_PATIENT.CITY 'CITY',
PATIENT.DOB DATE_OF_BIRTH,
PATIENT.HEALTH_RECORD_NO..HEALTH_RECORD_NO. 'HEALTH_RECORD_NO_',
STATE.ABBREVIATION StateID,
PATIENT.MAILING_ADDRESS-STREET 'STREET_ADDRESS_LINE_1',
PATIENT.HOME_PHONE 'PHONE_NUMBER_RESIDENCE',
PATIENT.OFFICE_PHONE 'PHONE_NUMBER_WORK'
FROM VA_PATIENT, PATIENT, STATE
WHERE VA_PATIENT.BMXIEN =* INTERNAL[PATIENT.NAME] 
    AND INTERNAL[VA_PATIENT.STATE] = STATE.BMXIEN  
    AND VA_PATIENT.BMXIEN = {0}", patientId);

            try
            {
                Connect();


                using (var reader = RunSelectQuery(sql))
                {
                    if (reader.Read())
                    {
                        patient = CreatePatientFromCursor(reader);

                    }
                }
            }
            finally
            {
                Disconnect();
            }
            return patient;

        }
        /// <summary>
        /// Update patient's record
        /// </summary>
        /// <param name="modifications"></param>
        /// <param name="patientId"></param>
        /// <param name="visitId"></param>
        /// <returns># of affected rows</returns>
        public virtual int UpdatePatientRecordFields(IEnumerable<Common.Models.PatientRecordModification> modifications, int patientId, int visitId)
        {
            char fieldDelimeter = (char)30;

            int affectedRows = 0;
            StringBuilder sql = new StringBuilder(@"UPDATE 2^"); // 2 - is a VA_PATIENT file ID

            sql.AppendFormat("{0}^", patientId); //append row ID

            sql.AppendFormat("{0}|{1}",
               _bmxPatientRecordSystemFieldIds[PatientSystemFields.ADDRESS_CHANGE_DT],
               DateTime.Today.ToString("M/d/yyyy", CultureInfo.InvariantCulture)
               );
            sql.AppendFormat("{2}{0}|{1}",
               _bmxPatientRecordSystemFieldIds[PatientSystemFields.ADDRESS_CHANGE_SOURCE],
               Resources.Provider,
               fieldDelimeter
            );


            bool isCityOrStateUpdated = false;
            foreach (var field in modifications)
            {
                sql.AppendFormat("{2}{0}|{1}",
                    _bmxPatientRecordFieldIds[field.Field],
                    field.UpdateWithValue,
                    fieldDelimeter);

                if (!isCityOrStateUpdated && (field.Field == PatientRecordExportFields.City || field.Field == PatientRecordExportFields.StateID))
                {
                    isCityOrStateUpdated = true;
                }
            }

            if (isCityOrStateUpdated) //if city or state is updating, clear COUNTY
            {
                sql.AppendFormat("{2}{0}|{1}",
                    _bmxPatientRecordSystemFieldIds[PatientSystemFields.COUNTY],
                    "",
                    fieldDelimeter);
            }

            Connect();
            try
            {
                string sqlStr = sql.ToString();
                System.Diagnostics.Debug.WriteLine("Patient update SQL: " + sqlStr);

                affectedRows = RunNonSelectQuery(sqlStr);
            }
            finally
            {
                Disconnect();
            }
            return affectedRows;
        }



        public virtual string GetPatientName(PatientSearch patientSearch)
        {
            if (patientSearch == null)
            {
                throw new ArgumentNullException(nameof(patientSearch));
            }
            int patientId = patientSearch.ID;
            string patientName = String.Empty;


            string sql = string.Format(@"SELECT 
VA_PATIENT.NAME 'NAME',
FROM VA_PATIENT
WHERE VA_PATIENT.BMXIEN = {0}", patientId);

            try
            {
                Connect();


                using (var reader = RunSelectQuery(sql))
                {
                    if (reader.Read() && reader.IsValidRecordInResult())
                    {
                        patientName = Convert.ToString(reader[0]);

                    }
                }
            }
            finally
            {
                Disconnect();
            }
            return patientName;
        }
    }
}

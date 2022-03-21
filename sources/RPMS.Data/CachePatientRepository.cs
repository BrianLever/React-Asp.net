using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using RPMS.Common;
using RPMS.Common.Models;
using RPMS.Data.Descriptors;
using System.Diagnostics;
using RPMS.Common.Builders;

namespace RPMS.Data
{
    public class CachePatientRepository : CacheDatabase, IPatientRepository
    {

        #region Shared Queries

        private const string patientInfoSql = @"
SELECT TOP {1} 
    p.RowId,
    hr.HEALTH_RECORD_NO_,
    vp.NAME,
    vp.DATE_OF_BIRTH,
    st.ABBREVIATION as StateID, 
    vp.CITY, 
    vp.ZIP_CODE, 
    vp.STREET_ADDRESS_LINE_1,
    vp.STREET_ADDRESS_LINE_2,
    vp.STREET_ADDRESS_LINE_3, 
    vp.PHONE_NUMBER_RESIDENCE, 
    vp.PHONE_NUMBER_WORK  
FROM {0}.PATIENT p INNER JOIN {0}.VA_PATIENT vp ON p.NAME = vp.RowID 
    INNER JOIN {0}.STATE st ON st.RowId= vp.STATE  
    INNER JOIN {0}.HEALTH_RECORD_NO__9000001C9000001_41 hr ON p.RowId = hr.PATIENT
";

        #endregion


        public List<Patient> GetMatchedPatients(Patient patient)
        {
            List<Patient> patients = new List<Patient>();
            


            string sql = string.Format(
                patientInfoSql +
"WHERE vp.NAME %STARTSWITH ? AND vp.DATE_OF_BIRTH = ? ",
                DbDescriptor.DatabaseSchemaName,
                1000);

            ///NOTE: Don't forget to remove \r befor run sql

            //string sql = 
            //    "SELECT NAME ,SEX,DOB " + 
            //    "FROM IHS.VA_PATIENT  " + 
            //    "WHERE NAME  = ? ";

            #region Paging parameters

            CommandObject.Parameters.Clear();
            AddParameter("name", DbType.String).Value = patient.LastName + ",";
            AddParameter("dob", DbType.Date).Value = patient.DateOfBirth;

            #endregion

            try
            {
                Connect();
                using (var reader = RunSelectQuery(sql))
                {
                    int rowNum = 0;
                    while (reader.Read())
                    {
                        patients.Add(CreatePatientFromCursor(reader));
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                Disconnect();
            }
            return patients;
        }


        public Patient GetPatientRecord(int patientID)
        {
            Patient patient = null;


            string sql = string.Format(patientInfoSql +
"WHERE p.RowId = ? ",
                DbDescriptor.DatabaseSchemaName,
                1);

            #region Paging parameters

            CommandObject.Parameters.Clear();
            AddParameter("rowId", DbType.Int32).Value = patientID;

            #endregion

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
            catch
            {
                throw;
            }
            finally
            {
                Disconnect();
            }
            return patient;
        }

        public int GetMatchedPatientsCount(Patient patient)
        {
            int count = 0;

            string sql = string.Format(@"
SELECT COUNT(*)  
FROM {0}.PATIENT p INNER JOIN {0}.VA_PATIENT vp ON p.NAME = vp.RowID 
    INNER JOIN {0}.STATE st ON st.RowId= vp.STATE  
WHERE vp.NAME %STARTSWITH ? AND vp.DATE_OF_BIRTH = ? ",
                DbDescriptor.DatabaseSchemaName
                );

            CommandObject.Parameters.Clear();
            AddParameter("name", DbType.String).Value = patient.LastName + ",";
            AddParameter("dob", DbType.Date).Value = patient.DateOfBirth;

            try
            {
                Connect();
                count = Convert.ToInt32(RunScalarQuery(sql));
            }
            catch
            {
                throw;
            }
            finally
            {
                Disconnect();
            }
            return count;
        }

        IEntityBuilder<Patient> _patientBuilder = null;

        private Patient CreatePatientFromCursor(IDataReader reader)
        {
            if (_patientBuilder == null)
            {
                _patientBuilder = EntityBuilderFactory.GetPatientBuilder();
            }
            return _patientBuilder.CreateFromDbReader(reader);
        }




        #region IPatientRepository Members


        public int UpdatePatientRecordFields(IEnumerable<PatientRecordModification> modifications, int patientId, int visitId)
        {
            int affectedRows = 0;
            if (modifications == null || !modifications.Any())
            {
                return affectedRows;
            }

            StringBuilder strUpdate = new StringBuilder();
            StringBuilder strWhere = new StringBuilder(" WHERE ");


            List<IDbDataParameter> updateParameters = new List<IDbDataParameter>();
            List<IDbDataParameter> whereParameters = new List<IDbDataParameter>();
            IDbDataParameter currentParam, updateParam;


            strUpdate.AppendFormat("UPDATE {0}.VA_PATIENT SET ",
                DbDescriptor.DatabaseSchemaName);

            CommandObject.Parameters.Clear();
            int i = 0;
            foreach (var m in modifications)
            {
                string column = string.Empty;
                currentParam = CreateParameter("curParam" + i, DbType.String);
                updateParam = CreateParameter("updateParam" + i, DbType.String);

                updateParam.Value = SqlParameterSafe(m.UpdateWithValue);
                currentParam.Value = SqlParameterSafe(m.CurrentValue);

               

                if (m.Field == PatientRecordExportFields.StateID)
                {
                    column = "STATE";
                    //lookup state id and update the id
                    strUpdate.AppendFormat("{0} {1}=(select RowId from Moonwalk.STATE where ABBREVIATION=?)", i > 0 ? "," : "", column);
                    strWhere.AppendFormat("{0} {1}=(select RowId from Moonwalk.STATE where ABBREVIATION=?) ", i > 0 ? "AND" : "", column);

                    updateParameters.Add(updateParam);
                    whereParameters.Add(currentParam);
                }
                else
                {
                    switch (m.Field)
                    {
                        case PatientRecordExportFields.AddressLine1:
                            column = "STREET_ADDRESS_LINE_1";
                            break;
                        case PatientRecordExportFields.AddressLine2:
                            column = "STREET_ADDRESS_LINE_2";
                            break;
                        case PatientRecordExportFields.AddressLine3:
                            column = "STREET_ADDRESS_LINE_3";
                            break;
                        case PatientRecordExportFields.City:
                            column = "CITY";
                            break;
                        case PatientRecordExportFields.ZipCode:
                            column = "ZIP_CODE";
                            break;
                        case PatientRecordExportFields.Phone:
                            column = "PHONE_NUMBER_RESIDENCE";
                            break;

                    }
                    updateParameters.Add(updateParam);
                
                   

                    strUpdate.AppendFormat("{0} {1}=?", i > 0 ? "," : "", column);
                    if (!Convert.IsDBNull(currentParam.Value))
                    {
                        strWhere.AppendFormat("{0} {1}=? ", i > 0 ? "AND" : "", column);
                        whereParameters.Add(currentParam);
                    }
                    else
                    {
                        strWhere.AppendFormat("{0} {1} IS NULL ", i > 0 ? "AND" : "", column);
                    }
                }



                i++;
            }

            currentParam = CreateParameter("patientID", DbType.AnsiString);
            currentParam.Value = patientId;
            whereParameters.Add(currentParam);
           
            
            strWhere.AppendFormat("{0} RowId=? ", i > 0 ? "AND" : "");
            strUpdate.Append(strWhere.ToString());

            foreach (var par in updateParameters)
            {
                CommandObject.Parameters.Add(par);
            }
            foreach (var par in whereParameters)
            {
                CommandObject.Parameters.Add(par);
            }   

            try
            {
                Connect();
               

                Debug.WriteLine("Patient update sql: " + strUpdate.ToString());
                affectedRows = RunNonSelectQuery(strUpdate.ToString());
            }
            finally
            {
                Disconnect();
            }

            return affectedRows;
        }

        public string GetPatientName(int patientID)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

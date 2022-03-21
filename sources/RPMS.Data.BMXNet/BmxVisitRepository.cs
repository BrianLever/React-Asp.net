using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPMS.Common;
using RPMS.Common.Security;
using RPMS.Data.BMXNet.Framework;
using RPMS.Common.Models;
using System.Data;


namespace RPMS.Data.BMXNet
{
    public class BmxVisitRepository : BMXNetDatabase, IVisitRepository
    {
        public BmxVisitRepository() { }

        public BmxVisitRepository(IRpmsCredentialsService credentialsService) : base(credentialsService) { }

        public virtual List<Visit> GetVisitsByPatient(int patientID, int startRow, int maxRows)
        {
            var result = new List<Visit>();
            //paging
            int totalRows = startRow + maxRows;

            string sqlTempl = @"SELECT 
VISIT.BMXIEN 'VisitID', 
VISIT.VISIT/ADMIT_DATE&TIME 'VISITADMIT_DATETIME',
VISIT.SERVICE_CATEGORY 'SERVICE_CATEGORY',
INTERNAL[VISIT.PATIENT_NAME] 'PatientID',
VISIT.PATIENT_NAME, 
VISIT.LOC._OF_ENCOUNTER 'LocationName'
FROM VISIT
WHERE INTERNAL[VISIT.PATIENT_NAME][ = {1}
";


            string sql = string.Format(sqlTempl, totalRows, patientID);

            try
            {
                Connect();

                using (var reader = RunSelectQuery(sql))
                {
                    //if reading first record, check if it is not contains the error message
                    while (reader.Read() && (result.Count > 0 || reader.IsValidRecordInResult()))
                    {
                        result.Add(CreateVisitFromReader(reader));
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

            //sort and trim

            var query = result.OrderByDescending(x => x.Date).Skip(startRow).Take(maxRows);

            return query.ToList();

        }

        public virtual List<Visit> GetVisitsByPatient(PatientSearch patientSearch, int startRow, int maxRows)
        {
            List<Visit> result = new List<Visit>();
            //paging
            int totalRows = startRow + maxRows;

            string sqlTempl = @"SELECT 
VISIT.BMXIEN 'VisitID', 
VISIT.VISIT/ADMIT_DATE&TIME 'VISITADMIT_DATETIME',
VISIT.SERVICE_CATEGORY 'SERVICE_CATEGORY',
VISIT.PATIENT_NAME, 
VISIT.LOC._OF_ENCOUNTER 'LocationName',
PATIENT.DOB,
INTERNAL[PATIENT.NAME] 'PatientID'
FROM VISIT, PATIENT
WHERE VISIT.PATIENT_NAME LIKE '{1}' 
		AND INTERNAL[VISIT.PATIENT_NAME] = INTERNAL[PATIENT.NAME]
";


            string sql = string.Format(sqlTempl, totalRows, SqlParameterSafe(patientSearch.LastName));

            try
            {
                Connect();
                int? rowPatientID;

                using (var reader = RunSelectQuery(sql))
                {
                    //if reading first record, check if it is not contains the error message
                    while (reader.Read() && (result.Count > 0 || reader.IsValidRecordInResult()))
                    {
                        rowPatientID = !Convert.IsDBNull(reader["PatientID"]) ? Convert.ToInt32(reader["PatientID"]) : (int?)null;

                        if (rowPatientID.HasValue && rowPatientID == patientSearch.ID)
                        {
                            result.Add(CreateVisitFromReader(reader));
                        }
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

            //sort and trim

            var query = result.OrderByDescending(x => x.Date).Skip(startRow).Take(maxRows);

            return query.ToList();
        }





        public virtual int GetVisitsByPatientCount(int patientID)
        {

            int count = 0;
            string sqlTempl = @"SELECT 
VISIT.BMXIEN,
VISIT.VISIT/ADMIT_DATE&TIME 'VISITADMIT_DATETIME', 
FROM VISIT
WHERE INTERNAL[VISIT.PATIENT_NAME] = {0}
";


            string sql = string.Format(sqlTempl, patientID);

            try
            {
                Connect();

                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read() && (count > 0 || reader.IsValidRecordInResult()))
                    {
                        ++count;
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
            return count;

        }

        public virtual int GetVisitsByPatientCount(PatientSearch patientSearch)
        {

            int count = 0;
            string sqlTempl = @"SELECT 
VISIT.BMXIEN 'VisitID', 
VISIT.VISIT/ADMIT_DATE&TIME 'VISITADMIT_DATETIME',
INTERNAL[PATIENT.NAME] 'PatientID'
FROM VISIT, PATIENT
WHERE VISIT.PATIENT_NAME LIKE '{0}' 
		AND INTERNAL[VISIT.PATIENT_NAME] = INTERNAL[PATIENT.NAME]
";


            string sql = string.Format(sqlTempl, SqlParameterSafe(patientSearch.LastName));

            try
            {
                Connect();

                using (var reader = RunSelectQuery(sql))
                {
                    int? rowPatientID;

                    while (reader.Read() && (count > 0 || reader.IsValidRecordInResult()))
                    {
                        rowPatientID = !Convert.IsDBNull(reader["PatientID"]) ? Convert.ToInt32(reader["PatientID"]) : (int?)null;
                        if (rowPatientID.HasValue && rowPatientID == patientSearch.ID)
                        {
                            ++count;
                        }
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
            return count;

        }


        public virtual Common.Models.Visit GetVisitRecord(int visitId, PatientSearch patientSearch)
        {
            Visit result = null;


            string sqlTempl = @"SELECT 
VISIT.BMXIEN 'VisitID', 
VISIT.VISIT/ADMIT_DATE&TIME 'VISITADMIT_DATETIME',
VISIT.SERVICE_CATEGORY 'SERVICE_CATEGORY',
VISIT.PATIENT_NAME, 
VISIT.LOC._OF_ENCOUNTER 'LocationName'
FROM VISIT
WHERE VISIT.BMXIEN = {0}
";


            string sql = string.Format(sqlTempl, visitId);

            try
            {
                Connect();

                using (var reader = RunSelectQuery(sql))
                {
                    if (reader.Read())
                    {
                        result = CreateVisitFromReader(reader);
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
            return result;

        }

        internal Visit CreateVisitFromReader(IDataReader reader)
        {
            return new Visit
            {
                ID = Convert.ToInt32(reader["VisitID"]),
                Date = Convert.ToDateTime(reader["VISITADMIT_DATETIME"]),
                ServiceCategory = /*Visit.GetServiceCategoryName(*/Convert.ToString(reader["SERVICE_CATEGORY"])/*)*/,
                Location = CreateLocationFromReader(reader)
            };
        }

        internal Location CreateLocationFromReader(IDataReader reader)
        {

            return new Location
            {
                ID = 0,
                //    ID = Convert.ToInt32(reader["LocationID"]),
                Name = Convert.ToString(reader["LocationName"])
            };
        }
    }
}

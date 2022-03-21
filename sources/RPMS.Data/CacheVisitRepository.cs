using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPMS.Common;
using System.Data;
using RPMS.Data.Descriptors;
using RPMS.Common.Models;

namespace RPMS.Data
{
    public class CacheVisitRepository : CacheDatabase, IVisitRepository
    {
        #region IVisitRepository Members

        public List<Visit> GetVisitsByPatient(int patientID, string patientName, int startRow, int maxRows)
        {
            return GetVisitsByPatient(patientID, startRow, maxRows);
        }

        public List<Visit> GetVisitsByPatient(int patientID, int startRow, int maxRows)
        {
            List<Visit> results = new List<Visit>();

            //paging
            int totalRows = startRow + maxRows;

            string sql = string.Format(@"
SELECT TOP({1}) 
vz.RowId as VisitID,
vz.VISITADMIT_DATETIME,
vz.SERVICE_CATEGORY,
vz.PATIENT_NAME as PatientID,
vz.LOC__OF_ENCOUNTER as LocationID,
inst.Name as LocationName
FROM {0}.VISIT vz 
INNER JOIN {0}.Patient p ON p.RowId = vz.PATIENT_NAME
INNER JOIN {0}.VA_PATIENT vp ON vp.RowId = p.Name
INNER JOIN {0}.LOCATION loc ON loc.RowId  = vz.LOC__OF_ENCOUNTER
INNER JOIN {0}.INSTITUTION inst ON inst.RowId = loc.Name
where vp.RowId = ?
order by vz.VISITADMIT_DATETIME DESC
", DbDescriptor.DatabaseSchemaName,
 totalRows
 );

            //set parameters

            AddParameter("PatientID", DbType.Int32).Value = patientID;

            try
            {
                using (var rd = RunSelectQuery(sql))
                {
                    int rowNum = 0;
                    while (rd.Read())
                    {
                        if (rowNum >= startRow)
                        {
                            results.Add(CreateVisitFromReader(rd));
                        }
                        rowNum++;
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
            return results;
        }

        public int GetVisitsByPatientCount(int patientID, string patientName)
        {
            return GetVisitsByPatientCount(patientID);
        }

        #endregion


        internal Visit CreateVisitFromReader(IDataReader reader)
        {
            return new Visit
            {
                ID = Convert.ToInt32(reader["VisitID"]),
                Date = Convert.ToDateTime(reader["VISITADMIT_DATETIME"]),
                ServiceCategory = Visit.GetServiceCategoryName(Convert.ToString(reader["SERVICE_CATEGORY"])),
                Location = CreateLocationFromReader(reader)
            };
        }

        internal Location CreateLocationFromReader(IDataReader reader)
        {
            return new Location
            {
                ID = Convert.ToInt32(reader["LocationID"]),
                Name = Convert.ToString(reader["LocationName"])
            };
        }

        public int GetVisitsByPatientCount(int patientID)
        {
            int count = 0;

            string sql = string.Format(@"
SELECT COUNT(*)
FROM {0}.VISIT vz 
INNER JOIN {0}.Patient p ON p.RowId = vz.PATIENT_NAME
INNER JOIN {0}.VA_PATIENT vp ON vp.RowId = p.Name
INNER JOIN {0}.LOCATION loc ON loc.RowId  = vz.LOC__OF_ENCOUNTER
INNER JOIN {0}.INSTITUTION inst ON inst.RowId = loc.Name
where vp.RowId = ?
", DbDescriptor.DatabaseSchemaName
 );

            //set parameters
            CommandObject.Parameters.Clear();
            AddParameter("PatientID", DbType.Int32).Value = patientID;

            try
            {
                using (var rd = RunSelectQuery(sql))
                {
                    if (rd.Read())
                    {
                        count = Convert.ToInt32(rd[0]);
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

        public Visit GetVisitRecord(int visitID)
        {
            Visit result = null;

            string sql = string.Format(@"
SELECT TOP({1}) 
vz.RowId as VisitID,
vz.VISITADMIT_DATETIME,
vz.SERVICE_CATEGORY,
vz.PATIENT_NAME as PatientID,
vz.LOC__OF_ENCOUNTER as LocationID,
inst.Name as LocationName
FROM {0}.VISIT vz 
INNER JOIN {0}.Patient p ON p.RowId = vz.PATIENT_NAME
INNER JOIN {0}.VA_PATIENT vp ON vp.RowId = p.Name
INNER JOIN {0}.LOCATION loc ON loc.RowId  = vz.LOC__OF_ENCOUNTER
INNER JOIN {0}.INSTITUTION inst ON inst.RowId = loc.Name
where vz.RowId = ?
", DbDescriptor.DatabaseSchemaName,
 1
 );

            //set parameters

            AddParameter("VisitID", DbType.Int32).Value = visitID;

            try
            {
                using (var rd = RunSelectQuery(sql))
                {
                    if (rd.Read())
                    {
                        result = CreateVisitFromReader(rd);
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
    }


}

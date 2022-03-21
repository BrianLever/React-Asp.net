using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrontDesk.Common.Data;
using System.Data;
using InterSystems.Data.CacheClient;

namespace RPMS.Common.Data
{
    internal class VisitDatabase: CacheDatabase
    {
        internal List<Visit> GetPatientVisits(int patientID, int? locationID, int startRowNumber, int pageSize)
        {
            QueryBuilder inner = new QueryBuilder(@"
SELECT
vz.RowId as VisitID,
vz.VISITADMIT_DATETIME,
vz.SERVICE_CATEGORY,
vz.PATIENT_NAME as PatientID,
vz.LOC_OF_ENCOUNTER as LocationID,
inst.Name as LocationName
FROM IHS.VISIT vz 
INNER JOIN IHS.Patient p ON p.RowId = vz.PATIENT_NAME
INNER JOIN IHS.VA_PATIENT vp ON vp.RowId = p.Name
INNER JOIN IHS.LOCATION loc ON loc.RowId  = vz.LOC_OF_ENCOUNTER
INNER JOIN IHS.INSTITUTION inst ON inst.RowId = loc.Name
");
            CommandObject.Parameters.Clear();

            inner.TopValue = startRowNumber + pageSize - 1;
            inner.AppendOrderCondition("vz.VISITADMIT_DATETIME", OrderType.Desc);

            inner.AppendWhereCondition("vp.RowId = ?", ClauseType.And);
            AddParameter("RowId", CacheDbType.Int).Value = patientID;
            if (locationID.HasValue)
            {
                inner.AppendWhereCondition("loc.Name = ?", ClauseType.And);
                AddParameter("Name", CacheDbType.Int).Value = locationID.Value;
            }

            QueryBuilder outer = new QueryBuilder(
                String.Format(@"SELECT * FROM ({0})", inner.ToString()));
            outer.TopValue = pageSize;

            List<Visit> visits = new List<Visit>();

            try
            {
                Connect();
                using (IDataReader reader = RunSelectQuery(outer.ToString()))
                {
                    while (reader.Read())
                    {
                        visits.Add(new Visit(reader));
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

            return visits;
        }

        internal int GetPatientVisitsCount(int patientID, int? locationID)
        {
            QueryBuilder qb = new QueryBuilder(@"
SELECT count(*)
FROM IHS.VISIT vz 
INNER JOIN IHS.Patient p ON p.RowId = vz.PATIENT_NAME
INNER JOIN IHS.VA_PATIENT vp ON vp.RowId = p.Name
INNER JOIN IHS.LOCATION loc ON loc.RowId  = vz.LOC_OF_ENCOUNTER
");
            CommandObject.Parameters.Clear();


            qb.AppendWhereCondition("vp.RowId = ?", ClauseType.And);
            AddParameter("RowId", CacheDbType.Int).Value = patientID;
            if (locationID.HasValue)
            {
                qb.AppendWhereCondition("loc.Name = ?", ClauseType.And);
                AddParameter("Name", CacheDbType.Int).Value = locationID.Value;
            }

            try
            {
                Connect();
                return Convert.ToInt32(RunScalarQuery(qb.ToString()));
            }
            catch
            {
                throw;
            }
            finally
            {
                Disconnect();
            }

        }
    }
}

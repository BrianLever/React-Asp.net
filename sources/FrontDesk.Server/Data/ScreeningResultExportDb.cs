using System.Data;
using System.Data.Common;
using System.Linq;

using FrontDesk.Common.Data;
using FrontDesk.Server.Screening.Models;

namespace FrontDesk.Server.Data
{
    public interface IScreeningResultExportRepository
    {
        DataTable GetScreeningsResultsForExcelExport(SimpleFilterModel filter);
        DataTable GetUniqueScreeningsResultsForExcelExport(SimpleFilterModel filter);
        DataTable GetDrugsOfChoiceScreeningsResultsForExcelExport(SimpleFilterModel filter);
        DataTable GetUniqueDrugsOfChoiceScreeningsResultsForExcelExport(SimpleFilterModel filter);
        DataTable GetUniqueCombinedResultsForExcelExport(SimpleFilterModel simpleFilterModel);
    }

    public class ScreeningResultExportDb : DBDatabase, IScreeningResultExportRepository
    {
        #region Constructors
        public ScreeningResultExportDb() : base(0)
        {

        }

        internal ScreeningResultExportDb(DbConnection sharedConnection) : base(sharedConnection)
        {

        }



        #endregion

        public DataTable GetScreeningsResultsForExcelExport(SimpleFilterModel filter)
        {
            var qbSql = new QueryBuilder(@"
SELECT *
FROM dbo.vScreeningResultsForExcelExport r ");

            qbSql.AppendOrderCondition("ScreeningDate ASC");

            ClearParameters();

            if (filter.Location.HasValue)
            {
                // add join to kiosks table, choose kiosks by location
                qbSql.AppendWhereCondition("r.LocationID = @LocationID", ClauseType.And);
                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;
            }
            if (filter.StartDate.HasValue)
            {
                qbSql.AppendWhereCondition("r.ScreeningDate >= @StartDate", ClauseType.And);

                AddParameter("@StartDate", DbType.DateTimeOffset).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                qbSql.AppendWhereCondition("r.ScreeningDate < @EndDate", ClauseType.And);

                AddParameter("@EndDate", DbType.DateTimeOffset).Value = filter.EndDate.Value;
            }


            var ds = GetSimplyDataSetQuery(qbSql.ToString());

            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }

            return new DataTable();

        }

        public DataTable GetUniqueScreeningsResultsForExcelExport(SimpleFilterModel filter)
        {
            const string sql = "dbo.uspGetUniquePatientScreeningResultsForExcelExport";

            return GetProcedureFileredResults(sql, filter);
        }

        public DataTable GetDrugsOfChoiceScreeningsResultsForExcelExport(SimpleFilterModel filter)
        {
            var result = new DataTable();
            var qbSql = new QueryBuilder(
@"SELECT 
[ScreeDox Record No.],
[ScreeningDate],
[LastName],
[FirstName],
[MiddleName],
[Birthday],
[LocationID],
[Location],
[DemographicsId],
[Primary Drug],
[Secondary Drug],
[Tertiary Drug]

FROM dbo.vScreeningResultsForExcelExport r ");

            qbSql.AppendWhereCondition("[Primary Drug] is NOT NULL", ClauseType.And);
            qbSql.AppendOrderCondition("ScreeningDate ASC");

            ClearParameters();

            if (filter.Location.HasValue)
            {
                // add join to kiosks table, choose kiosks by location
                qbSql.AppendWhereCondition("r.LocationID = @LocationID", ClauseType.And);
                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;
            }
            if (filter.StartDate.HasValue)
            {
                qbSql.AppendWhereCondition("r.ScreeningDate >= @StartDate", ClauseType.And);

                AddParameter("@StartDate", DbType.DateTimeOffset).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                qbSql.AppendWhereCondition("r.ScreeningDate < @EndDate", ClauseType.And);

                AddParameter("@EndDate", DbType.DateTimeOffset).Value = filter.EndDate.Value;
            }


            var ds = GetSimplyDataSetQuery(qbSql.ToString());

            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }

            return new DataTable();
        }


        public DataTable GetUniqueDrugsOfChoiceScreeningsResultsForExcelExport(SimpleFilterModel filter)
        {
            const string sql = "dbo.uspGetUniqueDrugsOfChoiceScreeningsResultsForExcelExport";

            return GetProcedureFileredResults(sql, filter);
        }

        public DataTable GetUniqueCombinedResultsForExcelExport(SimpleFilterModel filter)
        {
            const string sql = "dbo.uspGetUniquePatientCombinedResultsForExcelExport";

            return GetProcedureFileredResults(sql, filter);
        }

        private DataTable GetProcedureFileredResults(string procedureName, SimpleFilterModel filter)
        {
            ClearParameters();
            AddParameter("@StartDate", DbType.DateTimeOffset).Value = filter.StartDate.Value;
            AddParameter("@EndDate", DbType.DateTimeOffset).Value = filter.EndDate.Value;
            AddParameter("@LocationID", DbType.Int32).Value = SqlParameterSafe(filter.Location);

            var ds = GetSimplyProcedureDataSetQuery(procedureName);

            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }

            return new DataTable();
        }
    }
}
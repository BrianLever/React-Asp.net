using Frontdesk.Server.SmartExport.Models;
using FrontDesk.Common.Data;
using FrontDesk.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using FrontDesk;

namespace Frontdesk.Server.SmartExport.Data
{
    public interface ISmartExportRepository: ITransactionalDatabase
    {
        List<ScreeningResultInfo> GetScreeningResultsForExport(int batchSize);
        List<ScreeningResultInfo> GetScreeningResults(DateTime startDate, int batchSize);
        ExportSummary GetExportSummary(DateTime? startDate, DateTime? endDate);
        bool LogExportResult(SmartExportLog model);

        SmartExportLog GetLogExportResult(Int64 screeningResultId);
    }

    public class SmartExportDb : DBDatabase, ISmartExportRepository
    {
        public SmartExportDb() : base(0) { }

        internal SmartExportDb(DbConnection sharedConnection) : base(sharedConnection) { }


        public List<ScreeningResultInfo> GetScreeningResultsForExport(int batchSize)
        {
            const string sql = "dbo.uspGetScreeningResultsForExport";
            var result = new List<ScreeningResultInfo>();

            ClearParameters();
            AddParameter("@BatchSize", System.Data.DbType.Int32).Value = batchSize;

            try
            {
                using (var reader = RunProcedureSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        result.Add(
                            new ScreeningResultInfo
                            {
                                ID = reader.GetInt64(0),
                                PatientName = reader.GetString(1),
                                Birthday = reader.Get<DateTime>(2, true)
                            });
                    }
                }
            }
            finally
            {
                Disconnect();
            }

            return result;
        }

        public List<ScreeningResultInfo> GetScreeningResults(DateTime startDate, int batchSize)
        {
            const string sql = /*"dbo.uspGetScreeningResults";*/
                @"    SELECT TOP (@BatchSize)
        r.ScreeningResultID, 
        r.PatientName,
        r.Birthday,
        r.CreatedDate
    FROM dbo.ScreeningResult r
    WHERE r.IsDeleted = 0 AND CreatedDate >= @StartDate
    ORDER BY r.CreatedDate ASC";

            var result = new List<ScreeningResultInfo>();

            ClearParameters();
            AddParameter("@BatchSize", System.Data.DbType.Int32).Value = batchSize;
            AddParameter("@StartDate", System.Data.DbType.DateTime).Value = startDate;

            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        result.Add(
                            new ScreeningResultInfo
                            {
                                ID = reader.GetInt64(0),
                                PatientName = reader.GetString(1),
                                Birthday = reader.Get<DateTime>(2, true),
                                CreatedDate = reader.Get<DateTimeOffset>(3, true)
                            });
                    }
                }
            }
            finally
            {
                Disconnect();
            }

            return result;
        }

        public ExportSummary GetExportSummary(DateTime? startDate, DateTime? endDate)
        {
            var result = new ExportSummary();

            var sql = new QueryBuilder(@"SELECT 
COUNT(*) as Cnt,
Succeed
FROM export.SmartExportLog");

            ClearParameters();

            sql.AppendGroupCondition("Succeed");
            sql.AppendOrderCondition("Succeed ASC");

            if (startDate.HasValue)
            {
                AddParameter("@StartDate", DbType.Date).Value = startDate;
                sql.AppendWhereCondition("ExportDate >= @StartDate", ClauseType.And);
            }

            if (endDate.HasValue)
            {
                AddParameter("@EndDate", DbType.Date).Value = endDate;
                sql.AppendWhereCondition("ExportDate < @EndDate", ClauseType.And);
            }
            try
            {
                using (var reader = RunSelectQuery(sql.ToString()))
                {
                    while (reader.Read())
                    {
                        int cnt = reader.Get<int>(0, true);
                        int succeed = reader.Get<int>(1, true);

                        if (succeed == 1)
                        {
                            result.Succeed = cnt;
                        }
                        else
                        {
                            result.Failed = cnt;
                        }

                    }
                }

                return result;
            }
            finally
            {
                Disconnect();
            }

        }

        public bool LogExportResult(SmartExportLog model)
        {
            const string sql = "[export].[uspLogExportResult]";

            ClearParameters();

            AddParameter("@ScreeningResultID", DbType.Int64).Value = model.ID;
            AddParameter("@Succeed", DbType.Boolean).Value = model.Succeed;
            AddParameter("@ExportDate", DbType.DateTimeOffset).Value = model.ExportDate;
            AddParameter("@FailedAttemptCount", DbType.Int32).Value = SqlParameterSafe(model.FailedAttemptCount ?? 0);
            AddParameter("@FailedReason", DbType.String, 256).Value = SqlParameterSafe(model.FailedReason);
            AddParameter("@LastError", DbType.String).Value = SqlParameterSafe(model.LastError);
            AddParameter("@Completed", DbType.Boolean).Value = model.Completed;

            try
            {
                return RunProcedureNonSelectQuery(sql) > 0;
            }
            finally
            {
                Disconnect();
            }
        }


        public SmartExportLog GetLogExportResult(Int64 screeningResultId)
        {
            SmartExportLog result = null;
            const string sql = @"
SELECT [ScreeningResultID]
,[Succeed]
,[ExportDate]
,[FailedAttemptCount]
,[FailedReason]
,[LastError]
,[Completed]
FROM [export].[SmartExportLog]
WHERE [ScreeningResultID] = @ScreeningResultID
";

            ClearParameters();
            AddParameter("@ScreeningResultID", DbType.Int64).Value = screeningResultId;
            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    if (reader.Read())
                    {
                        result = SmartExportLogFactory.CreateFromReader(reader);
                    }
                }
            }
            finally
            {
                Disconnect();
            }

            return result;
        }
    }
}

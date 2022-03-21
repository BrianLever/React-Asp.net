using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using FrontDesk.Common.Data;
using FrontDesk.Common.Extensions;
using FrontDesk.Common.Screening;
using FrontDesk.Server.Screening.Models;

namespace FrontDesk.Server.Data.BhsVisits
{
    public interface IScreeningTimeLogRepository
    {
        bool Add(long screeningResultID, ScreeningTimeLogRecord[] model);
        List<ScreeningTimeLogReportItem> GetReport(SimpleFilterModel filter);
    }

    public class ScreeningTimeLogDb : DBDatabase, IScreeningTimeLogRepository
    {

        public ScreeningTimeLogDb() : base(0) { }

        internal ScreeningTimeLogDb(DbConnection sharedConnection) : base(sharedConnection) { }


        public bool Add(long screeningResultID, ScreeningTimeLogRecord[] model)
        {
            bool result = true;
            model = model ?? new ScreeningTimeLogRecord[0];

            const string sql = @"
MERGE [dbo].[ScreeningTimeLog] as target
USING (SELECT @ScreeningResultID, @ScreeningSectionID,@StartDate,@EndDate) 
AS source(ScreeningResultID, ScreeningSectionID,StartDate,EndDate) 
ON (target.ScreeningResultID = source.ScreeningResultID AND ISNULL(target.ScreeningSectionID, '') = ISNULL(source.ScreeningSectionID, ''))
WHEN MATCHED THEN
	UPDATE SET target.StartDate = source.StartDate, target.EndDate = source.EndDate
WHEN NOT MATCHED THEN
	INSERT (ScreeningResultID, ScreeningSectionID,StartDate,EndDate)
	VALUES(source.ScreeningResultID, source.ScreeningSectionID, source.StartDate, source.EndDate)
;
";

            ClearParameters();

            AddParameter("@ScreeningResultID", DbType.Int64).Value = screeningResultID;

            try
            {
                Connect();
                BeginTransaction();


                var screeningSectionIDParam = AddParameter("@ScreeningSectionID", DbType.AnsiString, 5);
                var startDateParam = AddParameter("@StartDate", DbType.DateTimeOffset);
                var endDateParam = AddParameter("@EndDate", DbType.DateTimeOffset);


                foreach (var item in model)
                {
                    screeningSectionIDParam.Value =
                        SqlParameterSafe(
                            item.ScreeningSectionID == ScreeningTimeLog.ENTIRE_SCREENING_SECTION_ID ?
                            string.Empty :
                            item.ScreeningSectionID);
                    startDateParam.Value = item.Started;
                    endDateParam.Value = item.Ended;

                    RunNonSelectQuery(sql);
                }

                result = true;

                CommitTransaction();
            }
            catch
            {
                RollbackTransaction();
                result = false;
                throw;
            }
            finally
            {
                base.Disconnect();
            }
            return result;
        }

        public List<ScreeningTimeLogReportItem> GetReport(SimpleFilterModel filter)
        {
            var result = new List<ScreeningTimeLogReportItem>();


            var sql = new QueryBuilder(@"
SELECT
    ScreeningSectionName, 
    ScreeningSectionID, 
    OrderIndex,
    COUNT(*) as cnt,
    SUM(DurationInSeconds) as total_duration,
    AVG(DurationInSeconds) as avg_duration
FROM [dbo].[vScreeningTimeLogReport]
");

            sql.AppendGroupCondition("ScreeningSectionID, ScreeningSectionName, OrderIndex");
            sql.AppendOrderCondition("OrderIndex", OrderType.Asc);

            ClearParameters();


            if (filter.Location.HasValue)
            {
                sql.AppendWhereCondition("BranchLocationID = @BranchingLocationID", ClauseType.And);
                AddParameter("@BranchingLocationID", DbType.Int32).Value = filter.Location.Value;
            }

            if (filter.StartDate.HasValue)
            {
                sql.AppendWhereCondition("CreatedDate >= @StartDate", ClauseType.And);

                AddParameter("@StartDate", DbType.DateTime).Value = filter.StartDate.Value;

            }
            if (filter.EndDate.HasValue)
            {
                sql.AppendWhereCondition("CreatedDate < @EndDate", ClauseType.And);

                AddParameter("@EndDate", DbType.DateTime).Value = filter.EndDate.Value;

            }
            try
            {
                using (var reader = RunSelectQuery(sql.ToString()))
                {
                    while (reader.Read())
                    {

                        result.Add(new ScreeningTimeLogReportItem()
                        {
                            ScreeningSectionName = reader.Get<string>(0, false),
                            ScreeningSectionID = reader.Get<string>(1, false)?.Trim(),
                            NumberOfReports = reader.Get<int>("cnt"),
                            TotalTime = TimeSpan.FromSeconds(reader.Get<int>("total_duration")),
                            AverageTime = TimeSpan.FromSeconds(reader.Get<double>("avg_duration"))
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

    }
}

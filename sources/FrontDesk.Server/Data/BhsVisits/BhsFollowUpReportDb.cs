using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using FrontDesk.Common.Data;
using FrontDesk.Common.Extensions;
using FrontDesk.Server.Screening;
using FrontDesk.Common.Bhservice;
using FrontDesk.Services;
using FrontDesk.Server.Screening.Services;

namespace FrontDesk.Server.Data.BhsVisits
{
    public interface IBhsFollowUpReportRepository : ITransactionalDatabase
    {
        List<BhsIndicatorReportByAgeItem> GetUniquePatientReportByAge(string tableName, int? locationIdFilter, DateTime? startDate, DateTime? endDate);
        List<BhsIndicatorReportByAgeItem> GetPatientReportByAge(string tableName, int? locationIdFilter, DateTime? startDate, DateTime? endDate);
        List<BhsIndicatorReportByAgeItem> GetFollowUpPatientReportByAge(bool isUniquePatient, int? locationIdFilter, DateTime? startDate, DateTime? endDate);

    }

    public class BhsFollowUpReportDb : DBDatabase, IBhsFollowUpReportRepository
    {

        #region Constructors
        public BhsFollowUpReportDb() : base(0) { }

        internal BhsFollowUpReportDb(DbConnection sharedConnection) : base(sharedConnection) { }

        #endregion

        public List<BhsIndicatorReportByAgeItem> GetUniquePatientReportByAge(string tableName, int? locationIdFilter, DateTime? startDate, DateTime? endDate)
        {

            var reportItems = new List<BhsIndicatorReportByAgeItem>();

            var withSqlBuilder = new QueryBuilder(@"
SELECT 
	l.ID
    ,[dbo].[fn_GetAge](r.Birthday) as Age
FROM dbo.BhsFollowUp d
    INNER JOIN dbo.ScreeningResult r ON d.ScreeningResultID=r.ScreeningResultID
	INNER JOIN dbo.{0} l ON l.ID = d.{0}ID
".FormatWith(tableName));

            CommandObject.Parameters.Clear();
            withSqlBuilder.AppendWhereCondition("d.CompleteDate IS NOT NULL", ClauseType.And);

            withSqlBuilder.AppendGroupCondition("l.ID, r.PatientName, r.Birthday");

            if (locationIdFilter.HasValue)
            {
                withSqlBuilder.AppendJoinStatement("INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID");
                withSqlBuilder.AppendWhereCondition("k.BranchLocationID = @LocationID", ClauseType.And);
                AddParameter("@LocationID", DbType.Int32).Value = locationIdFilter.Value;

            }
            if (startDate.HasValue)
            {
                withSqlBuilder.AppendWhereCondition("d.CreatedDate >= @StartDate", ClauseType.And);

                AddParameter("@StartDate", DbType.DateTime).Value = startDate.Value;

            }
            if (endDate.HasValue)
            {
                withSqlBuilder.AppendWhereCondition("d.CreatedDate < @EndDate", ClauseType.And);

                AddParameter("@EndDate", DbType.DateTime).Value = endDate.Value;

            }
            string sql = string.Format(@"
WITH tblResults(ID, Age) AS
({0})
SELECT 
    '{1}' as CategoryId
	,l.ID 
	,l.Name
	,ISNULL(t2.TotalCount, 0) as TotalCount
    ,ISNULL(t2.Age, 0) as Age
FROM dbo.{1} l
LEFT JOIN (
    SELECT 
		r.ID
        ,COUNT_BIG(*) as TotalCount
        ,r.Age
    FROM tblResults r
    GROUP BY r.ID, r.Age
) t2 ON l.ID = t2.ID 
ORDER BY l.OrderIndex ASC, t2.Age ASC
", withSqlBuilder, tableName);

            try
            {

                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        var item = new BhsIndicatorReportByAgeItem()
                        {
                            CategoryID = reader.GetString(0),
                            IndicatorID = reader.Get<int>(1, true),
                            IndicatorName = reader.GetString(2),
                            Count = reader.Get<long>(3, true),
                            Age = reader.Get<int>(4, true)
                        };

                        reportItems.Add(item);
                    }

                }
            }
            finally
            {
                Disconnect();
            }
            return reportItems;
        }

        public List<BhsIndicatorReportByAgeItem> GetPatientReportByAge(string tableName, int? locationIdFilter, DateTime? startDate, DateTime? endDate)
        {

            var reportItems = new List<BhsIndicatorReportByAgeItem>();

            var withSqlBuilder = new QueryBuilder(@"
SELECT 
	l.ID
    ,[dbo].[fn_GetAge](r.Birthday) as Age
FROM dbo.BhsFollowUp d
    INNER JOIN dbo.ScreeningResult r ON d.ScreeningResultID=r.ScreeningResultID
	INNER JOIN dbo.{0} l ON l.ID = d.{0}ID
".FormatWith(tableName));

            CommandObject.Parameters.Clear();
            withSqlBuilder.AppendWhereCondition("d.CompleteDate IS NOT NULL", ClauseType.And);


            if (locationIdFilter.HasValue)
            {
                withSqlBuilder.AppendJoinStatement("INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID");
                withSqlBuilder.AppendWhereCondition("k.BranchLocationID = @LocationID", ClauseType.And);
                AddParameter("@LocationID", DbType.Int32).Value = locationIdFilter.Value;

            }
            if (startDate.HasValue)
            {
                withSqlBuilder.AppendWhereCondition("d.CreatedDate >= @StartDate", ClauseType.And);

                AddParameter("@StartDate", DbType.DateTime).Value = startDate.Value;

            }
            if (endDate.HasValue)
            {
                withSqlBuilder.AppendWhereCondition("d.CreatedDate < @EndDate", ClauseType.And);

                AddParameter("@EndDate", DbType.DateTime).Value = endDate.Value;

            }
            string sql = string.Format(@"
WITH tblResults(ID, Age) AS
({0})
SELECT 
    '{1}' as CategoryId
	,l.ID 
	,l.Name
	,ISNULL(t2.TotalCount, 0) as TotalCount
    ,ISNULL(t2.Age, 0) as Age
FROM dbo.{1} l
LEFT JOIN (
    SELECT 
		r.ID
        ,COUNT_BIG(*) as TotalCount
        ,r.Age
    FROM tblResults r
    GROUP BY r.ID, r.Age
) t2 ON l.ID = t2.ID 
ORDER BY l.OrderIndex ASC, t2.Age ASC
", withSqlBuilder, tableName);

            try
            {

                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        var item = new BhsIndicatorReportByAgeItem()
                        {
                            CategoryID = reader.GetString(0),
                            IndicatorID = reader.Get<int>(1, true),
                            IndicatorName = reader.GetString(2),
                            Count = reader.Get<long>(3, true),
                            Age = reader.Get<int>(4, true)
                        };

                        reportItems.Add(item);
                    }

                }
            }
            finally
            {
                Disconnect();
            }
            return reportItems;
        }

        public List<BhsIndicatorReportByAgeItem> GetFollowUpPatientReportByAge(bool isUniquePatient, int? locationIdFilter, DateTime? startDate, DateTime? endDate)
        {

            var reportItems = new List<BhsIndicatorReportByAgeItem>();

            var withSqlBuilder = new QueryBuilder(@"
SELECT 
	(CASE WHEN d.ThirtyDatyFollowUpFlag = 0 THEN 0 ELSE 1 END) as ThirtyDatyFollowUpFlag
    ,[dbo].[fn_GetAge](r.Birthday) as Age
FROM dbo.BhsFollowUp d
    INNER JOIN dbo.ScreeningResult r ON d.ScreeningResultID=r.ScreeningResultID
");

            CommandObject.Parameters.Clear();
            withSqlBuilder.AppendWhereCondition("d.CompleteDate IS NOT NULL", ClauseType.And);

            if (isUniquePatient)
            {
                withSqlBuilder.AppendGroupCondition("r.PatientName, r.Birthday, d.ThirtyDatyFollowUpFlag");
            }
            else
            {
                withSqlBuilder.AppendGroupCondition("d.ID, r.PatientName, r.Birthday, d.ThirtyDatyFollowUpFlag");
            }



            if (locationIdFilter.HasValue)
            {
                withSqlBuilder.AppendJoinStatement("INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID");
                withSqlBuilder.AppendWhereCondition("k.BranchLocationID = @LocationID", ClauseType.And);
                AddParameter("@LocationID", DbType.Int32).Value = locationIdFilter.Value;

            }
            if (startDate.HasValue)
            {
                withSqlBuilder.AppendWhereCondition("d.CreatedDate >= @StartDate", ClauseType.And);

                AddParameter("@StartDate", DbType.DateTime).Value = startDate.Value;

            }
            if (endDate.HasValue)
            {
                withSqlBuilder.AppendWhereCondition("d.CreatedDate < @EndDate", ClauseType.And);

                AddParameter("@EndDate", DbType.DateTime).Value = endDate.Value;

            }
            string sql = string.Format(@"
WITH tblResults(Cnt, Age) AS
({0})
SELECT 
    ISNULL(t2.Cnt, 0) as TotalCount
    ,ISNULL(t2.Age, 0) as Age
FROM tblResults t2
ORDER BY t2.Age ASC
", withSqlBuilder);

            try
            {

                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        var item = new BhsIndicatorReportByAgeItem()
                        {
                            CategoryID = "FollowUp",
                            IndicatorID = 1,
                            IndicatorName = "Requested",
                            Count = reader.Get<long>(0, true),
                            Age = reader.Get<int>(1, true)
                        };

                        reportItems.Add(item);
                    }

                }

                if (reportItems.Count == 0)
                {
                    reportItems.Add(new BhsIndicatorReportByAgeItem()
                    {
                        CategoryID = "FollowUp",
                        IndicatorID = 1,
                        IndicatorName = "Requested",
                        Count = 0,
                        Age = 0
                    });
                }
            }
            finally
            {
                Disconnect();
            }
            return reportItems;
        }


    }
}

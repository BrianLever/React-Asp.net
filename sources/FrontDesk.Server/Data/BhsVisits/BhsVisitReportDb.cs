using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using FrontDesk.Common.Data;
using FrontDesk.Common.Extensions;
using FrontDesk.Server.Screening;
using System.Diagnostics;
using FrontDesk.Common.Debugging;
using FrontDesk.Common.Bhservice;
using FrontDesk.Server.Controllers;
using FrontDesk.Services;
using FrontDesk.Server.Screening.Services;
using FrontDesk.Server.Printouts.Bhs;

namespace FrontDesk.Server.Data.BhsVisits
{
    public interface IBhsVisitReportRepository : ITransactionalDatabase
    {

        List<BhsIndicatorReportByAgeItem> GetTreatmentActionsReportByAge(int? locationIdFilter, DateTime? startDate, DateTime? endDate);
        List<BhsIndicatorReportByAgeItem> GetTreatmentActionsUniquePatientReportByAge(int? locationIdFilter, DateTime? startDate, DateTime? endDate);

        List<BhsIndicatorReportByAgeItem> GetUniquePatientReportByAge(string tableName, int? locationIdFilter, DateTime? startDate, DateTime? endDate);
        List<BhsIndicatorReportByAgeItem> GetPatientReportByAge(string tableName, int? locationIdFilter, DateTime? startDate, DateTime? endDate);
        List<BhsIndicatorReportByAgeItem> GetFollowUpPatientReportByAge(bool isUniquePatient, int? locationIdFilter, DateTime? startDate, DateTime? endDate);
    }

    public class BhsVisitReportDb : DBDatabase, IBhsVisitReportRepository
    {

        #region Constructors
        public BhsVisitReportDb() : base(0) { }

        internal BhsVisitReportDb(DbConnection sharedConnection) : base(sharedConnection) { }

        #endregion
        public List<BhsIndicatorReportByAgeItem> GetTreatmentActionsReportByAge(int? locationIdFilter, DateTime? startDate, DateTime? endDate)
        {

            var reportItems = new List<BhsIndicatorReportByAgeItem>();

            var withSqlBuilder = new QueryBuilder(@"
SELECT 
	o.CategoryID
	,o.TreatmentActionID
	,1 as Cnt
	,[dbo].[fn_GetAge](o.Birthday) as Age
	,o.PatientName
	,o.Birthday
FROM dbo.BhsVisit v
	INNER JOIN dbo.ScreeningResult r ON r.ScreeningResultID = v.ScreeningResultID
	CROSS APPLY (
		VALUES
			(v.ID, v.TreatmentAction1ID, 'TreatmentAction1', r.PatientName, r.Birthday)
			,(v.ID, ISNULL(v.TreatmentAction2ID, 0), 'TreatmentAction2', r.PatientName, r.Birthday)
			,(v.ID, ISNULL(v.TreatmentAction3ID, 0), 'TreatmentAction3', r.PatientName, r.Birthday)
			,(v.ID, ISNULL(v.TreatmentAction4ID, 0), 'TreatmentAction4', r.PatientName, r.Birthday)
			,(v.ID, ISNULL(v.TreatmentAction5ID, 0), 'TreatmentAction5', r.PatientName, r.Birthday)
		) o (VisitID, TreatmentActionID, CategoryID, PatientName, Birthday)
");

            CommandObject.Parameters.Clear();
            withSqlBuilder.AppendWhereCondition("v.CompleteDate IS NOT NULL", ClauseType.And);
            //withSqlBuilder.AppendGroupCondition("o.CategoryID, o.PatientName, o.Birthday, o.TreatmentActionID");


            if (locationIdFilter.HasValue)
            {
                withSqlBuilder.AppendWhereCondition("v.LocationID = @LocationID", ClauseType.And);
                AddParameter("@LocationID", DbType.Int32).Value = locationIdFilter.Value;

            }
            if (startDate.HasValue)
            {
                withSqlBuilder.AppendWhereCondition("v.CreatedDate >= @StartDate", ClauseType.And);

                AddParameter("@StartDate", DbType.DateTime).Value = startDate.Value;

            }
            if (endDate.HasValue)
            {
                withSqlBuilder.AppendWhereCondition("v.CreatedDate < @EndDate", ClauseType.And);

                AddParameter("@EndDate", DbType.DateTime).Value = endDate.Value;

            }
            string sql = string.Format(@"
WITH tblResults(CategoryID, TreatmentActionID, Cnt, Age, PatientName, Birthday) AS 
({0})
SELECT 
	cat.CategoryID
	,ta.ID
	,ta.Name as Name
	,ISNULL(t.TotalCnt, 0) as TotalCnt
    ,ISNULL(t.Age,0) as Age
FROM 
	(VALUES('TreatmentAction1'), ('TreatmentAction2'), ('TreatmentAction3'), ('TreatmentAction4'), ('TreatmentAction5')) cat (CategoryID) 
	CRoSS JOIN ( 
            SELECT ID, Name, OrderIndex
				FROM dbo.TreatmentAction  
			UNION ALL
			SELECT 0, 'None', 100
        ) ta 
		LEFT JOIN (
			SELECT 
				CategoryID,
				TreatmentActionID,
				Age,
				SUM(Cnt) as TotalCnt
			FROM tblResults
			GROUP BY tblResults.CategoryID, tblResults.TreatmentActionID, tblResults.Age
		) t ON ta.ID = t.TreatmentActionID AND t.CategoryID = cat.CategoryID
ORDER BY CategoryID, ta.OrderIndex ASC, Age
", withSqlBuilder);

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

        public List<BhsIndicatorReportByAgeItem> GetTreatmentActionsUniquePatientReportByAge(int? locationIdFilter, DateTime? startDate, DateTime? endDate)
        {

            var reportItems = new List<BhsIndicatorReportByAgeItem>();

            var withSqlBuilder = new QueryBuilder(@"
SELECT 
	o.CategoryID
	,o.TreatmentActionID
	,1 as Cnt
	,[dbo].[fn_GetAge](o.Birthday) as Age
	,o.PatientName
	,o.Birthday
FROM dbo.BhsVisit v
	INNER JOIN dbo.ScreeningResult r ON r.ScreeningResultID = v.ScreeningResultID
	CROSS APPLY (
		VALUES
			(v.ID, v.TreatmentAction1ID, 'TreatmentAction1', r.PatientName, r.Birthday)
		    ,(v.ID, ISNULL(v.TreatmentAction2ID, 0), 'TreatmentAction2', r.PatientName, r.Birthday)
			,(v.ID, ISNULL(v.TreatmentAction3ID, 0), 'TreatmentAction3', r.PatientName, r.Birthday)
			,(v.ID, ISNULL(v.TreatmentAction4ID, 0), 'TreatmentAction4', r.PatientName, r.Birthday)
			,(v.ID, ISNULL(v.TreatmentAction5ID, 0), 'TreatmentAction5', r.PatientName, r.Birthday)
		) o (VisitID, TreatmentActionID, CategoryID, PatientName, Birthday)
");

            CommandObject.Parameters.Clear();
            withSqlBuilder.AppendWhereCondition("v.CompleteDate IS NOT NULL", ClauseType.And);
            withSqlBuilder.AppendGroupCondition("o.CategoryID, o.PatientName, o.Birthday, o.TreatmentActionID");


            if (locationIdFilter.HasValue)
            {
                withSqlBuilder.AppendWhereCondition("v.LocationID = @LocationID", ClauseType.And);
                AddParameter("@LocationID", DbType.Int32).Value = locationIdFilter.Value;

            }
            if (startDate.HasValue)
            {
                withSqlBuilder.AppendWhereCondition("v.CreatedDate >= @StartDate", ClauseType.And);

                AddParameter("@StartDate", DbType.DateTime).Value = startDate.Value;

            }
            if (endDate.HasValue)
            {
                withSqlBuilder.AppendWhereCondition("v.CreatedDate < @EndDate", ClauseType.And);

                AddParameter("@EndDate", DbType.DateTime).Value = endDate.Value;

            }
            string sql = string.Format(@"
WITH tblResults(CategoryID, TreatmentActionID, Cnt, Age, PatientName, Birthday) AS 
({0})
SELECT 
	cat.CategoryID
	,ta.ID
	,ta.Name as Name
	,ISNULL(t.TotalCnt, 0) as TotalCnt
    ,ISNULL(t.Age, 0) as Age
FROM 
	(VALUES('TreatmentAction1'), ('TreatmentAction2'), ('TreatmentAction3'), ('TreatmentAction4'), ('TreatmentAction5')) cat (CategoryID) 
	CRoSS JOIN ( 
            SELECT ID, Name, OrderIndex
				FROM dbo.TreatmentAction  
			UNION ALL
			SELECT 0, 'None', 100
        ) ta  
		LEFT JOIN (
			SELECT 
				CategoryID,
				TreatmentActionID,
				Age,
				SUM(Cnt) as TotalCnt
			FROM tblResults
			GROUP BY tblResults.CategoryID, tblResults.TreatmentActionID, tblResults.Age
		) t ON ta.ID = t.TreatmentActionID AND t.CategoryID = cat.CategoryID
ORDER BY CategoryID, ta.OrderIndex ASC, Age
", withSqlBuilder);

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


        public List<BhsIndicatorReportByAgeItem> GetUniquePatientReportByAge(string tableName, int? locationIdFilter, DateTime? startDate, DateTime? endDate)
        {

            var reportItems = new List<BhsIndicatorReportByAgeItem>();

            var withSqlBuilder = new QueryBuilder(@"
SELECT 
	l.ID
    ,[dbo].[fn_GetAge](r.Birthday) as Age
FROM dbo.BhsVisit d
    INNER JOIN dbo.ScreeningResult r ON d.ScreeningResultID=r.ScreeningResultID
	INNER JOIN dbo.{0} l ON l.ID = d.{0}ID
".FormatWith(tableName));

            CommandObject.Parameters.Clear();
            withSqlBuilder.AppendWhereCondition("d.CompleteDate IS NOT NULL", ClauseType.And);

            withSqlBuilder.AppendGroupCondition("l.ID, r.PatientName, r.Birthday");

            if (locationIdFilter.HasValue)
            {
                withSqlBuilder.AppendWhereCondition("d.LocationID = @LocationID", ClauseType.And);
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
FROM dbo.BhsVisit d
    INNER JOIN dbo.ScreeningResult r ON d.ScreeningResultID=r.ScreeningResultID
	INNER JOIN dbo.{0} l ON l.ID = d.{0}ID
".FormatWith(tableName));

            CommandObject.Parameters.Clear();
            withSqlBuilder.AppendWhereCondition("d.CompleteDate IS NOT NULL", ClauseType.And);


            if (locationIdFilter.HasValue)
            {
                withSqlBuilder.AppendWhereCondition("d.LocationID = @LocationID", ClauseType.And);
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

        public List<BhsIndicatorReportByAgeItem> GetFollowUpPatientReportByAge(bool isUniquePatient,  int? locationIdFilter, DateTime? startDate, DateTime? endDate)
        {

            var reportItems = new List<BhsIndicatorReportByAgeItem>();

            var withSqlBuilder = new QueryBuilder(@"
SELECT 
	(CASE WHEN d.ThirtyDatyFollowUpFlag = 0 THEN 0 ELSE 1 END) as ThirtyDatyFollowUpFlag
    ,[dbo].[fn_GetAge](r.Birthday) as Age
FROM dbo.BhsVisit d
    INNER JOIN dbo.ScreeningResult r ON d.ScreeningResultID=r.ScreeningResultID
");

            CommandObject.Parameters.Clear();
            withSqlBuilder.AppendWhereCondition("d.CompleteDate IS NOT NULL", ClauseType.And);

            if(isUniquePatient)
            {
                withSqlBuilder.AppendGroupCondition("r.PatientName, r.Birthday, d.ThirtyDatyFollowUpFlag");
            }
            else
            {
                withSqlBuilder.AppendGroupCondition("d.ID, r.PatientName, r.Birthday, d.ThirtyDatyFollowUpFlag");
            }



            if (locationIdFilter.HasValue)
            {
                withSqlBuilder.AppendWhereCondition("d.LocationID = @LocationID", ClauseType.And);
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


                if(reportItems.Count == 0) // if empty result, insert single row
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

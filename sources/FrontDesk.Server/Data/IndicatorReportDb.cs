using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using FrontDesk.Common.Data;
using FrontDesk.Common.Extensions;
using FrontDesk.Server.Screening;
using System.Diagnostics;
using FrontDesk.Common.Debugging;
using Common.Logging;
using FrontDesk.Server.Screening.Models;

namespace FrontDesk.Server.Data
{
    public interface IIndicatorReportRepository : ITransactionalDatabase
    {
        List<IndicatorReportByAgeItem> GetIndicatorReportPositiveByAges(SimpleFilterModel filter);
        List<IndicatorReportByAgeItem> GetIndicatorReportUniquePatientsPositiveByAges(SimpleFilterModel filter);

        List<IndicatorReportByAgeItem> GetBhsIndicatorReportByScoreLevelAndAge(SimpleFilterModel filter);
        List<IndicatorReportByAgeItem> GetBhsIndicatorReportUniquePatientsByScoreLevelAndAge(SimpleFilterModel filter);

        List<IndicatorReportByAgeItem> GetBhsIndicatorReportByScoreLevelAndAgeForDepressionLike(
            SimpleFilterModel filter, string mainScreeningSectionID, string shortScreeningSectionID);
        List<IndicatorReportByAgeItem> GetBhsIndicatorReportUniquePatientsByScoreLevelAndAgeForDepressionLike(
            SimpleFilterModel filter, string mainScreeningSectionID, string shortScreeningSectionID);


        List<IndicatorReportByAgeItem> GetBhsIndicatorReportByAgeForTCC(SimpleFilterModel filter);
        List<IndicatorReportByAgeItem> GetBhsIndicatorReportUniquePatientsByAgeForTCC(SimpleFilterModel filter);

        List<IndicatorReportByAgeItem> GetBhsIndicatorReportByScoreLevelAndAgeForThinkingOfDeath(SimpleFilterModel filter);
        List<IndicatorReportByAgeItem> GetBhsIndicatorReportUniquePatientsByScoreLevelAndAgeForThinkingOfDeath(SimpleFilterModel filter);


        List<IndicatorReportByAgeItem> GetBhsDrugsofChoiceByAge(SimpleFilterModel filter);
        List<IndicatorReportByAgeItem> GetBhsDrugsofChoiceUniquePatientsByAge(SimpleFilterModel filter);

        List<IndicatorReportItem> GetUniqueBhsIndicatorReport_v2(SimpleFilterModel filter);
        List<IndicatorReportItem> GetBhsIndicatorReport_v2(SimpleFilterModel filter);
        List<IndicatorReportItem> GetUniqueBhsPositiveNagativeIndicatorReportForTCC(SimpleFilterModel filter);
        List<IndicatorReportItem> GetBhsPositiveNagativeIndicatorReportForTCC(SimpleFilterModel filter);
        List<IndicatorReportItem> GetBhsIndicatorReportByScoreLevelForThinkingOfDeath(SimpleFilterModel filter);

        List<IndicatorReportItem> GetUniqueBhsIndicatorReportByScoreLevelForThinkingOfDeath(SimpleFilterModel filter);
        List<IndicatorReportItem> GetUniqueBhsIndicatorReportByScoreLevel(SimpleFilterModel filter);
        List<IndicatorReportItem> GetBhsIndicatorReportByScoreLevel(SimpleFilterModel filter);
        List<IndicatorReportItem> GetUniqueBhsIndicatorReportByScoreLevelForDepressionLike(
            SimpleFilterModel filter,
            string mainScreeningSectionID,
            string shortScreeningSectionID
        );
        List<IndicatorReportItem> GetBhsIndicatorReportByScoreLevelForDepressionLike(
            SimpleFilterModel filter,
            string mainScreeningSectionID,
            string shortScreeningSectionID
        );
    }

    public class IndicatorReportDb : DBDatabase, IIndicatorReportRepository
    {
        private readonly IndicatorReportItemFactory _indicatorReportItemFactory = new IndicatorReportItemFactory();
        private readonly IndicatorReportByAgeItemFactory _indicatorReportByAgeItemFactory = new IndicatorReportByAgeItemFactory();
        private ILog _logger = LogManager.GetLogger<IndicatorReportDb>();

        #region Constructors
        public IndicatorReportDb() : base(0) { }

        internal IndicatorReportDb(DbConnection sharedConnection) : base(sharedConnection) { }

        #endregion

        public List<IndicatorReportByAgeItem> GetIndicatorReportPositiveByAges(SimpleFilterModel filter)
        {

            var reportItems = new List<IndicatorReportByAgeItem>();

            var withSqlBuilder = new QueryBuilder(@"
SELECT 
    qr.ScreeningSectionID
    ,qr.QuestionID
    ,qr.AnswerValue
    ,[dbo].[fn_GetAge](r.Birthday) as Age
    ,1 as PositiveScore
FROM dbo.ScreeningSectionQuestionResult qr
    INNER JOIN dbo.ScreeningSectionQuestion q ON q.ScreeningSectionID = qr.ScreeningSectionID AND q.QuestionID = qr.QuestionID
    INNER JOIN dbo.ScreeningResult r ON qr.ScreeningResultID = r.ScreeningResultID
");

            CommandObject.Parameters.Clear();
            withSqlBuilder.AppendWhereCondition("q.IsMainQuestion = 1", ClauseType.And);
            withSqlBuilder.AppendWhereCondition("r.IsDeleted = 0", ClauseType.And);
            withSqlBuilder.AppendWhereCondition("qr.AnswerValue > 0", ClauseType.And);

            if (filter.Location.HasValue)
            {
                withSqlBuilder.AppendJoinStatement(
@"INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID");
                withSqlBuilder.AppendWhereCondition("k.BranchLocationID = @LocationID", ClauseType.And);
                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;

            }
            if (filter.StartDate.HasValue)
            {
                withSqlBuilder.AppendWhereCondition("r.CreatedDate >= @StartDate", ClauseType.And);

                AddParameter("@StartDate", DbType.DateTime).Value = filter.StartDate.Value;

            }
            if (filter.EndDate.HasValue)
            {
                withSqlBuilder.AppendWhereCondition("r.CreatedDate < @EndDate", ClauseType.And);

                AddParameter("@EndDate", DbType.DateTime).Value = filter.EndDate.Value;

            }
            string sql = string.Format(@"
WITH tblResults(ScreeningSectionID, QuestionId, AnswerValue, Age, PositiveScore) AS
({0})
SELECT q.ScreeningSectionID
      ,q.QuestionId
      ,q.QuestionText
       ,ISNULL(t2.PositiveScoreCount, 0) as PositiveScoreCount
       ,ISNULL(t2.Age, 0) as Age
      ,q.PreambleText
FROM dbo.ScreeningSectionQuestion q
    INNER JOIN dbo.ScreeningSection s ON s.ScreeningSectionID = q.ScreeningSectionID
LEFT JOIN (
    SELECT 
        r.ScreeningSectionID
        ,r.QuestionId
        ,r.PositiveScore
        ,COUNT_BIG(r.PositiveScore) as PositiveScoreCount
        ,r.Age
    FROM tblResults r
    GROUP BY r.ScreeningSectionID, r.QuestionId, r.PositiveScore, r.Age
) t2 ON q.ScreeningSectionID = t2.ScreeningSectionID AND q.QuestionID = t2.QuestionId 

WHERE s.ScreeningID = 'BHS' AND q.IsMainQuestion = 1
ORDER BY s.OrderIndex ASC, q.OrderIndex ASC, t2.Age ASC
", withSqlBuilder);

            try
            {

                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        var preamleText = Convert.ToString(reader[5]);
                        var questionText = Convert.ToString(reader[2]);

                        var sectionId = reader.GetString(0).TrimEnd();
                        var questionId = Convert.ToInt32(reader[1]);

                        //start new section
                        var item = new IndicatorReportByAgeItem()
                        {
                            ScreeningSectionID = sectionId,
                            QuestionID = questionId,
                            ScreeningSectionQuestion = string.IsNullOrEmpty(preamleText) ? questionText : preamleText + "|" + questionText,
                            PositiveCount = Convert.ToInt32(reader[3]),
                            Age = Convert.ToInt32(reader[4])
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

        public List<IndicatorReportByAgeItem> GetIndicatorReportUniquePatientsPositiveByAges(SimpleFilterModel filter)
        {

            var reportItems = new List<IndicatorReportByAgeItem>();

            var withSqlBuilder = new QueryBuilder(@"
SELECT 
    qr.ScreeningSectionID
    ,qr.QuestionID
    ,MAX(qr.AnswerValue) as AnswerValue 
    ,[dbo].[fn_GetAge](r.Birthday) as Age
    ,1 as PositiveScore
FROM dbo.ScreeningSectionQuestionResult qr
    INNER JOIN dbo.ScreeningSectionQuestion q ON q.ScreeningSectionID = qr.ScreeningSectionID AND q.QuestionID = qr.QuestionID
    INNER JOIN dbo.ScreeningResult r ON qr.ScreeningResultID = r.ScreeningResultID
");

            CommandObject.Parameters.Clear();
            withSqlBuilder.AppendWhereCondition("q.IsMainQuestion = 1", ClauseType.And);
            withSqlBuilder.AppendWhereCondition("r.IsDeleted = 0", ClauseType.And);
            withSqlBuilder.AppendWhereCondition("qr.AnswerValue > 0", ClauseType.And);

            withSqlBuilder.AppendGroupCondition(" r.FirstName,r.LastName,r.MiddleName,r.Birthday,qr.ScreeningSectionID,qr.QuestionID");


            if (filter.Location.HasValue)
            {
                withSqlBuilder.AppendJoinStatement(
@"INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID");
                withSqlBuilder.AppendWhereCondition("k.BranchLocationID = @LocationID", ClauseType.And);
                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;

            }
            if (filter.StartDate.HasValue)
            {
                withSqlBuilder.AppendWhereCondition("r.CreatedDate >= @StartDate", ClauseType.And);

                AddParameter("@StartDate", DbType.DateTime).Value = filter.StartDate.Value;

            }
            if (filter.EndDate.HasValue)
            {
                withSqlBuilder.AppendWhereCondition("r.CreatedDate < @EndDate", ClauseType.And);

                AddParameter("@EndDate", DbType.DateTime).Value = filter.EndDate.Value;

            }
            string sql = string.Format(@"
WITH tblResults(ScreeningSectionID, QuestionId, AnswerValue, Age, PositiveScore) AS
({0})
SELECT q.ScreeningSectionID
      ,q.QuestionId
      ,q.QuestionText
       ,ISNULL(t2.PositiveScoreCount, 0) as PositiveScoreCount
       ,ISNULL(t2.Age, 0) as Age
      ,q.PreambleText
FROM dbo.ScreeningSectionQuestion q
    INNER JOIN dbo.ScreeningSection s ON s.ScreeningSectionID = q.ScreeningSectionID
LEFT JOIN (
    SELECT 
        r.ScreeningSectionID
        ,r.QuestionId
        ,r.PositiveScore
        ,COUNT_BIG(r.PositiveScore) as PositiveScoreCount
        ,r.Age
    FROM tblResults r
    GROUP BY r.ScreeningSectionID, r.QuestionId, r.PositiveScore, r.Age
) t2 ON q.ScreeningSectionID = t2.ScreeningSectionID AND q.QuestionID = t2.QuestionId 

WHERE s.ScreeningID = 'BHS' AND q.IsMainQuestion = 1  AND s.ScreeningSectionID not in ('PHQ-9','GAD-7')
ORDER BY s.OrderIndex ASC, q.OrderIndex ASC, t2.Age ASC
", withSqlBuilder);

            try
            {

                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        var preamleText = Convert.ToString(reader[5]);
                        var questionText = Convert.ToString(reader[2]);

                        var sectionId = reader.GetString(0).TrimEnd();
                        var questionId = Convert.ToInt32(reader[1]);

                        //start new section
                        var item = new IndicatorReportByAgeItem()
                        {
                            ScreeningSectionID = sectionId,
                            QuestionID = questionId,
                            ScreeningSectionQuestion = string.IsNullOrEmpty(preamleText) ? questionText : preamleText + "|" + questionText,
                            PositiveCount = Convert.ToInt32(reader[3]),
                            Age = Convert.ToInt32(reader[4])
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


        /// <summary>
        /// Get data for indicator report by score level
        /// </summary>
        /// <returns>Indicator are grouped by people that report “positive” and “negative” for each severity level</returns>
        /// <remarks>
        /// See complete script w/o filter by Branch location in the Database project: FrontDeskDatabase/Queries/IndicatorReport/IndicatorReportPosNegativeBySeverityLevels.sql
        /// </remarks>
        public List<IndicatorReportByAgeItem> GetBhsIndicatorReportByScoreLevelAndAge(SimpleFilterModel filter)
        {
            var reportItems = new List<IndicatorReportByAgeItem>();

            //inner SELECT query for actual screening results
            QueryBuilder withInnerSqlBuilder = new QueryBuilder(@"
SELECT
    sr.ScreeningSectionID,
    sr.ScoreLevel,
    [dbo].[fn_GetAge](r.Birthday) as Age
FROM dbo.ScreeningSectionResult sr 
    INNER JOIN dbo.ScreeningResult r ON sr.ScreeningResultID = r.ScreeningResultID 
");

            CommandObject.Parameters.Clear();
            withInnerSqlBuilder.AppendWhereCondition("r.IsDeleted = 0", ClauseType.And);
            //withSqlBuilder.AppendWhereCondition("sl.ScoreLevel > 0", ClauseType.And);
            withInnerSqlBuilder.AppendWhereCondition("sr.ScreeningSectionID NOT IN('SIH','TCC', 'PHQ-9','GAD-7')", ClauseType.And);

            if (filter.Location.HasValue)
            {
                withInnerSqlBuilder.AppendJoinStatement(@"INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID");
                withInnerSqlBuilder.AppendWhereCondition("k.BranchLocationID = @LocationID", ClauseType.And);
                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;
            }
            if (filter.StartDate.HasValue)
            {
                withInnerSqlBuilder.AppendWhereCondition("r.CreatedDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTime).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                withInnerSqlBuilder.AppendWhereCondition("r.CreatedDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTime).Value = filter.EndDate.Value;
            }

            //CTE query with all score levels left joined with actual screenings filtered by filer criteria
            var withSql = string.Format(@"
SELECT 
        sl.ScreeningSectionID,
        sl.ScoreLevel,
        sl.Name,
        sl.Indicates,
        Convert(int, (CASE WHEN sr.ScoreLevel = sl.ScoreLevel THEN 1 ELSE 0 END)) as IsPositive,
        ISNULL(sr.Age, 0) as Age
    FROM dbo.ScreeningScoreLevel sl 
        LEFT JOIN (
{0}
        ) sr ON sl.ScreeningSectionID = sr.ScreeningSectionID 
    WHERE sl.ScreeningSectionID NOT IN('SIH','TCC', 'PHQ-9','GAD-7') 
", withInnerSqlBuilder);

            //Final SQL
            string sql = string.Format(@"
WITH tblResults(ScreeningSectionID,	ScoreLevel, Name, Indicates, IsPositive, Age) AS
(
{0}
)
Select 
    r.ScreeningSectionID,
    r.Name,
    r.Indicates,
    r.Age,
    SUM(Convert(bigint,ISNULL(IsPositive, 0))) as TotalPositive
FROM tblResults r
GROUP BY r.Name, r.ScreeningSectionID, r.ScoreLevel, r.Indicates, r.Age
ORDER BY ScreeningSectionID, ScoreLevel, Age
", withSql);

            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        var item = new IndicatorReportByAgeItem
                        {
                            ScreeningSectionID = reader.GetString(0).TrimEnd(),
                            ScreeningSectionQuestion = reader.GetString(1),
                            ScreeningSectionIndicates = reader.IsDBNull(2) ? String.Empty : reader.GetString(2),

                            Age = reader.Get<int>(3) ?? 0,
                            PositiveCount = reader.GetInt64(4),

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


        public List<IndicatorReportByAgeItem> GetBhsIndicatorReportUniquePatientsByScoreLevelAndAge(SimpleFilterModel filter)
        {
            DebugLogger.WriteTraceMessage("Calling GetBhsIndicatorReportUniquePatientsByScoreLevelAndAge");

            var reportItems = new List<IndicatorReportByAgeItem>();

            //inner SELECT query for actual screening results
            QueryBuilder withInnerSqlBuilder = new QueryBuilder(@"
SELECT
    sr.ScreeningSectionID,
    MAX(sr.ScoreLevel) as ScoreLevel,
    [dbo].[fn_GetAge](r.Birthday) as Age
FROM dbo.ScreeningSectionResult sr 
    INNER JOIN dbo.ScreeningResult r ON sr.ScreeningResultID = r.ScreeningResultID 
");

            withInnerSqlBuilder.AppendGroupCondition("r.FirstName,r.LastName,r.MiddleName,r.Birthday,sr.ScreeningSectionID");

            CommandObject.Parameters.Clear();
            withInnerSqlBuilder.AppendWhereCondition("r.IsDeleted = 0", ClauseType.And);
            withInnerSqlBuilder.AppendWhereCondition("sr.ScreeningSectionID NOT IN('SIH','TCC','PHQ-9','GAD-7')", ClauseType.And);

            if (filter.Location.HasValue)
            {
                withInnerSqlBuilder.AppendJoinStatement(@"INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID");
                withInnerSqlBuilder.AppendWhereCondition("k.BranchLocationID = @LocationID", ClauseType.And);
                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;
            }
            if (filter.StartDate.HasValue)
            {
                withInnerSqlBuilder.AppendWhereCondition("r.CreatedDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTime).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                withInnerSqlBuilder.AppendWhereCondition("r.CreatedDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTime).Value = filter.EndDate.Value;
            }

            //CTE query with all score levels left joined with actual screenings filtered by filer criteria
            var withSql = string.Format(@"
SELECT 
        sl.ScreeningSectionID,
        sl.ScoreLevel,
        sl.Name,
        sl.Indicates,
        Convert(int, (CASE WHEN sr.ScoreLevel = sl.ScoreLevel THEN 1 ELSE 0 END)) as IsPositive,
        ISNULL(sr.Age, 0) as Age
    FROM dbo.ScreeningScoreLevel sl 
        LEFT JOIN (
{0}
        ) sr ON sl.ScreeningSectionID = sr.ScreeningSectionID 
    WHERE sl.ScreeningSectionID NOT IN('SIH','TCC','PHQ-9','GAD-7') 
", withInnerSqlBuilder);

            //Final SQL
            string sql = string.Format(@"
WITH tblResults(ScreeningSectionID,	ScoreLevel, Name, Indicates, IsPositive, Age) AS
(
{0}
)
Select 
    r.ScreeningSectionID,
    r.Name,
    r.Indicates,
    r.Age,
    SUM(Convert(bigint,ISNULL(IsPositive, 0))) as TotalPositive
FROM tblResults r
GROUP BY r.Name, r.ScreeningSectionID, r.ScoreLevel, r.Indicates, r.Age
ORDER BY ScreeningSectionID, ScoreLevel, Age
", withSql);

            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        var item = new IndicatorReportByAgeItem
                        {
                            ScreeningSectionID = reader.GetString(0).TrimEnd(),
                            ScreeningSectionQuestion = reader.GetString(1),
                            ScreeningSectionIndicates = reader.IsDBNull(2) ? String.Empty : reader.GetString(2),

                            Age = reader.Get<int>(3) ?? 0,
                            PositiveCount = reader.GetInt64(4),

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

        /// <summary>
        /// Get data source for Report by Age for Tool with 2 or all required sections (like Depression and Anxiety)
        /// </summary>
        /// <param name="mainScreeningSectionID">i.e. ScreeningSectionDescriptor.Depression</param>
        /// <param name="shortScreeningSectionID">i.e. ScreeningSectionDescriptor.DepressionPhq2ID</param>
        /// <returns></returns>
        public List<IndicatorReportByAgeItem> GetBhsIndicatorReportByScoreLevelAndAgeForDepressionLike(
            SimpleFilterModel filter,
            string mainScreeningSectionID,
            string shortScreeningSectionID
        )
        {
            var reportItems = new List<IndicatorReportByAgeItem>();

            //inner SELECT query for actual screening results
            QueryBuilder withInnerSqlBuilder = new QueryBuilder(@"
SELECT
    sr.ScreeningSectionID,
    q.QuestionsAsked,
    sr.ScoreLevel,
    [dbo].[fn_GetAge](r.Birthday) as Age
FROM dbo.ScreeningSectionResult sr 
    INNER JOIN dbo.ScreeningResult r ON sr.ScreeningResultID = r.ScreeningResultID
    CROSS APPLY (SELECT CASE WHEN COUNT(qr.QuestionID) = 2 THEN 2 ELSE 10 END as QuestionsAsked FROM dbo.ScreeningSectionQuestionResult qr 
        WHERE qr.ScreeningResultID = r.ScreeningResultID AND
            qr.ScreeningSectionID = sr.ScreeningSectionID) as q
");

            ClearParameters();

            AddParameter("@ScreeningSectionID", DbType.AnsiString, 5).Value = mainScreeningSectionID;

            withInnerSqlBuilder.AppendWhereCondition("r.IsDeleted = 0", ClauseType.And);
            withInnerSqlBuilder.AppendWhereCondition("sr.ScreeningSectionID IN(@ScreeningSectionID)", ClauseType.And);

            if (filter.Location.HasValue)
            {
                withInnerSqlBuilder.AppendJoinStatement(@"INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID");
                withInnerSqlBuilder.AppendWhereCondition("k.BranchLocationID = @LocationID", ClauseType.And);
                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;
            }
            if (filter.StartDate.HasValue)
            {
                withInnerSqlBuilder.AppendWhereCondition("r.CreatedDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTime).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                withInnerSqlBuilder.AppendWhereCondition("r.CreatedDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTime).Value = filter.EndDate.Value;
            }

            //CTE query with all score levels left joined with actual screenings filtered by filer criteria
            var withSql = string.Format(@"
SELECT 
        sl.ScreeningSectionID,
        sr.QuestionsAsked,
        sl.ScoreLevel,
        sl.Name,
        sl.Indicates,
        Convert(int, (CASE WHEN sr.ScoreLevel = sl.ScoreLevel THEN 1 ELSE 0 END)) as IsPositive,
        ISNULL(sr.Age, 0) as Age
    FROM dbo.ScreeningScoreLevel sl 
        LEFT JOIN (
{0}
        ) sr ON sl.ScreeningSectionID = sr.ScreeningSectionID 
    WHERE sl.ScreeningSectionID IN(@ScreeningSectionID) 
", withInnerSqlBuilder);

            //Final SQL
            string sql = string.Format(@"
WITH tblResults(ScreeningSectionID,QuestionsAsked,ScoreLevel, Name, Indicates, IsPositive, Age) AS
(
{0}
)
Select 
    r.ScreeningSectionID,
    r.QuestionsAsked,
    r.Name,
    r.Indicates,
    r.Age,
    SUM(Convert(bigint,ISNULL(IsPositive, 0))) as TotalPositive,
    r.ScoreLevel
FROM tblResults r
GROUP BY r.Name, r.ScreeningSectionID, r.QuestionsAsked, r.ScoreLevel, r.Indicates, r.Age
ORDER BY ScreeningSectionID, QuestionsAsked, ScoreLevel, Age
", withSql);

            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        var questionsAsked = reader.Get<int>("QuestionsAsked", 0);

                        if (questionsAsked == 0) //no records, add to both both
                        {
                            reportItems.Add(_indicatorReportByAgeItemFactory.CreateForDepression(reader, shortScreeningSectionID));
                            reportItems.Add(_indicatorReportByAgeItemFactory.CreateForDepression(reader, mainScreeningSectionID));
                        }
                        else
                        {
                            reportItems.Add(_indicatorReportByAgeItemFactory.CreateForDepression(reader,
                                questionsAsked == 2 ? shortScreeningSectionID : mainScreeningSectionID));
                        }
                    }
                }
            }
            finally
            {
                Disconnect();
            }

            return reportItems;
        }

        /// <summary>
        /// Get data source for Report by Age for Tool with 2 or all required sections (like Depression and Anxiety)
        /// </summary>
        /// <param name="mainScreeningSectionID">i.e. ScreeningSectionDescriptor.Depression</param>
        /// <param name="shortScreeningSectionID">i.e. ScreeningSectionDescriptor.DepressionPhq2ID</param>
        /// <returns></returns>
        public List<IndicatorReportByAgeItem> GetBhsIndicatorReportUniquePatientsByScoreLevelAndAgeForDepressionLike(
            SimpleFilterModel filter,
            string mainScreeningSectionID,
            string shortScreeningSectionID
        )
        {
            var reportItems = new List<IndicatorReportByAgeItem>();

            //inner SELECT query for actual screening results
            QueryBuilder withInnerSqlBuilder = new QueryBuilder(@"
SELECT
    sr.ScreeningSectionID,
    q.QuestionsAsked,
    MAX(sr.ScoreLevel) as ScoreLevel,
    [dbo].[fn_GetAge](r.Birthday) as Age
FROM dbo.ScreeningSectionResult sr 
    INNER JOIN dbo.ScreeningResult r ON sr.ScreeningResultID = r.ScreeningResultID 
    CROSS APPLY (SELECT CASE WHEN COUNT(qr.QuestionID) = 2 THEN 2 ELSE 10 END as QuestionsAsked FROM dbo.ScreeningSectionQuestionResult qr 
        WHERE qr.ScreeningResultID = r.ScreeningResultID AND qr.ScreeningSectionID = sr.ScreeningSectionID) as q
");

            withInnerSqlBuilder.AppendGroupCondition("r.FirstName,r.LastName,r.MiddleName,r.Birthday,sr.ScreeningSectionID, q.QuestionsAsked");

            ClearParameters();

            AddParameter("@ScreeningSectionID", DbType.AnsiString, 5).Value = mainScreeningSectionID;


            withInnerSqlBuilder.AppendWhereCondition("r.IsDeleted = 0", ClauseType.And);
            withInnerSqlBuilder.AppendWhereCondition("sr.ScreeningSectionID IN(@ScreeningSectionID)", ClauseType.And);

            if (filter.Location.HasValue)
            {
                withInnerSqlBuilder.AppendJoinStatement(@"INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID");
                withInnerSqlBuilder.AppendWhereCondition("k.BranchLocationID = @LocationID", ClauseType.And);
                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;
            }
            if (filter.StartDate.HasValue)
            {
                withInnerSqlBuilder.AppendWhereCondition("r.CreatedDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTime).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                withInnerSqlBuilder.AppendWhereCondition("r.CreatedDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTime).Value = filter.EndDate.Value;
            }

            //CTE query with all score levels left joined with actual screenings filtered by filer criteria
            var withSql = string.Format(@"
SELECT 
        sl.ScreeningSectionID,
        sr.QuestionsAsked,
        sl.ScoreLevel,
        sl.Name,
        sl.Indicates,
        Convert(int, (CASE WHEN sr.ScoreLevel = sl.ScoreLevel THEN 1 ELSE 0 END)) as IsPositive,
        ISNULL(sr.Age, 0) as Age
    FROM dbo.ScreeningScoreLevel sl 
        LEFT JOIN (
{0}
        ) sr ON sl.ScreeningSectionID = sr.ScreeningSectionID 
    WHERE sl.ScreeningSectionID IN(@ScreeningSectionID) 
", withInnerSqlBuilder);

            //Final SQL
            string sql = string.Format(@"
WITH tblResults(ScreeningSectionID,QuestionsAsked,ScoreLevel, Name, Indicates, IsPositive, Age) AS
(
{0}
)
Select 
    r.ScreeningSectionID,
    r.QuestionsAsked,
    r.Name,
    r.Indicates,
    r.Age,
    SUM(Convert(bigint,ISNULL(IsPositive, 0))) as TotalPositive,
    r.ScoreLevel
FROM tblResults r
GROUP BY r.Name, r.ScreeningSectionID, r.QuestionsAsked,r.ScoreLevel, r.Indicates, r.Age
ORDER BY ScreeningSectionID, QuestionsAsked, ScoreLevel, Age
", withSql);

            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        var questionsAsked = reader.Get<int>("QuestionsAsked", 0);

                        if (questionsAsked == 0) //no records, add to both both
                        {
                            reportItems.Add(_indicatorReportByAgeItemFactory.CreateForDepression(reader, shortScreeningSectionID));
                            reportItems.Add(_indicatorReportByAgeItemFactory.CreateForDepression(reader, mainScreeningSectionID));
                        }
                        else
                        {
                            reportItems.Add(_indicatorReportByAgeItemFactory.CreateForDepression(reader, questionsAsked == 2 ? shortScreeningSectionID : mainScreeningSectionID));
                        }
                    }
                }
            }
            finally
            {
                Disconnect();
            }

            return reportItems;
        }



        public List<IndicatorReportByAgeItem> GetBhsIndicatorReportByAgeForTCC(SimpleFilterModel filter)
        {

            var reportItems = new List<IndicatorReportByAgeItem>();

            //inner SELECT query for actual screening results
            QueryBuilder withInnerSqlBuilder = new QueryBuilder(@"
SELECT
    sr.ScreeningSectionID,
    qr.QuestionID,
    qr.AnswerValue,
    [dbo].[fn_GetAge](r.Birthday) as Age
FROM dbo.ScreeningSectionResult sr 
    INNER JOIN dbo.ScreeningResult r ON sr.ScreeningResultID = r.ScreeningResultID 
    LEFT JOIN dbo.ScreeningSectionQuestionResult qr ON qr.ScreeningResultID = sr.ScreeningResultID AND qr.ScreeningSectionID = sr.ScreeningSectionID
");

            CommandObject.Parameters.Clear();
            withInnerSqlBuilder.AppendWhereCondition("sr.ScreeningSectionID = 'TCC'", ClauseType.And);
            withInnerSqlBuilder.AppendWhereCondition("r.IsDeleted = 0", ClauseType.And);

            if (filter.Location.HasValue)
            {
                withInnerSqlBuilder.AppendJoinStatement(@"INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID");
                withInnerSqlBuilder.AppendWhereCondition("k.BranchLocationID = @LocationID", ClauseType.And);
                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;
            }
            if (filter.StartDate.HasValue)
            {
                withInnerSqlBuilder.AppendWhereCondition("r.CreatedDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTime).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                withInnerSqlBuilder.AppendWhereCondition("r.CreatedDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTime).Value = filter.EndDate.Value;
            }

            //CTE query with all score levels left joined with actual screenings filtered by filer criteria
            var withSql = string.Format(@"
SELECT 
    sl.ScreeningSectionID,
    sl.QuestionID,
    sl.QuestionText,
    ISNULL(sr.AnswerValue, 0) as IsPositive,
    Age
FROM dbo.ScreeningSectionQuestion sl 
    LEFT JOIN (
{0}
    ) sr ON sl.ScreeningSectionID = sr.ScreeningSectionID AND (sr.QuestionID IS NULL OR sl.QuestionID = sr.QuestionID)
WHERE sl.ScreeningSectionID = 'TCC' AND sl.IsMainQuestion = 0
", withInnerSqlBuilder);

            //Final SQL
            string sql = string.Format(@"
WITH tblResults(ScreeningSectionID,	QuestionID, QuestionText, IsPositive, Age) AS
(
{0}
)
SELECT 
    r.ScreeningSectionID,
    r.QuestionText,
    SUM(Convert(bigint,ISNULL(IsPositive, 0))) as TotalPositive,
    r.QuestionID,
    r.Age
FROM tblResults r
GROUP BY r.QuestionText, r.ScreeningSectionID, r.QuestionID, r.Age
ORDER BY ScreeningSectionID, QuestionID
", withSql);

            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        var item = new IndicatorReportByAgeItem
                        {
                            ScreeningSectionID = reader.GetString(0).TrimEnd(),
                            ScreeningSectionQuestion = reader.GetString(1),
                            PositiveCount = reader.GetInt64(2),
                            Age = reader.Get<int>(4) ?? 0,
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

        public List<IndicatorReportByAgeItem> GetBhsIndicatorReportUniquePatientsByAgeForTCC(SimpleFilterModel filter)
        {

            var reportItems = new List<IndicatorReportByAgeItem>();

            //inner SELECT query for actual screening results
            QueryBuilder withInnerSqlBuilder = new QueryBuilder(@"
SELECT
    sr.ScreeningSectionID,
    qr.QuestionID,
    MAX(qr.AnswerValue) as AnswerValue,
    [dbo].[fn_GetAge](r.Birthday) as Age
FROM dbo.ScreeningSectionResult sr 
    INNER JOIN dbo.ScreeningResult r ON sr.ScreeningResultID = r.ScreeningResultID 
    LEFT JOIN dbo.ScreeningSectionQuestionResult qr ON qr.ScreeningResultID = sr.ScreeningResultID AND qr.ScreeningSectionID = sr.ScreeningSectionID
");

            withInnerSqlBuilder.AppendGroupCondition("r.FirstName,r.LastName,r.MiddleName,r.Birthday,sr.ScreeningSectionID,qr.QuestionID");
            CommandObject.Parameters.Clear();
            withInnerSqlBuilder.AppendWhereCondition("sr.ScreeningSectionID = 'TCC'", ClauseType.And);
            withInnerSqlBuilder.AppendWhereCondition("r.IsDeleted = 0", ClauseType.And);

            if (filter.Location.HasValue)
            {
                withInnerSqlBuilder.AppendJoinStatement(@"INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID");
                withInnerSqlBuilder.AppendWhereCondition("k.BranchLocationID = @LocationID", ClauseType.And);
                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;
            }
            if (filter.StartDate.HasValue)
            {
                withInnerSqlBuilder.AppendWhereCondition("r.CreatedDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTime).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                withInnerSqlBuilder.AppendWhereCondition("r.CreatedDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTime).Value = filter.EndDate.Value;
            }

            //CTE query with all score levels left joined with actual screenings filtered by filer criteria
            var withSql = string.Format(@"
SELECT 
    sl.ScreeningSectionID,
    sl.QuestionID,
    sl.QuestionText,
    ISNULL(sr.AnswerValue, 0) as IsPositive,
    Age
FROM dbo.ScreeningSectionQuestion sl 
    LEFT JOIN (
{0}
    ) sr ON sl.ScreeningSectionID = sr.ScreeningSectionID AND (sr.QuestionID IS NULL OR sl.QuestionID = sr.QuestionID)
WHERE sl.ScreeningSectionID = 'TCC' AND sl.IsMainQuestion = 0
", withInnerSqlBuilder);

            //Final SQL
            string sql = string.Format(@"
WITH tblResults(ScreeningSectionID,	QuestionID, QuestionText, IsPositive, Age) AS
(
{0}
)
SELECT 
    r.ScreeningSectionID,
    r.QuestionText,
    SUM(Convert(bigint,ISNULL(IsPositive, 0))) as TotalPositive,
    r.QuestionID,
    r.Age
FROM tblResults r
GROUP BY r.QuestionText, r.ScreeningSectionID, r.QuestionID, r.Age
ORDER BY ScreeningSectionID, QuestionID
", withSql);

            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        var item = new IndicatorReportByAgeItem
                        {
                            ScreeningSectionID = reader.GetString(0).TrimEnd(),
                            ScreeningSectionQuestion = reader.GetString(1),
                            PositiveCount = reader.GetInt64(2),
                            Age = reader.Get<int>(4) ?? 0,
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


        public List<IndicatorReportByAgeItem> GetBhsIndicatorReportByScoreLevelAndAgeForThinkingOfDeath(SimpleFilterModel filter)
        {
            var reportItems = new List<IndicatorReportByAgeItem>();

            //inner SELECT query for actual screening results
            QueryBuilder withInnerSqlBuilder = new QueryBuilder(@"
        SELECT
            qr.AnswerValue
            ,q.AnswerScaleID
            ,[dbo].[fn_GetAge](r.Birthday) as Age
        FROM dbo.ScreeningSectionQuestionResult qr
            INNER JOIN dbo.ScreeningSectionQuestion q ON q.ScreeningSectionID = qr.ScreeningSectionID AND q.QuestionID = qr.QuestionID 
            INNER JOIN dbo.ScreeningResult r ON r.ScreeningResultID = qr.ScreeningResultID 
");

            CommandObject.Parameters.Clear();
            withInnerSqlBuilder.AppendWhereCondition("qr.ScreeningSectionID = 'PHQ-9'", ClauseType.And);
            withInnerSqlBuilder.AppendWhereCondition("qr.QuestionID = 9", ClauseType.And);
            withInnerSqlBuilder.AppendWhereCondition("r.IsDeleted = 0", ClauseType.And);

            if (filter.Location.HasValue)
            {
                withInnerSqlBuilder.AppendJoinStatement(@"INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID");
                withInnerSqlBuilder.AppendWhereCondition("k.BranchLocationID = @LocationID", ClauseType.And);
                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;
            }
            if (filter.StartDate.HasValue)
            {
                withInnerSqlBuilder.AppendWhereCondition("r.CreatedDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTime).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                withInnerSqlBuilder.AppendWhereCondition("r.CreatedDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTime).Value = filter.EndDate.Value;
            }

            //CTE query with all score levels left joined with actual screenings filtered by filer criteria
            var withSql = string.Format(@"
SELECT 
    ao.AnswerScaleID,
    ao.AnswerScaleOptionID,
    ao.OptionValue,
    ao.OptionText,
    Convert(int, (CASE WHEN sr.AnswerValue = ao.OptionValue THEN 1 ELSE 0 END)) as IsPositive,
    sr.Age
    FROM dbo.ScreeningSectionQuestion question 
        INNER JOIN dbo.AnswerScale sc ON question.AnswerScaleID = sc.AnswerScaleID 
        INNER JOIN dbo.AnswerScaleOption ao ON sc.AnswerScaleID = ao.AnswerScaleID 
    LEFT JOIN (
{0}
    )  sr ON sr.AnswerScaleID = ao.AnswerScaleID
WHERE question.ScreeningSectionID = 'PHQ-9' AND question.QuestionID = 9 
", withInnerSqlBuilder);

            //Final SQL
            string sql = string.Format(@"
WITH tblResults(AnswerScaleID, AnswerScaleOptionID, OptionValue, Name, IsPositive, Age) AS
(
{0}
)
SELECT 
    r.AnswerScaleID
    ,r.Name
    ,SUM(Convert(bigint,ISNULL(IsPositive, 0))) as TotalPositive
    ,r.Age
FROM tblResults r
GROUP BY r.Name, r.AnswerScaleID, r.AnswerScaleOptionID, r.OptionValue, r.Age
ORDER BY AnswerScaleOptionID
", withSql);

            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        var item = new IndicatorReportByAgeItem
                        {
                            ScreeningSectionID = ScreeningSectionDescriptor.DepressionThinkOfDeath,
                            ScreeningSectionQuestion = reader.GetString(1),
                            PositiveCount = reader.GetInt64(2),
                            Age = reader.Get<int>(3) ?? 0,
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


        public List<IndicatorReportByAgeItem> GetBhsIndicatorReportUniquePatientsByScoreLevelAndAgeForThinkingOfDeath(SimpleFilterModel filter)
        {
            DebugLogger.WriteTraceMessage("Calling GetBhsIndicatorReportUniquePatientsByScoreLevelAndAgeForThinkingOfDeath");

            var reportItems = new List<IndicatorReportByAgeItem>();

            //inner SELECT query for actual screening results
            QueryBuilder withInnerSqlBuilder = new QueryBuilder(@"
SELECT
    MAX(qr.AnswerValue) as AnswerValue
    ,q.AnswerScaleID
    ,[dbo].[fn_GetAge](r.Birthday) as Age
FROM dbo.ScreeningSectionQuestionResult qr
    INNER JOIN dbo.ScreeningSectionQuestion q ON q.ScreeningSectionID = qr.ScreeningSectionID AND q.QuestionID = qr.QuestionID 
    INNER JOIN dbo.ScreeningResult r ON r.ScreeningResultID = qr.ScreeningResultID 
");
            withInnerSqlBuilder.AppendGroupCondition("r.FirstName,r.LastName,r.MiddleName,r.Birthday,q.AnswerScaleID");

            CommandObject.Parameters.Clear();
            withInnerSqlBuilder.AppendWhereCondition("qr.ScreeningSectionID = 'PHQ-9'", ClauseType.And);
            withInnerSqlBuilder.AppendWhereCondition("qr.QuestionID = 9", ClauseType.And);
            withInnerSqlBuilder.AppendWhereCondition("r.IsDeleted = 0", ClauseType.And);

            if (filter.Location.HasValue)
            {
                withInnerSqlBuilder.AppendJoinStatement(@"INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID");
                withInnerSqlBuilder.AppendWhereCondition("k.BranchLocationID = @LocationID", ClauseType.And);
                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;
            }
            if (filter.StartDate.HasValue)
            {
                withInnerSqlBuilder.AppendWhereCondition("r.CreatedDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTime).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                withInnerSqlBuilder.AppendWhereCondition("r.CreatedDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTime).Value = filter.EndDate.Value;
            }

            //CTE query with all score levels left joined with actual screenings filtered by filer criteria
            var withSql = string.Format(@"
SELECT 
    ao.AnswerScaleID,
    ao.AnswerScaleOptionID,
    ao.OptionValue,
    ao.OptionText,
    Convert(int, (CASE WHEN sr.AnswerValue = ao.OptionValue THEN 1 ELSE 0 END)) as IsPositive,
    sr.Age
    FROM dbo.ScreeningSectionQuestion question 
        INNER JOIN dbo.AnswerScale sc ON question.AnswerScaleID = sc.AnswerScaleID 
        INNER JOIN dbo.AnswerScaleOption ao ON sc.AnswerScaleID = ao.AnswerScaleID 
    LEFT JOIN (
{0}
    )  sr ON sr.AnswerScaleID = ao.AnswerScaleID
WHERE question.ScreeningSectionID = 'PHQ-9' AND question.QuestionID = 9 
", withInnerSqlBuilder);

            //Final SQL
            string sql = string.Format(@"
WITH tblResults(AnswerScaleID, AnswerScaleOptionID, OptionValue, Name, IsPositive, Age) AS
(
{0}
)
SELECT 
    r.AnswerScaleID
    ,r.Name
    ,SUM(Convert(bigint,ISNULL(IsPositive, 0))) as TotalPositive
    ,r.Age
FROM tblResults r
GROUP BY r.Name, r.AnswerScaleID, r.AnswerScaleOptionID, r.OptionValue, r.Age
ORDER BY AnswerScaleOptionID
", withSql);

            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        var item = new IndicatorReportByAgeItem
                        {
                            ScreeningSectionID = ScreeningSectionDescriptor.DepressionThinkOfDeath,
                            ScreeningSectionQuestion = reader.GetString(1),
                            PositiveCount = reader.GetInt64(2),
                            Age = reader.Get<int>(3) ?? 0,
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

        /// <summary>
        /// Get the total number of screenings for certain filter criteria
        /// </summary>
        public long GetBhsIndicatorReportTotalRecords(SimpleFilterModel filter)
        {

            long count = 0;

            var sqlBuilder = new QueryBuilder(@"
SELECT 
    COUNT_BIG(*)
FROM dbo.ScreeningResult r 
");

            CommandObject.Parameters.Clear();
            sqlBuilder.AppendWhereCondition("r.IsDeleted = 0", ClauseType.And);

            if (filter.Location.HasValue)
            {
                sqlBuilder.AppendJoinStatement(
                    @"INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID");
                sqlBuilder.AppendWhereCondition("k.BranchLocationID = @LocationID", ClauseType.And);
                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;

            }
            if (filter.StartDate.HasValue)
            {
                sqlBuilder.AppendWhereCondition("r.CreatedDate >= @StartDate", ClauseType.And);

                AddParameter("@StartDate", DbType.DateTime).Value = filter.StartDate.Value;

            }
            if (filter.EndDate.HasValue)
            {
                sqlBuilder.AppendWhereCondition("r.CreatedDate < @EndDate", ClauseType.And);

                AddParameter("@EndDate", DbType.DateTime).Value = filter.EndDate.Value;
            }

            try
            {
                count = Convert.ToInt64(RunScalarQuery(sqlBuilder.ToString()));
            }
            finally
            {
                Disconnect();
            }
            return count;
        }


        public List<IndicatorReportItem> GetBhsIndicatorReport_v2(SimpleFilterModel filter)
        {

            List<IndicatorReportItem> reportItems = new List<IndicatorReportItem>();

            var withSqlBuilder = new QueryBuilder(@"
SELECT 
 sr.ScreeningSectionID
    ,q.QuestionID
    ,Convert(int, (CASE WHEN qr.AnswerValue > 0 THEN 1 ELSE 0 END)) as AnswerValue
    ,Convert(int, (CASE WHEN qr.AnswerValue > 0 THEN 1 ELSE 0 END)) as PositiveScore
FROM 
    dbo.ScreeningSectionQuestionResult qr 
    INNER JOIN dbo.ScreeningSectionQuestion q ON q.ScreeningSectionID = qr.ScreeningSectionID AND q.QuestionID = qr.QuestionID
    INNER JOIN dbo.ScreeningSectionResult sr ON sr.ScreeningResultID = qr.ScreeningResultID AND sr.ScreeningSectionID = qr.ScreeningSectionID
    INNER JOIN dbo.ScreeningResult r ON sr.ScreeningResultID = r.ScreeningResultID
");

            CommandObject.Parameters.Clear();

            withSqlBuilder.AppendWhereCondition("q.IsMainQuestion = 1", ClauseType.And);
            withSqlBuilder.AppendWhereCondition("r.IsDeleted = 0", ClauseType.And);

            if (filter.Location.HasValue)
            {
                withSqlBuilder.AppendJoinStatement(
                    @"INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID");
                withSqlBuilder.AppendWhereCondition("k.BranchLocationID = @LocationID", ClauseType.And);
                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;

            }
            if (filter.StartDate.HasValue)
            {
                withSqlBuilder.AppendWhereCondition("r.CreatedDate >= @StartDate", ClauseType.And);

                AddParameter("@StartDate", DbType.DateTime).Value = filter.StartDate.Value;

            }
            if (filter.EndDate.HasValue)
            {
                withSqlBuilder.AppendWhereCondition("r.CreatedDate < @EndDate", ClauseType.And);

                AddParameter("@EndDate", DbType.DateTime).Value = filter.EndDate.Value;

            }
            string sql = string.Format(@"
WITH tblResults(ScreeningSectionID, QuestionID, AnswerValue, PositiveScore) AS
({0})
SELECT q.ScreeningSectionID
      ,q.QuestionID
      ,q.QuestionText
      ,ISNULL(t1.AnswerValue,0) as AnswerValue
      ,ISNULL(t1.AnswerCount, 0) as AnswerCount
      ,ISNULL(t2.PositiveScore, 0) as PositiveScore
      ,ISNULL(t2.PositiveScoreCount, 0) as PositiveScoreCount
      ,q.PreambleText
FROM dbo.ScreeningSectionQuestion q
    INNER JOIN dbo.ScreeningSection s ON s.ScreeningSectionID = q.ScreeningSectionID
LEFT JOIN (
    SELECT 
        r.ScreeningSectionID
        ,r.QuestionID
        ,r.AnswerValue
        ,COUNT_BIG(r.AnswerValue) as AnswerCount
    FROM tblResults r
    GROUP BY r.ScreeningSectionID, r.QuestionID, r.AnswerValue
) t1 ON q.ScreeningSectionID = t1.ScreeningSectionID AND q.QuestionID = t1.QuestionID
LEFT JOIN (
    SELECT 
        r.ScreeningSectionID
        ,r.QuestionID
        ,r.PositiveScore
        ,COUNT_BIG(r.PositiveScore) as PositiveScoreCount
    FROM tblResults r
    GROUP BY r.ScreeningSectionID, r.QuestionID, r.PositiveScore
) t2 ON q.ScreeningSectionID = t2.ScreeningSectionID AND q.QuestionID = t2.QuestionID 

WHERE s.ScreeningID = 'BHS' AND q.IsMainQuestion = 1
ORDER BY s.OrderIndex ASC, q.OrderIndex ASC, t1.AnswerValue ASC, t2.PositiveScore ASC
", withSqlBuilder.ToString());

            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    string prevSectionID = string.Empty;
                    string sectionID = string.Empty;
                    string preamleText = string.Empty;
                    int questionId = 0;
                    int prevQuestionId = 0;

                    int answerValue = -1;
                    int positiveValue = -1;
                    IndicatorReportItem item = null;

                    while (reader.Read())
                    {
                        sectionID = reader.GetString(0).TrimEnd();
                        questionId = Convert.ToInt32(reader[1]);
                        answerValue = Convert.ToInt32(reader[3]);
                        positiveValue = Convert.ToInt32(reader[5]);
                        preamleText = Convert.ToString(reader[7]);

                        if (sectionID != prevSectionID || questionId != prevQuestionId)
                        {
                            string questionText = reader.GetString(2);
                            //start new section
                            item = new IndicatorReportItem
                            {
                                ScreeningSectionID = sectionID,
                                QuestionId = questionId,
                                ScreeningSectionQuestion = string.IsNullOrEmpty(preamleText) ? questionText : preamleText + "|" + questionText
                            };
                            reportItems.Add(item);
                        }
                        //else item is pointed to the current section
                        //KSA: Display Yes/No values vs. Positive and Negative
                        if (answerValue == 1) item.PositiveCount = reader.GetInt64(4);
                        else item.NegativeCount = reader.Get<long>(4, true);

                        prevSectionID = sectionID;
                        prevQuestionId = questionId;

                    }

                }
            }
            finally
            {
                Disconnect();
            }
            return reportItems;
        }



        public List<IndicatorReportItem> GetUniqueBhsIndicatorReport_v2(SimpleFilterModel filter)
        {
            Trace.TraceInformation("Calling GetUniqueBhsIndicatorReport_v2");

            List<IndicatorReportItem> reportItems = new List<IndicatorReportItem>();

            var withSqlBuilder = new QueryBuilder(@"
SELECT 
sr.ScreeningSectionID
,q.QuestionID
,Convert(int, (CASE WHEN MAX(qr.AnswerValue) > 0 THEN 1 ELSE 0 END)) as AnswerValue
,Convert(int, (CASE WHEN MAX(qr.AnswerValue) > 0 THEN 1 ELSE 0 END)) as PositiveScore
,r.FirstName
,r.LastName
,r.MiddleName
,r.Birthday
FROM 
    dbo.ScreeningSectionQuestionResult qr 
    INNER JOIN dbo.ScreeningSectionQuestion q ON q.ScreeningSectionID = qr.ScreeningSectionID AND q.QuestionID = qr.QuestionID
    INNER JOIN dbo.ScreeningSectionResult sr ON sr.ScreeningResultID = qr.ScreeningResultID AND sr.ScreeningSectionID = qr.ScreeningSectionID
    INNER JOIN dbo.ScreeningResult r ON sr.ScreeningResultID = r.ScreeningResultID
");

            CommandObject.Parameters.Clear();

            withSqlBuilder.AppendWhereCondition("q.IsMainQuestion = 1", ClauseType.And);
            withSqlBuilder.AppendWhereCondition("r.IsDeleted = 0", ClauseType.And);

            withSqlBuilder.AppendGroupCondition("sr.ScreeningSectionID,q.QuestionID,r.FirstName,r.LastName,r.MiddleName,r.Birthday");

            if (filter.Location.HasValue)
            {
                withSqlBuilder.AppendJoinStatement(
                    @"INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID");
                withSqlBuilder.AppendWhereCondition("k.BranchLocationID = @LocationID", ClauseType.And);
                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;

            }
            if (filter.StartDate.HasValue)
            {
                withSqlBuilder.AppendWhereCondition("r.CreatedDate >= @StartDate", ClauseType.And);

                AddParameter("@StartDate", DbType.DateTime).Value = filter.StartDate.Value;

            }
            if (filter.EndDate.HasValue)
            {
                withSqlBuilder.AppendWhereCondition("r.CreatedDate < @EndDate", ClauseType.And);

                AddParameter("@EndDate", DbType.DateTime).Value = filter.EndDate.Value;

            }
            string sql = string.Format(@"
WITH tblResults(ScreeningSectionID, QuestionID, AnswerValue, PositiveScore, FirstName, LastName, MiddleName, Birthday) AS
({0})
SELECT q.ScreeningSectionID
      ,q.QuestionID
      ,q.QuestionText
      ,ISNULL(t1.AnswerValue,0) as AnswerValue
      ,ISNULL(t1.AnswerCount, 0) as AnswerCount
      ,ISNULL(t2.PositiveScore, 0) as PositiveScore
      ,ISNULL(t2.PositiveScoreCount, 0) as PositiveScoreCount
      ,q.PreambleText
FROM dbo.ScreeningSectionQuestion q
    INNER JOIN dbo.ScreeningSection s ON s.ScreeningSectionID = q.ScreeningSectionID
LEFT JOIN (
    SELECT 
        r.ScreeningSectionID
        ,r.QuestionID
        ,r.AnswerValue
        ,COUNT_BIG(r.AnswerValue) as AnswerCount
    FROM tblResults r
    GROUP BY r.ScreeningSectionID, r.QuestionID, r.AnswerValue
) t1 ON q.ScreeningSectionID = t1.ScreeningSectionID AND q.QuestionID = t1.QuestionID
LEFT JOIN (
    SELECT 
        r.ScreeningSectionID
        ,r.QuestionID
        ,r.PositiveScore
        ,COUNT_BIG(r.PositiveScore) as PositiveScoreCount
    FROM tblResults r
    GROUP BY r.ScreeningSectionID, r.QuestionID, r.PositiveScore
) t2 ON q.ScreeningSectionID = t2.ScreeningSectionID AND q.QuestionID = t2.QuestionID 

WHERE s.ScreeningID = 'BHS' AND q.IsMainQuestion = 1
ORDER BY s.OrderIndex ASC, q.OrderIndex ASC, t1.AnswerValue ASC, t2.PositiveScore ASC
", withSqlBuilder.ToString());

            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    string prevSectionID = string.Empty;
                    string sectionID = string.Empty;
                    string preamleText = string.Empty;
                    int questionId = 0;
                    int prevQuestionId = 0;

                    int answerValue = -1;
                    int positiveValue = -1;
                    IndicatorReportItem item = null;

                    while (reader.Read())
                    {
                        sectionID = reader.GetString(0).TrimEnd();
                        questionId = Convert.ToInt32(reader[1]);
                        answerValue = Convert.ToInt32(reader[3]);
                        positiveValue = Convert.ToInt32(reader[5]);
                        preamleText = Convert.ToString(reader[7]);

                        if (sectionID != prevSectionID || questionId != prevQuestionId)
                        {
                            string questionText = reader.GetString(2);
                            //start new section
                            item = new IndicatorReportItem
                            {
                                ScreeningSectionID = sectionID,
                                QuestionId = questionId,
                                ScreeningSectionQuestion = string.IsNullOrEmpty(preamleText) ? questionText : preamleText + "|" + questionText
                            };
                            reportItems.Add(item);
                        }
                        //else item is pointed to the current section
                        //KSA: Display Yes/No values vs. Positive and Negative
                        if (answerValue == 1) item.PositiveCount = reader.GetInt64(4);
                        else item.NegativeCount = reader.GetInt64(4);

                        //if (positiveValue == 1) item.PositiveCount = reader.GetInt64(5);
                        //else item.NegativeCount = reader.GetInt64(5);

                        prevSectionID = sectionID;
                        prevQuestionId = questionId;

                    }

                }
            }
            finally
            {
                Disconnect();
            }
            return reportItems;
        }


        /// <summary>
        /// Get data for indicator report by score level
        /// </summary>
        /// <returns>Indicator are grouped by people that report “positive” and “negative” for each severity level</returns>
        /// <remarks>
        /// See complete script w/o filter by Branch location in the Database project: FrontDeskDatabase/Queries/IndicatorReport/IndicatorReportPosNegativeBySeverityLevels.sql
        /// </remarks>
        public List<IndicatorReportItem> GetBhsIndicatorReportByScoreLevel(SimpleFilterModel filter)
        {
            List<IndicatorReportItem> reportItems = new List<IndicatorReportItem>();

            //inner SELECT query for actual screening results
            QueryBuilder withInnerSqlBuilder = new QueryBuilder(@"
SELECT
    sr.ScreeningSectionID,
    sr.ScoreLevel
FROM dbo.ScreeningSectionResult sr 
    INNER JOIN dbo.ScreeningResult r ON sr.ScreeningResultID = r.ScreeningResultID 
");

            CommandObject.Parameters.Clear();
            withInnerSqlBuilder.AppendWhereCondition("r.IsDeleted = 0", ClauseType.And);
            //withSqlBuilder.AppendWhereCondition("sl.ScoreLevel > 0", ClauseType.And);
            withInnerSqlBuilder.AppendWhereCondition("sr.ScreeningSectionID NOT IN('SIH','TCC','PHQ-9','GAD-7')", ClauseType.And);

            if (filter.Location.HasValue)
            {
                withInnerSqlBuilder.AppendJoinStatement(@"INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID");
                withInnerSqlBuilder.AppendWhereCondition("k.BranchLocationID = @LocationID", ClauseType.And);
                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;
            }
            if (filter.StartDate.HasValue)
            {
                withInnerSqlBuilder.AppendWhereCondition("r.CreatedDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTime).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                withInnerSqlBuilder.AppendWhereCondition("r.CreatedDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTime).Value = filter.EndDate.Value;
            }

            //CTE query with all score levels left joined with actual screenings filtered by filer criteria
            string withSql = string.Format(@"
SELECT 
        sl.ScreeningSectionID,
        sl.ScoreLevel,
        sl.Name,
        sl.Indicates,
        Convert(int, (CASE WHEN sr.ScoreLevel = sl.ScoreLevel THEN 1 ELSE 0 END)) as IsPositive,
        Convert(int, (CASE WHEN sr.ScoreLevel IS NOT NULL THEN 1 ELSE 0 END)) as TotalCountItem
    FROM dbo.ScreeningScoreLevel sl 
        LEFT JOIN (
{0}
        ) sr ON sl.ScreeningSectionID = sr.ScreeningSectionID 
    WHERE sl.ScreeningSectionID NOT IN('SIH','TCC','PHQ-9','GAD-7') 
", withInnerSqlBuilder);

            //Final SQL
            string sql = string.Format(@"
WITH tblResults(ScreeningSectionID,	ScoreLevel, Name, Indicates, IsPositive,TotalCountItem) AS
(
{0}
)
Select 
    r.ScreeningSectionID,
    r.Name,
    r.Indicates,
    SUM(Convert(bigint,ISNULL(IsPositive, 0))) as TotalPositive,
    SUM(TotalCountItem) as Total,
    r.ScoreLevel
FROM tblResults r
GROUP BY r.Name, r.ScreeningSectionID, r.ScoreLevel, r.Indicates
ORDER BY ScreeningSectionID, ScoreLevel
", withSql);

            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        reportItems.Add(_indicatorReportItemFactory.CreateForScreeningSection(reader));
                    }
                }
            }
            finally
            {
                Disconnect();
            }

            return reportItems;
        }

        public List<IndicatorReportItem> GetUniqueBhsIndicatorReportByScoreLevel(SimpleFilterModel filter)
        {
            List<IndicatorReportItem> reportItems = new List<IndicatorReportItem>();

            //inner SELECT query for actual screening results
            QueryBuilder withInnerSqlBuilder = new QueryBuilder(@"
SELECT sr.ScreeningSectionID
,MAX(sr.ScoreLevel) as ScoreLevel
,r.FirstName
,r.LastName
,r.MiddleName
,r.Birthday
FROM dbo.ScreeningSectionResult sr 
    INNER JOIN dbo.ScreeningResult r ON sr.ScreeningResultID = r.ScreeningResultID 
");
            //group by
            withInnerSqlBuilder.AppendGroupCondition("r.FirstName, r.LastName, r.MiddleName, r.Birthday, sr.ScreeningSectionID");

            CommandObject.Parameters.Clear();
            withInnerSqlBuilder.AppendWhereCondition("r.IsDeleted = 0", ClauseType.And);
            withInnerSqlBuilder.AppendWhereCondition("sr.ScreeningSectionID NOT IN('SIH','TCC','PHQ-9','GAD-7')", ClauseType.And);

            if (filter.Location.HasValue)
            {
                withInnerSqlBuilder.AppendJoinStatement(@"INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID");
                withInnerSqlBuilder.AppendWhereCondition("k.BranchLocationID = @LocationID", ClauseType.And);
                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;
            }
            if (filter.StartDate.HasValue)
            {
                withInnerSqlBuilder.AppendWhereCondition("r.CreatedDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTime).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                withInnerSqlBuilder.AppendWhereCondition("r.CreatedDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTime).Value = filter.EndDate.Value;
            }

            //CTE query with all score levels left joined with actual screenings filtered by filer criteria
            string withSql = string.Format(@"
SELECT 
        sl.ScreeningSectionID,
        sl.ScoreLevel,
        sl.Name,
        sl.Indicates,
        Convert(int, (CASE WHEN sr.ScoreLevel = sl.ScoreLevel THEN 1 ELSE 0 END)) as IsPositive,
        Convert(int, (CASE WHEN sr.ScoreLevel IS NOT NULL THEN 1 ELSE 0 END)) as TotalCountItem
    FROM dbo.ScreeningScoreLevel sl 
        LEFT JOIN (
{0}
        ) sr ON sl.ScreeningSectionID = sr.ScreeningSectionID 
    WHERE sl.ScreeningSectionID NOT IN('SIH','TCC','PHQ-9','GAD-7') 
", withInnerSqlBuilder);

            //Final SQL
            string sql = string.Format(@"
WITH tblResults(ScreeningSectionID,	ScoreLevel, Name, Indicates, IsPositive, TotalCountItem) AS
(
{0}
)
Select 
    r.ScreeningSectionID,
    r.Name,
    r.Indicates,
    SUM(Convert(bigint,ISNULL(IsPositive, 0))) as TotalPositive,
    SUM(TotalCountItem) as Total,
    r.ScoreLevel
FROM tblResults r
GROUP BY r.Name, r.ScreeningSectionID, r.ScoreLevel, r.Indicates
ORDER BY ScreeningSectionID, ScoreLevel
", withSql);

            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        reportItems.Add(_indicatorReportItemFactory.CreateForScreeningSection(reader));
                    }
                }
            }
            finally
            {
                Disconnect();
            }

            return reportItems;
        }


        //depression PHQ-2/PHQ-9

        public List<IndicatorReportItem> GetBhsIndicatorReportByScoreLevelForDepressionLike(
            SimpleFilterModel filter,
            string mainScreeningSectionID,
            string shortScreeningSectionID
        )
        {
            List<IndicatorReportItem> reportItems = new List<IndicatorReportItem>();

            //inner SELECT query for actual screening results
            QueryBuilder withInnerSqlBuilder = new QueryBuilder(@"
SELECT
    sr.ScreeningSectionID,
    q.QuestionsAsked,
    sr.ScoreLevel
FROM dbo.ScreeningSectionResult sr 
    INNER JOIN dbo.ScreeningResult r ON sr.ScreeningResultID = r.ScreeningResultID 
");

            ClearParameters();

            AddParameter("@ScreeningSectionID", DbType.AnsiString, 5).Value = mainScreeningSectionID;



            withInnerSqlBuilder.AppendWhereCondition("r.IsDeleted = 0", ClauseType.And);
            withInnerSqlBuilder.AppendWhereCondition("sr.ScreeningSectionID IN(@ScreeningSectionID)", ClauseType.And);

            if (filter.Location.HasValue)
            {
                withInnerSqlBuilder.AppendJoinStatement(@"INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID");
                withInnerSqlBuilder.AppendWhereCondition("k.BranchLocationID = @LocationID", ClauseType.And);
                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;
            }
            if (filter.StartDate.HasValue)
            {
                withInnerSqlBuilder.AppendWhereCondition("r.CreatedDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTime).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                withInnerSqlBuilder.AppendWhereCondition("r.CreatedDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTime).Value = filter.EndDate.Value;
            }

            withInnerSqlBuilder.AppendJoinStatement(@"CROSS APPLY (SELECT CASE WHEN COUNT(qr.QuestionID) = 2 THEN 2 ELSE 10 END as QuestionsAsked FROM dbo.ScreeningSectionQuestionResult qr WHERE qr.ScreeningResultID = r.ScreeningResultID AND qr.ScreeningSectionID = sr.ScreeningSectionID) as q");

            //CTE query with all score levels left joined with actual screenings filtered by filer criteria
            string withSql = string.Format(@"
SELECT 
        sl.ScreeningSectionID,
        sr.QuestionsAsked,
        sl.ScoreLevel,
        sl.Name,
        sl.Indicates,
        Convert(int, (CASE WHEN sr.ScoreLevel = sl.ScoreLevel THEN 1 ELSE 0 END)) as IsPositive,
        Convert(int, (CASE WHEN sr.ScoreLevel IS NOT NULL THEN 1 ELSE 0 END)) as TotalCountItem
    FROM dbo.ScreeningScoreLevel sl 
        LEFT JOIN (
{0}
        ) sr ON sl.ScreeningSectionID = sr.ScreeningSectionID 
    WHERE sl.ScreeningSectionID IN(@ScreeningSectionID) 
", withInnerSqlBuilder);

            //Final SQL
            string sql = string.Format(@"
WITH tblResults(ScreeningSectionID,QuestionsAsked,ScoreLevel, Name, Indicates, IsPositive, TotalCountItem) AS
(
{0}
)
Select 
    r.ScreeningSectionID,
    r.QuestionsAsked,
    r.Name,
    r.Indicates,
    SUM(Convert(bigint,ISNULL(IsPositive, 0))) as TotalPositive,
    SUM(TotalCountItem) as Total,
    ScoreLevel
FROM tblResults r
GROUP BY r.Name, r.ScreeningSectionID, r.QuestionsAsked, r.ScoreLevel, r.Indicates
ORDER BY ScreeningSectionID, QuestionsAsked, ScoreLevel
", withSql);

            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        var questionsAsked = reader.Get<int>(1) ?? 0;

                        if (questionsAsked == 0) //no records, add to both both
                        {
                            reportItems.Add(_indicatorReportItemFactory.CreateForDepression(reader, shortScreeningSectionID));
                            reportItems.Add(_indicatorReportItemFactory.CreateForDepression(reader, mainScreeningSectionID));
                        }
                        else
                        {
                            reportItems.Add(_indicatorReportItemFactory.CreateForDepression(reader, questionsAsked == 2 ? shortScreeningSectionID : mainScreeningSectionID));
                        }
                    }
                }
            }
            finally
            {
                Disconnect();
            }

            return reportItems;
        }



        private string GetSqlForUniqueBhsIndicatorReportByScoreLevelForDepressionLike(SimpleFilterModel filter, string screeningSectionID)
        {
            //inner SELECT query for actual screening results
            QueryBuilder withInnerSqlBuilder = new QueryBuilder(@"
SELECT sr.ScreeningSectionID
,q.QuestionsAsked
,MAX(sr.ScoreLevel) as ScoreLevel
,r.FirstName
,r.LastName
,r.MiddleName
,r.Birthday
FROM dbo.ScreeningSectionResult sr 
    INNER JOIN dbo.ScreeningResult r ON sr.ScreeningResultID = r.ScreeningResultID 
");
            //group by
            withInnerSqlBuilder.AppendGroupCondition("r.FirstName, r.LastName, r.MiddleName, r.Birthday, sr.ScreeningSectionID,q.QuestionsAsked");

            ClearParameters();

            AddParameter("@ScreeningSectionID", DbType.AnsiString, 5).Value = screeningSectionID;


            withInnerSqlBuilder.AppendWhereCondition("r.IsDeleted = 0", ClauseType.And);
            withInnerSqlBuilder.AppendWhereCondition("sr.ScreeningSectionID IN(@ScreeningSectionID)", ClauseType.And);

            if (filter.Location.HasValue)
            {
                withInnerSqlBuilder.AppendJoinStatement(@"INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID");
                withInnerSqlBuilder.AppendWhereCondition("k.BranchLocationID = @LocationID", ClauseType.And);
                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;
            }
            if (filter.StartDate.HasValue)
            {
                withInnerSqlBuilder.AppendWhereCondition("r.CreatedDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTime).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                withInnerSqlBuilder.AppendWhereCondition("r.CreatedDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTime).Value = filter.EndDate.Value;
            }

            withInnerSqlBuilder.AppendJoinStatement(@"CROSS APPLY (SELECT CASE WHEN COUNT(qr.QuestionID) = 2 THEN 2 ELSE 10 END as QuestionsAsked FROM dbo.ScreeningSectionQuestionResult qr WHERE qr.ScreeningResultID = r.ScreeningResultID AND qr.ScreeningSectionID = sr.ScreeningSectionID) as q");


            //CTE query with all score levels left joined with actual screenings filtered by filer criteria
            string withSql = string.Format(@"
SELECT 
        sl.ScreeningSectionID,
        sr.QuestionsAsked,
        sl.ScoreLevel,
        sl.Name,
        sl.Indicates,
        Convert(int, (CASE WHEN sr.ScoreLevel = sl.ScoreLevel THEN 1 ELSE 0 END)) as IsPositive,
        Convert(int, (CASE WHEN sr.ScoreLevel IS NOT NULL THEN 1 ELSE 0 END)) as TotalCountItem
    FROM dbo.ScreeningScoreLevel sl 
        LEFT JOIN (
{0}
        ) sr ON sl.ScreeningSectionID = sr.ScreeningSectionID 
    WHERE sl.ScreeningSectionID IN(@ScreeningSectionID) 
", withInnerSqlBuilder);

            //Final SQL
            string sql = string.Format(@"
WITH tblResults(ScreeningSectionID,QuestionsAsked,ScoreLevel, Name, Indicates, IsPositive, TotalCountItem) AS
(
{0}
)
Select 
    r.ScreeningSectionID,
    r.QuestionsAsked,
    r.Name,
    r.Indicates,
    SUM(Convert(bigint,ISNULL(IsPositive, 0))) as TotalPositive,
    SUM(TotalCountItem) as Total,
    ScoreLevel
FROM tblResults r
GROUP BY r.Name, r.ScreeningSectionID, r.QuestionsAsked, r.ScoreLevel, r.Indicates
ORDER BY ScreeningSectionID, QuestionsAsked, ScoreLevel
", withSql);

            _logger.DebugFormat("GetSqlForUniqueBhsIndicatorReportByScoreLevelForDepressionLike: {0}", sql);

            return sql;

        }

        /// <summary>
        /// Get data for By Problem Report with the screen tools that Has 2 or all required questions
        /// </summary>
        /// <param name="mainScreeningSectionID"></param>
        /// <param name="shortScreeningSectionID">i.e. ScreeningSectionDescriptor.DepressionPhq2ID</param>
        /// <returns></returns>
        public List<IndicatorReportItem> GetUniqueBhsIndicatorReportByScoreLevelForDepressionLike(
            SimpleFilterModel filter,
            string mainScreeningSectionID,
            string shortScreeningSectionID
        )
        {

            List<IndicatorReportItem> reportItems = new List<IndicatorReportItem>();

            try
            {
                using (var reader = RunSelectQuery(
                    GetSqlForUniqueBhsIndicatorReportByScoreLevelForDepressionLike(
                        filter,
                        mainScreeningSectionID)))
                {
                    while (reader.Read())
                    {
                        var questionsAsked = reader.Get<int>(1) ?? 0;

                        if (questionsAsked == 0) //no records, add to both
                        {
                            reportItems.Add(_indicatorReportItemFactory.CreateForDepression(reader, shortScreeningSectionID));
                            reportItems.Add(_indicatorReportItemFactory.CreateForDepression(reader, mainScreeningSectionID));
                        }
                        else
                        {
                            reportItems.Add(_indicatorReportItemFactory.CreateForDepression(reader, questionsAsked == 2 ? shortScreeningSectionID : mainScreeningSectionID));
                        }
                    }
                }
            }
            finally
            {
                Disconnect();
            }

            return reportItems;
        }


        //Thinking of Death

        public List<IndicatorReportItem> GetBhsIndicatorReportByScoreLevelForThinkingOfDeath(SimpleFilterModel filter)
        {
            List<IndicatorReportItem> reportItems = new List<IndicatorReportItem>();

            //inner SELECT query for actual screening results
            QueryBuilder withInnerSqlBuilder = new QueryBuilder(@"
SELECT
    qr.AnswerValue,
    q.AnswerScaleID
FROM dbo.ScreeningSectionQuestionResult qr
    INNER JOIN dbo.ScreeningSectionQuestion q ON q.ScreeningSectionID = qr.ScreeningSectionID AND q.QuestionID = qr.QuestionID 
    INNER JOIN dbo.ScreeningResult r ON r.ScreeningResultID = qr.ScreeningResultID 
");

            CommandObject.Parameters.Clear();

            withInnerSqlBuilder.AppendWhereCondition("r.IsDeleted = 0", ClauseType.And);
            withInnerSqlBuilder.AppendWhereCondition("qr.ScreeningSectionID = 'PHQ-9'", ClauseType.And);
            withInnerSqlBuilder.AppendWhereCondition("qr.QuestionID = 9", ClauseType.And);

            if (filter.Location.HasValue)
            {
                withInnerSqlBuilder.AppendJoinStatement(@"INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID");
                withInnerSqlBuilder.AppendWhereCondition("k.BranchLocationID = @LocationID", ClauseType.And);
                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;
            }
            if (filter.StartDate.HasValue)
            {
                withInnerSqlBuilder.AppendWhereCondition("r.CreatedDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTime).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                withInnerSqlBuilder.AppendWhereCondition("r.CreatedDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTime).Value = filter.EndDate.Value;
            }

            //CTE query with all score levels left joined with actual screenings filtered by filer criteria
            string withSql = string.Format(@"
SELECT 
    ao.AnswerScaleID,
    ao.AnswerScaleOptionID,
    ao.OptionValue,
    ao.OptionText,
    Convert(int, (CASE WHEN sr.AnswerValue = ao.OptionValue THEN 1 ELSE 0 END)) as IsPositive,
    Convert(int, (CASE WHEN sr.AnswerValue IS NOT NULL THEN 1 ELSE 0 END)) as TotalCountItem

    FROM dbo.ScreeningSectionQuestion question 
        INNER JOIN dbo.AnswerScale sc ON question.AnswerScaleID = sc.AnswerScaleID 
        INNER JOIN dbo.AnswerScaleOption ao ON sc.AnswerScaleID = ao.AnswerScaleID 
         
        LEFT JOIN (
{0}
        ) sr ON ao.AnswerScaleID = sr.AnswerScaleID
    WHERE question.ScreeningSectionID = 'PHQ-9' AND question.QuestionID = 9
", withInnerSqlBuilder);

            //Final SQL
            string sql = string.Format(@"
WITH tblResults(AnswerScaleID,	AnswerScaleOptionID, OptionValue, Name, IsPositive,TotalCountItem) AS
(
{0}
)
Select 
    r.AnswerScaleID,
    r.Name,
    SUM(Convert(bigint,ISNULL(IsPositive, 0))) as TotalPositive,
    SUM(TotalCountItem) as Total
FROM tblResults r
GROUP BY r.Name, r.AnswerScaleID, r.AnswerScaleOptionID, r.OptionValue
ORDER BY AnswerScaleOptionID
", withSql);

            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    long total = 0;

                    IndicatorReportItem item;
                    while (reader.Read())
                    {
                        item = new IndicatorReportItem();
                        item.ScreeningSectionID = ScreeningSectionDescriptor.DepressionThinkOfDeath;
                        item.ScreeningSectionQuestion = reader.GetString(1);
                        item.PositiveCount = Convert.ToInt64(reader[2]);

                        total = reader.Get<long>(3, true);

                        item.NegativeCount = total - item.PositiveCount;
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

        public List<IndicatorReportItem> GetUniqueBhsIndicatorReportByScoreLevelForThinkingOfDeath(SimpleFilterModel filter)
        {
            List<IndicatorReportItem> reportItems = new List<IndicatorReportItem>();

            //inner SELECT query for actual screening results
            QueryBuilder withInnerSqlBuilder = new QueryBuilder(@"
SELECT
MAX(qr.AnswerValue) as AnswerValue
,q.AnswerScaleID
,r.FirstName
,r.LastName
,r.MiddleName
,r.Birthday
FROM dbo.ScreeningSectionQuestionResult qr
    INNER JOIN dbo.ScreeningSectionQuestion q ON q.ScreeningSectionID = qr.ScreeningSectionID AND q.QuestionID = qr.QuestionID 
    INNER JOIN dbo.ScreeningResult r ON r.ScreeningResultID = qr.ScreeningResultID 
");
            withInnerSqlBuilder.AppendGroupCondition("r.FirstName,r.LastName, r.MiddleName, r.Birthday,q.AnswerScaleID");

            CommandObject.Parameters.Clear();

            withInnerSqlBuilder.AppendWhereCondition("r.IsDeleted = 0", ClauseType.And);
            withInnerSqlBuilder.AppendWhereCondition("qr.ScreeningSectionID = 'PHQ-9'", ClauseType.And);
            withInnerSqlBuilder.AppendWhereCondition("qr.QuestionID = 9", ClauseType.And);

            if (filter.Location.HasValue)
            {
                withInnerSqlBuilder.AppendJoinStatement(@"INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID");
                withInnerSqlBuilder.AppendWhereCondition("k.BranchLocationID = @LocationID", ClauseType.And);
                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;
            }
            if (filter.StartDate.HasValue)
            {
                withInnerSqlBuilder.AppendWhereCondition("r.CreatedDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTime).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                withInnerSqlBuilder.AppendWhereCondition("r.CreatedDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTime).Value = filter.EndDate.Value;
            }

            //CTE query with all score levels left joined with actual screenings filtered by filer criteria
            string withSql = string.Format(@"
SELECT 
    ao.AnswerScaleID,
    ao.AnswerScaleOptionID,
    ao.OptionValue,
    ao.OptionText,
    Convert(int, (CASE WHEN sr.AnswerValue = ao.OptionValue THEN 1 ELSE 0 END)) as IsPositive,
    Convert(int, (CASE WHEN sr.AnswerValue IS NOT NULL THEN 1 ELSE 0 END)) as TotalCountItem
    FROM dbo.ScreeningSectionQuestion question 
        INNER JOIN dbo.AnswerScale sc ON question.AnswerScaleID = sc.AnswerScaleID 
        INNER JOIN dbo.AnswerScaleOption ao ON sc.AnswerScaleID = ao.AnswerScaleID 
         
        LEFT JOIN (
{0}
        ) sr ON ao.AnswerScaleID = sr.AnswerScaleID
    WHERE question.ScreeningSectionID = 'PHQ-9' AND question.QuestionID = 9
", withInnerSqlBuilder);

            //Final SQL
            string sql = string.Format(@"
WITH tblResults(AnswerScaleID,	AnswerScaleOptionID, OptionValue, Name, IsPositive,TotalCountItem) AS
(
{0}
)
Select 
    r.AnswerScaleID,
    r.Name,
    SUM(Convert(bigint,ISNULL(IsPositive, 0))) as TotalPositive,
    SUM(TotalCountItem) as Total
FROM tblResults r
GROUP BY r.Name, r.AnswerScaleID, r.AnswerScaleOptionID, r.OptionValue
ORDER BY AnswerScaleOptionID
", withSql);

            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    long total = 0;

                    IndicatorReportItem item;
                    while (reader.Read())
                    {
                        item = new IndicatorReportItem();
                        item.ScreeningSectionID = ScreeningSectionDescriptor.DepressionThinkOfDeath;
                        item.ScreeningSectionQuestion = reader.GetString(1);
                        item.PositiveCount = Convert.ToInt64(reader[2]);

                        total = reader.Get<long>(3, true);

                        item.NegativeCount = total - item.PositiveCount;
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


        // anxiety
        #region 




        #endregion







        /// <summary>
        /// Get data for Indicator Report, TCC section, by TCC questions
        /// </summary>
        /// <param name="totalCount">return total number of processed patient records</param>
        /// <returns></returns>
        public List<IndicatorReportItem> GetBhsPositiveNagativeIndicatorReportForTCC(SimpleFilterModel filter)
        {

            List<IndicatorReportItem> reportItems = new List<IndicatorReportItem>();

            //inner SELECT query for actual screening results
            QueryBuilder withInnerSqlBuilder = new QueryBuilder(@"
SELECT
    sr.ScreeningSectionID,
    qr.QuestionID,
    qr.AnswerValue
FROM dbo.ScreeningSectionResult sr 
    INNER JOIN dbo.ScreeningResult r ON sr.ScreeningResultID = r.ScreeningResultID 
    LEFT JOIN dbo.ScreeningSectionQuestionResult qr ON qr.ScreeningResultID = sr.ScreeningResultID AND qr.ScreeningSectionID = sr.ScreeningSectionID
    LEFT JOIN dbo.ScreeningSectionQuestion q ON qr.ScreeningSectionID = q.ScreeningSectionID AND qr.QuestionID = q.QuestionID

");

            CommandObject.Parameters.Clear();
            withInnerSqlBuilder.AppendWhereCondition("sr.ScreeningSectionID = 'TCC'", ClauseType.And);
            withInnerSqlBuilder.AppendWhereCondition("q.IsMainQuestion = 0", ClauseType.And);
            withInnerSqlBuilder.AppendWhereCondition("r.IsDeleted = 0", ClauseType.And);

            if (filter.Location.HasValue)
            {
                withInnerSqlBuilder.AppendJoinStatement(@"INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID");
                withInnerSqlBuilder.AppendWhereCondition("k.BranchLocationID = @LocationID", ClauseType.And);
                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;
            }
            if (filter.StartDate.HasValue)
            {
                withInnerSqlBuilder.AppendWhereCondition("r.CreatedDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTime).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                withInnerSqlBuilder.AppendWhereCondition("r.CreatedDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTime).Value = filter.EndDate.Value;
            }

            //CTE query with all score levels left joined with actual screenings filtered by filer criteria
            string withSql = string.Format(@"
SELECT 
    sl.ScreeningSectionID,
    sl.QuestionID,
    sl.QuestionText,
    ISNULL(sr.AnswerValue, 0) as IsPositive,
    Convert(int, (CASE WHEN sr.AnswerValue IS NOT NULL THEN 1 ELSE 0 END)) as TotalCountItem
FROM dbo.ScreeningSectionQuestion sl 
    LEFT JOIN (
{0}
    ) sr ON sl.ScreeningSectionID = sr.ScreeningSectionID AND (sr.QuestionID IS NULL OR sl.QuestionID = sr.QuestionID)
WHERE sl.ScreeningSectionID = 'TCC' and sl.IsMainQuestion=0
", withInnerSqlBuilder);

            //Final SQL
            string sql = string.Format(@"
WITH tblResults(ScreeningSectionID,	QuestionID, QuestionText, IsPositive,TotalCountItem) AS
(
{0}
)
SELECT 
    r.ScreeningSectionID,
    r.QuestionText,
    SUM(Convert(bigint,ISNULL(IsPositive, 0))) as TotalPositive,
    SUM(TotalCountItem) as Total,
    r.QuestionID
FROM tblResults r
GROUP BY r.QuestionText, r.ScreeningSectionID, r.QuestionID
ORDER BY ScreeningSectionID, QuestionID
", withSql);

            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    long total = 0;

                    IndicatorReportItem item;
                    while (reader.Read())
                    {
                        item = new IndicatorReportItem();
                        item.ScreeningSectionID = reader.GetString(0).TrimEnd();
                        item.ScreeningSectionQuestion = reader.GetString(1);
                        item.PositiveCount = reader.Get<long>(2, true);

                        total = reader.Get<long>(3, true);

                        item.NegativeCount = total - item.PositiveCount;
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

        public List<IndicatorReportItem> GetUniqueBhsPositiveNagativeIndicatorReportForTCC(SimpleFilterModel filter)
        {
            List<IndicatorReportItem> reportItems = new List<IndicatorReportItem>();

            //inner SELECT query for actual screening results
            QueryBuilder withInnerSqlBuilder = new QueryBuilder(@"
SELECT sr.ScreeningSectionID
,qr.QuestionID
,MAX(qr.AnswerValue) AS AnswerValue
,r.FirstName
,r.LastName
,r.MiddleName
,r.Birthday
FROM dbo.ScreeningSectionResult sr 
    INNER JOIN dbo.ScreeningResult r ON sr.ScreeningResultID = r.ScreeningResultID 
    LEFT JOIN dbo.ScreeningSectionQuestionResult qr ON qr.ScreeningResultID = sr.ScreeningResultID AND qr.ScreeningSectionID = sr.ScreeningSectionID
    LEFT JOIN dbo.ScreeningSectionQuestion q ON qr.ScreeningSectionID = q.ScreeningSectionID AND qr.QuestionID = q.QuestionID
");

            CommandObject.Parameters.Clear();
            withInnerSqlBuilder.AppendWhereCondition("sr.ScreeningSectionID = 'TCC'", ClauseType.And);
            withInnerSqlBuilder.AppendWhereCondition("q.IsMainQuestion = 0", ClauseType.And);
            withInnerSqlBuilder.AppendWhereCondition("r.IsDeleted = 0", ClauseType.And);

            withInnerSqlBuilder.AppendGroupCondition("sr.ScreeningSectionID,qr.QuestionID,r.FirstName,r.LastName,r.MiddleName,r.Birthday");

            if (filter.Location.HasValue)
            {
                withInnerSqlBuilder.AppendJoinStatement(@"INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID");
                withInnerSqlBuilder.AppendWhereCondition("k.BranchLocationID = @LocationID", ClauseType.And);
                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;
            }
            if (filter.StartDate.HasValue)
            {
                withInnerSqlBuilder.AppendWhereCondition("r.CreatedDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTime).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                withInnerSqlBuilder.AppendWhereCondition("r.CreatedDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTime).Value = filter.EndDate.Value;
            }

            //CTE query with all score levels left joined with actual screenings filtered by filer criteria
            string withSql = string.Format(@"
SELECT 
    sl.ScreeningSectionID,
    sl.QuestionID,
    sl.QuestionText,
    ISNULL(sr.AnswerValue, 0) as IsPositive,
    Convert(int, (CASE WHEN sr.AnswerValue IS NOT NULL THEN 1 ELSE 0 END)) as TotalCountItem
FROM dbo.ScreeningSectionQuestion sl 
    LEFT JOIN (
{0}
    ) sr ON sl.ScreeningSectionID = sr.ScreeningSectionID AND (sr.QuestionID IS NULL OR sl.QuestionID = sr.QuestionID)
WHERE sl.ScreeningSectionID = 'TCC' and sl.IsMainQuestion=0
", withInnerSqlBuilder);

            //Final SQL
            string sql = string.Format(@"
WITH tblResults(ScreeningSectionID,	QuestionID, QuestionText, IsPositive,TotalCountItem) AS
(
{0}
)
SELECT 
    r.ScreeningSectionID,
    r.QuestionText,
    SUM(Convert(bigint,ISNULL(IsPositive, 0))) as TotalPositive,
    SUM(TotalCountItem) as Total,
    r.QuestionID
FROM tblResults r
GROUP BY r.QuestionText, r.ScreeningSectionID, r.QuestionID
ORDER BY ScreeningSectionID, QuestionID
", withSql);

            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    long total = 0;

                    IndicatorReportItem item;
                    while (reader.Read())
                    {
                        item = new IndicatorReportItem();
                        item.ScreeningSectionID = reader.GetString(0).TrimEnd();
                        item.ScreeningSectionQuestion = reader.GetString(1);
                        item.PositiveCount = reader.Get<int>(2, true);

                        total = reader.Get<long>(3, true);

                        item.NegativeCount = total - item.PositiveCount;
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



        /// <summary>
        /// Get # of unique patients for each section for certain fileter criteria
        /// </summary>
        internal Dictionary<string, int> GetUniquePatientsCountBySection(SimpleFilterModel filter)
        {
            Dictionary<string, int> uniquePatientBySection = new Dictionary<string, int>();
            QueryBuilder withSqlBuilder = new QueryBuilder(@"
SELECT 
    sr.ScreeningSectionID,
    COUNT(*) as totalUnique
FROM dbo.ScreeningResult r 
    INNER JOIN dbo.ScreeningSectionResult sr ON r.ScreeningResultID = sr.ScreeningResultID
");
            CommandObject.Parameters.Clear();
            withSqlBuilder.AppendWhereCondition("r.IsDeleted = 0", ClauseType.And);

            if (filter.Location.HasValue)
            {
                withSqlBuilder.AppendJoinStatement(@"INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID");
                withSqlBuilder.AppendWhereCondition("k.BranchLocationID = @LocationID", ClauseType.And);
                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;
            }
            if (filter.StartDate.HasValue)
            {
                withSqlBuilder.AppendWhereCondition("r.CreatedDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTime).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                withSqlBuilder.AppendWhereCondition("r.CreatedDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTime).Value = filter.EndDate.Value;
            }
            withSqlBuilder.AppendGroupCondition(" FirstName, LastName, MiddleName, Birthday, sr.ScreeningSectionID");

            string sql = string.Format(@"
WITH uniquePatientScreenings (ScreeningSectionID, cnt) AS 
(
{0}
)
SELECT ScreeningSectionID, COUNT(*) 
FROM uniquePatientScreenings
GROUP BY ScreeningSectionID
", withSqlBuilder);
            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        uniquePatientBySection.Add(reader.GetString(0).TrimEnd(), reader.GetInt32(1));
                    }
                }
            }
            finally
            {
                Disconnect();
            }
            return uniquePatientBySection;
        }

        public List<IndicatorReportByAgeItem> GetBhsDrugsofChoiceByAge(SimpleFilterModel filter)
        {
            Trace.TraceInformation("Calling GetBhsDrugsofChoiceByAge");


            List<IndicatorReportByAgeItem> reportItems = new List<IndicatorReportByAgeItem>();

            //inner SELECT query for actual screening results
            QueryBuilder withInnerSqlBuilder = new QueryBuilder(@"
SELECT 
qr.AnswerValue
,qr.QuestionID
,r.PatientName
,[dbo].[fn_GetAge](r.Birthday) as Age
FROM dbo.ScreeningSectionQuestionResult qr
    INNER JOIN dbo.ScreeningSectionResult sr ON sr.ScreeningResultID = qr.ScreeningResultID AND sr.ScreeningSectionID = qr.ScreeningSectionID
    INNER JOIN dbo.ScreeningResult r ON sr.ScreeningResultID = r.ScreeningResultID
");

            CommandObject.Parameters.Clear();
            withInnerSqlBuilder.AppendWhereCondition("r.IsDeleted = 0", ClauseType.And);
            withInnerSqlBuilder.AppendWhereCondition("sr.ScreeningSectionID IN(@DrugsSectionID)", ClauseType.And);
            AddParameter("@DrugsSectionID", DbType.String, 5).Value = "DOCH";

            if (filter.Location.HasValue)
            {
                withInnerSqlBuilder.AppendJoinStatement(@"INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID");
                withInnerSqlBuilder.AppendWhereCondition("k.BranchLocationID = @LocationID", ClauseType.And);
                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;
            }
            if (filter.StartDate.HasValue)
            {
                withInnerSqlBuilder.AppendWhereCondition("r.CreatedDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTime).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                withInnerSqlBuilder.AppendWhereCondition("r.CreatedDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTime).Value = filter.EndDate.Value;
            }


            //CTE query with all drug of choice options left joined with actual screenings filtered by filer criteria
            string withSql = string.Format(@"
SELECT 
    d.ID,
    d.Name,
    d.OrderIndex,
    sr.AnswerValue,
    questions.QuestionID,
    ISNULL(sr.Age, 0) as Age
FROM dbo.DrugOfChoice d 
    CROSS JOIN (select QuestionID from dbo.ScreeningSectionQuestion WHERE ScreeningSectionID = @DrugsSectionID) as questions
        LEFT JOIN (
{0}
        ) sr ON d.ID = sr.AnswerValue AND sr.QuestionID = questions.QuestionID
    WHERE d.ID > 0 
", withInnerSqlBuilder);

            //Final SQL
            string sql = string.Format(@"
WITH tblResults(ID,Name, OrderIndex, AnswerValue, QuestionID, Age) AS
(
{0}
)
Select 
    r.ID,
    r.Name,
    r.Age,
    r.QuestionID,
    COUNT_BIG(r.AnswerValue) as Total
FROM tblResults r
GROUP BY r.Name, r.ID, r.OrderIndex, r.QuestionID, Age
ORDER BY r.QuestionID, r.OrderIndex, Age
", withSql);

            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        var item = new IndicatorReportByAgeItem
                        {
                            ScreeningSectionID = string.Concat(ScreeningSectionDescriptor.DrugOfChoice, "_", reader.Get<int>(3)),
                            QuestionID = reader.Get<int>(0) ?? 0,
                            ScreeningSectionQuestion = reader.GetString(1),
                            ScreeningSectionIndicates = String.Empty,
                            Age = reader.Get<int>(2) ?? 0,
                            PositiveCount = reader.GetInt64(4),

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

        public List<IndicatorReportByAgeItem> GetBhsDrugsofChoiceUniquePatientsByAge(SimpleFilterModel filter)
        {
            Trace.TraceInformation("Calling GetBhsDrugsofChoiceUniquePatientsByAge");


            List<IndicatorReportByAgeItem> reportItems = new List<IndicatorReportByAgeItem>();

            //inner SELECT query for actual screening results
            QueryBuilder withInnerSqlBuilder = new QueryBuilder(@"
SELECT 
qr.AnswerValue
,qr.QuestionID
,r.PatientName
,[dbo].[fn_GetAge](r.Birthday) as Age
FROM dbo.ScreeningSectionQuestionResult qr
    INNER JOIN dbo.ScreeningSectionResult sr ON sr.ScreeningResultID = qr.ScreeningResultID AND sr.ScreeningSectionID = qr.ScreeningSectionID
    INNER JOIN dbo.ScreeningResult r ON sr.ScreeningResultID = r.ScreeningResultID
");
            //group by
            withInnerSqlBuilder.AppendGroupCondition("r.PatientName, r.Birthday, qr.QuestionID, qr.AnswerValue");

            CommandObject.Parameters.Clear();
            withInnerSqlBuilder.AppendWhereCondition("r.IsDeleted = 0", ClauseType.And);
            withInnerSqlBuilder.AppendWhereCondition("sr.ScreeningSectionID IN(@DrugsSectionID)", ClauseType.And);
            AddParameter("@DrugsSectionID", DbType.String, 5).Value = "DOCH";

            if (filter.Location.HasValue)
            {
                withInnerSqlBuilder.AppendJoinStatement(@"INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID");
                withInnerSqlBuilder.AppendWhereCondition("k.BranchLocationID = @LocationID", ClauseType.And);
                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;
            }
            if (filter.StartDate.HasValue)
            {
                withInnerSqlBuilder.AppendWhereCondition("r.CreatedDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTime).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                withInnerSqlBuilder.AppendWhereCondition("r.CreatedDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTime).Value = filter.EndDate.Value;
            }

            //CTE query with all drug of choice options left joined with actual screenings filtered by filer criteria
            string withSql = string.Format(@"
SELECT 
    d.ID,
    d.Name,
    d.OrderIndex,
    sr.AnswerValue,
    questions.QuestionID,
    ISNULL(sr.Age, 0) as Age
FROM dbo.DrugOfChoice d 
    CROSS JOIN (select QuestionID from dbo.ScreeningSectionQuestion WHERE ScreeningSectionID = @DrugsSectionID) as questions
        LEFT JOIN (
{0}
        ) sr ON d.ID = sr.AnswerValue and sr.QuestionID = questions.QuestionID
    WHERE d.ID > 0 
", withInnerSqlBuilder);

            //Final SQL
            string sql = string.Format(@"
WITH tblResults(ID,Name, OrderIndex, AnswerValue, QuestionID, Age) AS
(
{0}
)
Select 
    r.ID,
    r.Name,
    r.Age,
    r.QuestionID,
    COUNT_BIG(r.AnswerValue) as Total
FROM tblResults r
GROUP BY r.QuestionID, r.Name, r.ID, r.OrderIndex, Age
ORDER BY r.QuestionID, r.OrderIndex, Age
", withSql);

            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        var item = new IndicatorReportByAgeItem
                        {
                            ScreeningSectionID = string.Concat(ScreeningSectionDescriptor.DrugOfChoice, "_", reader.Get<int>(3)),
                            QuestionID = reader.Get<int>(0) ?? 0,
                            ScreeningSectionQuestion = reader.GetString(1),
                            ScreeningSectionIndicates = String.Empty,
                            Age = reader.Get<int>(2) ?? 0,
                            PositiveCount = reader.GetInt64(4),

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
    }


}

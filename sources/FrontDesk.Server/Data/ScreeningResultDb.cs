using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using FrontDesk.Common.Data;
using FrontDesk.Server.Screening;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Data.Helpers;
using FrontDesk.Common.Extensions;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Data
{
    public interface IScreenerResultReadRepository
    {
        ScreeningResult GetScreeningResult(long screeningResultID);
        List<PatientCheckInDtoModel> GetPatientScreeningsBySort(ScreeningResultFilterModel filter);
        ScreeningsByScoreLevelCountResult GetScreeningsCountByScoreLevel(SimpleFilterModel filter);
        List<PatientCheckInViewModel> GetRelatedPatientScreeningsByProblemSort(long mainRowID, ScreeningResultFilterModel filter);
        long? GetLatestForKiosk(short kioskID, ScreeningPatientIdentity result);
        void Update(ScreeningResult screeningResult);
    }

    public interface IScreenerResultRepository : IScreenerResultReadRepository
    {
        void UpdatePatientInfo(ScreeningResult result);
        void UpdateExportInfo(ScreeningResult screeningResult);
        long GetTotalCount();
    }

    public class ScreeningResultDb : DBDatabase, IScreenerResultRepository
    {

        #region Constructors
        public ScreeningResultDb() : base(0)
        {

        }

        internal ScreeningResultDb(DbConnection sharedConnection) : base(sharedConnection)
        {

        }

        #endregion


        /// <summary>
        /// Add new screening result into database
        /// </summary>
        /// <param name="screeningResult"></param>
        internal void InsertScreeningResult(ScreeningResult screeningResult)
        {
            string sql = @"
INSERT INTO dbo.ScreeningResult(
    [ScreeningID]
    ,[FirstName]
    ,[LastName]
    ,[MiddleName]
    ,[Birthday]
    ,[StreetAddress]
    ,[City]
    ,[StateID]
    ,[ZipCode]
    ,[Phone]
    ,[KioskID]
    ,[CreatedDate]
    ,WithErrors
)
VALUES(
    @ScreeningID
    ,@FirstName
    ,@LastName
    ,@MiddleName
    ,@Birthday
    ,@StreetAddress
    ,@City
    ,@StateID
    ,@ZipCode
    ,@Phone
    ,@KioskID
    ,@CreatedDate
    ,@WithErrors
);
SET @ScreeningResultID = SCOPE_IDENTITY();
";
            CommandObject.Parameters.Clear();
            var IdParam = AddParameter("@ScreeningResultID", DbType.Int64, ParameterDirection.Output);

            AddParameter("@ScreeningID", DbType.AnsiString, 4).Value = screeningResult.ScreeningID;
            AddParameter("@FirstName", DbType.String, 128).Value = SqlParameterSafe(screeningResult.FirstName);
            AddParameter("@LastName", DbType.String, 128).Value = SqlParameterSafe(screeningResult.LastName);
            AddParameter("@MiddleName", DbType.String, 128).Value = SqlParameterSafe(screeningResult.MiddleName);
            AddParameter("@Birthday", DbType.Date).Value = screeningResult.Birthday.Date;
            AddParameter("@StreetAddress", DbType.String, 512).Value = SqlParameterSafe(screeningResult.StreetAddress);
            AddParameter("@City", DbType.String, 255).Value = SqlParameterSafe(screeningResult.City);
            AddParameter("@StateID", DbType.AnsiString, 2).Value = SqlParameterSafe(screeningResult.StateID);
            AddParameter("@ZipCode", DbType.AnsiString, 5).Value = SqlParameterSafe(screeningResult.ZipCode);
            AddParameter("@Phone", DbType.AnsiString, 14).Value = SqlParameterSafe(screeningResult.Phone);

            AddParameter("@KioskID", DbType.Int16).Value = SqlParameterSafe(screeningResult.KioskID, true, (short)0); //save null when equal to zero
            AddParameter("@CreatedDate", DbType.DateTimeOffset).Value = screeningResult.CreatedDate;

            AddParameter("@WithErrors", DbType.Boolean).Value = screeningResult.WithErrors;

            try
            {
                BeginTransaction();
                StartConnectionSharing();

                RunNonSelectQuery(sql);
                screeningResult.ID = Convert.ToInt64(IdParam.Value);

                //save section answers
                foreach (var section in screeningResult.SectionAnswers)
                {
                    section.ScreeningResultID = screeningResult.ID;
                    InsertScreeningSectionResult(section);
                }

                StopConnectionSharing();
                CommitTransaction();

            }
            catch (Exception)
            {
                StopConnectionSharing();
                RollbackTransaction();
                throw;
            }
            finally
            {
                Disconnect();
            }
        }

        /// <summary>
        /// add new screening section answer
        /// </summary>
        /// <param name="sectionResult"></param>
        internal void InsertScreeningSectionResult(ScreeningSectionResult sectionResult)
        {
            string sql = @"
INSERT INTO dbo.ScreeningSectionResult(
    ScreeningResultID
    ,ScreeningSectionID
    ,AnswerValue
    ,Score
    ,ScoreLevel
)
VALUES(
    @ScreeningResultID
    ,@ScreeningSectionID
    ,@AnswerValue
    ,@Score
    ,@ScoreLevel
);
SET @ScreeningResultID = SCOPE_IDENTITY();
";
            CommandObject.Parameters.Clear();
            AddParameter("@ScreeningResultID", DbType.Int64).Value = sectionResult.ScreeningResultID;
            AddParameter("@ScreeningSectionID", DbType.AnsiString, 5).Value = sectionResult.ScreeningSectionID;
            AddParameter("@AnswerValue", DbType.Int32).Value = sectionResult.AnswerValue;
            AddParameter("@Score", DbType.Int32).Value = sectionResult.Score.HasValue ? (object)sectionResult.Score.Value : DBNull.Value;
            AddParameter("@ScoreLevel", DbType.Int32).Value = sectionResult.ScoreLevel.HasValue ? (object)sectionResult.ScoreLevel : DBNull.Value;

            try
            {

                BeginTransaction();
                StartConnectionSharing();

                RunNonSelectQuery(sql);

                //save questions' answers
                foreach (var question in sectionResult.Answers)
                {
                    question.ScreeningResultID = sectionResult.ScreeningResultID;
                    InsertScreeningSectionQuestionResult(question);
                }

                StopConnectionSharing();
                CommitTransaction();

            }
            catch (Exception)
            {
                StopConnectionSharing();
                RollbackTransaction();
                throw;
            }
            finally
            {
                Disconnect();
            }
        }
        /// <summary>
        /// Add new screening section's question answer
        /// </summary>
        /// <param name="questionResult"></param>
        internal void InsertScreeningSectionQuestionResult(ScreeningSectionQuestionResult questionResult)
        {
            string sql = @"
INSERT INTO dbo.ScreeningSectionQuestionResult(
    ScreeningResultID
    ,ScreeningSectionID
    ,QuestionID
    ,AnswerValue
)
VALUES(
    @ScreeningResultID
    ,@ScreeningSectionID
    ,@QuestionID
    ,@AnswerValue
);
SET @ScreeningResultID = SCOPE_IDENTITY();
";
            CommandObject.Parameters.Clear();
            AddParameter("@ScreeningResultID", DbType.Int64).Value = questionResult.ScreeningResultID;
            AddParameter("@ScreeningSectionID", DbType.AnsiString, 5).Value = questionResult.ScreeningSectionID;
            AddParameter("@QuestionID", DbType.Int32).Value = questionResult.QuestionID;
            AddParameter("@AnswerValue", DbType.Int32).Value = questionResult.AnswerValue;

            try
            {

                RunNonSelectQuery(sql);
            }
            finally
            {
                Disconnect();
            }
        }

        /// <summary>
        /// Delete screening result
        /// </summary>
        [Obsolete("migrated to ScreensRepository")]
        public void Delete(long screeningResultID)
        {
            string sql = @"
delete from ScreeningSectionQuestionResult where ScreeningResultID=@ScreeningResultID;
delete from ScreeningSectionResult where ScreeningResultID=@ScreeningResultID;
update dbo.BhsDemographics SET ScreeningResultID = NULL where ScreeningResultID=@ScreeningResultID;
delete from export.SmartExportLog WHERE ScreeningResultID=@ScreeningResultID;
delete from ScreeningResult where ScreeningResultID=@ScreeningResultID;

";

            AddParameter("@ScreeningResultID", DbType.Int64).Value = screeningResultID;

            try
            {
                Connect();
                BeginTransaction();
                RunNonSelectQuery(sql);
                CommitTransaction();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                Disconnect();
            }
        }

        /// <summary>
        /// Update screening result
        /// </summary>
        /// <param name="screeningResult"></param>
        public void Update(ScreeningResult screeningResult)
        {
            string sql = @"[dbo].[uspUpdateScreeningResultPatientInfo]";
            CommandObject.Parameters.Clear();
            AddParameter("@ScreeningResultID", DbType.Int64).Value = screeningResult.ID;
            AddParameter("@FirstName", DbType.String, 128).Value = SqlParameterSafe(screeningResult.FirstName);
            AddParameter("@LastName", DbType.String, 128).Value = SqlParameterSafe(screeningResult.LastName);
            AddParameter("@MiddleName", DbType.String, 128).Value = SqlParameterSafe(screeningResult.MiddleName);
            AddParameter("@Birthday", DbType.Date).Value = screeningResult.Birthday;
            AddParameter("@Phone", DbType.AnsiString, 14).Value = SqlParameterSafe(screeningResult.Phone?.Trim());
            AddParameter("@StreetAddress", DbType.String, 512).Value = SqlParameterSafe(screeningResult.StreetAddress);
            AddParameter("@City", DbType.String, 255).Value = SqlParameterSafe(screeningResult.City);
            AddParameter("@StateID", DbType.AnsiString, 2).Value = SqlParameterSafe(screeningResult.StateID);
            AddParameter("@ZipCode", DbType.AnsiString, 5).Value = SqlParameterSafe(screeningResult.ZipCode);

            try
            {
                Connect();
                RunProcedureNonSelectQuery(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Disconnect();
            }
        }

        /// <summary>
        /// Update patient info from EHR
        /// </summary>
        /// <param name="screeningResult"></param>
        public void UpdatePatientInfo(ScreeningResult screeningResult)
        {
            string sql = @"
UPDATE dbo.ScreeningResult SET
    [FirstName] = @FirstName
    ,[LastName] = @LastName
    ,[MiddleName] = @MiddleName
    ,[Birthday] = @Birthday
WHERE ScreeningResultID = @ScreeningResultID;

-- update name 
UPDATE dbo.BhsDemographics SET
    [FirstName] = @FirstName
    ,[LastName] = @LastName
    ,[MiddleName] = @MiddleName
    ,[Birthday] = @Birthday
WHERE ScreeningResultID = @ScreeningResultID AND CompleteDate IS NULL;
";

            CommandObject.Parameters.Clear();
            AddParameter("@ScreeningResultID", DbType.Int64).Value = screeningResult.ID;
            AddParameter("@FirstName", DbType.String, 128).Value = SqlParameterSafe(screeningResult.FirstName);
            AddParameter("@LastName", DbType.String, 128).Value = SqlParameterSafe(screeningResult.LastName);
            AddParameter("@MiddleName", DbType.String, 128).Value = SqlParameterSafe(screeningResult.MiddleName);
            AddParameter("@Birthday", DbType.Date).Value = screeningResult.Birthday;

            try
            {
                RunNonSelectQuery(sql);
            }
            finally
            {
                Disconnect();
            }
        }

        /// <summary>
        /// Update screening result
        /// </summary>
        /// <param name="screeningResult"></param>
        public void UpdateExportInfo(ScreeningResult screeningResult)
        {
            string sql = @"
UPDATE dbo.ScreeningResult SET
    [ExportDate] = @ExportDate
    ,[ExportedBy] = @ExportedBy
    ,[ExportedToPatientID] = @ExportedToPatientID
    ,[ExportedToVisitID] = @ExportedToVisitID
    ,[ExportedToHRN] = @ExportedToHRN
    ,[ExportedToVisitDate] = @ExportedToVisitDate
    ,[ExportedToVisitLocation] = @ExportedToVisitLocation
WHERE ScreeningResultID = @ScreeningResultID";

            CommandObject.Parameters.Clear();
            AddParameter("@ScreeningResultID", DbType.Int64).Value = screeningResult.ID;

            AddParameter("@ExportDate", DbType.DateTimeOffset).Value = SqlParameterSafe(screeningResult.ExportDate);
            AddParameter("@ExportedBy", DbType.Int32).Value = screeningResult.ExportedBy;
            AddParameter("@ExportedToPatientID", DbType.Int32).Value = screeningResult.ExportedToPatientID;
            AddParameter("@ExportedToVisitID", DbType.Int32).Value = screeningResult.ExportedToVisitID;
            AddParameter("@ExportedToHRN", DbType.String, 255).Value = SqlParameterSafe(screeningResult.ExportedToHRN);
            AddParameter("@ExportedToVisitDate", DbType.DateTime).Value = SqlParameterSafe(screeningResult.ExportedToVisitDate);
            AddParameter("@ExportedToVisitLocation", DbType.String, 255).Value = SqlParameterSafe(screeningResult.ExportedToVisitLocation);

            try
            {
                Connect();
                RunNonSelectQuery(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Disconnect();
            }
        }


        #region UI Binding

        /// <summary>
        /// Get screen result for check-in list
        /// </summary>
        internal DataSet GetLatestCheckinsForDisplay(ScreeningResultFilterModel filter, int startRowIndex, int maximumRows, string orderBy)
        {
            string innerOrderBy = string.Empty;
            string totalOrderBy = string.Empty;

            if (string.IsNullOrEmpty(orderBy)) orderBy = "LastCheckinDate DESC"; // default sort order

            //map user field names to the query field names
            if (orderBy.Contains("LastCheckinDate"))
            {
                innerOrderBy = orderBy.Replace("LastCheckinDate", "MAX(r.CreatedDate)");
                totalOrderBy = orderBy;
            }
            else if (orderBy.Contains("FullName"))
            {
                innerOrderBy = orderBy.Replace("FullName", "r.PatientName");
                totalOrderBy = orderBy;
            }
            else if (orderBy.Contains("Birthday"))
            {
                innerOrderBy = orderBy.Replace("Birthday", "r.Birthday");
                totalOrderBy = orderBy;
            }
            else if (orderBy.Contains("ExportDate"))
            {
                innerOrderBy = orderBy.Replace("ExportDate", "MAX(ExportDate)");
                if (orderBy.Contains("DESC"))
                {
                    totalOrderBy = orderBy.Replace("ExportDate", "HasExport, LastCheckinDate DESC, ExportDate");
                }
                else
                {
                    totalOrderBy = orderBy.Replace("ExportDate", "HasExport DESC, LastCheckinDate DESC, ExportDate");
                }
            }
            QueryBuilder qbInnerSql = new QueryBuilder(string.Format(@"
SELECT TOP(@startRowIndex + @maxRows) 
    ROW_NUMBER() OVER ( ORDER BY {0}) as RowNumber, 
    MAX(r.ScreeningResultID) as ScreeningResultID
    , r.PatientName as FullName
    , r.Birthday
    , MAX(r.CreatedDate) AS LastCheckinDate
    --get export date
    -- if 0 all member have export date
    ,(Convert(bit,  COUNT(r.ScreeningResultID) - COUNT(r.ExportDate) - COUNT(CASE WHEN qr.SectionCount = 0 THEN 1 END))) AS HasExport
    ,MAX(ExportDate)
FROM ScreeningResult r
    OUTER APPLY (SELECT ISNULL(COUNT(*),0) as SectionCount FROM dbo.ScreeningSectionResult qr WHERE qr.ScreeningResultID = r.ScreeningResultID) qr
", innerOrderBy));

            string outerSql = @"
WITH tblCheckIns(RowNumber, ScreeningResultID, FullName, Birthday, LastCheckinDate, HasExport, ExportDate)
AS ({0})
SELECT t.ScreeningResultID, FullName, Birthday, LastCheckinDate, HasExport, ExportDate
FROM tblCheckIns t
WHERE RowNumber BETWEEN @startRowIndex AND (@startRowIndex + @maxRows)
ORDER BY {1} ";

            // -- GROUP BY FirstName, MiddleName, LastName, Birthday
            // -- ORDER BY LastCheckinDate DESC
            CommandObject.Parameters.Clear();
            //parameters for pagination
            AddParameter("@startRowIndex", DbType.Int32).Value = startRowIndex + 1;
            AddParameter("@maxRows", DbType.Int32).Value = maximumRows;

            qbInnerSql.AppendGroupCondition("r.PatientName, r.Birthday");
            qbInnerSql.AppendOrderCondition(innerOrderBy);

            if (filter.ScreeningResultID.HasValue)
            {
                qbInnerSql.AppendWhereCondition("(r.ScreeningResultID = @ScreeningResultID)", ClauseType.And);
                AddParameter("@ScreeningResultID", DbType.Int64).Value = filter.ScreeningResultID.Value;
            }

            if (!string.IsNullOrEmpty(filter.FirstName))
            {
                qbInnerSql.AppendWhereCondition("(r.FirstName LIKE @FirstName)", ClauseType.And);
                AddParameter("@FirstName", DbType.String, 128).Value = SqlLikeStringPrepeare(filter.FirstName, LikeCondition.StartsWith);
            }
            if (!string.IsNullOrEmpty(filter.LastName))
            {
                qbInnerSql.AppendWhereCondition("(r.LastName LIKE @LastName)", ClauseType.And);
                AddParameter("@LastName", DbType.String, 128).Value = SqlLikeStringPrepeare(filter.LastName, LikeCondition.StartsWith);
            }
            if (filter.Location.HasValue)
            {
                // TODO: add join to kiosks table, choose kiosks by location
                qbInnerSql.AppendJoinStatement(@" INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID ");
                qbInnerSql.AppendWhereCondition("k.BranchLocationID = @LocationID", ClauseType.And);

                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;
            }
            if (filter.StartDate.HasValue)
            {
                qbInnerSql.AppendWhereCondition("r.CreatedDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTimeOffset).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                qbInnerSql.AppendWhereCondition("r.CreatedDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTimeOffset).Value = filter.EndDate.Value;
            }

            try
            {
                Connect();
                var sql = string.Format(outerSql, qbInnerSql.ToString(), totalOrderBy);

                Logger.DebugFormat("GetLatestCheckinsForDisplay SQL:\r\n{0}", sql);

                DataSet ds = this.GetDataSet(sql);
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                this.Disconnect();
            }
        }

        internal int GetLatestCheckinsForDisplayCount(ScreeningResultFilterModel filter)
        {
            int rowCount = 0;
            //WITH statement
            QueryBuilder qbInnerSql = new QueryBuilder(@"
SELECT 
    MAX(r.ScreeningResultID) as ScreeningResultID
    , r.PatientName as FullName
    , r.Birthday
    , MAX(r.CreatedDate) AS LastCheckinDate
    , COUNT(r.ScreeningResultID) - COUNT(r.ExportDate) AS HaveExport
    ,MAX(ExportDate)
FROM ScreeningResult r
");

            string outerSql = @"
WITH tblCheckIns(ScreeningResultID, FullName, Birthday, LastCheckinDate, HaveExport, ExportDate)
AS ({0})
SELECT count(*)
FROM tblCheckIns t
";

            CommandObject.Parameters.Clear();
            try
            {


                qbInnerSql.AppendGroupCondition("r.PatientName, r.Birthday");

                if (filter.ScreeningResultID.HasValue)
                {
                    qbInnerSql.AppendWhereCondition("(r.ScreeningResultID = @ScreeningResultID)", ClauseType.And);
                    AddParameter("@ScreeningResultID", DbType.Int64).Value = filter.ScreeningResultID.Value;
                }

                if (!string.IsNullOrEmpty(filter.FirstName))
                {
                    qbInnerSql.AppendWhereCondition("(r.FirstName LIKE @FirstName)", ClauseType.And);
                    AddParameter("@FirstName", DbType.String, 128).Value = SqlLikeStringPrepeare(filter.FirstName, LikeCondition.StartsWith);
                }
                if (!string.IsNullOrEmpty(filter.LastName))
                {
                    qbInnerSql.AppendWhereCondition("(r.LastName LIKE @LastName)", ClauseType.And);
                    AddParameter("@LastName", DbType.String, 128).Value = SqlLikeStringPrepeare(filter.LastName, LikeCondition.StartsWith);
                }
                if (filter.Location.HasValue)
                {
                    // TODO: add join to kiosks table, choose kiosks by location
                    qbInnerSql.AppendJoinStatement(@" INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID ");
                    qbInnerSql.AppendWhereCondition("k.BranchLocationID = @LocationID", ClauseType.And);

                    AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;
                }
                if (filter.StartDate.HasValue)
                {
                    qbInnerSql.AppendWhereCondition("r.CreatedDate >= @StartDate", ClauseType.And);
                    AddParameter("@StartDate", DbType.DateTimeOffset).Value = filter.StartDate.Value;
                }
                if (filter.EndDate.HasValue)
                {
                    qbInnerSql.AppendWhereCondition("r.CreatedDate < @EndDate", ClauseType.And);
                    AddParameter("@EndDate", DbType.DateTimeOffset).Value = filter.EndDate.Value;
                }

                Connect();

                rowCount = Convert.ToInt32(this.RunScalarQuery(string.Format(outerSql, qbInnerSql.ToString())));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                this.Disconnect();
            }
            return rowCount;
        }

        /// <summary>
        /// Get  screen results for check-in list
        /// </summary>
        internal List<PatientCheckInViewModel> GetRelatedPatientScreenings(long mainRowID, ScreeningResultFilterModel filter)
        {
            List<PatientCheckInViewModel> result = new List<PatientCheckInViewModel>();

            //script change for persons under fifteen years old
            //INNER JOIN dbo.ScreeningSectionResult to LEFT JOIN dbo.ScreeningSectionResult
            string sql = @"
SELECT
r.ScreeningResultID
, s.ScreeningName
, r.CreatedDate
, CASE 
    WHEN SUM(ISNULL(ssr.ScoreLevel,0)) > 0 THEN 1
    ELSE 0
  END AS Positive
 ,r.ExportDate
 ,r.ExportedToHRN
 ,CASE WHEN COUNT(ssr.ScoreLevel) > 0 THEN 1 ELSE 0 END AS HasAnyScreening
 ,CASE WHEN r.StreetAddress IS NOT NULL THEN 1 ELSE 0 END AS HasAddress
FROM dbo.ScreeningResult r
    INNER JOIN dbo.Screening s ON r.ScreeningID = s.ScreeningID
    INNER JOIN dbo.ScreeningResult main 
        ON r.ScreeningResultID = main.ScreeningResultID 
            OR (main.PatientName = r.PatientName AND main.Birthday = r.Birthday)
    LEFT JOIN dbo.ScreeningSectionResult ssr ON r.ScreeningResultID = ssr.ScreeningResultID
";
            CommandObject.Parameters.Clear();

            QueryBuilder qb = new QueryBuilder(sql);

            qb.AppendGroupCondition("r.ScreeningResultID, s.ScreeningName, r.CreatedDate, r.ExportDate, r.ExportedToHRN, r.StreetAddress");
            qb.AppendOrderCondition("r.CreatedDate", OrderType.Desc);
            qb.AppendWhereCondition("main.ScreeningResultID = @ScreeningResultID and r.IsDeleted = 0 and main.IsDeleted = 0", ClauseType.And);

            AddParameter("@ScreeningResultID", DbType.Int64).Value = mainRowID;

            if (filter.ScreeningResultID.HasValue)
            {
                qb.AppendWhereCondition("r.ScreeningResultID = @ScreeningResultID", ClauseType.And);
            }

            if (filter.Location.HasValue)
            {
                // add join to kiosks table, choose kiosks by location
                qb.AppendJoinStatement(@" INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID ");
                qb.AppendWhereCondition("k.BranchLocationID = @LocationID", ClauseType.And);

                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;
            }
            if (filter.StartDate.HasValue)
            {
                qb.AppendWhereCondition("r.CreatedDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTimeOffset).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                qb.AppendWhereCondition("r.CreatedDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTimeOffset).Value = filter.EndDate.Value;
            }

            try
            {
                //ds = GetDataSet(qb.ToString());
                using (var reader = RunSelectQuery(qb.ToString()))
                {
                    while (reader.Read())
                    {
                        result.Add(new PatientCheckInViewModel
                        {
                            ScreeningResultID = reader.GetInt64(0),
                            ScreeningName = reader.GetString(1),
                            CreatedDate = (DateTimeOffset)reader[2],
                            IsPositive = Convert.ToInt32(reader[3]) > 0,
                            ExportDate = Convert.IsDBNull(reader[4]) ? (DateTimeOffset?)null : (DateTimeOffset?)reader[4],
                            ExportedToHRN = Convert.ToString(reader[5]),
                            HasAnyScreening = Convert.ToInt32(reader[6]) > 0,
                            HasAddress = Convert.ToInt32(reader[7]) > 0
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

        /// <summary>
        /// Get total number of check-in records in the database
        /// </summary>
        /// <returns></returns>
        public long GetTotalCount()
        {
            const string sql = @"SELECT COUNT_BIG(*) FROM dbo.ScreeningResult";
            try
            {
                return RunScalarQuery<long>(sql) ?? 0;
            }
            finally
            {
                Disconnect();
            }
        }





        #endregion

        #region Get Screening Result data object

        internal List<long> GetAllScreeningResultIDsForScreening(string screeningID)
        {
            List<long> list = new List<long>();

            var sql = @"
SELECT 
    r.ScreeningResultID
FROM dbo.ScreeningResult r
WHERE r.ScreeningID = @ScreeningID
";
            CommandObject.Parameters.Clear();
            AddParameter("@ScreeningID", DbType.AnsiString, 4).Value = screeningID;

            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        list.Add(reader.GetInt64(0));

                    }
                }

            }
            finally
            {
                Disconnect();
            }
            return list;
        }

        /// <summary>
        /// Get screening result
        /// </summary>
        /// <param name="screeningResultID"></param>
        /// <returns></returns>
        public ScreeningResult GetScreeningResult(long screeningResultID)
        {
            ScreeningResult result = null;

            var sql = @"
SELECT 
    r.[ScreeningResultID]
    ,r.[ScreeningID]
    ,r.[FirstName]
    ,r.[LastName]
    ,r.[MiddleName]
    ,r.[Birthday]
    ,r.[StreetAddress]
    ,r.[City]
    ,r.[StateID]
    ,s.[Name] as StateName
    ,r.[ZipCode]
    ,r.[Phone]
    ,r.[KioskID]
    ,r.[CreatedDate]
    ,r.[IsDeleted]
    ,r.[DeletedDate]
    ,r.[DeletedBy]
    ,r.[WithErrors]
    ,r.ExportDate
    ,r.ExportedBy
    ,r.ExportedToPatientID
    ,r.ExportedToVisitID
    ,r.ExportedToHRN
    ,r.ExportedToVisitDate
    ,r.ExportedToVisitLocation
    ,k.[KioskName] as KioskLabel
    ,l.[Name] as LocationLabel
    ,l.[BranchLocationID]
FROM dbo.ScreeningResult r
    LEFT JOIN dbo.State s ON r.StateID = s.StateCode
    LEFT JOIN dbo.Kiosk k ON r.KioskID = k.KioskID
    LEFT JOIN dbo.BranchLocation l ON k.BranchLocationID = l.BranchLocationID
WHERE r.ScreeningResultID = @ScreeningResultID
";
            CommandObject.Parameters.Clear();
            AddParameter("@ScreeningResultID", DbType.Int64).Value = screeningResultID;

            try
            {
                BeginTransaction();
                StartConnectionSharing();

                using (var reader = RunSelectQuery(sql))
                {
                    if (reader.Read())
                    {
                        result = new ScreeningResult(reader);

                    }
                }
                if (result != null)
                {
                    var sections = GetScreeningSectionResults(screeningResultID);
                    if (sections.Count > 0)
                    {
                        result.ImportSectionAnswerRange(sections);
                    }
                }


                StopConnectionSharing();
                CommitTransaction();
            }
            catch (Exception)
            {
                StopConnectionSharing();
                RollbackTransaction();

                throw;
            }
            finally
            {
                Disconnect();
            }
            return result;
        }

        internal List<ScreeningSectionResult> GetScreeningSectionResults(long screeningResultID)
        {
            List<ScreeningSectionResult> list = new List<ScreeningSectionResult>();
            string sql = @"
SELECT 
    s.ScreeningResultID
    ,s.ScreeningSectionID
    ,s.AnswerValue
    ,s.Score
    ,s.ScoreLevel
    ,l.Name as ScoreLevelLabel
    ,sec.QuestionText
    ,l.Indicates
FROM dbo.ScreeningSectionResult s
    INNER JOIN dbo.ScreeningSection sec ON s.ScreeningSectionID = sec.ScreeningSectionID
    LEFT JOIN dbo.ScreeningScoreLevel l ON s.ScreeningSectionID = l.ScreeningSectionID AND s.ScoreLevel = l.ScoreLevel 
WHERE s.ScreeningResultID = @ScreeningResultID
ORDER BY sec.OrderIndex ASC 
";
            CommandObject.Parameters.Clear();
            AddParameter("@ScreeningResultID", DbType.Int64).Value = screeningResultID;

            try
            {
                BeginTransaction();
                StartConnectionSharing();

                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        list.Add(new ScreeningSectionResult(reader));
                    }
                }

                foreach (var section in list)
                {
                    var questions = GetScreeningSectionQuestionResults(screeningResultID, section.ScreeningSectionID);
                    if (questions.Count > 0)
                    {
                        section.ImportQuestionAnswerRange(questions);
                    }
                }

                StopConnectionSharing();
                CommitTransaction();
            }
            catch (Exception)
            {
                StopConnectionSharing();
                RollbackTransaction();
                throw;
            }
            finally
            {
                Disconnect();
            }
            return list;
        }


        /// <summary>
        /// get answer results for section
        /// </summary>
        /// <param name="screeningResultID"></param>
        /// <param name="sectionID"></param>
        /// <returns></returns>
        internal List<ScreeningSectionQuestionResult> GetScreeningSectionQuestionResults(long screeningResultID, string sectionID)
        {
            List<ScreeningSectionQuestionResult> list = new List<ScreeningSectionQuestionResult>();
            string sql = @"
SELECT ScreeningResultID
      ,ScreeningSectionID
      ,QuestionID
      ,AnswerValue
FROM dbo.ScreeningSectionQuestionResult
WHERE ScreeningResultID = @ScreeningResultID AND ScreeningSectionID = @ScreeningSectionID
ORDER BY QuestionID
";
            CommandObject.Parameters.Clear();
            AddParameter("@ScreeningResultID", DbType.Int64).Value = screeningResultID;
            AddParameter("@ScreeningSectionID", DbType.AnsiString, 5).Value = sectionID;

            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        list.Add(new ScreeningSectionQuestionResult()
                        {
                            ScreeningResultID = reader.GetInt64(0),
                            ScreeningSectionID = reader.GetString(1),
                            QuestionID = reader.GetInt32(2),
                            AnswerValue = reader.GetInt32(3)
                        });
                    }
                }
            }
            finally
            {
                Disconnect();
            }
            return list;
        }
        #endregion

        #region For Administrative or management purposes
        //[AspNetHostingPermissionAttribute(SecurityAction.Deny, Level = AspNetHostingPermissionLevel.Minimal)]
        internal void UpdateScreeningSectionScore(ScreeningSectionResult section)
        {
            string sql = @"
UPDATE dbo.ScreeningSectionResult SET 
    Score = @Score,
    ScoreLevel = @ScoreLevel
WHERE ScreeningResultID = @ScreeningResultID AND ScreeningSectionID = @ScreeningSectionID
";
            CommandObject.Parameters.Clear();
            AddParameter("@ScreeningResultID", DbType.Int64).Value = section.ScreeningResultID;
            AddParameter("@ScreeningSectionID", DbType.AnsiString, 5).Value = section.ScreeningSectionID;

            try
            {
                RunNonSelectQuery(sql);
            }
            finally
            {
                Disconnect();
            }
        }

        #endregion

        /// <summary>
        /// GET minimum date for screening result
        /// </summary>
        internal DateTimeOffset? GetMinDate()
        {
            //DateTimeOffset minDate = DateTimeOffset.MinValue;                        
            DateTimeOffset? minDate = null;
            string sql = @"Select MIN(CreatedDate) FROM dbo.ScreeningResult WHERE IsDeleted = 0";
            try
            {
                base.Connect();
                using (var reader = RunSelectQuery(sql))
                {
                    if (reader.Read() && (!Convert.IsDBNull(reader[0])))
                    {
                        minDate = (DateTimeOffset)reader[0];
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                base.Disconnect();
            }
            return minDate;
        }


        #region Results by Problem

        /// <summary>
        /// Get screen result for check-in list
        /// </summary>
        internal DataSet GetUniquePatientCheckInsByProblem(ScreeningResultFilterModel filter, int startRowIndex, int maximumRows, string orderBy)
        {
            string innerOrderBy = string.Empty;
            string totalOrderBy = string.Empty;

            CommandObject.Parameters.Clear();

            if (string.IsNullOrEmpty(orderBy)) orderBy = "LastCheckinDate DESC"; // default sort order

            //map user field names to the query field names
            if (orderBy.Contains("LastCheckinDate"))
            {
                innerOrderBy = orderBy.Replace("LastCheckinDate", "MAX(r.CreatedDate)");
                totalOrderBy = orderBy;
            }
            else if (orderBy.Contains("FullName"))
            {
                innerOrderBy = orderBy.Replace("FullName", "r.PatientName");
                totalOrderBy = orderBy;
            }
            else if (orderBy.Contains("Birthday"))
            {
                innerOrderBy = orderBy.Replace("Birthday", "r.Birthday");
                totalOrderBy = orderBy;
            }


            StringBuilder qbUnionAllSql = new StringBuilder();

            ScreeningResultByProblemFilterModelToSqlHelper.TranslateToScreeningDetailsSql(filter.ProblemScoreFilter, qbUnionAllSql, CommandObject);



            QueryBuilder qbInnerSql = new QueryBuilder(@"
SELECT TOP(@startRowIndex + @maxRows) 
    ROW_NUMBER() OVER ( ORDER BY {0}) as RowNumber, 
    MAX(r.ScreeningResultID) as ScreeningResultID
    , r.PatientName as FullName
    , r.Birthday
    , MAX(r.CreatedDate) AS LastCheckinDate
FROM ({1}) r
".FormatWith(innerOrderBy, qbUnionAllSql.ToString()));

            string outerSql = @"
WITH tblCheckIns(RowNumber, ScreeningResultID, FullName, Birthday, LastCheckinDate)
AS ({0})
SELECT t.ScreeningResultID, FullName, Birthday, LastCheckinDate
FROM tblCheckIns t
WHERE RowNumber BETWEEN @startRowIndex AND (@startRowIndex + @maxRows)
ORDER BY {1} ";

            // -- GROUP BY FirstName, MiddleName, LastName, Birthday
            // -- ORDER BY LastCheckinDate DESC

            //parameters for pagination
            AddParameter("@startRowIndex", DbType.Int32).Value = startRowIndex + 1;
            AddParameter("@maxRows", DbType.Int32).Value = maximumRows;

            qbInnerSql.AppendGroupCondition("r.PatientName, r.Birthday");
            qbInnerSql.AppendOrderCondition(innerOrderBy);

            if (!string.IsNullOrEmpty(filter.FirstName))
            {
                qbInnerSql.AppendWhereCondition("(r.FirstName LIKE @FirstName)", ClauseType.And);
                AddParameter("@FirstName", DbType.String, 128).Value = SqlLikeStringPrepeare(filter.FirstName, LikeCondition.StartsWith);
            }
            if (!string.IsNullOrEmpty(filter.LastName))
            {
                qbInnerSql.AppendWhereCondition("(r.LastName LIKE @LastName)", ClauseType.And);
                AddParameter("@LastName", DbType.String, 128).Value = SqlLikeStringPrepeare(filter.LastName, LikeCondition.StartsWith);
            }
            if (filter.Location.HasValue)
            {
                // TODO: add join to kiosks table, choose kiosks by location
                qbInnerSql.AppendWhereCondition("r.BranchLocationID = @LocationID", ClauseType.And);

                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;
            }
            if (filter.StartDate.HasValue)
            {
                qbInnerSql.AppendWhereCondition("r.CreatedDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTimeOffset).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                qbInnerSql.AppendWhereCondition("r.CreatedDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTimeOffset).Value = filter.EndDate.Value;
            }




            try
            {
                Connect();
                var sql = string.Format(outerSql, qbInnerSql.ToString(), totalOrderBy);

                Logger.TraceFormat("GetUniquePatientCheckInsByProblem SQL:\r\n{0}", sql);

                DataSet ds = this.GetDataSet(sql);
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                this.Disconnect();
            }
        }


        internal int GetUniquePatientCheckInsByProblemCount(ScreeningResultFilterModel filter)
        {
            int rowCount = 0;

            string totalOrderBy = string.Empty;
            CommandObject.Parameters.Clear();

            StringBuilder qbUnionAllSql = new StringBuilder();

            ScreeningResultByProblemFilterModelToSqlHelper.TranslateToScreeningDetailsSql(filter.ProblemScoreFilter, qbUnionAllSql, CommandObject);



            QueryBuilder qbInnerSql = new QueryBuilder(@"
SELECT 
    MAX(r.ScreeningResultID) as ScreeningResultID
    , r.PatientName as FullName
    , r.Birthday
    , MAX(r.CreatedDate) AS LastCheckinDate
FROM ({0}) r
".FormatWith(qbUnionAllSql.ToString()));

            string outerSql = @"
WITH tblCheckIns(ScreeningResultID, FullName, Birthday, LastCheckinDate)
AS ({0})
SELECT count(*)
FROM tblCheckIns t
 ";

            // -- GROUP BY FirstName, MiddleName, LastName, Birthday
            qbInnerSql.AppendGroupCondition("r.PatientName, r.Birthday");

            if (!string.IsNullOrEmpty(filter.FirstName))
            {
                qbInnerSql.AppendWhereCondition("(r.FirstName LIKE @FirstName)", ClauseType.And);
                AddParameter("@FirstName", DbType.String, 128).Value = SqlLikeStringPrepeare(filter.FirstName, LikeCondition.StartsWith);
            }
            if (!string.IsNullOrEmpty(filter.LastName))
            {
                qbInnerSql.AppendWhereCondition("(r.LastName LIKE @LastName)", ClauseType.And);
                AddParameter("@LastName", DbType.String, 128).Value = SqlLikeStringPrepeare(filter.LastName, LikeCondition.StartsWith);
            }
            if (filter.Location.HasValue)
            {
                // TODO: add join to kiosks table, choose kiosks by location
                qbInnerSql.AppendWhereCondition("r.BranchLocationID = @LocationID", ClauseType.And);

                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;
            }
            if (filter.StartDate.HasValue)
            {
                qbInnerSql.AppendWhereCondition("r.CreatedDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTimeOffset).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                qbInnerSql.AppendWhereCondition("r.CreatedDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTimeOffset).Value = filter.EndDate.Value;
            }

            try
            {
                Connect();
                var sql = string.Format(outerSql, qbInnerSql.ToString());
                rowCount = Convert.ToInt32(this.RunScalarQuery(sql));

                Logger.TraceFormat("GetUniquePatientCheckInsByProblemCount SQL:\r\n{0}", sql);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Disconnect();
            }
            return rowCount;
        }


        /// <summary>
        /// Get positive screen results for Screening Results By Sort page
        /// </summary>
        public List<PatientCheckInViewModel> GetRelatedPatientScreeningsByProblemSort(long mainRowID, ScreeningResultFilterModel filter)
        {
            List<PatientCheckInViewModel> result = new List<PatientCheckInViewModel>();

            CommandObject.Parameters.Clear();

            StringBuilder qbUnionAllSql = new StringBuilder();
            ScreeningResultByProblemFilterModelToSqlHelper.TranslateToScreeningDetailsSql(filter.ProblemScoreFilter, qbUnionAllSql, CommandObject, linkRule: ClauseType.Or);

            string sql = @"
SELECT DISTINCT
r.ScreeningResultID
,r.CreatedDate
FROM 
(
{0}
) r
    INNER JOIN dbo.ScreeningResult main 
        ON (r.PatientName = main.PatientName AND r.Birthday = main.Birthday)
".FormatWith(qbUnionAllSql.ToString());


            QueryBuilder qb = new QueryBuilder(sql);

            qb.AppendOrderCondition("r.CreatedDate", OrderType.Desc);
            qb.AppendWhereCondition("main.ScreeningResultID = @ScreeningResultID", ClauseType.And);

            AddParameter("@ScreeningResultID", DbType.Int64).Value = mainRowID;

            if (filter.Location.HasValue)
            {
                qb.AppendWhereCondition("r.BranchLocationID = @LocationID", ClauseType.And);

                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;
            }
            if (filter.StartDate.HasValue)
            {
                qb.AppendWhereCondition("r.CreatedDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTimeOffset).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                qb.AppendWhereCondition("r.CreatedDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTimeOffset).Value = filter.EndDate.Value;
            }

            try
            {
                using (var reader = RunSelectQuery(qb.ToString()))
                {
                    while (reader.Read())
                    {
                        result.Add(new PatientCheckInViewModel
                        {
                            ScreeningResultID = reader.GetInt64(0),
                            CreatedDate = (DateTimeOffset)reader[1],
                            IsPositive = true,
                            HasAnyScreening = true
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

        public List<PatientCheckInDtoModel> GetPatientScreeningsBySort(ScreeningResultFilterModel filter)
        {

            List<PatientCheckInDtoModel> result = new List<PatientCheckInDtoModel>();

            CommandObject.Parameters.Clear();

            StringBuilder qbUnionAllSql = new StringBuilder();
            ScreeningResultByProblemFilterModelToSqlHelper.TranslateToScreeningDetailsSql(filter.ProblemScoreFilter, qbUnionAllSql, CommandObject, linkRule: ClauseType.Or);


            string sql = @"
SELECT DISTINCT
r.ScreeningResultID
,r.CreatedDate
,r.Birthday
,r.PatientName
FROM 
(
{0}
) r
    INNER JOIN dbo.ScreeningResult main 
        ON (r.PatientName = main.PatientName AND r.Birthday = main.Birthday)
".FormatWith(qbUnionAllSql.ToString());

            QueryBuilder qb = new QueryBuilder(sql);

            qb.AppendOrderCondition("r.CreatedDate", OrderType.Desc);


            if (filter.Location.HasValue)
            {
                qb.AppendWhereCondition("r.BranchLocationID = @LocationID", ClauseType.And);

                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;
            }
            if (filter.StartDate.HasValue)
            {
                qb.AppendWhereCondition("r.CreatedDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTimeOffset).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                qb.AppendWhereCondition("r.CreatedDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTimeOffset).Value = filter.EndDate.Value;
            }

            try
            {
                //ds = GetDataSet(qb.ToString());
                using (var reader = RunSelectQuery(qb.ToString()))
                {
                    while (reader.Read())
                    {
                        result.Add(new PatientCheckInDtoModel
                        {
                            ScreeningResultID = reader.GetInt64(0),
                            CreatedDate = (DateTimeOffset)reader[1],
                            IsPositive = true,
                            HasAnyScreening = true,
                            Birthday = reader.Get<DateTime>(2, true),
                            PatientName = reader.GetString(3)
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


        public ScreeningsByScoreLevelCountResult GetScreeningsCountByScoreLevel(SimpleFilterModel filter)
        {
            var result = new ScreeningsByScoreLevelCountResult();

            const string sqlSections = @"
SELECT 
    ssr.ScreeningSectionID,
    ssr.ScoreLevel,
    COUNT(*) as score_count
FROM ScreeningResult r 
    INNER JOIN dbo.ScreeningSectionResult ssr ON ssr.ScreeningResultID = r.ScreeningResultID 
";

            const string sqlTobacco = @"
 SELECT 
    'TCC',
    q.QuestionID,
    COUNT(*) as score_count
FROM ScreeningResult r 
    INNER JOIN dbo.ScreeningSectionResult ssr ON ssr.ScreeningResultID = r.ScreeningResultID
    INNER JOIN dbo.ScreeningSectionQuestionResult q ON ssr.ScreeningResultID = q.ScreeningResultID and ssr.ScreeningSectionID = q.ScreeningSectionID 
";

            const string sqlDepressionDeath = @"
 SELECT 
    'PHQ2',
    q.AnswerValue,
    COUNT(*) as score_count
FROM ScreeningResult r 
    INNER JOIN dbo.ScreeningSectionResult ssr ON ssr.ScreeningResultID = r.ScreeningResultID
    INNER JOIN dbo.ScreeningSectionQuestionResult q ON ssr.ScreeningResultID = q.ScreeningResultID and ssr.ScreeningSectionID = q.ScreeningSectionID 
";

            const string sqlDrugsOfChoice = @"
 SELECT 
    'DOCH',
    q.AnswerValue,
    COUNT(*) as score_count
FROM ScreeningResult r 
    INNER JOIN dbo.ScreeningSectionResult ssr ON ssr.ScreeningResultID = r.ScreeningResultID
    INNER JOIN dbo.ScreeningSectionQuestionResult q ON ssr.ScreeningResultID = q.ScreeningResultID and ssr.ScreeningSectionID = q.ScreeningSectionID 
";


            CommandObject.Parameters.Clear();

            var qbSections = new QueryBuilder(sqlSections);
            var qbTobacco = new QueryBuilder(sqlTobacco);
            var qbDepressionDeath = new QueryBuilder(sqlDepressionDeath);
            var qbDrugsOfChoice = new QueryBuilder(sqlDrugsOfChoice);


            //  group by q.ScreeningSectionID, q.AnswerValue

            qbSections.AppendGroupCondition("ssr.ScreeningSectionID, ssr.ScoreLevel");
            qbSections.AppendOrderCondition("ssr.ScreeningSectionID, ssr.ScoreLevel", OrderType.Desc);
            qbSections.AppendWhereCondition("r.IsDeleted = 0", ClauseType.And);
            qbSections.AppendWhereCondition("ssr.ScoreLevel > 0", ClauseType.And);

            //Tobacco
            qbTobacco.AppendWhereCondition("r.IsDeleted = 0", ClauseType.And);
            qbTobacco.AppendWhereCondition("q.AnswerValue > 0", ClauseType.And);
            qbTobacco.AppendWhereCondition("ssr.ScreeningSectionID = 'TCC'", ClauseType.And);


            qbTobacco.AppendGroupCondition("q.QuestionID");
            qbTobacco.AppendOrderCondition("q.QuestionID", OrderType.Asc);

            //Depression Thinking of Death

            qbDepressionDeath.AppendWhereCondition("r.IsDeleted = 0", ClauseType.And);
            qbDepressionDeath.AppendWhereCondition("ssr.ScreeningSectionID = @DepressionSectionID", ClauseType.And);
            qbDepressionDeath.AppendWhereCondition("q.QuestionID = @DepressionQuestionID", ClauseType.And);

            AddParameter("@DepressionSectionID", DbType.AnsiString, 5).Value = ScreeningSectionDescriptor.Depression;
            AddParameter("@DepressionQuestionID", DbType.Int32).Value = ScreeningSectionDescriptor.DepressionThinkOfDeathQuestionID;


            qbDepressionDeath.AppendGroupCondition("q.ScreeningSectionID, q.AnswerValue");
            qbDepressionDeath.AppendOrderCondition("q.AnswerValue", OrderType.Asc);

            //Drugs of Choice

            qbDrugsOfChoice.AppendWhereCondition("r.IsDeleted = 0", ClauseType.And);
            qbDrugsOfChoice.AppendWhereCondition("ssr.ScreeningSectionID = @DrugsOfChoiceSectionID", ClauseType.And);
            qbDrugsOfChoice.AppendWhereCondition("q.AnswerValue > 0", ClauseType.And);

            AddParameter("@DrugsOfChoiceSectionID", DbType.AnsiString, 5).Value = ScreeningSectionDescriptor.DrugOfChoice;

            qbDrugsOfChoice.AppendGroupCondition("q.ScreeningSectionID, q.AnswerValue");
            qbDrugsOfChoice.AppendOrderCondition("q.AnswerValue", OrderType.Asc);
            ///

            QueryBuilder[] queries = new[] { qbSections, qbTobacco, qbDepressionDeath, qbDrugsOfChoice };

            if (filter.Location.HasValue)
            {
                // add join to kiosks table, choose kiosks by location
                Array.ForEach(queries, qb =>
                {
                    qb.AppendJoinStatement(@" INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID ");
                    qb.AppendWhereCondition("k.BranchLocationID = @LocationID", ClauseType.And);
                });

                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;
            }
            if (filter.StartDate.HasValue)
            {
                Array.ForEach(queries, qb =>
                {
                    qb.AppendWhereCondition("r.CreatedDate >= @StartDate", ClauseType.And);
                });

                AddParameter("@StartDate", DbType.DateTimeOffset).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                Array.ForEach(queries, qb =>
                {
                    qb.AppendWhereCondition("r.CreatedDate < @EndDate", ClauseType.And);
                });

                AddParameter("@EndDate", DbType.DateTimeOffset).Value = filter.EndDate.Value;
            }

            try
            {

                using (var reader = RunSelectQuery(qbSections.ToString()))
                {
                    while (reader.Read())
                    {
                        result.Insert(
                            reader.GetString(0).TrimEnd(),
                            reader.GetInt32(1),
                            reader.GetInt32(2)
                        );
                    }

                }

                //tobacco
                using (var reader = RunSelectQuery(qbTobacco.ToString()))
                {
                    while (reader.Read())
                    {
                        result.Insert(
                            reader.GetString(0).TrimEnd() + reader.GetInt32(1).ToString(), //for UI binding
                            reader.GetInt32(1),
                            reader.GetInt32(2)
                        );
                    }

                }


                //depression thinking of death
                using (var reader = RunSelectQuery(qbDepressionDeath.ToString()))
                {
                    while (reader.Read())
                    {
                        result.Insert(
                            VisitSettingsDescriptor.DepressionThinkOfDeath, //for UI binding
                            reader.GetInt32(1),
                            reader.GetInt32(2)
                        );
                    }
                }

                //drugs of choice
                using (var reader = RunSelectQuery(qbDrugsOfChoice.ToString()))
                {
                    while (reader.Read())
                    {
                        result.Insert(
                            VisitSettingsDescriptor.DrugOfChoice, //for UI binding
                            reader.GetInt32(1),
                            reader.GetInt32(2)
                        );
                    }

                }
            }
            finally
            {
                Disconnect();
            }

            return result;
        }

        public long? GetLatestForKiosk(short kioskID, ScreeningPatientIdentity patient)
        {

            long? result = null;
            const string sql = @"
SELECT TOP(1) ScreeningResultID 
FROM dbo.ScreeningResult r
WHERE r.PatientName = dbo.fn_GetPatientName(@LastName, @FirstName, @MiddleName) 
AND r.Birthday = @Birthday
AND r.KioskID = @KioskID
ORDER BY r.CreatedDate DESC
";

            CommandObject.Parameters.Clear();
            ClearParameters();
            AddParameter("@FirstName", DbType.String, 128).Value = SqlParameterSafe(patient.FirstName);
            AddParameter("@LastName", DbType.String, 128).Value = SqlParameterSafe(patient.LastName);
            AddParameter("@MiddleName", DbType.String, 128).Value = SqlParameterSafe(patient.MiddleName);
            AddParameter("@Birthday", DbType.Date).Value = patient.Birthday;
            AddParameter("@KioskID", DbType.Int16).Value = kioskID;

            try
            {
                Connect();

                result = RunScalarQuery<long>(sql);
            }

            finally
            {
                Disconnect();
            }
            return result;
        }

    }

    #endregion
}
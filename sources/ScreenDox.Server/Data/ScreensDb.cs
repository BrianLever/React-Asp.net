using FrontDesk.Common.Data;
using FrontDesk.Common.Extensions;
using FrontDesk.Server.Screening;
using FrontDesk.Server.Screening.Models;

using ScreenDox.Server.Models;
using ScreenDox.Server.Models.ViewModels;

using System;
using System.Collections.Generic;
using System.Data;

namespace ScreenDox.Server.Data
{
    public interface IScreensRepository
    {
        List<UniquePatientScreenViewModel> GetLatestCheckinsForDisplay(PagedScreeningResultFilterModel filter);
        int GetLatestCheckinsForDisplayCount(ScreeningResultFilterModel filter);
        List<PatientCheckInViewModel> GetRelatedPatientScreenings(long mainRowID, ScreeningResultFilterModel filter);
        DateTimeOffset? GetMinDate();
        void Delete(long screeningResultID);
        List<PatientSearchInfoMatch> SearchPatient(PatientSearchFilter filter);
    }

    public class ScreensDb : DBDatabase, IScreensRepository
    {
        #region Constructors
        public ScreensDb() : base(0)
        {

        }


        #endregion

        /// <summary>
        /// Get screen result for check-in list
        /// </summary>
        public List<UniquePatientScreenViewModel> GetLatestCheckinsForDisplay(PagedScreeningResultFilterModel filter)
        {

            var result = new List<UniquePatientScreenViewModel>();

            var orderBy = (filter.OrderBy ?? "lastcheckindate").ToLowerInvariant();

            string innerOrderBy = string.Empty;
            string totalOrderBy = string.Empty;

            if (string.IsNullOrEmpty(orderBy)) orderBy = "lastcheckindate DESC"; // default sort order

            //map user field names to the query field names
            if (orderBy.Contains("lastcheckindate"))
            {
                innerOrderBy = orderBy.Replace("lastcheckindate", "MAX(r.CreatedDate)");
                totalOrderBy = orderBy;
            }
            else if (orderBy.Contains("fullname"))
            {
                innerOrderBy = string.Concat(orderBy.Replace("fullname", "r.PatientName"), ", MAX(r.CreatedDate) DESC");
                totalOrderBy = string.Concat(orderBy, ", lastcheckindate DESC");
            }
            else if (orderBy.Contains("birthday"))
            {
                innerOrderBy = orderBy.Replace("birthday", "r.Birthday");
                totalOrderBy = orderBy;
            }
            else if (orderBy.Contains("exportdate"))
            {
                innerOrderBy = orderBy.Replace("exportdate", "MAX(ExportDate)");
                if (orderBy.Contains("DESC"))
                {
                    totalOrderBy = orderBy.Replace("exportdate", "HasExport, LastCheckinDate DESC, ExportDate");
                }
                else
                {
                    totalOrderBy = orderBy.Replace("exportdate", "HasExport DESC, LastCheckinDate DESC, ExportDate");
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
    ,(Convert(bit,  COUNT(r.ScreeningResultID) - COUNT(r.ExportDate))) AS HasExport
    ,MAX(ExportDate)
FROM ScreeningResult r
", innerOrderBy));

            string outerSql = @"
WITH tblCheckIns(RowNumber, ScreeningResultID, FullName, Birthday, LastCheckinDate, HasExport, ExportDate)
AS ({0})
SELECT t.ScreeningResultID, FullName, Birthday, LastCheckinDate, HasExport, ExportDate
FROM tblCheckIns t
WHERE RowNumber BETWEEN @startRowIndex AND (@startRowIndex + @maxRows)
ORDER BY {1} ";


            CommandObject.Parameters.Clear();
            //parameters for pagination
            AddParameter("@startRowIndex", DbType.Int32).Value = filter.StartRowIndex + 1;
            AddParameter("@maxRows", DbType.Int32).Value = filter.MaximumRows - 1;

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

                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        result.Add(new UniquePatientScreenViewModel
                        {
                            //t.ScreeningResultID, FullName, Birthday, LastCheckinDate, HasExport, ExportDate
                            ScreeningResultID = reader.Get<long>(0), //ScreeningResultID
                            PatientName = reader.GetString(1), //FullName
                            Birthday = reader.Get<DateTime>(2), //Birthday
                            LastCheckinDate = reader.Get<DateTimeOffset>(3), // LastCheckinDate
                            HasExport = reader.Get<bool>(4), // HasExport
                            ExportDate = reader.Get<DateTimeOffset>(5) // ExportDate
                        });
                    }
                }

            }
            finally
            {
                this.Disconnect();
            }

            return result;
        }

        public int GetLatestCheckinsForDisplayCount(ScreeningResultFilterModel filter)
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
        public List<PatientCheckInViewModel> GetRelatedPatientScreenings(long mainRowID, ScreeningResultFilterModel filter)
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
        /// GET minimum date for screening result
        /// </summary>
        public DateTimeOffset? GetMinDate()
        {
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


        /// <summary>
        /// Delete screening result
        /// </summary>
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
        /// Get patient records for using in custom assessments and New Patient flow
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<PatientSearchInfoMatch> SearchPatient(PatientSearchFilter filter)
        {
            var result = new List<PatientSearchInfoMatch>();

            var sql = new QueryBuilder(@"
SELECT 
    r.ScreeningResultID,
    r.PatientName,
    r.Birthday,
    r.LastName,
    r.FirstName,
    r.MiddleName,
    r.ExportedToPatientId,
    r.ExportedToHRN,
    r.StreetAddress,
    r.City,
    r.StateID,
    r.StateName,
    r.ZipCode,
    r.DemographicsID
FROM dbo.vPatients r ");

            ClearParameters();
            sql.AppendOrderCondition("r.PatientName");
            sql.AppendOrderCondition("r.Birthday");


            // birthday
            sql.AppendWhereCondition("(r.Birthday = @Birthday)", ClauseType.And);
            AddParameter("@Birthday", DbType.Date).Value = filter.Birthday;

            if (!string.IsNullOrEmpty(filter.LastName))
            {
                sql.AppendWhereCondition("(r.LastName LIKE @LastName)", ClauseType.And);
                AddParameter("@LastName", DbType.String, 128).Value = SqlLikeStringPrepeare(filter.LastName, LikeCondition.StartsWith);
            }


            if (!string.IsNullOrEmpty(filter.FirstName))
            {
                sql.AppendWhereCondition("(r.FirstName LIKE @FirstName)", ClauseType.And);
                AddParameter("@FirstName", DbType.String, 128).Value = SqlLikeStringPrepeare(filter.FirstName, LikeCondition.StartsWith);
            }
            

            if (!string.IsNullOrEmpty(filter.MiddleName))
            {
                sql.AppendWhereCondition("(r.MiddleName LIKE @MiddleName)", ClauseType.And);
                AddParameter("@MiddleName", DbType.String, 128).Value = SqlLikeStringPrepeare(filter.MiddleName, LikeCondition.StartsWith);
            }
            

            try
            {
                Connect();
                
                Logger.DebugFormat("SearchPatient SQL:\r\n{0}", sql);

                using (var reader = RunSelectQuery(sql.ToString()))
                {
                    while (reader.Read())
                    {
                        result.Add(new PatientSearchInfoMatch
                        {
                            ScreeningResultID = reader.Get<long>("ScreeningResultID"),
                            FullName = reader.Get<string>("PatientName"),
                            Birthday = reader.Get<DateTime>("Birthday"),
                            LastName = reader.Get<string>("LastName"),
                            FirstName = reader.Get<string>("FirstName"),
                            MiddleName = reader.Get<string>("MiddleName"),
                            EhrPatientId = reader.Get<int>("ExportedToPatientId"),
                            StreetAddress = reader.Get<string>("StreetAddress"),
                            City = reader.Get<string>("City"),
                            StateID = reader.Get<string>("StateID"),
                            StateName = reader.Get<string>("StateName"),
                            ZipCode = reader.Get<string>("ZipCode"),
                            DemographicsID = reader.Get<long>("DemographicsID"),
                            IsEhrSource = false
                            
                        });
                    }
                }

            }
            finally
            {
                this.Disconnect();
            }

            return result;
        }
    }
}

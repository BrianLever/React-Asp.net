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
using FrontDesk.Server.Printouts.Bhs;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Common.Bhservice.Export;
using ScreenDox.Server.Models;

namespace FrontDesk.Server.Data.BhsVisits
{
    public interface IBhsFollowUpRepository : ITransactionalDatabase
    {
        long Add(BhsFollowUp model);
        bool UpdateManualEntries(BhsFollowUp model);

        void Delete(long BhsThirtyDayFollowUpID);
        BhsFollowUp Get(long id);
        DataSet GetUnique(BhsSearchFilterModel filter, int startRowIndex, int maximumRows, string orderBy);
        int GetUniqueCount(BhsSearchFilterModel filter);
        List<BhsFollowUpViewModel> GetRelatedReports(BhsSearchRelatedItemsFilter filter);
        long? FindByVisitId(long bhsVisitID);
        long? FindByParentFollowUpId(long parentFollowUpID);
        List<BhsFollowUpViewModel> GetFollowUpsForVisit(long visitId);
        List<BhsFollowUpListItemDtoModel> GetAllItems(BhsSearchFilterModel filter);
        List<BhsFollowUpExtendedWithIdentity> GetAllForExport(BhsSearchFilterModel filter);
        long GetLatestItemsForDisplayCount(PagedVisitFilterModel filter);
        List<UniqueFollowUpViewModel> GetLatestItemsForDisplay(PagedVisitFilterModel filter);
    }

    public class BhsFollowUpDb : DBDatabase, IBhsFollowUpRepository
    {

        #region Constructors
        public BhsFollowUpDb() : base(0) { }

        internal BhsFollowUpDb(DbConnection sharedConnection) : base(sharedConnection) { }

        #endregion

        public long Add(BhsFollowUp model)
        {
            string sql = @"
INSERT INTO [dbo].[BhsFollowUp](
[ScreeningResultID]
,[BhsVisitID]
,[ParentFollowUpID]
,[VisitDate]
,[FollowUpDate]
,[CreatedDate]
,[BhsStaffNameCompleted]
,[CompleteDate]
,[PatientAttendedVisitID]
,[FollowUpContactDate]
,[FollowUpContactOutcomeID]
,[NewVisitReferralRecommendationID]
,[NewVisitReferralRecommendationDescription]
,[NewVisitReferralRecommendationAcceptedID]
,[ReasonNewVisitReferralRecommendationNotAcceptedID]
,[NewVisitDate]
,[DischargedID]
,[ThirtyDatyFollowUpFlag]
,[NewFollowUpDate]
,[Notes]
)
VALUES(
@ScreeningResultID
,@BhsVisitID
,@ParentFollowUpID
,@VisitDate
,@FollowUpDate
,@CreatedDate
,@BhsStaffNameCompleted
,@CompleteDate
,@PatientAttendedVisitID
,@FollowUpContactDate
,@FollowUpContactOutcomeID
,@NewVisitReferralRecommendationID
,@NewVisitReferralRecommendationDescription
,@NewVisitReferralRecommendationAcceptedID
,@ReasonNewVisitReferralRecommendationNotAcceptedID
,@NewVisitDate
,@DischargedID
,@ThirtyDatyFollowUpFlag
,@NewFollowUpDate
,@Notes
);
SET @ID = SCOPE_IDENTITY();
";

            ClearParameters();

            AddParameter("@ScreeningResultID", DbType.Int64).Value = model.ScreeningResultID;
            AddParameter("@BhsVisitID", DbType.Int64).Value = model.BhsVisitID;
            AddParameter("@ParentFollowUpID", DbType.Int64).Value = SqlParameterSafe(model.ParentFollowUpID);
            AddParameter("@VisitDate", DbType.DateTimeOffset).Value = model.ScheduledVisitDate;
            AddParameter("@FollowUpDate", DbType.DateTimeOffset).Value = model.ScheduledFollowUpDate;

            AddParameter("@PatientAttendedVisitID", DbType.Int32).Value = SqlParameterSafe(model.PatientAttendedVisit?.Id);
            AddParameter("@FollowUpContactDate", DbType.DateTimeOffset).Value = SqlParameterSafe(model.FollowUpContactDate);
            AddParameter("@FollowUpContactOutcomeID", DbType.Int32).Value = SqlParameterSafe(model.FollowUpContactOutcome?.Id);

            AddParameter("@NewVisitReferralRecommendationID", DbType.Int32).Value = SqlParameterSafe(model.NewVisitReferralRecommendation?.Id);
            AddParameter("@NewVisitReferralRecommendationDescription", DbType.String, 64).Value = SqlParameterSafe(model.NewVisitReferralRecommendation?.Description);

            AddParameter("@NewVisitReferralRecommendationAcceptedID", DbType.Int32).Value = SqlParameterSafe(model.NewVisitReferralRecommendationAccepted?.Id);
            AddParameter("@ReasonNewVisitReferralRecommendationNotAcceptedID", DbType.Int32).Value = SqlParameterSafe(model.ReasonNewVisitReferralRecommendationNotAccepted?.Id);

            AddParameter("@NewVisitDate", DbType.DateTimeOffset).Value = SqlParameterSafe(model.NewVisitDate);
            AddParameter("@DischargedID", DbType.Int64).Value = SqlParameterSafe(model.Discharged?.Id);
            AddParameter("@ThirtyDatyFollowUpFlag", DbType.Boolean).Value = model.ThirtyDatyFollowUpFlag;
            AddParameter("@NewFollowUpDate", DbType.DateTimeOffset).Value = SqlParameterSafe(model.FollowUpDate);
            AddParameter("@Notes", DbType.String).Value = SqlParameterSafe(model.Notes);

            AddParameter("@CreatedDate", DbType.DateTimeOffset).Value = model.CreatedDate;
            AddParameter("@BhsStaffNameCompleted", DbType.String, 128).Value = SqlParameterSafe(model.BhsStaffNameCompleted);
            AddParameter("@CompleteDate", DbType.DateTimeOffset).Value = SqlParameterSafe(model.CompleteDate);

            var IdParam = AddParameter("@ID", DbType.Int64);
            IdParam.Direction = ParameterDirection.Output;

            try
            {
                base.Connect();

                RunNonSelectQuery(sql);

                model.ID = (long)IdParam.Value;

            }
            finally
            {
                base.Disconnect();
            }
            return model.ID;
        }


        public bool UpdateManualEntries(BhsFollowUp model)
        {
            string sql = @"
UPDATE [dbo].[BhsFollowUp]
SET
[PatientAttendedVisitID] = @PatientAttendedVisitID
,[FollowUpContactDate] = @FollowUpContactDate
,[FollowUpContactOutcomeID] = @FollowUpContactOutcomeID
,[NewVisitReferralRecommendationID] = @NewVisitReferralRecommendationID
,[NewVisitReferralRecommendationDescription] = @NewVisitReferralRecommendationDescription
,[NewVisitReferralRecommendationAcceptedID] = @NewVisitReferralRecommendationAcceptedID
,[ReasonNewVisitReferralRecommendationNotAcceptedID] = @ReasonNewVisitReferralRecommendationNotAcceptedID
,[NewVisitDate] = @NewVisitDate
,[DischargedID] = @DischargedID
,[ThirtyDatyFollowUpFlag] = @ThirtyDatyFollowUpFlag
,[NewFollowUpDate] = @NewFollowUpDate
,[Notes] = @Notes
,[BhsStaffNameCompleted] = @BhsStaffNameCompleted
,[CompleteDate] = @CompleteDate
WHERE ID = @ID
";

            ClearParameters();

            AddParameter("@ID", DbType.Int64).Value = model.ID;
            AddParameter("@BhsStaffNameCompleted", DbType.String, 128).Value = SqlParameterSafe(model.BhsStaffNameCompleted);
            AddParameter("@CompleteDate", DbType.DateTimeOffset).Value = SqlParameterSafe(model.CompleteDate);

            AddParameter("@PatientAttendedVisitID", DbType.Int32).Value = SqlParameterSafe(model.PatientAttendedVisit?.Id);
            AddParameter("@FollowUpContactDate", DbType.DateTimeOffset).Value = SqlParameterSafe(model.FollowUpContactDate);
            AddParameter("@FollowUpContactOutcomeID", DbType.Int32).Value = SqlParameterSafe(model.FollowUpContactOutcome?.Id);

            AddParameter("@NewVisitReferralRecommendationID", DbType.Int32).Value = SqlParameterSafe(model.NewVisitReferralRecommendation?.Id);
            AddParameter("@NewVisitReferralRecommendationDescription", DbType.String, 64).Value = SqlParameterSafe(model.NewVisitReferralRecommendation?.Description);
            AddParameter("@NewVisitReferralRecommendationAcceptedID", DbType.Int32).Value = SqlParameterSafe(model.NewVisitReferralRecommendationAccepted?.Id);
            AddParameter("@ReasonNewVisitReferralRecommendationNotAcceptedID", DbType.Int32).Value = SqlParameterSafe(model.ReasonNewVisitReferralRecommendationNotAccepted?.Id);
            AddParameter("@NewVisitDate", DbType.DateTimeOffset).Value = SqlParameterSafe(model.NewVisitDate);
            AddParameter("@DischargedID", DbType.Int64).Value = SqlParameterSafe(model.Discharged?.Id);
            AddParameter("@ThirtyDatyFollowUpFlag", DbType.Boolean).Value = model.ThirtyDatyFollowUpFlag;
            AddParameter("@NewFollowUpDate", DbType.DateTimeOffset).Value = SqlParameterSafe(model.FollowUpDate);
            AddParameter("@Notes", DbType.String).Value = SqlParameterSafe(model.Notes);


            try
            {
                base.Connect();


                return RunNonSelectQuery(sql) > 0;

            }
            finally
            {
                Disconnect();
            }
        }

        public void Delete(long id)
        {
            string sql = @"
delete from dbo.BhsFollowUp where ID=@ID

";
            ClearParameters();
            AddParameter("@ID", DbType.Int64).Value = id;

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

        public BhsFollowUp Get(long id)
        {
            const string sql = @"
SELECT v.[ID]
,v.[ScreeningResultID]
,v.[BhsVisitID]
,v.[ParentFollowUpID]
,v.[VisitDate]
,v.[FollowUpDate]
,v.[CreatedDate]
,v.[BhsStaffNameCompleted]
,v.[CompleteDate]
,v.[PatientAttendedVisitID]
,av.[Name] as PatientAttendedVisitName
,v.[FollowUpContactDate]
,v.[FollowUpContactOutcomeID]
,co.[Name] as FollowUpContactOutcomeName
,v.[NewVisitReferralRecommendationID]
,refRec.[Name] as NewVisitReferralRecommendationName
,v.[NewVisitReferralRecommendationDescription]
,v.[NewVisitReferralRecommendationAcceptedID]
,accept.[Name] as NewVisitReferralRecommendationAcceptedName
,v.[ReasonNewVisitReferralRecommendationNotAcceptedID]
,notaccept.Name as ReasonNewVisitReferralRecommendationNotAcceptedName
,v.[NewVisitDate]
,v.[DischargedID]
,discharge.Name as DischargedName
,v.[ThirtyDatyFollowUpFlag]
,v.[NewFollowUpDate]
,v.[Notes]
FROM [dbo].[BhsFollowUp] v
    LEFT JOIN dbo.PatientAttendedVisit av ON v.PatientAttendedVisitID = av.ID
    LEFT JOIN dbo.FollowUpContactOutcome co ON v.FollowUpContactOutcomeID = co.ID
    LEFT JOIN dbo.NewVisitReferralRecommendation refRec ON v.NewVisitReferralRecommendationID = refRec.ID
    LEFT JOIN dbo.NewVisitReferralRecommendationAccepted accept ON v.[NewVisitReferralRecommendationAcceptedID] = accept.ID
    LEFT JOIN dbo.ReasonNewVisitReferralRecommendationNotAccepted notaccept ON v.ReasonNewVisitReferralRecommendationNotAcceptedID = notaccept.ID
    LEFT JOIN dbo.Discharged discharge ON v.[DischargedID] = discharge.ID
WHERE v.ID = @ID
";
            ClearParameters();

            AddParameter("@ID", DbType.Int64).Value = id;


            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    if (reader.Read())
                    {
                        return new BhsFollowUpFactory().InitFromReader(reader);
                    }
                }
            }
            finally
            {
                Disconnect();
            }
            return null;
        }

        //TODO: Change
        public DataSet GetUnique(BhsSearchFilterModel filter, int startRowIndex, int maximumRows, string orderBy)
        {

            string innerOrderBy = string.Empty;
            string totalOrderBy = string.Empty;

            if (string.IsNullOrEmpty(orderBy)) orderBy = "LastFollowUpDate DESC"; // default sort order

            //map user field names to the query field names
            if (orderBy.Contains("LastFollowUpDate"))
            {
                innerOrderBy = orderBy.Replace("LastFollowUpDate", "MAX(f.FollowUpDate)");
                totalOrderBy = orderBy;
            }
            else if (orderBy.Contains("LastVisitDate"))
            {
                innerOrderBy = orderBy.Replace("LastVisitDate", "MAX(f.VisitDate)");
                totalOrderBy = orderBy;
            }
            else if (orderBy.Contains("FullName"))
            {
                innerOrderBy = orderBy.Replace("FullName", "dbo.fn_GetPatientName(r.LastName, r.FirstName, r.MiddleName)");
                totalOrderBy = orderBy;
            }
            else if (orderBy.Contains("LastCompleteDate"))
            {
                innerOrderBy = orderBy.Replace("LastCompleteDate", "MAX(f.CompleteDate)");
                totalOrderBy = orderBy;
            }
            else if (orderBy.Contains("Birthday"))
            {
                innerOrderBy = orderBy.Replace("Birthday", "r.Birthday");
                totalOrderBy = orderBy;
            }

            QueryBuilder qbInnerSql = new QueryBuilder(string.Format(@"
SELECT TOP(@startRowIndex + @maxRows) 
ROW_NUMBER() OVER ( ORDER BY {0}) as RowNumber, 
MAX(r.ScreeningResultID) as ScreeningResultID
,dbo.fn_GetPatientName(r.LastName, r.FirstName, r.MiddleName) as FullName
,r.Birthday
,MAX(f.VisitDate) AS LastVisitDate
,MAX(f.CompleteDate) AS LastCompleteDate
,MAX(f.FollowUpDate) AS LastFollowUpDate
FROM ScreeningResult r
    INNER JOIN dbo.BhsFollowUp f ON r.ScreeningResultID = f.ScreeningResultID
", innerOrderBy));

            string outerSql = @"
WITH tblCheckIns(RowNumber, ScreeningResultID, FullName, Birthday, LastVisitDate, LastCompleteDate, LastFollowUpDate)
AS ({0})
SELECT t.ScreeningResultID, FullName, Birthday, LastVisitDate, LastCompleteDate, LastFollowUpDate
FROM tblCheckIns t
WHERE RowNumber BETWEEN @startRowIndex AND (@startRowIndex + @maxRows)
ORDER BY {1} ";


            CommandObject.Parameters.Clear();
            //parameters for pagination
            AddParameter("@startRowIndex", DbType.Int32).Value = startRowIndex + 1;
            AddParameter("@maxRows", DbType.Int32).Value = maximumRows;

            qbInnerSql.AppendGroupCondition("r.FirstName, r.MiddleName, r.LastName, r.Birthday");
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
            if (filter.LocationId.HasValue)
            {
                qbInnerSql.AppendJoinStatement(" INNER JOIN dbo.BhsVisit v ON v.ID = f.BhsVisitID ");
                qbInnerSql.AppendWhereCondition("v.LocationID = @LocationID", ClauseType.And);

                AddParameter("@LocationID", DbType.Int32).Value = filter.LocationId.Value;
            }
            if (filter.StartDate.HasValue)
            {
                qbInnerSql.AppendWhereCondition("f.FollowUpDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTimeOffset).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                qbInnerSql.AppendWhereCondition("f.FollowUpDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTimeOffset).Value = filter.EndDate.Value;
            }

            if (filter.ReportType != BhsReportType.AllReports)
            {
                if (filter.ReportType == BhsReportType.CompletedReports)
                {
                    qbInnerSql.AppendWhereCondition("(f.CompleteDate IS NOT NULL)", ClauseType.And);
                }
                else if (filter.ReportType == BhsReportType.IncompleteReports)
                {
                    qbInnerSql.AppendWhereCondition("(f.CompleteDate IS NULL)", ClauseType.And);
                }
            }
            try
            {
                Connect();
                DataSet ds = this.GetDataSet(string.Format(outerSql, qbInnerSql.ToString(), totalOrderBy));
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

        public int GetUniqueCount(BhsSearchFilterModel filter)
        {
            int rowCount = 0;
            //WITH statement
            QueryBuilder qbInnerSql = new QueryBuilder(@"
SELECT 
MAX(r.ScreeningResultID) as ScreeningResultID
,dbo.fn_GetPatientName(r.LastName, r.FirstName, r.MiddleName) as FullName
,r.Birthday
,MAX(f.VisitDate) AS LastVisitDate
,MAX(f.CompleteDate) AS LastCompleteDate
,MAX(f.FollowUpDate) AS LastFollowUpDate
FROM ScreeningResult r
    INNER JOIN dbo.BhsFollowUp f ON r.ScreeningResultID = f.ScreeningResultID
");

            string outerSql = @"
WITH tblCheckIns(ScreeningResultID, FullName, Birthday, LastVisitDate, LastCompleteDate, LastFollowUpDate)
AS ({0})
SELECT count(*)
FROM tblCheckIns t
";

            CommandObject.Parameters.Clear();

            qbInnerSql.AppendGroupCondition("r.FirstName, r.MiddleName, r.LastName, r.Birthday");

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
            if (filter.LocationId.HasValue)
            {
                qbInnerSql.AppendJoinStatement(" INNER JOIN dbo.BhsVisit v ON v.ID = f.BhsVisitID ");
                qbInnerSql.AppendWhereCondition("v.LocationID = @LocationID", ClauseType.And);

                AddParameter("@LocationID", DbType.Int32).Value = filter.LocationId.Value;
            }
            if (filter.StartDate.HasValue)
            {
                qbInnerSql.AppendWhereCondition("f.FollowUpDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTimeOffset).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                qbInnerSql.AppendWhereCondition("f.FollowUpDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTimeOffset).Value = filter.EndDate.Value;
            }

            if (filter.ReportType != BhsReportType.AllReports)
            {
                if (filter.ReportType == BhsReportType.CompletedReports)
                {
                    qbInnerSql.AppendWhereCondition("(f.CompleteDate IS NOT NULL)", ClauseType.And);
                }
                else if (filter.ReportType == BhsReportType.IncompleteReports)
                {
                    qbInnerSql.AppendWhereCondition("(f.CompleteDate IS NULL)", ClauseType.And);
                }
            }


            try
            {
                this.Connect();

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


        public List<BhsFollowUpViewModel> GetRelatedReports(BhsSearchRelatedItemsFilter filter)
        {
            List<BhsFollowUpViewModel> result = new List<BhsFollowUpViewModel>();

            string sql = @"
SELECT f.ID, f.ScreeningResultID, f.CreatedDate,f.VisitDate, f.CompleteDate, f.FollowUpDate
FROM dbo.BhsFollowUp f
INNER JOIN dbo.ScreeningResult r  
    ON r.ScreeningResultID = f.ScreeningResultID
INNER JOIN dbo.ScreeningResult main 
    ON main.ScreeningResultID = f.ScreeningResultID 
            OR (dbo.fn_GetPatientName(main.LastName, main.FirstName, main.MiddleName) 
                = dbo.fn_GetPatientName(r.LastName, r.FirstName, r.MiddleName)
            AND main.Birthday = r.Birthday)
";
            CommandObject.Parameters.Clear();

            QueryBuilder qb = new QueryBuilder(sql);

            qb.AppendOrderCondition("f.VisitDate", OrderType.Desc);
            qb.AppendOrderCondition("f.CreatedDate", OrderType.Desc);

            qb.AppendWhereCondition("main.ScreeningResultID = @ScreeningResultID", ClauseType.And);

            AddParameter("@ScreeningResultID", DbType.Int64).Value = filter.MainRowID;

            if (filter.ScreeningResultID.HasValue)
            {
                qb.AppendWhereCondition("(r.ScreeningResultID = @ScreeningResultID)", ClauseType.And);
            }

            if (filter.LocationId.HasValue)
            {
                qb.AppendJoinStatement(" INNER JOIN dbo.BhsVisit v ON v.ID = f.BhsVisitID ");
                qb.AppendWhereCondition("v.LocationID = @LocationID", ClauseType.And);

                AddParameter("@LocationID", DbType.Int32).Value = filter.LocationId.Value;
            }
            if (filter.StartDate.HasValue)
            {
                qb.AppendWhereCondition("f.VisitDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTimeOffset).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                qb.AppendWhereCondition("f.VisitDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTimeOffset).Value = filter.EndDate.Value;
            }
            if (filter.ReportType != BhsReportType.AllReports)
            {
                if (filter.ReportType == BhsReportType.CompletedReports)
                {
                    qb.AppendWhereCondition("f.CompleteDate IS NOT NULL", ClauseType.And);
                }
                else if (filter.ReportType == BhsReportType.IncompleteReports)
                {
                    qb.AppendWhereCondition("f.CompleteDate IS NULL", ClauseType.And);
                }
            }


            try
            {
                using (var reader = RunSelectQuery(qb.ToString()))
                {
                    while (reader.Read())
                    {
                        result.Add(new BhsFollowUpViewModel
                        {
                            ID = reader.GetInt64(0),
                            ScreeningResultID = reader.GetInt64(1),
                            CreatedDate = reader.Get<DateTimeOffset>(2).Value,
                            VisitDate = reader.Get<DateTimeOffset>(3).Value,
                            CompletedDate = reader.GetNullable<DateTimeOffset>(4),
                            FollowUpDate = reader.Get<DateTimeOffset>(5).Value,
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

        public long? FindByVisitId(long bhsVisitID)
        {
            const string sql = @"
SELECT TOP 1 ID FROM dbo.BhsFollowUp WHERE BhsVisitID = @BhsVisitID
";
            ClearParameters();
            AddParameter("@BhsVisitID", DbType.Int64).Value = bhsVisitID;

            try
            {
                return RunScalarQuery<long>(sql);
            }
            finally
            {
                Disconnect();
            }
        }

        public long? FindByParentFollowUpId(long parentFollowUpID)
        {
            const string sql = @"
SELECT TOP 1 ID FROM dbo.BhsFollowUp WHERE ParentFollowUpID = @ID
";
            ClearParameters();
            AddParameter("@ID", DbType.Int64).Value = parentFollowUpID;

            try
            {
                return RunScalarQuery<long>(sql);
            }
            finally
            {
                Disconnect();
            }
        }

        public List<BhsFollowUpViewModel> GetFollowUpsForVisit(long visitId)
        {
            List<BhsFollowUpViewModel> result = new List<BhsFollowUpViewModel>();

            const string sql = @"
SELECT f.ID, f.ScreeningResultID, f.CreatedDate,f.VisitDate, f.CompleteDate, f.FollowUpDate
FROM dbo.BhsFollowUp f
WHERE f.BhsVisitID = @VisitID
ORDER BY f.CreatedDate DESC
";
            ClearParameters();
            AddParameter("@VisitID", DbType.Int64).Value = visitId;
            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        result.Add(new BhsFollowUpViewModel
                        {
                            ID = reader.GetInt64(0),
                            ScreeningResultID = reader.GetInt64(1),
                            CreatedDate = reader.Get<DateTimeOffset>(2).Value,
                            VisitDate = reader.Get<DateTimeOffset>(3).Value,
                            CompletedDate = reader.GetNullable<DateTimeOffset>(4),
                            FollowUpDate = reader.Get<DateTimeOffset>(5).Value
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


        public List<BhsFollowUpListItemDtoModel> GetAllItems(BhsSearchFilterModel filter)
        {
            List<BhsFollowUpListItemDtoModel> result = new List<BhsFollowUpListItemDtoModel>();

            string sql = @"
SELECT f.ID, f.ScreeningResultID, f.CreatedDate,f.VisitDate, f.CompleteDate, f.FollowUpDate,
     r.Birthday, r.PatientName
FROM dbo.BhsFollowUp f
INNER JOIN dbo.ScreeningResult r  
    ON r.ScreeningResultID = f.ScreeningResultID
";
            CommandObject.Parameters.Clear();

            QueryBuilder qb = new QueryBuilder(sql);

            qb.AppendOrderCondition("f.FollowUpDate", OrderType.Desc);
            qb.AppendOrderCondition("f.VisitDate", OrderType.Desc);
            qb.AppendOrderCondition("f.CreatedDate", OrderType.Desc);

            if (filter.ScreeningResultID.HasValue)
            {
                qb.AppendWhereCondition("(r.ScreeningResultID = @ScreeningResultID)", ClauseType.And);
                AddParameter("@ScreeningResultID", DbType.Int64).Value = filter.ScreeningResultID.Value;
            }

            if (!string.IsNullOrEmpty(filter.FirstName))
            {
                qb.AppendWhereCondition("(r.FirstName LIKE @FirstName)", ClauseType.And);
                AddParameter("@FirstName", DbType.String, 128).Value = SqlLikeStringPrepeare(filter.FirstName, LikeCondition.StartsWith);
            }
            if (!string.IsNullOrEmpty(filter.LastName))
            {
                qb.AppendWhereCondition("(r.LastName LIKE @LastName)", ClauseType.And);
                AddParameter("@LastName", DbType.String, 128).Value = SqlLikeStringPrepeare(filter.LastName, LikeCondition.StartsWith);
            }

            if (filter.LocationId.HasValue)
            {
                qb.AppendJoinStatement(" INNER JOIN dbo.BhsVisit v ON v.ID = f.BhsVisitID ");
                qb.AppendWhereCondition("v.LocationID = @LocationID", ClauseType.And);

                AddParameter("@LocationID", DbType.Int32).Value = filter.LocationId.Value;
            }
            if (filter.StartDate.HasValue)
            {
                qb.AppendWhereCondition("f.VisitDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTimeOffset).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                qb.AppendWhereCondition("f.VisitDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTimeOffset).Value = filter.EndDate.Value;
            }
            if (filter.ReportType != BhsReportType.AllReports)
            {
                if (filter.ReportType == BhsReportType.CompletedReports)
                {
                    qb.AppendWhereCondition("f.CompleteDate IS NOT NULL", ClauseType.And);
                }
                else if (filter.ReportType == BhsReportType.IncompleteReports)
                {
                    qb.AppendWhereCondition("f.CompleteDate IS NULL", ClauseType.And);
                }
            }


            try
            {
                using (var reader = RunSelectQuery(qb.ToString()))
                {
                    while (reader.Read())
                    {
                        result.Add(new BhsFollowUpListItemDtoModel
                        {
                            ID = reader.GetInt64(0),
                            ScreeningResultID = reader.GetInt64(1),
                            CreatedDate = reader.Get<DateTimeOffset>(2).Value,
                            VisitDate = reader.Get<DateTimeOffset>(3).Value,
                            CompletedDate = reader.GetNullable<DateTimeOffset>(4),
                            FollowUpDate = reader.Get<DateTimeOffset>(5).Value,
                            Birthday = reader.Get<DateTime>(6, true),
                            PatientName = reader.Get<string>(7, true)
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

        public List<BhsFollowUpExtendedWithIdentity> GetAllForExport(BhsSearchFilterModel filter)
        {
            var result = new List<BhsFollowUpExtendedWithIdentity>();

            string sql = @"SELECT * FROM dbo.vBhsFollowUpForExport t ";
            CommandObject.Parameters.Clear();

            QueryBuilder qb = new QueryBuilder(sql);

            qb.AppendOrderCondition("t.CreatedDate", OrderType.Desc);

            if (filter.ScreeningResultID.HasValue)
            {
                qb.AppendWhereCondition("(t.ScreeningResultID = @ScreeningResultID)", ClauseType.And);
                AddParameter("@ScreeningResultID", DbType.Int64).Value = filter.ScreeningResultID.Value;
            }

            if (!string.IsNullOrEmpty(filter.FirstName))
            {
                qb.AppendWhereCondition("(t.FirstName LIKE @FirstName)", ClauseType.And);
                AddParameter("@FirstName", DbType.String, 128).Value = SqlLikeStringPrepeare(filter.FirstName, LikeCondition.StartsWith);
            }
            if (!string.IsNullOrEmpty(filter.LastName))
            {
                qb.AppendWhereCondition("(t.LastName LIKE @LastName)", ClauseType.And);
                AddParameter("@LastName", DbType.String, 128).Value = SqlLikeStringPrepeare(filter.LastName, LikeCondition.StartsWith);
            }
            if (filter.LocationId.HasValue)
            {
                qb.AppendWhereCondition("t.LocationID = @LocationID", ClauseType.And);

                AddParameter("@LocationID", DbType.Int32).Value = filter.LocationId.Value;
            }
            if (filter.StartDate.HasValue)
            {
                qb.AppendWhereCondition("t.ScreeningDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTimeOffset).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                qb.AppendWhereCondition("t.ScreeningDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTimeOffset).Value = filter.EndDate.Value;
            }

            if (filter.ReportType != BhsReportType.AllReports)
            {
                if (filter.ReportType == BhsReportType.CompletedReports)
                {
                    qb.AppendWhereCondition("t.CompleteDate IS NOT NULL", ClauseType.And);
                }
                else if (filter.ReportType == BhsReportType.IncompleteReports)
                {
                    qb.AppendWhereCondition("t.CompleteDate IS NULL", ClauseType.And);
                }
            }


            try
            {
                using (var reader = RunSelectQuery(qb.ToString()))
                {
                    while (reader.Read())
                    {
                        result.Add(new BhsFollowUpFactory().InitExtendedModelFromReader(reader));
                    }

                }
            }
            finally
            {
                Disconnect();
            }
            return result;
        }

        public long GetLatestItemsForDisplayCount(PagedVisitFilterModel filter)
        {
            long rowCount = 0;

            //WITH statement
            QueryBuilder qbInnerSql = new QueryBuilder(@"
SELECT 
MAX(r.ScreeningResultID) as ScreeningResultID
,dbo.fn_GetPatientName(r.LastName, r.FirstName, r.MiddleName) as FullName
,r.Birthday
,MAX(f.VisitDate) AS LastVisitDate
,MAX(f.CompleteDate) AS LastCompleteDate
,MAX(f.FollowUpDate) AS LastFollowUpDate
FROM ScreeningResult r
    INNER JOIN dbo.BhsFollowUp f ON r.ScreeningResultID = f.ScreeningResultID
");

            string outerSql = @"
WITH tblCheckIns(ScreeningResultID, FullName, Birthday, LastVisitDate, LastCompleteDate, LastFollowUpDate)
AS ({0})
SELECT count(*)
FROM tblCheckIns t
";

            CommandObject.Parameters.Clear();

            qbInnerSql.AppendGroupCondition("r.FirstName, r.MiddleName, r.LastName, r.Birthday");

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
                qbInnerSql.AppendJoinStatement(" INNER JOIN dbo.BhsVisit v ON v.ID = f.BhsVisitID ");
                qbInnerSql.AppendWhereCondition("v.LocationID = @LocationID", ClauseType.And);

                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;
            }
            if (filter.StartDate.HasValue)
            {
                qbInnerSql.AppendWhereCondition("f.FollowUpDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTimeOffset).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                qbInnerSql.AppendWhereCondition("f.FollowUpDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTimeOffset).Value = filter.EndDate.Value;
            }

            if (filter.ReportType != BhsReportType.AllReports)
            {
                if (filter.ReportType == BhsReportType.CompletedReports)
                {
                    qbInnerSql.AppendWhereCondition("(f.CompleteDate IS NOT NULL)", ClauseType.And);
                }
                else if (filter.ReportType == BhsReportType.IncompleteReports)
                {
                    qbInnerSql.AppendWhereCondition("(f.CompleteDate IS NULL)", ClauseType.And);
                }
            }

            try
            {
                Connect();

                var sql = string.Format(outerSql, qbInnerSql.ToString());
                rowCount = this.RunScalarQuery<long>(sql) ?? 0;
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

        public List<UniqueFollowUpViewModel> GetLatestItemsForDisplay(PagedVisitFilterModel filter)
        {
            List<UniqueFollowUpViewModel> result = new List<UniqueFollowUpViewModel>();

            string innerOrderBy = string.Empty;
            string totalOrderBy = string.Empty;

            // default sort order
            var orderBy = (filter.OrderBy ?? "LASTFOLLOWUPDATE DESC").ToUpperInvariant();


            //map user field names to the query field names
            if (orderBy.Contains("LASTFOLLOWUPDATE"))
            {
                innerOrderBy = orderBy.Replace("LASTFOLLOWUPDATE", "MAX(f.FollowUpDate)");
                totalOrderBy = orderBy;
            }
            else if (orderBy.Contains("LASTVISITDATE"))
            {
                innerOrderBy = orderBy.Replace("LASTVISITDATE", "MAX(f.VisitDate)");
                totalOrderBy = orderBy;
            }
            else if (orderBy.Contains("FULLNAME"))
            {
                innerOrderBy = orderBy.Replace("FULLNAME", "dbo.fn_GetPatientName(r.LastName, r.FirstName, r.MiddleName)");
                totalOrderBy = orderBy;
            }
            else if (orderBy.Contains("LASTCOMPLETEDATE"))
            {
                innerOrderBy = orderBy.Replace("LASTCOMPLETEDATE", "MAX(f.CompleteDate)");
                totalOrderBy = orderBy;
            }
            else if (orderBy.Contains("BIRTHDAY"))
            {
                innerOrderBy = orderBy.Replace("BIRTHDAY", "r.Birthday");
                totalOrderBy = orderBy;
            }

            QueryBuilder qbInnerSql = new QueryBuilder(string.Format(@"
SELECT TOP(@startRowIndex + @maxRows) 
ROW_NUMBER() OVER ( ORDER BY {0}) as RowNumber, 
MAX(r.ScreeningResultID) as ScreeningResultID
,dbo.fn_GetPatientName(r.LastName, r.FirstName, r.MiddleName) as FullName
,r.Birthday
,MAX(f.VisitDate) AS LastVisitDate
,MAX(f.CompleteDate) AS LastCompleteDate
,MAX(f.FollowUpDate) AS LastFollowUpDate
FROM ScreeningResult r
    INNER JOIN dbo.BhsFollowUp f ON r.ScreeningResultID = f.ScreeningResultID
", innerOrderBy));

            string outerSql = @"
WITH tblCheckIns(RowNumber, ScreeningResultID, FullName, Birthday, LastVisitDate, LastCompleteDate, LastFollowUpDate)
AS ({0})
SELECT t.ScreeningResultID, FullName, Birthday, LastVisitDate, LastCompleteDate, LastFollowUpDate
FROM tblCheckIns t
WHERE RowNumber BETWEEN @startRowIndex AND (@startRowIndex + @maxRows)
ORDER BY {1} ";


            CommandObject.Parameters.Clear();
            //parameters for pagination
            AddParameter("@startRowIndex", DbType.Int32).Value = filter.StartRowIndex + 1;
            AddParameter("@maxRows", DbType.Int32).Value = filter.MaximumRows - 1;

            qbInnerSql.AppendGroupCondition("r.FirstName, r.MiddleName, r.LastName, r.Birthday");
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
                qbInnerSql.AppendJoinStatement(" INNER JOIN dbo.BhsVisit v ON v.ID = f.BhsVisitID ");
                qbInnerSql.AppendWhereCondition("v.LocationID = @LocationID", ClauseType.And);

                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;
            }
            if (filter.StartDate.HasValue)
            {
                qbInnerSql.AppendWhereCondition("f.FollowUpDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTimeOffset).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                qbInnerSql.AppendWhereCondition("f.FollowUpDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTimeOffset).Value = filter.EndDate.Value;
            }

            if (filter.ReportType != BhsReportType.AllReports)
            {
                if (filter.ReportType == BhsReportType.CompletedReports)
                {
                    qbInnerSql.AppendWhereCondition("(f.CompleteDate IS NOT NULL)", ClauseType.And);
                }
                else if (filter.ReportType == BhsReportType.IncompleteReports)
                {
                    qbInnerSql.AppendWhereCondition("(f.CompleteDate IS NULL)", ClauseType.And);
                }
            }
            try
            {
                Connect();

                var sql = string.Format(outerSql, qbInnerSql.ToString(), totalOrderBy);


                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        result.Add(new UniqueFollowUpViewModel
                        {
                            // t.ScreeningResultID, FullName, Birthday, LastVisitDate, LastCompleteDate, LastFollowUpDate
                            ScreeningResultID = reader.Get<long>(0), //ScreeningResultID
                            PatientName = reader.GetString(1), //FullName
                            Birthday = reader.Get<DateTime>(2), //Birthday
                            LastVisitDate = reader.Get<DateTimeOffset>(3), // LastVisitDate
                            LastCompleteDate = reader.Get<DateTimeOffset>(4), // LastCompleteDate
                            LastFollowUpDate = reader.Get<DateTimeOffset>(5) // LastFollowUpDate

                        });
                    }
                }
                return result;
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
    }
}

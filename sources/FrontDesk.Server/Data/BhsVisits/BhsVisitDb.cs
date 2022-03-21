using FrontDesk.Common.Bhservice;
using FrontDesk.Common.Bhservice.Export;
using FrontDesk.Common.Data;
using FrontDesk.Common.Extensions;
using FrontDesk.Server.Printouts.Bhs;
using FrontDesk.Server.Screening;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Screening.Services;
using FrontDesk.Services;

using ScreenDox.Server.Models;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace FrontDesk.Server.Data.BhsVisits
{
    public interface IBhsVisitRepository : ITransactionalDatabase
    {
        long Add(BhsVisit model);
        bool UpdateManualEntries(BhsVisit model);

        void Delete(long bhsVisitID);
        BhsVisit Get(long id);
        long? FindByScreeningResultId(long screeningResultId);
        DataSet GetUniqueVisits(BhsSearchFilterModel filter, int startRowIndex, int maximumRows, string orderBy);
        int GetUniqueVisitsCount(BhsSearchFilterModel filter);
        List<BhsVisitViewModel> GetRelatedReports(BhsSearchRelatedItemsFilter filter);
        void SetPatientAddressToAllPatientScreenings(ScreeningResult patient);
        List<BhsVisitListItemDtoModel> GetAllItems(BhsSearchFilterModel filter);
        List<BhsVisitExtendedWithIdentity> GetAllForExport(BhsSearchFilterModel filter);

        void UpdateDrugOfChoice(long screeningResultId, DrugOfChoiceModel drugOfChoice);
        
        List<UniqueVisitViewModel> GetLatestVisitsForDisplay(PagedVisitFilterModel filter);
        int GetLatestVisitsForDisplayCount(PagedVisitFilterModel filter);
    }

    public class BhsVisitDb : DBDatabase, IBhsVisitRepository
    {

        #region Constructors
        public BhsVisitDb() : base(0) { }

        internal BhsVisitDb(DbConnection sharedConnection) : base(sharedConnection) { }

        #endregion

        public long Add(BhsVisit model)
        {
            string sql = @"
INSERT INTO [dbo].[BhsVisit]
([ScreeningResultID]
,[LocationID]
,[CreatedDate]
,[ScreeningDate]
,[TobacoExposureSmokerInHomeFlag]
,[TobacoExposureCeremonyUseFlag]
,[TobacoExposureSmokingFlag]
,[TobacoExposureSmoklessFlag]
,[AlcoholUseFlagScoreLevel]
,[AlcoholUseFlagScoreLevelLabel]
,[SubstanceAbuseFlagScoreLevel]
,[SubstanceAbuseFlagScoreLevelLabel]
,[DepressionFlagScoreLevel]
,[DepressionFlagScoreLevelLabel]
,[DepressionThinkOfDeathAnswer]
,[PartnerViolenceFlagScoreLevel]
,[PartnerViolenceFlagScoreLevelLabel]
,[NewVisitReferralRecommendationID]
,[NewVisitReferralRecommendationDescription]
,[NewVisitReferralRecommendationAcceptedID]
,[ReasonNewVisitReferralRecommendationNotAcceptedID]
,[NewVisitDate]
,[DischargedID]
,[ThirtyDatyFollowUpFlag]
,[FollowUpDate]
,[Notes]
,[BhsStaffNameCompleted]
,[CompleteDate]
,[AnxietyFlagScoreLevel]
,[AnxietyFlagScoreLevelLabel]
,[ProblemGamblingFlagScoreLevel]
,[ProblemGamblingFlagScoreLevelLabel]
)
VALUES
(@ScreeningResultID
,@LocationID
,@CreatedDate
,@ScreeningDate
,@TobacoExposureSmokerInHomeFlag
,@TobacoExposureCeremonyUseFlag
,@TobacoExposureSmokingFlag
,@TobacoExposureSmoklessFlag
,@AlcoholUseFlagScoreLevel
,@AlcoholUseFlagScoreLevelLabel
,@SubstanceAbuseFlagScoreLevel
,@SubstanceAbuseFlagScoreLevelLabel
,@DepressionFlagScoreLevel
,@DepressionFlagScoreLevelLabel
,@DepressionThinkOfDeathAnswer
,@PartnerViolenceFlagScoreLevel
,@PartnerViolenceFlagScoreLevelLabel
,@NewVisitReferralRecommendationID
,@NewVisitReferralRecommendationDescription
,@NewVisitReferralRecommendationAcceptedID
,@ReasonNewVisitReferralRecommendationNotAcceptedID
,@NewVisitDate
,@DischargedID
,@ThirtyDatyFollowUpFlag
,@FollowUpDate
,@Notes
,@BhsStaffNameCompleted
,@CompleteDate
,@AnxietyFlagScoreLevel
,@AnxietyFlagScoreLevelLabel
,@ProblemGamblingFlagScoreLevel
,@ProblemGamblingFlagScoreLevelLabel
);
SET @ID = SCOPE_IDENTITY();
";

            try
            {
                base.Connect();

                ClearParameters();

                AddParameter("@ScreeningResultID", DbType.Int64).Value = model.ScreeningResultID;
                AddParameter("@LocationID", DbType.Int32).Value = model.LocationID;
                AddParameter("@ScreeningDate", DbType.DateTimeOffset).Value = model.ScreeningDate;
                AddParameter("@TobacoExposureSmokerInHomeFlag", DbType.Boolean).Value = model.TobacoExposureSmokerInHomeFlag;
                AddParameter("@TobacoExposureCeremonyUseFlag", DbType.Boolean).Value = model.TobacoExposureCeremonyUseFlag;
                AddParameter("@TobacoExposureSmokingFlag", DbType.Boolean).Value = model.TobacoExposureSmokingFlag;
                AddParameter("@TobacoExposureSmoklessFlag", DbType.Boolean).Value = model.TobacoExposureSmoklessFlag;
                AddParameter("@AlcoholUseFlagScoreLevel", DbType.Int32).Value = SqlParameterSafe(model.AlcoholUseFlag?.ScoreLevel);
                AddParameter("@AlcoholUseFlagScoreLevelLabel", DbType.String, 64).Value = SqlParameterSafe(model.AlcoholUseFlag?.ScoreLevelLabel);
                AddParameter("@SubstanceAbuseFlagScoreLevel", DbType.Int32).Value = SqlParameterSafe(model.SubstanceAbuseFlag?.ScoreLevel);
                AddParameter("@SubstanceAbuseFlagScoreLevelLabel", DbType.String, 64).Value = SqlParameterSafe(model.SubstanceAbuseFlag?.ScoreLevelLabel);
                AddParameter("@DepressionFlagScoreLevel", DbType.Int64).Value = SqlParameterSafe(model.DepressionFlag?.ScoreLevel);
                AddParameter("@DepressionFlagScoreLevelLabel", DbType.String).Value = SqlParameterSafe(model.DepressionFlag?.ScoreLevelLabel);
                AddParameter("@DepressionThinkOfDeathAnswer", DbType.String, 64).Value = SqlParameterSafe(model.DepressionThinkOfDeathAnswer);
                AddParameter("@PartnerViolenceFlagScoreLevel", DbType.Int32).Value = SqlParameterSafe(model.PartnerViolenceFlag?.ScoreLevel);
                AddParameter("@PartnerViolenceFlagScoreLevelLabel", DbType.String, 64).Value = SqlParameterSafe(model.PartnerViolenceFlag?.ScoreLevelLabel);

                AddParameter("@NewVisitReferralRecommendationID", DbType.Int32).Value = SqlParameterSafe(model.NewVisitReferralRecommendation?.Id);
                AddParameter("@NewVisitReferralRecommendationDescription", DbType.String, 64).Value = SqlParameterSafe(model.NewVisitReferralRecommendation?.Description);

                AddParameter("@NewVisitReferralRecommendationAcceptedID", DbType.Int32).Value = SqlParameterSafe(model.NewVisitReferralRecommendationAccepted?.Id);
                AddParameter("@ReasonNewVisitReferralRecommendationNotAcceptedID", DbType.Int32).Value = SqlParameterSafe(model.ReasonNewVisitReferralRecommendationNotAccepted?.Id);

                AddParameter("@NewVisitDate", DbType.DateTimeOffset).Value = SqlParameterSafe(model.NewVisitDate);
                AddParameter("@DischargedID", DbType.Int64).Value = SqlParameterSafe(model.Discharged?.Id);
                AddParameter("@ThirtyDatyFollowUpFlag", DbType.Boolean).Value = model.ThirtyDatyFollowUpFlag;
                AddParameter("@FollowUpDate", DbType.DateTimeOffset).Value = SqlParameterSafe(model.FollowUpDate);


                AddParameter("@Notes", DbType.String).Value = SqlParameterSafe(model.Notes);

                AddParameter("@CreatedDate", DbType.DateTimeOffset).Value = model.CreatedDate;
                AddParameter("@BhsStaffNameCompleted", DbType.String, 128).Value = SqlParameterSafe(model.BhsStaffNameCompleted);
                AddParameter("@CompleteDate", DbType.DateTimeOffset).Value = SqlParameterSafe(model.CompleteDate);


                AddParameter("@AnxietyFlagScoreLevel", DbType.Int64).Value = SqlParameterSafe(model.AnxietyFlag?.ScoreLevel);
                AddParameter("@AnxietyFlagScoreLevelLabel", DbType.String).Value = SqlParameterSafe(model.AnxietyFlag?.ScoreLevelLabel);

                AddParameter("@ProblemGamblingFlagScoreLevel", DbType.Int64).Value = SqlParameterSafe(model.ProblemGamblingFlag?.ScoreLevel);
                AddParameter("@ProblemGamblingFlagScoreLevelLabel", DbType.String).Value = SqlParameterSafe(model.ProblemGamblingFlag?.ScoreLevelLabel);


                var IdParam = AddParameter("@ID", DbType.Int64);
                IdParam.Direction = ParameterDirection.Output;

                RunNonSelectQuery(sql);

                model.ID = (long)IdParam.Value;

            }
            finally
            {
                base.Disconnect();
            }
            return model.ID;
        }


        public bool UpdateManualEntries(BhsVisit model)
        {
            string sql = @"
UPDATE [dbo].[BhsVisit]
SET 
[NewVisitReferralRecommendationID] = @NewVisitReferralRecommendationID
,[NewVisitReferralRecommendationDescription] = @NewVisitReferralRecommendationDescription
,[NewVisitReferralRecommendationAcceptedID] = @NewVisitReferralRecommendationAcceptedID
,[ReasonNewVisitReferralRecommendationNotAcceptedID] = @ReasonNewVisitReferralRecommendationNotAcceptedID
,[NewVisitDate] = @NewVisitDate
,[DischargedID] = @DischargedID
,[ThirtyDatyFollowUpFlag] = @ThirtyDatyFollowUpFlag
,[FollowUpDate] = @FollowUpDate
,[Notes] = @Notes
,[BhsStaffNameCompleted] = @BhsStaffNameCompleted
,[CompleteDate] = @CompleteDate
,[TreatmentAction1ID] = @TreatmentAction1ID
,[TreatmentAction1Description] = @TreatmentAction1Description
,[TreatmentAction2ID] = @TreatmentAction2ID
,[TreatmentAction2Description] = @TreatmentAction2Description
,[TreatmentAction3ID] = @TreatmentAction3ID
,[TreatmentAction3Description] = @TreatmentAction3Description
,[TreatmentAction4ID] = @TreatmentAction4ID
,[TreatmentAction4Description] = @TreatmentAction4Description
,[TreatmentAction5ID] = @TreatmentAction5ID
,[TreatmentAction5Description] = @TreatmentAction5Description
,[OtherScreeningTools] = @OtherScreeningTools
WHERE ID = @ID
";

            ClearParameters();

            AddParameter("@ID", DbType.Int64).Value = model.ID;
            AddParameter("@BhsStaffNameCompleted", DbType.String, 128).Value = SqlParameterSafe(model.BhsStaffNameCompleted);
            AddParameter("@CompleteDate", DbType.DateTimeOffset).Value = SqlParameterSafe(model.CompleteDate);

            AddParameter("@NewVisitReferralRecommendationID", DbType.Int32).Value = SqlParameterSafe(model.NewVisitReferralRecommendation?.Id);
            AddParameter("@NewVisitReferralRecommendationDescription", DbType.String, 64).Value = SqlParameterSafe(model.NewVisitReferralRecommendation?.Description);

            AddParameter("@NewVisitReferralRecommendationAcceptedID", DbType.Int32).Value = SqlParameterSafe(model.NewVisitReferralRecommendationAccepted?.Id);
            AddParameter("@ReasonNewVisitReferralRecommendationNotAcceptedID", DbType.Int32).Value = SqlParameterSafe(model.ReasonNewVisitReferralRecommendationNotAccepted?.Id);

            AddParameter("@NewVisitDate", DbType.DateTimeOffset).Value = SqlParameterSafe(model.NewVisitDate);
            AddParameter("@DischargedID", DbType.Int64).Value = SqlParameterSafe(model.Discharged?.Id);
            AddParameter("@ThirtyDatyFollowUpFlag", DbType.Boolean).Value = model.ThirtyDatyFollowUpFlag;
            AddParameter("@FollowUpDate", DbType.DateTimeOffset).Value = SqlParameterSafe(model.FollowUpDate);

            AddParameter("@Notes", DbType.String).Value = SqlParameterSafe(model.Notes);

            AddParameter("@OtherScreeningTools", DbType.Xml).Value = model.OtherScreeningTools.ToXmlString();

            //saving treatment actions
            for (int index = 0; index < 5; index++)
            {
                var orderIndex = index + 1;

                if (model.TreatmentActions.Count > index)
                {
                    //if not empty
                    var item = model.TreatmentActions[index];
                    AddParameter(string.Concat("@TreatmentAction", orderIndex, "ID"), DbType.Int32).Value = item.Id;
                    AddParameter(string.Concat("@TreatmentAction", orderIndex, "Description"), DbType.String).Value = SqlParameterSafe(item.Description);
                }
                else
                {
                    AddParameter(string.Concat("@TreatmentAction", orderIndex, "ID"), DbType.Int32).Value = SqlParameterSafe(null);
                    AddParameter(string.Concat("@TreatmentAction", orderIndex, "Description"), DbType.String).Value = SqlParameterSafe(null);
                }
            }

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

        public void Delete(long bhsVisitID)
        {
            string sql = @"
delete from dbo.BhsVisit where ID=@ID

";
            ClearParameters();
            AddParameter("@ID", DbType.Int64).Value = bhsVisitID;

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

        public long? FindByScreeningResultId(long screeningResultId)
        {
            const string sql = @"
SELECT TOP 1 ID FROM [dbo].[BhsVisit] WHERE ScreeningResultID = @ScreeningResultID
";
            ClearParameters();
            AddParameter("@ScreeningResultID", DbType.Int64).Value = screeningResultId;
            try
            {
                return RunScalarQuery<long>(sql);
            }
            finally
            {
                Disconnect();
            }
        }

        public BhsVisit Get(long id)
        {
            const string sql = @"[dbo].[uspGetBhsVisitByID]";
            ClearParameters();

            AddParameter("@ID", DbType.Int64).Value = id;


            try
            {
                using (var reader = RunProcedureSelectQuery(sql))
                {
                    if (reader.Read())
                    {
                        return new BhsVisitFactory().InitFromReader(reader);
                    }
                }
            }
            finally
            {
                Disconnect();
            }
            return null;
        }


        /// <summary>
        /// Get visit results
        /// </summary>
        public List<UniqueVisitViewModel> GetLatestVisitsForDisplay(PagedVisitFilterModel filter)
        {

            var result = new List<UniqueVisitViewModel>();

            var orderBy = (filter.OrderBy ?? "lastcreateddate").ToLowerInvariant();


            string innerOrderBy = string.Empty;
            string totalOrderBy = string.Empty;

            if (string.IsNullOrEmpty(orderBy)) orderBy = "lastcreateddate DESC"; // default sort order

            //map user field names to the query field names
            if (orderBy.Contains("lastcreateddate"))
            {
                innerOrderBy = orderBy.Replace("lastcreateddate", "MAX(r.CreatedDate)");
                totalOrderBy = orderBy;
            }
            else if (orderBy.Contains("fullname"))
            {
                innerOrderBy = orderBy.Replace("fullname", "r.PatientName");
                totalOrderBy = orderBy;
            }
            else if (orderBy.Contains("birthday"))
            {
                innerOrderBy = orderBy.Replace("birthday", "r.Birthday");
                totalOrderBy = orderBy;
            }

            QueryBuilder qbInnerSql = new QueryBuilder(string.Format(@"
SELECT TOP(@startRowIndex + @maxRows) 
    ROW_NUMBER() OVER ( ORDER BY {0}) as RowNumber, 
    MAX(r.ScreeningResultID) as ScreeningResultID
    , r.PatientName as FullName
    , r.Birthday
    , MAX(r.CreatedDate) AS LastCreatedDate
    , MAX(v.CompleteDate) as LastCompleteDate
FROM ScreeningResult r
    INNER JOIN dbo.BhsVisit v ON r.ScreeningResultID = v.ScreeningResultID
", innerOrderBy));

            string outerSql = @"
WITH tblCheckIns(RowNumber, ScreeningResultID, FullName, Birthday, LastCreatedDate, LastCompleteDate)
AS ({0})
SELECT t.ScreeningResultID, FullName, Birthday, LastCreatedDate, LastCompleteDate,
( SELECT TOP (1) Name 
    FROM dbo.BranchLocation l 
        INNER JOIN dbo.Kiosk k ON k.BranchLocationID = l.BranchLocationID
        INNER JOIN dbo.ScreeningResult r ON r.KioskID = k.KioskID
        WHERE r.ScreeningResultID = t.ScreeningResultID
) as LocationName
FROM tblCheckIns t
WHERE RowNumber BETWEEN @startRowIndex AND (@startRowIndex + @maxRows)
ORDER BY {1} ";


            CommandObject.Parameters.Clear();
            //parameters for pagination
            AddParameter("@startRowIndex", DbType.Int32).Value = filter.StartRowIndex + 1;
            AddParameter("@maxRows", DbType.Int32).Value = filter.MaximumRows - 1;

            qbInnerSql.AppendWhereCondition("(v.ID IS NOT NULL)", ClauseType.And);

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
                qbInnerSql.AppendWhereCondition("(v.LocationID = @LocationID)", ClauseType.And);

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

            if (filter.ReportType != BhsReportType.AllReports)
            {
                if (filter.ReportType == BhsReportType.CompletedReports)
                {
                    qbInnerSql.AppendWhereCondition("(v.CompleteDate IS NOT NULL)", ClauseType.And);
                }
                else if (filter.ReportType == BhsReportType.IncompleteReports)
                {
                    qbInnerSql.AppendWhereCondition("(v.CompleteDate IS NULL)", ClauseType.And);
                }
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
                        result.Add(new UniqueVisitViewModel
                        {
                            //t.ScreeningResultID, FullName, Birthday, LastCreatedDate, LastCompleteDate, LocationName, LocationId
                            ScreeningResultID = reader.Get<long>(0), //ScreeningResultID
                            PatientName = reader.GetString(1), //FullName
                            Birthday = reader.Get<DateTime>(2), //Birthday
                            LastCreatedDate = reader.Get<DateTimeOffset>(3), // LastCreatedDate
                            LastCompleteDate = reader.Get<DateTimeOffset>(4), // LastCompleteDate
                            LocationName = reader.GetString(5) // LocationName
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


        /// <summary>
        /// Get count of visits for Visit Search
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>

        public int GetLatestVisitsForDisplayCount(PagedVisitFilterModel filter)
        {
            int rowCount = 0;
            //WITH statement
            QueryBuilder qbInnerSql = new QueryBuilder(@"
SELECT 
    MAX(r.ScreeningResultID) as ScreeningResultID
    , r.PatientName as FullName
    , r.Birthday
    , MAX(r.CreatedDate) AS LastCreatedDate
FROM ScreeningResult r
    INNER JOIN dbo.BhsVisit v ON r.ScreeningResultID = v.ScreeningResultID
    LEFT JOIN dbo.BhsDemographics d ON 
        d.Birthday = r.Birthday AND r.PatientName = d.PatientName
");

            string outerSql = @"
WITH tblCheckIns(ScreeningResultID, FullName, Birthday, LastCreatedDate)
AS ({0})
SELECT count(*)
FROM tblCheckIns t
";

            CommandObject.Parameters.Clear();

            qbInnerSql.AppendWhereCondition("(v.ID IS NOT NULL OR d.ID IS NOT NULL)", ClauseType.And);
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
                qbInnerSql.AppendWhereCondition("(v.LocationID = @LocationID OR d.LocationID = @LocationID)", ClauseType.And);

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

            if (filter.ReportType != BhsReportType.AllReports)
            {
                if (filter.ReportType == BhsReportType.CompletedReports)
                {
                    qbInnerSql.AppendWhereCondition("(v.CompleteDate IS NOT NULL OR d.CompleteDate IS NOT NULL)", ClauseType.And);
                }
                else if (filter.ReportType == BhsReportType.IncompleteReports)
                {
                    qbInnerSql.AppendWhereCondition("(v.CompleteDate IS NULL OR d.CompleteDate IS NULL)", ClauseType.And);
                }
            }

            try
            {
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


        #region Legacy

        [Obsolete("Used for legacy server app")]

        public DataSet GetUniqueVisits(
            BhsSearchFilterModel filter, 
            int startRowIndex, 
            int maximumRows, 
            string orderBy
            )
        {
            string innerOrderBy = string.Empty;
            string totalOrderBy = string.Empty;

            if (string.IsNullOrEmpty(orderBy)) orderBy = "LastCreatedDate DESC"; // default sort order

            //map user field names to the query field names
            if (orderBy.Contains("LastCreatedDate"))
            {
                innerOrderBy = orderBy.Replace("LastCreatedDate", "MAX(r.CreatedDate)");
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

            QueryBuilder qbInnerSql = new QueryBuilder(string.Format(@"
SELECT TOP(@startRowIndex + @maxRows) 
    ROW_NUMBER() OVER ( ORDER BY {0}) as RowNumber, 
    MAX(r.ScreeningResultID) as ScreeningResultID
    , r.PatientName as FullName
    , r.Birthday
    , MAX(r.CreatedDate) AS LastCreatedDate
    , MAX(v.CompleteDate) as LastCompleteDate
FROM ScreeningResult r
    INNER JOIN dbo.BhsVisit v ON r.ScreeningResultID = v.ScreeningResultID
", innerOrderBy));

            string outerSql = @"
WITH tblCheckIns(RowNumber, ScreeningResultID, FullName, Birthday, LastCreatedDate, LastCompleteDate)
AS ({0})
SELECT t.ScreeningResultID, FullName, Birthday, LastCreatedDate, LastCompleteDate,
( SELECT TOP (1) Name 
    FROM dbo.BranchLocation l 
        INNER JOIN dbo.Kiosk k ON k.BranchLocationID = l.BranchLocationID
        INNER JOIN dbo.ScreeningResult r ON r.KioskID = k.KioskID
        WHERE r.ScreeningResultID = t.ScreeningResultID
) as LocationName
FROM tblCheckIns t
WHERE RowNumber BETWEEN @startRowIndex AND (@startRowIndex + @maxRows)
ORDER BY {1} ";


            CommandObject.Parameters.Clear();
            //parameters for pagination
            AddParameter("@startRowIndex", DbType.Int32).Value = startRowIndex + 1;
            AddParameter("@maxRows", DbType.Int32).Value = maximumRows;

            qbInnerSql.AppendWhereCondition("(v.ID IS NOT NULL)", ClauseType.And);

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
            if (filter.LocationId.HasValue)
            {
                // TODO: add join to kiosks table, choose kiosks by location
                qbInnerSql.AppendWhereCondition("(v.LocationID = @LocationID)", ClauseType.And);

                AddParameter("@LocationID", DbType.Int32).Value = filter.LocationId.Value;
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

            if (filter.ReportType != BhsReportType.AllReports)
            {
                if (filter.ReportType == BhsReportType.CompletedReports)
                {
                    qbInnerSql.AppendWhereCondition("(v.CompleteDate IS NOT NULL)", ClauseType.And);
                }
                else if (filter.ReportType == BhsReportType.IncompleteReports)
                {
                    qbInnerSql.AppendWhereCondition("(v.CompleteDate IS NULL)", ClauseType.And);
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


        [Obsolete("Used for legacy server app")]
        public int GetUniqueVisitsCount(BhsSearchFilterModel filter)
        {
            int rowCount = 0;
            //WITH statement
            QueryBuilder qbInnerSql = new QueryBuilder(@"
SELECT 
    MAX(r.ScreeningResultID) as ScreeningResultID
    , r.PatientName as FullName
    , r.Birthday
    , MAX(r.CreatedDate) AS LastCreatedDate
FROM ScreeningResult r
    INNER JOIN dbo.BhsVisit v ON r.ScreeningResultID = v.ScreeningResultID
    LEFT JOIN dbo.BhsDemographics d ON 
        d.Birthday = r.Birthday AND r.PatientName = d.PatientName
");

            string outerSql = @"
WITH tblCheckIns(ScreeningResultID, FullName, Birthday, LastCreatedDate)
AS ({0})
SELECT count(*)
FROM tblCheckIns t
";

            CommandObject.Parameters.Clear();

            qbInnerSql.AppendWhereCondition("(v.ID IS NOT NULL OR d.ID IS NOT NULL)", ClauseType.And);
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
            if (filter.LocationId.HasValue)
            {
                // TODO: add join to kiosks table, choose kiosks by location
                qbInnerSql.AppendWhereCondition("(v.LocationID = @LocationID OR d.LocationID = @LocationID)", ClauseType.And);

                AddParameter("@LocationID", DbType.Int32).Value = filter.LocationId.Value;
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

            if (filter.ReportType != BhsReportType.AllReports)
            {
                if (filter.ReportType == BhsReportType.CompletedReports)
                {
                    qbInnerSql.AppendWhereCondition("(v.CompleteDate IS NOT NULL OR d.CompleteDate IS NOT NULL)", ClauseType.And);
                }
                else if (filter.ReportType == BhsReportType.IncompleteReports)
                {
                    qbInnerSql.AppendWhereCondition("(v.CompleteDate IS NULL OR d.CompleteDate IS NULL)", ClauseType.And);
                }
            }

            try
            {
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

        #endregion

        public List<BhsVisitViewModel> GetRelatedReports(BhsSearchRelatedItemsFilter filter)
        {
            List<BhsVisitViewModel> result = new List<BhsVisitViewModel>();

            string sql = @"
SELECT t.ID, t.ScreeningResultID, t.CreatedDate,t.ScreeningDate, t.CompleteDate, t.HasAddress, t.IsVisitRecordType, 
    t.DemographicsID,t.DemographicsScreeningDate,t.DemographicsCreateDate,t.DemographicsCompleteDate, t.LocationName
FROM vBhsVisitsWithDemographics t 
INNER JOIN dbo.ScreeningResult main 
        ON t.ScreeningResultID = main.ScreeningResultID 
            OR (main.PatientName = t.FullName AND main.Birthday = t.Birthday)
";
            CommandObject.Parameters.Clear();

            QueryBuilder qb = new QueryBuilder(sql);

            //qb.AppendOrderCondition("t.IsVisitRecordType ", OrderType.Asc);
            qb.AppendOrderCondition("t.CreatedDate", OrderType.Desc);

            qb.AppendWhereCondition("main.ScreeningResultID = @ScreeningResultID", ClauseType.And);

            AddParameter("@ScreeningResultID", DbType.Int64).Value = filter.MainRowID;

            if (filter.ScreeningResultID.HasValue)
            {
                qb.AppendWhereCondition("t.ScreeningResultID = @ScreeningResultID", ClauseType.And);
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
                    bool addPatientRecordFlag = true;
                    while (reader.Read())
                    {
                        if (addPatientRecordFlag)
                        {
                            if (reader.GetNullable<Int64>("DemographicsID").HasValue)
                            {

                                result.Add(new BhsVisitViewModel
                                {
                                    ID = reader.Get<Int64>("DemographicsID"),
                                    ScreeningResultID = reader.GetNullable<long>(1) ?? 0,
                                    ScreeningDate = reader.Get<DateTimeOffset>("DemographicsScreeningDate"),
                                    CreatedDate = reader.Get<DateTimeOffset>("DemographicsCreateDate"),
                                    CompletedDate = reader.GetNullable<DateTimeOffset>("DemographicsCompleteDate"),
                                    HasAddress = reader.Get<Boolean>(5, true),
                                    IsVisitRecordType = false 
                                });

                            }

                            addPatientRecordFlag = false;
                        }

                        if (reader.GetNullable<Int64>("ID").HasValue)
                        {
                            result.Add(new BhsVisitViewModel
                            {
                                ID = reader.GetInt64(0),
                                ScreeningResultID = reader.GetNullable<long>(1) ?? 0,
                                ScreeningDate = reader.Get<DateTimeOffset>(2).Value,
                                CreatedDate = reader.Get<DateTimeOffset>(3).Value,
                                CompletedDate = reader.GetNullable<DateTimeOffset>(4),
                                HasAddress = reader.Get<Boolean>(5, true),
                                IsVisitRecordType = reader.Get<Boolean>(6, true),
                                LocationName = reader.Get<string>("LocationName")
                            });
                        }
                    }

                }
            }
            finally
            {
                Disconnect();
            }
            return result;
        }

        public void SetPatientAddressToAllPatientScreenings(ScreeningResult patient)
        {
            const string sql = "dbo.uspSetMissingPatientAddress";

            ClearParameters();
            AddParameter("@ID", DbType.Int64).Value = patient.ID;
            AddParameter("@StreetAddress", DbType.String, 512).Value = SqlParameterSafe(patient.StreetAddress);
            AddParameter("@City", DbType.String, 255).Value = SqlParameterSafe(patient.City);
            AddParameter("@StateID", DbType.AnsiString, 2).Value = SqlParameterSafe(patient.StateID);
            AddParameter("@ZipCode", DbType.AnsiString, 5).Value = SqlParameterSafe(patient.ZipCode);
            AddParameter("@Phone", DbType.AnsiString, 14).Value = SqlParameterSafe(patient.Phone);
            AddParameter("@ExportedToHRN", DbType.String, 255).Value = SqlParameterSafe(patient.ExportedToHRN);

            try
            {
                RunProcedureNonSelectQuery(sql);
            }
            finally
            {
                Disconnect();
            }
        }

        public List<BhsVisitListItemDtoModel> GetAllItems(BhsSearchFilterModel filter)
        {
            List<BhsVisitListItemDtoModel> result = new List<BhsVisitListItemDtoModel>();

            string sql = @"
SELECT TOP (1000) t.ID, t.ScreeningResultID, t.CreatedDate,t.ScreeningDate, t.CompleteDate, t.HasAddress, 
    t.IsVisitRecordType, t.DemographicsID,t.DemographicsScreeningDate,t.DemographicsCreateDate,
    t.DemographicsCompleteDate, t.Birthday, t.FullName as PatientName, r.LastName, r.FirstName, r.MiddleName
FROM vBhsVisitsWithDemographics t 
INNER JOIN dbo.ScreeningResult r 
        ON t.ScreeningResultID = r.ScreeningResultID
";
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
                        result.Add(new BhsVisitListItemDtoModel
                        {
                            ID = reader.Get<Int64>(0),
                            ScreeningResultID = reader.GetNullable<long>(1) ?? 0,
                            ScreeningDate = reader.Get<DateTimeOffset>(2).Value,
                            CreatedDate = reader.Get<DateTimeOffset>(3).Value,
                            CompletedDate = reader.GetNullable<DateTimeOffset>(4),
                            HasAddress = reader.Get<Boolean>(5, true),
                            IsVisitRecordType = reader.Get<Boolean>(6, true),
                            DemographicsID = reader.GetNullable<Int64>(7),
                            DemographicsScreeningDate = reader.GetNullable<DateTimeOffset>(8),
                            DemographicsCreatedDate = reader.GetNullable<DateTimeOffset>(9),
                            DemographicsCompleteDate = reader.GetNullable<DateTimeOffset>(10),
                            Birthday = reader.Get<DateTime>(11, true),
                            PatientName = reader.Get<string>(12, true),
                            LastName = reader.Get<string>(13, false),
                            FirstName = reader.Get<string>(14, false),
                            MiddleName = reader.Get<string>(15, false),

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


        public List<BhsVisitExtendedWithIdentity> GetAllForExport(BhsSearchFilterModel filter)
        {
            var result = new List<BhsVisitExtendedWithIdentity>();

            string sql = @"SELECT * FROM dbo.vBhsVisitForExport t ";
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
                        result.Add(new BhsVisitFactory().InitExtendedModelFromReader(reader));
                    }

                }
            }
            finally
            {
                Disconnect();
            }
            return result;
        }

        public void UpdateDrugOfChoice(long screeningResultId, DrugOfChoiceModel drugOfChoice)
        {
            const string sql = "[dbo].[uspUpdateDrugOfChoice]";

            ClearParameters();
            
            AddParameter("@ScreeningResultID", DbType.Int64).Value = screeningResultId;
            AddParameter("@PrimaryAnswer", DbType.Int32).Value = drugOfChoice.Primary;
            AddParameter("@SecondaryAnswer", DbType.Int64).Value = drugOfChoice.Secondary;
            AddParameter("@TertiaryAnswer", DbType.Int64).Value = drugOfChoice.Tertiary;

            try
            {
                BeginTransaction();

                RunProcedureNonSelectQuery(sql);

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
    }
}

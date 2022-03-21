using FrontDesk.Common.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using FrontDesk.Common.Bhservice;
using System.Data;
using FrontDesk.Services;
using FrontDesk.Common.Extensions;
using FrontDesk.Server.Screening;
using FrontDesk.Server.Printouts.Bhs;
using FrontDesk.Common;

namespace FrontDesk.Server.Data.BhsVisits
{
    public interface IBhsDemographicsRepository : ITransactionalDatabase
    {
        bool Exists(ScreeningResult screeningResult);
        long? Find(IScreeningPatientIdentity patient);
        long Add(BhsDemographics bhsDemographics);
        BhsDemographics Get(long id);
        bool UpdateManualEntries(BhsDemographics model);

        List<BhsIndicatorReportByAgeItem> GetDemographicsReportUniquePatientsByAge(string tablename, int? locationIDFilter, DateTime? startDate, DateTime? endDate);
        List<BhsIndicatorReportByAgeItem> GetDemographicsReportPatientsByAge(string tableName, int? locationIDFilter, DateTime? startDate, DateTime? endDate);
        List<BhsDemographics> GetAllItems(BhsSearchFilterModel filter);
    }

    public class BhsDemographicsDb : DBDatabase, IBhsDemographicsRepository
    {

        public BhsDemographicsDb() : base(0) { }

        internal BhsDemographicsDb(DbConnection sharedConnection) : base(sharedConnection) { }

        public long Add(BhsDemographics model)
        {
            var sql = @"
INSERT INTO [dbo].[BhsDemographics](
[ScreeningResultID]
,[LocationID]
,[CreatedDate]
,[ScreeningDate]
,[BhsStaffNameCompleted]
,[CompleteDate]
,[FirstName]
,[LastName]
,[MiddleName]
,[Birthday]
,[StreetAddress]
,[City]
,[StateID]
,[ZipCode]
,[Phone]
,[RaceID]
,[GenderID]
,[SexualOrientationID]
,[TribalAffiliation]
,[MaritalStatusID]
,[EducationLevelID]
,[LivingOnReservationID]
,[CountyOfResidence]
,[MilitaryExperienceID]
)
VALUES (
@ScreeningResultID
,@LocationID
,@CreatedDate
,@ScreeningDate
,@BhsStaffNameCompleted
,@CompleteDate
,@FirstName
,@LastName
,@MiddleName
,@Birthday
,@StreetAddress
,@City
,@StateID
,@ZipCode
,@Phone
,@RaceID
,@GenderID
,@SexualOrientationID
,@TribalAffiliation
,@MaritalStatusID
,@EducationLevelID
,@LivingOnReservationID
,@CountyOfResidence
,@MilitaryExperienceID
);
SET @ID = SCOPE_IDENTITY();
";

            ClearParameters();
            AddParameter("@ScreeningResultID", DbType.Int64).Value = SqlParameterSafe(model.ScreeningResultID);
            AddParameter("@LocationID", DbType.Int32).Value = model.LocationID;
            AddParameter("@ScreeningDate", DbType.DateTimeOffset).Value = model.ScreeningDate;
            AddParameter("@CreatedDate", DbType.DateTimeOffset).Value = model.CreatedDate;
            AddParameter("@BhsStaffNameCompleted", DbType.String).Value = SqlParameterSafe(model.BhsStaffNameCompleted);
            AddParameter("@CompleteDate", DbType.DateTimeOffset).Value = SqlParameterSafe(model.CompleteDate);
            AddParameter("@FirstName", DbType.String).Value = SqlParameterSafe(model.FirstName);
            AddParameter("@LastName", DbType.String).Value = SqlParameterSafe(model.LastName);
            AddParameter("@MiddleName", DbType.String).Value = SqlParameterSafe(model.MiddleName);
            AddParameter("@Birthday", DbType.Date).Value = SqlParameterSafe(model.Birthday);
            AddParameter("@StreetAddress", DbType.String).Value = SqlParameterSafe(model.StreetAddress);
            AddParameter("@City", DbType.String).Value = SqlParameterSafe(model.City);
            AddParameter("@StateID", DbType.AnsiString, 2).Value = SqlParameterSafe(model.StateID);
            AddParameter("@ZipCode", DbType.AnsiString, 5).Value = SqlParameterSafe(model.ZipCode);
            AddParameter("@Phone", DbType.String).Value = SqlParameterSafe(model.Phone);
            AddParameter("@GenderID", DbType.Int32).Value = SqlParameterSafe(model.Gender?.Id);
            AddParameter("@SexualOrientationID", DbType.Int32).Value = SqlParameterSafe(model.SexualOrientation?.Id);
            AddParameter("@RaceID", DbType.Int32).Value = SqlParameterSafe(model.Race?.Id);

            AddParameter("@TribalAffiliation", DbType.String, 128).Value = SqlParameterSafe(model.TribalAffiliation);
            AddParameter("@MaritalStatusID", DbType.Int32).Value = SqlParameterSafe(model.MaritalStatus?.Id);
            AddParameter("@EducationLevelID", DbType.Int32).Value = SqlParameterSafe(model.EducationLevel?.Id);
            AddParameter("@LivingOnReservationID", DbType.Int32).Value = SqlParameterSafe(model.LivingOnReservation?.Id);
            AddParameter("@CountyOfResidence", DbType.String, 128).Value = SqlParameterSafe(model.CountyOfResidence);
            AddParameter("@MilitaryExperienceID", DbType.AnsiString, 32).Value = SqlParameterSafe(model.MilitaryExperience.ToSqlCsv());
            var IdParam = AddParameter("@ID", DbType.Int64);
            IdParam.Direction = ParameterDirection.Output;

            try
            {

                RunNonSelectQuery(sql);
                model.ID = (long)IdParam.Value;

                return model.ID;
            }
            finally
            {
                Disconnect();
            }
        }

        public bool Exists(ScreeningResult screeningResult)
        {
            var @sql = @"
SELECT COUNT(*) 
FROM dbo.BhsDemographics d
WHERE dbo.fn_GetPatientName(d.LastName, d.FirstName, d.MiddleName) = dbo.fn_GetPatientName(@LastName, @FirstName, @MiddleName) 
AND d.Birthday = @Birthday
";

            ClearParameters();
            AddParameter("@FirstName", DbType.String, 128).Value = SqlParameterSafe(screeningResult.FirstName);
            AddParameter("@LastName", DbType.String, 128).Value = SqlParameterSafe(screeningResult.LastName);
            AddParameter("@MiddleName", DbType.String, 128).Value = SqlParameterSafe(screeningResult.MiddleName);
            AddParameter("@Birthday", DbType.Date).Value = screeningResult.Birthday;

            try
            {
                return RunScalarQuery<int>(sql) > 0;
            }
            finally
            {
                Disconnect();
            }
        }

        public long? Find(IScreeningPatientIdentity patient)
        {
            long? result = null; ;

            var @sql = @"
SELECT TOP (1) d.ID 
FROM dbo.BhsDemographics d
WHERE d.PatientName = dbo.fn_GetPatientName(@LastName, @FirstName, @MiddleName) 
AND d.Birthday = @Birthday
ORDER BY d.ScreeningDate DESC
";

            ClearParameters();
            AddParameter("@FirstName", DbType.String, 128).Value = SqlParameterSafe(patient.FirstName);
            AddParameter("@LastName", DbType.String, 128).Value = SqlParameterSafe(patient.LastName);
            AddParameter("@MiddleName", DbType.String, 128).Value = SqlParameterSafe(patient.MiddleName);
            AddParameter("@Birthday", DbType.Date).Value = patient.Birthday;

            try
            {
                result = RunScalarQuery<long>(sql);
            }
            finally
            {
                Disconnect();
            }
            return result;
        }

        public void Delete(long bhsVisitID)
        {
            string sql = @"
delete from dbo.BhsDemographics where ID=@ID

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

        public BhsDemographics Get(long id)
        {

            BhsDemographics result = null;
            const string sql = @"
SELECT 
[ID]
,[ScreeningResultID]
,[LocationID]
,LocationName
,[CreatedDate]
,[ScreeningDate]
,[BhsStaffNameCompleted]
,[CompleteDate]
,[FirstName]
,[LastName]
,[MiddleName]
,[Birthday]
,[StreetAddress]
,[City]
,[StateID]
,StateName
,[ZipCode]
,[Phone]
,[RaceID]
,RaceName
,[GenderID]
,GenderName
,[SexualOrientationID]
,SexualOrientationName
,[TribalAffiliation]
,[MaritalStatusID]
,[MaritalStatusName]
,[EducationLevelID]
,EducationLevelName
,[LivingOnReservationID]
,LivingOnReservationName
,[CountyOfResidence]
,ExportedToHRN
FROM [dbo].[vBhsDemographics] d
WHERE ID=@ID
;

SELECT ID, Name 
FROM dbo.MilitaryExperience WHERE ID IN (
SELECT t.value 
FROM dbo.BhsDemographics d OUTER APPLY( SELECT * FROM dbo.fn_IntListToTable(d.MilitaryExperienceID)) as t
WHERE d.ID = @ID
)
ORDER BY OrderIndex ASC
";

            ClearParameters();
            AddParameter("@ID", DbType.Int64).Value = id;

            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    if (reader.Read())
                    {
                        result = new BhsDemographicsFactory().InitFromReader(reader);
                    }
                    else
                    {
                        return null;
                    }

                    if (reader.NextResult())
                    {
                        result.MilitaryExperience = new List<LookupValue>();

                        while (reader.Read())
                        {
                            result.MilitaryExperience.Add(
                                new LookupValue
                                {
                                    Id = reader.Get<int>("ID", 0),
                                    Name = reader.Get<string>("Name"),
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

        public bool UpdateManualEntries(BhsDemographics model)
        {
            string sql = @"
UPDATE [dbo].[BhsDemographics]
SET 
[BhsStaffNameCompleted] = @BhsStaffNameCompleted
,[CompleteDate] = @CompleteDate
,[RaceID] = @RaceID
,[GenderID] = @GenderID
,[SexualOrientationID] = @SexualOrientationID
,[TribalAffiliation] = @TribalAffiliation
,[MaritalStatusID] = @MaritalStatusID
,[EducationLevelID] = @EducationLevelID
,[LivingOnReservationID] = @LivingOnReservationID
,[CountyOfResidence] = @CountyOfResidence
,[MilitaryExperienceID] = @MilitaryExperienceID
WHERE ID = @ID
";

            ClearParameters();

            AddParameter("@ID", DbType.Int64).Value = model.ID;
            AddParameter("@BhsStaffNameCompleted", DbType.String, 128).Value = SqlParameterSafe(model.BhsStaffNameCompleted);
            AddParameter("@CompleteDate", DbType.DateTimeOffset).Value = SqlParameterSafe(model.CompleteDate);

            AddParameter("@RaceID", DbType.Int32).Value = SqlParameterSafe(model.Race?.Id);
            AddParameter("@GenderID", DbType.Int32).Value = SqlParameterSafe(model.Gender?.Id);

            AddParameter("@SexualOrientationID", DbType.Int32).Value = SqlParameterSafe(model.SexualOrientation?.Id);
            AddParameter("@TribalAffiliation", DbType.String, 128).Value = SqlParameterSafe(model.TribalAffiliation);
            AddParameter("@MaritalStatusID", DbType.Int32).Value = SqlParameterSafe(model.MaritalStatus?.Id);
            AddParameter("@EducationLevelID", DbType.Int32).Value = SqlParameterSafe(model.EducationLevel?.Id);
            AddParameter("@LivingOnReservationID", DbType.Int32).Value = SqlParameterSafe(model.LivingOnReservation?.Id);
            AddParameter("@CountyOfResidence", DbType.String, 128).Value = SqlParameterSafe(model.CountyOfResidence);
            AddParameter("@MilitaryExperienceID", DbType.AnsiString, 32).Value = SqlParameterSafe(model.MilitaryExperience.ToSqlCsv());

            try
            {
                Connect();

                return RunNonSelectQuery(sql) > 0;

            }
            finally
            {
                Disconnect();
            }
        }

        public List<BhsIndicatorReportByAgeItem> GetDemographicsReportUniquePatientsByAge(string tableName, int? locationIdFilter, DateTime? startDate, DateTime? endDate)
        {
            return GetDemographicsReportPatientsByAge(tableName, locationIdFilter, startDate, endDate);

        }

        public List<BhsIndicatorReportByAgeItem> GetDemographicsReportPatientsByAge(string tableName, int? locationIdFilter, DateTime? startDate, DateTime? endDate)
        {

            var reportItems = new List<BhsIndicatorReportByAgeItem>();
            QueryBuilder withSqlBuilder;

            if (tableName == "MilitaryExperience")
            {
                withSqlBuilder = new QueryBuilder(@"
SELECT DISTINCT 
	l.ID
    ,d.ID AS PatientID
    ,[dbo].[fn_GetAge](d.Birthday) as Age
FROM dbo.BhsVisit vst 
	INNER JOIN dbo.ScreeningResult r ON vst.ScreeningResultID = r.ScreeningResultID
	INNER JOIN dbo.BhsDemographics d  ON d.Birthday = r.Birthday AND d.PatientName = r.PatientName
	OUTER APPLY (SELECT ID FROM dbo.MilitaryExperience l WHERE l.ID IN (SELECT value from dbo.fn_IntListToTable(d.MilitaryExperienceID))) as l
");
            }

            else
            {
                withSqlBuilder = new QueryBuilder(@"
SELECT DISTINCT 
	l.ID
    ,d.ID AS PatientID
    ,[dbo].[fn_GetAge](d.Birthday) as Age
FROM dbo.BhsVisit vst 
	INNER JOIN dbo.ScreeningResult r ON vst.ScreeningResultID = r.ScreeningResultID
	INNER JOIN dbo.BhsDemographics d  ON d.Birthday = r.Birthday AND d.PatientName = r.PatientName
	INNER JOIN dbo.{0} l ON l.ID = d.{0}ID
".FormatWith(tableName));
            }
            CommandObject.Parameters.Clear();
            withSqlBuilder.AppendWhereCondition("d.CompleteDate IS NOT NULL", ClauseType.And);


            if (locationIdFilter.HasValue)
            {
                withSqlBuilder.AppendWhereCondition("d.LocationID = @LocationID", ClauseType.And);
                AddParameter("@LocationID", DbType.Int32).Value = locationIdFilter.Value;

            }
            if (startDate.HasValue)
            {
                withSqlBuilder.AppendWhereCondition("vst.CreatedDate >= @StartDate", ClauseType.And);

                AddParameter("@StartDate", DbType.DateTime).Value = startDate.Value;

            }
            if (endDate.HasValue)
            {
                withSqlBuilder.AppendWhereCondition("vst.CreatedDate < @EndDate", ClauseType.And);

                AddParameter("@EndDate", DbType.DateTime).Value = endDate.Value;

            }
            string sql = string.Format(@"
WITH tblResults(ID, PatientID, Age) AS
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
        public List<BhsDemographics> GetAllItems(BhsSearchFilterModel filter)
        {
            var result = new List<BhsDemographics>();

            string sql = @"SELECT * FROM dbo.vBhsDemographics t ";
            CommandObject.Parameters.Clear();

            QueryBuilder qb = new QueryBuilder(sql);

            qb.AppendOrderCondition("t.CreatedDate", OrderType.Desc);

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


            try
            {
                using (var reader = RunSelectQuery(qb.ToString()))
                {
                    while (reader.Read())
                    {
                        result.Add(new BhsDemographicsFactory().InitFromReader(reader));
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

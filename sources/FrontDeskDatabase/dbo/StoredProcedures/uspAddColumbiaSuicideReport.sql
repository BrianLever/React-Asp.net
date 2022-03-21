CREATE PROCEDURE [dbo].[uspAddColumbiaSuicideReport]
    @ID bigint OUTPUT 
    ,@FirstName nvarchar(128)
    ,@LastName nvarchar(128)
    ,@MiddleName nvarchar(128)
    ,@Birthday date
    ,@StreetAddress nvarchar(512)
    ,@City nvarchar(255)
    ,@StateID char(2)
    ,@ZipCode char(5)
    ,@Phone char(14)
    ,@BranchLocationID int
    ,@EhrPatientID int
    ,@EhrPatientHRN nvarchar(255)

    ,@ScreeningResultID bigint
    ,@BhsVisitID bigint
    ,@CreatedDate datetimeoffset(7)
    ,@BhsStaffNameCompleted nvarchar(128)
    ,@CompleteDate datetimeoffset(7)
    ,@ScoreLevel int
    ,@LifetimeMostSevereIdeationLevel int
    ,@LifetimeMostSevereIdeationDescription nvarchar(max)
    ,@RecentMostSevereIdeationLevel int
    ,@RecentMostSevereIdeationDescription nvarchar(max)
    ,@SuicideMostRecentAttemptDate datetime
    ,@SuicideMostLethalRecentAttemptDate datetime
    ,@SuicideFirstAttemptDate datetime
    ,@MedicalDamageMostRecentAttemptCode int
    ,@MedicalDamageMostLethalAttemptCode int
    ,@MedicalDamageFirstAttemptCode int
    ,@PotentialLethalityMostRecentAttemptCode int
    ,@PotentialLethalityMostLethalAttemptCode int
    ,@PotentialLethalityFirstAttemptCode int
AS
    INSERT dbo.ColumbiaSuicideReport (
    FirstName
    ,LastName
    ,MiddleName
    ,Birthday
    ,StreetAddress
    ,City
    ,StateID
    ,ZipCode
    ,Phone
    ,BranchLocationID
    ,EhrPatientID
    ,EhrPatientHRN
    ,[ScreeningResultID]
    ,[BhsVisitID]
    ,[CreatedDate]
    ,[BhsStaffNameCompleted]
    ,[CompleteDate]
    ,[ScoreLevel]
    ,[LifetimeMostSevereIdeationLevel]
    ,[LifetimeMostSevereIdeationDescription]
    ,[RecentMostSevereIdeationLevel]
    ,[RecentMostSevereIdeationDescription]
    ,[SuicideMostRecentAttemptDate]
    ,[SuicideMostLethalRecentAttemptDate]
    ,[SuicideFirstAttemptDate]
    ,[MedicalDamageMostRecentAttemptCode]
    ,[MedicalDamageMostLethalAttemptCode]
    ,[MedicalDamageFirstAttemptCode]
    ,[PotentialLethalityMostRecentAttemptCode]
    ,[PotentialLethalityMostLethalAttemptCode]
    ,[PotentialLethalityFirstAttemptCode]

    ) VALUES (
     @FirstName
    ,@LastName
    ,@MiddleName
    ,@Birthday
    ,@StreetAddress
    ,@City
    ,@StateID
    ,@ZipCode
    ,@Phone
    ,@BranchLocationID
    ,@EhrPatientID
    ,@EhrPatientHRN
    ,@ScreeningResultID
    ,@BhsVisitID
    ,@CreatedDate
    ,@BhsStaffNameCompleted
    ,@CompleteDate
    ,@ScoreLevel
    ,@LifetimeMostSevereIdeationLevel
    ,@LifetimeMostSevereIdeationDescription
    ,@RecentMostSevereIdeationLevel
    ,@RecentMostSevereIdeationDescription
    ,@SuicideMostRecentAttemptDate
    ,@SuicideMostLethalRecentAttemptDate
    ,@SuicideFirstAttemptDate
    ,@MedicalDamageMostRecentAttemptCode
    ,@MedicalDamageMostLethalAttemptCode
    ,@MedicalDamageFirstAttemptCode
    ,@PotentialLethalityMostRecentAttemptCode
    ,@PotentialLethalityMostLethalAttemptCode
    ,@PotentialLethalityFirstAttemptCode
)


    SET @ID = SCOPE_IDENTITY();

RETURN 0
GO


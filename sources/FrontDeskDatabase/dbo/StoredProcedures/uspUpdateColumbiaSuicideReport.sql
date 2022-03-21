CREATE PROCEDURE dbo.uspUpdateColumbiaSuicideReport
    @ID bigint
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
    UPDATE dbo.ColumbiaSuicideReport SET
  
         FirstName = @FirstName
        ,LastName = @LastName
        ,MiddleName = @MiddleName
        ,Birthday = @Birthday
        ,StreetAddress = @StreetAddress
        ,City = @City
        ,StateID = @StateID
        ,ZipCode = @ZipCode
        ,Phone = @Phone
        ,BranchLocationID = @BranchLocationID
        ,EhrPatientID = @EhrPatientID
        ,EhrPatientHRN = @EhrPatientHRN

        ,BhsStaffNameCompleted = @BhsStaffNameCompleted
        ,CompleteDate = @CompleteDate
        ,ScoreLevel = @ScoreLevel
        ,LifetimeMostSevereIdeationLevel = @LifetimeMostSevereIdeationLevel
        ,LifetimeMostSevereIdeationDescription = @LifetimeMostSevereIdeationDescription
        ,RecentMostSevereIdeationLevel = @RecentMostSevereIdeationLevel
        ,RecentMostSevereIdeationDescription = @RecentMostSevereIdeationDescription
        ,SuicideMostRecentAttemptDate = @SuicideMostRecentAttemptDate
        ,SuicideMostLethalRecentAttemptDate = @SuicideMostLethalRecentAttemptDate
        ,SuicideFirstAttemptDate = @SuicideFirstAttemptDate
        ,MedicalDamageMostRecentAttemptCode = @MedicalDamageMostRecentAttemptCode
        ,MedicalDamageMostLethalAttemptCode = @MedicalDamageMostLethalAttemptCode
        ,MedicalDamageFirstAttemptCode = @MedicalDamageFirstAttemptCode
        ,PotentialLethalityMostRecentAttemptCode = @PotentialLethalityMostRecentAttemptCode
        ,PotentialLethalityMostLethalAttemptCode = @PotentialLethalityMostLethalAttemptCode
        ,PotentialLethalityFirstAttemptCode = @PotentialLethalityFirstAttemptCode
    WHERE ID = @ID
GO


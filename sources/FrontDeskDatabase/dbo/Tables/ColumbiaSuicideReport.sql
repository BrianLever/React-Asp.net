CREATE TABLE [dbo].[ColumbiaSuicideReport]
(
    ID bigint NOT NULL IDENTITY(1,1),
    
    /* patient info */
    FirstName nvarchar(128) NOT NULL,
    LastName nvarchar(128) NOT NULL,
    MiddleName nvarchar(128) NULL,
    Birthday date NOT NULL,
    StreetAddress nvarchar(512) NULL,
    City nvarchar(255)	NULL,
    StateID char(2) NULL,
    ZipCode char(5) NULL,
    Phone char(14) NULL,
    BranchLocationID int NOT NULL, /*foreign key*/
    PatientName as dbo.fn_GetPatientName(LastName, FirstName, MiddleName) PERSISTED,
    
    ScreeningResultID bigint NULL, /*foreign key*/
    BhsVisitID bigint NULL, /*foreign key*/
    EhrPatientID int null,
    EhrPatientHRN nvarchar(255) null,


    CreatedDate DateTimeOffset NOT NULL,
    BhsStaffNameCompleted nvarchar(128) NULL,
    CompleteDate datetimeoffset NULL,


    ScoreLevel int NULL, /* Risk Level */

    /* INTENSITY OF IDEATION */
    LifetimeMostSevereIdeationLevel int NULL, 
    LifetimeMostSevereIdeationDescription nvarchar(max) NULL,
    
    RecentMostSevereIdeationLevel int NULL, 
    RecentMostSevereIdeationDescription nvarchar(max) NULL,
    

    /* SUICIDE BEHAVIOR */

    SuicideMostRecentAttemptDate datetime NULL,
    SuicideMostLethalRecentAttemptDate datetime NULL,
    SuicideFirstAttemptDate datetime NULL, 
    MedicalDamageMostRecentAttemptCode int NULL, /* 0 - 5 */
    MedicalDamageMostLethalAttemptCode int NULL, /* 0 - 5 */
    MedicalDamageFirstAttemptCode int NULL, /* 0 - 5 */

    PotentialLethalityMostRecentAttemptCode int NULL, /* 0 - 2 */
    PotentialLethalityMostLethalAttemptCode int NULL, /* 0 - 2 */
    PotentialLethalityFirstAttemptCode int NULL, /* 0 - 2 */
    
    

    CONSTRAINT PK__ColumbiaSuicideReport PRIMARY KEY CLUSTERED (ID),

    CONSTRAINT FK__ColumbiaSuicideReport__BranchLocation FOREIGN KEY (BranchLocationID)
        REFERENCES dbo.BranchLocation(BranchLocationID) ON UPDATE NO ACTION ON DELETE NO ACTION,

    CONSTRAINT FK__ColumbiaSuicideReport__ScreeningResult FOREIGN KEY (ScreeningResultID)
        REFERENCES dbo.ScreeningResult(ScreeningResultID) ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT FK__ColumbiaSuicideReport__BhsVisit FOREIGN KEY (BhsVisitID)
        REFERENCES dbo.BhsVisit(ID) ON UPDATE NO ACTION ON DELETE NO ACTION,

    CONSTRAINT FK__ColumbiaSuicideReport__ColumbiaSuicideRiskScoreLevel FOREIGN KEY (ScoreLevel)
        REFERENCES dbo.ColumbiaSuicideRiskScoreLevel(ScoreLevel) ON UPDATE NO ACTION ON DELETE NO ACTION,

);
GO




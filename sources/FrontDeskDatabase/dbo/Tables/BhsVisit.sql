CREATE TABLE [dbo].[BhsVisit]
(
    ID bigint NOT NULL IDENTITY(1,1),
    ScreeningResultID bigint NOT NULL, /*foreign key*/
    LocationID int NOT NULL, /*foreign key*/
    CreatedDate DateTimeOffset NOT NULL,
    ScreeningDate DateTimeOffset NOT NULL,
    TobacoExposureSmokerInHomeFlag bit NOT NULL,
    TobacoExposureCeremonyUseFlag bit NOT NULL,
    TobacoExposureSmokingFlag bit NOT NULL,
    TobacoExposureSmoklessFlag bit NOT NULL,
    
    AlcoholUseFlagScoreLevel int NULL,
    AlcoholUseFlagScoreLevelLabel nvarchar(64) NULL,

    SubstanceAbuseFlagScoreLevel int NULL,
    SubstanceAbuseFlagScoreLevelLabel nvarchar(64) NULL,

    DepressionFlagScoreLevel int NULL,
    DepressionFlagScoreLevelLabel nvarchar(64) NULL,
    DepressionThinkOfDeathAnswer nvarchar(64) NULL,

    PartnerViolenceFlagScoreLevel int NULL,
    PartnerViolenceFlagScoreLevelLabel nvarchar(64) NULL,

    NewVisitReferralRecommendationID int NULL, /*foreign key*/
    NewVisitReferralRecommendationDescription nvarchar(max) NULL,

    NewVisitReferralRecommendationAcceptedID int NULL, /*foreign key*/

    ReasonNewVisitReferralRecommendationNotAcceptedID int NULL, /*foreign key*/

    NewVisitDate DateTimeOffset NULL,

    DischargedID int NULL, /*foreign key*/

    ThirtyDatyFollowUpFlag bit NOT NULL,

    Notes nvarchar(max) NULL,

    BhsStaffNameCompleted nvarchar(128) NULL,
    CompleteDate datetimeoffset NULL,

    TreatmentAction1ID int NULL,
    TreatmentAction1Description nvarchar(max) NULL,
    TreatmentAction2ID int NULL,
    TreatmentAction2Description nvarchar(max) NULL,
    TreatmentAction3ID int NULL,
    TreatmentAction3Description nvarchar(max) NULL,
    TreatmentAction4ID int NULL,
    TreatmentAction4Description nvarchar(max) NULL,
    TreatmentAction5ID int NULL,
    TreatmentAction5Description nvarchar(max) NULL,

    OtherScreeningTools xml NULL,
    FollowUpDate DateTimeOffset NULL,

    /* Additional Screening Tools */
    AnxietyFlagScoreLevel int NULL,
    AnxietyFlagScoreLevelLabel nvarchar(64) NULL,
    ProblemGamblingFlagScoreLevel int NULL,
    ProblemGamblingFlagScoreLevelLabel nvarchar(64) NULL,

    CONSTRAINT PK_BhsVisit PRIMARY KEY(ID),
    CONSTRAINT FK_BhsVisit__ScreeningResult FOREIGN KEY (ScreeningResultID)
        REFERENCES dbo.ScreeningResult(ScreeningResultID),
    CONSTRAINT FK_BhsVisit__BranchLocation FOREIGN KEY (LocationID)
        REFERENCES dbo.BranchLocation(BranchLocationID)
        ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT FK_BhsVisit__NewVisitReferralRecommendation 
        FOREIGN KEY (NewVisitReferralRecommendationID)
        REFERENCES dbo.NewVisitReferralRecommendation(ID)
        ON UPDATE NO ACTION ON DELETE NO ACTION,
     CONSTRAINT FK_BhsVisit__NewVisitReferralRecommendationAccepted 
        FOREIGN KEY (NewVisitReferralRecommendationAcceptedID)
        REFERENCES dbo.NewVisitReferralRecommendationAccepted(ID)
        ON UPDATE NO ACTION ON DELETE NO ACTION,
    
     CONSTRAINT FK_BhsVisit__ReasonNewVisitReferralRecommendationNotAccepted 
        FOREIGN KEY (ReasonNewVisitReferralRecommendationNotAcceptedID)
        REFERENCES dbo.ReasonNewVisitReferralRecommendationNotAccepted(ID)
        ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT FK_BhsVisit__Discharged 
        FOREIGN KEY (DischargedID)
        REFERENCES dbo.Discharged(ID)
        ON UPDATE NO ACTION ON DELETE NO ACTION,

    CONSTRAINT FK_BhsVisit__TreatmentAction1 
        FOREIGN KEY (TreatmentAction1ID)
        REFERENCES dbo.TreatmentAction(ID)
        ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT FK_BhsVisit__TreatmentAction2 
        FOREIGN KEY (TreatmentAction2ID)
        REFERENCES dbo.TreatmentAction(ID)
        ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT FK_BhsVisit__TreatmentAction3 
        FOREIGN KEY (TreatmentAction3ID)
        REFERENCES dbo.TreatmentAction(ID)
        ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT FK_BhsVisit__TreatmentAction4
        FOREIGN KEY (TreatmentAction4ID)
        REFERENCES dbo.TreatmentAction(ID)
        ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT FK_BhsVisit__TreatmentAction5
        FOREIGN KEY (TreatmentAction5ID)
        REFERENCES dbo.TreatmentAction(ID)
        ON UPDATE NO ACTION ON DELETE NO ACTION
);
GO

create index IX__BhsVisit_ScreeningResultID ON dbo.BhsVisit(ScreeningResultID)
GO


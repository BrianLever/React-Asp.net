CREATE TABLE [dbo].[BhsFollowUp]
(
    ID bigint NOT NULL IDENTITY(1,1),
	ScreeningResultID bigint NOT NULL, /*foreign key*/
    BhsVisitID bigint NOT NULL, /*foreign key*/
	VisitDate DateTimeOffset NOT NULL,
    CreatedDate DateTimeOffset NOT NULL,
    BhsStaffNameCompleted nvarchar(128) NULL,
    CompleteDate datetimeoffset NULL,

    PatientAttendedVisitID int NULL, /*foreign key*/
    FollowUpContactDate datetimeoffset NULL,
    FollowUpContactOutcomeID int NULL,/*foreign key*/

    NewVisitReferralRecommendationID int NULL, /*foreign key*/
    NewVisitReferralRecommendationDescription nvarchar(max) NULL,

    NewVisitReferralRecommendationAcceptedID int NULL, /*foreign key*/
    ReasonNewVisitReferralRecommendationNotAcceptedID int NULL, /*foreign key*/

    NewVisitDate DateTimeOffset NULL,
    DischargedID int NULL, /*foreign key*/
    ThirtyDatyFollowUpFlag bit NOT NULL,

    Notes nvarchar(max) NULL,
    ParentFollowUpID bigint NULL, /* Id of the other Follow-Up report from which this was created using 30-Day Follow-Up flag*/
    FollowUpDate DateTimeOffset NOT NULL,
    NewFollowUpDate DateTimeOffset NOT NULL,

    CONSTRAINT PK_BhsFollowUp PRIMARY KEY(ID),
	CONSTRAINT FK_BhsFollowUp__ScreeningResult FOREIGN KEY (ScreeningResultID)
		REFERENCES dbo.ScreeningResult(ScreeningResultID) ON UPDATE NO ACTION ON DELETE NO ACTION,
	CONSTRAINT FK_BhsFollowUp__BhsVisit FOREIGN KEY (BhsVisitID)
		REFERENCES dbo.BhsVisit(ID) ON UPDATE NO ACTION ON DELETE NO ACTION,

    CONSTRAINT FK_BhsFollowUp__PatientAttendedVisit FOREIGN KEY (PatientAttendedVisitID)
		REFERENCES dbo.PatientAttendedVisit(ID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,

     CONSTRAINT FK_BhsFollowUp__FollowUpContactOutcome FOREIGN KEY (FollowUpContactOutcomeID)
		REFERENCES dbo.FollowUpContactOutcome(ID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,

    CONSTRAINT FK_BhsFollowUp__NewVisitReferralRecommendation 
        FOREIGN KEY (NewVisitReferralRecommendationID)
		REFERENCES dbo.NewVisitReferralRecommendation(ID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,
     CONSTRAINT FK_BhsFollowUp__NewVisitReferralRecommendationAccepted 
        FOREIGN KEY (NewVisitReferralRecommendationAcceptedID)
		REFERENCES dbo.NewVisitReferralRecommendationAccepted(ID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,
    
     CONSTRAINT FK_BhsFollowUp__ReasonNewVisitReferralRecommendationNotAccepted 
        FOREIGN KEY (ReasonNewVisitReferralRecommendationNotAcceptedID)
		REFERENCES dbo.ReasonNewVisitReferralRecommendationNotAccepted(ID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT FK_BhsFollowUp__Discharged 
        FOREIGN KEY (DischargedID)
		REFERENCES dbo.Discharged(ID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT FK_BhsFollowUp__BhsFollowUp 
        FOREIGN KEY (ParentFollowUpID)
		REFERENCES dbo.BhsFollowUp(ID)
		ON UPDATE NO ACTION ON DELETE NO ACTION
);
GO

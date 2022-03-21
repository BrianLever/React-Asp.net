CREATE TABLE [dbo].[BhsDemographics]
(
    ID bigint NOT NULL IDENTITY(1,1),
    ScreeningResultID bigint NULL, /*foreign key*/
    LocationID int NOT NULL, /*foreign key*/
    CreatedDate DateTimeOffset NOT NULL,
    ScreeningDate DateTimeOffset NOT NULL,
    BhsStaffNameCompleted nvarchar(128) NULL,
    CompleteDate datetimeoffset NULL,

    FirstName nvarchar(128) NOT NULL,
	LastName nvarchar(128) NOT NULL,
	MiddleName nvarchar(128) NULL,
	Birthday date NOT NULL,
	StreetAddress nvarchar(512) NULL,
	City nvarchar(255)	NULL,
	StateID char(2) NULL,
	ZipCode char(5) NULL,
	Phone char(14) NULL,
    RaceID int NULL,
    GenderID int NULL,
    SexualOrientationID int NULL,
    TribalAffiliation nvarchar(128) NULL,
    MaritalStatusID int NULL,
    EducationLevelID int NULL,
    LivingOnReservationID int NULL,
    CountyOfResidence nvarchar(128) NULL,
    MilitaryExperienceID varchar(32) NULL,
    ExportedToHRN nvarchar(255) null,
    PatientName as dbo.fn_GetPatientName(LastName, FirstName, MiddleName) PERSISTED,
    CONSTRAINT PK_BhsDemographics PRIMARY KEY(ID),
	CONSTRAINT FK_BhsDemographics__ScreeningResult FOREIGN KEY (ScreeningResultID)
		REFERENCES dbo.ScreeningResult(ScreeningResultID)
        ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT FK_BhsDemographics__BranchLocation FOREIGN KEY (LocationID)
		REFERENCES dbo.BranchLocation(BranchLocationID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT FK_BhsDemographics__Race FOREIGN KEY (RaceID)
		REFERENCES dbo.Race(ID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT FK_BhsDemographics__Gender FOREIGN KEY (GenderID)
		REFERENCES dbo.Gender(ID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT FK_BhsDemographics__SexualOrientation FOREIGN KEY (SexualOrientationID)
		REFERENCES dbo.SexualOrientation(ID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT FK_BhsDemographics__MaritalStatus FOREIGN KEY (MaritalStatusID)
		REFERENCES dbo.MaritalStatus(ID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT FK_BhsDemographics__EducationLevel FOREIGN KEY (EducationLevelID)
		REFERENCES dbo.EducationLevel(ID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT FK_BhsDemographics__LivingOnReservation FOREIGN KEY (LivingOnReservationID)
		REFERENCES dbo.LivingOnReservation(ID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT CK_BhsDemographics__MilitaryExperience 
        CHECK([dbo].[fn_CheckMilitaryExperienceValues](MilitaryExperienceID) = 1)
 )
GO

--CREATE INDEX IX__BhsDemographics__PatientName
--    ON [dbo].[BhsDemographics]
--    (PatientName) INCLUDE(Birthday)
--GO

create index IX__BhsDemographics_Birthday ON dbo.BhsDemographics(Birthday) INCLUDE(PatientName, CreatedDate, ScreeningDate, CompleteDate)
GO



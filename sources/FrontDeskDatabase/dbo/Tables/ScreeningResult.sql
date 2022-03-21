CREATE TABLE dbo.ScreeningResult
(
	ScreeningResultID bigint NOT NULL IDENTITY(1,1),
	ScreeningID char(4) NOT NULL,
	FirstName nvarchar(128) NOT NULL,
	LastName nvarchar(128) NOT NULL,
	MiddleName nvarchar(128) NULL,
	Birthday date NOT NULL,
	StreetAddress nvarchar(512) NULL,
	City nvarchar(255)	NULL,
	StateID char(2) NULL,
	ZipCode char(5) NULL,
	Phone char(14) NULL,
	KioskID smallint NOT NULL,
	CreatedDate datetimeoffset NOT NULL,
	IsDeleted bit NOT NULL CONSTRAINT DF_ScreeningResult_IsDeleted DEFAULT(0),
	DeletedDate datetimeoffset NULL,
	DeletedBy int NULL,
	WithErrors bit NOT NULL CONSTRAINT DF_ScreeningResult_WithErrors DEFAULT(0),
	ExportDate DateTimeOffset null,
    ExportedBy int null,
    ExportedToPatientID int null,
    ExportedToHRN nvarchar(255) null,
    ExportedToVisitID int null,
    ExportedToVisitDate datetime null,
    ExportedToVisitLocation nvarchar(255) null,
    PatientName as dbo.fn_GetPatientName(LastName, FirstName, MiddleName) PERSISTED,
	CONSTRAINT PK_ScreeningResult PRIMARY KEY(ScreeningResultID),
	CONSTRAINT FK_ScreeningResult__Screening FOREIGN KEY (ScreeningID)
		REFERENCES dbo.Screening(ScreeningID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,
	CONSTRAINT FK_ScreeningResult__State FOREIGN KEY(StateID) 
		REFERENCES dbo.State(StateCode) 
		ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT FK_ScreeningResult__Users FOREIGN KEY(DeletedBy) 
		REFERENCES dbo.Users(PKID) 
		ON DELETE SET NULL ON UPDATE CASCADE,
	CONSTRAINT FK_ScreeningResult__Kiosk FOREIGN KEY(KioskID) 
		REFERENCES dbo.Kiosk(KioskID) 
		ON DELETE NO ACTION ON UPDATE CASCADE
)
GO
CREATE INDEX IX_ScreeningResult_CreatedDate ON dbo.ScreeningResult(CreatedDate DESC) INCLUDE(FirstName, LastName, MiddleName, Birthday, ScreeningID)
	WHERE IsDeleted = 0;
GO
CREATE INDEX IX_ScreeningResult_IsDeleted ON dbo.ScreeningResult(IsDeleted) INCLUDE(FirstName, LastName, MiddleName, Birthday, CreatedDate, ScreeningID, KioskID )
	Where IsDeleted = 0
GO


CREATE NONCLUSTERED INDEX IX__ScreeningResult_Birthday
ON [dbo].[ScreeningResult] ([Birthday])

GO

CREATE NONCLUSTERED INDEX IX__ScreeningResult_ExportDate
ON [dbo].[ScreeningResult] ([IsDeleted],[ExportDate])


--CREATE INDEX IX_ScreeningResult_PatientName
--    ON [dbo].[ScreeningResult]
--    (PatientName) INCLUDE(Birthday, IsDeleted)
--GO
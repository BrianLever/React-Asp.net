IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'ScreeningSectionQuestionResult')
	BEGIN
		DROP  Table dbo.ScreeningSectionQuestionResult
	END
GO


IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'ScreeningSectionResult')
	BEGIN
		DROP  Table dbo.ScreeningSectionResult
	END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'ScreeningResult')
	BEGIN
		DROP  Table dbo.ScreeningResult
	END
GO

CREATE TABLE dbo.ScreeningResult
(
	ScreeningResultID bigint NOT NULL IDENTITY(1,1),
	ScreeningID char(4) NOT NULL,
	FirstName nvarchar(128) NOT NULL,
	LastName nvarchar(128) NOT NULL,
	MiddleName nvarchar(128) NULL,
	Birthday date NOT NULL,
	StreetAddress nvarchar(512) NOT NULL,
	City nvarchar(255)	NOT NULL,
	StateID char(2) NOT NULL,
	ZipCode char(5) NOT NULL,
	Phone char(12) NOT NULL,
	KioskID uniqueidentifier NOT NULL,
	CreatedDate datetimeoffset NOT NULL,
	IsDeleted bit NOT NULL CONSTRAINT DF_ScreeningResult_IsDeleted DEFAULT(0),
	DeletedDate datetimeoffset NULL,
	DeletedBy int NULL,
	CONSTRAINT PK_ScreeningResult PRIMARY KEY(ScreeningResultID),
	CONSTRAINT FK_ScreeningResult__Screening FOREIGN KEY (ScreeningID)
		REFERENCES dbo.Screening(ScreeningID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,
	CONSTRAINT FK_ScreeningResult__State FOREIGN KEY(StateID) 
		REFERENCES dbo.State(StateCode) 
		ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT FK_ScreeningResult__Users FOREIGN KEY(DeletedBy) 
		REFERENCES dbo.Users(PKID) 
		ON DELETE SET NULL ON UPDATE CASCADE
)
GO


GRANT SELECT, INSERT, UPDATE ON dbo.ScreeningResult TO frontdesk_appuser

GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'ScreeningSectionResult')
	BEGIN
		DROP  Table dbo.ScreeningSectionResult
	END
GO

CREATE TABLE dbo.ScreeningSectionResult
(
	ScreeningResultID bigint NOT NULL,
	ScreeningSectionID char(5) NOT NULL,
	AnswerValue int NOT NULL,
	Score int NULL,
	ScoreLevel int NULL,
	CONSTRAINT PK_ScreeningSectionResult PRIMARY KEY(ScreeningResultID, ScreeningSectionID),
	CONSTRAINT FK_ScreeningSectionResult__ScreeningResult FOREIGN KEY(ScreeningResultID)
		REFERENCES dbo.ScreeningResult(ScreeningResultID) ON UPDATE NO ACTION ON DELETE CASCADE,
	CONSTRAINT FK_ScreeningSectionResult__ScreeningScoreLevel FOREIGN KEY(ScreeningSectionID, ScoreLevel)
		REFERENCES dbo.ScreeningScoreLevel(ScreeningSectionID, ScoreLevel) 
		ON UPDATE NO ACTION ON DELETE CASCADE
)
GO

GRANT SELECT, INSERT ON dbo.ScreeningSectionResult TO frontdesk_appuser

GO



IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'ScreeningSectionQuestionResult')
	BEGIN
		DROP  Table dbo.ScreeningSectionQuestionResult
	END
GO

CREATE TABLE dbo.ScreeningSectionQuestionResult
(
	ScreeningResultID bigint NOT NULL,
	ScreeningSectionID char(5) NOT NULL,
	QuestionID int NOT NULL,
	AnswerValue int NOT NULL,
	CONSTRAINT PK_ScreeningSectionQuestionResult PRIMARY KEY(ScreeningResultID, ScreeningSectionID, QuestionID),
	CONSTRAINT FK_ScreeningSectionQuestionResult__ScreeningSectionResult FOREIGN KEY(ScreeningResultID, ScreeningSectionID)
		REFERENCES dbo.ScreeningSectionResult(ScreeningResultID, ScreeningSectionID)
		ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT FK_ScreeningSectionQuestionResult__ScreeningSectionQuestion FOREIGN KEY(ScreeningSectionID, QuestionID)
		REFERENCES dbo.ScreeningSectionQuestion(ScreeningSectionID, QuestionID)
		ON DELETE NO ACTION ON UPDATE NO ACTION
	
)
GO


GRANT SELECT, INSERT ON dbo.ScreeningSectionQuestionResult TO frontdesk_appuser

GO


 
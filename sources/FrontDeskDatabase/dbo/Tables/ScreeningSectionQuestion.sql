CREATE TABLE dbo.ScreeningSectionQuestion
( 
	QuestionID int NOT NULL,
	ScreeningSectionID char(5) NOT NULL,
	PreambleText nvarchar(max) NULL,
	QuestionText nvarchar(max) NOT NULL,
	AnswerScaleID int NOT NULL,
    IsMainQuestion bit NOT NULL,
    OrderIndex int NOT NULL,
	OnlyWhenPossitive bit NOT NULL 
		CONSTRAINT DF_ScreeningSectionQuestion_OnlyWhenPossitive DEFAULT (0),
	CONSTRAINT PK_ScreeningSectionQuestion PRIMARY KEY (ScreeningSectionID, QuestionID),
	CONSTRAINT FK_ScreeningSectionQuestion__ScreeningSection FOREIGN KEY(ScreeningSectionID)
		REFERENCES dbo.ScreeningSection(ScreeningSectionID) ON UPDATE NO ACTION ON DELETE NO ACTION,
	CONSTRAINT FK_ScreeningSectionQuestion__AnswerScaleID FOREIGN KEY(AnswerScaleID)
		REFERENCES dbo.AnswerScale(AnswerScaleID) ON UPDATE NO ACTION ON DELETE NO ACTION
)
GO

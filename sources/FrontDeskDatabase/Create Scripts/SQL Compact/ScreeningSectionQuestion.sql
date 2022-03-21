CREATE TABLE ScreeningSectionQuestion
( 
	QuestionID int NOT NULL,
	ScreeningSectionID nvarchar(5) NOT NULL,
	PreambleText nvarchar(4000) NULL,
	QuestionText nvarchar(4000) NOT NULL,
	AnswerScaleID int NOT NULL,
    IsMainQuestion bit NOT NULL,
    OrderIndex int NOT NULL,
	OnlyWhenAllPossitive bit NOT NULL 
		CONSTRAINT DF_ScreeningSectionQuestion_OnlyWhenAllPossitive DEFAULT (0),
	CONSTRAINT PK_ScreeningSectionQuestion PRIMARY KEY (ScreeningSectionID, QuestionID),
	CONSTRAINT FK_ScreeningSectionQuestion__ScreeningSection FOREIGN KEY(ScreeningSectionID)
		REFERENCES ScreeningSection(ScreeningSectionID) ON UPDATE NO ACTION ON DELETE NO ACTION,
	CONSTRAINT FK_ScreeningSectionQuestion__AnswerScaleID FOREIGN KEY(AnswerScaleID)
		REFERENCES AnswerScale(AnswerScaleID) ON UPDATE NO ACTION ON DELETE NO ACTION

)
CREATE TABLE dbo.ScreeningScoreLevel
( 
	ScreeningSectionID char(5) NOT NULL,
	ScoreLevel int NOT NULL,
	[Name] nvarchar(64) NOT NULL,
	Indicates nvarchar(max) NULL,
	[Label] nvarchar(64) NOT NULL,
	CONSTRAINT PK_ScreeningScoreLevel PRIMARY KEY(ScreeningSectionID, ScoreLevel),
	CONSTRAINT FK_ScreeningScoreLevel__ScreeningSection FOREIGN KEY(ScreeningSectionID)
		REFERENCES dbo.ScreeningSection(ScreeningSectionID) ON UPDATE CASCADE ON DELETE NO ACTION
);
GO

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
    CONSTRAINT FK_ScreeningSectionResult__ScreeningSection FOREIGN KEY(ScreeningSectionID)
		REFERENCES dbo.ScreeningSection(ScreeningSectionID) ON UPDATE NO ACTION ON DELETE NO ACTION,
	CONSTRAINT FK_ScreeningSectionResult__ScreeningScoreLevel FOREIGN KEY(ScreeningSectionID, ScoreLevel)
		REFERENCES dbo.ScreeningScoreLevel(ScreeningSectionID, ScoreLevel) 
		ON UPDATE NO ACTION ON DELETE CASCADE
)
GO

CREATE NONCLUSTERED INDEX IX__ScreeningSectionResult_ScreeningSectionID
ON [dbo].[ScreeningSectionResult] ([ScreeningSectionID])
INCLUDE ([ScreeningResultID],[ScoreLevel])

GO
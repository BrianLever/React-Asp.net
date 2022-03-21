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
	
);
GO


CREATE NONCLUSTERED INDEX IX__ScreeningSectionQuestionResult_Answer
ON [dbo].[ScreeningSectionQuestionResult] ([ScreeningSectionID],[QuestionID],[AnswerValue])
INCLUDE ([ScreeningResultID])
GO

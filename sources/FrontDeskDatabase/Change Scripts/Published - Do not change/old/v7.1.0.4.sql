

MERGE INTO ScreeningSectionQuestion as Target
USING( VALUES
('PHQ-9', 9, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Thoughts that you would be BETTER OFF DEAD or of HURTING YOURSELF in some way?', 2, 0, 100),
('PHQ9A', 9, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Thoughts that you would be BETTER OFF DEAD or of HURTING YOURSELF in some way?', 2, 1, 100)
) as Source(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
	ON Target.ScreeningSectionID = Source.ScreeningSectionID AND Target.QuestionID = Source.QuestionID

WHEN MATCHED THEN
	UPDATE SET 
		PreambleText = Source.PreambleText, 
		QuestionText = Source.QuestionText,
		AnswerScaleID = Source.AnswerScaleID,
		IsMainQuestion = Source.IsMainQuestion,
		OrderIndex = Source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
	INSERT (ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
	VALUES(Source.ScreeningSectionID, Source.QuestionID, Source.PreambleText, Source.QuestionText, Source.AnswerScaleID, Source.IsMainQuestion, Source.OrderIndex)
	;
GO



---------------------------------------------
IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '7.1.0.4')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('7.1.0.4');
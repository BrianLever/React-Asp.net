
MERGE INTO ScreeningSectionQuestion as Target
USING( VALUES
('PHQ-9', 1, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Little interest or pleasure in doing things?', 2, 1, 10),
('PHQ-9', 2, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Feeling down, depressed, or hopeless?', 2, 1, 20),
('PHQ-9', 3, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Trouble falling or staying asleep, or sleeping too much?', 2, 0, 100),
('PHQ-9', 4, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Feeling tired or having little energy?', 2, 0, 100),
('PHQ-9', 5, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Poor appetite or overeating?', 2, 0, 100),
('PHQ-9', 6, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Feeling bad about yourself - or that you are a failure or have let yourself or your family down?', 2, 0, 100),
('PHQ-9', 7, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Trouble concentrating on things, such as reading the newspaper or watching television?', 2, 0, 100),
('PHQ-9', 8, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Moving or speaking so slowly that other people could have noticed. Or the opposite - being so fidgety or restless that you have been moving around a lot more than usual?', 2, 0, 100),
('PHQ-9', 9, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Thoughts that you would be BETTER OFF DEAD or of HURTING YOURSELF in some way?', 2, 0, 100),
('PHQ-9', 10, NULL, 'If you checked off ANY problems, how DIFFICULT have these problems made it for you to do your work, take care of things at home, or get along with other people?', 3, 0, 100)
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
IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '7.1.0.3')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('7.1.0.3');
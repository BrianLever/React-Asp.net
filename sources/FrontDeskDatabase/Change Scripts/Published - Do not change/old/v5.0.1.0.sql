-- SIH
INSERT INTO [dbo].[ScreeningSectionQuestionResult]
           ([ScreeningResultID]
           ,[ScreeningSectionID]
           ,[QuestionID]
           ,[AnswerValue])
SELECT 	ScreeningResultID, 'SIH', 1, r.AnswerValue
FROM dbo.ScreeningSectionResult r
WHERE ScreeningSectionID = 'SIH' AND NOT EXISTS(SELECT 1 FROM dbo.ScreeningSectionQuestionResult WHERE ScreeningResultID = r.ScreeningResultID AND ScreeningSectionID = 'SIH' and QuestionID = 1)

--TCC
INSERT INTO [dbo].[ScreeningSectionQuestionResult]
           ([ScreeningResultID]
           ,[ScreeningSectionID]
           ,[QuestionID]
           ,[AnswerValue])
SELECT 	ScreeningResultID, 'TCC', 4, r.AnswerValue
FROM dbo.ScreeningSectionResult r
WHERE ScreeningSectionID = 'TCC' AND NOT EXISTS(SELECT 1 FROM dbo.ScreeningSectionQuestionResult WHERE ScreeningResultID = r.ScreeningResultID AND ScreeningSectionID = 'TCC' and QuestionID = 4)


--CAGE
INSERT INTO [dbo].[ScreeningSectionQuestionResult]
           ([ScreeningResultID]
           ,[ScreeningSectionID]
           ,[QuestionID]
           ,[AnswerValue])
SELECT 	ScreeningResultID, 'CAGE', 5, r.AnswerValue
FROM dbo.ScreeningSectionResult r
WHERE ScreeningSectionID = 'CAGE' AND NOT EXISTS(SELECT 1 FROM dbo.ScreeningSectionQuestionResult WHERE ScreeningResultID = r.ScreeningResultID AND ScreeningSectionID = 'CAGE' and QuestionID = 5)


-- DAST
INSERT INTO [dbo].[ScreeningSectionQuestionResult]
           ([ScreeningResultID]
           ,[ScreeningSectionID]
           ,[QuestionID]
           ,[AnswerValue])
SELECT 	ScreeningResultID, 'DAST', 10, r.AnswerValue
FROM dbo.ScreeningSectionResult r
WHERE ScreeningSectionID = 'DAST' AND NOT EXISTS(SELECT 1 FROM dbo.ScreeningSectionQuestionResult WHERE ScreeningResultID = r.ScreeningResultID AND ScreeningSectionID = 'DAST' and QuestionID = 10)



-- HITS
INSERT INTO [dbo].[ScreeningSectionQuestionResult]
           ([ScreeningResultID]
           ,[ScreeningSectionID]
           ,[QuestionID]
           ,[AnswerValue])
SELECT 	ScreeningResultID, 'HITS', 5, r.AnswerValue
FROM dbo.ScreeningSectionResult r
WHERE ScreeningSectionID = 'HITS' AND NOT EXISTS(SELECT 1 FROM dbo.ScreeningSectionQuestionResult WHERE ScreeningResultID = r.ScreeningResultID AND ScreeningSectionID = 'HITS' and QuestionID = 5)


-- PHQ-9, inserting two No for the first two questions
INSERT INTO [dbo].[ScreeningSectionQuestionResult]
           ([ScreeningResultID]
           ,[ScreeningSectionID]
           ,[QuestionID]
           ,[AnswerValue])
SELECT 	ScreeningResultID, 'PHQ-9', 1, 0
FROM dbo.ScreeningSectionResult r
WHERE ScreeningSectionID = 'PHQ-9' AND AnswerValue = 0 AND NOT EXISTS(SELECT 1 FROM dbo.ScreeningSectionQuestionResult WHERE ScreeningResultID = r.ScreeningResultID AND ScreeningSectionID = 'PHQ-9' and QuestionID = 1)

INSERT INTO [dbo].[ScreeningSectionQuestionResult]
           ([ScreeningResultID]
           ,[ScreeningSectionID]
           ,[QuestionID]
           ,[AnswerValue])
SELECT 	ScreeningResultID, 'PHQ-9', 2, 0
FROM dbo.ScreeningSectionResult r
WHERE ScreeningSectionID = 'PHQ-9' AND AnswerValue = 0 AND NOT EXISTS(SELECT 1 FROM dbo.ScreeningSectionQuestionResult WHERE ScreeningResultID = r.ScreeningResultID AND ScreeningSectionID = 'PHQ-9' and QuestionID = 2)






IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '5.0.1.0')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('5.0.1.0');
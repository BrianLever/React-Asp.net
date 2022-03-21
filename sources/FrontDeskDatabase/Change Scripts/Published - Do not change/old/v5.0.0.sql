if exists (SELECT NULL FROM sys.columns where object_id = OBJECT_ID('dbo.ScreeningSectionQuestion') AND name = 'IsMainQuestion')
SET NOEXEC ON
GO

ALTER TABLE dbo.ScreeningSectionQuestion
ADD IsMainQuestion bit NULL;
GO
UPDATE dbo.ScreeningSectionQuestion SET IsMainQuestion = 0;
GO

ALTER TABLE dbo.ScreeningSectionQuestion
ALTER COLUMN IsMainQuestion bit NOT NULL;
GO
SET NOEXEC OFF
GO


if exists (SELECT NULL FROM sys.columns where object_id = OBJECT_ID('dbo.ScreeningSectionQuestion') AND name = 'OrderIndex')
SET NOEXEC ON
GO

ALTER TABLE dbo.ScreeningSectionQuestion
ADD OrderIndex int NULL;
GO


UPDATE dbo.ScreeningSectionQuestion SET OrderIndex = 100;

ALTER TABLE dbo.ScreeningSectionQuestion
ALTER COLUMN OrderIndex int NOT NULL;
GO

SET NOEXEC OFF
GO

---------------------------------------------
--- INSERT QUESTIONS
---------------------------------------------

MERGE INTO ScreeningSectionQuestion as Target
USING( VALUES
('SIH', 1, NULL, 'Does anyone in the home smoke tobacco (such as cigarettes, cigars, pipes, etc.)?', 1, 1, 10),

('TCC', 4, NULL, 'Do you use tobacco?', 1, 1, 10),
('TCC', 1, NULL, 'Do you use tobacco for ceremony?', 1, 0, 100),
('TCC', 2, NULL, 'Do you smoke tobacco (such as cigarettes, cigars, pipes, etc.)?', 1, 0, 100),
('TCC', 3, NULL, 'Do you use smokeless tobacco?', 1, 0, 100),

('CAGE', 5, NULL, 'Do you drink alcohol?', 1, 1, 10),
('CAGE', 1, NULL, 'Have you ever felt you should CUT down on your drinking?', 1, 0, 100),
('CAGE', 2, NULL, 'Have people ANNOYED you by criticizing your drinking?', 1, 0, 100),
('CAGE', 3, NULL, 'Have you ever felt bad or GUILTY about your drinking?', 1, 0, 100),
('CAGE', 4, NULL, 'Have you ever had a drink first thing in the morning to steady your nerves or get rid of a hangover (EYE-OPENER)?', 1, 0, 100),

('DAST', 10, 'Over the LAST 12 MONTHS:', 'Have you used drugs other than those required for medical reasons?', 1, 1, 10),
('DAST', 1, 'Over the LAST 12 MONTHS:', 'Do you abuse more than one drug at a time?', 1, 0, 100),
('DAST', 2, 'Over the LAST 12 MONTHS:', 'Are you always able to stop using drugs when you want to?', 1, 0, 100),
('DAST', 3, 'Over the LAST 12 MONTHS:', 'Have you had “blackouts” or “flashbacks” as a result of drug use?', 1, 0, 100),
('DAST', 4, 'Over the LAST 12 MONTHS:', 'Do you ever feel bad or guilty about your drug use?', 1, 0, 100),
('DAST', 5, 'Over the LAST 12 MONTHS:', 'Does your spouse (or parent) ever complain about your involvement with drugs?', 1, 0, 100),
('DAST', 6, 'Over the LAST 12 MONTHS:', 'Have you neglected your family because of your use of drugs?', 1, 0, 100),
('DAST', 7, 'Over the LAST 12 MONTHS:', 'Have you engaged in illegal activities in order to obtain drugs?', 1, 0, 100),
('DAST', 8, 'Over the LAST 12 MONTHS:', 'Have you ever experienced withdrawal symptoms (felt sick) when you stopped taking drugs?', 1, 0, 100),
('DAST', 9, 'Over the LAST 12 MONTHS:', 'Have you had medical problems as a result of your drug use (e.g., memory loss, hepatitis, convulsions, bleeding)?', 1, 0, 100),

('PHQ-9', 1, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Little interest or pleasure in doing things?', 2, 1, 10),
('PHQ-9', 2, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Feeling down, depressed, or hopeless?', 2, 1, 20),
('PHQ-9', 3, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Trouble falling or staying asleep, or sleeping too much?', 2, 0, 100),
('PHQ-9', 4, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Feeling tired or having little energy?', 2, 0, 100),
('PHQ-9', 5, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Poor appetite or overeating?', 2, 0, 100),
('PHQ-9', 6, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Feeling bad about yourself - or that you are a failure or have let yourself or your family down?', 2, 0, 100),
('PHQ-9', 7, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Trouble concentrating on things, such as reading the newspaper or watching television?', 2, 0, 100),
('PHQ-9', 8, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Moving or speaking so slowly that other people could have noticed. Or the opposite - being so fidgety or restless that you have been moving around a lot more than usual?', 2, 0, 100),
('PHQ-9', 9, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Thoughts that you would be BETTER OF DEAD or of HURTING YOURSELF in some way?', 2, 0, 100),
('PHQ-9', 10, NULL, 'If you checked off ANY problems, how DIFFICULT have these problems made it for you to do your work, take care of things at home, or get along with other people?', 3, 0, 100),


('HITS', 5, 'Over the LAST 12 MONTHS:', 'Did a partner, family member, or caregiver hurt, insult, threaten, or scream at you?', 1, 1, 10),
('HITS', 1, 'Over the LAST 12 MONTHS, how often did your partner, family member, or caregiver:', 'Physically HURT you?', 4, 0, 100),
('HITS', 2, 'Over the LAST 12 MONTHS, how often did your partner, family member, or caregiver:', 'INSULT or talk down to you?', 4, 0, 100),
('HITS', 3, 'Over the LAST 12 MONTHS, how often did your partner, family member, or caregiver:', 'THREATEN you with physical harm?', 4, 0, 100),
('HITS', 4, 'Over the LAST 12 MONTHS, how often did your partner, family member, or caregiver:', 'SCREAM or curse at you?', 4, 0, 100)
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



--- CIF
UPDATE dbo.ScreeningFrequency SET ID = 'CIF' WHERE ID = '_Contact';

----------------------------------------                               

IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '5.0.0.0')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('5.0.0.0');

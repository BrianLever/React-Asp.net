-- Question changes
UPDATE ScreeningSection SET ScreeningSectionName = 'Tobacco Use' WHERE  ScreeningSectionID = 'TCC';

INSERT INTO ScreeningSection(ScreeningSectionID,ScreeningID,ScreeningSectionName,QuestionText, OrderIndex)
VALUES
('SIH', 'BHS', 'Smoker in the Home', 'Does anyone in the home smoke tobacco (such as cigarettes, cigars, pipes, etc.)?', 0);


delete from ScreeningSectionQuestionResult;
DELETE FROM ScreeningSectionResult


----
DELETE FROM ScreeningSectionQuestion WHERE ScreeningSectionID = 'TCC';

INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID)
VALUES
('TCC', 1, NULL, 'Do you use tobacco for ceremony?', 1),
('TCC', 2, NULL, 'Do you smoke tobacco (such as cigarettes, cigars, pipes, etc.)?', 2),
('TCC', 3, NULL, 'Do you use smokeless tobacco?', 3);


INSERT INTO dbo.ScreeningScoreLevel(ScreeningSectionID, ScoreLevel, [Name])
VALUES
('SIH', 0, 'Negative'),
('SIH', 1, 'Positive')
GO


-------------------------------------------
-- NEW Substance Abuse Screening
update ScreeningSection
set OrderIndex = OrderIndex+1
where ScreeningSectionID IN('PHQ-9', 'HITS');

INSERT INTO ScreeningSection(ScreeningSectionID,ScreeningID,ScreeningSectionName,QuestionText, OrderIndex)
VALUES ('DAST', 'BHS', 'DAST-10 Screening Tool', 'Have you used drugs other than those required for medical reasons?',3);

INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID)
VALUES

('DAST', 1, NULL, 'Do you abuse more than one drug at a time?', 1),
('DAST', 2, NULL, 'Are you always able to stop using drugs when you want to?', 1),
('DAST', 3, NULL, 'Have you had “blackouts” or “flashbacks” as a result of drug use?', 1),
('DAST', 4, NULL, 'Do you ever feel bad or guilty about your drug use?', 1),
('DAST', 5, NULL, 'Does your spouse (or parent) ever complain about your involvement with drugs?', 1),
('DAST', 6, NULL, 'Have you neglected your family because of your use of drugs?', 1),
('DAST', 7, NULL, 'Have you engaged in illegal activities in order to obtain drugs?', 1),
('DAST', 8, NULL, 'Have you ever experienced withdrawal symptoms (felt sick) when you stopped taking drugs?', 1),
('DAST', 9, NULL, 'Have you had medical problems as a result of your drug use (e.g., memory loss, hepatitis, convulsions, bleeding)?', 1);


---------------------------------------------

INSERT INTO dbo.ScreeningScoreLevel(ScreeningSectionID, ScoreLevel, [Name])
VALUES
('DAST', 0, 'No Problem Reported (at this time)'),
('DAST', 1, 'Low Level (monitor, reassess at a later date)'),
('DAST', 2, 'Moderate Level (further investigation is required)'),
('DAST', 3, 'Substantial Level (assessment required)'),
('DAST', 4, 'Severe Level (assessment required)')
GO



IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'ScreeningSectionAge')
	BEGIN
		DROP  Table dbo.ScreeningSectionAge
	END
GO

CREATE TABLE dbo.ScreeningSectionAge
(
	ScreeningSectionID char(5) NOT NULL,
	MinimalAge tinyint NOT NULL,
	LastModifiedDateUTC datetime NOT NULL,
	CONSTRAINT PK_ScreeningSectionAge PRIMARY KEY(ScreeningSectionID ASC),
	CONSTRAINT FK_ScreeningSectionAge__ScreeningSection FOREIGN KEY(ScreeningSectionID)
		REFERENCES dbo.ScreeningSection(ScreeningSectionID) ON UPDATE CASCADE ON DELETE CASCADE
)
GO


GRANT SELECT,INSERT, UPDATE ON dbo.ScreeningSection TO frontdesk_appuser
GO

INSERT INTO ScreeningSectionAge(ScreeningSectionID, MinimalAge, LastModifiedDateUTC) VALUES
('SIH', 0, GETUTCDATE()),
('TCC', 15, GETUTCDATE()),
('CAGE', 15, GETUTCDATE()),
('HITS', 15, GETUTCDATE()),
('PHQ-9', 15, GETUTCDATE());

INSERT INTO ScreeningSectionAge(ScreeningSectionID, MinimalAge, LastModifiedDateUTC) VALUES
('DAST', 15, GETUTCDATE());


---- NEW COLUMN for UI


alter table ScreeningSection
add ScreeningSectionShortName varchar(16) NULL;
GO
UPDATE ScreeningSection SET ScreeningSectionShortName = ScreeningSectionID;
UPDATE ScreeningSection SET ScreeningSectionShortName = 'DAST-10' WHERE ScreeningSectionID='DAST';
GO
alter table ScreeningSection
alter column ScreeningSectionShortName varchar(16) NOT NULL;
GO





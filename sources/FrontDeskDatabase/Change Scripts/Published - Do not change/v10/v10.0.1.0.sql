-- Remove RPMS word

MERGE INTO dbo.SecurityEvent as target
USING ( VALUES
(20, 2, 'Patient address was updated from EHR',1)

) as source(SecurityEventID, SecurityEventCategoryID, [Description], Enabled) 
    ON target.SecurityEventID = source.SecurityEventID
WHEN MATCHED THEN 
    UPDATE SET [Description] = source.[Description], Enabled = source.Enabled
;
GO



IF NOT EXISTS(SELECT 1 FROM dbo.ScreeningProfileFrequency WHERE Frequency > 100 AND Frequency <> 2400)
BEGIN

UPDATE dbo.ScreeningProfileFrequency
    SET Frequency = Frequency * 100
WHERE Frequency > 0

END

GO


IF OBJECT_ID('[dbo].[vDrugsOfChoiceForExcelExport]') IS NOT NULL
    DROP VIEW [dbo].[vDrugsOfChoiceForExcelExport]
GO

CREATE VIEW [dbo].[vDrugsOfChoiceForExcelExport]
AS 
SELECT 
[ScreeDox Record No.],
[ScreeningDate],
[LastName],
[FirstName],
[MiddleName],
[Birthday],
[LocationID],
[Location],
[DemographicsId],
[Primary Drug],
[Secondary Drug],
[Tertiary Drug]

FROM dbo.vScreeningResultsForExcelExport r
WHERE [Primary Drug] is NOT NULL

GO

-------

IF OBJECT_ID('[dbo].[vUniquePatients]') IS NOT NULL
    DROP VIEW [dbo].[vUniquePatients]
GO

CREATE VIEW [dbo].[vUniquePatients]
AS
SELECT 
	MAX(r.ScreeningResultID) as ScreeningResultID, 
	r.PatientName, 
	r.Birthday 
FROM dbo.ScreeningResult r
	INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID
	INNER JOIN dbo.BranchLocation l ON l.BranchLocationID = k.BranchLocationID
WHERE r.IsDeleted = 0
GROUP BY r.PatientName, r.Birthday

GO



--- Cut Score

IF EXISTS(select 1 from sys.columns where object_id = OBJECT_ID('[dbo].[VisitSettings]') and name = 'CutScore')
BEGIN
    SET NOEXEC ON
END
GO


ALTER TABLE [dbo].[VisitSettings]
    ADD CutScore int NULL;

GO
UPDATE dbo.VisitSettings SET CutScore = 1;

GO
ALTER TABLE dbo.[VisitSettings] 
    ALTER COLUMN CutScore int NOT NULL;
GO

ALTER TABLE dbo.[VisitSettings]
    ADD CONSTRAINT DF_VisitSettings_CustScore DEFAULT 1 FOR CutScore;

GO
ALTER TABLE dbo.[VisitSettings]
    ADD CONSTRAINT CK_VisitSettings_CutScore CHECK (CutScore > 0)


SET NOEXEC OFF
GO
;

--- Cut Score -- end

-- ScreeningScoreLevel

IF EXISTS(select 1 from sys.columns where object_id = OBJECT_ID('[dbo].[ScreeningScoreLevel]') and name = 'Label')
BEGIN
    SET NOEXEC ON
END
GO

ALTER TABLE [dbo].ScreeningScoreLevel
    ADD [Label] nvarchar(64) NULL

GO


MERGE INTO dbo.ScreeningScoreLevel as target
USING ( VALUES
('TCC', 0, 'NEGATIVE', 'Negative', 'Negative'),
('TCC', 1, 'POSITIVE', 'Positive', 'Positive'),
('SIH', 0, 'NEGATIVE', 'Negative', 'Negative'),
('SIH', 1, 'POSITIVE', 'Positive', 'Positive'),

('CAGE', 0, 'NEGATIVE', 'No problems reported', 'Negative'),
('CAGE', 1, 'Evidence of AT RISK', 'Need for further clinical investigation, including questions on amount, frequency, etc.', 'At Risk'),
('CAGE', 2, 'Evidence of CURRENT PROBLEM', 'Need for further clinical investigation and/or referral as indicated by clinician''s expertise', 'Current Problem'),
('CAGE', 3, 'Evidence of DEPENDENCE until ruled out', 'Evaluate, treat, and/or referral as indicated by clinician''s expertise', 'Dependence'),

('PHQ-9', 0, 'NONE-MINIMAL depression severity', 'No proposed treatment action', 'None-Minimal'), -- No Depression

('PHQ-9', 2, 'MILD depression severity', 'Watchful waiting; repeat PHQ-9 at follow-up', 'Mild'),
('PHQ-9', 3, 'MODERATE depression severity', 'Treatment plan, considering counseling, follow-up and/or pharmacotherapy', 'Moderate'),
('PHQ-9', 4, 'MODERATELY SEVERE depression severity' , 'Active treatment with pharmacotherapy and/or psychotherapy', 'Moderately Severe'),
('PHQ-9', 5, 'SEVERE depression severity', 'Immediate initiation of pharmacotherapy and, if severe impairment or poor response to therapy, expedited referral to a mental health specialist for psychotherapy and/or collaborative management', 'Severe'),

('HITS', 0, 'NEGATIVE', 'No problems reported. Review with patient (if possible)', 'Negative'),
('HITS', 1, 'Evidence of CURRENT PROBLEM', 'Need for immediate investigation and/or referral', 'Current Problem'),

('DAST', 0, 'NEGATIVE', 'No problems reported', 'Negative'),
('DAST', 1, 'LOW LEVEL degree of problem related to drug use', 'Monitor and re-assess at a later date', 'Low'),
('DAST', 2, 'MODERATE LEVEL degree of problem related to drug use', 'Further investigation is required', 'Moderate'),
('DAST', 3, 'SUBSTANTIAL LEVEL degree of problem related to drug use', 'Assessment required', 'Substantial'),
('DAST', 4, 'SEVERE LEVEL degree of problem related to drug use', 'Assessment required', 'Severe'),

('DOCH', 0, 'NEGATIVE', 'Negative', 'Negative'),
('DOCH', 1, 'POSITIVE', 'Positive', 'Positive')

) AS source (ScreeningSectionID, ScoreLevel, [Name], Indicates, [Label])
    ON target.ScreeningSectionID = source.ScreeningSectionID AND target.ScoreLevel = source.ScoreLevel
WHEN MATCHED THEN
    UPDATE SET 
        [Name] = Source.[Name], 
        Indicates = Source.Indicates,
        Label = Source.Label
WHEN NOT MATCHED BY TARGET THEN
    INSERT (ScreeningSectionID, ScoreLevel, [Name], Indicates, Label)
    VALUES(source.ScreeningSectionID, source.ScoreLevel, source.[Name], source.Indicates, source.Label)
;
GO


ALTER TABLE [dbo].ScreeningScoreLevel
    ALTER COLUMN [Label] nvarchar(64) NOT NULL

GO



SET NOEXEC OFF
GO
;
-- ScreeningScoreLevel -- end

IF OBJECT_ID('[dbo].[uspGetAllScoreLevels]') IS NOT NULL
    DROP PROC [dbo].[uspGetAllScoreLevels];
GO

CREATE PROCEDURE [dbo].[uspGetAllScoreLevels]
AS
    SELECT ScreeningSectionID, ScoreLevel, Name, Label
    FROM dbo.ScreeningScoreLevel
    ORDER BY ScreeningSectionID ASC, ScoreLevel ASC

RETURN 0
GO

-- [uspGetQuestionPositiveScoreLevels]
IF OBJECT_ID('[dbo].[uspGetQuestionPositiveScoreLevels]') IS NOT NULL
    DROP PROC [dbo].[uspGetQuestionPositiveScoreLevels];
GO


CREATE PROCEDURE [dbo].[uspGetQuestionPositiveScoreLevels]
    @ScreeningSectionID char(5),
    @QuestionID int
AS
    SELECT ScreeningSectionID, options.OptionValue, options.OptionText, options.OptionText
    FROM dbo.ScreeningSectionQuestion q 
        INNER JOIN dbo.AnswerScale scale ON scale.AnswerScaleID = q.AnswerScaleID
        INNER JOIN dbo.AnswerScaleOption options ON scale.AnswerScaleID = options.AnswerScaleID
    WHERE q.ScreeningSectionID = @ScreeningSectionID AND q.QuestionID = @QuestionID
        AND options.OptionValue > 0
    ORDER BY options.OptionValue

RETURN 0
GO



IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '10.0.1.0')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('10.0.1.0');
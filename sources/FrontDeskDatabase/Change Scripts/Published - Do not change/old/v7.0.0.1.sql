MERGE INTO dbo.AnswerScale as target
USING (
VALUES
(1, 'Yes / No'),
(2, 'PHQ-9'),
(3, 'PHQ-9 Difficulty'),
(4, 'HITS'),
(5, 'Drug Of Choice First'),
(6, 'Drug Of Choice Others')
) AS source(AnswerScaleID, Description) ON target.AnswerScaleID = source.AnswerScaleID
WHEN MATCHED THEN  
    UPDATE SET [Description] = source.[Description]
WHEN NOT MATCHED BY TARGET THEN 
    INSERT (AnswerScaleID, Description) VALUES (source.AnswerScaleID, source.Description)  
;
GO


ALTER TABLE dbo.AnswerScaleOption
    ALTER COLUMN OptionText nvarchar(48) NOT NULL;
GO


MERGE INTO dbo.AnswerScaleOption as target
USING (
VALUES
( 1, 1, 'Yes', 1),
( 2, 1, 'No', 0),
( 3, 2, 'Not at all', 0),
( 4, 2, 'Several days', 1),
( 5, 2, 'More than half the days', 2),
( 6, 2, 'Nearly every day', 3),
( 7, 3, 'Not difficult at all', 0),
( 8, 3, 'Somewhat difficult', 1),
( 9, 3, 'Very difficult', 2),
( 10, 3, 'Extremely difficult', 3),
( 11, 4, 'Never', 1),
( 12, 4, 'Rarely', 2),
( 13, 4, 'Sometimes', 3),
( 14, 4, 'Fairly Often', 4),
( 15, 4, 'Frequently', 5),

(20, 6, '(None) Don’t Use Any Other Drugs', 0),

(21, 5, 'Marijuana/Cannabis/Wax/Hashish', 1),
(22, 6, 'Marijuana/Cannabis/Wax/Hashish', 1),

(23, 5, 'Methamphetamine', 2),
(24, 6, 'Methamphetamine', 2),

(25, 5, 'Other Amphetamines', 3),
(26, 6, 'Other Amphetamines', 3),

(27, 5, 'Benzodiazepines', 4),
(28, 6, 'Benzodiazepines', 4),

(29, 5, 'Opiate/Heroin', 5),
(30, 6, 'Opiate/Heroin', 5),

(31, 5, 'Opiate/Pain Medication', 6),
(32, 6, 'Opiate/Pain Medication', 6),

(33, 5, 'Cocaine/Crack', 7),
(34, 6, 'Cocaine/Crack', 7),

(35, 5, 'Hallucinogens/Psychedelics', 8),
(36, 6, 'Hallucinogens/Psychedelics', 8),

(37, 5, 'Sedatives/Hypnotics/Non-Benzo Tranquilizers', 9),
(38, 6, 'Sedatives/Hypnotics/Non-Benzo Tranquilizers', 9),

(39, 5, 'Inhalants', 10),
(40, 6, 'Inhalants', 10),

(41, 5, 'Barbiturates/Downers', 11),
(42, 6, 'Barbiturates/Downers', 11),

(43, 5, 'PCP/Ketamine/GHB/Designer Drugs', 12),
(44, 6, 'PCP/Ketamine/GHB/Designer Drugs', 12),

(45, 5, 'Other Stimulants', 13),
(46, 6, 'Other Stimulants', 13),

(47, 5, 'Other', 14),
(48, 6, 'Other', 14)
) AS source(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
    ON source.AnswerScaleOptionID = target.AnswerScaleOptionID
WHEN MATCHED THEN  
    UPDATE SET AnswerScaleID = source.AnswerScaleID, OptionText = source.OptionText, OptionValue = source.OptionValue
WHEN NOT MATCHED BY TARGET THEN 
    INSERT (AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue) 
        VALUES (source.AnswerScaleOptionID, source.AnswerScaleID, source.OptionText, source.OptionValue)  
;
GO




-- Section Qiestions, with Y/N answer
MERGE INTO dbo.ScreeningSection as target
USING ( VALUES
('CIF', 'BHS', 'CIF', 'Contact Information', '', 0),
('SIH', 'BHS', 'SIH', 'Smoker in the Home', 'Does anyone in the home smoke tobacco (such as cigarettes, cigars, pipes, electronic nicotine delivery devices, etc.)?', 0),
('TCC', 'BHS', 'TCC', 'Tobacco Use', 'Do you use tobacco?', 1),
('CAGE', 'BHS', 'CAGE', 'Alcohol Use (CAGE)', 'Do you drink alcohol?',2),
('DAST', 'BHS', 'DAST-10', 'Non-Medical Drug Use (DAST-10)', 'Have you used drugs other than those required for medical reasons?',3),
('PHQ-9', 'BHS', 'PHQ-9', 'Depression (PHQ-9)', 'Do you feel down, depressed, or hopeless?', 5),
('HITS', 'BHS', 'HITS', 'Intimate Partner/Domestic Violence (HITS)', 'Do you feel UNSAFE in your home?', 6),
('DOCH', 'BHS', 'DOCH', 'Drug of Choice', 'What Drug do you USE THE MOST?', 4)
) AS source(ScreeningSectionID, ScreeningID, ScreeningSectionShortName, ScreeningSectionName, QuestionText, OrderIndex)
    ON source.ScreeningSectionID = target.ScreeningSectionID
WHEN MATCHED THEN  
    UPDATE SET ScreeningID = source.ScreeningID, 
        ScreeningSectionShortName = source.ScreeningSectionShortName, 
        ScreeningSectionName = source.ScreeningSectionName,
        QuestionText = source.QuestionText,
        OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN 
    INSERT (ScreeningSectionID,ScreeningID,ScreeningSectionShortName, ScreeningSectionName,QuestionText, OrderIndex) 
        VALUES (source.ScreeningSectionID, source.ScreeningID, source.ScreeningSectionShortName, source.ScreeningSectionName, source.QuestionText, source.OrderIndex)  
;
GO



MERGE INTO ScreeningSectionQuestion as Target
USING( VALUES
('SIH', 1, NULL, 'Does anyone in the home smoke tobacco (such as cigarettes, cigars, pipes, electronic nicotine delivery devices, etc.)?', 1, 1, 10),

('TCC', 4, NULL, 'Do you use tobacco?', 1, 1, 10),
('TCC', 1, NULL, 'Do you use tobacco for ceremony?', 1, 0, 100),
('TCC', 2, NULL, 'Do you smoke tobacco (such as cigarettes, cigars, pipes, electronic nicotine delivery devices, etc.)?', 1, 0, 100),
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
('HITS', 4, 'Over the LAST 12 MONTHS, how often did your partner, family member, or caregiver:', 'SCREAM or curse at you?', 4, 0, 100),


('DOCH', 1, NULL, 'What DRUG do you use THE MOST?', 5, 1, 10),
('DOCH', 2, NULL, 'What DRUG do you use SECOND MOST?', 6, 0, 100),
('DOCH', 3, NULL, 'Do you use ANY OTHER DRUG?', 6, 0, 100)

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



MERGE INTO dbo.ScreeningScoreLevel as target
USING ( VALUES
('TCC', 0, 'NEGATIVE', 'Negative'),
('TCC', 1, 'POSITIVE', 'Positive'),
('SIH', 0, 'NEGATIVE', 'Negative'),
('SIH', 1, 'POSITIVE', 'Positive'),

('CAGE', 0, 'NEGATIVE', 'No problems reported'),
('CAGE', 1, 'Evidence of AT RISK', 'Need for further clinical investigation, including questions on amount, frequency, etc.'),
('CAGE', 2, 'Evidence of CURRENT PROBLEM', 'Need for further clinical investigation and/or referral as indicated by clinician''s expertise'),
('CAGE', 3, 'Evidence of DEPENDENCE until ruled out', 'Evaluate, treat, and/or referral as indicated by clinician''s expertise'),
('PHQ-9', 0, 'NONE-MINIMAL depression severity', 'No proposed treatment action'), -- No Depression
--('PHQ-9', 1, 'Minimal Depression', 'No proposed treatment action'), -- Minimal Depression
('PHQ-9', 2, 'MILD depression severity', 'Watchful waiting; repeat PHQ-9 at follow-up'),
('PHQ-9', 3, 'MODERATE depression severity', 'Treatment plan, considering counseling, follow-up and/or pharmacotherapy'),
('PHQ-9', 4, 'MODERATELY SEVERE depression severity' , 'Active treatment with pharmacotherapy and/or psychotherapy'),
('PHQ-9', 5, 'SEVERE depression severity', 'Immediate initiation of pharmacotherapy and, if severe impairment or poor response to therapy, expedited referral to a mental health specialist for psychotherapy and/or collaborative management'),
('HITS', 0, 'NEGATIVE', 'No problems reported. Review with patient (if possible)'),
('HITS', 1, 'Evidence of CURRENT PROBLEM', 'Need for immediate investigation and/or referral'),
('DAST', 0, 'NEGATIVE', 'No problems reported'),
('DAST', 1, 'LOW LEVEL degree of problem related to drug use', 'Monitor and re-assess at a later date'),
('DAST', 2, 'MODERATE LEVEL degree of problem related to drug use', 'Further investigation is required'),
('DAST', 3, 'SUBSTANTIAL LEVEL degree of problem related to drug use', 'Assessment required'),
('DAST', 4, 'SEVERE LEVEL degree of problem related to drug use', 'Assessment required'),
('DOCH', 0, 'NEGATIVE', 'Negative'),
('DOCH', 1, 'POSITIVE', 'Positive')

) AS source (ScreeningSectionID, ScoreLevel, [Name], Indicates)
    ON target.ScreeningSectionID = source.ScreeningSectionID AND target.ScoreLevel = source.ScoreLevel
WHEN MATCHED THEN
	UPDATE SET 
		[Name] = Source.[Name], 
		Indicates = Source.Indicates
WHEN NOT MATCHED BY TARGET THEN
	INSERT (ScreeningSectionID, ScoreLevel, [Name], Indicates)
	VALUES(source.ScreeningSectionID, source.ScoreLevel, source.[Name], source.Indicates)
;
GO


IF EXISTS(select * from sys.indexes where name = 'IX_Discharged_OrderIndex' and object_id = OBJECT_ID('dbo.NewVisitReferralRecommendation'))
    DROP INDEX IX_Discharged_OrderIndex ON dbo.NewVisitReferralRecommendation
GO

IF EXISTS(select * from sys.indexes where name = 'IX_Discharged_OrderIndex' and object_id = OBJECT_ID('dbo.Discharged'))
    DROP INDEX IX_Discharged_OrderIndex ON dbo.Discharged
GO

CREATE INDEX IX_Discharged_OrderIndex 
    ON dbo.[Discharged]([OrderIndex] DESC) INCLUDE([Name])
GO


IF OBJECT_ID('[dbo].[DrugOfChoice]') IS NOT NULL
    SET NOEXEC ON;
GO

CREATE TABLE [dbo].[DrugOfChoice]
(
    [ID] INT NOT NULL,
    [Name] NVARCHAR(64) NOT NULL,
    [OrderIndex] INT NOT NULL
    CONSTRAINT PK_DrugOfChoice PRIMARY KEY CLUSTERED (ID),
);
GO
CREATE INDEX IX_DrugOfChoice_OrderIndex 
    ON dbo.[DrugOfChoice]([OrderIndex] DESC) INCLUDE([Name])
GO

SET NOEXEC OFF;
GO


print 'Populating dbo.[DrugOfChoice]...'
GO

MERGE INTO dbo.DrugOfChoice as target
USING (VALUES
(0, '(None) Don’t Use Any Other Drugs', 1),
(1, 'Marijuana/Cannabis/Wax/Hashish', 2),
(2, 'Methamphetamine', 3),
(3, 'Other Amphetamines', 4),
(4, 'Benzodiazepines', 5),
(5, 'Opiate/Heroin', 6),
(6, 'Opiate/Pain Medication', 7),
(7, 'Cocaine/Crack', 8),
(8, 'Hallucinogens/Psychedelics', 9),
(9, 'Sedatives/Hypnotics/Non-Benzo Tranquilizers', 10),
(10, 'Inhalants', 11),
(11, 'Barbiturates/Downers', 12),
(12, 'PCP/Ketamine/GHB/Designer Drugs', 13),
(13, 'Other Stimulants', 14),
(14, 'Other', 15)

) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
WHEN NOT MATCHED BY SOURCE THEN
    DELETE
;
GO

IF OBJECT_ID('[dbo].[uspUpdateDrugOfChoice]') IS NOT NULL
    DROP PROCEDURE [dbo].[uspUpdateDrugOfChoice];
GO

CREATE PROCEDURE [dbo].[uspUpdateDrugOfChoice]
    @ScreeningResultID bigint,
    @PrimaryAnswer int,
    @SecondaryAnswer int,
    @TertiaryAnswer int

AS
BEGIN
    DECLARE @DrugOfChoiceSectionID char(5) = 'DOCH';
    DECLARE @sectionAnswer int = 0

    IF @PrimaryAnswer > 0
        SET @sectionAnswer = 1

    MERGE INTO dbo.ScreeningSectionResult target
    USING (
        VALUES(@ScreeningResultID,@DrugOfChoiceSectionID, @sectionAnswer, @sectionAnswer,@sectionAnswer)
        ) AS source(ScreeningResultID,ScreeningSectionID,AnswerValue,Score,ScoreLevel)
        ON target.ScreeningResultID = source.ScreeningResultID AND target.ScreeningSectionID = source.ScreeningSectionID
    WHEN MATCHED THEN  
        UPDATE SET AnswerValue = source.AnswerValue, Score = source.Score,  ScoreLevel = source.ScoreLevel
    WHEN NOT MATCHED BY TARGET THEN  
        INSERT (ScreeningResultID,ScreeningSectionID,AnswerValue,Score,ScoreLevel) 
        VALUES (source.ScreeningResultID,source.ScreeningSectionID,source.AnswerValue,source.Score, source.ScoreLevel); 

    ;

    MERGE INTO dbo.ScreeningSectionQuestionResult target
    USING ( VALUES
        (@ScreeningResultID, @DrugOfChoiceSectionID, 1, @PrimaryAnswer),
        (@ScreeningResultID, @DrugOfChoiceSectionID, 2, @SecondaryAnswer),
        (@ScreeningResultID, @DrugOfChoiceSectionID, 3, @TertiaryAnswer)

    ) AS source(ScreeningResultID,ScreeningSectionID,QuestionID,AnswerValue)
        ON target.ScreeningResultID = source.ScreeningResultID 
            AND target.ScreeningSectionID = source.ScreeningSectionID
            AND target.QuestionID = source.QuestionID
    WHEN MATCHED THEN  
        UPDATE SET AnswerValue = source.AnswerValue
    WHEN NOT MATCHED BY TARGET THEN  
        INSERT (ScreeningResultID,ScreeningSectionID,QuestionID,AnswerValue) 
        VALUES (source.ScreeningResultID,source.ScreeningSectionID,source.QuestionID,source.AnswerValue); 

    ;
END
RETURN 0

GO


MERGE INTO dbo.SecurityEvent as target
USING ( VALUES
--system security
(1, 1, 'User was logged into the system',1),
(2, 1, 'Password was changed',1),
(3, 1, 'Security question and/or answer were changed',1),
(4, 1, 'New user was created',1),
(5, 1, 'New account was activated',1),
-- accessing screen results 
(6, 2, 'Behavioral Health Screening Report was read',1),
(7, 2, 'Behavioral Health Screening Report was printed',1),
(12, 2, 'Behavioral Health Screening Report was removed',1),
(13, 2, 'Patient contact information was changed',1),	  
(14, 2, 'Behavioral Health Screening Report was exported',1),
(15, 2, 'BHS Visit Information was completed',1),
(16, 2, 'BHS Visit was created manually',1),
(18, 2, 'BHS Patient Demographics was completed.',1),
(19, 2, 'BHS Follow-Up was completed',1),
(20, 2, 'Patient address was updated from RPMS',1),
(21, 2, 'BHS Visit Report was printed',1),
(22, 2, 'BHS Follow-Up Report was printed',1),
(23, 2, 'BHS Patient Demographics was printed',1),
(24, 2, 'Patient Demographics was printed',1),
(25, 2, 'Drug of Choice was changed',1),


-- Branch location mgmt
(8, 3, 'New branch location was created',1),
(9, 3, 'Branch location was removed',1),
-- Kiosk mgmt
(10, 4, 'New kiosk was registered',1),
(11, 4, 'Kiosk was removed',1)
) as source(SecurityEventID, SecurityEventCategoryID, [Description], Enabled) 
    ON target.SecurityEventID = source.SecurityEventID
WHEN MATCHED THEN 
    UPDATE SET [Description] = source.[Description], Enabled = source.Enabled
WHEN NOT MATCHED BY TARGET THEN
    INSERT(SecurityEventID, SecurityEventCategoryID, [Description], Enabled) 
        VALUES (source.SecurityEventID, source.SecurityEventCategoryID, source.[Description], Enabled)
;
GO

GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.ScreeningTimeLog TO frontdesk_appuser
GO

---------------------------------------------
IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '7.0.0.1')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('7.0.0.1');
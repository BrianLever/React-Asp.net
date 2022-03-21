
--------------------------------------------
-- Problem Gambling (BBGS)
--------------------------------------------


MERGE INTO dbo.ScreeningSection as target
USING ( VALUES
('BBGS', 'BHS', 'BBGS', 'Problem Gambling (BBGS)', '', 20)

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

------------------------------------
MERGE INTO ScreeningSectionQuestion as Target
USING( VALUES

-- Problem Gambling
('BBGS', 1, NULL, 'Have you gambled in the past 12 months? (Examples of gambling include lottery scratchers or draw tickets, casino games, daily fantasy sports, bingo, online poker, horseracing, etc.)?', 1, 1, 0, 10),
('BBGS', 2, 'During the PAST 12 MONTHS:', 'Have you become restless, irritable, or anxious when trying to stop/cut down on gambling?', 1, 0, 0, 100),
('BBGS', 3, 'During the PAST 12 MONTHS:', 'Have you tried to keep your family or friends from knowing how much you gambled?', 1, 0, 0, 100),
('BBGS', 4, 'During the PAST 12 MONTHS:', 'Did you have such financial trouble as a result of your gambling that you had to get help with living expenses from family, friends, or welfare?', 1, 0, 0, 100)


) as Source(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OnlyWhenPossitive,OrderIndex)
    ON Target.ScreeningSectionID = Source.ScreeningSectionID AND Target.QuestionID = Source.QuestionID

WHEN MATCHED THEN
    UPDATE SET 
        PreambleText = Source.PreambleText, 
        QuestionText = Source.QuestionText,
        AnswerScaleID = Source.AnswerScaleID,
        IsMainQuestion = Source.IsMainQuestion,
        OnlyWhenPossitive = Source.OnlyWhenPossitive,
        OrderIndex = Source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
    INSERT (ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OnlyWhenPossitive, OrderIndex)
    VALUES(Source.ScreeningSectionID, Source.QuestionID, Source.PreambleText, Source.QuestionText, Source.AnswerScaleID, Source.IsMainQuestion, Source.OnlyWhenPossitive, Source.OrderIndex)
    ;
GO

--------------

MERGE INTO dbo.ScreeningScoreLevel as target
USING ( VALUES

('BBGS', 0, 'NEGATIVE', 'No problems reported', 'Negative'),
('BBGS', 1, 'Evidence of PROBLEM GAMBLING', 'Need for immediate investigation and/or referral', 'Problem Gambling')



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
--------------

---- Screening Profile Frequency
MERGE INTO dbo.ScreeningProfileFrequency as target
USING ( VALUES
(1, 'BBGS', 1200, GETUTCDATE())

) AS source (ScreeningProfileID, ScreeningSectionID, Frequency, LastModifiedDateUTC)
    ON target.ScreeningProfileID = source.ScreeningProfileID AND target.ScreeningSectionID = source.ScreeningSectionID
WHEN MATCHED THEN
    UPDATE SET 
        Frequency = Source.Frequency, 
        LastModifiedDateUTC = Source.LastModifiedDateUTC
WHEN NOT MATCHED BY TARGET THEN
    INSERT (ScreeningProfileID, ScreeningSectionID, Frequency, LastModifiedDateUTC)
    VALUES(source.ScreeningProfileID, source.ScreeningSectionID, source.Frequency, source.LastModifiedDateUTC)
;
GO

-- Populate all from DAST-10
IF NOT EXISTS( SELECT 1 FROM dbo.ScreeningProfileFrequency WHERE ScreeningProfileID <> 1 AND ScreeningSectionID IN ('BBGS'))
BEGIN

INSERT INTO dbo.ScreeningProfileFrequency (ScreeningProfileID, ScreeningSectionID, Frequency, LastModifiedDateUTC)
SELECT DISTINCT t1.ScreeningProfileID, source.ScreeningSectionID, source.Frequency, GETUTCDATE()
FROM dbo.ScreeningProfileFrequency t1 INNER JOIN dbo.ScreeningProfileFrequency source 
    ON source.ScreeningProfileID = 1 AND source.ScreeningSectionID IN ('BBGS')
WHERE t1.ScreeningProfileID <> 1

END
----------------------------


-- Age

/* ScreeningSectionAge */
MERGE INTO dbo.ScreeningProfileSectionAge target
USING ( 
VALUES 
(1, 'BBGS', 0, 1, 0)

) AS source (ScreeningProfileID, ScreeningSectionID, MinimalAge, IsEnabled, AgeIsNotConfigurable) 
    ON source.ScreeningProfileID = target.ScreeningProfileID AND source.ScreeningSectionID = target.ScreeningSectionID
WHEN MATCHED THEN
    UPDATE SET MinimalAge = source.MinimalAge, IsEnabled = source.IsEnabled, AgeIsNotConfigurable = source.AgeIsNotConfigurable, LastModifiedDateUTC = GETUTCDATE()
WHEN NOT MATCHED THEN
    INSERT (ScreeningProfileID, ScreeningSectionID, MinimalAge, IsEnabled, AgeIsNotConfigurable,LastModifiedDateUTC)
    VALUES(source.ScreeningProfileID, source.ScreeningSectionID, source.MinimalAge, source.IsEnabled, source.AgeIsNotConfigurable, GETUTCDATE())
;
GO

IF NOT EXISTS( SELECT 1 FROM dbo.ScreeningProfileSectionAge WHERE ScreeningProfileID <> 1 AND ScreeningSectionID IN ('BBGS'))
BEGIN

INSERT INTO dbo.ScreeningProfileSectionAge (ScreeningProfileID, ScreeningSectionID, MinimalAge, IsEnabled, AgeIsNotConfigurable,LastModifiedDateUTC)
SELECT DISTINCT t1.ScreeningProfileID, source.ScreeningSectionID, source.MinimalAge, source.IsEnabled, source.AgeIsNotConfigurable, GETUTCDATE()
FROM dbo.ScreeningProfileSectionAge t1 INNER JOIN dbo.ScreeningProfileSectionAge source 
    ON source.ScreeningProfileID = 1 AND source.ScreeningSectionID IN ('BBGS')
WHERE t1.ScreeningProfileID <> 1

END

------------------------------------

MERGE INTO dbo.[VisitSettings] target
USING ( 
VALUES
('BBGS', 'Problem Gambling (BBGS)', 1, 200, 1)

) AS source (MeasureToolId, Name, IsEnabled, OrderIndex, CutScore)
    ON source.MeasureToolId = target.MeasureToolId
WHEN MATCHED THEN
    UPDATE SET IsEnabled = source.IsEnabled, OrderIndex = source.OrderIndex, CutScore = source.CutScore, LastModifiedDateUTC = GETUTCDATE()
WHEN NOT MATCHED THEN
    INSERT (MeasureToolId, Name, IsEnabled, OrderIndex, CutScore, LastModifiedDateUTC)
    VALUES(source.MeasureToolId, source.Name, source.IsEnabled, source.OrderIndex, source.CutScore, GETUTCDATE())
;

GO

--- 
if exists (select * from sys.columns WHERE object_id = OBJECT_ID('[dbo].[BhsVisit]') and name = 'ProblemGamblingFlagScoreLevel')
    SET NOEXEC ON
GO
ALTER TABLE [dbo].[BhsVisit] ADD 
    ProblemGamblingFlagScoreLevel int NULL;

ALTER TABLE [dbo].[BhsVisit] ADD 
    ProblemGamblingFlagScoreLevelLabel nvarchar(64) NULL;

GO

SET NOEXEC OFF
GO

--------------------------
ALTER PROCEDURE [dbo].[uspGetBhsVisitByID]
    @ID bigint
AS
SELECT v.[ID]
,v.[ScreeningResultID]
,v.[LocationID]
,v.[CreatedDate]
,v.[ScreeningDate]
,v.[TobacoExposureSmokerInHomeFlag]
,v.[TobacoExposureCeremonyUseFlag]
,v.[TobacoExposureSmokingFlag]
,v.[TobacoExposureSmoklessFlag]
,v.[AlcoholUseFlagScoreLevel]
,v.[AlcoholUseFlagScoreLevelLabel]
,v.[SubstanceAbuseFlagScoreLevel]
,v.[SubstanceAbuseFlagScoreLevelLabel]
,v.[AnxietyFlagScoreLevel]
,v.[AnxietyFlagScoreLevelLabel]
,v.[DepressionFlagScoreLevel]
,v.[DepressionFlagScoreLevelLabel]
,v.[DepressionThinkOfDeathAnswer]
,v.[PartnerViolenceFlagScoreLevel]
,v.[PartnerViolenceFlagScoreLevelLabel]
,v.[ProblemGamblingFlagScoreLevel]
,v.[ProblemGamblingFlagScoreLevelLabel]
,v.[NewVisitReferralRecommendationID]
,refRec.[Name] as NewVisitReferralRecommendationName
,v.[NewVisitReferralRecommendationDescription]
,v.[NewVisitReferralRecommendationAcceptedID]
,accept.[Name] as NewVisitReferralRecommendationAcceptedName
,v.[ReasonNewVisitReferralRecommendationNotAcceptedID]
,notaccept.Name as ReasonNewVisitReferralRecommendationNotAcceptedName
,v.[NewVisitDate]
,v.[DischargedID]
,discharge.Name as DischargedName
,v.[ThirtyDatyFollowUpFlag]
,v.[FollowUpDate]
,v.[Notes]
,v.[BhsStaffNameCompleted]
,v.[CompleteDate]
,v.[TreatmentAction1ID]
,ta1.Name as TreatmentAction1Name
,v.[TreatmentAction1Description]
,v.[TreatmentAction2ID]
,ta2.Name as TreatmentAction2Name
,v.[TreatmentAction2Description]
,v.[TreatmentAction3ID]
,ta3.Name as TreatmentAction3Name
,v.[TreatmentAction3Description]
,v.[TreatmentAction4ID]
,ta4.Name as TreatmentAction4Name
,v.[TreatmentAction4Description]
,v.[TreatmentAction5ID]
,ta5.Name as TreatmentAction5Name
,v.[TreatmentAction5Description]
,v.[OtherScreeningTools]
FROM [dbo].[BhsVisit] v
    LEFT JOIN dbo.NewVisitReferralRecommendation refRec ON v.NewVisitReferralRecommendationID = refRec.ID
    LEFT JOIN dbo.NewVisitReferralRecommendationAccepted accept ON v.[NewVisitReferralRecommendationAcceptedID] = accept.ID
    LEFT JOIN dbo.ReasonNewVisitReferralRecommendationNotAccepted notaccept ON v.ReasonNewVisitReferralRecommendationNotAcceptedID = notaccept.ID
    LEFT JOIN dbo.Discharged discharge ON v.[DischargedID] = discharge.ID
    LEFT JOIN dbo.TreatmentAction ta1 ON ta1.ID = v.[TreatmentAction1ID]
    LEFT JOIN dbo.TreatmentAction ta2 ON ta2.ID = v.[TreatmentAction2ID]
    LEFT JOIN dbo.TreatmentAction ta3 ON ta3.ID = v.[TreatmentAction3ID]
    LEFT JOIN dbo.TreatmentAction ta4 ON ta4.ID = v.[TreatmentAction4ID]
    LEFT JOIN dbo.TreatmentAction ta5 ON ta5.ID = v.[TreatmentAction5ID]

WHERE v.ID = @ID;
RETURN 0

GO
--------------------------

ALTER VIEW [dbo].[vBhsVisitForExport]
AS SELECT 
r.PatientName as FullName
,r.LastName
,r.FirstName
,r.MiddleName
,r.Birthday
,r.ExportedToHRN
,pd.ID as DemographicsID
,v.ScreeningResultID
,v.ScreeningDate
,v.ID
,v.CreatedDate
,v.CompleteDate
,v.[BhsStaffNameCompleted]
,v.LocationID
,l.Name as BranchLocationName
,v.[NewVisitReferralRecommendationID]
,refRec.[Name] as NewVisitReferralRecommendationName
,v.[NewVisitReferralRecommendationDescription]
,v.[NewVisitReferralRecommendationAcceptedID]
,accept.[Name] as NewVisitReferralRecommendationAcceptedName
,v.[ReasonNewVisitReferralRecommendationNotAcceptedID]
,notaccept.Name as ReasonNewVisitReferralRecommendationNotAcceptedName
,v.[NewVisitDate]
,v.[DischargedID]
,discharge.Name as DischargedName
,v.[ThirtyDatyFollowUpFlag]
,v.[FollowUpDate]
,v.[Notes]

,v.[TreatmentAction1ID]
,ta1.Name as TreatmentAction1Name
,v.[TreatmentAction1Description]
,v.[TreatmentAction2ID]
,ta2.Name as TreatmentAction2Name
,v.[TreatmentAction2Description]
,v.[TreatmentAction3ID]
,ta3.Name as TreatmentAction3Name
,v.[TreatmentAction3Description]
,v.[TreatmentAction4ID]
,ta4.Name as TreatmentAction4Name
,v.[TreatmentAction4Description]
,v.[TreatmentAction5ID]
,ta5.Name as TreatmentAction5Name
,v.[TreatmentAction5Description]
,v.[OtherScreeningTools]

,v.[TobacoExposureSmokerInHomeFlag]
,v.[TobacoExposureCeremonyUseFlag]
,v.[TobacoExposureSmokingFlag]
,v.[TobacoExposureSmoklessFlag]
,v.[AlcoholUseFlagScoreLevel]
,v.[AlcoholUseFlagScoreLevelLabel]
,v.[SubstanceAbuseFlagScoreLevel]
,v.[SubstanceAbuseFlagScoreLevelLabel]
,v.[DepressionFlagScoreLevel]
,v.[DepressionFlagScoreLevelLabel]
,v.[DepressionThinkOfDeathAnswer]
,v.[PartnerViolenceFlagScoreLevel]
,v.[PartnerViolenceFlagScoreLevelLabel]
,v.AnxietyFlagScoreLevel
,v.AnxietyFlagScoreLevelLabel
,v.[ProblemGamblingFlagScoreLevel]
,v.[ProblemGamblingFlagScoreLevelLabel]

FROM 
    dbo.BhsVisit v 
    INNER JOIN dbo.ScreeningResult r ON v.ScreeningResultID = r.ScreeningResultID
    INNER JOIN dbo.BranchLocation l ON v.LocationID = l.BranchLocationID
    LEFT JOIN dbo.BhsDemographics pd ON pd.Birthday = r.Birthday AND pd.PatientName = r.PatientName
    LEFT JOIN dbo.NewVisitReferralRecommendation refRec ON v.NewVisitReferralRecommendationID = refRec.ID
    LEFT JOIN dbo.NewVisitReferralRecommendationAccepted accept ON v.[NewVisitReferralRecommendationAcceptedID] = accept.ID
    LEFT JOIN dbo.ReasonNewVisitReferralRecommendationNotAccepted notaccept ON v.ReasonNewVisitReferralRecommendationNotAcceptedID = notaccept.ID
    LEFT JOIN dbo.Discharged discharge ON v.[DischargedID] = discharge.ID
    LEFT JOIN dbo.TreatmentAction ta1 ON ta1.ID = v.[TreatmentAction1ID]
    LEFT JOIN dbo.TreatmentAction ta2 ON ta2.ID = v.[TreatmentAction2ID]
    LEFT JOIN dbo.TreatmentAction ta3 ON ta3.ID = v.[TreatmentAction3ID]
    LEFT JOIN dbo.TreatmentAction ta4 ON ta4.ID = v.[TreatmentAction4ID]
    LEFT JOIN dbo.TreatmentAction ta5 ON ta5.ID = v.[TreatmentAction5ID]

WHERE r.IsDeleted = 0 and r.IsDeleted = 0 
GO
--------------------------


ALTER VIEW [dbo].[vScreeningResultsForExcelExport]
AS
SELECT
r.ScreeningResultID as 'ScreeDox Record No.',
CONVERT(char(10),r.CreatedDate, 101) as 'ScreeningDate',
r.LastName,
r.FirstName,
ISNULL(r.MiddleName,'') as MiddleName,
CONVERT(char(10), r.Birthday, 101) as 'Birthday'
,l.BranchLocationID as 'LocationID'
,l.Name as 'Location'
-- tobacco
,q1.AnswerValue as 'Tobacco Use - Ceremony'
,q2.AnswerValue as 'Tobacco Use - Smoking'
,q3.AnswerValue as 'Tobacco Use - Smokeless'

,c1.AnswerValue as 'CAGE / Alcohol Use - Q1'
,c2.AnswerValue as 'CAGE / Alcohol Use - Q2'
,c3.AnswerValue as 'CAGE / Alcohol Use - Q3'
,c4.AnswerValue as 'CAGE / Alcohol Use - Q4'
,c0.Score as 'CAGE / Alcohol Use - Total'
,c0.ScoreLevel as 'CAGE / Alcohol Use - Score Level'

-- dast
,d1.AnswerValue as 'DAST-10 / Drug Use - Q1'
,d2.AnswerValue as 'DAST-10 / Drug Use - Q2'
,ABS(d3.AnswerValue - 1) as 'DAST-10 / Drug Use - Q3'
,d4.AnswerValue as 'DAST-10 / Drug Use - Q4'
,d5.AnswerValue as 'DAST-10 / Drug Use - Q5'
,d6.AnswerValue as 'DAST-10 / Drug Use - Q6'
,d7.AnswerValue as 'DAST-10 / Drug Use - Q7'
,d8.AnswerValue as 'DAST-10 / Drug Use - Q8'
,d9.AnswerValue as 'DAST-10 / Drug Use - Q9'
,d10.AnswerValue as 'DAST-10 / Drug Use - Q10'

,d0.Score as 'DAST-10 / Drug Use - Total'
,d0.ScoreLevel as 'DAST-10 / Drug Use - Score Level'

--phq
,p1.AnswerValue as 'PHQ-9 / Depression - Q1'
,p2.AnswerValue as 'PHQ-9 / Depression - Q2'
,p3.AnswerValue as 'PHQ-9 / Depression - Q3'
,p4.AnswerValue as 'PHQ-9 / Depression - Q4'
,p5.AnswerValue as 'PHQ-9 / Depression - Q5'
,p6.AnswerValue as 'PHQ-9 / Depression - Q6'
,p7.AnswerValue as 'PHQ-9 / Depression - Q7'
,p8.AnswerValue as 'PHQ-9 / Depression - Q8'
,p9.AnswerValue as 'PHQ-9 / Depression - Q9'
--,p10.AnswerValue as 'PHQ-9 / Depression - Q10'
,p10a.OptionText as 'PHQ-9 / Depression - Q10'


,p0.Score as 'PHQ-9 / Depression - Total'
,p0.ScoreLevel as 'PHQ-9 / Depression - Score Level'

--hits
,h1.AnswerValue as 'HITS / Violence - Q1'
,h2.AnswerValue as 'HITS / Violence - Q2'
,h3.AnswerValue as 'HITS / Violence - Q3'
,h4.AnswerValue as 'HITS / Violence - Q4'

,h0.Score as 'HITS / Violence - Total'
,h0.ScoreLevel as 'HITS / Violence - Score Level'
--
,doch1a.Name as 'Primary Drug'
,doch2a.Name as 'Secondary Drug'
,doch3a.Name as 'Tertiary Drug'


--gad
,gad1.AnswerValue as 'GAD-7 / Anxiety - Q1'
,gad2.AnswerValue as 'GAD-7 / Anxiety - Q2'
,gad3.AnswerValue as 'GAD-7 / Anxiety - Q3'
,gad4.AnswerValue as 'GAD-7 / Anxiety - Q4'
,gad5.AnswerValue as 'GAD-7 / Anxiety - Q5'
,gad6.AnswerValue as 'GAD-7 / Anxiety - Q6'
,gad7.AnswerValue as 'GAD-7 / Anxiety - Q7'
,gad8a.OptionText as 'GAD-7 / Anxiety - Q8'

,gad0.Score as 'GAD-7 / Anxiety - Total'
,gad0.ScoreLevel as 'GAD-7 / Anxiety - Score Level'


--hits
,bbgs1.AnswerValue as 'BBGS / Problem Gambling - Q1'
,bbgs2.AnswerValue as 'BBGS / Problem Gambling - Q2'
,bbgs3.AnswerValue as 'BBGS / Problem Gambling - Q3'
,bbgs4.AnswerValue as 'BBGS / Problem Gambling - Q4'

,bbgs0.Score as 'BBGS / Problem Gambling - Total'
,bbgs0.ScoreLevel as 'BBGS / Problem Gambling - Score Level'
--


,ISNULL(demographics.ID,'') as 'DemographicsId'


FROM dbo.ScreeningResult r
    INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID
    INNER JOIN dbo.BranchLocation l ON l.BranchLocationID = k.BranchLocationID
    LEFT JOIN dbo.ScreeningSectionQuestionResult q1 ON r.ScreeningResultID = q1.ScreeningResultID AND q1.ScreeningSectionID = 'TCC' AND q1.QuestionID = 1 
    LEFT JOIN dbo.ScreeningSectionQuestionResult q2 ON r.ScreeningResultID = q2.ScreeningResultID AND q2.ScreeningSectionID = 'TCC' AND q2.QuestionID = 2 
    LEFT JOIN dbo.ScreeningSectionQuestionResult q3 ON r.ScreeningResultID = q3.ScreeningResultID AND q3.ScreeningSectionID = 'TCC' AND q3.QuestionID = 3 
    --cage
    LEFT JOIN dbo.ScreeningSectionQuestionResult c1 ON r.ScreeningResultID = c1.ScreeningResultID AND c1.ScreeningSectionID = 'CAGE' AND c1.QuestionID = 1 
    LEFT JOIN dbo.ScreeningSectionQuestionResult c2 ON r.ScreeningResultID = c2.ScreeningResultID AND c2.ScreeningSectionID = 'CAGE' AND c2.QuestionID = 2 
    LEFT JOIN dbo.ScreeningSectionQuestionResult c3 ON r.ScreeningResultID = c3.ScreeningResultID AND c3.ScreeningSectionID = 'CAGE' AND c3.QuestionID = 3 
    LEFT JOIN dbo.ScreeningSectionQuestionResult c4 ON r.ScreeningResultID = c4.ScreeningResultID AND c4.ScreeningSectionID = 'CAGE' AND c4.QuestionID = 4 
    
    LEFT JOIN dbo.ScreeningSectionResult c0 ON r.ScreeningResultID = c0.ScreeningResultID AND c0.ScreeningSectionID = 'CAGE'
    
    -- dast

    LEFT JOIN dbo.ScreeningSectionQuestionResult d1 ON r.ScreeningResultID = d1.ScreeningResultID AND d1.ScreeningSectionID = 'DAST' AND d1.QuestionID = 10 
    LEFT JOIN dbo.ScreeningSectionQuestionResult d2 ON r.ScreeningResultID = d2.ScreeningResultID AND d2.ScreeningSectionID = 'DAST' AND d2.QuestionID = 1 
    LEFT JOIN dbo.ScreeningSectionQuestionResult d3 ON r.ScreeningResultID = d3.ScreeningResultID AND d3.ScreeningSectionID = 'DAST' AND d3.QuestionID = 2 
    LEFT JOIN dbo.ScreeningSectionQuestionResult d4 ON r.ScreeningResultID = d4.ScreeningResultID AND d4.ScreeningSectionID = 'DAST' AND d4.QuestionID = 3 
    LEFT JOIN dbo.ScreeningSectionQuestionResult d5 ON r.ScreeningResultID = d5.ScreeningResultID AND d5.ScreeningSectionID = 'DAST' AND d5.QuestionID = 4 	 
    LEFT JOIN dbo.ScreeningSectionQuestionResult d6 ON r.ScreeningResultID = d6.ScreeningResultID AND d6.ScreeningSectionID = 'DAST' AND d6.QuestionID = 5 	 
    LEFT JOIN dbo.ScreeningSectionQuestionResult d7 ON r.ScreeningResultID = d7.ScreeningResultID AND d7.ScreeningSectionID = 'DAST' AND d7.QuestionID = 6 	 
    LEFT JOIN dbo.ScreeningSectionQuestionResult d8 ON r.ScreeningResultID = d8.ScreeningResultID AND d8.ScreeningSectionID = 'DAST' AND d8.QuestionID = 7 	 
    LEFT JOIN dbo.ScreeningSectionQuestionResult d9 ON r.ScreeningResultID = d9.ScreeningResultID AND d9.ScreeningSectionID = 'DAST' AND d9.QuestionID = 8 	 
    LEFT JOIN dbo.ScreeningSectionQuestionResult d10 ON r.ScreeningResultID = d10.ScreeningResultID AND d10.ScreeningSectionID = 'DAST' AND d10.QuestionID = 9 	 
    
    LEFT JOIN dbo.ScreeningSectionResult d0 ON r.ScreeningResultID = d0.ScreeningResultID AND d0.ScreeningSectionID = 'DAST'

    --phq
    LEFT JOIN dbo.ScreeningSectionQuestionResult p1 ON r.ScreeningResultID = p1.ScreeningResultID AND p1.ScreeningSectionID = 'PHQ-9' AND p1.QuestionID = 1 
    LEFT JOIN dbo.ScreeningSectionQuestionResult p2 ON r.ScreeningResultID = p2.ScreeningResultID AND p2.ScreeningSectionID = 'PHQ-9' AND p2.QuestionID = 2 
    LEFT JOIN dbo.ScreeningSectionQuestionResult p3 ON r.ScreeningResultID = p3.ScreeningResultID AND p3.ScreeningSectionID = 'PHQ-9' AND p3.QuestionID = 3 
    LEFT JOIN dbo.ScreeningSectionQuestionResult p4 ON r.ScreeningResultID = p4.ScreeningResultID AND p4.ScreeningSectionID = 'PHQ-9' AND p4.QuestionID = 4 
    LEFT JOIN dbo.ScreeningSectionQuestionResult p5 ON r.ScreeningResultID = p5.ScreeningResultID AND p5.ScreeningSectionID = 'PHQ-9' AND p5.QuestionID = 5 
    LEFT JOIN dbo.ScreeningSectionQuestionResult p6 ON r.ScreeningResultID = p6.ScreeningResultID AND p6.ScreeningSectionID = 'PHQ-9' AND p6.QuestionID = 6 
    LEFT JOIN dbo.ScreeningSectionQuestionResult p7 ON r.ScreeningResultID = p7.ScreeningResultID AND p7.ScreeningSectionID = 'PHQ-9' AND p7.QuestionID = 7 
    LEFT JOIN dbo.ScreeningSectionQuestionResult p8 ON r.ScreeningResultID = p8.ScreeningResultID AND p8.ScreeningSectionID = 'PHQ-9' AND p8.QuestionID = 8 
    LEFT JOIN dbo.ScreeningSectionQuestionResult p9 ON r.ScreeningResultID = p9.ScreeningResultID AND p9.ScreeningSectionID = 'PHQ-9' AND p9.QuestionID = 9 
    LEFT JOIN dbo.ScreeningSectionQuestionResult p10 ON r.ScreeningResultID = p10.ScreeningResultID AND p10.ScreeningSectionID = 'PHQ-9' AND p10.QuestionID = 10
        LEFT JOIN dbo.ScreeningSectionQuestion p10q ON p10q.ScreeningSectionID = p10.ScreeningSectionID AND p10q.QuestionID = p10.QuestionID
        LEFT JOIN dbo.AnswerScaleOption p10a ON p10a.AnswerScaleID = p10q.AnswerScaleID AND p10a.OptionValue = p10.AnswerValue

    LEFT JOIN dbo.ScreeningSectionResult p0 ON r.ScreeningResultID = p0.ScreeningResultID AND p0.ScreeningSectionID = 'PHQ-9'
            
    --hits
    LEFT JOIN dbo.ScreeningSectionQuestionResult h1 ON r.ScreeningResultID = h1.ScreeningResultID AND h1.ScreeningSectionID = 'HITS' AND h1.QuestionID = 1
    LEFT JOIN dbo.ScreeningSectionQuestionResult h2 ON r.ScreeningResultID = h2.ScreeningResultID AND h2.ScreeningSectionID = 'HITS' AND h2.QuestionID = 2
    LEFT JOIN dbo.ScreeningSectionQuestionResult h3 ON r.ScreeningResultID = h3.ScreeningResultID AND h3.ScreeningSectionID = 'HITS' AND h3.QuestionID = 3
    LEFT JOIN dbo.ScreeningSectionQuestionResult h4 ON r.ScreeningResultID = h4.ScreeningResultID AND h4.ScreeningSectionID = 'HITS' AND h4.QuestionID = 4
    LEFT JOIN dbo.ScreeningSectionResult h0 ON r.ScreeningResultID = h0.ScreeningResultID AND h0.ScreeningSectionID = 'HITS'


        --doch
    LEFT JOIN dbo.ScreeningSectionQuestionResult doch1 ON r.ScreeningResultID = doch1.ScreeningResultID AND doch1.ScreeningSectionID = 'DOCH' AND doch1.QuestionID = 1
        LEFT JOIN dbo.DrugOfChoice doch1a ON doch1.AnswerValue = doch1a.ID
    LEFT JOIN dbo.ScreeningSectionQuestionResult doch2 ON r.ScreeningResultID = doch2.ScreeningResultID AND doch2.ScreeningSectionID = 'DOCH' AND doch2.QuestionID = 2
        LEFT JOIN dbo.DrugOfChoice doch2a ON ISNULL(doch2.AnswerValue,0) = doch2a.ID
    LEFT JOIN dbo.ScreeningSectionQuestionResult doch3 ON r.ScreeningResultID = doch3.ScreeningResultID AND doch3.ScreeningSectionID = 'DOCH' AND doch3.QuestionID = 3
        LEFT JOIN dbo.DrugOfChoice doch3a ON ISNULL(doch3.AnswerValue,0) = doch3a.ID


    --phq
    LEFT JOIN dbo.ScreeningSectionQuestionResult gad1 ON r.ScreeningResultID = gad1.ScreeningResultID AND gad1.ScreeningSectionID = 'GAD-7' AND gad1.QuestionID = 1 
    LEFT JOIN dbo.ScreeningSectionQuestionResult gad2 ON r.ScreeningResultID = gad2.ScreeningResultID AND gad2.ScreeningSectionID = 'GAD-7' AND gad2.QuestionID = 2 
    LEFT JOIN dbo.ScreeningSectionQuestionResult gad3 ON r.ScreeningResultID = gad3.ScreeningResultID AND gad3.ScreeningSectionID = 'GAD-7' AND gad3.QuestionID = 3 
    LEFT JOIN dbo.ScreeningSectionQuestionResult gad4 ON r.ScreeningResultID = gad4.ScreeningResultID AND gad4.ScreeningSectionID = 'GAD-7' AND gad4.QuestionID = 4 
    LEFT JOIN dbo.ScreeningSectionQuestionResult gad5 ON r.ScreeningResultID = gad5.ScreeningResultID AND gad5.ScreeningSectionID = 'GAD-7' AND gad5.QuestionID = 5 
    LEFT JOIN dbo.ScreeningSectionQuestionResult gad6 ON r.ScreeningResultID = gad6.ScreeningResultID AND gad6.ScreeningSectionID = 'GAD-7' AND gad6.QuestionID = 6 
    LEFT JOIN dbo.ScreeningSectionQuestionResult gad7 ON r.ScreeningResultID = gad7.ScreeningResultID AND gad7.ScreeningSectionID = 'GAD-7' AND gad7.QuestionID = 7 
     LEFT JOIN dbo.ScreeningSectionQuestionResult gad8 ON r.ScreeningResultID = gad8.ScreeningResultID AND gad8.ScreeningSectionID = 'GAD-7' AND gad8.QuestionID = 8
        LEFT JOIN dbo.ScreeningSectionQuestion gad8q ON gad8q.ScreeningSectionID = gad8.ScreeningSectionID AND gad8q.QuestionID = gad8.QuestionID
        LEFT JOIN dbo.AnswerScaleOption gad8a ON gad8a.AnswerScaleID = gad8q.AnswerScaleID AND gad8a.OptionValue = gad8.AnswerValue

    LEFT JOIN dbo.ScreeningSectionResult gad0 ON r.ScreeningResultID = gad0.ScreeningResultID AND gad0.ScreeningSectionID = 'GAD-7'
    

    --gambling
    LEFT JOIN dbo.ScreeningSectionQuestionResult bbgs1 ON r.ScreeningResultID = bbgs1.ScreeningResultID AND bbgs1.ScreeningSectionID = 'BBGS' AND bbgs1.QuestionID = 1
    LEFT JOIN dbo.ScreeningSectionQuestionResult bbgs2 ON r.ScreeningResultID = bbgs2.ScreeningResultID AND bbgs2.ScreeningSectionID = 'BBGS' AND bbgs2.QuestionID = 2
    LEFT JOIN dbo.ScreeningSectionQuestionResult bbgs3 ON r.ScreeningResultID = bbgs3.ScreeningResultID AND bbgs3.ScreeningSectionID = 'BBGS' AND bbgs3.QuestionID = 3
    LEFT JOIN dbo.ScreeningSectionQuestionResult bbgs4 ON r.ScreeningResultID = bbgs4.ScreeningResultID AND bbgs4.ScreeningSectionID = 'BBGS' AND bbgs4.QuestionID = 4
    LEFT JOIN dbo.ScreeningSectionResult bbgs0 ON r.ScreeningResultID = bbgs0.ScreeningResultID AND bbgs0.ScreeningSectionID = 'BBGS'

    
    -- demogr
    LEFT JOIN (SELECT MAX(d.ID) as ID, d.PatientName, d.Birthday FROM dbo.BhsDemographics d GROUP BY d.PatientName, d.Birthday) demographics ON  demographics.PatientName = r.PatientName AND demographics.Birthday = r.Birthday
WHERE r.IsDeleted = 0


GO
---------------------------


ALTER PROCEDURE [dbo].[uspGetUniquePatientScreeningResultsForExcelExport]
    @StartDate DateTimeOffset,
    @EndDate DateTimeOffset,
    @LocationID int  = NULL
AS


SELECT
r.ScreeningResultID as 'ScreeDox Record No.',
CONVERT(char(10),r.CreatedDate, 101) as 'ScreeningDate',
r.LastName,
r.FirstName,
ISNULL(r.MiddleName,'') as MiddleName,
CONVERT(char(10), r.Birthday, 101) as 'Birthday'
,l.BranchLocationID as 'LocationID'
,l.Name as 'Location'


-- tobacco
,tcc.[Tobacco Use - Ceremony]
,tcc.[Tobacco Use - Smoking]
,tcc.[Tobacco Use - Smokeless]
,tcc.[Tobaco Use - ScreeningDate]

,cage.[CAGE / Alcohol Use - Q1]
,cage.[CAGE / Alcohol Use - Q2]
,cage.[CAGE / Alcohol Use - Q3]
,cage.[CAGE / Alcohol Use - Q4]
,cage.[CAGE / Alcohol Use - Total]
,cage.[CAGE / Alcohol Use - Score Level]
,[CAGE / Alcohol Use - ScreeningDate]


-- dast
,dast.[DAST-10 / Drug Use - Q1]
,dast.[DAST-10 / Drug Use - Q2]
,dast.[DAST-10 / Drug Use - Q3]
,dast.[DAST-10 / Drug Use - Q4]
,dast.[DAST-10 / Drug Use - Q5]
,dast.[DAST-10 / Drug Use - Q6]
,dast.[DAST-10 / Drug Use - Q7]
,dast.[DAST-10 / Drug Use - Q8]
,dast.[DAST-10 / Drug Use - Q9]
,dast.[DAST-10 / Drug Use - Q10]
,dast.[DAST-10 / Drug Use - Total]
,dast.[DAST-10 / Drug Use - Score Level]
,dast.[DAST-10 / Drug Use - ScreeningDate]

--phq
,phq.[PHQ-9 / Depression - Q1]
,phq.[PHQ-9 / Depression - Q2]
,phq.[PHQ-9 / Depression - Q3]
,phq.[PHQ-9 / Depression - Q4]
,phq.[PHQ-9 / Depression - Q5]
,phq.[PHQ-9 / Depression - Q6]
,phq.[PHQ-9 / Depression - Q7]
,phq.[PHQ-9 / Depression - Q8]
,phq.[PHQ-9 / Depression - Q9]
,phq.[PHQ-9 / Depression - Q10]

,phq.[PHQ-9 / Depression - Total]
,phq.[PHQ-9 / Depression - Score Level]
,phq.[PHQ-9 / Depression - ScreeningDate]


--hits
,hits.[HITS / Violence - Q1]
,hits.[HITS / Violence - Q2]
,hits.[HITS / Violence - Q3]
,hits.[HITS / Violence - Q4]

,hits.[HITS / Violence - Total]
,hits.[HITS / Violence - Score Level]
,hits.[HITS / Violence - ScreeningDate]

-- doch
,doch.[Primary Drug]
,doch.[Secondary Drug]
,doch.[Tertiary Drug]
,doch.[Drug Use - ScreeningDate]

--gad
,gad.[GAD-7 / Anxiety - Q1]
,gad.[GAD-7 / Anxiety - Q2]
,gad.[GAD-7 / Anxiety - Q3]
,gad.[GAD-7 / Anxiety - Q4]
,gad.[GAD-7 / Anxiety - Q5]
,gad.[GAD-7 / Anxiety - Q6]
,gad.[GAD-7 / Anxiety - Q7]
,gad.[GAD-7 / Anxiety - Q8]

,gad.[GAD-7 / Anxiety - Total]
,gad.[GAD-7 / Anxiety - Score Level]
,gad.[GAD-7 / Anxiety - ScreeningDate]


--bbgs
,bbgs.[BBGS / Problem Gambling - Q1]
,bbgs.[BBGS / Problem Gambling - Q2]
,bbgs.[BBGS / Problem Gambling - Q3]
,bbgs.[BBGS / Problem Gambling - Q4]

,bbgs.[BBGS / Problem Gambling - Total]
,bbgs.[BBGS / Problem Gambling - Score Level]
,bbgs.[BBGS / Problem Gambling - ScreeningDate]


-- demographics
,ISNULL(demographics.ID,'') as 'DemographicsId'

FROM 
    [dbo].[fn_GetUniquePatientScreenings] ( @StartDate, @EndDate, @LocationID) as ur
    INNER JOIN dbo.ScreeningResult r ON r.ScreeningResultID = ur.ScreeningResultID
    INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID
    INNER JOIN dbo.BranchLocation l ON l.BranchLocationID = k.BranchLocationID
    
    -- tobacco
    OUTER APPLY (
        SELECT TOP (1)
            q1.AnswerValue as 'Tobacco Use - Ceremony'
            ,q2.AnswerValue as 'Tobacco Use - Smoking'
            ,q3.AnswerValue as 'Tobacco Use - Smokeless'
            ,CONVERT(char(10), r01.CreatedDate, 101) as 'Tobaco Use - ScreeningDate'
        
        FROM 
            dbo.ScreeningResult r01 
            INNER JOIN dbo.ScreeningSectionQuestionResult q4 ON r01.ScreeningResultID = q4.ScreeningResultID AND q4.ScreeningSectionID = 'TCC' AND q4.QuestionID = 4
            LEFT JOIN dbo.ScreeningSectionQuestionResult q1 ON r01.ScreeningResultID = q1.ScreeningResultID AND q1.ScreeningSectionID = 'TCC' AND q1.QuestionID = 1 
            LEFT JOIN dbo.ScreeningSectionQuestionResult q2 ON r01.ScreeningResultID = q2.ScreeningResultID AND q2.ScreeningSectionID = 'TCC' AND q2.QuestionID = 2 
            LEFT JOIN dbo.ScreeningSectionQuestionResult q3 ON r01.ScreeningResultID = q3.ScreeningResultID AND q3.ScreeningSectionID = 'TCC' AND q3.QuestionID = 3 
        WHERE r01.PatientName = ur.PatientName AND r01.Birthday = ur.Birthday 
            AND r01.CreatedDate <= r.CreatedDate
        ORDER BY r01.CreatedDate DESC
    ) as tcc

    --cage
    OUTER APPLY 
    (
        SELECT TOP (1)
            c1.AnswerValue as 'CAGE / Alcohol Use - Q1'
            ,c2.AnswerValue as 'CAGE / Alcohol Use - Q2'
            ,c3.AnswerValue as 'CAGE / Alcohol Use - Q3'
            ,c4.AnswerValue as 'CAGE / Alcohol Use - Q4'
            ,c0.Score as 'CAGE / Alcohol Use - Total'
            ,c0.ScoreLevel as 'CAGE / Alcohol Use - Score Level'
            ,CONVERT(char(10), r01.CreatedDate, 101) as 'CAGE / Alcohol Use - ScreeningDate'
        FROM dbo.ScreeningResult r01
            INNER JOIN dbo.ScreeningSectionResult c0 ON r01.ScreeningResultID = c0.ScreeningResultID AND c0.ScreeningSectionID = 'CAGE'

            LEFT JOIN dbo.ScreeningSectionQuestionResult c1 ON r01.ScreeningResultID = c1.ScreeningResultID AND c1.ScreeningSectionID = 'CAGE' AND c1.QuestionID = 1 
            LEFT JOIN dbo.ScreeningSectionQuestionResult c2 ON r01.ScreeningResultID = c2.ScreeningResultID AND c2.ScreeningSectionID = 'CAGE' AND c2.QuestionID = 2 
            LEFT JOIN dbo.ScreeningSectionQuestionResult c3 ON r01.ScreeningResultID = c3.ScreeningResultID AND c3.ScreeningSectionID = 'CAGE' AND c3.QuestionID = 3 
            LEFT JOIN dbo.ScreeningSectionQuestionResult c4 ON r01.ScreeningResultID = c4.ScreeningResultID AND c4.ScreeningSectionID = 'CAGE' AND c4.QuestionID = 4 
    
            
        WHERE r01.PatientName = r.PatientName AND r01.Birthday = r.Birthday
            AND r01.CreatedDate <= r.CreatedDate
        ORDER BY r01.CreatedDate DESC
    ) as cage

    -- dast
    OUTER APPLY 
    (
        SELECT TOP (1)
            d1.AnswerValue as 'DAST-10 / Drug Use - Q1'
            ,d2.AnswerValue as 'DAST-10 / Drug Use - Q2'
            ,ABS(d3.AnswerValue - 1) as 'DAST-10 / Drug Use - Q3'
            ,d4.AnswerValue as 'DAST-10 / Drug Use - Q4'
            ,d5.AnswerValue as 'DAST-10 / Drug Use - Q5'
            ,d6.AnswerValue as 'DAST-10 / Drug Use - Q6'
            ,d7.AnswerValue as 'DAST-10 / Drug Use - Q7'
            ,d8.AnswerValue as 'DAST-10 / Drug Use - Q8'
            ,d9.AnswerValue as 'DAST-10 / Drug Use - Q9'
            ,d10.AnswerValue as 'DAST-10 / Drug Use - Q10'

            ,d0.Score as 'DAST-10 / Drug Use - Total'
            ,d0.ScoreLevel as 'DAST-10 / Drug Use - Score Level'
            ,CONVERT(char(10), r01.CreatedDate, 101) as 'DAST-10 / Drug Use - ScreeningDate'
        FROM dbo.ScreeningResult r01
            INNER JOIN dbo.ScreeningSectionResult d0 ON r01.ScreeningResultID = d0.ScreeningResultID AND d0.ScreeningSectionID = 'DAST'
            INNER JOIN dbo.ScreeningSectionQuestionResult d1 ON r01.ScreeningResultID = d1.ScreeningResultID AND d1.ScreeningSectionID = 'DAST' AND d1.QuestionID = 10 
            LEFT JOIN dbo.ScreeningSectionQuestionResult d2 ON r01.ScreeningResultID = d2.ScreeningResultID AND d2.ScreeningSectionID = 'DAST' AND d2.QuestionID = 1 
            LEFT JOIN dbo.ScreeningSectionQuestionResult d3 ON r01.ScreeningResultID = d3.ScreeningResultID AND d3.ScreeningSectionID = 'DAST' AND d3.QuestionID = 2 
            LEFT JOIN dbo.ScreeningSectionQuestionResult d4 ON r01.ScreeningResultID = d4.ScreeningResultID AND d4.ScreeningSectionID = 'DAST' AND d4.QuestionID = 3 
            LEFT JOIN dbo.ScreeningSectionQuestionResult d5 ON r01.ScreeningResultID = d5.ScreeningResultID AND d5.ScreeningSectionID = 'DAST' AND d5.QuestionID = 4 	 
            LEFT JOIN dbo.ScreeningSectionQuestionResult d6 ON r01.ScreeningResultID = d6.ScreeningResultID AND d6.ScreeningSectionID = 'DAST' AND d6.QuestionID = 5 	 
            LEFT JOIN dbo.ScreeningSectionQuestionResult d7 ON r01.ScreeningResultID = d7.ScreeningResultID AND d7.ScreeningSectionID = 'DAST' AND d7.QuestionID = 6 	 
            LEFT JOIN dbo.ScreeningSectionQuestionResult d8 ON r01.ScreeningResultID = d8.ScreeningResultID AND d8.ScreeningSectionID = 'DAST' AND d8.QuestionID = 7 	 
            LEFT JOIN dbo.ScreeningSectionQuestionResult d9 ON r01.ScreeningResultID = d9.ScreeningResultID AND d9.ScreeningSectionID = 'DAST' AND d9.QuestionID = 8 	 
            LEFT JOIN dbo.ScreeningSectionQuestionResult d10 ON r01.ScreeningResultID = d10.ScreeningResultID AND d10.ScreeningSectionID = 'DAST' AND d10.QuestionID = 9 	 
            
        WHERE r01.PatientName = r.PatientName AND r01.Birthday = r.Birthday
                    AND r01.CreatedDate <= r.CreatedDate 
        ORDER BY r01.CreatedDate DESC
    ) as dast


    --phq
    OUTER APPLY 
    (
    SELECT TOP (1)
        p1.AnswerValue as 'PHQ-9 / Depression - Q1'
        ,p2.AnswerValue as 'PHQ-9 / Depression - Q2'
        ,p3.AnswerValue as 'PHQ-9 / Depression - Q3'
        ,p4.AnswerValue as 'PHQ-9 / Depression - Q4'
        ,p5.AnswerValue as 'PHQ-9 / Depression - Q5'
        ,p6.AnswerValue as 'PHQ-9 / Depression - Q6'
        ,p7.AnswerValue as 'PHQ-9 / Depression - Q7'
        ,p8.AnswerValue as 'PHQ-9 / Depression - Q8'
        ,p9.AnswerValue as 'PHQ-9 / Depression - Q9'
        ,p10a.OptionText as 'PHQ-9 / Depression - Q10'
        ,p0.Score as 'PHQ-9 / Depression - Total'
        ,p0.ScoreLevel as 'PHQ-9 / Depression - Score Level'
        ,CONVERT(char(10), r01.CreatedDate, 101) as 'PHQ-9 / Depression - ScreeningDate'

    FROM dbo.ScreeningResult r01
        INNER JOIN dbo.ScreeningSectionResult p0 ON r01.ScreeningResultID = p0.ScreeningResultID AND p0.ScreeningSectionID = 'PHQ-9'
        INNER JOIN dbo.ScreeningSectionQuestionResult p1 ON r01.ScreeningResultID = p1.ScreeningResultID AND p1.ScreeningSectionID = 'PHQ-9' AND p1.QuestionID = 1 
        INNER JOIN dbo.ScreeningSectionQuestionResult p2 ON r01.ScreeningResultID = p2.ScreeningResultID AND p2.ScreeningSectionID = 'PHQ-9' AND p2.QuestionID = 2 
        LEFT JOIN dbo.ScreeningSectionQuestionResult p3 ON r01.ScreeningResultID = p3.ScreeningResultID AND p3.ScreeningSectionID = 'PHQ-9' AND p3.QuestionID = 3 
        LEFT JOIN dbo.ScreeningSectionQuestionResult p4 ON r01.ScreeningResultID = p4.ScreeningResultID AND p4.ScreeningSectionID = 'PHQ-9' AND p4.QuestionID = 4 
        LEFT JOIN dbo.ScreeningSectionQuestionResult p5 ON r01.ScreeningResultID = p5.ScreeningResultID AND p5.ScreeningSectionID = 'PHQ-9' AND p5.QuestionID = 5 
        LEFT JOIN dbo.ScreeningSectionQuestionResult p6 ON r01.ScreeningResultID = p6.ScreeningResultID AND p6.ScreeningSectionID = 'PHQ-9' AND p6.QuestionID = 6 
        LEFT JOIN dbo.ScreeningSectionQuestionResult p7 ON r01.ScreeningResultID = p7.ScreeningResultID AND p7.ScreeningSectionID = 'PHQ-9' AND p7.QuestionID = 7 
        LEFT JOIN dbo.ScreeningSectionQuestionResult p8 ON r01.ScreeningResultID = p8.ScreeningResultID AND p8.ScreeningSectionID = 'PHQ-9' AND p8.QuestionID = 8 
        LEFT JOIN dbo.ScreeningSectionQuestionResult p9 ON r01.ScreeningResultID = p9.ScreeningResultID AND p9.ScreeningSectionID = 'PHQ-9' AND p9.QuestionID = 9 
        LEFT JOIN dbo.ScreeningSectionQuestionResult p10 ON r01.ScreeningResultID = p10.ScreeningResultID AND p10.ScreeningSectionID = 'PHQ-9' AND p10.QuestionID = 10
            LEFT JOIN dbo.ScreeningSectionQuestion p10q ON p10q.ScreeningSectionID = p10.ScreeningSectionID AND p10q.QuestionID = p10.QuestionID
            LEFT JOIN dbo.AnswerScaleOption p10a ON p10a.AnswerScaleID = p10q.AnswerScaleID AND p10a.OptionValue = p10.AnswerValue
    
        WHERE r01.PatientName = r.PatientName AND r01.Birthday = r.Birthday
                        AND r01.CreatedDate <= r.CreatedDate 
        ORDER BY r01.CreatedDate DESC

    ) as phq

            
    --hits
    OUTER APPLY (
        SELECT TOP(1)
        h1.AnswerValue as 'HITS / Violence - Q1'
        ,h2.AnswerValue as 'HITS / Violence - Q2'
        ,h3.AnswerValue as 'HITS / Violence - Q3'
        ,h4.AnswerValue as 'HITS / Violence - Q4'

        ,h0.Score as 'HITS / Violence - Total'
        ,h0.ScoreLevel as 'HITS / Violence - Score Level'
        ,CONVERT(char(10), r01.CreatedDate, 101) as 'HITS / Violence - ScreeningDate'
        FROM dbo.ScreeningResult r01
            INNER JOIN dbo.ScreeningSectionResult h0 ON r01.ScreeningResultID = h0.ScreeningResultID AND h0.ScreeningSectionID = 'HITS'
            LEFT JOIN dbo.ScreeningSectionQuestionResult h1 ON r01.ScreeningResultID = h1.ScreeningResultID AND h1.ScreeningSectionID = 'HITS' AND h1.QuestionID = 1
            LEFT JOIN dbo.ScreeningSectionQuestionResult h2 ON r01.ScreeningResultID = h2.ScreeningResultID AND h2.ScreeningSectionID = 'HITS' AND h2.QuestionID = 2
            LEFT JOIN dbo.ScreeningSectionQuestionResult h3 ON r01.ScreeningResultID = h3.ScreeningResultID AND h3.ScreeningSectionID = 'HITS' AND h3.QuestionID = 3
            LEFT JOIN dbo.ScreeningSectionQuestionResult h4 ON r01.ScreeningResultID = h4.ScreeningResultID AND h4.ScreeningSectionID = 'HITS' AND h4.QuestionID = 4
    
        WHERE r01.PatientName = r.PatientName AND r01.Birthday = r.Birthday
                        AND r01.CreatedDate <= r.CreatedDate 
        ORDER BY r01.CreatedDate DESC
    ) as hits

        --doch
    OUTER APPLY (
        SELECT TOP (1)
            doch1a.Name as 'Primary Drug'
            ,doch2a.Name as 'Secondary Drug'
            ,doch3a.Name as 'Tertiary Drug'
            ,CONVERT(char(10), r01.CreatedDate, 101) as 'Drug Use - ScreeningDate'
        FROM dbo.ScreeningResult r01
            INNER JOIN dbo.ScreeningSectionQuestionResult doch1 ON r01.ScreeningResultID = doch1.ScreeningResultID AND doch1.ScreeningSectionID = 'DOCH' AND doch1.QuestionID = 1
                LEFT JOIN dbo.DrugOfChoice doch1a ON doch1.AnswerValue = doch1a.ID
            LEFT JOIN dbo.ScreeningSectionQuestionResult doch2 ON r01.ScreeningResultID = doch2.ScreeningResultID AND doch2.ScreeningSectionID = 'DOCH' AND doch2.QuestionID = 2
                LEFT JOIN dbo.DrugOfChoice doch2a ON ISNULL(doch2.AnswerValue,0) = doch2a.ID
            LEFT JOIN dbo.ScreeningSectionQuestionResult doch3 ON r01.ScreeningResultID = doch3.ScreeningResultID AND doch3.ScreeningSectionID = 'DOCH' AND doch3.QuestionID = 3
                LEFT JOIN dbo.DrugOfChoice doch3a ON ISNULL(doch3.AnswerValue,0) = doch3a.ID
        WHERE r01.PatientName = r.PatientName AND r01.Birthday = r.Birthday
                        AND r01.CreatedDate <= r.CreatedDate 
        ORDER BY r01.CreatedDate DESC
    ) as doch


    -- gad
    OUTER APPLY 
    (
    SELECT TOP (1)
            p1.AnswerValue as 'GAD-7 / Anxiety - Q1'
            ,p2.AnswerValue as 'GAD-7 / Anxiety - Q2'
            ,p3.AnswerValue as 'GAD-7 / Anxiety - Q3'
            ,p4.AnswerValue as 'GAD-7 / Anxiety - Q4'
            ,p5.AnswerValue as 'GAD-7 / Anxiety - Q5'
            ,p6.AnswerValue as 'GAD-7 / Anxiety - Q6'
            ,p7.AnswerValue as 'GAD-7 / Anxiety - Q7'
            ,p8a.OptionText as 'GAD-7 / Anxiety - Q8'
            ,p0.Score as 'GAD-7 / Anxiety - Total'
            ,p0.ScoreLevel as 'GAD-7 / Anxiety - Score Level'
            ,CONVERT(char(10), r01.CreatedDate, 101) as 'GAD-7 / Anxiety - ScreeningDate'

        FROM dbo.ScreeningResult r01
            INNER JOIN dbo.ScreeningSectionResult p0 ON r01.ScreeningResultID = p0.ScreeningResultID AND p0.ScreeningSectionID = 'GAD-7'
            INNER JOIN dbo.ScreeningSectionQuestionResult p1 ON r01.ScreeningResultID = p1.ScreeningResultID AND p1.ScreeningSectionID = 'GAD-7' AND p1.QuestionID = 1 
            INNER JOIN dbo.ScreeningSectionQuestionResult p2 ON r01.ScreeningResultID = p2.ScreeningResultID AND p2.ScreeningSectionID = 'GAD-7' AND p2.QuestionID = 2 
            LEFT JOIN dbo.ScreeningSectionQuestionResult p3 ON r01.ScreeningResultID = p3.ScreeningResultID AND p3.ScreeningSectionID = 'GAD-7' AND p3.QuestionID = 3 
            LEFT JOIN dbo.ScreeningSectionQuestionResult p4 ON r01.ScreeningResultID = p4.ScreeningResultID AND p4.ScreeningSectionID = 'GAD-7' AND p4.QuestionID = 4 
            LEFT JOIN dbo.ScreeningSectionQuestionResult p5 ON r01.ScreeningResultID = p5.ScreeningResultID AND p5.ScreeningSectionID = 'GAD-7' AND p5.QuestionID = 5 
            LEFT JOIN dbo.ScreeningSectionQuestionResult p6 ON r01.ScreeningResultID = p6.ScreeningResultID AND p6.ScreeningSectionID = 'GAD-7' AND p6.QuestionID = 6 
            LEFT JOIN dbo.ScreeningSectionQuestionResult p7 ON r01.ScreeningResultID = p7.ScreeningResultID AND p7.ScreeningSectionID = 'GAD-7' AND p7.QuestionID = 7 
            LEFT JOIN dbo.ScreeningSectionQuestionResult p8 ON r01.ScreeningResultID = p8.ScreeningResultID AND p8.ScreeningSectionID = 'GAD-7' AND p8.QuestionID = 8
                LEFT JOIN dbo.ScreeningSectionQuestion p8q ON p8q.ScreeningSectionID = p8.ScreeningSectionID AND p8q.QuestionID = p8.QuestionID
                LEFT JOIN dbo.AnswerScaleOption p8a ON p8a.AnswerScaleID = p8q.AnswerScaleID AND p8a.OptionValue = p8.AnswerValue
    
            WHERE r01.PatientName = r.PatientName AND r01.Birthday = r.Birthday
                            AND r01.CreatedDate <= r.CreatedDate 
            ORDER BY r01.CreatedDate DESC
    ) as gad

     --bbgs
    OUTER APPLY (
        SELECT TOP(1)
        h1.AnswerValue as 'BBGS / Problem Gambling - Q1'
        ,h2.AnswerValue as 'BBGS / Problem Gambling - Q2'
        ,h3.AnswerValue as 'BBGS / Problem Gambling - Q3'
        ,h4.AnswerValue as 'BBGS / Problem Gambling - Q4'

        ,h0.Score as 'BBGS / Problem Gambling - Total'
        ,h0.ScoreLevel as 'BBGS / Problem Gambling - Score Level'
        ,CONVERT(char(10), r01.CreatedDate, 101) as 'BBGS / Problem Gambling - ScreeningDate'
        FROM dbo.ScreeningResult r01
            INNER JOIN dbo.ScreeningSectionResult h0 ON r01.ScreeningResultID = h0.ScreeningResultID AND h0.ScreeningSectionID = 'BBGS'
            LEFT JOIN dbo.ScreeningSectionQuestionResult h1 ON r01.ScreeningResultID = h1.ScreeningResultID AND h1.ScreeningSectionID = 'BBGS' AND h1.QuestionID = 1
            LEFT JOIN dbo.ScreeningSectionQuestionResult h2 ON r01.ScreeningResultID = h2.ScreeningResultID AND h2.ScreeningSectionID = 'BBGS' AND h2.QuestionID = 2
            LEFT JOIN dbo.ScreeningSectionQuestionResult h3 ON r01.ScreeningResultID = h3.ScreeningResultID AND h3.ScreeningSectionID = 'BBGS' AND h3.QuestionID = 3
            LEFT JOIN dbo.ScreeningSectionQuestionResult h4 ON r01.ScreeningResultID = h4.ScreeningResultID AND h4.ScreeningSectionID = 'BBGS' AND h4.QuestionID = 4
    
        WHERE r01.PatientName = r.PatientName AND r01.Birthday = r.Birthday
                        AND r01.CreatedDate <= r.CreatedDate 
        ORDER BY r01.CreatedDate DESC
    ) as bbgs

    -- demographics
    LEFT JOIN (SELECT MAX(d.ID) as ID, d.PatientName, d.Birthday FROM dbo.BhsDemographics d GROUP BY d.PatientName, d.Birthday) demographics ON  demographics.PatientName = r.PatientName AND demographics.Birthday = r.Birthday
ORDER BY ur.CreatedDate ASC

RETURN 0
GO


---------------------------

ALTER PROCEDURE [dbo].[uspGetUniquePatientCombinedResultsForExcelExport]
    @StartDate DateTimeOffset,
    @EndDate DateTimeOffset,
    @LocationID int  = NULL
AS


SELECT
r.ScreeningResultID as 'ScreeDox Record No.',
CONVERT(char(10),r.CreatedDate, 101) as 'ScreeningDate',
r.LastName,
r.FirstName,
ISNULL(r.MiddleName,'') as MiddleName,
CONVERT(char(10), r.Birthday, 101) as 'Birthday'
,l.BranchLocationID as 'LocationID'
,l.Name as 'Location'


-- tobacco
,tcc.[Tobacco Use - Ceremony]
,tcc.[Tobacco Use - Smoking]
,tcc.[Tobacco Use - Smokeless]
,tcc.[Tobaco Use - ScreeningDate]

,cage.[CAGE / Alcohol Use - Q1]
,cage.[CAGE / Alcohol Use - Q2]
,cage.[CAGE / Alcohol Use - Q3]
,cage.[CAGE / Alcohol Use - Q4]
,cage.[CAGE / Alcohol Use - Total]
,cage.[CAGE / Alcohol Use - Score Level]
,[CAGE / Alcohol Use - ScreeningDate]


-- dast
,dast.[DAST-10 / Drug Use - Q1]
,dast.[DAST-10 / Drug Use - Q2]
,dast.[DAST-10 / Drug Use - Q3]
,dast.[DAST-10 / Drug Use - Q4]
,dast.[DAST-10 / Drug Use - Q5]
,dast.[DAST-10 / Drug Use - Q6]
,dast.[DAST-10 / Drug Use - Q7]
,dast.[DAST-10 / Drug Use - Q8]
,dast.[DAST-10 / Drug Use - Q9]
,dast.[DAST-10 / Drug Use - Q10]
,dast.[DAST-10 / Drug Use - Total]
,dast.[DAST-10 / Drug Use - Score Level]
,dast.[DAST-10 / Drug Use - ScreeningDate]

--phq
,phq.[PHQ-9 / Depression - Q1]
,phq.[PHQ-9 / Depression - Q2]
,phq.[PHQ-9 / Depression - Q3]
,phq.[PHQ-9 / Depression - Q4]
,phq.[PHQ-9 / Depression - Q5]
,phq.[PHQ-9 / Depression - Q6]
,phq.[PHQ-9 / Depression - Q7]
,phq.[PHQ-9 / Depression - Q8]
,phq.[PHQ-9 / Depression - Q9]
,phq.[PHQ-9 / Depression - Q10]

,phq.[PHQ-9 / Depression - Total]
,phq.[PHQ-9 / Depression - Score Level]
,phq.[PHQ-9 / Depression - ScreeningDate]


--hits
,hits.[HITS / Violence - Q1]
,hits.[HITS / Violence - Q2]
,hits.[HITS / Violence - Q3]
,hits.[HITS / Violence - Q4]

,hits.[HITS / Violence - Total]
,hits.[HITS / Violence - Score Level]
,hits.[HITS / Violence - ScreeningDate]

-- doch
,doch.[Primary Drug]
,doch.[Secondary Drug]
,doch.[Tertiary Drug]
,doch.[Drug Use - ScreeningDate]

--gad
,gad.[GAD-7 / Anxiety - Q1]
,gad.[GAD-7 / Anxiety - Q2]
,gad.[GAD-7 / Anxiety - Q3]
,gad.[GAD-7 / Anxiety - Q4]
,gad.[GAD-7 / Anxiety - Q5]
,gad.[GAD-7 / Anxiety - Q6]
,gad.[GAD-7 / Anxiety - Q7]
,gad.[GAD-7 / Anxiety - Q8]

,gad.[GAD-7 / Anxiety - Total]
,gad.[GAD-7 / Anxiety - Score Level]
,gad.[GAD-7 / Anxiety - ScreeningDate]


--bbgs
,bbgs.[BBGS / Problem Gambling - Q1]
,bbgs.[BBGS / Problem Gambling - Q2]
,bbgs.[BBGS / Problem Gambling - Q3]
,bbgs.[BBGS / Problem Gambling - Q4]

,bbgs.[BBGS / Problem Gambling - Total]
,bbgs.[BBGS / Problem Gambling - Score Level]
,bbgs.[BBGS / Problem Gambling - ScreeningDate]

-- demographics
,ISNULL(demographics.ID,'') as 'DemographicsId'
,RaceId
,RaceName
,GenderId
,GenderName
,SexualOrientationId
,SexualOrientationName
,TribalAffiliation
,MaritalStatusId
,MaritalStatusName
,EducationLevelId
,EducationLevelName
,LivingOnReservationId
,LivingOnReservationName
,CountyOfResidence
,MilitaryExperienceId
,MilitaryExperienceName

FROM 
    [dbo].[fn_GetUniquePatientScreenings] ( @StartDate, @EndDate, @LocationID) as ur
    INNER JOIN dbo.ScreeningResult r ON r.ScreeningResultID = ur.ScreeningResultID
    INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID
    INNER JOIN dbo.BranchLocation l ON l.BranchLocationID = k.BranchLocationID
    
    -- tobacco
    OUTER APPLY (
        SELECT TOP (1)
            q1.AnswerValue as 'Tobacco Use - Ceremony'
            ,q2.AnswerValue as 'Tobacco Use - Smoking'
            ,q3.AnswerValue as 'Tobacco Use - Smokeless'
            ,CONVERT(char(10), r01.CreatedDate, 101) as 'Tobaco Use - ScreeningDate'
        
        FROM 
            dbo.ScreeningResult r01 
            INNER JOIN dbo.ScreeningSectionQuestionResult q4 ON r01.ScreeningResultID = q4.ScreeningResultID AND q4.ScreeningSectionID = 'TCC' AND q4.QuestionID = 4
            LEFT JOIN dbo.ScreeningSectionQuestionResult q1 ON r01.ScreeningResultID = q1.ScreeningResultID AND q1.ScreeningSectionID = 'TCC' AND q1.QuestionID = 1 
            LEFT JOIN dbo.ScreeningSectionQuestionResult q2 ON r01.ScreeningResultID = q2.ScreeningResultID AND q2.ScreeningSectionID = 'TCC' AND q2.QuestionID = 2 
            LEFT JOIN dbo.ScreeningSectionQuestionResult q3 ON r01.ScreeningResultID = q3.ScreeningResultID AND q3.ScreeningSectionID = 'TCC' AND q3.QuestionID = 3 
        WHERE r01.PatientName = ur.PatientName AND r01.Birthday = ur.Birthday 
            AND r01.CreatedDate <= r.CreatedDate
        ORDER BY r01.CreatedDate DESC
    ) as tcc

    --cage
    OUTER APPLY 
    (
        SELECT TOP (1)
            c1.AnswerValue as 'CAGE / Alcohol Use - Q1'
            ,c2.AnswerValue as 'CAGE / Alcohol Use - Q2'
            ,c3.AnswerValue as 'CAGE / Alcohol Use - Q3'
            ,c4.AnswerValue as 'CAGE / Alcohol Use - Q4'
            ,c0.Score as 'CAGE / Alcohol Use - Total'
            ,c0.ScoreLevel as 'CAGE / Alcohol Use - Score Level'
            ,CONVERT(char(10), r01.CreatedDate, 101) as 'CAGE / Alcohol Use - ScreeningDate'
        FROM dbo.ScreeningResult r01
            INNER JOIN dbo.ScreeningSectionResult c0 ON r01.ScreeningResultID = c0.ScreeningResultID AND c0.ScreeningSectionID = 'CAGE'

            LEFT JOIN dbo.ScreeningSectionQuestionResult c1 ON r01.ScreeningResultID = c1.ScreeningResultID AND c1.ScreeningSectionID = 'CAGE' AND c1.QuestionID = 1 
            LEFT JOIN dbo.ScreeningSectionQuestionResult c2 ON r01.ScreeningResultID = c2.ScreeningResultID AND c2.ScreeningSectionID = 'CAGE' AND c2.QuestionID = 2 
            LEFT JOIN dbo.ScreeningSectionQuestionResult c3 ON r01.ScreeningResultID = c3.ScreeningResultID AND c3.ScreeningSectionID = 'CAGE' AND c3.QuestionID = 3 
            LEFT JOIN dbo.ScreeningSectionQuestionResult c4 ON r01.ScreeningResultID = c4.ScreeningResultID AND c4.ScreeningSectionID = 'CAGE' AND c4.QuestionID = 4 
    
            
        WHERE r01.PatientName = r.PatientName AND r01.Birthday = r.Birthday
            AND r01.CreatedDate <= r.CreatedDate
        ORDER BY r01.CreatedDate DESC
    ) as cage

    -- dast
    OUTER APPLY 
    (
        SELECT TOP (1)
            d1.AnswerValue as 'DAST-10 / Drug Use - Q1'
            ,d2.AnswerValue as 'DAST-10 / Drug Use - Q2'
            ,ABS(d3.AnswerValue - 1) as 'DAST-10 / Drug Use - Q3'
            ,d4.AnswerValue as 'DAST-10 / Drug Use - Q4'
            ,d5.AnswerValue as 'DAST-10 / Drug Use - Q5'
            ,d6.AnswerValue as 'DAST-10 / Drug Use - Q6'
            ,d7.AnswerValue as 'DAST-10 / Drug Use - Q7'
            ,d8.AnswerValue as 'DAST-10 / Drug Use - Q8'
            ,d9.AnswerValue as 'DAST-10 / Drug Use - Q9'
            ,d10.AnswerValue as 'DAST-10 / Drug Use - Q10'

            ,d0.Score as 'DAST-10 / Drug Use - Total'
            ,d0.ScoreLevel as 'DAST-10 / Drug Use - Score Level'
            ,CONVERT(char(10), r01.CreatedDate, 101) as 'DAST-10 / Drug Use - ScreeningDate'
        FROM dbo.ScreeningResult r01
            INNER JOIN dbo.ScreeningSectionResult d0 ON r01.ScreeningResultID = d0.ScreeningResultID AND d0.ScreeningSectionID = 'DAST'
            INNER JOIN dbo.ScreeningSectionQuestionResult d1 ON r01.ScreeningResultID = d1.ScreeningResultID AND d1.ScreeningSectionID = 'DAST' AND d1.QuestionID = 10 
            LEFT JOIN dbo.ScreeningSectionQuestionResult d2 ON r01.ScreeningResultID = d2.ScreeningResultID AND d2.ScreeningSectionID = 'DAST' AND d2.QuestionID = 1 
            LEFT JOIN dbo.ScreeningSectionQuestionResult d3 ON r01.ScreeningResultID = d3.ScreeningResultID AND d3.ScreeningSectionID = 'DAST' AND d3.QuestionID = 2 
            LEFT JOIN dbo.ScreeningSectionQuestionResult d4 ON r01.ScreeningResultID = d4.ScreeningResultID AND d4.ScreeningSectionID = 'DAST' AND d4.QuestionID = 3 
            LEFT JOIN dbo.ScreeningSectionQuestionResult d5 ON r01.ScreeningResultID = d5.ScreeningResultID AND d5.ScreeningSectionID = 'DAST' AND d5.QuestionID = 4 	 
            LEFT JOIN dbo.ScreeningSectionQuestionResult d6 ON r01.ScreeningResultID = d6.ScreeningResultID AND d6.ScreeningSectionID = 'DAST' AND d6.QuestionID = 5 	 
            LEFT JOIN dbo.ScreeningSectionQuestionResult d7 ON r01.ScreeningResultID = d7.ScreeningResultID AND d7.ScreeningSectionID = 'DAST' AND d7.QuestionID = 6 	 
            LEFT JOIN dbo.ScreeningSectionQuestionResult d8 ON r01.ScreeningResultID = d8.ScreeningResultID AND d8.ScreeningSectionID = 'DAST' AND d8.QuestionID = 7 	 
            LEFT JOIN dbo.ScreeningSectionQuestionResult d9 ON r01.ScreeningResultID = d9.ScreeningResultID AND d9.ScreeningSectionID = 'DAST' AND d9.QuestionID = 8 	 
            LEFT JOIN dbo.ScreeningSectionQuestionResult d10 ON r01.ScreeningResultID = d10.ScreeningResultID AND d10.ScreeningSectionID = 'DAST' AND d10.QuestionID = 9 	 
            
        WHERE r01.PatientName = r.PatientName AND r01.Birthday = r.Birthday
                    AND r01.CreatedDate <= r.CreatedDate 
        ORDER BY r01.CreatedDate DESC
    ) as dast


    --phq
    OUTER APPLY 
    (
    SELECT TOP (1)
        p1.AnswerValue as 'PHQ-9 / Depression - Q1'
        ,p2.AnswerValue as 'PHQ-9 / Depression - Q2'
        ,p3.AnswerValue as 'PHQ-9 / Depression - Q3'
        ,p4.AnswerValue as 'PHQ-9 / Depression - Q4'
        ,p5.AnswerValue as 'PHQ-9 / Depression - Q5'
        ,p6.AnswerValue as 'PHQ-9 / Depression - Q6'
        ,p7.AnswerValue as 'PHQ-9 / Depression - Q7'
        ,p8.AnswerValue as 'PHQ-9 / Depression - Q8'
        ,p9.AnswerValue as 'PHQ-9 / Depression - Q9'
        ,p10a.OptionText as 'PHQ-9 / Depression - Q10'
        ,p0.Score as 'PHQ-9 / Depression - Total'
        ,p0.ScoreLevel as 'PHQ-9 / Depression - Score Level'
        ,CONVERT(char(10), r01.CreatedDate, 101) as 'PHQ-9 / Depression - ScreeningDate'

    FROM dbo.ScreeningResult r01
        INNER JOIN dbo.ScreeningSectionResult p0 ON r01.ScreeningResultID = p0.ScreeningResultID AND p0.ScreeningSectionID = 'PHQ-9'
        INNER JOIN dbo.ScreeningSectionQuestionResult p1 ON r01.ScreeningResultID = p1.ScreeningResultID AND p1.ScreeningSectionID = 'PHQ-9' AND p1.QuestionID = 1 
        INNER JOIN dbo.ScreeningSectionQuestionResult p2 ON r01.ScreeningResultID = p2.ScreeningResultID AND p2.ScreeningSectionID = 'PHQ-9' AND p2.QuestionID = 2 
        LEFT JOIN dbo.ScreeningSectionQuestionResult p3 ON r01.ScreeningResultID = p3.ScreeningResultID AND p3.ScreeningSectionID = 'PHQ-9' AND p3.QuestionID = 3 
        LEFT JOIN dbo.ScreeningSectionQuestionResult p4 ON r01.ScreeningResultID = p4.ScreeningResultID AND p4.ScreeningSectionID = 'PHQ-9' AND p4.QuestionID = 4 
        LEFT JOIN dbo.ScreeningSectionQuestionResult p5 ON r01.ScreeningResultID = p5.ScreeningResultID AND p5.ScreeningSectionID = 'PHQ-9' AND p5.QuestionID = 5 
        LEFT JOIN dbo.ScreeningSectionQuestionResult p6 ON r01.ScreeningResultID = p6.ScreeningResultID AND p6.ScreeningSectionID = 'PHQ-9' AND p6.QuestionID = 6 
        LEFT JOIN dbo.ScreeningSectionQuestionResult p7 ON r01.ScreeningResultID = p7.ScreeningResultID AND p7.ScreeningSectionID = 'PHQ-9' AND p7.QuestionID = 7 
        LEFT JOIN dbo.ScreeningSectionQuestionResult p8 ON r01.ScreeningResultID = p8.ScreeningResultID AND p8.ScreeningSectionID = 'PHQ-9' AND p8.QuestionID = 8 
        LEFT JOIN dbo.ScreeningSectionQuestionResult p9 ON r01.ScreeningResultID = p9.ScreeningResultID AND p9.ScreeningSectionID = 'PHQ-9' AND p9.QuestionID = 9 
        LEFT JOIN dbo.ScreeningSectionQuestionResult p10 ON r01.ScreeningResultID = p10.ScreeningResultID AND p10.ScreeningSectionID = 'PHQ-9' AND p10.QuestionID = 10
            LEFT JOIN dbo.ScreeningSectionQuestion p10q ON p10q.ScreeningSectionID = p10.ScreeningSectionID AND p10q.QuestionID = p10.QuestionID
            LEFT JOIN dbo.AnswerScaleOption p10a ON p10a.AnswerScaleID = p10q.AnswerScaleID AND p10a.OptionValue = p10.AnswerValue
    
        WHERE r01.PatientName = r.PatientName AND r01.Birthday = r.Birthday
                        AND r01.CreatedDate <= r.CreatedDate 
        ORDER BY r01.CreatedDate DESC

    ) as phq

            
    --hits
    OUTER APPLY (
        SELECT TOP(1)
        h1.AnswerValue as 'HITS / Violence - Q1'
        ,h2.AnswerValue as 'HITS / Violence - Q2'
        ,h3.AnswerValue as 'HITS / Violence - Q3'
        ,h4.AnswerValue as 'HITS / Violence - Q4'

        ,h0.Score as 'HITS / Violence - Total'
        ,h0.ScoreLevel as 'HITS / Violence - Score Level'
        ,CONVERT(char(10), r01.CreatedDate, 101) as 'HITS / Violence - ScreeningDate'
        FROM dbo.ScreeningResult r01
            INNER JOIN dbo.ScreeningSectionResult h0 ON r01.ScreeningResultID = h0.ScreeningResultID AND h0.ScreeningSectionID = 'HITS'
            LEFT JOIN dbo.ScreeningSectionQuestionResult h1 ON r01.ScreeningResultID = h1.ScreeningResultID AND h1.ScreeningSectionID = 'HITS' AND h1.QuestionID = 1
            LEFT JOIN dbo.ScreeningSectionQuestionResult h2 ON r01.ScreeningResultID = h2.ScreeningResultID AND h2.ScreeningSectionID = 'HITS' AND h2.QuestionID = 2
            LEFT JOIN dbo.ScreeningSectionQuestionResult h3 ON r01.ScreeningResultID = h3.ScreeningResultID AND h3.ScreeningSectionID = 'HITS' AND h3.QuestionID = 3
            LEFT JOIN dbo.ScreeningSectionQuestionResult h4 ON r01.ScreeningResultID = h4.ScreeningResultID AND h4.ScreeningSectionID = 'HITS' AND h4.QuestionID = 4
    
        WHERE r01.PatientName = r.PatientName AND r01.Birthday = r.Birthday
                        AND r01.CreatedDate <= r.CreatedDate 
        ORDER BY r01.CreatedDate DESC
    ) as hits

        --doch
    OUTER APPLY (
        SELECT TOP (1)
            doch1a.Name as 'Primary Drug'
            ,doch2a.Name as 'Secondary Drug'
            ,doch3a.Name as 'Tertiary Drug'
            ,CONVERT(char(10), r01.CreatedDate, 101) as 'Drug Use - ScreeningDate'
        FROM dbo.ScreeningResult r01
            INNER JOIN dbo.ScreeningSectionQuestionResult doch1 ON r01.ScreeningResultID = doch1.ScreeningResultID AND doch1.ScreeningSectionID = 'DOCH' AND doch1.QuestionID = 1
                LEFT JOIN dbo.DrugOfChoice doch1a ON doch1.AnswerValue = doch1a.ID
            LEFT JOIN dbo.ScreeningSectionQuestionResult doch2 ON r01.ScreeningResultID = doch2.ScreeningResultID AND doch2.ScreeningSectionID = 'DOCH' AND doch2.QuestionID = 2
                LEFT JOIN dbo.DrugOfChoice doch2a ON ISNULL(doch2.AnswerValue,0) = doch2a.ID
            LEFT JOIN dbo.ScreeningSectionQuestionResult doch3 ON r01.ScreeningResultID = doch3.ScreeningResultID AND doch3.ScreeningSectionID = 'DOCH' AND doch3.QuestionID = 3
                LEFT JOIN dbo.DrugOfChoice doch3a ON ISNULL(doch3.AnswerValue,0) = doch3a.ID
        WHERE r01.PatientName = r.PatientName AND r01.Birthday = r.Birthday
                        AND r01.CreatedDate <= r.CreatedDate 
        ORDER BY r01.CreatedDate DESC
    ) as doch


    -- gad
    OUTER APPLY 
    (
        SELECT TOP (1)
            p1.AnswerValue as 'GAD-7 / Anxiety - Q1'
            ,p2.AnswerValue as 'GAD-7 / Anxiety - Q2'
            ,p3.AnswerValue as 'GAD-7 / Anxiety - Q3'
            ,p4.AnswerValue as 'GAD-7 / Anxiety - Q4'
            ,p5.AnswerValue as 'GAD-7 / Anxiety - Q5'
            ,p6.AnswerValue as 'GAD-7 / Anxiety - Q6'
            ,p7.AnswerValue as 'GAD-7 / Anxiety - Q7'
            ,p8a.OptionText as 'GAD-7 / Anxiety - Q8'
            ,p0.Score as 'GAD-7 / Anxiety - Total'
            ,p0.ScoreLevel as 'GAD-7 / Anxiety - Score Level'
            ,CONVERT(char(10), r01.CreatedDate, 101) as 'GAD-7 / Anxiety - ScreeningDate'

        FROM dbo.ScreeningResult r01
            INNER JOIN dbo.ScreeningSectionResult p0 ON r01.ScreeningResultID = p0.ScreeningResultID AND p0.ScreeningSectionID = 'GAD-7'
            INNER JOIN dbo.ScreeningSectionQuestionResult p1 ON r01.ScreeningResultID = p1.ScreeningResultID AND p1.ScreeningSectionID = 'GAD-7' AND p1.QuestionID = 1 
            INNER JOIN dbo.ScreeningSectionQuestionResult p2 ON r01.ScreeningResultID = p2.ScreeningResultID AND p2.ScreeningSectionID = 'GAD-7' AND p2.QuestionID = 2 
            LEFT JOIN dbo.ScreeningSectionQuestionResult p3 ON r01.ScreeningResultID = p3.ScreeningResultID AND p3.ScreeningSectionID = 'GAD-7' AND p3.QuestionID = 3 
            LEFT JOIN dbo.ScreeningSectionQuestionResult p4 ON r01.ScreeningResultID = p4.ScreeningResultID AND p4.ScreeningSectionID = 'GAD-7' AND p4.QuestionID = 4 
            LEFT JOIN dbo.ScreeningSectionQuestionResult p5 ON r01.ScreeningResultID = p5.ScreeningResultID AND p5.ScreeningSectionID = 'GAD-7' AND p5.QuestionID = 5 
            LEFT JOIN dbo.ScreeningSectionQuestionResult p6 ON r01.ScreeningResultID = p6.ScreeningResultID AND p6.ScreeningSectionID = 'GAD-7' AND p6.QuestionID = 6 
            LEFT JOIN dbo.ScreeningSectionQuestionResult p7 ON r01.ScreeningResultID = p7.ScreeningResultID AND p7.ScreeningSectionID = 'GAD-7' AND p7.QuestionID = 7 
            LEFT JOIN dbo.ScreeningSectionQuestionResult p8 ON r01.ScreeningResultID = p8.ScreeningResultID AND p8.ScreeningSectionID = 'GAD-7' AND p8.QuestionID = 8
                LEFT JOIN dbo.ScreeningSectionQuestion p8q ON p8q.ScreeningSectionID = p8.ScreeningSectionID AND p8q.QuestionID = p8.QuestionID
                LEFT JOIN dbo.AnswerScaleOption p8a ON p8a.AnswerScaleID = p8q.AnswerScaleID AND p8a.OptionValue = p8.AnswerValue
    
            WHERE r01.PatientName = r.PatientName AND r01.Birthday = r.Birthday
                            AND r01.CreatedDate <= r.CreatedDate 
            ORDER BY r01.CreatedDate DESC
    ) as gad

     --bbgs
    OUTER APPLY (
        SELECT TOP(1)
            h1.AnswerValue as 'BBGS / Problem Gambling - Q1'
            ,h2.AnswerValue as 'BBGS / Problem Gambling - Q2'
            ,h3.AnswerValue as 'BBGS / Problem Gambling - Q3'
            ,h4.AnswerValue as 'BBGS / Problem Gambling - Q4'

            ,h0.Score as 'BBGS / Problem Gambling - Total'
            ,h0.ScoreLevel as 'BBGS / Problem Gambling - Score Level'
            ,CONVERT(char(10), r01.CreatedDate, 101) as 'BBGS / Problem Gambling - ScreeningDate'
        FROM dbo.ScreeningResult r01
            INNER JOIN dbo.ScreeningSectionResult h0 ON r01.ScreeningResultID = h0.ScreeningResultID AND h0.ScreeningSectionID = 'BBGS'
            LEFT JOIN dbo.ScreeningSectionQuestionResult h1 ON r01.ScreeningResultID = h1.ScreeningResultID AND h1.ScreeningSectionID = 'BBGS' AND h1.QuestionID = 1
            LEFT JOIN dbo.ScreeningSectionQuestionResult h2 ON r01.ScreeningResultID = h2.ScreeningResultID AND h2.ScreeningSectionID = 'BBGS' AND h2.QuestionID = 2
            LEFT JOIN dbo.ScreeningSectionQuestionResult h3 ON r01.ScreeningResultID = h3.ScreeningResultID AND h3.ScreeningSectionID = 'BBGS' AND h3.QuestionID = 3
            LEFT JOIN dbo.ScreeningSectionQuestionResult h4 ON r01.ScreeningResultID = h4.ScreeningResultID AND h4.ScreeningSectionID = 'BBGS' AND h4.QuestionID = 4
    
        WHERE r01.PatientName = r.PatientName AND r01.Birthday = r.Birthday
                        AND r01.CreatedDate <= r.CreatedDate 
        ORDER BY r01.CreatedDate DESC
    ) as bbgs

    -- demographics
    OUTER APPLY (
        SELECT TOP (1) 
            ID
            ,RaceId
            ,RaceName
            ,GenderId
            ,GenderName
            ,SexualOrientationId
            ,SexualOrientationName
            ,TribalAffiliation
            ,MaritalStatusId
            ,MaritalStatusName
            ,EducationLevelId
            ,EducationLevelName
            ,LivingOnReservationId
            ,LivingOnReservationName
            ,CountyOfResidence
            ,MilitaryExperienceId
            ,MilitaryExperienceName
        FROM [dbo].[vBhsDemographics] d
        WHERE d.FullName = r.PatientName AND d.Birthday = r.Birthday
    ) as demographics
ORDER BY ur.CreatedDate ASC

RETURN 0

GO




---------------------------

IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '10.0.4.0')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('10.0.4.0');

MERGE INTO dbo.ScreeningScoreLevel as target
USING ( VALUES
('GAD-7', 2, 'MODERATE anxiety severity', 'Further evaluation needed and referral to mental health program', 'Moderate'),
('GAD-7', 3, 'SEVERE anxiety severity', 'Further evaluation needed and referral to mental health program', 'Severe'),
('BBGS', 1, 'Evidence of PROBLEM GAMBLING', 'Need for immediate investigation and/or referral', 'Evidence of Problem Gambling')


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

---------------------------
MERGE INTO ScreeningSectionQuestion as Target
USING( VALUES

-- Problem Gambling
('BBGS', 1, 'In the PAST 12 MONTHS, have you gambled?', '(Examples of gambling include lottery scratchers or draw tickets, casino games, daily fantasy sports, bingo, online poker, horse racing, etc.)?', 1, 1, 0, 10),
('GAD7A', 1, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Feeling nervous, anxious, or on edge?', 2, 1, 0, 10),
('GAD-7', 1, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Feeling nervous, anxious, or on edge?', 2, 1, 0, 10)


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
---------------------------

IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '10.0.4.1')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('10.0.4.1');

-- Score Levels
MERGE INTO dbo.ColumbiaSuicideRiskScoreLevel as target
USING (
VALUES
( 1, 'NO RISK Identified', 'No response required', 'No Risk'),
( 2, 'LOW RISK Identified', 'Provide education and resources', 'Low'),
( 3, 'MODERATE RISK Identified', 'Provider to assess and determine disposition. Provider has option to provide safe environment, per clinical judgement and assessment. Obtain or complete further assessment. Behavioral health consultation, if available', 'Moderate'),
( 4, 'HIGH RISK Identified', 'Immediate notification to provider to assess and determine ultimate disposition. Provide safe environment', 'High')
) AS source(ScoreLevel, [Name], Indicates, [Label]) ON target.ScoreLevel = source.ScoreLevel
WHEN MATCHED THEN  
    UPDATE SET [Name] = source.[Name], Indicates = source.Indicates, [Label] = source.[Label]
WHEN NOT MATCHED BY TARGET THEN 
    INSERT (ScoreLevel, [Name], Indicates, [Label]) 
    VALUES (source.ScoreLevel, source.[Name], source.Indicates, [Label])  
;
GO


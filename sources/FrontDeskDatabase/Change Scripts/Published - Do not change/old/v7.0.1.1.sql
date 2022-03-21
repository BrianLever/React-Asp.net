

-- Section Qiestions, with Y/N answer
MERGE INTO dbo.ScreeningSection as target
USING ( VALUES
('DOCH', 'BHS', 'DOCH', 'Drug Use', 'What Drug do you USE THE MOST?', 6)
) AS source(ScreeningSectionID, ScreeningID, ScreeningSectionShortName, ScreeningSectionName, QuestionText, OrderIndex)
    ON source.ScreeningSectionID = target.ScreeningSectionID
WHEN MATCHED THEN  
    UPDATE SET ScreeningID = source.ScreeningID, 
        ScreeningSectionShortName = source.ScreeningSectionShortName, 
        ScreeningSectionName = source.ScreeningSectionName,
        QuestionText = source.QuestionText,
        OrderIndex = source.OrderIndex
;
GO


UPDATE dbo.SecurityEvent SET Description =  'Drug List Use was changed'
WHERE SecurityEventID = 25;

GO


MERGE INTO dbo.DrugOfChoice as target
USING (VALUES
(5, 'Opioid/Heroin', 6),
(6, 'Opioid/Medication', 7)

) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex
;
GO


MERGE INTO dbo.AnswerScaleOption as target
USING (
VALUES
(29, 5, 'Opioid/Heroin', 5),
(30, 6, 'Opioid/Heroin', 5),

(31, 5, 'Opioid/Medication', 6),
(32, 6, 'Opioid/Medication', 6)
) AS source(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
    ON source.AnswerScaleOptionID = target.AnswerScaleOptionID
WHEN MATCHED THEN  
    UPDATE SET AnswerScaleID = source.AnswerScaleID, OptionText = source.OptionText, OptionValue = source.OptionValue

;
GO



---------------------------------------------
IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '7.0.1.1')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('7.0.1.1');
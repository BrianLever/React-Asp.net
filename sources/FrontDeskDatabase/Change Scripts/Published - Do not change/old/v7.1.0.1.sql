
-- Section Questions, with Y/N answer
MERGE INTO dbo.ScreeningSection as target
USING ( VALUES
('CIF', 'BHS', 'CIF', 'Contact Information', '', 0),
('SIH', 'BHS', 'SIH', 'Smoker in the Home', 'Does anyone in the home smoke tobacco (such as cigarettes, cigars, pipes, electronic nicotine delivery devices, etc.)?', 2),
('TCC', 'BHS', 'TCC', 'Tobacco Use', 'Do you use tobacco?', 3),
('CAGE', 'BHS', 'CAGE', 'Alcohol Use (CAGE)', 'Do you drink alcohol?',4),
('DAST', 'BHS', 'DAST-10', 'Non-Medical Drug Use (DAST-10)', 'Have you used drugs other than those required for medical reasons?',5),
('PHQ-9', 'BHS', 'PHQ-9', 'Depression (PHQ-9)', 'Do you feel down, depressed, or hopeless?', 7),
('PHQ9A', 'BHS', 'PHQ9A', 'Depression (PHQ-9)', '', 7),
('HITS', 'BHS', 'HITS', 'Intimate Partner/Domestic Violence (HITS)', 'Do you feel UNSAFE in your home?', 8),
('DOCH', 'BHS', 'DOCH', 'Drug Use', 'What Drug do you USE THE MOST?', 6),
('DMGR', 'BHS', 'DMGR', 'Patient Demographics', '', 1)
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

MERGE INTO dbo.ScreeningSectionAge target
USING ( 
VALUES 
('CIF', 0, 0, 0),
('CAGE', 9, 1, 0),
('DAST', 12, 1, 0),
('DOCH', 0, 1, 1),
('HITS', 14, 1, 0),
('PHQ-9', 12, 1, 0),
('SIH', 0, 1, 0),
('TCC', 14, 1, 0),
('DMGR', 9, 1, 0),
('PHQ9A', 12, 1, 1)

) AS source (ScreeningSectionID, MinimalAge, IsEnabled, AgeIsNotConfigurable) 
    ON source.ScreeningSectionID = target.ScreeningSectionID
WHEN MATCHED THEN
    UPDATE SET MinimalAge = source.MinimalAge, IsEnabled = source.IsEnabled, AgeIsNotConfigurable = source.AgeIsNotConfigurable, LastModifiedDateUTC = GETUTCDATE()
WHEN NOT MATCHED THEN
    INSERT (ScreeningSectionID, MinimalAge, IsEnabled, AgeIsNotConfigurable,LastModifiedDateUTC)
    VALUES(source.ScreeningSectionID, source.MinimalAge, source.IsEnabled, source.AgeIsNotConfigurable, GETUTCDATE())
;
GO



---------------------------------------------
IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '7.1.0.1')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('7.1.0.1');
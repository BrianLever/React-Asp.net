SET IDENTITY_INSERT dbo.ScreeningProfile ON;
GO

MERGE INTO dbo.ScreeningProfile target
USING ( 
VALUES 
(1, 'Default', 'Default screening profile')

) AS source (ID, Name, Description) 
    ON source.ID = target.ID
WHEN MATCHED THEN
    UPDATE SET Name = source.Name, Description= source.Description, LastModifiedDateUTC = GETUTCDATE()
WHEN NOT MATCHED THEN
    INSERT (ID, Name, Description, LastModifiedDateUTC)
    VALUES(source.ID, source.Name, source.Description, GETUTCDATE())
;
GO

SET IDENTITY_INSERT dbo.ScreeningProfile OFF
;


/* ScreeningSectionAge */
MERGE INTO dbo.ScreeningProfileSectionAge target
USING ( 
VALUES 
(1, 'CIF', 0, 0, 0),
(1, 'CAGE', 9, 1, 0),
(1, 'DAST', 12, 1, 0),
(1, 'DOCH', 0, 1, 1),
(1, 'HITS', 14, 1, 0),
(1, 'PHQ-9', 12, 1, 0),
(1, 'SIH', 0, 1, 0),
(1, 'TCC', 14, 1, 0),
(1, 'DMGR', 9, 1, 0),
(1, 'PHQ9A', 12, 1, 1)


) AS source (ScreeningProfileID, ScreeningSectionID, MinimalAge, IsEnabled, AgeIsNotConfigurable) 
    ON source.ScreeningProfileID = target.ScreeningProfileID AND source.ScreeningSectionID = target.ScreeningSectionID
WHEN MATCHED THEN
    UPDATE SET MinimalAge = source.MinimalAge, IsEnabled = source.IsEnabled, AgeIsNotConfigurable = source.AgeIsNotConfigurable, LastModifiedDateUTC = GETUTCDATE()
WHEN NOT MATCHED THEN
    INSERT (ScreeningProfileID, ScreeningSectionID, MinimalAge, IsEnabled, AgeIsNotConfigurable,LastModifiedDateUTC)
    VALUES(source.ScreeningProfileID, source.ScreeningSectionID, source.MinimalAge, source.IsEnabled, source.AgeIsNotConfigurable, GETUTCDATE())
;
GO
----------------------

MERGE INTO dbo.[VisitSettings] target
USING ( 
VALUES
('SIH', 'Smoker in the Home', 0, 10, 0),
('TCC1', 'Tobacco Use (Ceremony)', 0, 20, 0),
('TCC2', 'Tobacco Use (Smoking)', 1, 30, 0),
('TCC3', 'Tobacco Use (Smokeless)', 1, 40, 1),
('CAGE', 'Alcohol Use (CAGE)', 1, 50, 1),
('DAST', 'Non-Medical Drug Use (DAST-10)', 1, 60, 1),
('PHQ1', 'Depression (PHQ-9)', 1, 70, 1),
('PHQ2', 'Suicide Identiation (PHQ-9)', 1, 80, 1),
('HITS', 'Intimate Partner/Domestic Violence (HITS)', 1, 90, 1)
) AS source (MeasureToolId, Name, IsEnabled, OrderIndex, CutScore)
    ON source.MeasureToolId = target.MeasureToolId
WHEN MATCHED THEN
    UPDATE SET IsEnabled = source.IsEnabled, OrderIndex = source.OrderIndex, LastModifiedDateUTC = GETUTCDATE()
WHEN NOT MATCHED THEN
    INSERT (MeasureToolId, Name, IsEnabled, OrderIndex, LastModifiedDateUTC)
    VALUES(source.MeasureToolId, source.Name, source.IsEnabled, source.OrderIndex,  GETUTCDATE())
;

GO




MERGE into dbo.SystemSettings  target
USING ( 
    VALUES 
    ('PasswordRenewalPeriodDays', '120', 'Password renewal period in days', 'Password renewal period in days', NULL, 1),
    ('ExportedSecurityReportMaximumLength', '3000', 'Maximum length of the exported security report', 'The Maximum number of security report''s rows that can be exported to excel document', NULL, 1),
    ('ActivationSupportEmail', 'kgerhardt@3si2.com', 'License Activation Email Address','Email address where activation request code need to be send.', NULL, 0),
    ('ActivationRequestEmailTemplate', 'Please activate my FrontDesk Behavioral Health Screener product license.%0A%0AMy activation request code: {0}', 'Activation Request Email Template','Email text for sending product activation request code', NULL, 0),
    ('ActivationSupportEmailSubject', 'FrontDesk License Activation', 'Activation Email Subject Text','Activation Email Subject Text.', NULL, 0),
    ('EHRSystem', 'RPMS', 'External EHR System', 'Integration with external EHR system. Values: RPMS, NEXTGEN, NONE', NULL, 0),
    ('IndicatorReport_AgeGroups', '0 - 9;10 - 11;12 - 17;18 - 24;25 - 54;55 or Older', 'Age groups for Indicator Reports', 'Age groups for the report in format: 0 - 9;10 - 11;12 - 17;18 - 24;25 - 54;55 or Older', '(\d+\s?-\s?\d+\s?)['',]?|(\d+\s?or\s+older\s?)['',]?', 1),
    ('KioskInstallationDirectoryRoot', 'c:\ScreenDox\KioskInstallationPackagesDirectory\', 'Kiosk Install Files Root Folder', 'Path to the root directory with Kiosk installation files', '^[a-zA-Z]:\\[\\\S|*\S]?.*$', 1)

    ) as source([Key], Value, Name, Description, RegExp, IsExposed)
ON target.[Key] = source.[Key]
WHEN MATCHED THEN
    UPDATE SET target.value = source.value, target.Name = source.Name, target.Description = source.Description, target.Regexp = source.RegExp, target.IsExposed = source.IsExposed
WHEN NOT MATCHED BY target THEN
    INSERT([Key], Value, Name, Description, RegExp, IsExposed) 
        VALUES(source.[Key], source.Value, source.Name, source.Description, source.RegExp, source.IsExposed)
;

GO





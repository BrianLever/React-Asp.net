CREATE TABLE [dbo].[VisitSettings]
(
    [MeasureToolId] char(5) NOT NULL, 
	[Name] varchar(64) NOT NULL,
    IsEnabled bit NOT NULL CONSTRAINT DF_VisitSettings_IsEnabled DEFAULT 1,
    OrderIndex tinyint NOT NULL,
	LastModifiedDateUTC datetime NOT NULL,
    CutScore int NOT NULL CONSTRAINT DF_VisitSettings_CustScore DEFAULT 1,
	CONSTRAINT PK_VisitSettings PRIMARY KEY CLUSTERED(MeasureToolId ASC),
    CONSTRAINT CK_VisitSettings_CutScore CHECK (CutScore > 0)
)
GO
CREATE INDEX IX_VisitSettings_OrderIndex ON dbo.VisitSettings(OrderIndex ASC, [MeasureToolId] ASC, [Name] ASC)
GO
;
/*
IF NOT EXISTS(SELECT NULL FROM [dbo].[VisitSettings])
BEGIN
INSERT INTO dbo.VisitSettings(MeasureToolId, Name, IsEnabled, OrderIndex, LastModifiedDateUTC) VALUES
('SIH', 'Smoker in the Home', 0, 10, GETUTCDATE()),
('TCC1', 'Tobacco Use (Ceremony)', 0, 20, GETUTCDATE()),
('TCC2', 'Tobacco Use (Smoking)', 1, 30, GETUTCDATE()),
('TCC3', 'Tobacco Use (Smokeless)', 1, 40, GETUTCDATE()),
('CAGE', 'Alcohol Use (CAGE)', 1, 50, GETUTCDATE()),
('DAST', 'Non-Medical Drug Use (DAST-10)', 1, 60, GETUTCDATE()),
('PHQ1', 'Depression (PHQ-9)', 1, 70, GETUTCDATE()),
('PHQ2', 'Suicide Identiation (PHQ-9)', 1, 80, GETUTCDATE()),
('HITS', 'Intimate Partner/Domestic Violence (HITS)', 1, 90, GETUTCDATE())
;

END
GO
*/


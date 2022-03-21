--Add export date column

if exists (select * from sys.columns WHERE object_id = OBJECT_ID('dbo.ScreeningResult') and name = 'ExportDate')
	ALTER TABLE dbo.ScreeningResult ALTER COLUMN ExportDate DateTimeOffset null;
else
	ALTER TABLE dbo.ScreeningResult ADD ExportDate DateTimeOffset null
GO	
 
if NOT exists (select * from sys.columns WHERE object_id = OBJECT_ID('dbo.ScreeningResult') and name = 'ExportedBy')
	ALTER TABLE dbo.ScreeningResult ADD ExportedBy int null
GO	
if NOT exists (select * from sys.columns WHERE object_id = OBJECT_ID('dbo.ScreeningResult') and name = 'ExportedToPatientID') 
	ALTER TABLE dbo.ScreeningResult ADD ExportedToPatientID int null
GO	 
if NOT exists (select * from sys.columns WHERE object_id = OBJECT_ID('dbo.ScreeningResult') and name = 'ExportedToHRN') 
	ALTER TABLE dbo.ScreeningResult ADD ExportedToHRN nvarchar(255) null
GO	 
if NOT exists (select * from sys.columns WHERE object_id = OBJECT_ID('dbo.ScreeningResult') and name = 'ExportedToVisitID') 
	ALTER TABLE dbo.ScreeningResult ADD ExportedToVisitID int null
GO
if NOT exists (select * from sys.columns WHERE object_id = OBJECT_ID('dbo.ScreeningResult') and name = 'ExportedToVisitDate') 
	ALTER TABLE dbo.ScreeningResult ADD ExportedToVisitDate datetime null
GO

if NOT exists (select * from sys.columns WHERE object_id = OBJECT_ID('dbo.ScreeningResult') and name = 'ExportedToVisitLocation') 
	ALTER TABLE dbo.ScreeningResult ADD ExportedToVisitLocation nvarchar(255) null
GO
	


IF NOT EXISTS (SELECT NULL FROM SecurityEvent WHERE SecurityEventID = 13)
	insert into SecurityEvent(SecurityEventID, SecurityEventCategoryID, [Description])
	VALUES(13, 2, 'Patient contact information was changed')
GO

IF NOT EXISTS (SELECT NULL FROM SecurityEvent WHERE SecurityEventID = 14)
	insert into SecurityEvent(SecurityEventID, SecurityEventCategoryID, [Description])
		VALUES(14, 2, 'Behavioral Health Screening Report was exported')
GO
-- App Settings

MERGE into dbo.SystemSettings  target
USING ( 
	VALUES 
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
;

-- Kiosk Key column
IF EXISTS(select 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Kiosk') and name = 'SecretKey')
SET NOEXEC ON
GO


ALTER TABLE dbo.Kiosk
	ADD SecretKey varchar(64) NULL;
GO

SET NOEXEC OFF
GO


ALTER VIEW [dbo].[vKiosk]
    AS
SELECT 
    k.KioskID, 
    k.KioskName, 
    k.Description, 
    k.CreatedDate , 
    k.LastActivityDate, 
    k.BranchLocationID, 
    k.Disabled, 
    l.Name as BranchLocationName,
    k.IpAddress,
    k.KioskAppVersion,
	p.ID as ScreeningProfileID,
	p.Name as ScreeningProfileName,
    k.SecretKey
FROM dbo.Kiosk k
INNER JOIN dbo.BranchLocation l ON k.BranchLocationID = l.BranchLocationID
INNER JOIN dbo.ScreeningProfile p ON p.ID = l.ScreeningProfileID
GO

------------------------------------------------------------------------------

IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '9.0.0.0')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('9.0.0.0');
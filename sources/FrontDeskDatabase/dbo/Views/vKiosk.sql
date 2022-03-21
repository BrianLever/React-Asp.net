CREATE VIEW [dbo].[vKiosk]
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

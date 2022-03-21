IF EXISTS(select * from sys.columns where object_id = OBJECT_ID('dbo.Kiosk') and name = 'IpAddress')
SET NOEXEC ON
GO

ALTER TABLE dbo.Kiosk ADD IpAddress varchar(45);
ALTER TABLE dbo.Kiosk ADD KioskAppVersion varchar(16);


SET NOEXEC OFF
GO

GRANT EXECUTE ON SCHEMA::[dbo]  TO frontdesk_appuser;

IF OBJECT_ID('[dbo].[uspChangeKioskLastActivityDate]') IS NOT NULL
DROP PROCEDURE [dbo].[uspChangeKioskLastActivityDate]

GO

CREATE PROCEDURE [dbo].[uspChangeKioskLastActivityDate]
    @KioskID smallint,
    @IpAddress varchar(45),
    @KioskAppVersion varchar(16),
    @LastActivityDate datetimeoffset
AS
    UPDATE dbo.Kiosk SET 
    LastActivityDate = @LastActivityDate,
    IpAddress = ISNULL(@IpAddress, IpAddress),
    KioskAppVersion = ISNULL(@KioskAppVersion, KioskAppVersion)
WHERE KioskID = @KioskID

    RETURN (SELECT [Disabled] From dbo.Kiosk WHERE KioskID = @KioskID)

 GO



---------------------------------------------
IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '7.1.0.0')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('7.1.0.0');
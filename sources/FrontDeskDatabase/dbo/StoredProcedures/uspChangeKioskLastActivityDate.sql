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


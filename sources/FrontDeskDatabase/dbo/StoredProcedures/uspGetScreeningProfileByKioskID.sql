CREATE PROCEDURE [dbo].[uspGetScreeningProfileByKioskID]
    @KioskID smallint
AS
BEGIN
    DECLARE @ScreeningProfileID int

    SELECT @ScreeningProfileID = ScreeningProfileID
    FROM dbo.vKiosk k
    WHERE k.KioskID = @KioskID

SELECT @ScreeningProfileID
END
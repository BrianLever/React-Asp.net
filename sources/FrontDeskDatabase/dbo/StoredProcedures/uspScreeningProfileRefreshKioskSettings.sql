CREATE PROCEDURE [dbo].[uspScreeningProfileRefreshKioskSettings]
    @ScreeningProfileID int,
    @LastModifiedDateUTC datetime
AS
BEGIN

    UPDATE dbo.ScreeningProfileSectionAge SET
        LastModifiedDateUTC = @LastModifiedDateUTC
    WHERE ScreeningProfileID = @ScreeningProfileID


    UPDATE dbo.ScreeningProfileFrequency SET
        LastModifiedDateUTC = @LastModifiedDateUTC
    WHERE ScreeningProfileID = @ScreeningProfileID



END
RETURN 0

CREATE PROCEDURE [dbo].[uspGetModifiedSectionMinimalAgeSettingsForKiosk]
    @KioskID smallint,
    @LastModifiedDateUTC datetime
AS
SELECT a.ScreeningSectionID, a.MinimalAge, a.IsEnabled, a.LastModifiedDateUTC
FROM dbo.ScreeningProfileSectionAge a
	INNER JOIN dbo.vKiosk k ON a.ScreeningProfileID = k.ScreeningProfileID
WHERE a.LastModifiedDateUTC > @LastModifiedDateUTC AND k.KioskID = @KioskID
ORDER BY a.ScreeningSectionID ASC
RETURN 0

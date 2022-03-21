CREATE PROCEDURE [dbo].[uspUpdateScreeningProfileAgeSettings]
    @ScreeningProfileID int,
    @ScreeningSectionID char(5),
    @MinimalAge tinyint,
    @IsEnabled bit,
    @LastModifiedDateUTC datetimeoffset
AS
BEGIN
IF NOT EXISTS(SELECT NULL FROM dbo.ScreeningProfileSectionAge WHERE ScreeningProfileID = @ScreeningProfileID AND ScreeningSectionID = @ScreeningSectionID)
    INSERT INTO dbo.ScreeningProfileSectionAge(ScreeningProfileID, ScreeningSectionID, MinimalAge, IsEnabled, LastModifiedDateUTC)
    VALUES(@ScreeningProfileID, @ScreeningSectionID, @MinimalAge, @IsEnabled, @LastModifiedDateUTC)
ELSE
    UPDATE dbo.ScreeningProfileSectionAge
        SET MinimalAge = @MinimalAge, IsEnabled=@IsEnabled, LastModifiedDateUTC = @LastModifiedDateUTC
    WHERE  ScreeningProfileID = @ScreeningProfileID AND ScreeningSectionID = @ScreeningSectionID 
        AND  (MinimalAge <> @MinimalAge OR IsEnabled <> @IsEnabled)
END
RETURN 0

GO

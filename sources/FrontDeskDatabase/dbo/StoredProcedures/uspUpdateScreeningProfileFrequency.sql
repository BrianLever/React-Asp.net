CREATE PROCEDURE [dbo].[uspUpdateScreeningProfileFrequency]
    @ScreeningProfileID int,
    @ScreeningSectionID char(5),
    @Frequency int,
    @LastModifiedDateUTC datetimeoffset
AS
BEGIN
IF NOT EXISTS(SELECT NULL FROM dbo.ScreeningProfileFrequency WHERE  ScreeningProfileID = @ScreeningProfileID AND ScreeningSectionID = @ScreeningSectionID)
    INSERT INTO dbo.ScreeningProfileFrequency(
        ScreeningProfileID, 
        ScreeningSectionID, 
        Frequency, 
        LastModifiedDateUTC
    )
    VALUES(
        @ScreeningProfileID, 
        @ScreeningSectionID, 
        @Frequency, 
        @LastModifiedDateUTC
    )
ELSE
    UPDATE dbo.ScreeningProfileFrequency SET 
        Frequency = @Frequency, 
        LastModifiedDateUTC = @LastModifiedDateUTC
    WHERE ScreeningProfileID = @ScreeningProfileID AND ScreeningSectionID = @ScreeningSectionID AND Frequency <> @Frequency

END
RETURN 0

GO

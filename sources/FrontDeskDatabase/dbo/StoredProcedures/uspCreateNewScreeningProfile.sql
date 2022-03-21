CREATE PROCEDURE [dbo].[uspCreateNewScreeningProfile]
    @NewID int OUT,
    @Name nvarchar(128),
    @Description nvarchar(max) = NULL
AS
BEGIN
    declare @DefaultScreeningProfileId int = 1

    INSERT INTO dbo.ScreeningProfile (Name, Description)
    VALUES (@Name, @Description);

    SET @NewID = SCOPE_IDENTITY();

    -- copy screening settings

    -- copy age settings from the default profile

    INSERT INTO dbo.ScreeningProfileSectionAge (
        ScreeningProfileID,
        ScreeningSectionID,
        MinimalAge,
        IsEnabled,
        LastModifiedDateUTC,
        AgeIsNotConfigurable
    )
    SELECT 
        @NewID,
        ScreeningSectionID,
        MinimalAge,
        IsEnabled,
        SYSDATETIMEOFFSET(),
        AgeIsNotConfigurable
    FROM dbo.ScreeningProfileSectionAge
    WHERE ScreeningProfileID = @DefaultScreeningProfileId
       

    INSERT INTO dbo.ScreeningProfileFrequency(
        ScreeningProfileID,
        ScreeningSectionID,
        Frequency,
        LastModifiedDateUTC
    )
    SELECT 
        @NewID,
        ScreeningSectionID,
        Frequency,
        SYSDATETIMEOFFSET()
    FROM dbo.ScreeningProfileFrequency
    WHERE ScreeningProfileID = @DefaultScreeningProfileId

END
RETURN @NewID

GO

-- Screening Profile

if OBJECT_ID('[dbo].[ScreeningProfile]') IS NOT NULL
SET NOEXEC ON
GO

CREATE TABLE [dbo].[ScreeningProfile]
(
    ID int NOT NULL IDENTITY(1, 1),
    [Name] nvarchar(128) NOT NULL,
    [Description] nvarchar(max) NULL,
    LastModifiedDateUTC datetime NOT NULL CONSTRAINT DF__ScreeningProfile__LastModifiedDateUTC DEFAULT GETUTCDATE(),
    CONSTRAINT PK_ScreeningProfile PRIMARY KEY (ID) 
)
GO

SET NOEXEC OFF
GO

SET IDENTITY_INSERT dbo.ScreeningProfile ON

MERGE INTO dbo.ScreeningProfile target
USING (
VALUES 
(1, 'Default', 'Default screening profile')

) AS source (ID, Name, Description) 
    ON source.ID = target.ID
WHEN MATCHED THEN
    UPDATE SET Name = source.Name, Description= source.Description, LastModifiedDateUTC = GETUTCDATE()
WHEN NOT MATCHED THEN
    INSERT (ID, Name, Description, LastModifiedDateUTC)
    VALUES(source.ID, source.Name, source.Description, GETUTCDATE())
;
GO

SET IDENTITY_INSERT dbo.ScreeningProfile OFF


if OBJECT_ID('[dbo].[ScreeningProfileFrequency]') IS NOT NULL
SET NOEXEC ON
GO
CREATE TABLE dbo.ScreeningProfileFrequency
(
    ScreeningProfileID INT NOT NULL,
    ScreeningSectionID char(5) NOT NULL,
    Frequency int NOT NULL,
    LastModifiedDateUTC datetime NOT NULL,
    CONSTRAINT PK__ScreeningProfileFrequency PRIMARY KEY(ScreeningProfileID ASC, ScreeningSectionID ASC),
    CONSTRAINT FK___ScreeningProfile FOREIGN KEY(ScreeningProfileID) REFERENCES dbo.ScreeningProfile(ID)
        ON UPDATE NO ACTION ON DELETE CASCADE,
        CONSTRAINT FK__ScreeningProfileFrequency__ScreeningSection FOREIGN KEY(ScreeningSectionID)
        REFERENCES dbo.ScreeningSection(ScreeningSectionID) ON UPDATE CASCADE ON DELETE CASCADE
)
GO

SET NOEXEC OFF
GO


if OBJECT_ID('[dbo].[ScreeningProfileSectionAge]') IS NOT NULL
SET NOEXEC ON
GO
CREATE TABLE dbo.ScreeningProfileSectionAge
(
    ScreeningProfileID INT NOT NULL,
    ScreeningSectionID char(5) NOT NULL,
    MinimalAge tinyint NOT NULL,
    IsEnabled bit NOT NULL DEFAULT 1,
    LastModifiedDateUTC datetime NOT NULL,
    AgeIsNotConfigurable bit NOT NULL 
        CONSTRAINT  DF__ScreeningProfileSectionAge_AgeIsNotConfigurable DEFAULT(0),

    CONSTRAINT PK__ScreeningProfileSectionAge PRIMARY KEY(ScreeningProfileID ASC, ScreeningSectionID ASC),
    CONSTRAINT FK__ScreeningProfileSectionAge__ScreeningSection FOREIGN KEY(ScreeningSectionID)
        REFERENCES dbo.ScreeningSection(ScreeningSectionID) ON UPDATE CASCADE ON DELETE CASCADE
)
GO

SET NOEXEC OFF
GO


-- copy settings to the default screening profile

IF ( NOT EXISTS( SELECT NULL FROM ScreeningProfileSectionAge WHERE ScreeningProfileID = 1))
BEGIN

    INSERT INTO dbo.ScreeningProfileSectionAge (
        ScreeningProfileID,
        ScreeningSectionID,
        MinimalAge,
        IsEnabled,
        LastModifiedDateUTC,
        AgeIsNotConfigurable
    )
    SELECT 
        1,
        ScreeningSectionID,
        MinimalAge,
        IsEnabled,
        LastModifiedDateUTC,
        AgeIsNotConfigurable
    FROM dbo.ScreeningSectionAge

    INSERT INTO dbo.ScreeningProfileFrequency(
        ScreeningProfileID,
        ScreeningSectionID,
        Frequency,
        LastModifiedDateUTC
    )
    SELECT 
        1,
        ID,
        Frequency,
        LastModifiedDateUTC
    FROM dbo.ScreeningFrequency
END

GO



IF OBJECT_ID('[dbo].[uspCreateNewScreeningProfile]') IS NOT NULL
DROP PROCEDURE [dbo].[uspCreateNewScreeningProfile];

GO
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


IF OBJECT_ID('[dbo].[uspUpdateScreeningProfileAgeSettings]') IS NOT NULL
DROP PROCEDURE [dbo].[uspUpdateScreeningProfileAgeSettings];

GO
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



IF OBJECT_ID('[dbo].[uspUpdateScreeningProfileFrequency]') IS NOT NULL
DROP PROCEDURE [dbo].[uspUpdateScreeningProfileFrequency];

GO

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


GRANT EXECUTE ON [dbo].[uspCreateNewScreeningProfile] TO frontdesk_appuser
GO
GRANT EXECUTE ON [dbo].[uspUpdateScreeningProfileAgeSettings] TO frontdesk_appuser
GO
GRANT EXECUTE ON [dbo].[uspUpdateScreeningProfileFrequency] TO frontdesk_appuser
GO



-- Adding ScreeningProfile to BranchLocation - Begin

-- 1. Add column
IF( EXISTS(select 1 from sys.columns WHERE object_id = OBJECT_ID('dbo.BranchLocation') and name = 'ScreeningProfileID'))
SET NOEXEC ON
GO


ALTER TABLE dbo.BranchLocation ADD ScreeningProfileID int NULL
GO
-- set default location
UPDATE dbo.BranchLocation SET ScreeningProfileID = 1

ALTER TABLE dbo.BranchLocation ALTER COLUMN ScreeningProfileID int NOT NULL

ALTER TABLE dbo.BranchLocation ADD CONSTRAINT FK__BranchLocation__ScreeningProfile FOREIGN KEY(ScreeningProfileID)  REFERENCES dbo.ScreeningProfile(ID)


GO
SET NOEXEC OFF
GO


-- Adding ScreeningProfile to BranchLocation - End



--- branching location SP

IF OBJECT_ID('[dbo].[uspGetAllBranchLocations]') IS NOT NULL
DROP PROCEDURE [dbo].[uspGetAllBranchLocations];

GO

CREATE PROCEDURE [dbo].[uspGetAllBranchLocations]
    @HideDisabled bit = 0
AS
    SELECT
        l.BranchLocationID,
        l.Name,
        l.Description,
        l.Disabled,
        p.ID as ScreeningProfileID,
        p.Name as ScreeningProfileName
    FROM dbo.BranchLocation l
        INNER JOIN dbo.ScreeningProfile p ON l.ScreeningProfileID = p.ID
    WHERE @HideDisabled = 0 OR l.Disabled = 0
    ORDER BY l.Name ASC
RETURN 0
GO

IF OBJECT_ID('[dbo].[uspGetBranchLocation]') IS NOT NULL
DROP PROCEDURE [dbo].[uspGetBranchLocation];

GO
CREATE PROCEDURE [dbo].[uspGetBranchLocation]
    @BranchLocationID int
AS
    SELECT
        l.BranchLocationID,
        l.Name,
        l.Description,
        l.Disabled,
        p.ID as ScreeningProfileID,
        p.Name as ScreeningProfileName
    FROM dbo.BranchLocation l
        INNER JOIN dbo.ScreeningProfile p ON l.ScreeningProfileID = p.ID
    WHERE BranchLocationID = @BranchLocationID
RETURN 0
GO




IF OBJECT_ID('[dbo].[vKiosk]') IS NOT NULL
DROP VIEW [dbo].[vKiosk];

GO

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
    p.Name as ScreeningProfileName
FROM dbo.Kiosk k
INNER JOIN dbo.BranchLocation l ON k.BranchLocationID = l.BranchLocationID
INNER JOIN dbo.ScreeningProfile p ON p.ID = l.ScreeningProfileID
GO


IF OBJECT_ID('[dbo].[uspGetScreeningProfileByKioskID]') IS NOT NULL
DROP PROCEDURE [dbo].[uspGetScreeningProfileByKioskID];

GO

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
---------------------------------------------
GO



IF OBJECT_ID('[dbo].[uspGetModifiedSectionMinimalAgeSettingsForKiosk]') IS NOT NULL
DROP PROCEDURE [dbo].[uspGetModifiedSectionMinimalAgeSettingsForKiosk];

GO

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

GO


IF OBJECT_ID('[dbo].uspGetModifiedSectionMinimalAgeSettings') IS NOT NULL
DROP PROCEDURE [dbo].[uspGetModifiedSectionMinimalAgeSettings]

GO

CREATE PROCEDURE [dbo].[uspGetModifiedSectionMinimalAgeSettings]
    @ScreeningProfileID int,
    @LastModifiedDateUTC datetime
AS
SELECT a.ScreeningSectionID, a.MinimalAge, a.IsEnabled, a.LastModifiedDateUTC
FROM dbo.ScreeningProfileSectionAge a
    INNER JOIN dbo.vKiosk k ON a.ScreeningProfileID = k.ScreeningProfileID
WHERE a.LastModifiedDateUTC > @LastModifiedDateUTC AND k.ScreeningProfileID = @ScreeningProfileID
ORDER BY a.ScreeningSectionID ASC
RETURN 0
GO


IF OBJECT_ID('[dbo].uspScreeningProfileRefreshKioskSettings') IS NOT NULL
DROP PROCEDURE [dbo].[uspScreeningProfileRefreshKioskSettings]

GO
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





GRANT EXECUTE ON [dbo].[uspGetModifiedSectionMinimalAgeSettingsForKiosk] TO frontdesk_appuser
GRANT EXECUTE ON [dbo].[uspGetScreeningProfileByKioskID] TO frontdesk_appuser
GRANT EXECUTE ON [dbo].[uspGetModifiedSectionMinimalAgeSettings] TO frontdesk_appuser
GRANT EXECUTE ON [dbo].[uspScreeningProfileRefreshKioskSettings] TO frontdesk_appuser
GO


GRANT EXECUTE ON [dbo].[uspGetAllBranchLocations] TO frontdesk_appuser
GO
GRANT EXECUTE ON [dbo].[uspGetBranchLocation] TO frontdesk_appuser
GO


IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '8.0.0.0')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('8.0.0.0');
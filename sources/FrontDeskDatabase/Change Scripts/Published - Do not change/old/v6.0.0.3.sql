---- BhsDemographics

IF EXISTS(SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('BhsDemographics') AND name = 'ExportedToHRN')
SET NOEXEC ON
GO

ALTER TABLE [dbo].BhsDemographics ADD  ExportedToHRN nvarchar(255) null;

SET NOEXEC OFF
GO

IF EXISTS(SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('BhsDemographics') AND name = 'PatientName')
SET NOEXEC ON
GO

ALTER TABLE [dbo].BhsDemographics ADD PatientName as dbo.fn_GetPatientName(LastName, FirstName, MiddleName) PERSISTED;

SET NOEXEC OFF

---- ScreeningResult
IF EXISTS(SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('ScreeningResult') AND name = 'PatientName')
SET NOEXEC ON
GO

ALTER TABLE [dbo].ScreeningResult ADD PatientName as dbo.fn_GetPatientName(LastName, FirstName, MiddleName) PERSISTED;

SET NOEXEC OFF
GO

-----------------------------


IF OBJECT_ID('[dbo].[uspSetMissingPatientAddress]') IS NOT NULL
DROP PROCEDURE [dbo].uspSetMissingPatientAddress
GO

CREATE PROCEDURE [dbo].[uspSetMissingPatientAddress]
    @ID bigint,
    @StreetAddress nvarchar(512),
    @City nvarchar(255),
    @StateID char(2),
    @ZipCode char(5),
    @Phone char(14),
    @ExportedToHRN nvarchar(255)

AS
SET NOCOUNT ON

-- update result
UPDATE dbo.ScreeningResult SET
    StreetAddress = @StreetAddress,
    City = @City, 
    StateID = @StateID,
    ZipCode = @ZipCode,
    Phone = @Phone,
    ExportedToHRN = @ExportedToHRN
WHERE ScreeningResultID = @ID

-- update other screenings for the same patient 
-- which happen later and does not contain address
UPDATE target
SET
    target.StreetAddress = source.StreetAddress,
    target.City = source.City, 
    target.StateID = source.StateID,
    target.ZipCode = source.ZipCode,
    target.Phone = source.Phone,
    target.ExportedToHRN = source.ExportedToHRN
FROM  dbo.ScreeningResult target 
    INNER JOIN dbo.ScreeningResult source ON
            target.ScreeningResultID > source.ScreeningResultID
            AND target.PatientName = source.PatientName
			AND target.Birthday = source.Birthday
WHERE source.ScreeningResultID = @ID AND ISNULL(target.StreetAddress, '') = ''


-- update demographics
UPDATE target
SET
    target.StreetAddress = @StreetAddress,
    target.City = @City, 
    target.StateID = @StateID,
    target.ZipCode = @ZipCode,
    target.Phone = @Phone,
    target.ExportedToHRN = @ExportedToHRN
FROM  dbo.BhsDemographics target 
    INNER JOIN dbo.ScreeningResult source 
        ON target.ScreeningResultID = source.ScreeningResultID 
            OR (source.PatientName = target.PatientName AND source.Birthday = target.Birthday)
WHERE source.ScreeningResultID = @ID 
    AND ISNULL(target.StreetAddress, '') = ''
GO



------------------------------------------------

GRANT EXECUTE ON  [dbo].uspSetMissingPatientAddress TO frontdesk_appuser

---------------------------------------------
IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '6.0.0.3')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('6.0.0.3');
IF OBJECT_ID('[dbo].[uspUpdateScreeningResultPatientInfo]') IS NOT NULL
    DROP PROCEDURE [dbo].[uspUpdateScreeningResultPatientInfo];
GO

CREATE PROCEDURE [dbo].[uspUpdateScreeningResultPatientInfo]
    @ScreeningResultID bigint,
    @FirstName nvarchar(128),
    @LastName nvarchar(128),
    @MiddleName nvarchar(128),
    @Birthday date,
    @Phone varchar(14),
    @StreetAddress nvarchar(512),
    @City nvarchar(255),
    @StateID varchar(2),
    @ZipCode varchar(5)
AS
BEGIN
    UPDATE dbo.ScreeningResult SET
    [FirstName] = @FirstName
    ,[LastName] = @LastName
    ,[MiddleName] = @MiddleName
    ,[Birthday] = @Birthday
    ,[Phone] = @Phone
    ,[StreetAddress] = @StreetAddress
    ,[City] = @City
    ,[StateID] = @StateID
    ,[ZipCode] = @ZipCode
WHERE ScreeningResultID = @ScreeningResultID;

-- update name 
UPDATE dbo.BhsDemographics SET
    [FirstName] = @FirstName
    ,[LastName] = @LastName
    ,[MiddleName] = @MiddleName
    ,[Birthday] = @Birthday
    ,[Phone] = @Phone
    ,[StreetAddress] = @StreetAddress
    ,[City] = @City
    ,[StateID] = @StateID
    ,[ZipCode] = @ZipCode
WHERE ScreeningResultID = @ScreeningResultID AND CompleteDate IS NULL;


-- reset export attempts if failed before
UPDATE l
SET l.FailedAttemptCount = 0, l.Completed = 0
FROM export.SmartExportLog l
    INNER JOIN dbo.ScreeningResult r ON l.ScreeningResultID = r.ScreeningResultID
WHERE l.ScreeningResultID = @ScreeningResultID 
    AND Succeed = 0
    AND r.ExportDate IS NULL


RETURN 0
END

GO



ALTER PROCEDURE [dbo].[uspFindMatchedPatientForExport]
    @LastName nvarchar(128),
    @FirstName nvarchar(128),
    @MiddleName nvarchar(128),
    @Birthday date
AS
BEGIN
    SELECT DISTINCT
        r.LastName,
        r.FirstName,
        r.MiddleName,
        r.Birthday,
        DIFFERENCE(ISNULL(r.MiddleName,''), dbo.ufnMapPatientName(@MiddleName)) AS MiddleNameDiff,
        r.ExportedToPatientID,
        r.ExportedToHRN
    FROM dbo.ScreeningResult r
        INNER JOIN export.SmartExportLog l ON r.ScreeningResultID = l.ScreeningResultID 
            AND l.Succeed = 1 and l.Completed = 1
    WHERE 
        l.ExportDate > '2020-09-21' /* ignore previous exports in DB where patient name might be wrong */
        AND r.Birthday = @Birthday
        AND DIFFERENCE(r.LastName, dbo.ufnMapPatientName(@LastName)) = 4
        AND DIFFERENCE(r.FirstName, dbo.ufnMapPatientName(@FirstName)) = 4
    ORDER BY DIFFERENCE(ISNULL(r.MiddleName,''), dbo.ufnMapPatientName(@MiddleName)) DESC
END
GO


IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '9.5.0.1')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('9.5.0.1');
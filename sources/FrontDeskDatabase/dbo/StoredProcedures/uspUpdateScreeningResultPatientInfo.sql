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

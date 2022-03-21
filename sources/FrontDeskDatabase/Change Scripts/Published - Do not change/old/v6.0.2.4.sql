

IF EXISTS(SELECT * 
FROM sys.indexes 
WHERE name='IX__BhsVisit_ScreeningResultID' AND object_id = OBJECT_ID('dbo.BhsVisit'))
SET NOEXEC ON
GO

create index IX__BhsVisit_ScreeningResultID ON dbo.BhsVisit(ScreeningResultID)

SET NOEXEC OFF
GO


IF EXISTS(SELECT * 
FROM sys.indexes 
WHERE name='IX__BhsDemographics_Birthday' AND object_id = OBJECT_ID('dbo.BhsDemographics'))
SET NOEXEC ON
GO

create index IX__BhsDemographics_Birthday ON dbo.BhsDemographics(Birthday) INCLUDE(PatientName, CreatedDate, ScreeningDate, CompleteDate)
GO

SET NOEXEC OFF
GO


ALTER PROCEDURE [dbo].[uspSetMissingPatientAddress]
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


UPDATE target
SET
     target.ExportedToHRN = source.ExportedToHRN
FROM  dbo.ScreeningResult target 
    INNER JOIN dbo.ScreeningResult source ON
            target.ScreeningResultID > source.ScreeningResultID
            AND target.PatientName = source.PatientName
			AND target.Birthday = source.Birthday
WHERE source.ScreeningResultID = @ID AND ISNULL(target.ExportedToHRN, '') = ''



-- update demographics - address
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
  

-- update demographics - EHR
UPDATE target
SET
    target.ExportedToHRN = @ExportedToHRN
FROM  dbo.BhsDemographics target 
    INNER JOIN dbo.ScreeningResult source 
        ON target.ScreeningResultID = source.ScreeningResultID 
            OR (source.PatientName = target.PatientName AND source.Birthday = target.Birthday)
WHERE source.ScreeningResultID = @ID 
    AND ISNULL(target.ExportedToHRN, '') = ''
  
GO


print 'Populating dbo.[SecurityEventCategory]...'
GO
MERGE INTO dbo.SecurityEventCategory as target
USING (VALUES(1, 'System Security'),
                    (2, 'Accessing patient info'),
                    (3, 'Branch management'),
                    (4, 'Kiosk management')
) as source(SecurityEventCategoryID, CategoryName) 
    ON target.SecurityEventCategoryID = source.SecurityEventCategoryID
WHEN MATCHED THEN 
    UPDATE SET CategoryName = source.CategoryName
WHEN NOT MATCHED BY TARGET THEN
    INSERT(SecurityEventCategoryID, CategoryName) 
        VALUES (source.SecurityEventCategoryID, source.CategoryName)
;
GO

GO	
print 'Populating dbo.[SecurityEvent]...'
GO

MERGE INTO dbo.SecurityEvent as target
USING ( VALUES
--system security
(1, 1, 'User was logged into the system',1),
(2, 1, 'Password was changed',1),
(3, 1, 'Security question and/or answer were changed',1),
(4, 1, 'New user was created',1),
(5, 1, 'New account was activated',1),
-- accessing screen results 
(6, 2, 'Behavioral Health Screening Report was read',1),
(7, 2, 'Behavioral Health Screening Report was printed',1),
(12, 2, 'Behavioral Health Screening Report was removed',1),
(13, 2, 'Patient contact information was changed',1),	  
(14, 2, 'Behavioral Health Screening Report was exported',1),
(15, 2, 'BHS Visit Information was completed',1),
(16, 2, 'BHS Visit was created manually',1),
(18, 2, 'BHS Patient Demographics was completed',1),
(19, 2, 'BHS Follow-Up was completed',1),
(20, 2, 'Patient address was updated from RPMS',1),
(21, 2, 'BHS Visit Report was printed',1),
(22, 2, 'BHS Follow-Up Report was printed',1),
(23, 2, 'BHS Patient Demographics was printed',1),


-- Branch location mgmt
(8, 3, 'New branch location was created',1),
(9, 3, 'Branch location was removed',1),
-- Kiosk mgmt
(10, 4, 'New kiosk was registered',1),
(11, 4, 'Kiosk was removed',1)
) as source(SecurityEventID, SecurityEventCategoryID, [Description], Enabled) 
    ON target.SecurityEventID = source.SecurityEventID
WHEN MATCHED THEN 
    UPDATE SET [Description] = source.[Description], Enabled = source.Enabled
WHEN NOT MATCHED BY TARGET THEN
    INSERT(SecurityEventID, SecurityEventCategoryID, [Description], Enabled) 
        VALUES (source.SecurityEventID, source.SecurityEventCategoryID, source.[Description], Enabled)
;
GO


---------------------------------------------
IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '6.0.2.4')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('6.0.2.4');
------------------ UPDATE DATA

print 'Populating dbo.[Race]...'
GO
MERGE INTO dbo.[Race] as target
USING (VALUES
(1, 'American Indian', 2),
(7, 'Alaska Native', 3),
(2, 'Asian', 4),
(3, 'Black or African American', 5),
(4, 'Hispanic or Latino', 6),
(5, 'Native Hawaiian or Other Pacific Islander', 7),
(8, 'White', 8),
(9, 'Other', 9),
(6, 'Unknown', 10)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex, LastModifiedDateUTC = GETUTCDATE()
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
;
GO

print 'Populating dbo.[Gender]...'
GO
MERGE INTO dbo.[Gender] as target
USING (VALUES
(1, 'Male', 2),
(2, 'Female', 3),
(3, 'Transgender', 4),
(5, 'Not Female, Male, or Transgender', 5),
(4, 'Don’t Know', 6),
(6, 'Decline to Answer', 7)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex, LastModifiedDateUTC = GETUTCDATE()
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
;
GO


print 'Populating dbo.[SexualOrientation]...'
GO
MERGE INTO dbo.[SexualOrientation] as target
USING (VALUES
(1, 'Straight/Heterosexual', 2),
(3, 'Gay/Lesbian', 3),
(2, 'Bisexual', 4),
(5, 'Other', 5),
(4, 'Don’t Know', 6),
(6, 'Decline to Answer', 7)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex, LastModifiedDateUTC = GETUTCDATE()
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
;
GO



print 'Populating dbo.[MaritalStatus]...'
GO
MERGE INTO dbo.[MaritalStatus] as target
USING (VALUES
(4, 'Single', 2),
(2, 'Married', 3),
(3, 'Partner', 4),
(6, 'Separated', 5),
(1, 'Divorced', 6),
(5, 'Widowed', 7)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex, LastModifiedDateUTC = GETUTCDATE()
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
;
GO


UPDATE dbo.BhsDemographics SET EducationLevelID = 7 WHERE EducationLevelID = 4; /* Migrate Some college (4)  to Bachelor's Degree */
GO

MERGE INTO dbo.[EducationLevel] as target
USING (VALUES
(1, 'Elementary School', 2),
(2, 'Some High School', 3),
(11, 'Completed GED or High School Equivalent', 4),
(3, 'Completed High School Diploma', 5),
(5, 'Technical School', 6),
(6, 'AA degree', 7),
(7, 'Bachelor''s Degree', 8),
(8, 'Master''s Degree', 9),
(10, 'Doctoral Degree', 10)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex, LastModifiedDateUTC = GETUTCDATE()
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
WHEN NOT MATCHED BY SOURCE THEN
    DELETE ;
GO



IF NOT EXISTS(SELECT 1 FROM dbo.Users WHERE username = 'kiosk')
BEGIN
INSERT INTO [dbo].[Users]
           (
            [Username]
           ,[Email]
           ,[Comment]
           ,[Password]
           ,[PasswordQuestion]
           ,[PasswordAnswer]	
           ,[IsApproved]
           ,[IsLockedOut]
           ,[FailedPasswordAttemptCount]
           ,[FailedPasswordAttemptWindowStart]
           ,[FailedPasswordAnswerAttemptCount]
           ,[FailedPasswordAnswerAttemptWindowStart]
            ,[CreationDate]
           ,[LastActivityDate])
     VALUES
           (
            'kiosk',
            '',
            'Built-in Kiosk User',
            'Sxuu1WEDNtOlDrmyf8uVqGjwZUw=',
            NULL,
            NULL,
            1,
            0,
            2,
            GETDATE(),
            1,
            GETDATE(),
            GETDATE(),
            GETDATE()
    );

    INSERT INTO UsersInRoles ([UserName], [RoleName]) VALUES ('kiosk', 'Medical Professionals');

    declare @exportIserID BIGINT;

    SET @exportIserID = (SELECT PKID FROM dbo.Users WHERE Username = 'kiosk');


    INSERT INTO dbo.UserDetails 
    (
        UserID,
        FirstName,    
        LastName,
        MiddleName,
        StateCode,
        City,
        ContactPhone,
        AddressLine1,
        AddressLine2,
        PostalCode	
    )
    VALUES
    (
         @exportIserID,
        'Built-in',    
        'Kiosk',
        null,
        null,
        null,
        null,
        null,
        null,
        null	
    );

END
;

-- Section Qiestions, with Y/N answer
MERGE INTO dbo.ScreeningSection as target
USING ( VALUES
('CIF', 'BHS', 'CIF', 'Contact Information', '', 0),
('SIH', 'BHS', 'SIH', 'Smoker in the Home', 'Does anyone in the home smoke tobacco (such as cigarettes, cigars, pipes, electronic nicotine delivery devices, etc.)?', 2),
('TCC', 'BHS', 'TCC', 'Tobacco Use', 'Do you use tobacco?', 3),
('CAGE', 'BHS', 'CAGE', 'Alcohol Use (CAGE)', 'Do you drink alcohol?',4),
('DAST', 'BHS', 'DAST-10', 'Non-Medical Drug Use (DAST-10)', 'Have you used drugs other than those required for medical reasons?',5),
('PHQ-9', 'BHS', 'PHQ-9', 'Depression (PHQ-9)', 'Do you feel down, depressed, or hopeless?', 7),
('HITS', 'BHS', 'HITS', 'Intimate Partner/Domestic Violence (HITS)', 'Do you feel UNSAFE in your home?', 8),
('DOCH', 'BHS', 'DOCH', 'Drug of Choice', 'What Drug do you USE THE MOST?', 6),
('DMGR', 'BHS', 'DMGR', 'Patient Demographics', '', 1)
) AS source(ScreeningSectionID, ScreeningID, ScreeningSectionShortName, ScreeningSectionName, QuestionText, OrderIndex)
    ON source.ScreeningSectionID = target.ScreeningSectionID
WHEN MATCHED THEN  
    UPDATE SET ScreeningID = source.ScreeningID, 
        ScreeningSectionShortName = source.ScreeningSectionShortName, 
        ScreeningSectionName = source.ScreeningSectionName,
        QuestionText = source.QuestionText,
        OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN 
    INSERT (ScreeningSectionID,ScreeningID,ScreeningSectionShortName, ScreeningSectionName,QuestionText, OrderIndex) 
        VALUES (source.ScreeningSectionID, source.ScreeningID, source.ScreeningSectionShortName, source.ScreeningSectionName, source.QuestionText, source.OrderIndex)  
;
GO


IF NOT EXISTS(SELECT * FROM dbo.ScreeningFrequency WHERE ID = 'DMGR ')
    insert into ScreeningFrequency(ID, Frequency, LastModifiedDateUTC) values('DMGR', 0, GETUTCDATE());
GO




ALTER VIEW [dbo].[vBhsVisitsWithDemographics] AS
SELECT
v.ID
,v.ScreeningResultID
,v.CreatedDate
,v.ScreeningDate
,v.CompleteDate
,CASE WHEN r.StreetAddress IS NOT NULL THEN 1 ELSE 0 END AS HasAddress
,1 as IsVisitRecordType
,v.LocationID
,l.Name as LocationName
,r.Birthday
,r.PatientName as FullName
,pd.ID as DemographicsID
,pd.ScreeningDate as DemographicsScreeningDate
,pd.CreatedDate as DemographicsCreateDate
,pd.CompleteDate as  DemographicsCompleteDate
FROM 
	dbo.BhsVisit v 
	INNER JOIN dbo.ScreeningResult r ON v.ScreeningResultID = r.ScreeningResultID
    INNER JOIN dbo.BranchLocation l ON l.BranchLocationID = v.LocationID
	LEFT JOIN dbo.BhsDemographics pd ON pd.Birthday = r.Birthday AND pd.PatientName = r.PatientName

WHERE r.IsDeleted = 0 and r.IsDeleted = 0 
UNION ALL
SELECT
NULL AS ID
,pd.ScreeningResultID
,r.CreatedDate
,pd.ScreeningDate as ScreeningDate
,pd.CompleteDate
,0 As HasAddress
,0 as IsVisitRecordType
,pd.LocationID
,l.Name as LocationName
,r.Birthday
,r.PatientName as FullName
,pd.ID as DemographicsID
,pd.ScreeningDate as DemographicsScreeningDate
,pd.CreatedDate as DemographicsCreateDate
,pd.CompleteDate as  DemographicsCompleteDate
FROM 
	dbo.BhsDemographics pd
	INNER JOIN dbo.ScreeningResult r ON pd.ScreeningResultID = r.ScreeningResultID
    INNER JOIN dbo.BranchLocation l ON l.BranchLocationID = pd.LocationID
WHERE r.IsDeleted = 0 and r.IsDeleted = 0 AND
NOT EXISTS( 
SELECT 1 FROM dbo.BhsVisit v 
INNER JOIN dbo.ScreeningResult r2 ON v.ScreeningResultID = r2.ScreeningResultID 
WHERE r2.PatientName = pd.PatientName AND r2.Birthday = pd.Birthday)
GO


---------------------------------------------
IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '7.0.0.5')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('7.0.0.5');
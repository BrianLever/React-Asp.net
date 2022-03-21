
print 'Populating dbo.[TreatmentAction]...'
GO

UPDATE BhsVisit SET TreatmentAction1ID = 1 WHERE TreatmentAction1ID IN (7,8);
UPDATE BhsVisit SET TreatmentAction2ID = 1 WHERE TreatmentAction2ID IN (7,8);
UPDATE BhsVisit SET TreatmentAction3ID = 1 WHERE TreatmentAction3ID IN (7,8);
UPDATE BhsVisit SET TreatmentAction4ID = 1 WHERE TreatmentAction4ID IN (7,8);
UPDATE BhsVisit SET TreatmentAction5ID = 1 WHERE TreatmentAction5ID IN (7,8);
GO

MERGE INTO dbo.TreatmentAction as target
USING (VALUES
(1, 'Evaluation', 1),
(2, 'Education', 2),
(3, 'Brief Intervention', 3),
(4, 'Brief Treatment', 4),
(5, 'Referral to Treatment', 5),
(6, 'Other', 8)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
WHEN NOT MATCHED BY SOURCE THEN
    DELETE
;
GO

print 'Populating dbo.NewVisitReferralRecommendation...'
GO
MERGE INTO dbo.NewVisitReferralRecommendation as target
USING (VALUES
(1, 'Behavioral Health Department', 1),
(2, 'Crisis/Emergency Service', 2),
(3, 'Medical Department', 3),
(4, 'Medication	Assisted Treatment (MAT)', 4),
(5, 'Pain Management', 5),
(6, 'Psychiatrist', 6),
(9, 'RX', 7),
(7, 'Other', 8),
(8, 'Not indicated/offered', 100)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
WHEN NOT MATCHED BY SOURCE THEN
    DELETE
;

print 'Populating dbo.[PatientAttendedVisit]...'
GO

UPDATE BhsFollowUp SET PatientAttendedVisitID = 1 WHERE PatientAttendedVisitID IN (2,3,4,5,6,7);
UPDATE BhsFollowUp SET PatientAttendedVisitID = 2 WHERE PatientAttendedVisitID = 10;
UPDATE BhsFollowUp SET PatientAttendedVisitID = 3 WHERE PatientAttendedVisitID = 9;
UPDATE BhsFollowUp SET PatientAttendedVisitID = 4 WHERE PatientAttendedVisitID = 8;



MERGE INTO dbo.PatientAttendedVisit as target
USING (VALUES
(1, 'Yes', 1),
(2, 'No', 2),
(3, 'Unknown', 3),
(4, 'Not indicated/offered', 4)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
WHEN NOT MATCHED BY SOURCE THEN
    DELETE
;
GO


ALTER VIEW [dbo].[vBhsDemographics]
AS 
SELECT 
d.[ID]
,d.[ScreeningResultID]
,d.[LocationID]
,location.Name as LocationName
,d.[CreatedDate]
,d.[ScreeningDate]
,d.[BhsStaffNameCompleted]
,d.[CompleteDate]
,d.PatientName as FullName
,d.[FirstName]
,d.[LastName]
,d.[MiddleName]
,d.[Birthday]
,d.[StreetAddress]
,d.[City]
,d.[StateID]
,state.Name as StateName
,d.[ZipCode]
,d.[Phone]
,d.[RaceID]
,r.[Name] as RaceName
,d.[GenderID]
,g.Name as GenderName
,d.[SexualOrientationID]
,s.Name as SexualOrientationName
,d.[TribalAffiliation]
,d.[MaritalStatusID]
,m.Name as [MaritalStatusName]
,d.[EducationLevelID]
,e.Name as EducationLevelName
,d.[LivingOnReservationID]
,l.Name as LivingOnReservationName
,d.[CountyOfResidence]
,d.[MilitaryExperienceID]
,military.Name as MilitaryExperienceName
,d.ExportedToHRN
FROM [dbo].[BhsDemographics] d
	INNER JOIN dbo.BranchLocation location ON d.LocationID = location.BranchLocationID
    LEFT JOIN dbo.State state ON d.StateID = state.StateCode 
    LEFT JOIN dbo.Race r ON d.RaceID = r.ID
	LEFT JOIN dbo.Gender g ON d.GenderID = g.ID
	LEFT JOIN dbo.SexualOrientation s ON d.SexualOrientationID = s.ID
	LEFT JOIN dbo.MaritalStatus m ON d.MaritalStatusID = m.ID
	LEFT JOIN dbo.EducationLevel e ON d.EducationLevelID = e.ID
	LEFT JOIN dbo.LivingOnReservation l ON d.LivingOnReservationID = l.ID
	LEFT JOIN dbo.MilitaryExperience military ON d.MilitaryExperienceID = military.ID
;

GO

IF OBJECT_ID('[dbo].[fn_IntListToTable]') IS NOT NULL
DROP FUNCTION [dbo].[fn_IntListToTable];
GO

CREATE FUNCTION [dbo].[fn_IntListToTable] (@InputString varchar(4000))
RETURNS  @OutputTable TABLE([value] BIGINT)
AS
BEGIN
    DECLARE @val VARCHAR(10),
    @Delimiter nvarchar(1) = ',';

    WHILE LEN(@InputString) > 0
    BEGIN
        SET @val = LEFT(@inputString, 
            ISNULL(NULLIF(CHARINDEX(@Delimiter, @InputString) - 1, -1),
            LEN(@InputString)))

        SET @InputString = SUBSTRING(@InputString,
                                     ISNULL(NULLIF(CHARINDEX(@Delimiter, @InputString), 0),
                                     LEN(@InputString)) + 1, LEN(@InputString))

        INSERT INTO @OutputTable([value]) VALUES (@val)
    END

    RETURN
END
GO


IF EXISTS(SELECT 1
    FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS  WHERE CONSTRAINT_NAME ='FK_BhsDemographics__MilitaryExperience')
    alter table dbo.bhsDemographics DROP CONSTRAINT FK_BhsDemographics__MilitaryExperience;
;
GO
IF EXISTS(SELECT 1
    FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS  WHERE CONSTRAINT_NAME ='CK_BhsDemographics__MilitaryExperience')
    alter table dbo.bhsDemographics DROP CONSTRAINT CK_BhsDemographics__MilitaryExperience;
;
GO

alter table dbo.bhsDemographics ALTER COLUMN MilitaryExperienceID varchar(32) NULL;

GO

IF OBJECT_ID('[dbo].[fn_CheckMilitaryExperienceValues]') IS NOT NULL
DROP FUNCTION [dbo].[fn_CheckMilitaryExperienceValues]
GO
;
CREATE FUNCTION [dbo].[fn_CheckMilitaryExperienceValues]
(
    @arrayString varchar(32)
)
RETURNS INT
AS
BEGIN
    declare @expectedCount int = 0,
    @actualCount int = 0;
    
    IF @arrayString IS NULL 
        RETURN 1;


    declare @IdValues table ([value] int);

    INSERT INTO @IdValues
    SELECT [value]
    FROM dbo.fn_IntListToTable(@arrayString);
    

    SET @expectedCount = (SELECT COUNT(*) FROM @IdValues);
    
    SET @actualCount = (
        SELECT COUNT(*)
        FROM @IdValues id INNER JOIN dbo.MilitaryExperience m ON id.value = m.ID
        );

    IF @actualCount = @expectedCount RETURN 1
    
    RETURN 0;
   
END
;
GO
alter table dbo.bhsDemographics ADD CONSTRAINT CK_BhsDemographics__MilitaryExperience 
    CHECK([dbo].[fn_CheckMilitaryExperienceValues](MilitaryExperienceID) = 1)
;
GO
ALTER VIEW [dbo].[vBhsDemographics]
AS 
SELECT 
d.[ID]
,d.[ScreeningResultID]
,d.[LocationID]
,location.Name as LocationName
,d.[CreatedDate]
,d.[ScreeningDate]
,d.[BhsStaffNameCompleted]
,d.[CompleteDate]
,d.PatientName as FullName
,d.[FirstName]
,d.[LastName]
,d.[MiddleName]
,d.[Birthday]
,d.[StreetAddress]
,d.[City]
,d.[StateID]
,state.Name as StateName
,d.[ZipCode]
,d.[Phone]
,d.[RaceID]
,r.[Name] as RaceName
,d.[GenderID]
,g.Name as GenderName
,d.[SexualOrientationID]
,s.Name as SexualOrientationName
,d.[TribalAffiliation]
,d.[MaritalStatusID]
,m.Name as [MaritalStatusName]
,d.[EducationLevelID]
,e.Name as EducationLevelName
,d.[LivingOnReservationID]
,l.Name as LivingOnReservationName
,d.[CountyOfResidence]
,d.[MilitaryExperienceID]
,d.ExportedToHRN
FROM [dbo].[BhsDemographics] d
	INNER JOIN dbo.BranchLocation location ON d.LocationID = location.BranchLocationID
    LEFT JOIN dbo.State state ON d.StateID = state.StateCode 
    LEFT JOIN dbo.Race r ON d.RaceID = r.ID
	LEFT JOIN dbo.Gender g ON d.GenderID = g.ID
	LEFT JOIN dbo.SexualOrientation s ON d.SexualOrientationID = s.ID
	LEFT JOIN dbo.MaritalStatus m ON d.MaritalStatusID = m.ID
	LEFT JOIN dbo.EducationLevel e ON d.EducationLevelID = e.ID
	LEFT JOIN dbo.LivingOnReservation l ON d.LivingOnReservationID = l.ID
	LEFT JOIN dbo.MilitaryExperience military ON d.MilitaryExperienceID = military.ID
;
GO


IF OBJECT_ID('[dbo].[fn_GetMilitaryExperienceNames]') IS NOT NULL
DROP FUNCTION [dbo].[fn_GetMilitaryExperienceNames]
GO

CREATE FUNCTION [dbo].[fn_GetMilitaryExperienceNames]
(
    @idValues varchar(32)
)
RETURNS  varchar(4000)
AS
BEGIN
    DECLARE @str NVARCHAR(MAX)

    DECLARE @Delimiter CHAR(1) = ','

    SELECT @str = COALESCE(@str + @Delimiter,'') + Name 
    FROM dbo.MilitaryExperience WHERE ID IN (
        SELECT [value] FROM dbo.fn_IntListToTable(@IdValues)
    )

    RETURN RTRIM(LTRIM(@str))
END
GO
;

ALTER VIEW [dbo].[vBhsDemographics]
AS 
SELECT 
d.[ID]
,d.[ScreeningResultID]
,d.[LocationID]
,location.Name as LocationName
,d.[CreatedDate]
,d.[ScreeningDate]
,d.[BhsStaffNameCompleted]
,d.[CompleteDate]
,d.PatientName as FullName
,d.[FirstName]
,d.[LastName]
,d.[MiddleName]
,d.[Birthday]
,d.[StreetAddress]
,d.[City]
,d.[StateID]
,state.Name as StateName
,d.[ZipCode]
,d.[Phone]
,d.[RaceID]
,r.[Name] as RaceName
,d.[GenderID]
,g.Name as GenderName
,d.[SexualOrientationID]
,s.Name as SexualOrientationName
,d.[TribalAffiliation]
,d.[MaritalStatusID]
,m.Name as [MaritalStatusName]
,d.[EducationLevelID]
,e.Name as EducationLevelName
,d.[LivingOnReservationID]
,l.Name as LivingOnReservationName
,d.[CountyOfResidence]
,d.[MilitaryExperienceID]
,dbo.fn_GetMilitaryExperienceNames(d.[MilitaryExperienceID]) as MilitaryExperienceName
,d.ExportedToHRN
FROM [dbo].[BhsDemographics] d
	INNER JOIN dbo.BranchLocation location ON d.LocationID = location.BranchLocationID
    LEFT JOIN dbo.State state ON d.StateID = state.StateCode 
    LEFT JOIN dbo.Race r ON d.RaceID = r.ID
	LEFT JOIN dbo.Gender g ON d.GenderID = g.ID
	LEFT JOIN dbo.SexualOrientation s ON d.SexualOrientationID = s.ID
	LEFT JOIN dbo.MaritalStatus m ON d.MaritalStatusID = m.ID
	LEFT JOIN dbo.EducationLevel e ON d.EducationLevelID = e.ID
	LEFT JOIN dbo.LivingOnReservation l ON d.LivingOnReservationID = l.ID
	LEFT JOIN dbo.MilitaryExperience military ON d.MilitaryExperienceID = military.ID
;

GO

---------------------------------------------
IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '6.2.1.0')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('6.2.1.0');
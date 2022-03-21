CREATE VIEW [dbo].[vBhsDemographics]
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


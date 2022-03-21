IF OBJECT_ID('[dbo].[vBhsDemographics]') IS NOT NULL
DROP VIEW [dbo].[vBhsDemographics]
GO
;
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
,military.Name as MilitaryExperienceName
,d.ExportedToHRN
FROM [dbo].[BhsDemographics] d
	INNER JOIN dbo.BranchLocation location ON d.LocationID = location.BranchLocationID
    INNER JOIN dbo.State state ON d.StateID = state.StateCode 
    LEFT JOIN dbo.Race r ON d.RaceID = r.ID
	LEFT JOIN dbo.Gender g ON d.GenderID = g.ID
	LEFT JOIN dbo.SexualOrientation s ON d.SexualOrientationID = s.ID
	LEFT JOIN dbo.MaritalStatus m ON d.MaritalStatusID = m.ID
	LEFT JOIN dbo.EducationLevel e ON d.EducationLevelID = e.ID
	LEFT JOIN dbo.LivingOnReservation l ON d.LivingOnReservationID = l.ID
	LEFT JOIN dbo.MilitaryExperience military ON d.MilitaryExperienceID = military.ID
;

GO

IF OBJECT_ID('[dbo].[vBhsFollowUpForExport]') IS NOT NULL
DROP VIEW [dbo].[vBhsFollowUpForExport]
GO
;
CREATE VIEW [dbo].[vBhsFollowUpForExport]
AS SELECT 
r.PatientName as FullName
,r.LastName
,r.FirstName
,r.MiddleName
,r.Birthday
,r.ExportedToHRN
,pd.ID as DemographicsID
,f.ScreeningResultID
,visit.ScreeningDate
,f.ID
,f.CreatedDate
,f.CompleteDate
,f.[BhsStaffNameCompleted]
,visit.LocationID
,l.Name as BranchLocationName
,f.[NewVisitReferralRecommendationID]
,refRec.[Name] as NewVisitReferralRecommendationName
,f.[NewVisitReferralRecommendationDescription]
,f.[NewVisitReferralRecommendationAcceptedID]
,accept.[Name] as NewVisitReferralRecommendationAcceptedName
,f.[ReasonNewVisitReferralRecommendationNotAcceptedID]
,notaccept.Name as ReasonNewVisitReferralRecommendationNotAcceptedName
,f.[NewVisitDate]
,f.[DischargedID]
,discharge.Name as DischargedName
,f.[ThirtyDatyFollowUpFlag]
,f.[NewFollowUpDate]
,f.[Notes]
,f.[PatientAttendedVisitID]
,av.[Name] as PatientAttendedVisitName
,f.BhsVisitID
,f.[ParentFollowUpID]
,f.VisitDate
,f.[FollowUpDate]
,f.[FollowUpContactDate]
,f.[FollowUpContactOutcomeID]
,co.[Name] as FollowUpContactOutcomeName

FROM 
	dbo.BhsFollowUp f 
	INNER JOIN dbo.BhsVisit visit ON f.BhsVisitID = visit.ID
	INNER JOIN dbo.ScreeningResult r ON f.ScreeningResultID = r.ScreeningResultID
	INNER JOIN dbo.BranchLocation l ON visit.LocationID = l.BranchLocationID
	LEFT JOIN dbo.BhsDemographics pd ON pd.Birthday = r.Birthday AND pd.PatientName = r.PatientName
    LEFT JOIN dbo.PatientAttendedVisit av ON f.PatientAttendedVisitID = av.ID
    LEFT JOIN dbo.FollowUpContactOutcome co ON f.FollowUpContactOutcomeID = co.ID
    LEFT JOIN dbo.NewVisitReferralRecommendation refRec ON f.NewVisitReferralRecommendationID = refRec.ID
	LEFT JOIN dbo.NewVisitReferralRecommendationAccepted accept ON f.[NewVisitReferralRecommendationAcceptedID] = accept.ID
	LEFT JOIN dbo.ReasonNewVisitReferralRecommendationNotAccepted notaccept ON f.ReasonNewVisitReferralRecommendationNotAcceptedID = notaccept.ID
	LEFT JOIN dbo.Discharged discharge ON f.[DischargedID] = discharge.ID

;
GO

IF OBJECT_ID('[dbo].[vBhsVisitForExport]') IS NOT NULL
DROP VIEW [dbo].[vBhsVisitForExport]
GO
;

CREATE VIEW [dbo].[vBhsVisitForExport]
AS SELECT 
r.PatientName as FullName
,r.LastName
,r.FirstName
,r.MiddleName
,r.Birthday
,r.ExportedToHRN
,pd.ID as DemographicsID
,v.ScreeningResultID
,v.ScreeningDate
,v.ID
,v.CreatedDate
,v.CompleteDate
,v.[BhsStaffNameCompleted]
,v.LocationID
,l.Name as BranchLocationName
,v.[NewVisitReferralRecommendationID]
,refRec.[Name] as NewVisitReferralRecommendationName
,v.[NewVisitReferralRecommendationDescription]
,v.[NewVisitReferralRecommendationAcceptedID]
,accept.[Name] as NewVisitReferralRecommendationAcceptedName
,v.[ReasonNewVisitReferralRecommendationNotAcceptedID]
,notaccept.Name as ReasonNewVisitReferralRecommendationNotAcceptedName
,v.[NewVisitDate]
,v.[DischargedID]
,discharge.Name as DischargedName
,v.[ThirtyDatyFollowUpFlag]
,v.[FollowUpDate]
,v.[Notes]

,v.[TreatmentAction1ID]
,ta1.Name as TreatmentAction1Name
,v.[TreatmentAction1Description]
,v.[TreatmentAction2ID]
,ta2.Name as TreatmentAction2Name
,v.[TreatmentAction2Description]
,v.[TreatmentAction3ID]
,ta3.Name as TreatmentAction3Name
,v.[TreatmentAction3Description]
,v.[TreatmentAction4ID]
,ta4.Name as TreatmentAction4Name
,v.[TreatmentAction4Description]
,v.[TreatmentAction5ID]
,ta5.Name as TreatmentAction5Name
,v.[TreatmentAction5Description]
,v.[OtherScreeningTools]

,v.[TobacoExposureSmokerInHomeFlag]
,v.[TobacoExposureCeremonyUseFlag]
,v.[TobacoExposureSmokingFlag]
,v.[TobacoExposureSmoklessFlag]
,v.[AlcoholUseFlagScoreLevel]
,v.[AlcoholUseFlagScoreLevelLabel]
,v.[SubstanceAbuseFlagScoreLevel]
,v.[SubstanceAbuseFlagScoreLevelLabel]
,v.[DepressionFlagScoreLevel]
,v.[DepressionFlagScoreLevelLabel]
,v.[DepressionThinkOfDeathAnswer]
,v.[PartnerViolenceFlagScoreLevel]
,v.[PartnerViolenceFlagScoreLevelLabel]


FROM 
	dbo.BhsVisit v 
	INNER JOIN dbo.ScreeningResult r ON v.ScreeningResultID = r.ScreeningResultID
	INNER JOIN dbo.BranchLocation l ON v.LocationID = l.BranchLocationID
	LEFT JOIN dbo.BhsDemographics pd ON pd.Birthday = r.Birthday AND pd.PatientName = r.PatientName
	LEFT JOIN dbo.NewVisitReferralRecommendation refRec ON v.NewVisitReferralRecommendationID = refRec.ID
	LEFT JOIN dbo.NewVisitReferralRecommendationAccepted accept ON v.[NewVisitReferralRecommendationAcceptedID] = accept.ID
	LEFT JOIN dbo.ReasonNewVisitReferralRecommendationNotAccepted notaccept ON v.ReasonNewVisitReferralRecommendationNotAcceptedID = notaccept.ID
	LEFT JOIN dbo.Discharged discharge ON v.[DischargedID] = discharge.ID
    LEFT JOIN dbo.TreatmentAction ta1 ON ta1.ID = v.[TreatmentAction1ID]
    LEFT JOIN dbo.TreatmentAction ta2 ON ta2.ID = v.[TreatmentAction2ID]
    LEFT JOIN dbo.TreatmentAction ta3 ON ta3.ID = v.[TreatmentAction3ID]
    LEFT JOIN dbo.TreatmentAction ta4 ON ta4.ID = v.[TreatmentAction4ID]
    LEFT JOIN dbo.TreatmentAction ta5 ON ta5.ID = v.[TreatmentAction5ID]

WHERE r.IsDeleted = 0 and r.IsDeleted = 0 
;

GO


---------------------------------------------
IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '6.1.1.0')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('6.1.1.0');
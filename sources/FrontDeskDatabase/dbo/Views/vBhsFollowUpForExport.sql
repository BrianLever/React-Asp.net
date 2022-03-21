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
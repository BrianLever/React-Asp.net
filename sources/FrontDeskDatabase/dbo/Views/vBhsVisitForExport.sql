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
,v.AnxietyFlagScoreLevel
,v.AnxietyFlagScoreLevelLabel
,v.[ProblemGamblingFlagScoreLevel]
,v.[ProblemGamblingFlagScoreLevelLabel]

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


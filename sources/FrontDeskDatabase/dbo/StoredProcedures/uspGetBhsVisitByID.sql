CREATE PROCEDURE [dbo].[uspGetBhsVisitByID]
    @ID bigint
AS
SELECT v.[ID]
,v.[ScreeningResultID]
,v.[LocationID]
,v.[CreatedDate]
,v.[ScreeningDate]
,v.[TobacoExposureSmokerInHomeFlag]
,v.[TobacoExposureCeremonyUseFlag]
,v.[TobacoExposureSmokingFlag]
,v.[TobacoExposureSmoklessFlag]
,v.[AlcoholUseFlagScoreLevel]
,v.[AlcoholUseFlagScoreLevelLabel]
,v.[SubstanceAbuseFlagScoreLevel]
,v.[SubstanceAbuseFlagScoreLevelLabel]
,v.[AnxietyFlagScoreLevel]
,v.[AnxietyFlagScoreLevelLabel]
,v.[DepressionFlagScoreLevel]
,v.[DepressionFlagScoreLevelLabel]
,v.[DepressionThinkOfDeathAnswer]
,v.[PartnerViolenceFlagScoreLevel]
,v.[PartnerViolenceFlagScoreLevelLabel]
,v.[ProblemGamblingFlagScoreLevel]
,v.[ProblemGamblingFlagScoreLevelLabel]
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
,v.[BhsStaffNameCompleted]
,v.[CompleteDate]
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
FROM [dbo].[BhsVisit] v
    LEFT JOIN dbo.NewVisitReferralRecommendation refRec ON v.NewVisitReferralRecommendationID = refRec.ID
    LEFT JOIN dbo.NewVisitReferralRecommendationAccepted accept ON v.[NewVisitReferralRecommendationAcceptedID] = accept.ID
    LEFT JOIN dbo.ReasonNewVisitReferralRecommendationNotAccepted notaccept ON v.ReasonNewVisitReferralRecommendationNotAcceptedID = notaccept.ID
    LEFT JOIN dbo.Discharged discharge ON v.[DischargedID] = discharge.ID
    LEFT JOIN dbo.TreatmentAction ta1 ON ta1.ID = v.[TreatmentAction1ID]
    LEFT JOIN dbo.TreatmentAction ta2 ON ta2.ID = v.[TreatmentAction2ID]
    LEFT JOIN dbo.TreatmentAction ta3 ON ta3.ID = v.[TreatmentAction3ID]
    LEFT JOIN dbo.TreatmentAction ta4 ON ta4.ID = v.[TreatmentAction4ID]
    LEFT JOIN dbo.TreatmentAction ta5 ON ta5.ID = v.[TreatmentAction5ID]

WHERE v.ID = @ID;
RETURN 0

GO
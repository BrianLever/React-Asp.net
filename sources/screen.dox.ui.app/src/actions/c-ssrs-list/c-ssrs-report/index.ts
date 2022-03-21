import { IBranchLocationItemResponse } from "actions/branch-locations";
import { Action } from "redux";
import {  IActionPayload, ECssrsReportActions, ILocationItemResponse  } from '../../';

export interface ICssrsPatientRecordItem {
    IsEhrSource: boolean;
    ScreeningResultID: number;
    DemographicsID: number;
    EhrPatientId: number;
    NotMatchesFields: Array<string>;
    ID: number;
    BirthdayFormatted: string | null;
    City: string | null;
    Phone: string | null; 
    StateID: string | null;
    StateName: string | null;
    StreetAddress: string | null;
    ZipCode: string | null;
    Birthday: string | null;
    FirstName: string | null;
    LastName: string | null;
    MiddleName: string | null;
    FullName: string | null;
    Age: number | null;
}


export interface ICssrsReportPatientRecordsRequest {
    Birthday: string | null;
    LastName: string | null;
    FirstName: string | null;
    MiddleName: string | null;
}


export interface ICssrsReportPatientRecordsResponse {
    Screendox: {
        Items: Array<ICssrsPatientRecordItem>;
        TotalCount: number;
    },
    Ehr: {
        Items: Array<ICssrsPatientRecordItem>;
        TotalCount: number;
    }
}

export interface ICssrsReportRequest {
    FirstName: string | null;
    LastName: string | null;
    MiddleName: string | null;
    Birthday: string | null;
    City?: string | null;
    Phone?: string | null;
    StateID?: string | null;
    StateName?: string | null;
    StreetAddress?: string | null;
    ZipCode?: string | null;
    EhrPatientID?: string | null;
    EhrPatientHRN?: string |  null;
    BranchLocationID?: number | null;
    ScreeningResultID?: number | null;
    DemographicsID?: number | null;
}

export interface ICssrsEditableReportItem {
    CreatedDateFormatted?: string | null;
    CompleteDateFormatted?: string | null;
    StaffNameCompleted?: string | null;
    FirstName?: string | null;
    LastName?: string | null;
    MiddleName?: string | null;
    Birthday?: string | null;
    FullName?: string | null;
    City?: string | null;
    Phone?: string | null;
    StateID?: string | null;
    StateName?: string | null;
    StreetAddress?: string | null;
    ZipCode?: string | null;
    EhrPatientID?: number | null;
    EhrPatientHRN?: string | null;
    BranchLocationID?: number | null;
    BranchLocationName?: string | null;
    ScreeningResultID?: number | null;
    BhsVisitID?: number | null;
    DemographicsID?: number | null;
    ScoreLevel?: string | null;
    ScoreLevelLabel?: string | null;
    RiskAssessmentReport?: {
        ID?: number | null;
        ActualSuicideAttempt?: boolean;
        LifetimeActualSuicideAttempt?: boolean;
        InterruptedSuicideAttempt?: boolean;
        LifetimeInterruptedSuicideAttempt?: boolean;
        AbortedSuicideAttempt?: boolean;
        LifetimeAbortedSuicideAttempt?: boolean;
        OtherPreparatoryActsToKillSelf?: boolean;
        LifetimeOtherPreparatoryActsToKillSelf?: boolean;
        ActualSelfInjuryBehaviorWithoutSuicideIntent?: boolean;
        LifetimeActualSelfInjuryBehaviorWithoutSuicideIntent?: boolean;
        WishToBeDead?: boolean;
        SuicidalThoughts?: boolean;
        SuicidalThoughtsWithMethod?: boolean;
        SuicidalIntent?: boolean;
        SuicidalIntentWithSpecificPlan?: boolean;
        RecentLoss?: boolean;
        DescribeRecentLoss?: string | null;
        PendingIncarceration?: boolean;
        CurrentOrPendingIsolation?: boolean;
        Hopelessness?: boolean;
        Helplessness?: boolean;
        FeelingTrapped?: boolean;
        MajorDepressiveEpisode?: boolean;
        MixedAffectiveEpisode?: boolean;
        CommandHallucinationsToHurtSelf?: boolean;
        HighlyImpulsiveBehavior?: boolean;
        SubstanceAbuseOrDependence?: boolean;
        AgitationOrSevereAnxiety?: boolean;
        PerceivedBurdenOnFamilyOrOthers?: boolean;
        ChronicPhysicalPain?: boolean;
        HomicidalIdeation?: boolean;
        AggressiveBehaviorTowardsOthers?: boolean;
        MethodForSuicideAvailable?: boolean;
        RefusesOrFeelsUnableToAgreeToSafetyPlan?: boolean;
        SexualAbuseLifetime?: boolean;
        FamilyHistoryOfSuicideLifetime?: boolean;
        PreviousPsychiatricDiagnosesAndTreatments?: boolean;
        HopelessOrDissatisfiedWithTreatment?: boolean;
        NonCompliantWithTreatment?: boolean;
        NotReceivingTreatment?: boolean;
        IdentifiesReasonsForLiving?: boolean;
        ResponsibilityToFamilyOrOthers?: boolean;
        SupportiveSocialNetworkOrFamily?: boolean;
        FearOfDeathOrDyingDueToPainAndSuffering?:boolean;
        BeliefThatSuicideIsImmoral?: boolean;
        EngagedInWorkOrSchool?: boolean;
        EngagedWithPhoneWorker?: boolean;
        OtherProtectiveFactors?: Array<string>;
        OtherRisksFactors?: Array<string>;
        DescribeAnySuicidalSelfInjuriousOrAggressiveBehavior?:string | null;
    },
    WishToDead?: {
        QuestionId?: number | null;
        LifetimeMostSucidal?: boolean | null;
        PastLastMonth?: boolean | null;
        Description?: string | null;
    },
    NonSpecificActiveSuicidalThoughts?: {
        QuestionId?: number | null;
        LifetimeMostSucidal?: boolean | null;
        PastLastMonth?: boolean | null;
        Description?: string | null;
    },
    ActiveSuicidalIdeationWithAnyMethods?: {
        QuestionId?: number | null;
        LifetimeMostSucidal?: boolean | null;
        PastLastMonth?: boolean | null;
        Description?: string | null;
    },
    ActiveSuicidalIdeationWithSomeIntentToAct?: {
        QuestionId?: number | null;
        LifetimeMostSucidal?: boolean | null;
        PastLastMonth?: boolean | null;
        Description?: string | null;
    },
    ActiveSuicidalIdeationWithSpecificPlanAndIntent?: {
        QuestionId?: number | null;
        LifetimeMostSucidal?: boolean | null;
        PastLastMonth?: boolean | null;
        Description?: string | null;
    },
    Frequency?: {
        QuestionId?: number | null;
        LifetimeMostSevere?: number | null;
        RecentMostSevere?: number | null;
    },
    Duration?: {
        QuestionId?: number | null;
        LifetimeMostSevere?: number |null;
        RecentMostSevere?: number | null;
    },
    Controllability?: {
        QuestionId?: number | null;
        LifetimeMostSevere?: number | null;
        RecentMostSevere?: number | null;
    },
    Deterrents?: {
        QuestionId?: number| null;
        LifetimeMostSevere?: number | null;
        RecentMostSevere?: number |  null;
    },
    ReasonsForIdeation?: {
        QuestionId?: number | null;
        LifetimeMostSevere?: number | null;
        RecentMostSevere?: number | null;
    },
    LifetimeMostSevereIdeationLevel?: number | null;
    LifetimeMostSevereIdeationDescription?: string | null;
    RecentMostSevereIdeationLevel?: number | null;
    RecentMostSevereIdeationDescription?: string | null;
    ActualAttempt?: {
        QuestionId?: number | null;
        LifetimeLevel?: boolean | null;
        LifetimeCount?: number | null;
        PastThreeMonths?: boolean | null;
        PastThreeMonthsCount?: number | null;
        Description?: string | null;
    },
    HasSubjectEngagedInNonSuicidalSelfInjuriousBehavior?: {
        QuestionId?: number | null;
        LifetimeLevel?: boolean | null;
        LifetimeCount?: number | null;
        PastThreeMonths?: boolean | null;
        PastThreeMonthsCount?: number | null;
        Description?: string | null;
    },
    InterruptedAttempt?: {
        QuestionId?: number | null;
        LifetimeLevel?: boolean | null;
        LifetimeCount?: number | null;
        PastThreeMonths?: boolean | null;
        PastThreeMonthsCount?: number | null;
        Description?: string | null;
    },
    AbortedAttempt?: {
        QuestionId?: number | null;
        LifetimeLevel?: boolean | null;
        LifetimeCount?: number | null;
        PastThreeMonths?: boolean | null;
        PastThreeMonthsCount?: number | null;
        Description?: string | null;
    },
    PreparatoryActs?: {
        QuestionId?: number | null;
        LifetimeLevel?: boolean | null;
        LifetimeCount?: number | null;
        PastThreeMonths?: boolean | null;
        PastThreeMonthsCount?: number | null;
        Description?: string | null;
    },
    SuicideMostRecentAttemptDate?: string | null;
    SuicideMostLethalRecentAttemptDate?: string | null;
    SuicideFirstAttemptDate?: string | null;
    ActualLethality?: {
        QuestionId?: number | null;
        MostRecentAttemptCode?: number | null;
        MostLethalAttemptCode?: number | null;
        InitialAttemptCode?: number | null;
    },
    PotentialLethality?: {
        QuestionId?: number | null;
        MostRecentAttemptCode?: number | null;
        MostLethalAttemptCode?: number | null;
        InitialAttemptCode?: number | null;
    },
    ID?: number | null;    
    BhsStaffNameCompleted?: string | null;
    CompleteDate?: string | null;
    CreatedDate?: string | null;
}

export const cssrsReportPatientRecordsRequest = ():Action => ({
    type: ECssrsReportActions.cssrsReportPatientRecordsRequest
})

export const resetCssrsReportParameter = ():Action => ({
    type: ECssrsReportActions.resetCssrsReportParameter
})

export const cssrsReportPatientRecordsRequestStart = ():Action => ({
    type: ECssrsReportActions.cssrsReportPatientRecordsRequestStart
})

export const cssrsReportPatientRecordsRequestError = ():Action => ({
    type: ECssrsReportActions.cssrsReportPatientRecordsRequestError
})

export const cssrsReportPatientRecordsRequestSuccess = (payload: { screendox: Array<ICssrsPatientRecordItem>, ehr: Array<ICssrsPatientRecordItem> }):IActionPayload => ({
    type: ECssrsReportActions.cssrsReportPatientRecordsRequestSuccess, payload
})

export const setCssrsReportFirstName = (firstName: string | null): IActionPayload => ({
    type: ECssrsReportActions.setCssrsReportFirstName, payload: { firstName }
})

export const setCssrsReportLastName = (lastName: string | null): IActionPayload => ({
    type: ECssrsReportActions.setCssrsReportLastName, payload: { lastName }
})

export const setCssrsReportMiddleName = (middleName: string | null): IActionPayload => ({
    type: ECssrsReportActions.setCssrsReportMiddleName, payload: { middleName }
})

export const setCssrsReportBirthday = (birthday: string | null): IActionPayload => ({
    type: ECssrsReportActions.setCssrsReportBirthday, payload: { birthday }
})

export const setCssrsReportLocations = (payload: Array<ILocationItemResponse>) => ({
    type: ECssrsReportActions.setCssrsReportLocations, payload
})

export const cssrsReportCreateRequest  = (requestBody: ICssrsReportRequest): IActionPayload => ({
    type: ECssrsReportActions.cssrsReportCreateRequest, payload: { requestBody }
})

export const cssrsReportCreateRequestStart  = (): Action => ({
    type: ECssrsReportActions.cssrsReportCreateRequestStart
})

export const cssrsReportCreateRequestError  = (): Action => ({
    type: ECssrsReportActions.cssrsReportCreateRequestError
})

export const cssrsReportCreateRequestSuccess  = (): Action => ({
    type: ECssrsReportActions.cssrsReportCreateRequestSuccess
})

export const setCssrsReportLocationId = (locationId: number):IActionPayload => ({
    type: ECssrsReportActions.setCssrsReportLocationId, payload: { locationId }
})

export const cssrsReportDetailRequest = (reportId: number):IActionPayload => ({
    type: ECssrsReportActions.cssrsReportDetailRequest, payload: { reportId }
})

export const cssrsReportDetailRequestStart = (): Action => ({
    type: ECssrsReportActions.cssrsReportDetailRequestStart
})

export const cssrsReportDetailRequestError = (): Action => ({
    type: ECssrsReportActions.cssrsReportDetailRequestError
})

export const cssrsReportDetailRequestSuccess = (payload: ICssrsEditableReportItem): IActionPayload => ({
    type: ECssrsReportActions.cssrsReportDetailRequestSuccess, payload
})

export const setCssrsReportEhrExportPatientRecordSelectedId = (ehrExportPatientRecordSelectedId: number): IActionPayload => ({
    type: ECssrsReportActions.setCssrsReportEhrExportPatientRecordSelectedId, payload: { ehrExportPatientRecordSelectedId }
})

export const setCssrsReport = (payload: ICssrsEditableReportItem):IActionPayload => ({
    type: ECssrsReportActions.setCssrsReport, payload,
})

export const cssrsReportUpdateRequest = (id: number): IActionPayload => ({
    type: ECssrsReportActions.cssrsReportUpdateRequest, payload: { id }
})

export const cssrsReportUpdateRequestStart = (): Action => ({
    type: ECssrsReportActions.cssrsReportUpdateRequestStart 
})

export const cssrsReportUpdateRequestError = (): Action => ({
    type: ECssrsReportActions.cssrsReportUpdateRequestError 
})

export const cssrsReportUpdateRequestSuccess = (): Action => ({
    type: ECssrsReportActions.cssrsReportUpdateRequestSuccess 
})

export const cssrsReportCopyRequest  = (id: number): IActionPayload => ({
    type: ECssrsReportActions.cssrsReportCopyRequest, payload: { id }
})

export const cssrsReportCopyRequestStart  = (): Action => ({
    type: ECssrsReportActions.cssrsReportCopyRequestStart
})

export const cssrsReportCopyRequestError  = (): Action => ({
    type: ECssrsReportActions.cssrsReportCopyRequestError
})

export const cssrsReportCopyRequestSuccess  = (): Action => ({
    type: ECssrsReportActions.cssrsReportCopyRequestSuccess
})
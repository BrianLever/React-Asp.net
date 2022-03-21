import { Action } from 'redux';
import { IActionPayload, EFollowUpRelatedReportActions, TChoiceItem } from '../../';

export const postFollowUpsRequest = (id: string): IActionPayload  => ({ 
    type: EFollowUpRelatedReportActions.getReportRequest, payload: { id }, 
});
export const postFollowUpsRequestStart = (): Action  => ({ type: EFollowUpRelatedReportActions.getReportRequestStart });
export const postFollowUpsRequestError = (): Action  => ({ type: EFollowUpRelatedReportActions.getReportRequestSuccess });

export type TFollowUpReportAnswerInnerItem = {
    QuestionID: number;
    AnswerValue: number;
    QuestionText: string;
    AnswerText: string;
}
export type TFollowUpReportActionItem =  {
    Description: string;
    Id: number;
    Name: string;
    OrderIndex: number;
}

export type TFollowUpPatientInfo = {
    City: string;
    Phone: string;
    StateID: string;
    StateName: string;
    StreetAddress: string;
    ZipCode: string;
    Birthday: string;
    FirstName: string;
    LastName: string;
    MiddleName: string;
    FullName: string;
    Age: number;
}

export type TFollowUpSection = {
    ScreeningSectionID: 'DAST' | 'PHQ-9';
    Score: number;
    ScoreLevel: number;
    ScoreLevelLabel: string;
    Indicates: string;
    Answers: Array<TFollowUpReportAnswerInnerItem>
};

export type TFollowUpSectorItem = {
    Id: number;
    Name: string;
    OrderIndex: number;
}

export type TFollowUpSectorDescItem = {
    Description: string;
    Id: number;
    Name: string;
    OrderIndex: number;
};

export type TFollowUpSectorItemA = {
    ScoreLevel: number;
    ScoreLevelLabel: string;
};

export type TFollowUpResult = {
    Sections: Array<TFollowUpSection>,
    ID: number;
    PatientInfo: TFollowUpPatientInfo,
    ExportedToHRN: string;
    VisitID: number;
    CreatedDate: string;
    ExportDate: string;
    ExportedByFullName: string;
    ExportedToPatientID: string;
    ExportedToVisitDate: string;
    ExportedToVisitID: string;
    ExportedToVisitLocation: string;
    IsContactInfoEligableForExport: boolean;
    IsEligible4Export: boolean;
    IsPassedAnySection: boolean;
    KioskID: number;
    LocationID: number;
    LocationLabel: string;
    BhsVisitStatus: string;
    BhsVisitID: string;
    WithErrors: boolean;
    WithErrorsMessage: string;
    PatientDemographicsID: string;
}

export type TFollowUpVisit = {
    OtherScreeningTools: [],
    TreatmentActions: Array<TFollowUpReportActionItem>,
    ScreeningDate: string;
    ScreeningResultID: number;
    Result: string;
    LocationID: number;
    TobacoExposureSmokerInHomeFlag: boolean;
    TobacoExposureCeremonyUseFlag: boolean;
    TobacoExposureSmokingFlag: boolean;
    TobacoExposureSmoklessFlag: boolean;
    AlcoholUseFlag: string;
    SubstanceAbuseFlag: TFollowUpSectorItemA;
    AnxietyFlag: string;
    DepressionFlag:TFollowUpSectorItemA;
    DepressionThinkOfDeathAnswer: string;
    PartnerViolenceFlag: string;
    ProblemGamblingFlag: string;
    NewVisitReferralRecommendation:TFollowUpSectorDescItem;
    NewVisitReferralRecommendationAccepted: TFollowUpSectorItem;
    ReasonNewVisitReferralRecommendationNotAccepted: TFollowUpSectorItem;
    NewVisitDate: string;
    Discharged: TFollowUpSectorItem;
    ThirtyDatyFollowUpFlag: boolean;
    FollowUpDate: string;
    Notes: string;
    IsCompleted: boolean;
    ID: number;
    StaffNameCompleted: string;
    CompleteDate: string;
    CreatedDate: string;
}

export interface IFollowUpReportResponse {
    ScreeningResultID: number;
    BhsVisitID: number;
    ParentFollowUpID: string;
    Result: TFollowUpResult;
    Visit: TFollowUpVisit;
    ScheduledFollowUpDate: string;
    ScheduledVisitDate: string;
    VisitRefferalRecommendation: string;
    PatientAttendedVisit: TFollowUpSectorItem;
    FollowUpContactDate: string;
    FollowUpContactOutcome: TFollowUpSectorItem;
    IsCompleted: boolean;
    Discharged: TFollowUpSectorItem;
    NewVisitDate: string;
    NewVisitReferralRecommendation: TFollowUpSectorDescItem;
    NewVisitReferralRecommendationAccepted: TFollowUpSectorItem;
    ReasonNewVisitReferralRecommendationNotAccepted: TFollowUpSectorItem;
    ThirtyDatyFollowUpFlag: boolean;
    FollowUpDate: string;
    Notes: string;
    ID: number;
    StaffNameCompleted: string;
    CompleteDate: Date;
    CreatedDate: Date;
}

export const postFollowUpsRequestSuccess = (report: IFollowUpReportResponse): IActionPayload  => ({ 
    type: EFollowUpRelatedReportActions.getReportRequestSuccess, payload: { report }, 
});

export const getFollowUpReportPatientAttendedVisitOptionsRequest = (): Action => ({
    type: EFollowUpRelatedReportActions.getFollowUpReportPatientAttendedVisitOptionsRequest
});

export const getFollowUpReportPatientAttendedVisitOptionsRequestStart = (): Action => ({
    type: EFollowUpRelatedReportActions.getFollowUpReportPatientAttendedVisitOptionsRequestStart
});

export const getFollowUpReportPatientAttendedVisitOptionsRequestError = (): Action => ({
    type: EFollowUpRelatedReportActions.getFollowUpReportPatientAttendedVisitOptionsRequestError
});

export const getFollowUpReportPatientAttendedVisitOptionsRequestSuccess = (options: Array<TChoiceItem>): IActionPayload  => ({ 
    type: EFollowUpRelatedReportActions.getFollowUpReportPatientAttendedVisitOptionsRequestSuccess, payload: { options }, 
});


export const setCurrentSelectedFollowUpReportPatientAttendedVisitOption = (option: number): IActionPayload  => ({ 
    type: EFollowUpRelatedReportActions.setCurrentSelectedFollowUpReportPatientAttendedVisitOption, payload: { option }, 
});

export const setCurrentSelectedFollowUpReportPatientAttendedVisitDate = (date: string): IActionPayload  => ({ 
    type: EFollowUpRelatedReportActions.setCurrentSelectedFollowUpReportPatientAttendedVisitDate, payload: { date }, 
});

export const getFollowUpReportFollowUpContactOutcomeRequest = (): Action => ({
    type: EFollowUpRelatedReportActions.getFollowUpReportFollowUpContactOutcome,
});

export const getFollowUpReportFollowUpContactOutcomeRequestStart = (): Action => ({
    type: EFollowUpRelatedReportActions.getFollowUpReportFollowUpContactOutcomeStart,
});

export const getFollowUpReportFollowUpContactOutcomeRequestError = (): Action => ({
    type: EFollowUpRelatedReportActions.getFollowUpReportFollowUpContactOutcomeError,
});

export const getFollowUpReportFollowUpContactOutcomeRequestSuccess = (options: Array<TChoiceItem>): IActionPayload  => ({ 
    type: EFollowUpRelatedReportActions.getFollowUpReportFollowUpContactOutcomeSuccess, payload: { options }, 
});

export const getFollowUpReportNewVisitReferralRecommendationRequest = (): Action => ({
    type: EFollowUpRelatedReportActions.getFollowUpReportNewVisitReferralRecommendation,
});

export const getFollowUpReportNewVisitReferralRecommendationRequestStart = (): Action => ({
    type: EFollowUpRelatedReportActions.getFollowUpReportNewVisitReferralRecommendationStart,
});

export const getFollowUpReportNewVisitReferralRecommendationRequestError = (): Action => ({
    type: EFollowUpRelatedReportActions.getFollowUpReportNewVisitReferralRecommendationError,
});

export const getFollowUpReportNewVisitReferralRecommendationRequestSuccess = (options: Array<TChoiceItem>): IActionPayload  => ({ 
    type: EFollowUpRelatedReportActions.getFollowUpReportNewVisitReferralRecommendationSuccess, payload: { options }, 
});

export const setFollowUpReportSelectedContactOutcomeOption = (option: number): IActionPayload  => ({ 
    type: EFollowUpRelatedReportActions.setFollowUpReportContactOutcomeOption, payload: { option }, 
});

export const setFollowUpReportSelectedNewVisitReferralRecommendationOption= (option: number): IActionPayload  => ({ 
    type: EFollowUpRelatedReportActions.setFollowUpReportNewVisitReferralRecommendationOption, payload: { option }, 
});

export const setNewVisitReferralRecommendation = (value: string): IActionPayload  => ({
    type: EFollowUpRelatedReportActions.setNewVisitReferralRecommendation, payload: { value }
})


export const getFollowUpReporNewVisitReferralRecommendationAcceptedRequest = (): Action => ({
    type: EFollowUpRelatedReportActions.getFollowUpReportNewVisitReferralRecommendationAcceptedRequest
});

export const getFollowUpReporNewVisitReferralRecommendationAcceptedRequestStart = (): Action => ({
    type: EFollowUpRelatedReportActions.getFollowUpReportNewVisitReferralRecommendationAcceptedRequestStart
});

export const getFollowUpReporNewVisitReferralRecommendationAcceptedRequestError = (): Action => ({
    type: EFollowUpRelatedReportActions.getFollowUpReportNewVisitReferralRecommendationAcceptedRequestError
});

export const getFollowUpReporNewVisitReferralRecommendationAcceptedRequestSuccess = (options: Array<TChoiceItem>): IActionPayload => ({
    type: EFollowUpRelatedReportActions.getFollowUpReportNewVisitReferralRecommendationAcceptedRequestSuccess,
    payload: { options }
});

export const setFollowUpReporNewVisitReferralRecommendationAcceptedOption = (option: number): IActionPayload => ({
    type: EFollowUpRelatedReportActions.setFollowUpReportNewVisitReferralRecommendationAcceptedOption, payload: { option }
});

export const getFollowUpReponewVisitReferralRecommendationNotAcceptedRequest = (): Action => ({
    type: EFollowUpRelatedReportActions.getFollowUpReportNewVisitReferralRecommendationNotAcceptedRequest
});

export const getFollowUpReponewVisitReferralRecommendationNotAcceptedRequestStart = (): Action => ({
    type: EFollowUpRelatedReportActions.getFollowUpReportNewVisitReferralRecommendationNotAcceptedRequestStart
});

export const getFollowUpReponewVisitReferralRecommendationNotAcceptedRequestError = (): Action => ({
    type: EFollowUpRelatedReportActions.getFollowUpReportNewVisitReferralRecommendationNotAcceptedRequestError
});

export const getFollowUpReponewVisitReferralRecommendationNotAcceptedRequestSuccess = (options: Array<TChoiceItem>): IActionPayload => ({
    type: EFollowUpRelatedReportActions.getFollowUpReportNewVisitReferralRecommendationNotAcceptedRequestSuccess,
    payload: { options }
});

export const setFollowUpReponewVisitReferralRecommendationNotAcceptedOption = (option: number): IActionPayload => ({
    type: EFollowUpRelatedReportActions.setFollowUpReportNewVisitReferralRecommendationNotAcceptedOption, payload: { option }
});


export const setFollowUpReportCreateFlag = (value: number): IActionPayload => ({
    type: EFollowUpRelatedReportActions.setFollowUpReportCreateFlag, payload: { value }
});

export const setFollowUpReportCreateDate = (date: string): IActionPayload => ({
    type: EFollowUpRelatedReportActions.setFollowUpReportCreateDate, payload: { date }
});

export const setFollowUpReportNewVisitDate = (date: string): IActionPayload => ({
    type: EFollowUpRelatedReportActions.setFollowUpReportNewVisitDate, payload: { date }
});

export const getFollowUpReportDischargedRequest = (): Action => ({
    type: EFollowUpRelatedReportActions.getFollowUpReportDischargedRequest
});

export const getFollowUpReportDischargedRequestStart = (): Action => ({
    type: EFollowUpRelatedReportActions.getFollowUpReportDischargedRequestStart
});

export const getFollowUpReportDischargedRequestError = (): Action => ({
    type: EFollowUpRelatedReportActions.getFollowUpReportDischargedRequestError
});

export const getFollowUpReportDischargedRequestSuccess = (options: Array<TChoiceItem>): IActionPayload => ({
    type: EFollowUpRelatedReportActions.getFollowUpReportDischargedRequestSuccess, payload: { options }
});

export const setFollowUpReportDischargedOption = (option: number): IActionPayload => ({
    type: EFollowUpRelatedReportActions.setFollowUpReportDischargedOption, payload: { option }
});

export const setFollowUpCreate = (value: boolean): IActionPayload => ({
    type: EFollowUpRelatedReportActions.setFollowUpCreate, payload: { value }
});
export const setFollowUpDate = (value: string): IActionPayload => ({
    type: EFollowUpRelatedReportActions.setFollowUpDate, payload: { value }
});
export const setFollowUpNote = (value: string): IActionPayload => ({
    type: EFollowUpRelatedReportActions.setFollowUpNote, payload: { value }
});

export const updateFollowUpReportRequest = (id: string): IActionPayload => ({
    type: EFollowUpRelatedReportActions.updateFollowUpReportRequest, payload: { id }
});

export const updateFollowUpReportRequestStart = (): Action => ({
    type: EFollowUpRelatedReportActions.updateFollowUpReportRequestStart
});

export const updateFollowUpReportRequestError = (): Action => ({
    type: EFollowUpRelatedReportActions.updateFollowUpReportRequestError
});

export const updateFollowUpReportRequestSuccess = (): Action => ({
    type: EFollowUpRelatedReportActions.updateFollowUpReportRequestSuccess
});

export const setVisitReportIsCompleted = (value: boolean): IActionPayload => ({
    type: EFollowUpRelatedReportActions.setFollowUpReportIsCompleted, payload: { value }
})
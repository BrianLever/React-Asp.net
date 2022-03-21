import { IFollowUpInnerInnerItem } from 'actions/follow-up';
import { Action } from 'redux';
import { EVisitReportActions, IActionPayload } from '../../';

export type TVisitAnswer = {
    QuestionID: number;
    AnswerValue: number;
    QuestionText: string;
    AnswerText: string;
}

export type TVisitSections = {
    ScreeningSectionID: string;
    Score: string;
    ScoreLevel: string;
    ScoreLevelLabel: string;
    Indicates: string;
    Answers: Array<TVisitAnswer>;
}

export type TVisitPatientInfo = {
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

export type TOtherScreeningToolsItem = {
    ScoreOrResult: string;
    ToolName: string;
}

export type TTreatmentActionsItem = {
    Id: number;
    Name: string;
    OrderIndex: number;
    Description?: string;
}

export type TReferalRecomendationItem = {
    Id: number;
    Name: string;
    OrderIndex: number;
}

export interface IVisitReportResponse {
    ID: number;
    Notes: string;
    LocationID: number;
    CompleteDate: string;
    IsCompleted: boolean;
    CreatedDate: string;
    NewVisitDate: string;
    FollowUpDate: string;
    ScreeningDate: string;
    TreatmentActions: Array<TTreatmentActionsItem>;
    ScreeningResultID: number;
    StaffNameCompleted: string;
    OtherScreeningTools: Array<TOtherScreeningToolsItem>;
    ThirtyDatyFollowUpFlag: boolean;
    TobacoExposureSmoklessFlag: boolean;
    TobacoExposureSmokingFlag: boolean;
    DepressionThinkOfDeathAnswer: string;
    TobacoExposureCeremonyUseFlag: boolean;
    TobacoExposureSmokerInHomeFlag: boolean;
    Result: {
        ID: number;
        KioskID: number;
        VisitID: number;
        Sections: Array<TVisitSections>;
        ExportDate: string;
        LocationID: number;
        BhsVisitID: string;
        WithErrors: boolean;
        CreatedDate: string;
        PatientInfo: TVisitPatientInfo;
        ExportedToHRN: string;
        LocationLabel: string;
        BhsVisitStatus: string;
        WithErrorsMessage: string;
        ExportedToVisitID: string;
        IsEligible4Export: boolean;
        ExportedByFullName: string;
        IsPassedAnySection: boolean;
        ExportedToVisitDate: string;
        ExportedToPatientID: string;
        PatientDemographicsID: number;
        ExportedToVisitLocation: string;
        IsContactInfoEligableForExport: boolean;
    },
    AlcoholUseFlag: {
        ScoreLevel: number;
        ScoreLevelLabel: string;
    },
    AnxietyFlag: {
        ScoreLevel: number;
        ScoreLevelLabel: string;
    },
    DepressionFlag: {
        ScoreLevel: number;
        ScoreLevelLabel: string;
    },
    PartnerViolenceFlag: {
        ScoreLevel: number;
        ScoreLevelLabel: string;
    },
    SubstanceAbuseFlag: {
        ScoreLevel: number;
        ScoreLevelLabel: string;
    }
    ProblemGamblingFlag: {
        ScoreLevel: number;
        ScoreLevelLabel: string;
    }
    NewVisitReferralRecommendation: {
        Id: number;
        Name: string;
        OrderIndex: number;
        Description: string;
    },
    NewVisitReferralRecommendationAccepted: {
        Id: number;
        Name: string;
        OrderIndex: number;
    },
    ReasonNewVisitReferralRecommendationNotAccepted: {
        Id: number;
        Name: string;
        OrderIndex: number;
    },
    Discharged: {
        Id: number;
        Name: string;
        OrderIndex: number;
    }
}

export const getVisitReportRequest = (reportId: number): IActionPayload => ({
    type: EVisitReportActions.getVisitReportByIdRequest,
    payload: { reportId }
});

export const getVisitReportRequestStart = (): Action => ({
    type: EVisitReportActions.getVisitReportByIdStart
});

export const getVisitReportRequestError = (): Action => ({
    type: EVisitReportActions.getVisitReportByIdError
});

export const getVisitReportRequestSuccess = (report: IVisitReportResponse | null): IActionPayload => ({
    type: EVisitReportActions.getVisitReportByIdSuccess,
    payload: { report }
});


export type TChoiceItem = {
    Id: number;
    Name: string;
    OrderIndex: number;
    Description?: string;
}

export const getVisitDrugChoiceRequest = (): Action => ({
    type: EVisitReportActions.getDrugChoiceOptionRequest,
});

export const getVisitDrugChoiceRequestStart = (): Action => ({
    type: EVisitReportActions.getDrugChoiceOptionRequestStart,
});

export const getVisitDrugChoiceRequestError = (): Action => ({
    type: EVisitReportActions.getDrugChoiceOptionRequestError,
});

export const getVisitDrugChoiceRequestSuccess = (options: Array<TChoiceItem>)
: IActionPayload => ({
    type: EVisitReportActions.getDrugChoiceOptionRequestSuccess,
    payload: { options }
});

export const initVisitReportOtherScreeningToolsItem = (otherScreeningTools: Array<TOtherScreeningToolsItem>)
: IActionPayload => ({
    type: EVisitReportActions.initOtherScreeningTools,
    payload: { otherScreeningTools }
});

export const initVisitReportTritmentActionToolsItem = (tritmentActionTools: Array<TTreatmentActionsItem>)
: IActionPayload => ({
    type: EVisitReportActions.initTritmentActionTools,
    payload: { tritmentActionTools }
});

export const getVisitTritmentOptionsRequest = (): Action => ({
    type: EVisitReportActions.getTritmentActionRequest,
});

export const getVisitTritmentOptionsRequestStart = (): Action => ({
    type: EVisitReportActions.getTritmentActionRequestStart,
});

export const getVisitTritmentOptionsRequestError = (): Action => ({
    type: EVisitReportActions.getTritmentActionRequestError,
});

export const getVisitTritmentOptionsRequestSuccess = (options: Array<TTreatmentActionsItem>)
: IActionPayload => ({
    type: EVisitReportActions.getTritmentActionRequestSuccess,
    payload: { options }
});


export const getVisitNewReferalRecomendationRequest = (): Action => ({
    type: EVisitReportActions.getVisitNewReferalRecomendationRequest,
});

export const getVisitNewReferalRecomendationRequestStart = (): Action => ({
    type: EVisitReportActions.getVisitNewReferalRecomendationRequestStart,
});

export const getVisitNewReferalRecomendationRequestError = (): Action => ({
    type: EVisitReportActions.getVisitNewReferalRecomendationRequestError,
});

export const getVisitNewReferalRecomendationRequestSuccess = (options: Array<TReferalRecomendationItem>)
: IActionPayload => ({
    type: EVisitReportActions.getVisitNewReferalRecomendationRequestSuccess,
    payload: { options }
});

export const getVisitNewReferalRecomendationAcceptedRequest = (): Action => ({
    type: EVisitReportActions.getVisitNewReferalRecomendationAcceptedRequest,
});

export const getVisitNewReferalRecomendationAcceptedRequestStart = (): Action => ({
    type: EVisitReportActions.getVisitNewReferalRecomendationAcceptedRequestStart,
});

export const getVisitNewReferalRecomendationAcceptedRequestError = (): Action => ({
    type: EVisitReportActions.getVisitNewReferalRecomendationAcceptedRequestError,
});

export const getVisitNewReferalRecomendationAcceptedRequestSuccess = (options: Array<TReferalRecomendationItem>)
: IActionPayload => ({
    type: EVisitReportActions.getVisitNewReferalRecomendationAcceptedRequestSuccess,
    payload: { options }
});

export const getVisitNewReferalRecomendationNotAcceptedRequest = (): Action => ({
    type: EVisitReportActions.getVisitNewReferalRecomendationNotAcceptedRequest,
});

export const getVisitNewReferalRecomendationNotAcceptedRequestStart = (): Action => ({
    type: EVisitReportActions.getVisitNewReferalRecomendationNotAcceptedRequestStart,
});

export const getVisitNewReferalRecomendationNotAcceptedRequestError = (): Action => ({
    type: EVisitReportActions.getVisitNewReferalRecomendationNotAcceptedRequestError,
});

export const getVisitNewReferalRecomendationNotAcceptedRequestSuccess = (options: Array<TReferalRecomendationItem>)
: IActionPayload => ({
    type: EVisitReportActions.getVisitNewReferalRecomendationNotAcceptedRequestSuccess,
    payload: { options }
});

export const getVisitDischargedRequest = (): Action => ({
    type: EVisitReportActions.getVisitDischargedOptionsRequest,
});

export const getVisitDischargedRequestStart = (): Action => ({
    type: EVisitReportActions.getVisitDischargedOptionsRequestStart,
});

export const getVisitDischargedRequestError = (): Action => ({
    type: EVisitReportActions.getVisitDischargedOptionsRequestError,
});

export const getVisitDischargedRequestSuccess = (options: Array<TReferalRecomendationItem>)
: IActionPayload => ({
    type: EVisitReportActions.getVisitDischargedOptionsRequestSuccess,
    payload: { options }
});

export const setVisitDrugPrimaryItem = (option: TChoiceItem)
: IActionPayload => ({
    type: EVisitReportActions.setDrugPrimaryItem,
    payload: { option }
});

export const setVisitDrugSecondaryItem = (option: TChoiceItem)
: IActionPayload => ({
    type: EVisitReportActions.setDrugSecondaryItem,
    payload: { option }
});

export const setVisitDrugTertiaryItem = (option: TChoiceItem)
: IActionPayload => ({
    type: EVisitReportActions.setDrugTertiaryItem,
    payload: { option }
});

export const updateVisitReportRequest = (reportId: string): IActionPayload => ({
    type: EVisitReportActions.updateVisitReportRequest,
    payload: { reportId }
});

export const updateVisitReportRequestStart = (): Action => ({
    type: EVisitReportActions.updateVisitReportRequestStart,
});

export const updateVisitReportRequestError = (): Action => ({
    type: EVisitReportActions.updateVisitReportRequestError,
});

export const updateVisitReportRequestSuccess = (): Action => ({
    type: EVisitReportActions.updateVisitReportRequestSuccess,
});

export const setVisitReferralRecommendationDescription = (value: string)
: IActionPayload => ({
    type: EVisitReportActions.setVisitReferralRecommendationDescription,
    payload: { value }
});

export const setVisitReferralRecommendationId = (value: number)
: IActionPayload => ({
    type: EVisitReportActions.setVisitReferralRecommendationId,
    payload: { value }
});

export const setVisitReferralRecommendationAccepted = (value: number)
: IActionPayload => ({
    type: EVisitReportActions.setVisitReferralRecommendationAccepted,
    payload: { value }
});

export const setVisitReferralRecommendationNotAccepted = (value: number)
: IActionPayload => ({
    type: EVisitReportActions.setVisitReferralRecommendationNotAccepted,
    payload: { value }
});

export const setVisitDischarged = (value: number): IActionPayload => 
({ type: EVisitReportActions.setVisitDischarged, payload: { value }});

export const setVisitDate = (value: string): IActionPayload => 
({ type: EVisitReportActions.setVisitDate, payload: { value }});

export const setVisitNotes = (value: string): IActionPayload => 
({ type: EVisitReportActions.setVisitNotes, payload: { value }});

export const setVisitFollowUp = (value: boolean): IActionPayload => 
({ type: EVisitReportActions.setVisitFollowUp, payload: { value }});

export const setVisitFollowUpDate = (value: string): IActionPayload => 
({ type: EVisitReportActions.setVisitFollowUpDate, payload: { value }});


export const setVisitReportIsCompleted = (isCompleted: boolean): IActionPayload => 
({ type: EVisitReportActions.setVisitReportIsCompleted, payload: { isCompleted }});

export const setFollowUPVisitArray = (payload: Array<IFollowUpInnerInnerItem>): IActionPayload => 
({ type: EVisitReportActions.setFollowUpVisitArray, payload })

export const getFollowUPVisitArrayRequest = (reportId: number) : IActionPayload =>
({ type: EVisitReportActions.getFollowUPVisitArrayRequest, payload: { reportId }})





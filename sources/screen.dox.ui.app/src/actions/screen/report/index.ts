import { Action } from 'redux';
import * as H from 'history';
import { IActionPayload, EScreenReportActions } from '../..';

export const getScreenReportDefinition = (): Action => ({
    type: EScreenReportActions.getScreenReportDefinitionRequest,
});

export const getScreenReportDefinitionStart = (): Action => ({
    type: EScreenReportActions.getScreenReportDefinitionStart,
});

export const getScreenReportDefinitionError = (): Action => ({
    type: EScreenReportActions.getScreenReportDefinitionStart,
});

export type TAnswerOptions = {
    AnswerScaleOptionID: number;
    AnswerScaleID: number;
    Text: string;
    Value: number;
}

export interface IMainSectionQuestions {
    QuestionID: number;
    ScreeningSectionID: string;
    PreambleText: string;
    QuestionText: string;
    AnswerScaleID: number;
    IsMainQuestion: boolean;
    ShowOnlyWhenPossitiveScore: boolean;
    IndexOrder: number;
    AnswerOptions: Array<TAnswerOptions>;
}

export interface IQuestions {
    QuestionID: number;
    ScreeningSectionID: string;
    PreambleText: string;
    QuestionText: string;
    AnswerScaleID: number;
    IsMainQuestion: boolean;
    ShowOnlyWhenPossitiveScore: boolean;
    IndexOrder: number;
    AnswerOptions: Array<TAnswerOptions>;
}

export interface IDefenitionResponse {
    Questions: Array<IQuestions>,
    ScreeningSectionID: string,
    ScreeningID: string,
    ScreeningSectionName: string,
    ScreeningSectionShortName: string,
    MainSectionQuestions: Array<IMainSectionQuestions>;
    NotMainSectionQuestions: Array<IMainSectionQuestions>;
}

export const getScreenReportDefinitionSuccess = (defenition: IDefenitionResponse): IActionPayload => ({
    type: EScreenReportActions.getScreenReportDefinitionSuccess,
    payload: { defenition }
});

export const getScreeningReportRequest = (reportId: number): IActionPayload => {
    return ({
        type: EScreenReportActions.getScreeningReportRequest, payload: { reportId } 
    })
}


export interface IScreeningReportPatientInfo {
    City: string | null;
    Phone: string | null;
    StateID: string | null;
    StateName: string | null;
    StreetAddress: string | null;
    ZipCode: string | null;
    Birthday: string | null;
    FirstName:string | null;
    LastName: string | null;
    MiddleName: string | null;
    FullName: string | null;
    Age: number | null;
}

export interface IScreeningReportSections {
    ScreeningSectionID:string;
    Score: number;
    ScoreLevel: number;
    ScoreLevelLabel: string;
    ScreeningSectionName: string;
    ScreeningSectionShortName: string;
    Indicates: string;
    Answers: Array<{
    QuestionID: number;
    AnswerValue: number;
    QuestionText: string;
    AnswerText:  string;
   }>
}

export interface IScreeningReport {
    Sections: Array<IScreeningReportSections>;
    ID: number;
    PatientInfo: IScreeningReportPatientInfo;
    ExportedToHRN: string;
    VisitID: number;
    CreatedDate: string;
    CreatedDateLabel: string;
    CreatedByFullName: string;
    ExportDate:string;
    ExportDateLabel:string;
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
    BhsVisitID: number;
    WithErrors: boolean;
    WithErrorsMessage: string;
    PatientDemographicsID: number;
}


export const getScreeningReportRequestSuccess = (payload: IScreeningReport): IActionPayload => {
    return ({
        type: EScreenReportActions.getScreeningReportRequestSuccess, payload
    })
}

export const getScreeningReportRequestStart = (): Action => ({
    type: EScreenReportActions.getScreeningReportRequestStart
})

export const resetScreeningReportRequest = (): Action => ({
    type: EScreenReportActions.resetScreeningReportRequest
})

export const getScreeningReportRequestError = (): Action => ({
    type: EScreenReportActions.getScreeningReportRequestError
})

export const setScreeningReportObejct = (obj: { [k: string]: IScreeningReportSections }): IActionPayload => {
    return ({
        type: EScreenReportActions.setScreeningReportSectionsObjet, payload: { obj }
    })
}

export const deleteScreeningReportRequest = (id: number, history: H.History): IActionPayload => ({
    type: EScreenReportActions.deleteScreeningReportRequest,
    payload: { id, history }
});

export const deleteScreeningReportRequestStart = (): Action => ({
    type: EScreenReportActions.deleteScreeningReportRequestStart
});

export const deleteScreeningReportRequestError = (): Action => ({
    type: EScreenReportActions.deleteScreeningReportRequestError
});

export const deleteScreeningReportRequestSuccess = (): Action => ({
    type: EScreenReportActions.deleteScreeningReportRequestSuccess
});

export const getScreenReportVisitRequest = (reportId: number): IActionPayload => ({
    type: EScreenReportActions.getScreenReportVisitId, payload: { reportId } 
});

export const getScreenReportVisitRequestSuccess = (payload: IScreeningReport): IActionPayload => ({
    type: EScreenReportActions.getScreenReportVisitIdRequestSuccess, payload
});


export const getScreenReportVisitRequestStart = (): Action => ({
    type: EScreenReportActions.getScreenReportVisitIdRequestStart
});

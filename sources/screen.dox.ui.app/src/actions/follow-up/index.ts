import { Action } from 'redux';
import { IActionPayload, EFollowUpActions, EBranchLocationsActions } from '../';
import { TGPRAPeriodResponseItem, ILocationItemResponse } from '../../actions';

export const postFilteredFollowUpsRequest = ():Action  => ({ type: EFollowUpActions.postAllFollowUpRequest });
export const postFilteredFollowUpsRequestStart = ():Action  => ({ type: EFollowUpActions.postAllFollowUpRequestStart });
export const postFilteredFollowUpsRequestError = ():Action  => ({ type: EFollowUpActions.postAllFollowUpRequestError });

export interface IFollowUpRequest {
    Location?: number | null;
    StartDate?: string | null;
    EndDate?: string | null;
    FirstName?: string | null;
    LastName?: string | null;
    ScreeningResultID?: number | null;
    OrderBy?: string;
    StartRowInde?: number;
    MaximumRows?: number;
    ReportType?: number;
    StartRowIndex?: number;
}

export interface IFollowUpResponseItem   {
    PatientName: string;
    ScreeningResultID: number;
    Birthday: string;
    LastVisitDate: string;
    LastVisitDateLabel: string;
    LastFollowUpDate: string;
    LastFollowUpDateLabel: string;  
    LastCompleteDate: string;
    LastCompleteDateLabel: string;
}

export const postFilteredFollowUpsRequestSuccess = (response: {
    Items: Array<IFollowUpResponseItem>;
    TotalCount: number;
    Offset: number;
    CurrentPage: number;
})
:IActionPayload  => ({ 
    type: EFollowUpActions.postAllFollowUpRequestSuccess,
    payload: { response } 
});

export const postFollowUpListItemRequest = (ScreeningResultID: number): IActionPayload => ({
    type: EFollowUpActions.getInternalFollowUpListItemDataRequest,
    payload: { ScreeningResultID }
});

export const postFollowUpListItemRequestStart = (): Action => ({
    type: EFollowUpActions.getInternalFollowUpListItemDataRequestStart,
});

export const postFollowUpListItemRequestError = (): Action => ({
    type: EFollowUpActions.getInternalFollowUpListItemDataRequestError,
});

export interface IFollowUpInnerInnerItem {
    ID: number;
    ScreeningResultID: number;
    CreatedDate: string;
    VisitDate: string;
    CompletedDate: string;
    FollowUpDate: string;
    CompleteDateLabel: string;
    ScheduledVisitDateLabel: string;
    FollowUpDateLabel: string;
    DetailsPageUrl: string;
    ReportName: string;
    Status: string;
}

export const postFollowUpListItemRequestSuccess = (
    payload: { 
        Items: IFollowUpInnerInnerItem[],  
        ScreeningResultID: number 
    }
): IActionPayload => {
    return ({
        type: EFollowUpActions.getInternalFollowUpListItemDataRequestSuccess, payload 
    })
} 

export const changeFollowUpActiveSortObject = (key: string, direction: string): IActionPayload => {
    return ({
        type: EFollowUpActions.changeActiveFollowUpSort, payload: { key, direction } 
    })
}


export const setCurrentFollowUpOffset = (offset: number): IActionPayload => {
    return ({
        type: EFollowUpActions.setCurrentFollowUpOffset, payload: { offset } 
    })
}

export const setCurrentFollowUpPage = (page: number): IActionPayload => {
    return ({
        type: EFollowUpActions.setCurrentFollowUpPage, payload: { page } 
    })
}

export const changeFollowUpActiveSortKey = (key: string): IActionPayload => {
    return ({
        type: EFollowUpActions.changeFollowUpActiveKey, payload: { key } 
    })
}

export const changeFollowUpActiveSortDirection = (key: string, direction: string): IActionPayload => {
    return ({
        type: EFollowUpActions.changeFollowUpActiveDirection, payload: { key, direction } 
    })
}

export const postFollowUpPageChangeRequest = (page: number): IActionPayload => {
    return ({
        type: EFollowUpActions.setFollowUpPageChangeRequest, payload: { page }
    })
} 

export const setFollowUpFirstName = (firstName: string): IActionPayload => ({
    type: EFollowUpActions.setFollowUpFirstName, payload: { firstName }
});

export const setFollowUpLastName = (lastName: string): IActionPayload => ({
    type: EFollowUpActions.setFollowUpLastName, payload: { lastName }
});

export const setFollowUpScreeningID = (screendoxResultId: number): IActionPayload => ({
    type: EFollowUpActions.setFollowUpScreendoxResultId, payload: { screendoxResultId }
});

export const setFollowUpLocationsId = (id: number): IActionPayload => ({ 
    type: EFollowUpActions.setFollowUpLocationId, payload: { id }
});


export const setFollowUpCurrentStartDate = (startDate: string | null): IActionPayload => {
    return ({
        type: EFollowUpActions.setFollowUpScreendoxStartDate, payload: { startDate } 
    })
}

export const setFollowUpCurrentEndDate = (endDate: string | null): IActionPayload => {
    return ({
        type: EFollowUpActions.setFollowUpScreendoxEndDate, payload: { endDate } 
    })
}

export const setFollowUpCurrentGPRAPeriodRangeChange = (periodKey: string): IActionPayload => {
    return ({
        type: EFollowUpActions.setFollowUpScreendoxGRPAPeriod, payload: { periodKey } 
    })
}

export const setFollowUpBSRReportType = (reportType: number): IActionPayload => {
    return ({
        type: EFollowUpActions.setFollowUpBSRReportType, payload: { reportType } 
    })
}

export const resetFollowUpSearchParameters = (): Action => ({ type: EFollowUpActions.resetFollowUpSearchParameters });
export const resetFollowUpSearchParametersRequest = (): Action => ({ type: EFollowUpActions.resetFollowUpSearchParametersRequest });

export const getFollowUpRelatedReportRequest = (id: number): IActionPayload => ({
    type: EFollowUpActions.getRelatedReportByIdRequest, payload: { id } 
});

export interface IFollowUpRelatedReportResponse {
    ID: number;
    Status: string;
    VisitDate: string;
    ReportName: string;
    CreatedDate: string;
    FollowUpDate: string;
    CompletedDate: string;
    DetailsPageUrl: string;
    CompleteDateLabel: string;
    FollowUpDateLabel: string;
    ScreeningResultID: number;
    ScheduledVisitDateLabel: string;
}
export const getFollowUpRelatedReportRequestStart = (): Action => ({ type: EFollowUpActions.getRelatedReportByIdRequestStart });
export const getFollowUpRelatedReportRequestError = (): Action => ({ type: EFollowUpActions.getRelatedReportByIdRequestError });
export const getFollowUpRelatedReportRequestSuccess = (report: Array<IFollowUpRelatedReportResponse>, id: number)
: IActionPayload => ({
    type: EFollowUpActions.getRelatedReportByIdRequestSuccess, payload: { report, id } 
});

export const getLocationListAction = (): Action => ({
    type: EFollowUpActions.getLocationListRequest,
});

export const getLocationListStartAction = (): Action => ({
    type: EFollowUpActions.getLocationListRequestStart,
});

export const getLocationListErrorAction = (): Action => ({
    type: EFollowUpActions.getLocationListRequestError,
});

export const getLocationListSuccessAction = (locations: Array<ILocationItemResponse>): IActionPayload => ({
    type: EFollowUpActions.getLocationListRequestSuccess,
    payload: { locations }
});

export const changeAutoUpdateStatus = (): Action => ({
    type: EFollowUpActions.changeAutoUpdateStatus,
});

export const changeAutoUpdateStatusRequest = (): Action => ({
    type: EFollowUpActions.changeAutoUpdateStatusRequest
})
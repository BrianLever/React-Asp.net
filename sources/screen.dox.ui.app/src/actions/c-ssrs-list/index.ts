import { Action } from "redux";
import {  ECssrsListActions, IActionPayload, ILocationItemResponse } from '../';


export interface ICssrsListRequest {
    Location?: number | null;
    StartDate?: string | null;
    EndDate?: string | null;
    FirstName?: string | null;
    LastName?: string | null;
    DateofBirth?:string | null;
    ScreeningResultID?: number | null;
    OrderBy?: string;
    MaximumRows?: number;
    ReportType?: number;
    StartRowIndex?: number;
}
export interface ICssrsListResponse {
    PatientName: string;
    ID: number;
    Birthday: string;
    LastCreatedDate: string;
    LastCreatedDateLabel: string;
    LastCompleteDate: string;
    LastCompleteDateLabel: string;
    LocationName: string;
    CompletedByName: string
}

export interface ICssrsVisitResponseItem {
    ID: number;
    ScreeningResultID: number;
    CreatedDate: string;
    CompletedDate: string;
    HasAddress: boolean;
    LocationName: string;
    StaffNameCompleted: string;
    ReportName: string;
    CreatedDateLabel: string;
    CompleteDateLabel: string;
}

export const getCssrsListRequest = (): Action => ({
    type: ECssrsListActions.getCssrsListRequest
})

export const getCssrsListRequestStart = (): Action => ({
    type: ECssrsListActions.getCssrsListRequestStart
})
export const getCssrsListRequestError = (): Action => ({
    type: ECssrsListActions.getCssrsListRequestError
})
export const getCssrsListRequestSuccess = (payload: { Items: Array<ICssrsListResponse>, TotalCount: number }): IActionPayload => ({
    type: ECssrsListActions.getCssrsListRequestSuccess, payload
})

export const cssrsListSortRequest = (key: string, direction: string): IActionPayload => ({
    type: ECssrsListActions.cssrsListSortRequest, payload: { key, direction }
});

export const setCssrsListCurrentPage = (currentPage: number): IActionPayload => ({
    type: ECssrsListActions.setCssrsListCurrentPage, payload: { currentPage }
})
export const setCssrsListOffset = (offset: number): IActionPayload => ({
    type: ECssrsListActions.setCssrsListOffset, payload: { offset }
});

export const setCssrsListSortKey = (sortKey: string) : IActionPayload => ({
    type: ECssrsListActions.setCssrsListSortKey, payload: { sortKey }
})

export const setCssrsListSortDirection = (sortDirection: string) :IActionPayload => ({
    type: ECssrsListActions.setCssrsListSortDirection, payload: { sortDirection }
})

export const setCssrsListFirstName = (firstName: string): IActionPayload => ({
    type: ECssrsListActions.setCssrsListFirstName, payload: { firstName }
})

export const setCssrsListLastName = (lastName: string): IActionPayload => ({
    type: ECssrsListActions.setCssrsListLastName, payload: { lastName }
})

export const setCssrsListDateofBirth = (dateofBirth: string | null): IActionPayload => ({
    type: ECssrsListActions.setCssrsListDateofBirth, payload: { dateofBirth }
})

export const setCssrsListLocationId = (locationId: number): IActionPayload => ({
    type: ECssrsListActions.setCssrsListLocationId, payload: { locationId }
})

export const setCssrsListScreeningResultId = (screeningResultId: string): IActionPayload => ({
    type: ECssrsListActions.setCssrsListScreeningResultId, payload: { screeningResultId }
})

export const setCssrsListStartDate = (startDate: string | null): IActionPayload => ({
    type: ECssrsListActions.setCssrsListStartDate, payload: { startDate }
})

export const setCssrsListEndDate = (endDate: string | null): IActionPayload => ({
    type: ECssrsListActions.setCssrsListEndDate, payload: { endDate }
})

export const setCssrsListGpraPeriodKey = (grpaPeriodKey: string): IActionPayload => ({
    type: ECssrsListActions.setCssrsListGpraPeriodKey, payload: { grpaPeriodKey }
})

export const setCssrsListBsrReportType = (reportType: number): IActionPayload => ({
    type: ECssrsListActions.setCssrsListBsrReportType, payload: { reportType }
})

export const resetCssrsListParameter = (): Action => ({
    type: ECssrsListActions.resetCssrsListParameter
})

export const cssrsListchangeAutoUpdateStatus = (): Action => ({ type: ECssrsListActions.cssrsListchangeAutoUpdateStatus });

export const cssrsListchangeAutoUpdateStatusRequest = (): Action => ({ type: ECssrsListActions.cssrsListchangeAutoUpdateStatusRequest });

export const cssrsListCurrentPageRequest = (page: number):IActionPayload => ({
    type: ECssrsListActions.cssrsListCurrentPageRequest, payload: { page }
})
export const getCssrsLocationListActionRequest = () : Action => ({ type: ECssrsListActions.getCssrsLocationsListRequest })
export const getCssrsLocationListActionRequestSuccess = (payload: any) :IActionPayload  => ({ 
    type: ECssrsListActions.getCssrsLocationsListRequestSuccess,
    payload
});

export const getRelatedByIdCssrsRequest = (id: number | null): IActionPayload => ({ type: ECssrsListActions.getRelatedByIdCssrsRequest, payload: { id } });
export const getRelatedByIdCssrsRequestStart = (): Action => ({ type: ECssrsListActions.getRelatedByIdCssrsRequestStart });
export const getRelatedByIdCssrsRequestError = (): Action => ({ type: ECssrsListActions.getRelatedByIdCssrsRequestError });
export const getRelatedByIdCssrsRequestSuccess = (payload: Array<ICssrsVisitResponseItem>): IActionPayload => ({ type: ECssrsListActions.getRelatedByIdCssrsRequestSuccess, payload });
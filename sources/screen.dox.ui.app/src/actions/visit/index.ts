import { Action } from "redux";
import { EVisitActions, IActionPayload, ILocationItemResponse } from '../';

export type EReportType = {
    key: string;
    name: string;
    value: number;
}

export const visitReportsArray: Array<EReportType> = [
    {
        key: 'AllReports ',
        name: 'All Reports',
        value: 0,
    },
    {
        key: 'CompletedReports',
        name: 'Completed Reports',
        value: 1,
    },
    {
        key: 'incompleteReports',
        name: 'Incomplete Reports',
        value: 2,
    }
]

export const getAllVisitRequest = (): Action => ({ type: EVisitActions.getVisitRequest }); 
export const getAllVisitRequestStart = (): Action => ({ type: EVisitActions.getVisitRequestStart }); 
export const getAllVisitRequestError = (): Action => ({ type: EVisitActions.getVisitRequestError });

export interface IVisitRequest {
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
export interface IVisitResponseItem {
    PatientName: string;
    ID: number;
    ScreeningResultID: number;
    Birthday: string;
    LastCreatedDate: string;
    LastCreatedDateLabel: string;
    LastCompleteDate: string;
    LastCompleteDateLabel: string;
    LocationName: string;
}

export const getAllVisitRequestSuccess = (data: {
    Items: Array<IVisitResponseItem>;
    TotalCount: number;
}): IActionPayload => ({ 
    type: EVisitActions.getVisitRequestSuccess,
    payload: data
});

export const getVisitFilteredItems = (): Action => ({ type: EVisitActions.postVisitListFilterRequest });
export const getVisitFilteredItemsStart = (): Action => ({ type: EVisitActions.postVisitListFilterRequestStart });
export const getVisitFilteredItemsError = (): Action => ({ type: EVisitActions.postVisitListFilterRequestError });
export const getVisitFilteredItemsSuccess = (data: {
    Items: Array<IVisitResponseItem>;
    Total: number;
    Offset: number;
    CurrentPage: number;
}): IActionPayload => ({ 
    type: EVisitActions.postVisitListFilterRequestSucess, payload: data
});

export const setVisitCurrentPage = (page: number): IActionPayload => ({
    type: EVisitActions.setVisitCurrentPage, payload: { page }
});

export const requestVisitCurrentPage = (page: number): IActionPayload => ({
    type: EVisitActions.requestVisitCurrentPage, payload: { page }
});

export const setVisitListOffset = (offset: number): IActionPayload => ({
    type: EVisitActions.setVisitOffset, payload: { offset }
});

export const requestVisitListSort = (key: string, direction: string): IActionPayload => ({
    type: EVisitActions.requestVisitListSort, payload: { key, direction }
});

export const setVisitListSortKey = (key: string): IActionPayload => ({
    type: EVisitActions.setVisitListSortKey, payload: { key }
});

export const setVisitListSortDirection = (direction: string): IActionPayload => ({
    type: EVisitActions.setVisitListSortDirection, payload: { direction }
});

export const setVisitListFirstName = (firstName: string): IActionPayload => ({
    type: EVisitActions.setVisitSearchFirstName, payload: { firstName }
});

export const setVisitListLastName = (lastName: string): IActionPayload => ({
    type: EVisitActions.setVisitSearchLastName, payload: { lastName }
});

export const setVisitListScreeningID = (id: number): IActionPayload => ({
    type: EVisitActions.setVisitSearchScreeningID, payload: { id }
});

export const setVisitSearchLocations = (): Action => ({ type: EVisitActions.getVisitLocationListRequest });
export const setVisitSearchLocationsStart = (): Action => ({ type: EVisitActions.getVisitLocationListRequestStart });
export const setVisitSearchLocationsError = (): Action => ({ type: EVisitActions.getVisitLocationListRequestError });
export const setVisitSearchLocationsSuccess = (locations: Array<ILocationItemResponse>): IActionPayload => ({ 
    type: EVisitActions.getVisitLocationListRequestSuccess,
    payload: { locations }
});

export const setVisitSearchLocationsId = (id: number): IActionPayload => ({ 
    type: EVisitActions.setVisitLocationId, payload: { id }
});


export const setVisitCurrentScreenListStartDate = (startDate: string | null): IActionPayload => {
    return ({
        type: EVisitActions.setVisitScreendoxStartDate, payload: { startDate } 
    })
}


export const setVisitCurrentScreenListEndDate = (endDate: string | null): IActionPayload => {
    return ({
        type: EVisitActions.setVisitScreendoxEndDate, payload: { endDate } 
    })
}

export const setVisitCurrentGPRAPeriodRangeChange = (periodKey: string): IActionPayload => {
    return ({
        type: EVisitActions.setVisitScreendoxGRPAPeriod, payload: { periodKey } 
    })
}

export const triggerVisitSearchScreeningListSearchBarParameters = (): Action => ({
    type: EVisitActions.triggerVisitSearchParameters
})

export const cleanVisitSearchParameters = (): Action => ({
    type: EVisitActions.cleanVisitSearchParameters,
})

export const requestToCleanVisitSearchParameters = (): Action => ({
    type: EVisitActions.requestTocleanVisitSearchParameters,
})

export const setVisitBSRReportType = (reportType: number): IActionPayload => {
    return ({
        type: EVisitActions.setBSRReportType, payload: { reportType } 
    })
}

export const setVisitScreeningID = (screeningID: number | null): IActionPayload => {
    return ({
        type: EVisitActions.setVisitScreeningId, payload: { screeningID } 
    })
}

export const getRelatedByIdVisitRequest = (id: number | null)
: IActionPayload => ({ type: EVisitActions.getRelatedVisitByIDRequest, payload: { id } });

export const getRelatedByIdVisitRequestStart = ()
: Action => ({ type: EVisitActions.getRelatedVisitByIDRequestStart });

export const getRelatedByIdVisitRequestError = ()
: Action => ({ type: EVisitActions.getRelatedVisitByIDRequestError });

export interface IVisitReportsResponse {
    ID: number;
    Href: string;
    HasAddress: boolean;
    ReportName: string;
    CreatedDate: string;
    LocationName: string;
    ScreeningDate: string;
    CompletedDate: string;
    CreatedDateLabel: string;
    CompleteDateLabel: string;
    IsVisitRecordType: boolean;
    ScreeningResultID: number;
    ScreeningDateLabel: string;
    IsDemographicCssClass: string;
}

export const getRelatedByIdVisitRequestSuccess = (report: Array<IVisitReportsResponse>)
: IActionPayload => ({ type: EVisitActions.getRelatedVisitByIDRequestSuccess, payload: { report } });

export const changeAutoUpdateStatus = ()
: Action => ({ type: EVisitActions.changeAutoUpdateStatus });

export const changeAutoUpdateStatusRequest = ()
: Action => ({ type: EVisitActions.changeAutoUpdateStatusRequest });
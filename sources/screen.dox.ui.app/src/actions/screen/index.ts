import { IEhrRecordPatientsItem } from 'actions/find-patient-address';
import { Action } from 'redux';
import { 
    EScreenActionType, IActionPayload, ILocationItemResponse, TGPRAPeriodResponseItem 
} from '..';
import { IScreeningReportPatientInfo } from './report';

export interface IScreenListResponseItem {
    PatientName: string;
    ScreeningResultID: number;
    Birthday: Date;
    LastCheckinDate: Date;
    LastCheckinDateLabel: string;
    HasExport: boolean;
    ExportDate: Date;
    ExportDateLabel: string;
}

export interface IScreenListEhrExportVisitRecord {
    DateFormatted: string;
    ID: number;
    Date: string;
    ServiceCategory: string;
    Location: {
        ID: number;
        Name: string;
    }
}

export interface IScreenListEhrExportFinalResultResponse {
    Errors: Array<string>;
    ExportResults: Array<{ ActionName: string; IsSuccessful: boolean, Fault: string | null}>
    ExportScope: { 
        CrisisAlerts: Array<string>;
        Errors: Array<string>;
        Exams: Array<{
            Code: string;
            Comment: string;
            ExamID: number;
            ExamName: string;
            Result: string;
            ResultLabel: string;
        }>;
        HealthFactors: Array<{
            Code: string;
            Comment: string;
            Factor: string;
            FactorID: number;
        }>;
        PatientRecordModifications: Array<{
            CurrentValue: string;
            Field: number;
            UpdateWithValue: string;
        }>;
        ScreeningSections: Array<string>;
    };
    IsSuccessful: boolean;
    OperationStatusText: string;
    Status: number;
}

export const postScreenListRequest = (): Action => ({
    type: EScreenActionType.postScreenListSearchRequest,
});

export const postScreenListRequestStart = (): Action => ({
    type: EScreenActionType.postScreenListSearchRequestStart,
});

export const postScreenListRequestError = (): Action => ({
    type: EScreenActionType.postScreenListSearchRequestError,
});

export const postScreenListRequestSuccess = (payload: { Items: IScreenListResponseItem[] }): IActionPayload => {
    return ({
        type: EScreenActionType.postScreenListSearchRequestSuccess, payload 
    })
} 

export interface IScreenListInnerItem {
    ScreeningResultID: number;
    ScreeningName: string;
    CreatedDate: Date;
    CreatedDateLabel: string;
    IsPositive: boolean;
    ExportDate: string;
    ExportDateLabel: string;
    ExportedToHRN: string;
    HasAnyScreening: boolean;
    HasAddress: boolean;
    ShowBeginExportButton: boolean;
    IsPositiveCssClass: string;
}

export const postScreenListItemRequest = (ScreeningResultID: number): IActionPayload => ({
    type: EScreenActionType.getInternalScreenListItemDataRequest,
    payload: { ScreeningResultID }
});

export const postScreenListItemRequestStart = (): Action => ({
    type: EScreenActionType.getInternalScreenListItemDataRequestStart,
});

export const postScreenListItemRequestError = (): Action => ({
    type: EScreenActionType.getInternalScreenListItemDataRequestError,
});

export const postScreenListItemRequestSuccess = (
    payload: { 
        Items: IScreenListInnerItem[],  
        ScreeningResultID: number 
    }
): IActionPayload => {
    return ({
        type: EScreenActionType.getInternalScreenListItemDataRequestSuccess, payload 
    })
} 


export type TPostScreenListFilterRequestProps = {
    page: number;
}

export const postScreenListFilterRequest = (payload: TPostScreenListFilterRequestProps): IActionPayload => ({
    type: EScreenActionType.postScreenListFilterRequest, payload
});

export const postScreenListFilterRequestStart = (): Action => ({
    type: EScreenActionType.postScreenListFilterRequestStart,
});

export const postScreenListFilterRequestError = (): Action => ({
    type: EScreenActionType.postScreenListFilterRequestError,
});

export const postScreenListFilterRequestSuccess = (
    payload: { 
        Items: IScreenListResponseItem[];
        Total: number;
        Offset: number;
        CurrentPage: number;
    }
): IActionPayload => {
    return ({
        type: EScreenActionType.postScreenListFilterRequestSucess, payload 
    })
} 

export const postScreenListFilterRequestAutoUpdate = (): Action => {
    return {
        type: EScreenActionType.postScreenListFilterAutoUpdat,
    }
}

export const postScreenListFilterRequestAutoUpdateRecall = (): Action => {
    return {
        type: EScreenActionType.postScreenListFilterAutoUpdatRecall,
    }
}


export const postScreenListFilterRequestAutoUpdateSet = (value: boolean): IActionPayload => {
    return {
        type: EScreenActionType.postScreenListFilterAutoUpdateSet,
        payload: { value }
    }
}

export const changeActiveSortObject = (key: string, direction: string): IActionPayload => {
    return ({
        type: EScreenActionType.changeActiveSort, payload: { key, direction } 
    })
}

export const changeActiveSortDirection = (key: string, direction: string): IActionPayload => {
    return ({
        type: EScreenActionType.changeActiveDirection, payload: { key, direction } 
    })
}

export const changeActiveSortKey = (key: string): IActionPayload => {
    return ({
        type: EScreenActionType.changeActiveKey, payload: { key } 
    })
}

export const setFirstName = (firstName: string): IActionPayload => {
    return ({
        type: EScreenActionType.setFirstName, payload: { firstName } 
    })
}

export const setLastName = (lastName: string): IActionPayload => {
    return ({
        type: EScreenActionType.setLastName, payload: { lastName } 
    })
}

export const setScreendoxId = (screeningResultID: number | null): IActionPayload => {
    return ({
        type: EScreenActionType.setScreendoxReultId, payload: { screeningResultID } 
    })
}

export const setRequestScreendoxId = (screeningID: number | null): IActionPayload => {
    return ({
        type: EScreenActionType.setRequestScreeningId, payload: { screeningID } 
    })
}

export const setCurrentScreenListOffset = (offset: number): IActionPayload => {
    return ({
        type: EScreenActionType.setCurrentScreenListOffset, payload: { offset } 
    })
}

export const setCurrentScreenListPage = (page: number): IActionPayload => {
    return ({
        type: EScreenActionType.setCurrentScreenListPage, payload: { page } 
    })
}

export const setCurrentScreenListStartDate = (startDate: string | null): IActionPayload => {
    return ({
        type: EScreenActionType.setScreendoxStartDate, payload: { startDate } 
    })
}


export const setCurrentScreenListEndDate = (endDate: string | null): IActionPayload => {
    return ({
        type: EScreenActionType.setScreendoxEndDate, payload: { endDate } 
    })
}

export const setCurrentGPRAPeriodRangeChange = (periodKey: string): IActionPayload => {
    return ({
        type: EScreenActionType.setScreendoxGRPAPeriod, payload: { periodKey } 
    })
}

export const clearScreeningListSearchBarParameters = (): Action => ({
    type: EScreenActionType.clearSearchParameters
})

export const triggerClearScreeningListSearchBarParameters = (): Action => ({
    type: EScreenActionType.triggerClearSearchParameters
})

export const triggerSearchScreeningListSearchBarParameters = (): Action => ({
    type: EScreenActionType.triggerSearchParameters
})

export const getGPRAPeriodsAction = (): Action => ({
    type: EScreenActionType.getGPRAPeriodRequest,
});

export const getGPRAPeriodsStartAction = (): Action => ({
    type: EScreenActionType.getGPRAPeriodStart,
});

export const getGPRAPeriodsErrorAction = (): Action => ({
    type: EScreenActionType.getGPRAPeriodError,
});

export const getGPRAPeriodsSuccessAction = (gpraPeriods: Array<TGPRAPeriodResponseItem>)
: IActionPayload => ({
    type: EScreenActionType.getGPRAPeriodSuccess,
    payload: { gpraPeriods }
});

export const setLocationIdAction = (id: number): IActionPayload => ({
    type: EScreenActionType.setLocationId, payload: { id }
});

export const getLocationListAction = (): Action => ({
    type: EScreenActionType.getLocationListRequest,
});

export const getLocationListStartAction = (): Action => ({
    type: EScreenActionType.getLocationListRequestStart,
});

export const getLocationListErrorAction = (): Action => ({
    type: EScreenActionType.getLocationListRequestError,
});

export const getLocationListSuccessAction = (locations: Array<ILocationItemResponse>): IActionPayload => ({
    type: EScreenActionType.getLocationListRequestSuccess,
    payload: { locations }
});

export const getScreenListEhrExportPatientInfoRequest = (id: number): IActionPayload => ({
    type: EScreenActionType.getScreenListEhrExportPatientInfoRequest, payload: { id }
})

export const getScreenListEhrExportPatientInfoRequestSuccess = (payload: IScreeningReportPatientInfo): IActionPayload => ({
    type: EScreenActionType.getScreenListEhrExportPatientInfoRequestSuccess, payload
})

export const postScreenListEhrExportPatientInfoRequest = (id: number): IActionPayload => ({
    type: EScreenActionType.postScreenListEhrExportPatientInfoRequest, payload: { id }
})

export const postScreenListEhrExportPatientInfoRequestStart = (): Action => ({
    type: EScreenActionType.postScreenListEhrExportPatientInfoRequestStart
})
export const postScreenListEhrExportPatientInfoRequestSuccess = (): Action => ({
    type: EScreenActionType.postScreenListEhrExportPatientInfoRequestSuccess
})
export const postScreenListEhrExportPatientInfoRequestError = (): Action => ({
    type: EScreenActionType.postScreenListEhrExportPatientInfoRequestError
})

export const getScreenListEhrExportPatientRecordsSuccess = (payload: { Items: Array<IEhrRecordPatientsItem>, TotalCount: number }): IActionPayload => ({
    type: EScreenActionType.getScreenListEhrExportPatientRecordsSuccess, payload
})

export const getScreenListEhrExportPatientRecordsError = ():Action => ({
    type: EScreenActionType.getScreenListEhrExportPatientRecordsError
})

export const getScreenListEhrExportPatientRecordsStart = ():Action => ({
    type: EScreenActionType.getScreenListEhrExportPatientRecordsStart
})

export const setEhrExportPatientRecordSelectedId = (ehrExportPatientRecordSelectedId: number | null): IActionPayload => ({
    type:EScreenActionType.setEhrExportPatientRecordSelectedId, payload: { ehrExportPatientRecordSelectedId }
})

export const setEhrExportPatientRecordsCurrentPage = (currentPage: number) : IActionPayload => ({
    type: EScreenActionType.setEhrExportPatientRecordsCurrentPage, payload: { currentPage }
})
export const getScreenListEhrExportVisitRecordsRequest = (patientId: number): IActionPayload => ({
    type: EScreenActionType.getScreenListEhrExportVisitRecordsRequest, payload: { patientId }
})

export const getScreenListEhrExportVisitRecordsRequestStart = (): Action => ({
    type: EScreenActionType.getScreenListEhrExportVisitRecordsRequestStart
})

export const getScreenListEhrExportVisitRecordsRequestError = (): Action => ({
    type: EScreenActionType.getScreenListEhrExportVisitRecordsRequestError
})

export const getScreenListEhrExportVisitRecordsRequestSuccess = (payload: { Items: Array<IScreenListEhrExportVisitRecord>, TotalCount: number }): IActionPayload => ({
    type: EScreenActionType.getScreenListEhrExportVisitRecordsRequestSuccess, payload
})

export const setEhrExportVisitRecordSelectedId = (ehrExportVisitRecordSelectedId: number | null): IActionPayload => ({
    type: EScreenActionType.setEhrExportVisitRecordSelectedId, payload: { ehrExportVisitRecordSelectedId }
})

export const setEhrExportVisitRecordsCurrentPage = (currentPage: number): IActionPayload => ({
    type: EScreenActionType.setEhrExportVisitRecordsCurrentPage, payload: { currentPage }
})

export const ehrExportFinalResultRequest = (screeningResultId: number): IActionPayload => ({
    type: EScreenActionType.ehrExportFinalResultRequest, payload: { screeningResultId }
})

export const ehrExportFinalResultRequestStart = (): Action => ({
    type: EScreenActionType.ehrExportFinalResultRequestStart
})

export const ehrExportFinalResultRequestSuccess = (): Action => ({
    type: EScreenActionType.ehrExportFinalResultRequestSuccess
})

export const ehrExportFinalResultRequestError = (): Action => ({
    type: EScreenActionType.ehrExportFinalResultRequestError
})

export const setEhrExportScreeningResultId = (screeningResultId: number | null): IActionPayload => ({
    type: EScreenActionType.setEhrExportScreeningResultId, payload: { screeningResultId }
})

export const setEhrExportScreeningDate = (screeningDate: string): IActionPayload => ({
    type: EScreenActionType.setEhrExportScreeningDate, payload: { screeningDate }
})

export const setEhrExportSelectedTab = (selectedTab: number): IActionPayload => ({
    type: EScreenActionType.setEhrExportSelectedTab, payload: { selectedTab }
})

export const setEhrExportScreeningResults = (payload: IScreenListEhrExportFinalResultResponse): IActionPayload => ({
    type: EScreenActionType.setEhrExportScreeningResults, payload
})
import { IScreeningReportPatientInfo } from 'actions/screen/report';
import { 
    IActionPayload, EScreenActionType, ISortState, TGPRAPeriodResponseItem, ILocationItemResponse 
} from '../../actions';
import { 
    IScreenListEhrExportVisitRecord,
    IScreenListInnerItem,  IScreenListResponseItem, IScreenListEhrExportFinalResultResponse,
} from '../../actions/screen';
import { IEhrRecordPatientsItem } from '../../actions/find-patient-address';

export const MAXIMUM_RECORDS_PER_PAGE = 20;

export interface IScreenState {
    currentPage: number;
    itemsToShow: number;
    offset: number;
    firstName: string;
    lastName: string;
    screeningResultID: number | null;
    screeningID: number | null;
    totalScreenListItems: number;
    isAutoUpdate: boolean;
    isLoadingScreenList: boolean;
    isScreenListError: boolean;
    screenList: IScreenListResponseItem[] | [];
    sortObject: { [k: string]: ISortState; } 
    activeSort: string;
    activeDirection: string;
    reportObject: { [k: number]: IScreenListInnerItem[] };
    startDate: string | null,
    endDate: string | null,
    grpaPeriodKey: string;
    grpaPeriods: Array<TGPRAPeriodResponseItem>;
    locations: Array<ILocationItemResponse>;
    selectedLocationId: number;
    EhrExportPatientInfo: IScreeningReportPatientInfo;
    isEhrExportPatientInfoSavingLoading: boolean;
    ehrExportPatientRecords: Array<IEhrRecordPatientsItem>;
    ehrExportCurrentPage: number;
    ehrExportTotalRecords: number;
    isEhrExportPatientRecordsError: boolean;
    isEhrExportPatientRecordsLoading: boolean;
    ehrExportPatientRecordSelectedId: number | null;
    ehrExportVisitRecords: Array<IScreenListEhrExportVisitRecord>;
    isEhrExportVisitRecordLoading: boolean;
    isEhrExportVisitRecordError: boolean;
    ehrExportVisitTotalRecords:number;
    ehrExportVisitRecordsCurrentPage: number;
    ehrExportVisitRecordSelectedId: number | null;
    ehrExportFinalResultLoading: boolean;
    ehrExportScreeningResultId: number | null;
    ehrExportScreeningDate: string | null;
    ehrExportSelectedTab: number;
    ehrExportFinalResults: IScreenListEhrExportFinalResultResponse | null;
}

export const screenInitState: IScreenState = {
    currentPage: 1,
    itemsToShow: MAXIMUM_RECORDS_PER_PAGE,
    offset: 0,
    totalScreenListItems: 0,
    isAutoUpdate: true,
    screenList: [],
    isLoadingScreenList: false,
    isScreenListError: false,
    sortObject: {},
    activeSort: '',
    activeDirection: '',
    reportObject: {},
    firstName: '',
    lastName: '',
    screeningResultID: null,
    screeningID: null,
    startDate: "2020-10-01",
    endDate: null,
    grpaPeriodKey: '',
    grpaPeriods: [],
    locations: [],
    selectedLocationId: 0,
    EhrExportPatientInfo: {
        City: '',
        Phone: '',
        StateID: '',
        StateName: '',
        StreetAddress: '',
        ZipCode: '',
        Birthday: '',
        FirstName: '',
        LastName: '',
        MiddleName: '',
        FullName: '',
        Age: 0
    },
    isEhrExportPatientInfoSavingLoading: false,
    ehrExportPatientRecords: [],
    ehrExportCurrentPage: 1,
    ehrExportTotalRecords: 0,
    isEhrExportPatientRecordsError: false,
    isEhrExportPatientRecordsLoading: false,
    ehrExportPatientRecordSelectedId: null,
    ehrExportVisitRecords: [],
    isEhrExportVisitRecordLoading: false,
    isEhrExportVisitRecordError: false,
    ehrExportVisitTotalRecords: 0,
    ehrExportVisitRecordsCurrentPage: 1,
    ehrExportVisitRecordSelectedId: null,
    ehrExportFinalResultLoading: false,
    ehrExportScreeningResultId: null,
    ehrExportSelectedTab: 0,
    ehrExportFinalResults: null,
    ehrExportScreeningDate: null,
}

const screenReducer = (state: IScreenState = screenInitState, action: IActionPayload) => {
    switch(action.type) {
        case EScreenActionType.setLocationId:
            return {
                ...state,
                selectedLocationId: action.payload.id,
            }
        case EScreenActionType.getLocationListRequestSuccess:
            return {
                ...state,
                locations: action.payload.locations,
            }
        case EScreenActionType.getGPRAPeriodSuccess:
            return {
                ...state,
                grpaPeriods: action.payload.gpraPeriods,
            }
        case EScreenActionType.clearSearchParameters:
            return {
                ...state,
                firstName: null,
                lastName: null,
                screeningResultID: null,
                startDate: null,
                endDate: null,
                grpaPeriodKey: screenInitState.grpaPeriodKey,
            }
        case EScreenActionType.setScreendoxGRPAPeriod:
            return {
                ...state,
                grpaPeriodKey:  action.payload.periodKey,
            }
        case EScreenActionType.setScreendoxStartDate:
            return {
                ...state,
                startDate:  action.payload.startDate,
            }
        case EScreenActionType.setScreendoxEndDate:
            return {
                ...state,
                endDate:  action.payload.endDate,
            }
        case EScreenActionType.setRequestScreeningId:
            return {
                ...state,
                screeningID: action.payload.screeningID,
            }
        case EScreenActionType.setScreendoxReultId:
            return {
                ...state,
                screeningResultID:  action.payload.screeningResultID,
            }
        case  EScreenActionType.setLastName:
            return {
                ...state,
                lastName: action.payload.lastName,
            }
        case  EScreenActionType.setFirstName:
            return {
                ...state,
                firstName: action.payload.firstName,
            }
        case EScreenActionType.changeActiveKey:
            return {
                ...state,
                activeSort: action.payload.key,
            }
        case EScreenActionType.changeActiveDirection:
            return {
                ...state,
                activeDirection: action.payload.direction,
            }
        case EScreenActionType.postScreenListFilterAutoUpdateSet: 
            return {
                ...state,
                isAutoUpdate: action.payload.value,
            }
        case EScreenActionType.getGPRAPeriodStart:
        case EScreenActionType.getLocationListRequestStart:
        case EScreenActionType.postScreenListFilterRequestStart:
        case EScreenActionType.postScreenListSearchRequestStart:
        case EScreenActionType.getInternalScreenListItemDataRequestStart:
            return {
                ...state,
                isLoadingScreenList: true,
                isScreenListError: false,
            }
        case EScreenActionType.getInternalScreenListItemDataRequestSuccess:
            return {
                ...state,
                reportObject: {
                    ...state.reportObject,
                    [action.payload.ScreeningResultID]: action.payload.Items,
                },
                isLoadingScreenList: false,
                ScreeningResultID: action.payload.ScreeningResultID,
            }
        case EScreenActionType.setCurrentScreenListOffset:
            return {
                ...state,
                offset: action.payload.offset,
            }
        case EScreenActionType.setCurrentScreenListPage:
            return {
                ...state,
                currentPage: action.payload.page,
            }
        case EScreenActionType.postScreenListFilterRequestSucess:
            return {
                ...state,
                offset: action.payload.Offset,
                currentPage: action.payload.CurrentPage,
                screenList: action.payload.Items,
                totalScreenListItems: action.payload.Total,
                isLoadingScreenList: false,
                isScreenListError: false,
                currentScreeningReport: null,
            }
        case EScreenActionType.postScreenListSearchRequestSuccess:
            return {
                ...state,
                screenList: action.payload.Items,
                currentScreeningReport: null,
                isLoadingScreenList: false,
                isScreenListError: false,
            }
        case EScreenActionType.getGPRAPeriodError:
        case EScreenActionType.getLocationListRequestError:
        case EScreenActionType.postScreenListFilterRequestError:
        case EScreenActionType.postScreenListSearchRequestError:
        case EScreenActionType.getInternalScreenListItemDataRequestError:
                        return {
                ...state,
                isLoadingScreenList: false,
                isScreenListError: true,
            }
        case EScreenActionType.getScreenListEhrExportPatientInfoRequestSuccess:
            return {
                ...state,
                EhrExportPatientInfo: action.payload
            }
        case EScreenActionType.postScreenListEhrExportPatientInfoRequestStart:
            return {
                ...state,
                isEhrExportPatientInfoSavingLoading: true,
            }
        case EScreenActionType.postScreenListEhrExportPatientInfoRequestSuccess:
            return {
                ...state,
                isEhrExportPatientInfoSavingLoading: false,
            }
        case EScreenActionType.postScreenListEhrExportPatientInfoRequestError:
            return {
                ...state,
                isEhrExportPatientInfoSavingLoading: false,
            }
        case EScreenActionType.getScreenListEhrExportPatientRecordsSuccess:
            return {
                ...state,
                ehrExportPatientRecords: action.payload.Items,
                isEhrExportPatientRecordsError: false,
                ehrExportTotalRecords: action.payload.TotalCount,
                isEhrExportPatientRecordsLoading: false,
            }
        case EScreenActionType.getScreenListEhrExportPatientRecordsError:
            return {
                ...state,
                isEhrExportPatientRecordsError: true,
                isEhrExportPatientRecordsLoading: false,
            }
        case EScreenActionType.getScreenListEhrExportPatientRecordsStart: 
            return {
                ...state,
                isEhrExportPatientRecordsError: false,
                isEhrExportPatientRecordsLoading: true,
            }
        case EScreenActionType.setEhrExportPatientRecordSelectedId: 
            return {
                ...state,
                ehrExportPatientRecordSelectedId: action.payload.ehrExportPatientRecordSelectedId
            }
        case EScreenActionType.setEhrExportPatientRecordsCurrentPage:
            return {
                ...state,
                ehrExportCurrentPage: action.payload.currentPage
            }
        case EScreenActionType.getScreenListEhrExportVisitRecordsRequestStart:
            return {
                ...state,
                isEhrExportVisitRecordLoading: true,
                isEhrExportVisitRecordError: false,
            }
        case EScreenActionType.getScreenListEhrExportVisitRecordsRequestError:
            return {
                ...state,
                isEhrExportVisitRecordLoading: false,
                isEhrExportVisitRecordError: true,
            }
        case EScreenActionType.getScreenListEhrExportVisitRecordsRequestSuccess:
            return {
                ...state,
                ehrExportVisitRecords: action.payload.Items,
                isEhrExportVisitRecordLoading: false,
                isEhrExportVisitRecordError: false,
                ehrExportVisitTotalRecords: action.payload.TotalCount
            }
        case EScreenActionType.setEhrExportVisitRecordSelectedId:
            return {
                ...state,
                ehrExportVisitRecordSelectedId: action.payload.ehrExportVisitRecordSelectedId
            }
        case EScreenActionType.setEhrExportVisitRecordsCurrentPage:
            return {
                ...state,
                ehrExportVisitRecordsCurrentPage: action.payload.currentPage
            }
        case EScreenActionType.ehrExportFinalResultRequestStart:
            return {
                ...state,
                ehrExportFinalResultLoading: true,
            }
        case EScreenActionType.ehrExportFinalResultRequestError:
            return {
                ...state,
                ehrExportFinalResultLoading: false,
            }
        case EScreenActionType.ehrExportFinalResultRequestSuccess:
            return {
                ...state,
                ehrExportFinalResultLoading: false,
            }
        case EScreenActionType.setEhrExportScreeningResultId:
            return {
                ...state,
                ehrExportScreeningResultId: action.payload.screeningResultId
            }
        case EScreenActionType.setEhrExportScreeningDate:
            return {
                ...state,
                ehrExportScreeningDate: action.payload.screeningDate
            }
        case EScreenActionType.setEhrExportSelectedTab:
            return {
                ...state,
                ehrExportSelectedTab: action.payload.selectedTab
            }
        case EScreenActionType.setEhrExportScreeningResults:
            return {
                ...state,
                ehrExportFinalResults: action.payload
            }
        default: return state;
    }
}

export default screenReducer;
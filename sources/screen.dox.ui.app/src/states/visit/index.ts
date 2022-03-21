import { 
    IActionPayload, EVisitActions, ISortState, TGPRAPeriodResponseItem, ILocationItemResponse 
} from '../../actions';
import  { IVisitReportsResponse, IVisitResponseItem } from '../../actions/visit';

export interface IVisitState {
    currentPage: number;
    itemsToShow: number;
    offset: number;
    firstName: string;
    lastName: string;
    totalVisitListItems: number;
    isAutoUpdate: boolean;
    sortObject: { [k: string]: ISortState; } 
    activeSort: string;
    activeDirection: string;
    screeningResultID: number | null;
    startDate: string | null,
    endDate: string | null,
    grpaPeriodKey: string;
    grpaPeriods: Array<TGPRAPeriodResponseItem>;
    locations: Array<ILocationItemResponse>;
    selectedLocationId: number;
    isVisitLoading: boolean;
    totalVisitItems: number;
    visitItems: Array<IVisitResponseItem>;
    bsrReportType: number;
    reportDescriptors: Array<IVisitReportsResponse>;
    isListError: boolean;
}

export const MAXIMUM_RECORDS_PER_PAGE = 20;

const visitInitState: IVisitState =  {
    bsrReportType: 0,
    currentPage: 1,
    itemsToShow: MAXIMUM_RECORDS_PER_PAGE,
    offset: 0,
    totalVisitListItems: 0,
    isAutoUpdate: true,
    sortObject: {},
    activeSort: '',
    activeDirection: '',
    firstName: '',
    lastName: '',
    screeningResultID: null,
    startDate: null,
    endDate: null,
    grpaPeriodKey: '',
    grpaPeriods: [],
    locations: [],
    selectedLocationId: 0,
    isVisitLoading: false,
    totalVisitItems: 0,
    visitItems: [],
    reportDescriptors: [],
    isListError: false,
}

const visitReducer = (state: IVisitState = visitInitState, action: IActionPayload) => {
    
    switch(action.type) {
        case EVisitActions.changeAutoUpdateStatus:
            return {
                ...state,
                isAutoUpdate: !state.isAutoUpdate,
            }
        case EVisitActions.getRelatedVisitByIDRequestError:
            return {
                ...state,
                isListError: true,
                isVisitLoading: false
            }
        case EVisitActions.getRelatedVisitByIDRequestSuccess:
            return {
                ...state,
                reportDescriptors:  action.payload.report,
                isVisitLoading: false
            }
        case EVisitActions.getRelatedVisitByIDRequestStart:
            return {
                ...state,
                isVisitLoading: true
            }
        case EVisitActions.setVisitScreeningId:
            return {
                ...state,
                screeningResultID:  action.payload.screeningID,
            }
        case EVisitActions.setBSRReportType:
            return {
                ...state,
                bsrReportType:  action.payload.reportType,
            }
        case EVisitActions.cleanVisitSearchParameters:
            return {
                ...state,
                isListError: false,
                grpaPeriodKey:  visitInitState.grpaPeriodKey,
                startDate:  visitInitState.startDate,
                endDate:  visitInitState.endDate,
                selectedLocationId: visitInitState.selectedLocationId,
                screeningResultID: visitInitState.screeningResultID,
                lastName: visitInitState.lastName,
                firstName: visitInitState.firstName,
                activeDirection: visitInitState.activeDirection,
                offset: visitInitState.offset,
                currentPage: visitInitState.currentPage,
                bsrReportType: 0
            }
        case EVisitActions.setVisitScreendoxGRPAPeriod:
            return {
                ...state,
                grpaPeriodKey:  action.payload.periodKey,
            }
        case EVisitActions.setVisitScreendoxStartDate:
            return {
                ...state,
                startDate:  action.payload.startDate,
            }
        case EVisitActions.setVisitScreendoxEndDate:
            return {
                ...state,
                endDate:  action.payload.endDate,
            }
        case EVisitActions.setVisitLocationId:
            return {
                ...state,
                selectedLocationId: action.payload.id,
            }
        case EVisitActions.getVisitLocationListRequestSuccess:
            return {
                ...state,
                locations: action.payload.locations,
            }
        case EVisitActions.setVisitSearchScreeningID:
            return {
                ...state,
                screeningResultID: action.payload.id,
            }
        case EVisitActions.setVisitSearchLastName:
            return {
                ...state,
                lastName: action.payload.lastName,
            }
        case EVisitActions.setVisitSearchFirstName:
            return {
                ...state,
                firstName: action.payload.firstName,
            }
        case EVisitActions.setVisitListSortDirection:
            return {
                ...state,
                activeDirection: action.payload.direction,
            }
        case EVisitActions.setVisitListSortKey:
            return {
                ...state,
                activeSort: action.payload.key,
            }
        case EVisitActions.setVisitOffset:
            return {
                ...state,
                offset: action.payload.offset,
            }
        case EVisitActions.setVisitCurrentPage:
            return {
                ...state,
                currentPage: action.payload.page,
            }
        // case EVisitActions.postVisitListFilterRequestStart:
        case EVisitActions.getVisitRequestStart:
            return {
                ...state,
                isVisitLoading: true,
            }
        case EVisitActions.postVisitListFilterRequestError:
        case EVisitActions.getVisitRequestError:
            return {
                ...state,
                isVisitLoading: false,
            }
        case EVisitActions.getVisitRequestSuccess:
            return {
                ...state,
                isVisitLoading: false,
                isListError: false,
                totalVisitItems: action.payload.TotalCount,
                visitItems: action.payload.Items,
            }
        case EVisitActions.postVisitListFilterRequestSucess:
            return {
                ...state,
                isVisitLoading: false,
                isListError: false,
                visitItems: action.payload.Items,
                totalVisitItems: action.payload.Total,
                offset: action.payload.Offset,
                currentPage: action.payload.CurrentPage,
            }
        default: return state;
    }
}

export default visitReducer;
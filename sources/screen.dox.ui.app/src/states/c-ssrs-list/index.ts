import { IActionPayload, ISortState, TGPRAPeriodResponseItem, ILocationItemResponse, ECssrsListActions } from '../../actions';
import {  ICssrsListResponse, ICssrsVisitResponseItem  } from 'actions/c-ssrs-list';


export interface ICssrsListState {
    isLoading: boolean;
    currentPage: number;
    itemsToShow: number;
    offset: number;
    firstName: string;
    lastName: string;
    dateofBirth:string | null;
    isAutoUpdate: boolean;
    sortObject: { [k: string]: ISortState; } 
    activeSort: string;
    activeDirection: string;  
    startDate: string | null,
    endDate: string | null,
    grpaPeriodKey: string;
    grpaPeriods: Array<TGPRAPeriodResponseItem>;
    locations: Array<ILocationItemResponse>;
    selectedLocationId: number;
    totalCssrsList: number;
    cssrsList: Array<ICssrsListResponse>;
    bsrReportType: number;
    isCssrsVisitLoading: boolean;
    cssrsVisitReport: Array<ICssrsVisitResponseItem>;
}

export const MAXIMUM_RECORDS_PER_PAGE = 20;


export const cssrsListInitState: ICssrsListState = {
    bsrReportType: 0,
    currentPage: 1,
    itemsToShow: MAXIMUM_RECORDS_PER_PAGE,
    offset: 0,
    totalCssrsList: 0,
    isAutoUpdate: true,
    sortObject: {},
    activeSort: 'LastCreatedDate',
    activeDirection: 'desc',
    firstName: '',
    lastName: '',
    dateofBirth: null,
    startDate: "2020-10-01",
    endDate: null,
    grpaPeriodKey: '',
    grpaPeriods: [],
    locations: [],
    selectedLocationId: 0,
    isLoading: false,
    cssrsList: [],
    isCssrsVisitLoading: false,
    cssrsVisitReport: []
}

const cssrsListReducer = (state: ICssrsListState = cssrsListInitState, action: IActionPayload) => {
    switch(action.type) {
        case ECssrsListActions.getCssrsListRequestStart:
            return {
                ...state,
                isLoading: true
            }
        case ECssrsListActions.getCssrsListRequestError:
            return {
                ...state,
                isLoading: false,
            }
        case ECssrsListActions.getCssrsListRequestSuccess:
            return {
                ...state,
                isLoading: false,
                cssrsList: action.payload.Items,
                totalCssrsList: action.payload.TotalCount,
            }
        case ECssrsListActions.setCssrsListCurrentPage:
            return {
                ...state,
                currentPage: action.payload.currentPage,
            }
        case ECssrsListActions.setCssrsListOffset:
             return {
                 ...state,
                offset: action.payload.offset,
            }
        case ECssrsListActions.setCssrsListSortKey:
            return {
                ...state,
                activeSort: action.payload.sortKey,
            }
        case ECssrsListActions.setCssrsListSortDirection:
            return {
                ...state,
                activeDirection: action.payload.sortDirection
            }
        case ECssrsListActions.setCssrsListFirstName:
            return {
                ...state,
                firstName: action.payload.firstName,
            }
        case ECssrsListActions.setCssrsListLastName:
            return {
                ...state,
                lastName: action.payload.lastName,
            }
        case ECssrsListActions.setCssrsListDateofBirth:
             return {
                ...state,
                dateofBirth: action.payload.dateofBirth,
            }
        case ECssrsListActions.setCssrsListScreeningResultId:
            return {
                ...state,
                screeningResultID: action.payload.screeningResultId,
            }
        case ECssrsListActions.setCssrsListLocationId:
            return {
                ...state,
                selectedLocationId: action.payload.locationId,
            }
        case ECssrsListActions.getCssrsLocationsListRequestSuccess:
            return {
                ...state,
                locations: action.payload,
            }
        case ECssrsListActions.setCssrsListStartDate:
            return {
                ...state,
                startDate : action.payload.startDate
            }
        case ECssrsListActions.setCssrsListEndDate:
            return {
                ...state,
                endDate: action.payload.endDate
            }
        case ECssrsListActions.setCssrsListGpraPeriodKey:
            return {
                ...state,
                grpaPeriodKey: action.payload.grpaPeriodKey
            }
        case ECssrsListActions.setCssrsListBsrReportType:
            return {
                ...state,
                bsrReportType: action.payload.reportType
            }
        case ECssrsListActions.resetCssrsListParameter:
            return {
                ...state,
                ...cssrsListInitState
            }
        case ECssrsListActions.cssrsListchangeAutoUpdateStatus:
            return {
                ...state,
                isAutoUpdate: !state.isAutoUpdate
            }
        case ECssrsListActions.getRelatedByIdCssrsRequestError:
            return {
                ...state,
                isCssrsVisitLoading: false,
            }
        case ECssrsListActions.getRelatedByIdCssrsRequestStart:
            return {
                ...state,
                isCssrsVisitLoading: true,
            }
        case ECssrsListActions.getRelatedByIdCssrsRequestSuccess:
            return {
                ...state,
                isCssrsVisitLoading: false,
                cssrsVisitReport: action.payload
            }
            
        default: return state;
    }
}

export default cssrsListReducer;
import { IActionPayload, EFollowUpActions, ISortState, TGPRAPeriodResponseItem, ILocationItemResponse, EBranchLocationsActions } from '../../actions';
import { IFollowUpRelatedReportResponse, IFollowUpResponseItem } from '../../actions/follow-up';

export interface IFollowUpState {
    currentPage: number;
    itemsToShow: number;
    offset: number;
    isFollowUpLoading: boolean;
    followUpResponseList: Array<IFollowUpResponseItem>;
    totalFollowUps: number;
    isAutoUpdate: boolean;
    firstName: string;
    lastName: string;
    screeningResultID: number | null;
    sortObject: { [k: string]: ISortState; } 
    activeSort: string;
    activeDirection: string;
    reportObject: { [k: number]: IFollowUpRelatedReportResponse[] };
    startDate: string | null,
    endDate: string | null,
    grpaPeriodKey: string;
    grpaPeriods: Array<TGPRAPeriodResponseItem>;
    locations: Array<ILocationItemResponse>;
    selectedLocationId: number;
    bsrReportType: number;
}

export const MAXIMUM_RECORDS_PER_PAGE = 20;

export const followUpInitState: IFollowUpState = {
    currentPage: 1,
    itemsToShow: MAXIMUM_RECORDS_PER_PAGE,
    offset: 0,
    isFollowUpLoading: true,
    followUpResponseList: [],
    totalFollowUps: 0,
    isAutoUpdate: true,
    sortObject: {},
    activeSort: '',
    activeDirection: '',
    reportObject: {},
    firstName: '',
    lastName: '',
    screeningResultID: null,
    startDate: "2019-10-01",
    endDate: null,
    grpaPeriodKey: '',
    grpaPeriods: [],
    locations: [],
    selectedLocationId: 0,
    bsrReportType: 0,
}

const followUpReducer = (state: IFollowUpState = followUpInitState, action: IActionPayload) => {

    switch(action.type) {
        case EFollowUpActions.resetFollowUpSearchParameters:
            return {
                ...state,
                grpaPeriodKey:  followUpInitState.grpaPeriodKey,
                startDate:  followUpInitState.startDate,
                endDate:  followUpInitState.endDate,
                selectedLocationId: followUpInitState.selectedLocationId,
                screeningResultID: followUpInitState.screeningResultID,
                lastName: followUpInitState.lastName,
                firstName: followUpInitState.firstName,
                activeDirection: followUpInitState.activeDirection,
                offset: followUpInitState.offset,
                currentPage: followUpInitState.currentPage,
            }
        case EFollowUpActions.getRelatedReportByIdRequestSuccess:
            return {
                ...state,
                reportObject: {
                    ...state.reportObject,
                    [action.payload.id]: action.payload.report,
                }
            }
        case EFollowUpActions.setFollowUpBSRReportType:
            return {
                ...state,
                bsrReportType:  action.payload.reportType,
            }
        case EFollowUpActions.setFollowUpScreendoxGRPAPeriod:
            return {
                ...state,
                grpaPeriodKey:  action.payload.periodKey,
            }
        case EFollowUpActions.setFollowUpScreendoxStartDate:
            return {
                ...state,
                startDate:  action.payload.startDate,
            }
        case EFollowUpActions.setFollowUpScreendoxEndDate:
            return {
                ...state,
                endDate:  action.payload.endDate,
            }
        case EFollowUpActions.setFollowUpLocationId:
            return {
                ...state,
                selectedLocationId:  action.payload.id,
            }
        case EFollowUpActions.setFollowUpScreendoxResultId:
            return {
                ...state,
                screeningResultID:  action.payload.screendoxResultId,
            }
        case EFollowUpActions.setFollowUpFirstName:
            return {
                ...state,
                firstName:  action.payload.firstName,
            }
        case EFollowUpActions.setFollowUpLastName:
            return {
                ...state,
                lastName:  action.payload.lastName,
            }
        case EFollowUpActions.setCurrentFollowUpOffset:
            return {
                ...state,
                offset:  action.payload.offset,
            }
        case EFollowUpActions.setCurrentFollowUpPage:
            return {
                ...state,
                currentPage: action.payload.page,
            }
        case EFollowUpActions.changeFollowUpActiveKey:
            return {
                ...state,
                activeSort: action.payload.key,
            }
        case EFollowUpActions.changeFollowUpActiveDirection:
            return {
                ...state,
                activeDirection: action.payload.direction,
            }
        case EFollowUpActions.postAllFollowUpRequestError:
            return {
                ...state,
                isFollowUpLoading: false,
            }
        case EFollowUpActions.postAllFollowUpRequestStart:
            return {
                ...state,
                isFollowUpLoading: true,
            }
        case EFollowUpActions.getLocationListRequestSuccess:
            return  {
                ...state,
                locations: action.payload.locations
            }
        case EFollowUpActions.postAllFollowUpRequestSuccess:
            return {
                ...state,
                isFollowUpLoading: false,
                followUpResponseList:  action.payload.response ?  action.payload.response.Items : [],
                totalFollowUps: action.payload.response ? action.payload.response.TotalCount : 0,
                offset: action.payload.response ? action.payload.response.Offset : 0,
                currentPage: action.payload.response ? action.payload.response.CurrentPage : 1,
            }
        case EFollowUpActions.changeAutoUpdateStatus:
            return {
                ...state,
                isAutoUpdate: !state.isAutoUpdate,
            }
        default: return state;
    }
}

export default followUpReducer;
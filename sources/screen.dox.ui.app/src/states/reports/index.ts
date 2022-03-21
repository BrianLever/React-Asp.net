import { IActionPayload, EReportsActions, ISortState, TGPRAPeriodResponseItem, ILocationItemResponse } from '../../actions';
import {  IReportsInnerItem, IScreeningReportResultsBySortItem } from '../../actions/reports';
import { IScreenListInnerItem } from 'actions/screen';

export interface IReportsState {
    currentPage: number;
    itemsToShow: number;
    offset: number;
    isReportsLoading: boolean;
    reportsResponseList: Array<IReportsInnerItem>;
    totalReports: number;
    isAutoUpdate: boolean;
    firstName: string;
    lastName: string;
    screeningResultID: number | null;
    sortObject: { [k: string]: ISortState; } 
    activeSort: string;
    activeDirection: string;
    reportObject: { [k: number]: IScreenListInnerItem[]};
    startDate: string | null,
    endDate: string | null,
    earliestDate:string | null,
    grpaPeriodKey: string;
    grpaPeriods: Array<TGPRAPeriodResponseItem>;
    locations: Array<ILocationItemResponse>;
    selectedLocationId: number;
    bsrReportType: number;
    ageGroupList:Array<any>;
    screeningResultReportsBySort: Array<IScreeningReportResultsBySortItem>;
    isInnerReportsBySortLoading: boolean;
    isInnerReportsBySortLoadingError: boolean;
    reportsBySortFilterArray: Array<{ ScreeningSection: string, MinScoreLevel: number }>;
    IncludeScreenings:boolean;
    IncludeDemographics:boolean;
    IncludeVisits:boolean;
    IncludeFollowUps:boolean;
    IncludeDrugsOfChoice:boolean;
    IncludeCombined:boolean;
    
}

export const MAXIMUM_RECORDS_PER_PAGE = 20;

export const reportsInitState: IReportsState = {
    currentPage: 1,
    itemsToShow: MAXIMUM_RECORDS_PER_PAGE,
    offset: 0,
    isReportsLoading: true,
    reportsResponseList: [],
    totalReports: 0,
    isAutoUpdate: true,
    sortObject: {},
    activeSort: '',
    activeDirection: 'asc',
    reportObject: {},
    firstName: '',
    lastName: '',
    screeningResultID: null,
    startDate: "",
    endDate: null,
    grpaPeriodKey: '',
    grpaPeriods: [],
    locations: [],
    selectedLocationId: 0,
    bsrReportType: 0,
    ageGroupList:[],
    earliestDate:'',
    screeningResultReportsBySort: [],
    reportsBySortFilterArray: [],
    IncludeScreenings:false,
    IncludeDemographics:false,
    IncludeVisits:false,
    IncludeFollowUps:false,
    IncludeDrugsOfChoice:false,
    IncludeCombined:false,
    isInnerReportsBySortLoading: false,
    isInnerReportsBySortLoadingError: false,

}

const reportsReducer = (state: IReportsState = reportsInitState, action: IActionPayload) => {
    switch(action.type) {
        case EReportsActions.resetReportsSearchParameters:
            return {
                ...state,
                grpaPeriodKey:  reportsInitState.grpaPeriodKey,               
                startDate:  reportsInitState.startDate,
                endDate:  reportsInitState.endDate,
                selectedLocationId: reportsInitState.selectedLocationId,
                screeningResultID: reportsInitState.screeningResultID,
                lastName: reportsInitState.lastName,
                firstName: reportsInitState.firstName,
                activeDirection: reportsInitState.activeDirection,
                offset: reportsInitState.offset,
                currentPage: reportsInitState.currentPage,
                bsrReportType:reportsInitState.bsrReportType,
                IncludeScreenings:reportsInitState.IncludeScreenings,
                IncludeDemographics:reportsInitState.IncludeDemographics,
                IncludeVisits:reportsInitState.IncludeVisits,
                IncludeFollowUps:reportsInitState.IncludeFollowUps,
                IncludeDrugsOfChoice:reportsInitState.IncludeDrugsOfChoice,
                IncludeCombined:reportsInitState.IncludeCombined,
            }      
      
         case EReportsActions.setReportsScreendoxGRPAPeriod:
                return {
                    ...state,
                    grpaPeriodKey:  action.payload.periodKey,           
                 }
        case EReportsActions.setReportsScreendoxStartDate:
            return {
                ...state,
                startDate:  action.payload.startDate,
            }
        case EReportsActions.setReportsScreendoxEndDate:
            return {
                ...state,
                endDate:  action.payload.endDate,
            }      
        //Report by Problm
        case EReportsActions.postReportsByProblemRequestError:
            return {
                ...state,
                isReportsLoading: false,
            }
        case EReportsActions.postReportsByProblemRequestStart:
            return {
                ...state,
                isReportsLoading: true,
            }
        case EReportsActions.postReportsByProblemRequestSuccess:
            return {
                ...state,
                isReportsLoading: false,
                reportsResponseList:  action.payload
            }    
            
        // Report by Age
        case EReportsActions.postReportsByAgeRequestError:
            return {
                ...state,
                isReportsLoading: false,
            }
        case EReportsActions.postReportsByAgeRequestStart:
            return {
                ...state,
                isReportsLoading: true,
            }
        case EReportsActions.postReportsByAgeRequestSuccess:
            return {
                ...state,
                isReportsLoading: false,
                reportsResponseList:  action.payload
            }  
        //AgeGroup of Report by Age
        case EReportsActions.reportAgeGroupRequestError:
                return {
                    ...state,
                    isReportsLoading: false,
            }
        case EReportsActions.reportAgeGroupRequestStart:
                return {
                    ...state,
                    isReportsLoading: true,
            }
        case EReportsActions.reportAgeGroupRequestSuccess:
                return {
                    ...state,
                    isReportsLoading: false,
                    ageGroupList:  action.payload
            }  

        //Drug by  Age

        case EReportsActions.postDrugByAgeRequestError:
            return {
                ...state,
                isReportsLoading: false,
            }
        case EReportsActions.postDrugByAgeRequestStart:
            return {
                ...state,
                isReportsLoading: true,
            }
        case EReportsActions.postDrugByAgeRequestSuccess:
            return {
                ...state,
                isReportsLoading: false,
                reportsResponseList:  action.payload
            } 
        
        //Patient Demographics

        case EReportsActions.postPatientDemographicsRequestError:
            return {
                ...state,
                isReportsLoading: false,
            }
        case EReportsActions.postPatientDemographicsRequestStart:
            return {
                ...state,
                isReportsLoading: true,
            }
        case EReportsActions.postPatientDemographicsRequestSuccess:
            return {
                ...state,
                isReportsLoading: false,
                reportsResponseList:  action.payload
            } 
        // Followup Outcomes
        case EReportsActions.postFollowupOutcomesRequestError:
                return {
                    ...state,
                    isReportsLoading: false,
                }
        case EReportsActions.postFollowupOutcomesRequestStart:
                return {
                    ...state,
                    isReportsLoading: true,
                }
        case EReportsActions.postFollowupOutcomesRequestSuccess:
                return {
                    ...state,
                    isReportsLoading: false,
                    reportsResponseList:  action.payload
                } 
         // Visits Outcomes       
        case EReportsActions.postVisitsOutcomesRequestError:
                    return {
                        ...state,
                        isReportsLoading: false,
                    }
            case EReportsActions.postVisitsOutcomesRequestStart:
                    return {
                        ...state,
                        isReportsLoading: true,
                    }
            case EReportsActions.postVisitsOutcomesRequestSuccess:
                    return {
                        ...state,
                        isReportsLoading: false,
                        reportsResponseList:  action.payload
                    } 
          // Screen Time Log      
         case EReportsActions.postScreenTimeLogRequestError:
                    return {
                        ...state,
                        isReportsLoading: false,
                    }
         case EReportsActions.postScreenTimeLogRequestStart:
                    return {
                        ...state,
                        isReportsLoading: true,
                    }
         case EReportsActions.postScreenTimeLogRequestSuccess:
                    return {
                        ...state,
                        isReportsLoading: false,
                        reportsResponseList:  action.payload
                    } 

         //IncludeScreenings
        case EReportsActions.postIncludeScreeningsRequestSuccess:
            return {
                 ...state,
                     isReportsLoading: false,
                    IncludeScreenings:  action.payload
            } 
         //IncludeDemographics
         case EReportsActions.postIncludeDemographicsRequestSuccess:
             return {
                 ...state,
                    isReportsLoading: false,
                    IncludeDemographics:  action.payload
                } 
         //IncludeVisits
         case EReportsActions.postIncludeVisitsRequestSuccess:
             return {
                 ...state,
                    isReportsLoading: false,
                    IncludeVisits:  action.payload
                } 
        //IncludeFollowUps
        case EReportsActions.postIncludeFollowUpsRequestSuccess:
            return {
                ...state,
                   isReportsLoading: false,
                   IncludeFollowUps:  action.payload
               }  
        //IncludeDrugsOfChoice
        case EReportsActions.postIncludeDrugsOfChoiceRequestSuccess:
            return {
                ...state,
                   isReportsLoading: false,
                   IncludeDrugsOfChoice:  action.payload
               }  
        //IncludeCombined
        case EReportsActions.postIncludeCombinedRequestSuccess:
            return {
                ...state,
                   isReportsLoading: false,
                   IncludeCombined:  action.payload
               }   
          // Location List          
        case EReportsActions.getLocationListActionRequestSuccess: 
            return {
                ...state,
                locations: action.payload
            }
        case EReportsActions.setReportsBSRReportType: 
            return {
                ...state,
                bsrReportType: action.payload.reportType
            }
        
        case EReportsActions.setReportsLocationId:
            return {
                ...state,
                selectedLocationId: action.payload.id
            }
        //getting earliest date
        case EReportsActions.reportEarliestDateRequestError:
                return {
                    ...state,
                    isReportsLoading: false,
            }
        case EReportsActions.reportEarliestDateRequestStart:
                return {
                    ...state,
                    isReportsLoading: true,
            }
        case EReportsActions.reportEarliestDateRequestSuccess:
                return {
                    ...state,
                    isReportsLoading: false,
                    earliestDate:  action.payload
            }  
        case EReportsActions.getScreeningResultReportsBySortRequestStart:
            return {
                ...state,
                isReportsLoading: true,
            }
        case EReportsActions.getScreeningResultReportsBySortRequestError:
            return {
                ...state,
                isReportsLoading: false,
            }
        case EReportsActions.getScreeningResultReportsBySortRequestSuccess:
            return {
                ...state,
                isReportsLoading: false,
                screeningResultReportsBySort: action.payload
            }
        case EReportsActions.getInternalReportsListItemDataRequestStart:
            return {
                ...state,
                isInnerReportsBySortLoading: true,
                isInnerReportsBySortLoadingError: false, 
            }
        case EReportsActions.getInternalReportsListItemDataRequestError:
            return {
                ...state,
                isInnerReportsBySortLoading: false,
                isInnerReportsBySortLoadingError: true, 
            }
        case EReportsActions.getInternalReportsListItemDataRequestSuccess:
            return {
                ...state,
                isInnerReportsBySortLoading: false,
                isInnerReportsBySortLoadingError: false, 
                reportObject: {
                    ...state.reportObject,
                    [action.payload.ScreeningResultID]: action.payload.Items,
                },
            }

        //GPRA Periods        
        case EReportsActions.reportGPRAPeriodsRequestSuccess:
                return {
                    ...state,
                    isReportsLoading: false,
                    grpaPeriods:  action.payload
            }  
        case EReportsActions.setScreeningResultReportsBySortFilterArray:
            return {
                ...state, 
                reportsBySortFilterArray: action.payload
            }
        case EReportsActions.setScreeningResultReportsBySortAutoStatus:
            return {
                ...state,
                isAutoUpdate: !state.isAutoUpdate
            }
        default: return state;
    }
}

export default reportsReducer;
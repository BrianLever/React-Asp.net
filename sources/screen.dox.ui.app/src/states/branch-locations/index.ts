import { IActionPayload, EBranchLocationsActions } from '../../actions';
import { IBranchLocationItemResponse } from '../../actions/branch-locations';

export interface IBranchLocationsState {
    isBranchLocationLoading: boolean;
    isNewBranchLocationLoading: boolean;
    branchLocationArray: Array<IBranchLocationItemResponse>;
    totalBranchLocations: number;
    currentPage: number;
    activeSort: string;
    activeDirection: string;
    selectedFilterBranchLocationNameKey: string;
    selectedFilterScreeningProfileId: number;
    selectedFilterBranchLocationId: number;
    selectedFilterShowDisabled: number;
    createBranchLocationName: string;
    createBranchLocationDescription: string;
    createBranchLocationScreenProfile: number;
    selectedBranchLocationId: number;
    isAutoStatus: boolean;
    branchLocationDisabled: boolean;
}

export const branchLocationsInitState: IBranchLocationsState = {
    isBranchLocationLoading: true,
    isNewBranchLocationLoading: false,
    branchLocationArray: [],
    totalBranchLocations: 0,
    currentPage: 1,
    activeSort: '',
    activeDirection: '',
    selectedFilterBranchLocationNameKey: '',
    selectedFilterScreeningProfileId: 0,
    selectedFilterBranchLocationId: 0,
    selectedFilterShowDisabled: 0,
    createBranchLocationName: '',
    createBranchLocationDescription:  '',
    createBranchLocationScreenProfile: 0,
    selectedBranchLocationId: 0,
    isAutoStatus: true,
    branchLocationDisabled: false
}

const branchLocationsReducer = (state: IBranchLocationsState = branchLocationsInitState, action: IActionPayload) => {
    switch(action.type) {
        case EBranchLocationsActions.setCurrentBranchLocation:
            return {
                ...state,
                selectedBranchLocationId: action.payload.id,
            }
        case EBranchLocationsActions.createBranchLocationScreenProfile:
            return {
                ...state,
                createBranchLocationScreenProfile: action.payload.value,
            }
        case EBranchLocationsActions.createBranchLocationDescription:
            return {
                ...state,
                createBranchLocationDescription: action.payload.value,
            }
        case EBranchLocationsActions.createBranchLocationName:
            return {
                ...state,
                createBranchLocationName: action.payload.value,
            }
        case EBranchLocationsActions.resetBranchLocationsFilter:
            return {
                ...state,
                selectedFilterBranchLocationId: branchLocationsInitState.selectedFilterBranchLocationId,
                selectedFilterBranchLocationNameKey: branchLocationsInitState.selectedFilterBranchLocationNameKey,
                selectedFilterShowDisabled: branchLocationsInitState.selectedFilterShowDisabled,
                selectedFilterScreeningProfileId:branchLocationsInitState.selectedFilterScreeningProfileId 
            }
        case EBranchLocationsActions.setFilterShowDisabled:
            return {
                ...state,
                selectedFilterShowDisabled: action.payload.value,
            }
        case EBranchLocationsActions.setFilterBranchLocationId:
            return {
                ...state,
                selectedFilterBranchLocationId: action.payload.id,
            }
        case EBranchLocationsActions.setScreeningProfileId:
            return {
                ...state,
                selectedFilterScreeningProfileId: action.payload.id,
            }
        case EBranchLocationsActions.setFilterLocationNameKey:
            return {
                ...state,
                selectedFilterBranchLocationNameKey: action.payload.name,
            }
        case EBranchLocationsActions.setCurrentPage:
            return {
                ...state,
                currentPage: action.payload.page,
            }
        case EBranchLocationsActions.changeActiveKey:
            return {
                ...state,
                activeSort: action.payload.key,
            }
        case EBranchLocationsActions.changeActiveDirection:
            return {
                ...state,
                activeDirection: action.payload.direction,
            }
        case EBranchLocationsActions.getBranchLocationsListRequestStart:
            return {
                ...state,
                isBranchLocationLoading: true,
            }
        case EBranchLocationsActions.getBranchLocationsListRequestError:
            return {
                ...state,
                isBranchLocationLoading: false,
            }
        case EBranchLocationsActions.getBranchLocationsListRequestSuccess:
            return {
                ...state,
                isBranchLocationLoading: false,
                branchLocationArray: action.payload.Items,
                totalBranchLocations: action.payload.TotalCount,
            }
        case EBranchLocationsActions.setNewBranchLocationLoading:
            return {
                ...state,
                isNewBranchLocationLoading: action.payload.value
            }
        case EBranchLocationsActions.setBranchLocationDisabled:
            return {
                ...state,
                branchLocationDisabled: action.payload.value
            }
        case EBranchLocationsActions.setBranchLocationListAutoStatus: 
            return {
                ...state,
                isAutoStatus: !state.isAutoStatus
            }
        default: return state;
    }
}

export default branchLocationsReducer;
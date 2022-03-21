import { IActionPayload, ESharedActions } from '../../actions';
import { TBranchLocationsItemResponse, TScreeningProfileItemResponse } from '../../actions/shared';

export interface ISharedState {
    branchLocationsArray: Array<TBranchLocationsItemResponse>;
    screeningProfileList: Array<TScreeningProfileItemResponse>;
    isCreateDateCustom: boolean;
}

export const profileInitState: ISharedState = {
    branchLocationsArray: [],
    screeningProfileList: [],
    isCreateDateCustom: false,
}

const sharedReducer = (state: ISharedState = profileInitState, action: IActionPayload) => {
    switch(action.type) {
        case ESharedActions.getScreeningProfileListRequestSuccess:
            return {
                ...state,
                screeningProfileList: action.payload.list,
            }
        case ESharedActions.getLocationsRequestSuccess:
            return {
                ...state,
                branchLocationsArray: action.payload.list,
            }
        case ESharedActions.setCreateDateCustomOrGPRA:
            return {
                ...state,
                isCreateDateCustom: !state.isCreateDateCustom
            }
        default: return state;
    }
}

export default sharedReducer;
import { 
    IActionPayload, EAgeGroupsActions
} from '../../actions';

import { ageGroupValueItem } from '../../actions/age-groups'; 

export interface IAgeGroupsState {
    Value: ageGroupValueItem;
    DefaultValue: string;
    Labels: Array<string>;
    isAgeGroupLoading: boolean;
}

export const ageGroupsInitState: IAgeGroupsState = {
    DefaultValue: '',
    Labels: [],
    Value: {
        Key: '',
        Value: '',
        Name: '',
        Description: '',
        RegExp: '',
        IsExposed: true,
    },
    isAgeGroupLoading: false,
}

const ageGroupsReducer = (state: IAgeGroupsState = ageGroupsInitState, action: IActionPayload) => {
    switch(action.type) {
        case EAgeGroupsActions.getAgeGroupsRequestStart:
            return {
                ...state,
                isAgeGroupLoading: true
            }
        case EAgeGroupsActions.getAgeGroupsRequestSuccess:
            return {
                ...state,
                isAgeGroupLoading: false,
                DefaultValue: action.payload.DefaultValue,
                Labels: action.payload.Labels,
                Value: action.payload.Value,
            }
        case EAgeGroupsActions.getAgeGroupsRequestError:
            return {
                ...state,
                isAgeGroupLoading: false
            }
        case EAgeGroupsActions.setAgeGroupsValue:
            return {
                ...state,
                Value: {
                    ...state.Value,
                    Value: action.payload.value
                }
            }
        case EAgeGroupsActions.updateAgeGroupRequestStart:
            return {
                ...state,
                isAgeGroupLoading: true
            }
        case EAgeGroupsActions.updateAgeGroupRequestError:
            return {
                ...state,
                isAgeGroupLoading: false
            }
        case EAgeGroupsActions.updateAgeGroupRequestSuccess:
            return {
                ...state,
                isAgeGroupLoading: false,
                DefaultValue: action.payload.DefaultValue,
                Labels: action.payload.Labels,
                Value: action.payload.Value,
            }
        default: return state;
    }
}

export default ageGroupsReducer;
import { IActionPayload, EUserProfileActionType } from '../../actions';
import { IProfileResponse } from '../../actions/profile';

export interface IProfileState {
    profile: IProfileResponse | null;
    isProfileLoading: boolean;
    isProfileError: boolean;
}

export const profileInitState: IProfileState = {
    profile: null,
    isProfileLoading: false,
    isProfileError: false,
}

const profileReducer = (state: IProfileState = profileInitState, action: IActionPayload) => {
    switch(action.type) {
        case EUserProfileActionType.getProfileRequestStart:
            return {
                ...state,
                isProfileLoading: true,
                isProfileError: false,
            }
        case EUserProfileActionType.getProfileRequestSuccess:
            return {
                ...state,
                isProfileLoading: false,
                isProfileError: false,
                profile: action.payload.profile,
            }
        case EUserProfileActionType.getProfileRequestError:
            return {
                ...state,
                isProfileLoading: false,
                isProfileError: true,
            }
        case EUserProfileActionType.setProfile: 
            return {
                ...state,
                profile: action.payload
            }
        default: return state;
    }
}

export default profileReducer;
import { IActionPayload, ELoginActions, EResetPasswordActions } from '../../actions';
import { IResetPasswordGetSecurityQuestionResponse } from '../../actions/reset-password';

export interface IResetPasswordState {
   isSecurityQuestionLoading: boolean;
   securityQuestion: string | null;
   isSecurityQuestionLoadingError: boolean;
   isResetPasswordLoading: boolean;
   resetPasswordErrors: Array<string>;
   isResetPasswordLoadingError: boolean;
   isResetPasswordSuccess: boolean;
}

export const resetPasswordInitState: IResetPasswordState = {
    isSecurityQuestionLoading: false,
    securityQuestion: null,
    isSecurityQuestionLoadingError: false,
    isResetPasswordLoading: false,
    resetPasswordErrors: [],
    isResetPasswordLoadingError: false,
    isResetPasswordSuccess: false
}

const resetPasswordReducer = (state: IResetPasswordState = resetPasswordInitState, action: IActionPayload) => {
    switch(action.type) {
        case EResetPasswordActions.resetPasswordGetSecurityQuestionRequestStart:
            return {
                ...state,
                isSecurityQuestionLoading: true,
                isSecurityQuestionLoadingError: false,
            }
        case EResetPasswordActions.resetPasswordGetSecurityQuestionRequestError:
            return {
                ...state,
                isSecurityQuestionLoading: false,
                isSecurityQuestionLoadingError: true,
            }
        case EResetPasswordActions.resetPasswordGetSecurityQuestionRequestSuccess:
            return {
                ...state,
                isSecurityQuestionLoading: false,
                securityQuestion: action.payload.SecurityQuestion,
                isSecurityQuestionLoadingError: false,
            }
        case EResetPasswordActions.resetPasswordRequestStart:
            return {
                ...state,
                isResetPasswordLoading: true,
                isResetPasswordLoadingError: false,
                resetPasswordErrors: [],
                isResetPasswordSuccess: false
            }
        case EResetPasswordActions.resetPasswordRequestError:
            return {
                ...state,
                isResetPasswordLoading: false,
                isResetPasswordLoadingError: true,
                resetPasswordErrors: action.payload,
                isResetPasswordSuccess: false,
            }
        case EResetPasswordActions.resetPasswordRequestSuccess:
            return {
                ...state,
                isResetPasswordLoading: false,
                isResetPasswordLoadingError: false,
                resetPasswordErrors: [],
                isResetPasswordSuccess: true,
            }
        default: return state;
    }
}

export default resetPasswordReducer;
import {  EChangeSecurityQuestionActions, IActionPayload } from '../../actions';

export interface IChangeSecurityQuestionState {
   isLoading: boolean;
   securityQuestionList: Array<string>;
}

export const changeSecurityQuestionInitState: IChangeSecurityQuestionState = {
    securityQuestionList: [],
    isLoading : false,
}

const changeSecurityQuestionReducer = (state: IChangeSecurityQuestionState = changeSecurityQuestionInitState, action: IActionPayload) => {
    switch(action.type) {
        case EChangeSecurityQuestionActions.securityQuestionListRequestSuccess:
            return {
                ...state,
                securityQuestionList: action.payload
            }
        case EChangeSecurityQuestionActions.changeSecurityQuestionUpdateRequestError:
            return {
                ...state,
                isLoading: false,
            }
        case EChangeSecurityQuestionActions.changeSecurityQuestionUpdateRequestStart:
            return {
                ...state,
                isLoading: true,
            }
        case EChangeSecurityQuestionActions.changeSecurityQuestionUpdateRequestSuccess:
            return {
                ...state,
                isLoading: false,
            }
        default: return state;
    }
}

export default changeSecurityQuestionReducer;
import { Action } from "redux";
import { EChangeSecurityQuestionActions, IActionPayload } from "..";


export interface IChangeSecurityQuestionRequest {
    Password: string;
    SecurityQuestion: string;
    SecurityQuestionAnswer: string;
}

export const getSecurityQuestionListRequest = (): Action => ({
    type: EChangeSecurityQuestionActions.securityQuestionListRequest
})

export const getSecurityQuestionListRequestSuccess =  (payload: Array<string>):IActionPayload => ({
    type: EChangeSecurityQuestionActions.securityQuestionListRequestSuccess, payload
})

export const changeSecurityQuestionUpdateRequest = (payload: IChangeSecurityQuestionRequest) :IActionPayload => ({
    type:EChangeSecurityQuestionActions.changeSecurityQuestionUpdateRequest, payload
})

export const changeSecurityQuestionUpdateRequestStart = ():Action => ({
    type: EChangeSecurityQuestionActions.changeSecurityQuestionUpdateRequestStart
})

export const changeSecurityQuestionUpdateRequestError = (): Action => ({
    type: EChangeSecurityQuestionActions.changeSecurityQuestionUpdateRequestError
})

export const changeSecurityQuestionUpdateRequestSuccess = (): Action => ({
    type: EChangeSecurityQuestionActions.changeSecurityQuestionUpdateRequestSuccess
})
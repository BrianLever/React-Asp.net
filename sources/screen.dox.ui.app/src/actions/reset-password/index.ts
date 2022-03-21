import { Action } from 'redux';
import {  EResetPasswordActions, IActionPayload } from '..';


export interface IResetPasswordGetSecurityQuestionResponse {
    SecurityQuestion: string;
}

export const resetPasswordGetSecurityQuestionRequest = (username: string):IActionPayload => ({
    type: EResetPasswordActions.resetPasswordGetSecurityQuestionRequest, payload: { username }
})

export const resetPasswordGetSecurityQuestionRequestStart = ():Action => ({
    type: EResetPasswordActions.resetPasswordGetSecurityQuestionRequestStart
})

export const resetPasswordGetSecurityQuestionRequestError = ():Action => ({
    type: EResetPasswordActions.resetPasswordGetSecurityQuestionRequestError
})

export const resetPasswordGetSecurityQuestionRequestSuccess = (payload: IResetPasswordGetSecurityQuestionResponse):IActionPayload => ({
    type: EResetPasswordActions.resetPasswordGetSecurityQuestionRequestSuccess, payload
})

export const resetPasswordRequest = (payload: { SecurityQuestionAnswer: string, NewPassword: string, username: string }): IActionPayload => ({
    type: EResetPasswordActions.resetPasswordRequest, payload
})

export const resetPasswordRequestStart = ():Action => ({
    type: EResetPasswordActions.resetPasswordRequestStart
})

export const resetPasswordRequestError = (payload: Array<string>):IActionPayload => ({
    type: EResetPasswordActions.resetPasswordRequestError, payload
})

export const resetPasswordRequestSuccess = ():Action => ({
    type: EResetPasswordActions.resetPasswordRequestSuccess
})
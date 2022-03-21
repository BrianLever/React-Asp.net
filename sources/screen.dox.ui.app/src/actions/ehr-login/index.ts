import { EEhrLoginActions, IActionPayload } from "actions";
import { Action } from "redux";

export interface IEhrLoginResponseItem {
    "ExpireAtFormatted": string;
    "IsActive": boolean;
    "Id": string;
    "AccessCode": string;
    "VerifyCode": string;
    "ExpireAt": string;
}

export interface IEhrLoginRequest {
    "AccessCode": string | null;
    "VerifyCode": string | null;
    "ExpireAt": string;
}

export const getEhrLoginListRequest = ():Action => ({
    type: EEhrLoginActions.getEhrLoginListRequest
})

export const getEhrLoginListRequestStart = ():Action => ({
    type: EEhrLoginActions.getEhrLoginListRequestStart
})

export const getEhrLoginListRequestError = ():Action => ({
    type: EEhrLoginActions.getEhrLoginListRequestError
})

export const getEhrLoginListRequestSuccess = (payload: IEhrLoginResponseItem[]):IActionPayload => ({
    type: EEhrLoginActions.getEhrLoginListRequestSuccess, payload
})

export const setEhrLoginAccessCode = (accessCode: string | null): IActionPayload => ({
    type: EEhrLoginActions.setEhrLoginAccessCode, payload: { accessCode }
})

export const setEhrLoginVerifyCode = (verifyCode: string | null): IActionPayload => ({
    type: EEhrLoginActions.setEhrLoginVerifyCode, payload: { verifyCode }
})

export const setEhrLoginExpireOn = (expireOn: string): IActionPayload => ({
    type: EEhrLoginActions.setEhrLoginExpireOn, payload: { expireOn }
})

export const setEhrLoginSelectedId = (selectedId: string | null): IActionPayload => ({
    type: EEhrLoginActions.setEhrLoginSelectedId, payload: { selectedId }
})

export const ehrLoginCreateRequest = (): Action => ({
    type: EEhrLoginActions.ehrLoginCreateRequest
})

export const ehrLoginCreateRequestError = (): Action => ({
    type: EEhrLoginActions.ehrLoginCreateRequestError
})

export const ehrLoginCreateRequestStart = (): Action => ({
    type: EEhrLoginActions.ehrLoginCreateRequestStart
})

export const ehrLoginCreateRequestSuccess = (): Action => ({
    type: EEhrLoginActions.ehrLoginCreateRequestSuccess
})
export const ehrLoginDeleteRequest = (id: string): IActionPayload => ({
    type: EEhrLoginActions.ehrLoginDeleteRequest, payload: { id }
})
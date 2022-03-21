import { Action } from 'redux';
import { ELoginActions, IActionPayload } from '..';

export interface IloginResponse {
    AccessToken: {
        Token: string;
        ExpiresIn: number;
    };
    RefreshToken: string;
}

export interface IloginRequest {
    Username: string | null;
    Password: string | null;
}


export const loginRequest = (payload: IloginRequest): IActionPayload => ({ type: ELoginActions.loginRequest, payload });
export const loginRequestStart = (): Action => ({ type: ELoginActions.loginRequestStart });
export const loginRequestError = (): Action => ({ type: ELoginActions.loginRequestError });
export const loginRequestSuccess = (payload: any) :IActionPayload => ({ type: ELoginActions.loginRequestSuccess, payload });
export const logoutRequest = ():Action => ({ type: ELoginActions.logoutRequest })
export const setLoginErrorList = (payload: Array<string>):IActionPayload => ({ 
    type: ELoginActions.setLoginErrorList, payload
})
export const refreshTokenRequest = (): Action => ({
    type: ELoginActions.refreshTokenRequest
})
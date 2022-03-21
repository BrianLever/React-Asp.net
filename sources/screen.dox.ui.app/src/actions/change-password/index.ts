import { Action } from 'redux';
import { IActionPayload, EChangePasswordActions } from '..';

export const  changePasswordRequest = (payload: { CurrentPassword: string, NewPassword: string }) :IActionPayload => ({
    type: EChangePasswordActions.changePasswordRequest, payload
})
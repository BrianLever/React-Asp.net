import { Action } from 'redux';
import { EUserProfileActionType, IActionPayload } from '..';

export interface IProfileResponse {
    UserID?: number;
    FirstName?: string;
    LastName?: string;
    MiddleName?: string;
    ContactPhone?: string;
    StateCode?: string;
    City?: string;
    AddressLine1?: string;
    AddressLine2?: string;
    PostalCode?: string;
    RoleName?: string;
    BranchLocationID?: string;
    IsBlock?: boolean;
    Comments?: string;
    Email?: string;
    UserName?: string;
    LastPasswordChangedDate?: string;
    FullName?: string;
    IsMustChangePassword?: boolean;
    IsMustSetupSecurityQuestion?: boolean;
}


export interface IProifleUpdateRequest {
    FirstName?: string;
    LastName?: string;
    MiddleName?: string;
    ContactPhone?: string;
    StateCode?: string;
    City?: string;
    AddressLine1?: string;
    AddressLine2?: string;
    PostalCode?: string;
    RoleName?: string;
    BranchLocationID: string;
    IsBlock?: boolean;
    Comments?: string;
    Email?: string;
    UserName?: string;
}

export const getProfileRequest = (clientCode: number): IActionPayload => ({
    type: EUserProfileActionType.getProfileRequest,
    payload: { clientCode }
});

export const getProfileRequestStart = (): Action => ({
    type: EUserProfileActionType.getProfileRequestStart,
});

export const getProfileRequestError = (): Action => ({
    type: EUserProfileActionType.getProfileRequestError,
});

export const getProfileRequestSuccess = (payload: { profile: IProfileResponse }): IActionPayload => {
    return ({
        type: EUserProfileActionType.getProfileRequestSuccess, payload 
    })
} 

export const setProfile = (payload: IProfileResponse): IActionPayload => {
    return ({
        type: EUserProfileActionType.setProfile, payload
    })
}


export const updateProfileRequest = (): Action => ({
    type: EUserProfileActionType.updateProfileRequest
})

export const updateProfileRequestStart = (): Action => ({
    type: EUserProfileActionType.updateProfileRequestStart
})

export const updateProfileRequestError = (): Action => ({
    type: EUserProfileActionType.updateProfileRequestError
})

export const updateProfileRequestSuccess = (): Action => ({
    type: EUserProfileActionType.updateProfileRequestSuccess
})
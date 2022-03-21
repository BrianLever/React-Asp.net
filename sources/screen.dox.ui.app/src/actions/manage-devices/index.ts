import { Action } from 'redux';
import { EManageDevicesActions, IActionPayload } from '..';

export const getManageDevicesListRequest = (): Action => ({
    type: EManageDevicesActions.getManagedDevicesListRequest,
});

export const getManageDevicesListRequestStart = (): Action => ({
    type: EManageDevicesActions.getManagedDevicesListRequestStart,
});

export const getManageDevicesListRequestError = (): Action => ({
    type: EManageDevicesActions.getManagedDevicesListRequestError,
});

export interface IManegeDevicesListRequest {
    OrderBy?: string;
    NameOrKey?: string | null;
    MaximumRows?: number;
    ShowDisabled?: boolean;
    StartRowIndex?: number;
    BranchLocationId?: number | null;
    ScreeningProfileId?: number | null;
}

export interface IManegeDevicesListResponse {
    KioskID?: number;
    Name?: string;
    Description?: string;
    CreatedDate?: string;
    LastActivityDate?: string;
    BranchLocationID?: number;
    Disabled?: boolean;
    IpAddress?: string;
    KioskAppVersion?: string;
    SecretKey?: string;
    ScreeningProfileName?: string;
    KioskKey?: string;
}

export const getManageDevicesListRequestSuccess = (
    payload: { items: Array<IManegeDevicesListResponse>, totalCount: number }
): IActionPayload => {
    return ({
        type: EManageDevicesActions.getManagedDevicesListRequestSuccess, payload 
    })
} 

export const changeActiveSortObjectAction = (key: string, direction: string): IActionPayload => {
    return ({
        type: EManageDevicesActions.changeActiveSort, payload: { key, direction } 
    })
}

export const changeActiveSortDirectionAction = (direction: string): IActionPayload => {
    return ({
        type: EManageDevicesActions.changeActiveDirection, payload: { direction } 
    })
}

export const changeActiveSortKeyAction = (key: string): IActionPayload => {
    return ({
        type: EManageDevicesActions.changeActiveKey, payload: { key } 
    })
}


export const selectAddNewKioskBranchLocation = (id: number): IActionPayload  => {
    return ({
        type: EManageDevicesActions.selectAddNewKioskBranchLocation, payload: { id } 
    })
}

export const selectAddNewKioskDeviceName = (name: string): IActionPayload  => {
    return ({
        type: EManageDevicesActions.selectAddNewKioskDeviceName, payload: { name } 
    })
}

export const selectAddNewKioskScreenProfile = (profile: string): IActionPayload  => {
    return ({
        type: EManageDevicesActions.selectAddNewKioskScreenProfile, payload: { profile } 
    })
}

export const selectAddNewKioskSecretKey = (key: string): IActionPayload  => {
    return ({
        type: EManageDevicesActions.selectAddNewKioskSecretKey, payload: { key } 
    })
}

export const selectAddNewKioskDescription = (description: string): IActionPayload  => {
    return ({
        type: EManageDevicesActions.selectAddNewKioskDescription, payload: { description } 
    })
}

export const addNewKioskInconsistencyInFields = (flag: boolean): IActionPayload  => {
    return ({
        type: EManageDevicesActions.addNewKioskInconsistencyInFields, payload: { flag } 
    })
}

export interface IAddNewKioskRequest {
    Name: string;
    Disabled?: boolean;
    SecretKey: string;
    Description?: string;
    BranchLocationId: number;
}

export interface IAddNewKioskResponse {
    KioskID: number;
    Name: string;
    Description: string;
    CreatedDate: string;
    LastActivityDate: string;
    BranchLocationID: number;
    Disabled: boolean;
    IpAddress: string;
    KioskAppVersion: string;
    SecretKey: string;
    ScreeningProfileName: string;
    KioskKey: string;
}

export const createNewKioskRequest = (): Action => ({ type: EManageDevicesActions.createNewKioskRequest });
export const createNewKioskRequestStart = (): Action => ({ type: EManageDevicesActions.createNewKioskRequestStart });
export const createNewKioskRequestError = (): Action => ({ type: EManageDevicesActions.createNewKioskRequestError });
export const createNewKioskRequestSuccess = (): Action => ({ type: EManageDevicesActions.createNewKioskRequestSuccess });

export const setCurrentPage = (page: number): IActionPayload  => {
    return ({
        type: EManageDevicesActions.setCurrentPage, payload: { page } 
    })
}  
export const changeCurrentPageRequest = (page: number): IActionPayload  => {
    return ({
        type: EManageDevicesActions.changeCurrentPageRequst, payload: { page } 
    })
}

export const setFilterBranchLocationId = (id: number): IActionPayload  => {
    return ({
        type: EManageDevicesActions.setFilterBranchLocationId, payload: { id } 
    })
}

export const setLocationNameKey = (name: string): IActionPayload  => {
    return ({
        type: EManageDevicesActions.setFilterLocationNameKey, payload: { name } 
    })
}

export const setShowDisabled = (value: number): IActionPayload  => {
    return ({
        type: EManageDevicesActions.setFilterShowDisabled, payload: { value } 
    })
}

export const resetKioskFilterRequest = (): Action => ({ type: EManageDevicesActions.resetKioskFilterRequest });
export const resetKioskFilter = (): Action => ({ type: EManageDevicesActions.resetKioskFilter });


export const setScreeningProfileId = (id: number): IActionPayload  => {
    return ({
        type: EManageDevicesActions.setScreeningProfileId, payload: { id } 
    })
}

export interface IKioskDetailsResponse {
    KioskID: number;
    Name: string;
    Description: string;
    CreatedDate: string;
    LastActivityDate: string;
    BranchLocationID: number;
    Disabled: boolean;
    IpAddress: string;
    KioskAppVersion: string;
    SecretKey: string;
    ScreeningProfileName: string;
    KioskKey: string;
}

export const getEditKioskDetailsByIdRequest = (id: number): IActionPayload  => ({
    type: EManageDevicesActions.getEditKioskDetailsByIdRequest, payload: { id } 
});

export const getEditKioskDetailsByIdRequestStart = (): Action => ({ type: EManageDevicesActions.getEditKioskDetailsByIdRequestStart });
export const getEditKioskDetailsByIdRequestError = (): Action => ({ type: EManageDevicesActions.getEditKioskDetailsByIdRequestError });

export const getEditKioskDetailsByIdRequestSuccess = (kiosk: IKioskDetailsResponse, id: number): IActionPayload  => ({
    type: EManageDevicesActions.getEditKioskDetailsByIdRequestSuccess, payload: { kiosk, id } 
});

export const changeEditKioskDetailsDescription = (description: string)
: IActionPayload  => ({ type: EManageDevicesActions.changeEditKioskParamaterDescription, payload: { description } });

export const changeEditKioskDetailsBranchLocation = (location: number)
: IActionPayload  => ({ type: EManageDevicesActions.changeEditKioskParamaterBranchLocation, payload: { location } });

export interface IUpdateKioskRequestParamaters {
    Name: string;
    Description?: string;
    Disabled?: boolean;
    BranchLocationId?: number;
    SecretKey: string;
}

export const updateKioskByIdRequest = (disable: boolean = false): IActionPayload  => 
({ type: EManageDevicesActions.updateKioskByIdRequest, payload: { disable } });
export const updateKioskByIdRequestStart = (): Action => ({ type: EManageDevicesActions.updateKioskByIdRequestStart });
export const updateKioskByIdRequestError = (): Action => ({ type: EManageDevicesActions.updateKioskByIdRequestError });
export const updateKioskByIdRequestSuccess = (): Action => ({ type: EManageDevicesActions.updateKioskByIdRequestSuccess });

export const deleteKioskByIdRequest = (): Action  => ({ type: EManageDevicesActions.deleteKioskByIdRequest });
export const deleteKioskByIdRequestStart = (): Action => ({ type: EManageDevicesActions.deleteKioskByIdRequestStart });
export const deleteKioskByIdRequestError = (): Action => ({ type: EManageDevicesActions.deleteKioskByIdRequestError });
export const deleteKioskByIdRequestSuccess = (): Action => ({ type: EManageDevicesActions.deleteKioskByIdRequestSuccess });

export const disbaleKioskByIdRequest = (): Action => ({ type: EManageDevicesActions.disableKioskByIdRequest });

export const setManageDeviceAutoStatus = (): Action => ({ type: EManageDevicesActions.setManageDeviceAutoStatus });
export const manageDeviceAutoRequest = (): Action => ({ type: EManageDevicesActions.manageDeviceAutoRequest });



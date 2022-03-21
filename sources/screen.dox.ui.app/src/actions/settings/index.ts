import { EApplicationSettingsActions, IActionPayload } from '../';
import { Action } from "redux";

export enum EModalWindowKeys {
    manageDevicesAddNewDevice = 'MANAGE_DEVICES_ADD_NEW_DEVICE',
    manageDevicesEditKioskDetails = 'MANAGE_DEVICES_EDIT_KIOSK_DETAILS',
    branchLocationsAddNewBranchLocation = 'BRANCH_LOCATIONS_ADD_NEW_BRANCH_LOCATION',
    branchLocationsEditBranchLocation = 'BRANCH_LOCATIONS_EDIT_BRANCH_LOCATION',
    screenProfileAdd = 'SCREEN_PROFILE_ADD',
    screenProfileEdit = 'SCREEN_PROFILE_EDIT',
    errorLogDetail = 'ERROR_LOG_DETAIL',

    licenseKeysDelete = 'LICENSE_KEYS_DELETE',
    licenseKeysActivate = 'LICENSE_KEYS_ACTIVATE',
    licenseKeysCreate = 'LICENSE_KEYS_CREATE',

    screenListSelectEHRRecord = 'SCREEN_LIST_SELECT_EHR_RECORD',

    screendoxEhrExportInformation = 'SCREENDOX_EHR_EXPORT_INFORMATION',

    screendoxAbout = 'SCREENDOX_ABOUT',

    manageUsersAddUser = 'MANAGE_USERS_ADD_USER',
    manageUsersEditUser = 'MANAGE_USERS_EDIT_USER',

    ehrLoginCreate = 'EHR_LOGIN_CREATE',
    ehrLoginDelete = 'EHR_LOGIN_DELETE',
}

export const switchOffLoading = (): Action => ({ type: EApplicationSettingsActions.switchLoadingOff });
export const switchOnLoading = (): Action => ({ type: EApplicationSettingsActions.switchLoadingOn });

export const sideDrawerIn = (): Action => ({ type: EApplicationSettingsActions.sideDrawerIn });
export const sideDrawerOut = (): Action => ({ type: EApplicationSettingsActions.sideDrawerOut });
export const triggerSideDrawerState = (value: boolean): IActionPayload => ({ 
    type: EApplicationSettingsActions.triggerSideDrawerState,
    payload: { value }
});

export const notifyError = (message: string): IActionPayload => ({ 
    type: EApplicationSettingsActions.notifyError,
    payload: { message } 
});
export const notifySuccess = (message: string): IActionPayload => ({ 
    type: EApplicationSettingsActions.notifySuccess, 
    payload: { message }  
});
export const resetNotify = (): Action => ({ type: EApplicationSettingsActions.resetNotify });

export const setCurrentPage = (key: string, path: string): IActionPayload => ({ 
    type: EApplicationSettingsActions.setCurrentPage, payload: { key, path }  
});

export const openModalWindow = (key: EModalWindowKeys): IActionPayload => ({ 
    type: EApplicationSettingsActions.modalWindowOpen, payload: { key }  
});

export const closeModalWindow = (key: EModalWindowKeys): IActionPayload => ({ 
    type: EApplicationSettingsActions.modalWindowClose, payload: { key }  
});
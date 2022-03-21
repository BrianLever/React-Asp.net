import { Action } from 'redux';
import { IActionPayload, ELicenseKeysActions } from '..';


export interface ILicenseKeysResponseItem {
    LicenseString: string;
    RegisteredDate: string;
    RegisteredDateLabel: string;
    MaxBranchLocations: number;
    MaxKiosks: number;
    DurationInYears: number;
    IsLicenseExpired: boolean,
    ActivatedDate: string;
    ActivatedDateLabel: string;
    ExpirationDate: string;
    ExpirationDateLabel: string;
}


export const getLicenseKeysRequest = (): Action => ({ type: ELicenseKeysActions.getLicenseKeysRequest });
export const getLicenseKeysRequestStart = (): Action => ({ type: ELicenseKeysActions.getLicenseKeysRequestStart });
export const getLicenseKeysRequestSuccess = (payload: Array<ILicenseKeysResponseItem>): IActionPayload => ({ type: ELicenseKeysActions.getLicenseKeysRequestSuccess, payload });
export const getLicenseKeysRequestError = (): Action => ({ type: ELicenseKeysActions.getLicenseKeysRequestError });
export const setLicenseKey = (value: string): IActionPayload => ({
    type: ELicenseKeysActions.setLicenseKey, payload: { value }
})
export const createLicenseKeyRequest = (): Action => ({ type: ELicenseKeysActions.createLicenseKeyRequest });
export const setLicenseKeyCreateLoading = (value: boolean): IActionPayload => ({
    type: ELicenseKeysActions.setLicenseKeyCreateLoading, payload: { value }
})
export const setLicenseActivationKey = (value: string): IActionPayload => ({
    type: ELicenseKeysActions.setLicenseActivationKey, payload: { value }
})
export const getLicenseKeyDetailRequest = (licenseKey: string): IActionPayload => ({
    type: ELicenseKeysActions.getLicenseKeyDetailRequest, payload: { licenseKey }
})

export const activeLicenseKeyRequest = (activationKey: string) : IActionPayload => ({
    type: ELicenseKeysActions.activeLicenseKeyRequest, payload: { activationKey }
})

export const deleteLicenseKeyRequest = (): Action => ({
    type: ELicenseKeysActions.deleteLicenseKeyRequest
})

export const setLicenseKeysSystemSettingsSummary = (payload: any | null):IActionPayload => ({
    type: ELicenseKeysActions.setLicenseKeysSystemSettingsSummary, payload
})